using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.Domain.Common;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Portal.JPDS.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Author- Vikas Patyal
    /// Base class to be inherited by  repository classes
    /// </summary>
    public abstract class BaseRepository
    {
        protected readonly AppDbContext _dbContext;
        protected readonly AppDbCentralContext _dbContextCentral;
        public BaseRepository(AppDbContext dbContext, AppDbCentralContext dbContextCentral = null)
        {
            _dbContext = dbContext;
            _dbContextCentral = dbContextCentral;
        }

        public UserApiModel GetUserApiModelFromLogin(Login login, Domain.CentralDBEntities.CentralLogin centralLogin = null)
        {
            var loginData = _dbContext.ViewLogins.Where(x => x.Guid == login.Guid).FirstOrDefault();

            if (loginData != null)
            {
                var moduleWithRolesData = GetAvailableModulesWithRoles(loginData.Guid);

                var appsList = GetApps(moduleWithRolesData);

                return new UserApiModel
                {
                    Id = loginData.Guid,
                    Email = loginData.Email,
                    Username = loginData.Gebruikersnaam,
                    Name = loginData.Naam,
                    Type = loginData.LoginAccountVoor,
                    EmployeeId = loginData.MedewerkerGuid,
                    RelationId = loginData.RelatieGuid,
                    BuyerId = loginData.KoperHuurderGuid,
                    PersonId = loginData.PersoonGuid,
                    Password = centralLogin == null ? loginData.Wachtwoord : _dbContext.DecryptPasswordForEmailAccount(centralLogin.PasswordHash, login.CentralLoginGuid.ToUpper()),
                    Token = string.Empty,
                    Active = loginData.Actief,
                    ResetPasswordLinkCreatedDateTime = centralLogin == null ? loginData.WijzigWachtwoordLinkAangemaakt : centralLogin.ResetPasswordLinkCreatedOn,
                    AvailableApps = appsList?.ToArray(),
                    OrganisationId = loginData.OrganisatieGuid
                };
            }
            return null;
        }

        public List<UserModuleModel> GetAvailableModulesWithRoles(string loginId, string projectId = null, string buildingId = null)
        {
            if (!string.IsNullOrWhiteSpace(loginId))
            {
                var centralLoginId = _dbContext.ViewLogins.Where(x => x.Guid == loginId).Select(x => x.CentralGuid).SingleOrDefault();

                var moduleWithRolesData = _dbContext.LoginRolWerks.Join(_dbContext.ViewModuleRoles,
                        x => new { x.ModuleGuid, RoleGuid = x.RolGuid },
                        y => new { y.ModuleGuid, y.RoleGuid },
                        (x, y) => new
                        {
                            x.LoginGuid,
                            x.WerkGuid,
                            x.GebouwGuid,
                            x.RolGuid,
                            x.ModuleGuid,
                            y.ModuleName,
                            y.RoleName,
                            y.Active,
                            x.Actief
                        })
                        .Where(x => x.LoginGuid == loginId && x.Active == true && x.Actief == true)
                        .Select(x => new UserModuleModel
                        {
                            ModuleId = x.ModuleGuid,
                            ModuleName = x.ModuleName,
                            RoleId = x.RolGuid,
                            RoleName = x.RoleName,
                            ProjectId = x.WerkGuid,
                            BuildingId = x.GebouwGuid
                        })
                        .Where(x => (projectId == null && buildingId == null) || (x.ProjectId == projectId && (string.IsNullOrWhiteSpace(x.BuildingId) || x.BuildingId == buildingId || buildingId == null)))
                        .Distinct().ToList();

                return moduleWithRolesData;
            }
            return null;
        }

        private List<Apps> GetApps(List<UserModuleModel> moduleWithRoles)
        {
            var AppsList = new List<Apps>();
            if (moduleWithRoles.Any(x => x.ModuleName == LoginModules.ConstructionQuality && x.RoleName == LoginRoles.SiteManager))
            {
                AppsList.Add(Apps.ConstructionQuality);
            }
            if (moduleWithRoles.Any(x => x.ModuleName == LoginModules.ConstructionQuality && x.RoleName == LoginRoles.SubContractor))
            {
                AppsList.Add(Apps.ResolverModule);
            }
            if (moduleWithRoles.Any(x => x.ModuleName == LoginModules.BuyerGuide && (x.RoleName == LoginRoles.BuyerOrRenter || x.RoleName == LoginRoles.BuyersGuide || x.RoleName == LoginRoles.SubContractor || x.RoleName == LoginRoles.SiteManager || x.RoleName == LoginRoles.Showroom || x.RoleName == LoginRoles.Spectator)))
            {
                AppsList.Add(Apps.BuyerGuide);
            }
            if (moduleWithRoles.Any(x => x.ModuleName == LoginModules.Survey && x.RoleName == LoginRoles.SiteManager))
            {
                AppsList.Add(Apps.Survey);
            }
            var moduleWithRolesForAfterCare = moduleWithRoles.Where(x => x.ModuleName == LoginModules.Aftercare && (x.RoleName == LoginRoles.BuyerOrRenter || x.RoleName == LoginRoles.PropertyManager || x.RoleName == LoginRoles.AftercareEmployee));
            var projectsForAfterCare = moduleWithRolesForAfterCare.Where(x => x.BuildingId == null).Select(x => x.ProjectId).Distinct().ToList();
            var buildingsForAfterCare = moduleWithRolesForAfterCare.Where(x => x.BuildingId != null).Select(x => x.BuildingId).Distinct().ToList();
            if ((projectsForAfterCare.Any() || buildingsForAfterCare.Any()) && _dbContext.Gebouw.Any(y => (projectsForAfterCare.Contains(y.WerkGuid) || buildingsForAfterCare.Contains(y.Guid)) && (y.DatumVoorschouw <= DateTime.Today || y.DatumOplevering <= DateTime.Today)))
            {
                AppsList.Add(Apps.Aftercare);
            }
            return AppsList.Distinct().ToList();
        }
    }
}
