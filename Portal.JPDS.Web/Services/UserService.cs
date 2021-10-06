using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.AppCore.Common;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Portal.JPDS.Web.Helpers;
using Portal.JPDS.AppCore.Models;
using Portal.JPDS.Domain.Common;


namespace Portal.JPDS.Web.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly IRepoSupervisor _repoSupervisor;
        private readonly IEmailService _emailService;
        public UserService(IOptions<AppSettings> appSettings, IRepoSupervisor repoSupervisor)
        {
            _appSettings = appSettings.Value;
            _repoSupervisor = repoSupervisor;
        }

        public UserApiModel Authenticate(string email, string password)
        {
            var user = _repoSupervisor.Logins.GetLoginByEmailAndPasswordAndUpdateLastLoginTime(email.Trim(), password);

            //Save changes;
            _repoSupervisor.Complete();
            _repoSupervisor.CentralComplete();

            if (user == null)
                return null;

            if (user.Active)
            {
                user.Token = GenerateToken(user.GetUserClaims(), 15, 0);
            }

           
            return user.RemovePassword();
        }

        private string GenerateToken(Claim[] userClaims, int expiresInDays, int expiresInHours = 0, int expiresinMinutes = 0)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.Now.AddDays(expiresInDays).AddHours(expiresInHours).AddMinutes(expiresinMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public UserApiModel GetById(string userId)
        {
            var user = _repoSupervisor.Logins.GetLoginById(userId);


            return user.RemovePassword();
        }

        public UserApiModel GetUserForViewOnly(string userId)
        {
            var user = _repoSupervisor.Logins.GetLoginById(userId);

            user.Token = GenerateToken(user.GetUserViewOnlyClaims(), 0, 1);

            return user.RemovePassword();
        }

        public string GetTokenForResetPasswordAndUpdateResetPasswordLinkCreatedDateTime(string email)
        {
            var user = _repoSupervisor.Logins.GetActiveAccountByEmailAndUpdateResetPasswordLinkCreatedDateTime(email.Trim());

            if (user == null)
                return null;

            //Save changes;
            _repoSupervisor.CentralComplete();

            return GenerateToken(user.GetForgotPasswordUserClaims(), 0, 1);
        }

        public string GetUserNameById(string userId)
        {
            return !string.IsNullOrEmpty(userId) ? _repoSupervisor.Logins.GetUserNameById(userId) : null;
        }

        public bool IsOldPassword(string userId, string newPassword)
        {
            return _repoSupervisor.Logins.IsOldPassword(userId, newPassword);
        }

        public bool UpdatePassword(string userId, string newPassword)
        {
            var User = _repoSupervisor.Logins.UpdatePassword(userId, newPassword);

            //Save changes;
            if (User != null)
            {
                _repoSupervisor.CentralComplete();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsValidResetPasswordLinkCreatedDateTime(string userId, DateTime resetPasswordLinkCreatedDateTimeFromClaim)
        {
            return _repoSupervisor.Logins.IsValidResetPasswordLinkCreatedDateTime(userId, resetPasswordLinkCreatedDateTimeFromClaim);
        }

        public Dictionary<string, string> GetDefaultEmailTokensForUser(string email)
        {
            var tokens = _repoSupervisor.Logins.GetDefaultEmailTokensForUser(email);
            var settings = _repoSupervisor.Config.GetConfigSettings();
            tokens["[bedrijf_naam]"] = settings?.CompanyName ?? string.Empty;
            return tokens;
        }

        public EmailTemplateModel GetForgotPasswordEmailTemplate()
        {
            return _repoSupervisor.Config.GetForgotPasswordEmailTemplate();
        }

        public Apps[] GetUserApps(string userId)
        {
            var user = _repoSupervisor.Logins.GetLoginById(userId);

            List<Apps> apps = new List<Apps>();
            foreach (Apps app in Enum.GetValues(typeof(Apps)))
            {
                switch (app)
                {
                    case Apps.Survey:
                        if (user.Type == (int)AccountType.Employee)
                        {
                            apps.Add(app);
                        }
                        break;
                    case Apps.ConstructionQuality:
                        if (user.Role == (int)LoginRole.servicedienst)
                        {
                            apps.Add(app);
                        }
                        break;
                    case Apps.ResolverModule:
                        if (user.Role == (int)LoginRole.onderaannemer)
                        {
                            apps.Add(app);
                        }
                        break;
                    default:
                        if (user.Role != (int)LoginRole.servicedienst && user.Role != (int)LoginRole.onderaannemer)
                        {
                            if (_repoSupervisor.UserObjects.GetUserObjectsForApp(userId, app).Any())
                            {
                                apps.Add(app);
                            }
                        }
                        break;
                }
            }
            return apps.ToArray();
        }

        public OfflineConfigApiModel GetOfflineConfig(string userId)
        {
            return _repoSupervisor.Config.GetOfflineConfig(userId);
        }

        public bool UpdateOfflineConfig(string userId, OfflineConfigApiModel offlineConfig)
        {
            var success = _repoSupervisor.Logins.UpdateOfflineConfig(userId, offlineConfig);
            if (success)
                _repoSupervisor.Complete();

            return success;
        }

        public bool VerifyCurrentPassword(string userId, string oldPassword)
        {
            return _repoSupervisor.Logins.VerifyCurrentPassword(userId, oldPassword);
        }
    }
}
