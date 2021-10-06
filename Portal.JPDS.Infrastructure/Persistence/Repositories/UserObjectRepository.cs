using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.AppCore.Repositories;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Portal.JPDS.Domain.Common;

namespace Portal.JPDS.Infrastructure.Persistence.Repositories
{
    public class UserObjectRepository : BaseRepository, IUserObjectRepository
    {

        public UserObjectRepository(AppDbContext dbContext) : base(dbContext)
        {

        }

        public AppDbContext AppDbContext
        {
            get { return _dbContext as AppDbContext; }
        }

        public string GetUploadLocationForBuilding(string buildingId, string repairRequestNo)
        {
            var building = _dbContext.Gebouw.Find(buildingId);
            var project = _dbContext.Werk.Find(building.WerkGuid);

            var uploadLocation = project.MapBijlagen;
            if (project.SubmapPerGebouw == true)
            {
                uploadLocation += building.BouwnummerIntern + "\\";
            }
            if (!string.IsNullOrEmpty(repairRequestNo))
            {
                uploadLocation += repairRequestNo + "\\";
            }

            return uploadLocation;
        }

        public string GetProjectIdForBuilding(string buildingId)
        {
            return _dbContext.Gebouw.Find(buildingId).WerkGuid;
        }

        public IEnumerable<object> GetUserObjectsForApp(string userId, Apps app)
        {
            var loginData = _dbContext.ViewLogins.Where(x => x.Guid == userId).FirstOrDefault();
            var login = loginData != null ? _dbContext.Login.Where(x => x.Guid == loginData.Guid && !x.Verwijderd).FirstOrDefault() : null;
            var user = GetUserApiModelFromLogin(login);

            if (user != null)
            {
                var query = _dbContext.LoginRolWerks.Join(_dbContext.ViewModuleRoles,
                      x => new { x.ModuleGuid, RoleGuid = x.RolGuid },
                      y => new { y.ModuleGuid, y.RoleGuid },
                      (x, y) => new
                      {
                          x.WerkGuid,
                          x.GebouwGuid,
                          y.ModuleName,
                          y.RoleName,
                          x.LoginGuid,
                          y.Active,
                          x.Actief
                      }).Where(x => x.LoginGuid == user.Id && x.Active == true && x.Actief == true);

                switch (app)
                {
                    case Apps.BuyerGuide:
                        {
                            List<UserObjectApiModel> lstUserObjects = new List<UserObjectApiModel>();

                            if (user.AvailableApps.Any(x => x == Apps.BuyerGuide))
                            {
                                if (user.Type == (byte)AccountType.Buyer)
                                {
                                    var availableProjectBuildingData = query.Where(x => x.ModuleName == LoginModules.BuyerGuide && x.RoleName == LoginRoles.BuyerOrRenter)
                                        .Distinct();

                                    lstUserObjects.AddRange(availableProjectBuildingData.Join(_dbContext.Gebouw
                                        .Include(x => x.WerkGu),
                                         x => x.GebouwGuid,
                                         y => y.Guid,
                                         (moduleData, building) => new
                                         {
                                             moduleData.GebouwGuid,
                                             moduleData.WerkGuid,
                                             building
                                         })
                                        .Select(x => new UserObjectApiModel
                                        {
                                            BuildingId = x.building.Guid,
                                            ProjectId = x.building.WerkGuid,
                                            BuildingBuyerRenterId = x.building.KoperHuurderGuid,
                                            ProjectNo = x.building.WerkGu.Werknummer,
                                            ProjectName = x.building.WerkGu.Werknaam,
                                            BuildingNoIntern = x.building.BouwnummerIntern,
                                            BuildingNoExtern = x.building.BouwnummerExtern
                                        }));
                                }
                                else
                                {
                                    var availableProjectBuildingData = query.Where(x => x.ModuleName == LoginModules.BuyerGuide && (x.RoleName == LoginRoles.BuyersGuide || x.RoleName == LoginRoles.SubContractor || x.RoleName == LoginRoles.SiteManager || x.RoleName == LoginRoles.Showroom || x.RoleName == LoginRoles.Spectator))
                                        .Distinct();

                                    var userObjects = availableProjectBuildingData.Join(_dbContext.Gebouw
                                    .Include(x => x.WerkGu)
                                    .Include(x => x.AdresGu)
                                    .Include(x => x.KoperHuurderGu)
                                    .Include(x => x.BouwstroomGu)
                                    .Include(x => x.WoningTypeGu)
                                    .Include(x => x.GebouwStatusGu)
                                    .Include(x => x.GebouwSoortGu),
                                     x => x.WerkGuid,
                                     y => y.WerkGuid,
                                     (moduleData, building) => new
                                     {
                                         moduleData.GebouwGuid,
                                         moduleData.RoleName,
                                         building
                                     })
                                     .Where(x => string.IsNullOrWhiteSpace(x.GebouwGuid))
                                     .Select(x => new
                                     {
                                         BuildingId = x.building.Guid,
                                         ProjectId = x.building.WerkGuid,
                                         BuildingBuyerRenterId = x.building.KoperHuurderGuid,
                                         ProjectNo = x.building.WerkGu.Werknummer,
                                         ProjectName = x.building.WerkGu.Werknaam,
                                         BuildingNoIntern = x.building.BouwnummerIntern,
                                         BuildingNoExtern = x.building.BouwnummerExtern,
                                         PropertyType = x.building.WoningTypeGu.WoningType1,
                                         BuildingType = x.building.GebouwSoortGu.GebouwSoort1,
                                         BuyerRenterName = x.building.KoperHuurderGu.VolledigeNaam,
                                         ConstructionFlow = x.building.BouwstroomGu.Bouwstroom1,
                                         Status = x.building.GebouwStatusGu.GebouwStatus1,
                                         Address = x.building.AdresGu.VolledigAdres,
                                         x.RoleName
                                     }).ToList();

                                    userObjects.AddRange(
                                     availableProjectBuildingData.Join(_dbContext.Gebouw
                                    .Include(x => x.WerkGu)
                                    .Include(x => x.AdresGu)
                                    .Include(x => x.KoperHuurderGu)
                                    .Include(x => x.BouwstroomGu)
                                    .Include(x => x.WoningTypeGu)
                                    .Include(x => x.GebouwStatusGu)
                                    .Include(x => x.GebouwSoortGu),
                                     x => x.GebouwGuid,
                                     y => y.Guid,
                                     (moduleData, building) => new
                                     {
                                         moduleData.RoleName,
                                         building
                                     })
                                     .Select(x => new
                                     {
                                         BuildingId = x.building.Guid,
                                         ProjectId = x.building.WerkGuid,
                                         BuildingBuyerRenterId = x.building.KoperHuurderGuid,
                                         ProjectNo = x.building.WerkGu.Werknummer,
                                         ProjectName = x.building.WerkGu.Werknaam,
                                         BuildingNoIntern = x.building.BouwnummerIntern,
                                         BuildingNoExtern = x.building.BouwnummerExtern,
                                         PropertyType = x.building.WoningTypeGu.WoningType1,
                                         BuildingType = x.building.GebouwSoortGu.GebouwSoort1,
                                         BuyerRenterName = x.building.KoperHuurderGu.VolledigeNaam,
                                         ConstructionFlow = x.building.BouwstroomGu.Bouwstroom1,
                                         Status = x.building.GebouwStatusGu.GebouwStatus1,
                                         Address = x.building.AdresGu.VolledigAdres,
                                         x.RoleName
                                     }));

                                    lstUserObjects.AddRange(userObjects.GroupBy(x => x.BuildingId)
                                        .Select(x => new UserObjectApiModel
                                        {
                                            BuildingId = x.Key,
                                            ProjectId = x.FirstOrDefault().ProjectId,
                                            BuildingBuyerRenterId = x.FirstOrDefault().BuildingBuyerRenterId,
                                            ProjectNo = x.FirstOrDefault().ProjectNo,
                                            ProjectName = x.FirstOrDefault().ProjectName,
                                            BuildingNoIntern = x.FirstOrDefault().BuildingNoIntern,
                                            BuildingNoExtern = x.FirstOrDefault().BuildingNoExtern,
                                            PropertyType = x.FirstOrDefault().PropertyType,
                                            BuildingType = x.FirstOrDefault().BuildingType,
                                            BuyerRenterName = x.FirstOrDefault().BuyerRenterName,
                                            ConstructionFlow = x.FirstOrDefault().ConstructionFlow,
                                            Status = x.FirstOrDefault().Status,
                                            Address = x.FirstOrDefault().Address,
                                            Roles = x.Select(y => Helper.GetLoginRoleName(y.RoleName)).Distinct().ToList()
                                        }));
                                }
                            }

                            return lstUserObjects
                                .Distinct()
                                .OrderBy(x => x.ProjectName)
                                .ThenBy(x => x.BuildingNoExtern);
                        }
                    case Apps.Aftercare:
                        {
                            List<UserObjectApiModel> lstUserObjects = new List<UserObjectApiModel>();
                            if (user.AvailableApps.Any(x => x == Apps.Aftercare))
                            {
                                if (user.Type == (byte)AccountType.Buyer)
                                {
                                    var availableProjectBuildingData = query.Where(x => x.ModuleName == LoginModules.Aftercare && x.RoleName == LoginRoles.BuyerOrRenter)
                                        .Distinct();

                                    lstUserObjects.AddRange(availableProjectBuildingData.Join(
                                        _dbContext.Gebouw
                                        .Include(x => x.AdresGu)
                                        .Include(x => x.WerkGu),
                                         x => x.GebouwGuid,
                                         y => y.Guid,
                                         (moduleData, building) => new
                                         {
                                             moduleData.GebouwGuid,
                                             moduleData.WerkGuid,
                                             building
                                         })
                                        .Where(x => x.building.KoperHuurderGuid == user.BuyerId && (x.building.DatumVoorschouw <= DateTime.Now.Date || x.building.DatumOplevering <= DateTime.Now.Date))
                                        .Select(x => new UserObjectApiModel
                                        {
                                            BuildingId = x.building.Guid,
                                            ProjectId = x.building.WerkGuid,
                                            BuildingBuyerRenterId = x.building.KoperHuurderGuid,
                                            ProjectNo = x.building.WerkGu.Werknummer,
                                            ProjectName = x.building.WerkGu.WerknummerWerknaam,
                                            BuildingNoIntern = x.building.BouwnummerIntern,
                                            BuildingNoExtern = x.building.BouwnummerExtern,
                                            Address = x.building.AdresGu.VolledigAdres
                                        }));
                                }
                                else
                                {
                                    var availableProjectBuildingData = query.Where(x => x.ModuleName == LoginModules.Aftercare && (x.RoleName == LoginRoles.PropertyManager || x.RoleName == LoginRoles.AftercareEmployee))
                                        .Distinct();

                                    lstUserObjects.AddRange(availableProjectBuildingData.Join(
                                     _dbContext.Gebouw.Include(x => x.AdresGu).Include(x => x.WerkGu),
                                     x => x.WerkGuid,
                                     y => y.WerkGuid,
                                     (moduleData, building) => new
                                     {
                                         moduleData.GebouwGuid,
                                         moduleData.WerkGuid,
                                         building
                                     })
                                    .Where(x => string.IsNullOrWhiteSpace(x.GebouwGuid) && (x.building.DatumVoorschouw <= DateTime.Now.Date || x.building.DatumOplevering <= DateTime.Now.Date))
                                    .Select(x => new UserObjectApiModel
                                    {
                                        BuildingId = x.building.Guid,
                                        ProjectId = x.building.WerkGuid,
                                        BuildingBuyerRenterId = x.building.KoperHuurderGuid,
                                        ProjectNo = x.building.WerkGu.Werknummer,
                                        ProjectName = x.building.WerkGu.WerknummerWerknaam,
                                        BuildingNoIntern = x.building.BouwnummerIntern,
                                        BuildingNoExtern = x.building.BouwnummerExtern,
                                        Address = x.building.AdresGu.VolledigAdres
                                    }));

                                    lstUserObjects.AddRange(availableProjectBuildingData.Join(
                                        _dbContext.Gebouw.Include(x => x.AdresGu).Include(x => x.WerkGu),
                                         x => x.GebouwGuid,
                                         y => y.Guid,
                                         (moduleData, building) => new
                                         {
                                             moduleData.GebouwGuid,
                                             moduleData.WerkGuid,
                                             building
                                         })
                                        .Where(x => (x.building.DatumVoorschouw <= DateTime.Now.Date || x.building.DatumOplevering <= DateTime.Now.Date))
                                        .Select(x => new UserObjectApiModel
                                        {
                                            BuildingId = x.building.Guid,
                                            ProjectId = x.building.WerkGuid,
                                            BuildingBuyerRenterId = x.building.KoperHuurderGuid,
                                            ProjectNo = x.building.WerkGu.Werknummer,
                                            ProjectName = x.building.WerkGu.WerknummerWerknaam,
                                            BuildingNoIntern = x.building.BouwnummerIntern,
                                            BuildingNoExtern = x.building.BouwnummerExtern,
                                            Address = x.building.AdresGu.VolledigAdres
                                        }));
                                }
                            }
                            return lstUserObjects
                                .Distinct()
                                .OrderBy(x => x.ProjectName)
                                .ThenBy(x => x.Address)
                                .ThenBy(x => x.BuildingNoExtern);
                        }
                    case Apps.Survey:
                        {
                            List<UserObjectForSurveyApiModel> lstUserObjects = new List<UserObjectForSurveyApiModel>();
                            if (user.AvailableApps.Any(x => x == Apps.Survey))
                            {
                                if (!string.IsNullOrEmpty(user.EmployeeId))
                                {
                                    var availableProjectBuildingData = query.Where(x => x.ModuleName == LoginModules.Survey && x.RoleName == LoginRoles.SiteManager)
                                        .Distinct();

                                    lstUserObjects.AddRange(availableProjectBuildingData.Join(_dbContext.Gebouw.Include(x => x.AdresGu).Include(x => x.WerkGu),
                                         x => x.WerkGuid,
                                         y => y.WerkGuid,
                                        (moduleData, building) => new
                                        {
                                            moduleData.GebouwGuid,
                                            moduleData.WerkGuid,
                                            building
                                        })
                                        .Join(_dbContext.Opname.Include(x => x.Melding)
                                        , p => p.WerkGuid, e => e.WerkGuid, (loginroledata, opname) => new { loginroledata, opname })
                                        .Where(x => string.IsNullOrWhiteSpace(x.loginroledata.GebouwGuid) && x.opname.UitgevoerdDoorMedewerkerGuid == user.EmployeeId)
                                        .Select(x => new UserObjectForSurveyApiModel
                                        {
                                            SurveyId = x.opname.Guid,
                                            SurveyType = (SurveyType)x.opname.OpnameSoort,
                                            BuildingId = x.opname.GebouwGuid,
                                            ProjectId = x.opname.WerkGuid,
                                            BuildingBuyerRenterId = x.loginroledata.building.KoperHuurderGuid ?? string.Empty,
                                            ProjectNo = x.loginroledata.building.WerkGu.Werknummer ?? string.Empty,
                                            ProjectName = x.loginroledata.building.WerkGu.WerknummerWerknaam ?? string.Empty,
                                            BuildingNoIntern = x.loginroledata.building.BouwnummerIntern ?? string.Empty,
                                            BuildingNoExtern = x.loginroledata.building.BouwnummerExtern ?? string.Empty,
                                            Address = x.loginroledata.building.AdresGu.VolledigAdres ?? string.Empty,
                                            ExecutedBy = x.opname.VolledigeNaamUitvoerder ?? string.Empty,
                                            Date = x.opname.Datum,
                                            Status = (SurveyStatus)x.opname.OpnameStatus,
                                            BuyerRenter1 = x.opname.VolledigeNaamKh1 ?? string.Empty,
                                            BuyerRenter2 = x.opname.VolledigeNaamKh2 ?? string.Empty,
                                            RepairRequestsCount = x.opname.Melding.Count
                                        }));

                                    lstUserObjects.AddRange(availableProjectBuildingData.Join(_dbContext.Gebouw
                                         .Include(x => x.AdresGu).Include(x => x.WerkGu),
                                          x => x.GebouwGuid,
                                          y => y.Guid,
                                          (moduleData, building) => new
                                          {
                                              moduleData.GebouwGuid,
                                              moduleData.WerkGuid,
                                              building
                                          })
                                         .Join(_dbContext.Opname.Include(x => x.Melding)
                                         , p => p.building.Guid, e => e.GebouwGuid, (loginroledata, opname) => new { loginroledata, opname })
                                         .Where(x => x.opname.UitgevoerdDoorMedewerkerGuid == user.EmployeeId)
                                         .Select(x => new UserObjectForSurveyApiModel
                                         {
                                             SurveyId = x.opname.Guid,
                                             SurveyType = (SurveyType)x.opname.OpnameSoort,
                                             BuildingId = x.opname.GebouwGuid,
                                             ProjectId = x.opname.WerkGuid,
                                             BuildingBuyerRenterId = x.loginroledata.building.KoperHuurderGuid ?? string.Empty,
                                             ProjectNo = x.loginroledata.building.WerkGu.Werknummer ?? string.Empty,
                                             ProjectName = x.loginroledata.building.WerkGu.WerknummerWerknaam ?? string.Empty,
                                             BuildingNoIntern = x.loginroledata.building.BouwnummerIntern ?? string.Empty,
                                             BuildingNoExtern = x.loginroledata.building.BouwnummerExtern ?? string.Empty,
                                             Address = x.loginroledata.building.AdresGu.VolledigAdres ?? string.Empty,
                                             ExecutedBy = x.opname.VolledigeNaamUitvoerder ?? string.Empty,
                                             Date = x.opname.Datum,
                                             Status = (SurveyStatus)x.opname.OpnameStatus,
                                             BuyerRenter1 = x.opname.VolledigeNaamKh1 ?? string.Empty,
                                             BuyerRenter2 = x.opname.VolledigeNaamKh2 ?? string.Empty,
                                             RepairRequestsCount = x.opname.Melding.Count
                                         }));
                                }
                            }
                            return lstUserObjects
                                .Distinct()
                                .OrderByDescending(x => x.Date)
                                .ThenBy(x => x.BuildingNoExtern)
                                .ThenBy(x => x.Address);
                        }
                    case Apps.ConstructionQuality:
                        {
                            List<UserObjectApiModel> lstUserObjects = new List<UserObjectApiModel>();
                            if (user.AvailableApps.Any(x => x == Apps.ConstructionQuality))
                            {
                                var availableProjectBuildingData = query.Where(x => x.ModuleName == LoginModules.ConstructionQuality && x.RoleName == LoginRoles.SiteManager)
                                    .Distinct();

                                lstUserObjects.AddRange(availableProjectBuildingData
                                .Join(_dbContext.Gebouw.Include(x => x.AdresGu).Include(x => x.WerkGu),
                                x => x.WerkGuid,
                                y => y.WerkGuid,
                                (moduleData, building) => new
                                {
                                    moduleData.GebouwGuid,
                                    moduleData.WerkGuid,
                                    building
                                })
                                .Where(x => string.IsNullOrWhiteSpace(x.GebouwGuid))
                                .Select(x => new UserObjectApiModel
                                {
                                    BuildingId = x.building.Guid,
                                    ProjectId = x.building.WerkGuid,
                                    BuildingBuyerRenterId = x.building.KoperHuurderGuid,
                                    ProjectNo = x.building.WerkGu.Werknummer,
                                    ProjectName = x.building.WerkGu.WerknummerWerknaam,
                                    BuildingNoIntern = x.building.BouwnummerIntern,
                                    BuildingNoExtern = x.building.BouwnummerExtern,
                                    Address = x.building.AdresGu.VolledigAdres
                                }));

                                lstUserObjects.AddRange(availableProjectBuildingData
                                .Join(_dbContext.Gebouw.Include(x => x.AdresGu).Include(x => x.WerkGu),
                                  x => x.GebouwGuid,
                                  y => y.Guid,
                                 (moduleData, building) => new
                                 {
                                     moduleData.GebouwGuid,
                                     moduleData.WerkGuid,
                                     building
                                 })
                                .Select(x => new UserObjectApiModel
                                {
                                    BuildingId = x.building.Guid,
                                    ProjectId = x.building.WerkGuid,
                                    BuildingBuyerRenterId = x.building.KoperHuurderGuid,
                                    ProjectNo = x.building.WerkGu.Werknummer,
                                    ProjectName = x.building.WerkGu.WerknummerWerknaam,
                                    BuildingNoIntern = x.building.BouwnummerIntern,
                                    BuildingNoExtern = x.building.BouwnummerExtern,
                                    Address = x.building.AdresGu.VolledigAdres
                                }));
                            }
                            return lstUserObjects
                                    .Distinct()
                                    .OrderBy(x => x.ProjectName)
                                    .ThenBy(x => x.Address)
                                    .ThenBy(x => x.BuildingNoExtern);
                        }
                    case Apps.ResolverModule:
                        {
                            List<UserObjectApiModel> lstUserObjects = new List<UserObjectApiModel>();
                            if (user.AvailableApps.Any(x => x == Apps.ResolverModule))
                            {
                                if (!string.IsNullOrEmpty(user.OrganisationId))
                                {
                                    var availableProjectBuildingData = query.Where(x => x.ModuleName == LoginModules.ConstructionQuality && x.RoleName == LoginRoles.SubContractor)
                                        .Distinct();

                                    lstUserObjects.AddRange(availableProjectBuildingData.Join(_dbContext.Gebouw.Include(x => x.AdresGu).Include(x => x.WerkGu),
                                      x => x.WerkGuid,
                                      y => y.WerkGuid,
                                        (moduleData, building) => new
                                        {
                                            moduleData.GebouwGuid,
                                            moduleData.WerkGuid,
                                            building
                                        })
                                     .Join(_dbContext.Oplosser.Include(x => x.MeldingGu),
                                      p => p.WerkGuid, e => e.MeldingGu.WerkGuid,
                                     (loginroledata, oplosser) => new { loginroledata, oplosser })
                                     .Where(x => string.IsNullOrWhiteSpace(x.loginroledata.GebouwGuid) && x.oplosser.OrganisatieGuid == user.OrganisationId && !string.IsNullOrWhiteSpace(x.oplosser.Werkbonnummer))
                                     .Select(x => new UserObjectApiModel
                                     {
                                         BuildingId = x.loginroledata.building.Guid,
                                         ProjectId = x.loginroledata.building.WerkGuid,
                                         BuildingBuyerRenterId = x.loginroledata.building.KoperHuurderGuid,
                                         ProjectNo = x.loginroledata.building.WerkGu.Werknummer,
                                         ProjectName = x.loginroledata.building.WerkGu.WerknummerWerknaam,
                                         BuildingNoIntern = x.loginroledata.building.BouwnummerIntern,
                                         BuildingNoExtern = x.loginroledata.building.BouwnummerExtern,
                                         Address = x.loginroledata.building.AdresGu.VolledigAdres
                                     }));

                                    lstUserObjects.AddRange(availableProjectBuildingData.Join(_dbContext.Gebouw.Include(x => x.AdresGu).Include(x => x.WerkGu),
                                      x => x.GebouwGuid,
                                      y => y.Guid,
                                      (moduleData, building) => new
                                      {
                                          moduleData.GebouwGuid,
                                          moduleData.WerkGuid,
                                          building
                                      })
                                      .Join(_dbContext.Oplosser.Include(x => x.MeldingGu),
                                          p => p.GebouwGuid, e => e.MeldingGu.GebouwGuid,
                                      (loginroledata, oplosser) => new { loginroledata, oplosser })
                                      .Where(x => x.oplosser.OrganisatieGuid == user.OrganisationId && !string.IsNullOrWhiteSpace(x.oplosser.Werkbonnummer))
                                      .Select(x => new UserObjectApiModel
                                      {
                                          BuildingId = x.loginroledata.building.Guid,
                                          ProjectId = x.loginroledata.building.WerkGuid,
                                          BuildingBuyerRenterId = x.loginroledata.building.KoperHuurderGuid,
                                          ProjectNo = x.loginroledata.building.WerkGu.Werknummer,
                                          ProjectName = x.loginroledata.building.WerkGu.WerknummerWerknaam,
                                          BuildingNoIntern = x.loginroledata.building.BouwnummerIntern,
                                          BuildingNoExtern = x.loginroledata.building.BouwnummerExtern,
                                          Address = x.loginroledata.building.AdresGu.VolledigAdres
                                      }));
                                }
                            }
                            return lstUserObjects
                                .Distinct()
                                .OrderBy(x => x.ProjectName)
                                .ThenBy(x => x.Address)
                                .ThenBy(x => x.BuildingNoExtern);
                        }
                    default:
                        return new List<UserObjectApiModel>();
                }
            }
            return null;
        }

        public BuildingInfoApiModel GetBuildingInfo(string buildingId)
        {
            return new BuildingInfoApiModel(_dbContext.ViewPortalGebouwAlgemeen.SingleOrDefault(x => x.GebouwGuid == buildingId));
        }

        public ProjectInfoApiModel GetProjectInfo(string projectId)
        {
            return new ProjectInfoApiModel(_dbContext.ViewPortalWerk.SingleOrDefault(x => x.WerkGuid == projectId));
        }

        public IEnumerable<RelaionInfoApiModel> GetProjectRelations(string projectId)
        {
            return _dbContext.ViewPortalBetrokkeneRelatieKopersbegeleider.Where(x => x.WerkGuid == projectId).OrderBy(x => x.Volgorde).Select(x => new RelaionInfoApiModel(x));
        }

        public IEnumerable<InvolvedPartyApiModel> GetInvolvedPartiesByProject(string projectId)
        {
            return _dbContext.ViewPortalBetrokkene.Where(x => x.WerkGuid == projectId).Select(x => new InvolvedPartyApiModel(x));
        }

        public BuyersInfoApiModel GetBuyerInfo(string buyerRenterId)
        {

            return new BuyersInfoApiModel(
                _dbContext.KoperHuurder
                .Include(x => x.Persoon1Gu)
                .Include(x => x.Persoon2Gu)
                .Include(x => x.OrganisatieGu)
                .Include(x => x.RelatieGu).ThenInclude(x => x.PersoonGu)
                .Include(x => x.RelatieGu).ThenInclude(x => x.FunctieGu)
                .Include(x => x.RelatieGu).ThenInclude(x => x.AfdelingGu)
                .Include(x => x.Login)
                .SingleOrDefault(x => x.Guid == buyerRenterId)
                );
        }

        public string GetOrganisationIdForProject(string projectId)
        {
            return _dbContext.ViewPortalBetrokkeneRelatieKopersbegeleider.Where(x => x.WerkGuid == projectId).OrderBy(x => x.Volgorde).Select(x => x.OrganisatieGuid).FirstOrDefault();
        }

        List<CommonKeyValueApiModel> projects = null;
        public IEnumerable<CommonKeyValueApiModel> GetProjectsForSelectionByPhases(string[] phases)
        {
            if (projects == null)
            {
                List<CommonKeyValueApiModel> prjcts = new List<CommonKeyValueApiModel>();
                var projectPhase = _dbContext.WerkFase.Where(x => phases.Contains(x.Fase)).Select(x => x.Guid).ToList();

                var queryBuilder = _dbContext.Werk.Where(x => x.Gebouw.Count > 0);
                //if not found any in database then get all..
                if (phases.Any() && projectPhase.Any())
                {
                    queryBuilder = queryBuilder.Where(x => projectPhase.Contains(x.WerkFaseGuid));
                }
                queryBuilder = queryBuilder.OrderBy(x => x.WerknummerWerknaam);
                foreach (var location in queryBuilder.ToList())
                {
                    prjcts.Add(new CommonKeyValueApiModel(location.Guid, location.WerknummerWerknaam));
                }
                projects = prjcts;
            }
            return projects;
        }

        public IEnumerable<CommonKeyValueApiModel> GetBuildingsForSelectionByProject(string projectId)
        {
            List<CommonKeyValueApiModel> buildings = new List<CommonKeyValueApiModel>();
            foreach (var location in _dbContext.Gebouw.Where(x => x.WerkGuid == projectId).OrderBy(x => x.BouwnummerExtern))
            {
                buildings.Add(new CommonKeyValueApiModel(location.Guid, location.BouwnummerIntern));
            }
            return buildings;
        }

        public Dictionary<string, string> GetDefaultEmailTokensForBuyers(string buildingId)
        {
            var building = _dbContext.Gebouw.Include(x => x.WerkGu.HoofdaannemerOrganisatieGu).Where(x => x.Guid == buildingId).SingleOrDefault();

            return new Dictionary<string, string>
            {
                { "[geachte]",_dbContext.GetSalutationForEmail(null,null,null,building.KoperHuurderGuid,true) },
                { "[geachte_informeel]",_dbContext.GetSalutationForEmail(null,null,null,building.KoperHuurderGuid,false) },
                { "[bouwnummer_intern]",building.BouwnummerIntern },
                { "[bouwnummer_extern]",building.BouwnummerExtern },
                { "[hoofdaannemer]",building.WerkGu?.HoofdaannemerOrganisatieGu?.Naam??string.Empty },
            };
        }

        public List<string> GetBuyerEmails(string buildingId)
        {
            List<string> lstEmails = new List<string>();
            var building = _dbContext.Gebouw.Find(buildingId);
            if (!string.IsNullOrWhiteSpace(building.KoperHuurderGuid))
            {
                lstEmails = (_dbContext.GetEmailsForBuyerRenter(building.KoperHuurderGuid) ?? string.Empty).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            else
            {
                var organisationEmail = _dbContext.Organisatie.Find(building.GebouwGebruikerOrganisatieGuid)?.Email;
                if (!string.IsNullOrWhiteSpace(organisationEmail))
                {
                    lstEmails.Add(organisationEmail);
                }
            }

            return lstEmails;
        }

        public void AddFeedback(string userId, string comment)
        {
            _dbContext.Feedback.Add(new Feedback()
            {
                Guid = Guid.NewGuid().ToUpperString(),
                LoginGuid = userId,
                Opmerking = comment
            });
        }

        public string GetNameForObjectBasedOnConstructionType(string buildingId)
        {
            var constructionType = _dbContext.Gebouw.Include(x => x.WerkGu).SingleOrDefault(x => x.Guid == buildingId)?.WerkGu?.WerkSoort;

            if (constructionType == (int)ConstructionType.nieuwbouw || constructionType == (int)ConstructionType.transformatie)
                return "Bouwnummer";

            return "Object";
        }

        public IEnumerable<ProductServiceApiModel> GetProductServices()
        {
            return _dbContext.ProductDienst.Where(x => x.Actief == true)
                .Include(x => x.ProductDienstSub1s.Where(y => y.Actief == true))
                .ThenInclude(x => x.ProductDienstSub2s.Where(z => z.Actief == true))
                .OrderBy(x => x.ProductCode).Select(x => new ProductServiceApiModel(x));
        }

    }
}
