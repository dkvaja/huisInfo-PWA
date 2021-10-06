using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.AppCore.Repositories;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Portal.JPDS.Domain.Common;
using Portal.JPDS.AppCore.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Portal.JPDS.Infrastructure.Persistence.Repositories
{
    public class LoginRepository : BaseRepository, ILoginRepository
    {

        public LoginRepository(AppDbContext dbContext, AppDbCentralContext dbContextCentral) : base(dbContext, dbContextCentral)
        {

        }

        public AppDbContext AppDbContext
        {
            get { return _dbContext as AppDbContext; }
        }

        public AppDbCentralContext AppDbCentralContext
        {
            get { return _dbContextCentral as AppDbCentralContext; }
        }

        public UserApiModel GetLoginByEmailAndPasswordAndUpdateLastLoginTime(string email, string password)
        {
            var loginData = _dbContext.ViewLogins.Where(x => x.Email.ToLower() == email.ToLower() && x.Wachtwoord == password).FirstOrDefault();
            var login = loginData != null ? _dbContext.Login.Where(x => x.Guid == loginData.Guid && !x.Verwijderd).SingleOrDefault() : null;
            var centralLogin = loginData != null ? _dbContextCentral.CentralLogins.Where(x => x.Guid == loginData.CentralGuid).SingleOrDefault() : null;
            if (login == null || centralLogin == null)
                return null;

            //Update login time...
            login.LaatsteLogin = DateTime.Now;
            centralLogin.LastLoginOn = DateTime.Now;
            return GetUserApiModelFromLogin(login, centralLogin);
        }

        public UserApiModel GetLoginById(string guid)
        {
            var loginData = _dbContext.ViewLogins.Where(x => x.Guid == guid).FirstOrDefault();
            var login = loginData != null ? _dbContext.Login.Where(x => x.Guid == loginData.Guid && !x.Verwijderd).FirstOrDefault() : null;
            return GetUserApiModelFromLogin(login);
        }

        public UserApiModel GetActiveAccountByEmailAndUpdateResetPasswordLinkCreatedDateTime(string email)
        {
            var loginData = _dbContext.ViewLogins.Where(x => x.Email.ToLower() == email.ToLower()).FirstOrDefault();
            var login = loginData != null ? _dbContext.Login.Where(x => x.Guid == loginData.Guid && !x.Verwijderd).SingleOrDefault() : null;
            var centralLogin = loginData != null ? _dbContextCentral.CentralLogins.Where(x => x.Guid == loginData.CentralGuid).SingleOrDefault() : null;
            if (login == null || centralLogin == null)
                return null;

            else
            {
                if (centralLogin.Active && ((login.Actief != null && Convert.ToBoolean(login.Actief)) || login.Actief == null))
                {
                    centralLogin.ResetPasswordLinkCreatedOn = DateTime.Now.TruncateMilliSeconds();
                    return GetUserApiModelFromLogin(login, centralLogin);
                }
                else
                {
                    return null;
                }
            }
        }

        public string GetUserNameById(string userId)
        {
            return _dbContext.ViewLogins.Where(x => x.Guid == userId.ToUpper()).Select(x => x.Email).SingleOrDefault();
        }

        public bool IsOldPassword(string userId, string newPassword)
        {
            return _dbContext.ViewLogins.Any(x => x.Guid.ToUpper() == userId.ToUpper() && (x.Wachtwoord == newPassword || x.VorigWachtwoord == newPassword));
        }

        public UserApiModel UpdatePassword(string userId, string newPassword)
        {
            var loginData = _dbContext.ViewLogins.Where(x => x.Guid == userId.ToUpper()).FirstOrDefault();
            var login = loginData != null ? _dbContext.Login.Where(x => x.Guid == loginData.Guid && !x.Verwijderd).SingleOrDefault() : null;
            var centralLogin = loginData != null ? _dbContextCentral.CentralLogins.Where(x => x.Guid == loginData.CentralGuid).SingleOrDefault() : null;
            if (login == null || centralLogin == null)
                return null;

            //Update oldPassword and password time in Central Database
            centralLogin.OldPasswordHash = _dbContext.EncryptPassword(loginData.Wachtwoord, loginData.CentralGuid.ToUpperString());
            centralLogin.PasswordHash = _dbContext.EncryptPassword(newPassword, loginData.CentralGuid.ToUpperString());
            centralLogin.ResetPasswordLinkCreatedOn = null;
            centralLogin.OldPasswordResetDate = DateTime.Now;
            return GetUserApiModelFromLogin(login, centralLogin);
        }

        public bool IsValidResetPasswordLinkCreatedDateTime(string userId, DateTime resetPasswordLinkCreatedDateTimeFromClaim)
        {
            return _dbContext.ViewLogins.Any(x => x.Guid.ToUpper() == userId.ToUpper() && x.WijzigWachtwoordLinkAangemaakt.Value == resetPasswordLinkCreatedDateTimeFromClaim);
        }

        public Dictionary<string, string> GetDefaultEmailTokensForUser(string email)
        {
            var user = _dbContext.ViewLogins.Where(login => login.Email.ToLower() == email.ToLower()).SingleOrDefault();
            var organisation = _dbContext.Organisatie.Find(user.OrganisatieGuid);

            var resultDict = new Dictionary<string, string>();
            resultDict["[geachte]"] = _dbContext.GetSalutationForEmail(user.OrganisatieGuid, user.PersoonGuid, user.RelatieGuid, user.KoperHuurderGuid, true);
            resultDict["[geachte_informeel]"] = _dbContext.GetSalutationForEmail(user.OrganisatieGuid, user.PersoonGuid, user.RelatieGuid, user.KoperHuurderGuid, false);

            return resultDict;
        }

        public bool UpdateOfflineConfig(string userId, OfflineConfigApiModel offlineConfig)
        {
            var user = _dbContext.Login.Where(x => x.Guid == userId && !x.Verwijderd).SingleOrDefault();
            if (user != null)
            {
                user.OfflineMode = offlineConfig.Mode;
                if (offlineConfig.Mode)
                {
                    user.OfflineOpleveringOpslaanAantalDagen = offlineConfig.DaysForDelivery;
                    user.OfflineInspectieOpslaanAantalDagen = offlineConfig.DaysForInspection;
                    user.OfflineVoorschouwOpslaanAantalDagen = offlineConfig.DaysForPreDelivery;
                    user.OfflineTweedeHandtekeningOpslaanAantalDagen = offlineConfig.DaysForSecondSignature;
                }
                return true;
            }
            return false;
        }

        public bool VerifyCurrentPassword(string userId, string oldPassword)
        {
            return _dbContext.ViewLogins.Any(x => x.Guid.ToUpper() == userId.ToUpper() && (x.Wachtwoord == oldPassword));
        }

        public List<UserModuleModel> GetAvailableModulesWithRolesForUser(string userId, string projectId = null, string buildingId = null)
        {
            return !string.IsNullOrWhiteSpace(userId) ? GetAvailableModulesWithRoles(userId, projectId, buildingId) : null;
        }

    }
}
