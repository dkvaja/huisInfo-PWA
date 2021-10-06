using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.AppCore.Models;
using Portal.JPDS.AppCore.Repositories;
using Portal.JPDS.Domain.Common;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;


namespace Portal.JPDS.Infrastructure.Persistence.Repositories
{
    public class DossierRepository : BaseRepository, IDossierRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _siteUrl;
        public DossierRepository(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, IConfiguration appSettingConfig) : base(dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _siteUrl = appSettingConfig["AppSettings:SiteUrl"];
        }

        public AppDbContext AppDbContext
        {
            get { return _dbContext as AppDbContext; }
        }

        /// <summary>
        /// create query for filtering dossiers based on user role and module
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="buildingId"></param>
        /// <param name="dossierId"></param>
        /// <returns></returns>
        private IQueryable<Dossier> BuildQueryForDossiers(string projectId = null, string buildingId = null, string dossierId = null)
        {
            try
            {
                var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                var availableModuleWithRoles = BuildQueryForAvailableModulesRolesForUser(currentUserId, projectId, dossierId);
                var lstAvailableRoles = availableModuleWithRoles.Select(x => x.RoleName).Distinct().ToList();
                if (lstAvailableRoles.Any())
                {
                    if (lstAvailableRoles.Any(x => x.Equals(LoginRoles.BuyerOrRenter)))
                    {
                        var buildings = availableModuleWithRoles.Where(x => x.RoleName.Equals(LoginRoles.BuyerOrRenter)).Select(x => x.BuildingId);
                        return _dbContext.Dossiers
                            .Include(x => x.DossierGebouws)
                            .Include(x => x.DossierVolgordes)
                            .Where(x => (projectId == null || x.WerkGuid == projectId) && (dossierId == null || x.Guid == dossierId) && x.Extern && x.DossierGebouws.Any(y => y.Actief && buildings.Contains(y.GebouwGuid) && (buildingId == null || buildingId == y.GebouwGuid)))
                            .OrderBy(x => x.DossierVolgordes.Min(x => x.Volgorde))
                            .ThenBy(x => x.Deadline);
                    }
                    else
                    {
                        var buildings = availableModuleWithRoles.Select(x => x.BuildingId).Distinct();
                        var isBuyerGuide = lstAvailableRoles.Any(x => x.Equals(LoginRoles.BuyersGuide));
                        var isSpectator = lstAvailableRoles.Any(x => x.Equals(LoginRoles.Spectator));
                        return _dbContext.Dossiers
                            .Include(x => x.LoginDossierRechts)
                            .Include(x => x.DossierGebouws)
                            .Include(x => x.DossierVolgordes)
                            .Where(x =>
                                (projectId == null || x.WerkGuid == projectId) && (dossierId == null || x.Guid == dossierId)
                                && (
                                    buildingId == null
                                    ||
                                    x.DossierGebouws.Any(y => y.Actief && buildingId == y.GebouwGuid && (buildings.Any(z => z == null) || buildings.Contains(y.GebouwGuid)))
                                    )
                                && (isSpectator || x.LoginDossierRechts.Any(x => x.LoginGuid == currentUserId))
                                && (isBuyerGuide || (x.Status != (byte)DossierStatus.Draft && !x.Gearchiveerd))
                                ).OrderBy(x => x.DossierVolgordes.Min(x => x.Volgorde))
                                 .ThenBy(x => x.Deadline);
                    }
                }

                return new List<Dossier>().AsQueryable();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// check if user has edit rights for section
        /// </summary>
        /// <param name="dossierId"></param>
        /// <param name="projectId"></param>
        /// <param name="buildingId"></param>
        /// <param name="isInternal">section - intern or extern</param>
        /// <returns></returns>
        public bool HasEditRightsToSection(string dossierId, string projectId, string buildingId, bool isInternal)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            //for buyer if building id is null return false
            if (IsUserBuyerForDossier(dossierId) && string.IsNullOrEmpty(buildingId))
                return false;

            return BuildQueryForDossiers(projectId, buildingId, dossierId)
             .Where(x => (isInternal || x.Extern) && x.Status == (int)DossierStatus.Open && !x.Gearchiveerd).Any() &&
             BuildQueryForAvailableModulesRolesForUser(currentUserId, projectId, dossierId)
             .Where(x => isInternal ? x.IsInternal && x.HasInternalEditRights : x.IsExternal && x.HasExternalEditRights).Any();

        }

        /// <summary>
        /// Get all dossiers details
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IEnumerable<DossierApiModel> GetAllDossiersByProjectId(string projectId, bool shouldHaveDeadline)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            var availableModuleWithRoles = BuildQueryForAvailableModulesRolesForUser(currentUserId, projectId);


            var isBuyer = availableModuleWithRoles.Select(x => x.RoleName).Any(x => x.Equals(LoginRoles.BuyerOrRenter));
            var buildings = availableModuleWithRoles.Select(x => x.BuildingId).Distinct();

            var dossiers = BuildQueryForDossiers(projectId).Include(x => x.DossierLoginLaatstGelezens)
                .Where(x => (x.Status == (int)DossierStatus.Closed || x.Status == (int)DossierStatus.Open) && !x.Gearchiveerd && (!shouldHaveDeadline || x.Deadline != null))
            .Select(x => new DossierApiModel
            {
                Id = x.Guid,
                ClosedOn = x.AfgeslotenOp,
                Deadline = x.Deadline,
                Name = x.Naam,
                Status = (DossierStatus)x.Status,
                IsArchived = x.Gearchiveerd,
                HasGeneralFiles = x.AlgemeneBestanden,
                HasObjectBoundFiles = x.ObjectgebondenBestanden,
                BuildingInfoList = x.DossierGebouws.Where(y => y.Actief && (buildings.Any(z => z == null) || buildings.Contains(y.GebouwGuid)) && (!isBuyer || x.Extern)).Select(y => new DossiersBuildingApiModel
                {
                    BuildingId = y.GebouwGuid,
                    Status = (DossierStatus)y.Status,
                    Deadline = y.Deadline,
                    IsActive = y.Actief,
                    ClosedOn = y.AfgeslotenOp,
                    IsOverdue = (DossierStatus)y.Status != DossierStatus.Closed && y.Deadline.HasValue && y.Deadline.Value < DateTime.Now.Date,
                    Is48hoursReminder = (DossierStatus)y.Status != DossierStatus.Closed && y.Deadline.HasValue && y.Deadline.Value >= DateTime.Now.Date && (y.Deadline.Value - DateTime.Now).TotalHours <= 48,
                    HasUpdates = !x.DossierLoginLaatstGelezens.Any(z => z.GebouwGuid == y.GebouwGuid && z.BijlageDossierGuid == null && z.LoginGuid == currentUserId && z.LaatstGelezen >= y.GewijzigdOp)
                }).ToList(),
                IsOverdue = (DossierStatus)x.Status != DossierStatus.Closed && x.Deadline.HasValue && x.Deadline.Value < DateTime.Now.Date,
                Is48hoursReminder = (DossierStatus)x.Status != DossierStatus.Closed && x.Deadline.HasValue && x.Deadline.Value >= DateTime.Now.Date && (x.Deadline.Value - DateTime.Now).TotalHours <= 48,
                IsExternal = x.Extern,
                HasUpdates = !x.DossierLoginLaatstGelezens.Any(z => z.LoginGuid == currentUserId && z.GebouwGuid == null && z.BijlageDossierGuid == null && z.LaatstGelezen >= x.GewijzigdOp),
            }).ToList();


            //update hasUpdates based on deeper links
            foreach (var dossier in dossiers.Where(x => !x.HasUpdates))
            {
                if (dossier.BuildingInfoList.Any(x => x.HasUpdates))
                {
                    dossier.HasUpdates = true;
                    continue;
                }
                if (dossier.HasObjectBoundFiles)
                {
                    var buildingsWithFileUpdates = new List<string>();
                    var buildingsWithoutUpdates = dossier.BuildingInfoList.Where(x => !x.HasUpdates).Select(x => x.BuildingId).Distinct().ToList();
                    if (!isBuyer)
                    {
                        buildingsWithFileUpdates = BuildQueryForDossierAttachment(dossier.Id, projectId, buildingsWithoutUpdates, true)
                            .Where(x => x.HasUpdates && (x.HasRights || (!x.IsArchived && !x.IsDeleted))).Select(x => x.BuildingId).ToList();
                        if (buildingsWithFileUpdates.Any())
                            buildingsWithoutUpdates = buildingsWithoutUpdates.Except(buildingsWithFileUpdates).ToList();
                    }
                    if (dossier.IsExternal && buildingsWithoutUpdates.Count > 0)
                    {
                        var result = BuildQueryForDossierAttachment(dossier.Id, projectId, buildingsWithoutUpdates, false)
                            .Where(x => x.HasUpdates && (x.HasRights || (!x.IsArchived && !x.IsDeleted))).Select(x => x.BuildingId).ToList();
                        if (result.Any())
                            buildingsWithFileUpdates = buildingsWithFileUpdates.Union(result).Distinct().ToList();
                    }

                    foreach (var buildingInfo in dossier.BuildingInfoList.Where(x => buildingsWithFileUpdates.Contains(x.BuildingId)))
                    {
                        buildingInfo.HasUpdates = true;
                        dossier.HasUpdates = true;
                    }
                }
                if (dossier.HasGeneralFiles && !dossier.HasUpdates)
                {
                    if (!isBuyer)
                    {
                        dossier.HasUpdates = BuildQueryForDossierAttachment(dossier.Id, projectId, null, true)
                            .Any(x => x.HasUpdates && (x.HasRights || (!x.IsArchived && !x.IsDeleted)));
                    }
                    if (dossier.IsExternal && !dossier.HasUpdates)
                    {
                        dossier.HasUpdates = BuildQueryForDossierAttachment(dossier.Id, projectId, null, false)
                                .Any(x => x.HasUpdates && (x.HasRights || (!x.IsArchived && !x.IsDeleted)));
                    }
                }
            }
            return dossiers;
        }

        /// <summary>
        /// get dossier overview
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        public List<DossierOverviewModel> GetDossiersListByBuildingId(string buildingId, bool shouldHaveDeadline)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            var projectId = _dbContext.Gebouw.Find(buildingId)?.WerkGuid;
            var dossiersForBuilding = BuildQueryForDossiers(projectId, buildingId).Include(x => x.DossierLoginLaatstGelezens)
                .Where(x => (x.Status == (int)DossierStatus.Closed || x.Status == (int)DossierStatus.Open) && !x.Gearchiveerd)
                .Select(y => new
                {
                    Id = y.Guid,
                    ProjectId = y.WerkGuid,
                    Deadline = y.DossierGebouws.Where(x => x.GebouwGuid == buildingId).Select(x => x.Deadline).FirstOrDefault(),
                    Name = y.Naam,
                    Status = y.DossierGebouws.Where(x => x.GebouwGuid == buildingId).Select(x => x.Status).FirstOrDefault(),
                    IsExternal = y.Extern,
                    HasGeneralFiles = y.AlgemeneBestanden,
                    HasObjectBoundFiles = y.ObjectgebondenBestanden,
                    HasUpdates = !y.DossierLoginLaatstGelezens.Any(z => (z.GebouwGuid == null || z.GebouwGuid == buildingId) && z.BijlageDossierGuid == null && z.LoginGuid == currentUserId && z.LaatstGelezen >= y.GewijzigdOp)
                }).Where(x => !shouldHaveDeadline || x.Deadline != null).ToList();

            var result = new List<DossierOverviewModel>();

            foreach (var buildingDossier in dossiersForBuilding)
            {
                var hasUpdates = buildingDossier.HasUpdates;
                if (!hasUpdates && buildingDossier.HasGeneralFiles)
                {
                    hasUpdates = BuildQueryForDossierAttachment(buildingDossier.Id, buildingDossier.ProjectId, null, true)
                                .Any(x => x.HasUpdates && (x.HasRights || (!x.IsArchived && !x.IsDeleted)));
                    if (!hasUpdates && buildingDossier.IsExternal && !buildingDossier.HasUpdates)
                    {
                        hasUpdates = BuildQueryForDossierAttachment(buildingDossier.Id, buildingDossier.ProjectId, null, false)
                            .Any(x => x.HasUpdates && (x.HasRights || (!x.IsArchived && !x.IsDeleted)));
                    }
                }
                if (!hasUpdates && buildingDossier.HasObjectBoundFiles)
                {
                    var buildingIds = new List<string> { buildingId };
                    hasUpdates = BuildQueryForDossierAttachment(buildingDossier.Id, buildingDossier.ProjectId, buildingIds, true)
                                .Any(x => x.HasUpdates && (x.HasRights || (!x.IsArchived && !x.IsDeleted)));
                    if (!hasUpdates && buildingDossier.IsExternal && !buildingDossier.HasUpdates)
                    {
                        hasUpdates = BuildQueryForDossierAttachment(buildingDossier.Id, buildingDossier.ProjectId, buildingIds, false)
                            .Any(x => x.HasUpdates && (x.HasRights || (!x.IsArchived && !x.IsDeleted)));
                    }
                }
                result.Add(new DossierOverviewModel
                {
                    Id = buildingDossier.Id,
                    Deadline = buildingDossier.Deadline,
                    Name = buildingDossier.Name,
                    Status = (DossierStatus)buildingDossier.Status,
                    IsExternal = buildingDossier.IsExternal,
                    HasUpdates = hasUpdates
                });
            }

            return result;
        }

        /// <summary>
        /// get dossier list for deadline view
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public DossiersList GetDossiersListByProjectId(string projectId)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            var filteredDossiers = BuildQueryForDossiers(projectId).Include(x => x.DossierLoginLaatstGelezens).Select(x => new
            {
                x.Guid,
                x.Status,
                x.Gearchiveerd,
                x.Naam,
                x.Deadline,
                x.Extern,
                hasUpdate = !x.DossierLoginLaatstGelezens.Any(y => y.LoginGuid == currentUserId && y.GebouwGuid == null && y.BijlageDossierGuid == null && y.LaatstGelezen >= x.GewijzigdOp)
            });

            return new DossiersList
            {
                OpenOrClosedDossiers = GetAllDossiersByProjectId(projectId, false)
                .Select(x => new DossierOverviewModel
                {
                    Id = x.Id,
                    Deadline = x.Deadline,
                    Name = x.Name,
                    Status = x.Status,
                    IsExternal = x.IsExternal,
                    HasUpdates = x.HasUpdates
                }).ToList(),
                DraftDossiers = filteredDossiers.Where(x => x.Status == (int)DossierStatus.Draft)
                .Select(x => new DossierOverviewModel
                {
                    Id = x.Guid,
                    Deadline = x.Deadline,
                    Name = x.Naam,
                    Status = (DossierStatus)x.Status,
                    IsExternal = x.Extern,
                    HasUpdates = x.hasUpdate
                }).ToList(),
                ArchiveDossiers = filteredDossiers.Where(x => x.Gearchiveerd)
                .Select(x => new DossierOverviewModel
                {
                    Id = x.Guid,
                    Deadline = x.Deadline,
                    Name = x.Naam,
                    Status = (DossierStatus)x.Status,
                    IsExternal = x.Extern,
                    HasUpdates = x.hasUpdate
                }).ToList()
            };

        }

        /// <summary>
        /// get dossier details with file and user details
        /// </summary>
        /// <param name="dossierId"></param>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        private DossierApiModel GetDossierDetails(string dossierId, string buildingId = null)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            var availableModuleWithRoles = BuildQueryForAvailableModulesRolesForUser(currentUserId, null, dossierId);//tolist

            var buildings = availableModuleWithRoles.Select(x => x.BuildingId);
            var isBuyer = availableModuleWithRoles.Select(x => x.RoleName).Any(x => x.Equals(LoginRoles.BuyerOrRenter));
            var filteredDossiers = BuildQueryForDossiers(null, buildingId, dossierId).Include(x => x.DossierLoginLaatstGelezens);
            var dossierData = filteredDossiers.Select(x => new DossierApiModel
            {
                Id = x.Guid,
                ProjectId = x.WerkGuid,
                ClosedOn = x.AfgeslotenOp,
                GeneralInformation = x.AlgemeneInformatie,
                HasBackground = !string.IsNullOrWhiteSpace(x.Afbeelding),
                BackgroundImageName = !string.IsNullOrWhiteSpace(x.Afbeelding) ? Path.GetFileName(x.Afbeelding) : string.Empty,
                Deadline = x.Deadline,
                Name = x.Naam,
                Status = (DossierStatus)x.Status,
                CreatedBy = _dbContext.Login.Where(y => y.Guid == x.AangemaaktDoorLoginGuid).Select(y => y.Naam).FirstOrDefault(),
                IsArchived = x.Gearchiveerd,
                HasGeneralFiles = x.AlgemeneBestanden,
                HasObjectBoundFiles = x.ObjectgebondenBestanden,
                IsOverdue = (DossierStatus)x.Status != DossierStatus.Closed && x.Deadline.HasValue && x.Deadline.Value < DateTime.Now.Date,
                Is48hoursReminder = (DossierStatus)x.Status != DossierStatus.Closed && x.Deadline.HasValue && x.Deadline.Value >= DateTime.Now.Date && (x.Deadline.Value - DateTime.Now).TotalHours <= 48,
                IsExternal = x.Extern,
                HasUpdates = !x.DossierLoginLaatstGelezens.Any(y => y.LoginGuid == currentUserId && y.GebouwGuid == null && y.BijlageDossierGuid == null && y.LaatstGelezen >= x.GewijzigdOp)
            }).FirstOrDefault();
            if (dossierData != null)
            {
                dossierData.BuildingInfoList = _dbContext.DossierGebouws
                    .Include(y => y.DossierGu.DossierLoginLaatstGelezens)
                    .Include(x => x.GebouwGu).ThenInclude(x => x.KoperHuurderGu)
                    .Include(y => y.GebouwGu.KoperHuurderGu.Login)
                    .Include(y => y.GebouwGu.KoperHuurderGu.Persoon1Gu)
                    .Include(y => y.GebouwGu.KoperHuurderGu.Persoon2Gu)
                    .Include(y => y.GebouwGu.KoperHuurderGu.OrganisatieGu)
                    .Include(y => y.GebouwGu.KoperHuurderGu.RelatieGu)
                    .Include(y => y.GebouwGu.KoperHuurderGu.RelatieGu.PersoonGu)
                    .Include(y => y.GebouwGu.KoperHuurderGu.RelatieGu.FunctieGu)
                    .Include(y => y.GebouwGu.KoperHuurderGu.RelatieGu.AfdelingGu)
                    .Where(y => y.DossierGuid == dossierId && (buildingId == null || buildingId == y.GebouwGuid) && y.Actief && (buildings.Any(z => z == null) || buildings.Contains(y.GebouwGuid)) && (!isBuyer || dossierData.IsExternal))
                    .Select(x => new DossiersBuildingApiModel
                    {
                        BuildingId = x.GebouwGuid,
                        Status = (DossierStatus)x.Status,
                        Deadline = x.Deadline,
                        IsActive = x.Actief,
                        ClosedOn = x.AfgeslotenOp,
                        BuyerContactInfo = buildingId != null && !string.IsNullOrWhiteSpace(x.GebouwGu.KoperHuurderGuid) && dossierData.IsExternal ? new BuyersInfoApiModel(x.GebouwGu.KoperHuurderGu) : null,
                        HasUpdates = !x.DossierGu.DossierLoginLaatstGelezens.Any(y => y.LoginGuid == currentUserId && y.GebouwGuid == x.GebouwGuid && y.BijlageDossierGuid == null && y.LaatstGelezen >= x.GewijzigdOp)
                    }).ToList();

                if (dossierData.HasObjectBoundFiles)
                {
                    if (buildingId != null)
                    {
                        foreach (var buildingList in dossierData.BuildingInfoList)
                        {
                            if (!isBuyer)
                            {
                                buildingList.InternalObjectFiles = GetFilesForDossier(dossierId, dossierData.ProjectId, true, buildingId, true);
                            }
                            if (dossierData.IsExternal)
                            {
                                buildingList.ExternalObjectFiles = GetFilesForDossier(dossierId, dossierData.ProjectId, false, buildingId, true);
                            }
                            if (!buildingList.HasUpdates)
                            {
                                buildingList.HasUpdates = buildingList.InternalObjectFiles?.UploadedFiles?.Any(x => x.HasUpdates) == true
                                          || buildingList.InternalObjectFiles?.ArchivedFiles?.Any(x => x.HasUpdates) == true
                                          || buildingList.InternalObjectFiles?.DeletedFiles?.Any(x => x.HasUpdates) == true
                                          || buildingList.ExternalObjectFiles?.UploadedFiles?.Any(x => x.HasUpdates) == true
                                          || buildingList.ExternalObjectFiles?.ArchivedFiles?.Any(x => x.HasUpdates) == true
                                          || buildingList.ExternalObjectFiles?.DeletedFiles?.Any(x => x.HasUpdates) == true;
                            }
                        }
                    }
                    else
                    {
                        foreach (var buildingList in dossierData.BuildingInfoList.Where(x => !x.HasUpdates))
                        {
                            var buildingsWithFileUpdates = new List<string>();
                            var buildingsWithoutUpdates = dossierData.BuildingInfoList.Where(x => !x.HasUpdates).Select(x => x.BuildingId).Distinct().ToList();
                            if (!isBuyer)
                            {
                                buildingsWithFileUpdates = BuildQueryForDossierAttachment(dossierId, dossierData.ProjectId, buildingsWithoutUpdates, true)
                                    .Where(x => x.HasUpdates && (x.HasRights || (!x.IsArchived && !x.IsDeleted))).Select(x => x.BuildingId).ToList();
                                if (buildingsWithFileUpdates.Any())
                                    buildingsWithoutUpdates = buildingsWithoutUpdates.Except(buildingsWithFileUpdates).ToList();
                            }
                            if (dossierData.IsExternal && buildingsWithoutUpdates.Count > 0)
                            {
                                var result = BuildQueryForDossierAttachment(dossierId, dossierData.ProjectId, buildingsWithoutUpdates, false)
                                    .Where(x => x.HasUpdates && (x.HasRights || (!x.IsArchived && !x.IsDeleted))).Select(x => x.BuildingId).ToList();
                                if (result.Any())
                                    buildingsWithFileUpdates = buildingsWithFileUpdates.Union(result).Distinct().ToList();
                            }

                            foreach (var buildingInfo in dossierData.BuildingInfoList.Where(x => buildingsWithFileUpdates.Contains(x.BuildingId)))
                            {
                                buildingInfo.HasUpdates = true;
                            }
                        }
                    }
                }
                if (dossierData.HasGeneralFiles)
                {
                    if (!isBuyer)
                    {
                        dossierData.InternalFiles = GetFilesForDossier(dossierId, dossierData.ProjectId, true, null, buildingId == null);
                    }
                    if (dossierData.IsExternal)
                    {
                        dossierData.ExternalFiles = GetFilesForDossier(dossierId, dossierData.ProjectId, false, null, buildingId == null);
                    }
                }
                dossierData.UserList = GetAllDossierUsersWithRights(dossierId, dossierData.IsExternal, dossierData.ProjectId, buildingId).Where(x=> !isBuyer || (x.IsExternal && x.IsExternalVisible)).ToList();
                if (buildingId == null && !dossierData.HasUpdates)
                {
                    dossierData.HasUpdates = dossierData.InternalFiles?.UploadedFiles?.Any(x => x.HasUpdates) == true
                                          || dossierData.InternalFiles?.ArchivedFiles?.Any(x => x.HasUpdates) == true
                                          || dossierData.InternalFiles?.DeletedFiles?.Any(x => x.HasUpdates) == true
                                          || dossierData.ExternalFiles?.UploadedFiles?.Any(x => x.HasUpdates) == true
                                          || dossierData.ExternalFiles?.ArchivedFiles?.Any(x => x.HasUpdates) == true
                                          || dossierData.ExternalFiles?.DeletedFiles?.Any(x => x.HasUpdates) == true;
                }
                return dossierData;
            }

            return null;
        }

        /// <summary>
        /// get dossier details with building details with file details
        /// </summary>
        /// <param name="dossierId"></param>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        public DossierApiModel GetDossierBuildingInfo(string dossierId, string buildingId)
        {
            return GetDossierDetails(dossierId, buildingId);
        }

        /// <summary>
        /// get dossier general info with file details
        /// </summary>
        /// <param name="dossierId"></param>
        /// <returns></returns>
        public DossierApiModel GetDossierGeneralInfo(string dossierId)
        {
            return GetDossierDetails(dossierId);
        }

        private IQueryable<LoginDossierUserRightsModel> BuildQueryForDossierUsersWithRights(string dossierId, string projectId = null, string buildingId = null)
        {
            var currentUser = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            bool isBuyer = IsUserBuyerForDossier(dossierId, currentUser);
            bool isBuyerGuide = IsUserBuyerGuideForDossier(dossierId, currentUser);
            return _dbContext.LoginRolWerks.Join(_dbContext.ViewModuleRoles,
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
                          .Where(x => x.Active == true && x.Actief == true && x.ModuleName == LoginModules.BuyerGuide && (projectId == null || x.WerkGuid == projectId) && (buildingId == null || x.GebouwGuid == null || x.GebouwGuid == buildingId))
                          .Join(_dbContext.LoginDossierRechts.Include(x => x.DossierGu).Include(x => x.LoginGu),
                          x => new { x.LoginGuid, x.WerkGuid, x.ModuleGuid, x.RolGuid },
                          y => new { y.LoginGuid, y.DossierGu.WerkGuid, y.ModuleGuid, y.RolGuid },
                          (x, y) => new LoginDossierUserRightsModel
                          {
                              DossierId = y.DossierGuid,
                              DossierRightId = y.Guid,
                              LoginId = x.LoginGuid,
                              Name = y.LoginGu.Naam,
                              Email = _dbContext.ViewLogins.Where(y => y.Guid == x.LoginGuid).Select(x => x.Email).SingleOrDefault(),
                              ModuleId = x.ModuleGuid,
                              RoleName = x.RoleName,
                              RoleId = x.RolGuid,
                              HasExternalEditRights = y.ExternBestandWijzigen,
                              IsExternalVisible = y.ExternRelatieZichtbaar,
                              LoginOrgId = y.LoginGu.OrganisatieGuid,
                              LoginRelationId = y.LoginGu.RelatieGuid,
                              IsExternal = y.Extern,
                              IsInternal = y.Intern,
                              HasInternalEditRights = y.InternBestandWijzigen,
                              IsInternalVisible = y.InternRelatieZichtbaar
                          })
                          .Where(x => x.DossierId == dossierId && (!isBuyer || x.HasExternalEditRights));
        }

        /// <summary>
        /// return dossieruserapi model from loginDossierRecht table. Combine multiple rows of same user in db into single record
        /// </summary>
        /// <param name="loginDossierRechts"></param>
        /// <returns></returns>
        private List<DossierUserApiModel> GetAllDossierUsersWithRights(string dossierId, bool isExternal, string projectId, string buildingId = null)
        {
            try
            {
                var currentUser = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                bool isBuyerGuide = IsUserBuyerGuideForDossier(dossierId, currentUser);
                //if buyer then externrela should be true, rest all users can view

                return BuildQueryForDossierUsersWithRights(dossierId, projectId, buildingId)
                            .ToList()
                            .Select(x => new DossierUserApiModel
                            {
                                DossierRightId = x.DossierRightId,
                                LoginId = x.LoginId,
                                Name = x.Name,
                                Email = x.Email,
                                ModuleId = x.ModuleId,
                                RoleId = x.RoleId,
                                RoleName = x.RoleName,
                                HasExternalEditRights = isBuyerGuide ? x.HasExternalEditRights : isExternal && x.HasExternalEditRights,
                                IsExternalVisible = isBuyerGuide ? x.IsExternalVisible : isExternal && x.IsExternalVisible,
                                IsInternal = x.IsInternal,
                                IsExternal = x.IsExternal,
                                HasInternalEditRights = x.HasInternalEditRights,
                                IsInternalVisible = x.IsInternalVisible,
                                UserContactInfo = GetUserContactInfo(x.LoginOrgId, x.LoginRelationId, projectId)
                            }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [Obsolete("Not required for any view. Will be removed in future.")]
        public IEnumerable<string> GetObjectsForDossier(string projectId)
        {
            return BuildQueryForDossiers(projectId).Where(x => x.DossierGebouws.Any()).SelectMany(x => x.DossierGebouws).Select(x => x.GebouwGuid).Distinct();

        }

        /// <summary>
        /// Get all Users info with module and role
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IEnumerable<DossierRoleswithUserApiModel> GetAvailableUsersAndRolesByProjectId(string projectId)
        {
            // Excepts Buyer
            var userList =
               _dbContext.LoginRolWerks.Include(x => x.LoginGu)
                 .Join(_dbContext.ViewModuleRoles,
                  x => new { x.ModuleGuid, RoleGuid = x.RolGuid },
                  y => new { y.ModuleGuid, y.RoleGuid },
                  (x, y) => new
                  {
                      x.LoginGuid,
                      x.WerkGuid,
                      x.LoginGu.Naam,
                      x.RolGuid,
                      y.RoleName,
                      x.ModuleGuid,
                      x.Actief,
                      y.Active,
                      y.ModuleName
                  }).Where(x => x.WerkGuid == projectId && (x.RoleName == LoginRoles.BuyersGuide || x.RoleName == LoginRoles.SubContractor || x.RoleName == LoginRoles.SiteManager || x.RoleName == LoginRoles.Showroom)
                                    && x.Active == true && x.Actief == true && x.ModuleName == LoginModules.BuyerGuide)
                .Select(x => new
                {
                    x.RolGuid,
                    x.ModuleGuid,
                    x.LoginGuid,
                    x.Naam,
                    x.RoleName,
                    x.ModuleName
                }).Distinct().ToList();

            return userList
            .GroupBy(x => new { x.RolGuid, x.ModuleGuid })
            .Select(y =>
            new DossierRoleswithUserApiModel()
            {
                RoleId = y.Key.RolGuid,
                RoleName = y.Select(x => x.RoleName).FirstOrDefault(),
                ModuleId = y.Key.ModuleGuid,
                ModuleName = y.Select(x => x.ModuleName).FirstOrDefault(),
                UsersList = y.Select(x => new DossierUserListModel
                {
                    LoginId = x.LoginGuid,
                    Name = x.Naam
                }).ToList()
            });

        }

        /// <summary>
        /// create or update dossier details based on dossier id present in model
        /// </summary>
        /// <param name="newDossier"></param>
        /// <returns>empty if failed</returns>
        public string CreateOrUpdateDossier(NewDossierApiModel newDossier)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            //only buyer's guide create dossier
            if (HasRolesForProject(newDossier.ProjectId, currentUserId, new List<string> { LoginRoles.BuyersGuide }))
            {
                var dossierOrder = _dbContext.DossierVolgordes.Where(x => x.WerkGuid == newDossier.ProjectId).OrderBy(x => x.Volgorde).ToList();
                var existing = _dbContext.Dossiers.Find(newDossier.DossierId);
                if (existing != null)
                {
                    //update
                    existing.WerkGuid = newDossier.ProjectId;
                    existing.Deadline = newDossier.Deadline;
                    existing.Afbeelding = UploadBackgroundImageforDossier(newDossier.DossierId, newDossier.ProjectId, newDossier.BackgroundImage);
                    existing.AlgemeneInformatie = newDossier.GeneralInformation;
                    existing.Naam = newDossier.Name.Length >= 100 ? newDossier.Name.Substring(0, 100).Trim() : newDossier.Name.Trim();
                    existing.Status = newDossier.IsDraft ? (byte)DossierStatus.Draft : (byte)DossierStatus.Open;
                    existing.AangemaaktDoorLoginGuid = currentUserId;
                    existing.AlgemeneBestanden = newDossier.HasGeneralFiles;
                    existing.ObjectgebondenBestanden = newDossier.HasObjectBoundFiles;
                    existing.Extern = newDossier.IsExternal;
                    if (newDossier.BuildingInfoList != null && newDossier.BuildingInfoList.Any())
                    {
                        //update for object
                        UpdateDossierbuilding(newDossier);
                    }
                    //update dossier user rights
                    UpdateAddRemoveDossierUserRights(newDossier.DossierId, newDossier.UserList);
                }
                else
                {
                    //insert
                    var dossier = new Dossier
                    {
                        Guid = Guid.NewGuid().ToUpperString(),
                        WerkGuid = newDossier.ProjectId,
                        Deadline = newDossier.Deadline,
                        AlgemeneInformatie = newDossier.GeneralInformation,
                        Naam = newDossier.Name.Length >= 100 ? newDossier.Name.Substring(0, 100).Trim() : newDossier.Name.Trim(),
                        Status = newDossier.IsDraft ? (byte)DossierStatus.Draft : (byte)DossierStatus.Open,
                        AangemaaktDoorLoginGuid = currentUserId,
                        AlgemeneBestanden = newDossier.HasGeneralFiles,
                        ObjectgebondenBestanden = newDossier.HasObjectBoundFiles,
                        Extern = newDossier.IsExternal
                    };
                    dossier.Afbeelding = UploadBackgroundImageforDossier(dossier.Guid, newDossier.ProjectId, newDossier.BackgroundImage);
                    _dbContext.Dossiers.Add(dossier);
                    // insert new dossier into Dossier Last view
                    _dbContext.DossierLoginLaatstGelezens.Add(new DossierLoginLaatstGelezen
                    {
                        Guid = Guid.NewGuid().ToUpperString(),
                        DossierGuid = dossier.Guid,
                        GebouwGuid = null,
                        BijlageDossierGuid = null,
                        LoginGuid = currentUserId,
                        LaatstGelezen = DateTime.Now
                    });
                    foreach (var buildingData in newDossier.BuildingInfoList)
                    {
                        _dbContext.DossierGebouws.Add(new DossierGebouw
                        {
                            Guid = Guid.NewGuid().ToUpperString(),
                            DossierGuid = dossier.Guid,
                            GebouwGuid = buildingData.BuildingId,
                            Deadline = newDossier.Deadline,
                            Status = newDossier.IsDraft ? (byte)DossierStatus.Draft : (byte)DossierStatus.Open,
                            Actief = buildingData.IsActive
                        });
                        // insert new dossier buildings into Dossier Last view  
                        _dbContext.DossierLoginLaatstGelezens.Add(new DossierLoginLaatstGelezen
                        {
                            Guid = Guid.NewGuid().ToUpperString(),
                            DossierGuid = dossier.Guid,
                            GebouwGuid = buildingData.BuildingId,
                            BijlageDossierGuid = null,
                            LoginGuid = currentUserId,
                            LaatstGelezen = DateTime.Now
                        });
                    }

                    AddDossierUserRights(dossier.Guid, newDossier.UserList);
                    //insert dossier Order
                    newDossier.DossierId = dossier.Guid;
                }
                if (!newDossier.IsDraft)
                {
                    if (dossierOrder.Any())
                    {
                        short order = 10;
                        AddDossierOrder(newDossier.DossierId, newDossier.ProjectId, order);
                        ReorderDossiers(null, newDossier.DossierId, dossierOrder, order);
                    }
                }
                return newDossier.DossierId;
            }
            return string.Empty;
        }

        /// <summary>
        /// get file location for dossier
        /// </summary>
        /// <param name="dossierId"></param>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        public string GetUploadLocationForDossierFiles(string dossierId, string buildingId)
        {
            var dossierData = _dbContext.Dossiers.Find(dossierId);
            var project = _dbContext.Werk.Find(dossierData.WerkGuid);
            var uploadLocation = project.MapBijlagen + "Dossier\\" + dossierData.Guid;
            if (!string.IsNullOrWhiteSpace(buildingId))
            {
                if (project.SubmapPerGebouw == true)
                {
                    uploadLocation += "\\" + buildingId + "\\";
                }
            }
            return uploadLocation;
        }

        /// <summary>
        /// create file for dossier
        /// </summary>
        /// <param name="fileApiModel"></param>
        /// <param name="attachmentId"></param>
        /// <returns></returns>
        public string CreateFile(DossierFileApiModel fileApiModel, string attachmentId)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            var dossier = _dbContext.Dossiers.Find(fileApiModel.DossierId);
            if (dossier != null)
            {
                var fileImageData = new BijlageDossier
                {
                    Guid = Guid.NewGuid().ToUpperString(),
                    BijlageGuid = attachmentId,
                    GebouwGuid = !string.IsNullOrWhiteSpace(fileApiModel.BuildingId) ? fileApiModel.BuildingId : null,
                    DossierGuid = fileApiModel.DossierId,
                    Intern = fileApiModel.IsInternal,
                    Gearchiveerd = false,
                    Verwijderd = false,
                    AangemaaktDoorLoginGuid = currentUserId
                };
                _dbContext.BijlageDossiers.Add(fileImageData);
                return fileImageData.Guid;
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// update individual fields of dossier
        /// </summary>
        /// <param name="dossierId"></param>
        /// <param name="lstKeyValues"></param>
        /// <returns></returns>
        public bool UpdateDossierDataKeyValue(string dossierId, CommonKeyValueApiModel lstKeyValues)
        {
            var dossierData = _dbContext.Dossiers.Where(x => x.Guid == dossierId).Include(x => x.DossierGebouws).SingleOrDefault();
            if (dossierData != null && (dossierData.Status == (int)DossierStatus.Open))
            {
                switch (lstKeyValues.Id.ToLower())
                {
                    case "generalinformation":
                        if (!IsUserBuyerForDossier(dossierId) && !dossierData.Gearchiveerd)
                        {
                            dossierData.AlgemeneInformatie = lstKeyValues.Name;
                            return true;
                        }
                        else
                            return false;
                    case "isgeneralfilessectionavailable":
                        if (IsUserBuyerGuideForDossier(dossierId) && !dossierData.Gearchiveerd)
                        {
                            bool value = Convert.ToBoolean(lstKeyValues.Name);
                            if (!value)
                            {
                                if (dossierData.ObjectgebondenBestanden)
                                {
                                    dossierData.AlgemeneBestanden = !string.IsNullOrWhiteSpace(lstKeyValues.Name) && Convert.ToBoolean(lstKeyValues.Name);
                                    return true;
                                }
                            }
                            else
                            {
                                dossierData.AlgemeneBestanden = !string.IsNullOrWhiteSpace(lstKeyValues.Name) && Convert.ToBoolean(lstKeyValues.Name);
                                return true;
                            }
                            return false;
                        }
                        else
                            return false;
                    case "isobjectfilessectionavailable":
                        if (IsUserBuyerGuideForDossier(dossierId) && !dossierData.Gearchiveerd)
                        {
                            bool value = Convert.ToBoolean(lstKeyValues.Name);
                            if (!value)
                            {
                                if (dossierData.AlgemeneBestanden)
                                {
                                    dossierData.ObjectgebondenBestanden = !string.IsNullOrWhiteSpace(lstKeyValues.Name) && Convert.ToBoolean(lstKeyValues.Name);
                                    return true;
                                }
                            }
                            else
                            {
                                dossierData.ObjectgebondenBestanden = !string.IsNullOrWhiteSpace(lstKeyValues.Name) && Convert.ToBoolean(lstKeyValues.Name);
                                return true;
                            }
                            return false;
                        }
                        else
                            return false;
                    case "archive":
                        if (IsUserBuyerGuideForDossier(dossierId))
                        {
                            dossierData.Gearchiveerd = !string.IsNullOrWhiteSpace(lstKeyValues.Name) && Convert.ToBoolean(lstKeyValues.Name);
                            return true;
                        }
                        else
                            return false;
                    case "extern":
                        if (IsUserBuyerGuideForDossier(dossierId) && !dossierData.Gearchiveerd)
                        {
                            dossierData.Extern = !string.IsNullOrWhiteSpace(lstKeyValues.Name) && Convert.ToBoolean(lstKeyValues.Name);
                            return true;
                        }
                        else
                            return false;
                    default:
                        return false;
                }
            }
            else
                return false;
        }

        /// <summary>
        /// update dossier building fields
        /// </summary>
        /// <param name="dossierId"></param>
        /// <param name="buildingId"></param>
        /// <param name="lstKeyValues"></param>
        /// <returns></returns>
        public bool UpdateBuildingDossierDataKeyValue(string dossierId, string buildingId, CommonKeyValueApiModel lstKeyValues)
        {
            var dossierData = _dbContext.Dossiers.Find(dossierId);
            if (dossierData != null && (dossierData.Status == (int)DossierStatus.Open || dossierData.Status == (int)DossierStatus.Draft))
            {
                var dossierBuildingData = _dbContext.DossierGebouws.Where(x => x.DossierGuid == dossierId && x.GebouwGuid == buildingId).SingleOrDefault();
                if (dossierBuildingData != null && dossierBuildingData.Status == (int)DossierStatus.Open)
                {
                    switch (lstKeyValues.Id.ToLower())
                    {
                        case "deadline":
                            if (IsUserBuyerGuideForDossier(dossierId))
                            {
                                dossierBuildingData.Deadline = !string.IsNullOrWhiteSpace(lstKeyValues.Name) ? Convert.ToDateTime(lstKeyValues.Name) : null;
                                return true;
                            }
                            else
                                return false;
                        default:
                            return false;
                    }
                }
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// delete dossier
        /// </summary>
        /// <param name="dossierId"></param>
        /// <returns></returns>
        public bool DeleteDossier(string dossierId)
        {
            var dossierData = _dbContext.Dossiers.Where(x => x.Guid == dossierId).Include(x => x.DossierGebouws)
                .Include(x => x.LoginDossierRechts).Include(x => x.BijlageDossiers).Include(x => x.DossierLoginLaatstGelezens).SingleOrDefault();
            //only delete when marked as draft
            if (dossierData != null && dossierData.Status == (int)DossierStatus.Draft)
            {
                if (dossierData.LoginDossierRechts.Any())
                    _dbContext.LoginDossierRechts.RemoveRange(dossierData.LoginDossierRechts);
                if (dossierData.DossierGebouws.Any())
                    _dbContext.DossierGebouws.RemoveRange(dossierData.DossierGebouws);
                if (dossierData.BijlageDossiers.Any())
                    _dbContext.BijlageDossiers.RemoveRange(dossierData.BijlageDossiers);
                if (dossierData.DossierLoginLaatstGelezens.Any())
                    _dbContext.DossierLoginLaatstGelezens.RemoveRange(dossierData.DossierLoginLaatstGelezens);
                _dbContext.Dossiers.Remove(dossierData);
                return true;
            }
            return false;
        }

        /// <summary>
        /// add users to LoginDossierRechts
        /// </summary>
        /// <param name="dossierId"></param>
        /// <param name="newUserApiModel"></param>
        /// <returns></returns>
        private bool AddDossierUserRights(string dossierId, IEnumerable<NewDossierUserRightApiModel> newUserApiModel)
        {
            try
            {
                if (dossierId != Guid.Empty.ToUpperString())
                {
                    var currentUser = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                    if (newUserApiModel.Any())
                    {
                        var buyerGuideModuleRole = _dbContext.ViewModuleRoles
                        .Where(x => x.ModuleName == LoginModules.BuyerGuide && x.RoleName == LoginRoles.BuyersGuide)
                        .Select(x => new { x.ModuleGuid, x.RoleGuid }).SingleOrDefault();

                        newUserApiModel = newUserApiModel.Where(x => !string.IsNullOrEmpty(x.ModuleId) && !string.IsNullOrEmpty(x.RoleId)).Distinct();
                        newUserApiModel.ToList().ForEach(x => x.HasInternalEditRights = x.IsInternalVisible = x.IsInternal);
                        //check if logged in user is present in input
                        bool isUserPresentAsBuyerGuide = newUserApiModel.Any(x => x.LoginId.Equals(currentUser) && x.ModuleId.Equals(buyerGuideModuleRole.ModuleGuid) && x.RoleId.Equals(buyerGuideModuleRole.RoleGuid));

                        if (isUserPresentAsBuyerGuide)
                            newUserApiModel.Where(x => x.LoginId.Equals(currentUser) && buyerGuideModuleRole.RoleGuid == x.RoleId && buyerGuideModuleRole.ModuleGuid == x.ModuleId).ToList().ForEach(x => { x.IsInternal = true; x.IsExternal = true; x.HasExternalEditRights = true; x.HasInternalEditRights = true; });

                        foreach (var userData in newUserApiModel)
                        {
                            _dbContext.LoginDossierRechts.Add(new LoginDossierRecht
                            {
                                Guid = Guid.NewGuid().ToUpperString(),
                                DossierGuid = dossierId,
                                LoginGuid = userData.LoginId,
                                ModuleGuid = userData.ModuleId,
                                RolGuid = userData.RoleId,
                                ExternRelatieZichtbaar = userData.IsExternalVisible,
                                ExternBestandWijzigen = userData.HasExternalEditRights,
                                Intern = userData.IsInternal,
                                InternBestandWijzigen = userData.HasInternalEditRights,
                                InternRelatieZichtbaar = userData.IsInternalVisible,
                                Extern = userData.IsExternal
                            });
                        }
                        if (!isUserPresentAsBuyerGuide)
                            InsertUserDossierRightsForBuyerGuide(dossierId, currentUser);
                    }
                    else
                    {
                        //insert buyer guide role by default
                        InsertUserDossierRightsForBuyerGuide(dossierId, currentUser);
                    }
                }
                else
                    return false;
                return true;
            }
            catch (Exception)
            {
                throw;
                //return false;
            }
        }

        private void InsertUserDossierRightsForBuyerGuide(string dossierId, string userId)
        {
            try
            {
                var moduleRole = _dbContext.ViewModuleRoles
                    .Where(x => x.ModuleName == LoginModules.BuyerGuide && x.RoleName == LoginRoles.BuyersGuide)
                    .Select(x => new { x.ModuleGuid, x.RoleGuid }).SingleOrDefault();
                _dbContext.LoginDossierRechts.Add(new LoginDossierRecht
                {
                    Guid = Guid.NewGuid().ToUpperString(),
                    DossierGuid = dossierId,
                    LoginGuid = userId,
                    ModuleGuid = moduleRole.ModuleGuid,
                    RolGuid = moduleRole.RoleGuid,
                    ExternBestandWijzigen = true,
                    ExternRelatieZichtbaar = false,
                    Intern = true,
                    InternRelatieZichtbaar = true,
                    InternBestandWijzigen = true,
                    Extern = true
                });
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Update, Insert or delete from LoginDossierRechts based on input
        /// </summary>
        /// <param name="dossierId"></param>
        /// <param name="newUserApiModel"></param>
        /// <returns></returns>
        private bool UpdateAddRemoveDossierUserRights(string dossierId, IEnumerable<NewDossierUserRightApiModel> newUserApiModel)
        {
            try
            {
                if (dossierId != Guid.Empty.ToUpperString() && newUserApiModel != null)
                {
                    var currentUser = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                    var allDossierLogins = _dbContext.LoginDossierRechts.Where(x => x.DossierGuid.Equals(dossierId));//.ToList();

                    foreach (var input in newUserApiModel.Distinct())
                    {
                        if (currentUser == input.LoginId)
                        {
                            input.IsInternal = true;
                            input.IsExternal = true;
                            input.HasExternalEditRights = true;
                        }

                        if (input.IsInternal || input.IsExternal)
                        {
                            //UpdateOrInsertDossierLoginRights(dossierId, input, allDossierLogins, true);
                            //check for update or insert
                            var existing = allDossierLogins.Where(x => x.LoginGuid.Equals(input.LoginId)
                                 && x.RolGuid == input.RoleId && x.ModuleGuid == input.ModuleId);
                            if (existing.Any())
                            {
                                //update                    
                                var updateObj = _dbContext.LoginDossierRechts.Find(existing.First().Guid);
                                updateObj.ExternRelatieZichtbaar = input.IsExternalVisible;
                                updateObj.ExternBestandWijzigen = input.HasExternalEditRights;
                                updateObj.Intern = input.IsInternal;
                                updateObj.InternRelatieZichtbaar = input.IsInternal;
                                updateObj.InternBestandWijzigen = input.IsInternal;
                                updateObj.Extern = input.IsExternal;
                            }
                            else
                            {
                                //insert
                                _dbContext.LoginDossierRechts.Add(new LoginDossierRecht
                                {
                                    Guid = Guid.NewGuid().ToUpperString(),
                                    DossierGuid = dossierId,
                                    LoginGuid = input.LoginId,
                                    ModuleGuid = input.ModuleId,
                                    RolGuid = input.RoleId,
                                    ExternBestandWijzigen = input.HasExternalEditRights,
                                    ExternRelatieZichtbaar = input.IsExternalVisible,
                                    Intern = input.IsInternal,
                                    InternBestandWijzigen = input.IsInternal,
                                    InternRelatieZichtbaar = input.IsInternal,
                                    Extern = input.IsExternal
                                });
                            }
                        }
                        else
                        {
                            //toremove
                            var removeItem = allDossierLogins.Where(x => x.LoginGuid.Equals(input.LoginId)
                                 && x.RolGuid == input.RoleId && x.ModuleGuid == input.ModuleId).FirstOrDefault();
                            if (removeItem != null)
                            {
                                _dbContext.LoginDossierRechts.Remove(removeItem);
                                var dossierData = _dbContext.Dossiers.Find(dossierId);
                                if (dossierData != null)
                                {
                                    dossierData.GewijzigdOp = DateTime.Now;
                                }
                            }

                        }
                    }
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Update, Insert or delete from LoginDossierRechts based on input
        /// </summary>
        /// <param name="dossierId"></param>
        /// <param name="newUserApiModel"></param>
        /// <returns>true if operation successful</returns>
        public bool UpdateDossierUserRights(string dossierId, IEnumerable<NewDossierUserRightApiModel> newUserApiModel)
        {
            if (IsUserBuyerGuideForDossier(dossierId))
                return UpdateAddRemoveDossierUserRights(dossierId, newUserApiModel.ToList());
            else
                return false;
        }

        /// <summary>
        /// Get Image ids from Bijlage based on project id and building id
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="buildingId">optional</param>
        /// <returns>list of all GUIDs</returns>
        public IEnumerable<DossierFileModel> GetExistingFilesForProject(string projectId)
        {
            try
            {
                var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                var buildingIds = _dbContext.LoginRolWerks.Where(x => x.LoginGuid == currentUserId && x.WerkGuid == projectId).Select(x => x.GebouwGuid).Distinct();

                return _dbContext.Bijlage.Where(x => x.Bijlage1 != null && x.Publiceren == true && x.WerkGuid == projectId && (buildingIds.Any(y => y == x.GebouwGuid) || x.GebouwGuid == null))
                .Select(x => new DossierFileModel
                {
                    FileId = x.Guid,
                    Name = x.Omschrijving,
                    Extension = Path.GetExtension(x.Bijlage1),
                    LastModifiedOn = x.GewijzigdOp,
                    LastModifiedBy = x.GewijzigdDoor
                });

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Build query for filtering dossier attachments
        /// </summary>
        /// <param name="dossierId"></param>
        /// <param name="projectId"></param>
        /// <param name="buildingId"></param>
        /// <param name="isInternal"></param>
        /// <returns></returns>
        private IQueryable<DossierAttachmentModel> BuildQueryForDossierAttachment(string dossierId, string projectId, List<string> buildingIds, bool isInternal)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            var currentUser = _dbContext.Login.Find(currentUserId);

            if (buildingIds != null)
            {
                buildingIds = buildingIds.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            }
            var hasBuildingIds = buildingIds != null && buildingIds.Count() > 0;
            var availableRoles = BuildQueryForAvailableModulesRolesForUser(currentUserId, projectId, dossierId).Where(x => !hasBuildingIds || x.BuildingId == null || buildingIds.Contains(x.BuildingId));
            var isBuyerGuide = IsUserBuyerGuideForDossier(dossierId, currentUserId);
            bool isBuyer = false, isSiteManager = false, isShowroomOrSubContractor = false;
            List<string> roleIds = new List<string>();
            if (!isBuyerGuide)
            {
                isBuyer = IsUserBuyerForDossier(dossierId, currentUserId);
                if (!isBuyer)
                {
                    roleIds = availableRoles.Where(x => x.RoleName == LoginRoles.SiteManager).Select(x => x.RoleId).ToList();
                    isSiteManager = roleIds.Any();
                }
                if (!isSiteManager)
                {
                    roleIds = availableRoles.Where(x => x.RoleName == LoginRoles.SubContractor || x.RoleName == LoginRoles.Showroom).Select(x => x.RoleId).ToList();
                    isShowroomOrSubContractor = roleIds.Any();
                }
            }
            var similarRoleUsers = _dbContext.LoginDossierRechts.Include(x => x.LoginGu)
                .Where(x =>
                    x.DossierGuid == dossierId
                    && (isInternal ? x.Intern && x.InternBestandWijzigen : x.Extern && x.ExternBestandWijzigen)
                    && (
                        (isSiteManager && roleIds.Any(y => y == x.RolGuid))
                        || (isShowroomOrSubContractor && roleIds.Any(y => y == x.RolGuid) && x.LoginGu.OrganisatieGuid == currentUser.OrganisatieGuid)
                        ))
                .Select(x => x.LoginGuid);

            bool hasInternalViewRights = !isBuyer && availableRoles.Any(x => x.IsInternal);
            bool hasInternalEditRights = !isBuyer && availableRoles.Any(x => x.HasInternalEditRights);
            bool hasExternalViewRights = availableRoles.AsQueryable().Any(x => x.IsExternal);
            bool hasExternalEditRights = availableRoles.AsQueryable().Any(x => x.HasExternalEditRights);

            //22-06 -- exclude archive files if role is buyer
            return _dbContext.BijlageDossiers.Include(x => x.DossierGu).Include(x => x.BijlageGu).Include(x => x.AangemaaktDoorLoginGu).Include(x => x.DossierLoginLaatstGelezens)
                    .Where(x => x.DossierGuid == dossierId && (isInternal || x.DossierGu.Extern) && (isInternal ? hasInternalViewRights : hasExternalViewRights) && x.Intern == isInternal
                    && (hasBuildingIds ? buildingIds.Contains(x.GebouwGuid) : x.GebouwGuid == null) && (!isBuyer || !x.Gearchiveerd))
                    .Select(x => new DossierAttachmentModel
                    {
                        Guid = x.Guid,
                        BuildingId = x.GebouwGuid,
                        Name = x.BijlageGu.Omschrijving,
                        FilePath = x.BijlageGu.Bijlage1,
                        UploadedByGuid = x.AangemaaktDoorLoginGuid,
                        UploadedBy = x.AangemaaktDoorLoginGu.Naam,
                        HasUpdates = !x.DossierLoginLaatstGelezens.Any(y => y.LoginGuid == currentUserId && y.LaatstGelezen >= x.GewijzigdOp),
                        LastModifiedOn = x.GewijzigdOp,
                        LastModifiedBy = x.GewijzigdDoor,
                        HasRights = isInternal ? hasInternalViewRights && hasInternalEditRights :
                                                 hasExternalViewRights && hasExternalEditRights
                                   && (isBuyerGuide
                                        || (isBuyer && currentUser.KoperHuurderGuid == x.AangemaaktDoorLoginGu.KoperHuurderGuid)
                                        || ((isSiteManager || isShowroomOrSubContractor) && similarRoleUsers.Any(y => y == x.AangemaaktDoorLoginGuid))
                                    ),
                        IsArchived = x.Gearchiveerd,
                        IsDeleted = x.Verwijderd
                    }
                );
        }

        /// <summary>
        /// Get files for dossier and buildings
        /// </summary>
        /// <param name="dossierId"></param>
        /// <param name="projectId"></param>
        /// <param name="isInternal"></param>
        /// <param name="buildingId"></param>
        /// <param name="areAllFilesRequired">if false only uploaded files are returned else all</param>
        /// <returns></returns>
        private DossierFilesList GetFilesForDossier(string dossierId, string projectId, bool isInternal, string buildingId, bool areAllFilesRequired)
        {
            try
            {
                var filteredFiles = BuildQueryForDossierAttachment(dossierId, projectId, new List<string> { buildingId }, isInternal);

                return new DossierFilesList
                {
                    UploadedFiles = filteredFiles.Where(x => !x.IsArchived && !x.IsDeleted)
                    .Select(file =>
                    new DossierFileModel
                    {
                        FileId = file.Guid,
                        Name = file.Name,
                        Extension = Path.GetExtension(file.FilePath),
                        LastModifiedOn = file.LastModifiedOn,
                        UploadedBy = file.UploadedBy,
                        HasRights = file.HasRights,
                        LastModifiedBy = file.LastModifiedBy,
                        HasUpdates = file.HasUpdates
                    }).OrderByDescending(x => x.LastModifiedOn).ThenBy(x => x.Name).ToList(),

                    ArchivedFiles = areAllFilesRequired ? filteredFiles.Where(x => x.IsArchived && !x.IsDeleted && x.HasRights)
                    .Select(file =>
                    new DossierFileModel
                    {
                        FileId = file.Guid,
                        Name = file.Name,
                        Extension = Path.GetExtension(file.FilePath),
                        LastModifiedOn = file.LastModifiedOn,
                        UploadedBy = file.UploadedBy,
                        HasRights = file.HasRights,
                        LastModifiedBy = file.LastModifiedBy,
                        HasUpdates = file.HasUpdates
                    }).OrderByDescending(x => x.LastModifiedOn).ThenBy(x => x.Name).ToList() :
                    new List<DossierFileModel>(),

                    DeletedFiles = areAllFilesRequired ? filteredFiles.Where(x => !x.IsArchived && x.IsDeleted && x.HasRights)
                    .Select(file =>
                    new DossierFileModel
                    {
                        FileId = file.Guid,
                        Name = file.Name,
                        Extension = Path.GetExtension(file.FilePath),
                        LastModifiedOn = file.LastModifiedOn,
                        UploadedBy = file.UploadedBy,
                        HasRights = file.HasRights,
                        LastModifiedBy = file.LastModifiedBy,
                        HasUpdates = file.HasUpdates
                    }).OrderByDescending(x => x.LastModifiedOn).ThenBy(x => x.Name).ToList() :
                    new List<DossierFileModel>()
                };
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// upload backgroud image for dossier view
        /// </summary>
        /// <param name="dossierId"></param>
        /// <param name="projectId"></param>
        /// <param name="backgroundImageModel"></param>
        /// <returns></returns>
        public string UploadBackgroundImageforDossier(string dossierId, string projectId, FileModel backgroundImageModel)
        {
            if (backgroundImageModel != null && backgroundImageModel.Content != null && backgroundImageModel.Content.Length > 0)
            {
                var project = _dbContext.Werk.Find(projectId);
                var uploadLocation = project.MapBijlagen + "Dossier\\" + dossierId + "\\BackgroundImage\\";
                if (!Directory.Exists(uploadLocation))
                {
                    Directory.CreateDirectory(uploadLocation);
                }
                uploadLocation += backgroundImageModel.Name;
                File.WriteAllBytes(uploadLocation, backgroundImageModel.Content);
                return uploadLocation;
            }
            return null;
        }

        /// <summary>
        /// update buidling details of dossier
        /// </summary>
        /// <param name="dossierModel"></param>
        /// <returns></returns>
        private bool UpdateDossierbuilding(NewDossierApiModel dossierModel)
        {
            if (dossierModel.BuildingInfoList.Any())
            {
                if (IsUserBuyerGuideForDossier(dossierModel.DossierId))
                {
                    //remove all buildings for dossier
                    var buildingEntity = _dbContext.DossierGebouws.Where(x => x.DossierGuid == dossierModel.DossierId);
                    if (buildingEntity != null)
                        _dbContext.DossierGebouws.RemoveRange(buildingEntity);


                    foreach (var buildingData in dossierModel.BuildingInfoList)
                    {
                        _dbContext.DossierGebouws.Add(new DossierGebouw
                        {
                            Guid = Guid.NewGuid().ToUpperString(),
                            DossierGuid = dossierModel.DossierId,
                            GebouwGuid = buildingData.BuildingId,
                            Deadline = dossierModel.Deadline,
                            Status = dossierModel.IsDraft ? (byte)DossierStatus.Draft : (byte)DossierStatus.Open,
                            Actief = buildingData.IsActive
                        });
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dossierId"></param>
        /// <param name="buildingIds"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetDefaultNotificationTokens(string dossierId, List<string> buildingIds = null, string employeeId = null)
        {
            var dossierData = _dbContext.Dossiers.Find(dossierId);
            if (dossierData != null)
            {
                var dossier_link = _siteUrl + "dossier/" + dossierId;
                var resultDict = new Dictionary<string, string>
                {
                    ["[werknaam]"] = _dbContext.Werk.Find(dossierData.WerkGuid)?.Werknaam,
                };
                var builder_dossier_link = new StringBuilder();
                foreach (var buildingId in buildingIds)
                {
                    if (!string.IsNullOrWhiteSpace(buildingId))
                    {
                        var building_number = _dbContext.Gebouw.Find(buildingId).BouwnummerIntern;
                        var building_link = dossier_link + "?buildingId=" + buildingId;
                        builder_dossier_link.Append("<li>" + building_number + "  <a href=\"" + building_link + "\" >" + building_link + "</a></li>");
                    }
                }
                resultDict["[dossier_link]"] = dossier_link;
                resultDict["[dossier_gebouw_link]"] = "<ul>" + builder_dossier_link.ToString() + "</ul>";
                resultDict["[slotgroet]"] = string.Empty;
                resultDict["[ondertekenaar_naam]"] = string.Empty;
                resultDict["[organisatie_naamonderdeel]"] = string.Empty;
                resultDict["[organisatie_logo]"] = string.Empty;
                if (!string.IsNullOrWhiteSpace(employeeId))
                {
                    var employeeData = _dbContext.Medewerker.Where(x => x.Guid == employeeId).Select(x => new { x.Slotgroet, x.Ondertekenaar }).SingleOrDefault();
                    if (employeeData != null)
                    {
                        resultDict["[slotgroet]"] = employeeData.Slotgroet;
                        resultDict["[ondertekenaar_naam]"] = employeeData.Ondertekenaar;
                        var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                        if (!string.IsNullOrWhiteSpace(currentUserId))
                        {
                            var organisationId = _dbContext.Login.Where(x => x.Guid == currentUserId && !x.Verwijderd).Select(x => x.OrganisatieGuid).SingleOrDefault();
                            if (!string.IsNullOrWhiteSpace(organisationId))
                            {
                                var organisationName = _dbContext.Organisatie.Find(organisationId)?.NaamOnderdeel;
                                resultDict["[organisatie_naamonderdeel]"] = !string.IsNullOrWhiteSpace(organisationName) ? organisationName : string.Empty;
                                var organisationLogo = _siteUrl + "api/Organisation/GetOrganisationLogo/" + organisationId;
                                resultDict["[organisatie_logo]"] = "<a href=\"" + organisationLogo + "\" ><img width=100 style=\"margin:5px\" src =\"" + organisationLogo + " \"/></a>";
                            }
                        }
                    }
                }
                return resultDict;
            }
            return null;
        }

        /// <summary>
        /// update status of object for dossier id, if all objects are closed then close dossier.
        /// If no objects present update dossier status
        /// </summary>
        /// <param name="dossierId"></param>
        /// <param name="isClosed"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public bool UpdateDossierBuildingStatus(string dossierId, bool isClosed, string buildingId)
        {
            try
            {
                if (IsUserBuyerGuideForDossier(dossierId))
                {
                    var dossier = _dbContext.Dossiers.Include(x => x.DossierGebouws).Where(x => x.Guid == dossierId).SingleOrDefault();
                    if (dossier != null && dossier.Status != (byte)DossierStatus.Draft)
                    {
                        if (!string.IsNullOrWhiteSpace(buildingId) && buildingId != Guid.Empty.ToUpperString())
                        {

                            var isAllDossierBuildingStatusCompleted = dossier.DossierGebouws.All(x => x.GebouwGuid == buildingId || x.Status == (int)DossierStatus.Closed && x.Actief);
                            if (isAllDossierBuildingStatusCompleted && isClosed)
                            {
                                var dossierBuildingData = dossier.DossierGebouws.Where(x => x.GebouwGuid == buildingId).SingleOrDefault();
                                if (dossierBuildingData != null)
                                {
                                    dossierBuildingData.Status = (byte)DossierStatus.Closed;
                                    dossierBuildingData.AfgeslotenOp = DateTime.Now;
                                }
                                dossier.Status = (byte)DossierStatus.Closed;
                                dossier.AfgeslotenOp = DateTime.Now;
                            }
                            else if (isAllDossierBuildingStatusCompleted && !isClosed)
                            {
                                var dossierBuildingData = dossier.DossierGebouws.Where(x => x.GebouwGuid == buildingId).SingleOrDefault();
                                if (dossierBuildingData != null)
                                {
                                    dossierBuildingData.Status = (byte)DossierStatus.Open;
                                }
                                dossier.Status = (byte)DossierStatus.Open;
                            }
                            else
                            {
                                var dossierBuildingData = dossier.DossierGebouws.Where(x => x.GebouwGuid == buildingId).SingleOrDefault();
                                if (dossierBuildingData != null)
                                {
                                    dossierBuildingData.Status = isClosed ? (byte)DossierStatus.Closed : (byte)DossierStatus.Open;
                                    if (isClosed)
                                    {
                                        dossierBuildingData.AfgeslotenOp = DateTime.Now;
                                    }
                                }
                            }
                            return true;
                        }
                        else
                        {
                            dossier.Status = isClosed ? (byte)DossierStatus.Closed : (byte)DossierStatus.Open;
                            if (isClosed)
                                dossier.AfgeslotenOp = DateTime.Now;
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get Uploaded image location based on guid from bijlage_dossier, and search in bijlage
        /// </summary>
        /// <param name="dossierImageId">bijlage_dossier guid</param>
        /// <returns>path if found else empty</returns>
        public DossierFileModel GetUploadedDossierFile(string dossierFileId, bool filePathOnly = false)
        {
            try
            {
                if (!string.IsNullOrEmpty(dossierFileId))
                {
                    var dossierFile = _dbContext.BijlageDossiers.Find(dossierFileId);
                    if (dossierFile != null)
                    {
                        if (!filePathOnly)
                        {
                            var dossier = _dbContext.Dossiers.Find(dossierFile.DossierGuid);
                            if (dossier != null)
                            {
                                var filteredDossierFile = BuildQueryForDossierAttachment(dossierFile.DossierGuid, dossier.WerkGuid, new List<string> { dossierFile.GebouwGuid }, dossierFile.Intern)
                                    .Where(x => x.Guid.Equals(dossierFileId));

                                return filteredDossierFile.Select(x => new DossierFileModel
                                {
                                    FileId = x.Guid,
                                    Name = x.Name,
                                    Extension = Path.GetExtension(x.FilePath),
                                    Path = x.FilePath,
                                    LastModifiedOn = x.LastModifiedOn,
                                    UploadedBy = x.UploadedBy,
                                    HasRights = x.HasRights,
                                    LastModifiedBy = x.LastModifiedBy,
                                    HasUpdates = x.HasUpdates
                                }).FirstOrDefault();
                            }
                        }
                        else
                        {
                            return new DossierFileModel
                            {
                                FileId = dossierFile.Guid,
                                Path = _dbContext.Bijlage.Find(dossierFile.BijlageGuid)?.Bijlage1 ?? null,
                                AttachmentId = dossierFile.BijlageGuid
                            };
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// get email id for users
        /// </summary>
        /// <param name="userIdList"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetUserEmailAddress(List<string> userIdList)
        {
            Dictionary<string, string> emailWithSalutations = new Dictionary<string, string>();
            if (userIdList.Any())
            {
                foreach (var userId in userIdList.Distinct())
                {
                    var user = _dbContext.ViewLogins.Where(x => x.Guid == userId)
                        .Select(x => new
                        {
                            x.OrganisatieGuid,
                            x.PersoonGuid,
                            x.RelatieGuid,
                            x.KoperHuurderGuid,
                            x.Email
                        }).SingleOrDefault();
                    if (!string.IsNullOrWhiteSpace(user.Email))
                    {
                        var formalSalutation = _dbContext.GetSalutationForEmail(user.OrganisatieGuid, user.PersoonGuid, user.RelatieGuid, user.KoperHuurderGuid, true);
                        emailWithSalutations.Add(user.Email, formalSalutation);
                    }
                }
                return emailWithSalutations;
            }
            return null;
        }

        /// <summary>
        /// get dossier info
        /// </summary>
        /// <param name="dossierId"></param>
        /// <returns></returns>
        public DossierApiModel GetDossierInfo(string dossierId)
        {
            var dossier = _dbContext.Dossiers.Find(dossierId);
            return new DossierApiModel
            {
                Id = dossier.Guid,
                ProjectId = dossier.WerkGuid,
                BackgroundImagePath = dossier.Afbeelding,
                Name = dossier.Naam
            };
        }

        /// <summary>
        /// move file between internal/external or archieve/deleted
        /// </summary>
        /// <param name="dossierFileApiModel"></param>
        /// <returns>true if move successful</returns>
        public DossierResponseTypes MoveDossierBuildingsFiles(DossierFileApiModel dossierFileApiModel)
        {
            try
            {
                if (dossierFileApiModel.IsArchived && dossierFileApiModel.IsDeleted)
                {
                    return DossierResponseTypes.UpdateFailed;
                }

                var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                var isUserBuyerGuide = IsUserBuyerGuideForDossier(dossierFileApiModel.DossierId, currentUserId);
                if (dossierFileApiModel.IsArchived && !isUserBuyerGuide)
                    return DossierResponseTypes.Unauthorized;

                if (HasEditRightsToSection(dossierFileApiModel.DossierId, null, dossierFileApiModel.BuildingId, dossierFileApiModel.IsInternal))
                {
                    var getExistingSectionType = _dbContext.BijlageDossiers.Find(dossierFileApiModel.DossierFileList[0].DossierFileId)?.Intern;
                    if (getExistingSectionType.HasValue)
                    {
                        var buildingId = _dbContext.BijlageDossiers.Where(x => dossierFileApiModel.DossierFileList.Select(y => y.DossierFileId).Contains(x.Guid))
                            .Select(x => x.GebouwGuid).Distinct().ToList();
                        //22-06 -- prevent file moving between dossier and building and between buildings
                        if (buildingId.Count == 1 && buildingId.First() == dossierFileApiModel.BuildingId)
                        {
                            var filteredFiles = BuildQueryForDossierAttachment(dossierFileApiModel.DossierId, null, new List<string> { buildingId[0] }, getExistingSectionType.Value)
                            .Where(x => x.HasRights && dossierFileApiModel.DossierFileList.Select(y => y.DossierFileId).Contains(x.Guid)).Distinct()
                            .Select(x => new { x.Guid, x.IsArchived }).ToList();

                            if (filteredFiles.Any(x => x.IsArchived) && !isUserBuyerGuide)
                                return DossierResponseTypes.Unauthorized;

                            if (dossierFileApiModel.DossierFileList.Select(x => x.DossierFileId).Distinct().Count().Equals(filteredFiles.Count))
                            {
                                foreach (var file in filteredFiles)
                                {
                                    var dossierFile = _dbContext.BijlageDossiers.Find(file.Guid);
                                    //15-06 -- change move files from any section to any section
                                    dossierFile.Intern = dossierFileApiModel.IsInternal;
                                    dossierFile.Gearchiveerd = dossierFileApiModel.IsArchived;
                                    dossierFile.Verwijderd = dossierFileApiModel.IsDeleted;
                                }
                                return DossierResponseTypes.Success;
                            }
                            else
                                return DossierResponseTypes.Unauthorized;
                        }
                        else
                            return DossierResponseTypes.UpdateFailed;
                    }
                    else
                        return DossierResponseTypes.UpdateFailed;

                }
                else
                    return DossierResponseTypes.Unauthorized;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Add/update/delete buildings based on building and dossier id
        /// </summary>
        /// <param name="dossiersBuildingUpdateModels"></param>
        /// <returns></returns>
        public DossierResponseTypes UpdateDossierBuildings(string dossierId, List<DossiersBuildingUpdateModel> dossiersBuildingUpdateModels)
        {
            try
            {
                if (_dbContext.Dossiers.Any(x => x.Guid.Equals(dossierId) && x.Status.Equals((int)DossierStatus.Open)))
                {
                    if (IsUserBuyerGuideForDossier(dossierId))
                    {
                        var allDossierBuildings = _dbContext.DossierGebouws.Where(x => dossiersBuildingUpdateModels.Select(x => x.BuildingId).Contains(x.GebouwGuid) && x.DossierGuid.Equals(dossierId));

                        foreach (var updateItem in allDossierBuildings)
                        {
                            var inputItem = dossiersBuildingUpdateModels.Where(x => x.BuildingId.Equals(updateItem.GebouwGuid)).SingleOrDefault();
                            updateItem.Actief = inputItem.IsActive;
                            updateItem.Status = (int)DossierStatus.Open;

                        }

                        var toInsert = dossiersBuildingUpdateModels.Select(x => x.BuildingId).Except(allDossierBuildings.Select(x => x.GebouwGuid));
                        foreach (var insertItem in toInsert)
                        {
                            var inputItem = dossiersBuildingUpdateModels.Where(x => x.BuildingId.Equals(insertItem)).SingleOrDefault();
                            _dbContext.DossierGebouws.Add(new DossierGebouw
                            {
                                Guid = Guid.NewGuid().ToUpperString(),
                                DossierGuid = dossierId,
                                GebouwGuid = inputItem.BuildingId,
                                Actief = inputItem.IsActive,
                                Status = (int)DossierStatus.Open
                            });
                        }
                        return DossierResponseTypes.Success;
                    }
                    else
                        return DossierResponseTypes.Unauthorized;
                }
                else
                    return DossierResponseTypes.UpdateFailed;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// get modules and roles info for user guid
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IQueryable<DossierUserRightsModel> BuildQueryForAvailableModulesRolesForUser(string userId, string projectId = null, string dossierId = null)
        {
            var loginData = _dbContext.ViewLogins.Where(x => x.Guid == userId && x.Actief == true)
                .Select(x => new { x.CentralGuid, x.LoginAccountVoor })
                .SingleOrDefault();

            if (loginData != null)
            {
                if (loginData.LoginAccountVoor == (byte)AccountType.Buyer)
                {
                    return _dbContext.LoginRolWerks.Include(x => x.LoginGu).Join(_dbContext.ViewModuleRoles,
                            x => new { x.ModuleGuid, RoleGuid = x.RolGuid },
                            y => new { y.ModuleGuid, y.RoleGuid },
                            (x, y) => new
                            {
                                x.LoginGuid,
                                x.LoginGu.KoperHuurderGuid,
                                x.WerkGuid,
                                x.GebouwGuid,
                                x.RolGuid,
                                x.ModuleGuid,
                                y.ModuleName,
                                y.RoleName,
                                y.Active,
                                x.Actief
                            })
                            .Where(x => x.LoginGuid == userId && x.Active == true && x.Actief == true && x.ModuleName == LoginModules.BuyerGuide && x.RoleName == LoginRoles.BuyerOrRenter && (projectId == null || x.WerkGuid == projectId))
                            .Join(_dbContext.DossierGebouws.Include(x => x.DossierGu).Where(x => x.Actief && x.DossierGu.Extern),
                            x => new { x.GebouwGuid },
                            y => new { y.GebouwGuid },
                            (x, y) => new DossierUserRightsModel
                            {
                                DossierId = y.DossierGuid,
                                ModuleId = x.ModuleGuid,
                                ModuleName = x.ModuleName,
                                RoleId = x.RolGuid,
                                RoleName = x.RoleName,
                                ProjectId = x.WerkGuid,
                                BuildingId = x.GebouwGuid,
                                IsExternalVisible = true,
                                HasExternalEditRights = true,
                                IsExternal = true
                            })
                            .Where(x => (dossierId == null || x.DossierId == dossierId) && (dossierId != null || projectId != null))
                            .Distinct();
                }
                else
                {
                    return _dbContext.LoginRolWerks.Join(_dbContext.ViewModuleRoles,
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
                            .Where(x => x.LoginGuid == userId && x.Active == true && x.Actief == true && x.ModuleName == LoginModules.BuyerGuide && (projectId == null || x.WerkGuid == projectId))
                            .Join(_dbContext.LoginDossierRechts.Include(x => x.DossierGu),
                            x => new { x.LoginGuid, x.WerkGuid, x.ModuleGuid, x.RolGuid },
                            y => new { y.LoginGuid, y.DossierGu.WerkGuid, y.ModuleGuid, y.RolGuid },
                            (x, y) => new DossierUserRightsModel
                            {
                                DossierId = y.DossierGuid,
                                ModuleId = x.ModuleGuid,
                                ModuleName = x.ModuleName,
                                RoleId = x.RolGuid,
                                RoleName = x.RoleName,
                                ProjectId = x.WerkGuid,
                                BuildingId = x.GebouwGuid,
                                HasExternalEditRights = y.ExternBestandWijzigen,
                                IsExternalVisible = y.ExternRelatieZichtbaar,
                                IsInternal = y.Intern,
                                IsExternal = y.Extern,
                                HasInternalEditRights = y.InternBestandWijzigen,
                                IsInternalVisible = y.InternRelatieZichtbaar
                            })
                            .Where(x => (dossierId == null || x.DossierId == dossierId) && (dossierId != null || projectId != null))
                            .Distinct()
                            .Union(_dbContext.LoginRolWerks.Join(_dbContext.ViewModuleRoles,
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
                            .Where(x => x.LoginGuid == userId && x.Active == true && x.Actief == true && x.ModuleName == LoginModules.BuyerGuide && x.RoleName == LoginRoles.Spectator && (projectId == null || x.WerkGuid == projectId))
                            .Join(_dbContext.Dossiers,
                            x => new { x.WerkGuid },
                            y => new { y.WerkGuid },
                            (x, y) => new DossierUserRightsModel
                            {
                                DossierId = y.Guid,
                                ModuleId = x.ModuleGuid,
                                ModuleName = x.ModuleName,
                                RoleId = x.RolGuid,
                                RoleName = x.RoleName,
                                ProjectId = x.WerkGuid,
                                BuildingId = x.GebouwGuid,
                                HasExternalEditRights = false,
                                IsExternalVisible = false,
                                IsInternal = true,
                                IsExternal = true,
                                HasInternalEditRights = false,
                                IsInternalVisible = false
                            })
                            .Where(x => (dossierId == null || x.DossierId == dossierId) && (dossierId != null || projectId != null))
                            .Distinct());
                }
            }
            return (new List<DossierUserRightsModel>()).AsQueryable();
        }

        public bool HasRolesForProject(string projectId, string userId, List<string> roleNames)
        {
            try
            {
                userId = string.IsNullOrEmpty(userId) ? _httpContextAccessor.HttpContext?.User?.Identity?.Name : userId;
                var loginData = _dbContext.ViewLogins.Where(x => x.Guid == userId && x.Actief == true)
                .Select(x => new { x.CentralGuid, x.LoginAccountVoor })
                .SingleOrDefault();

                if (loginData != null)
                {
                    return _dbContext.LoginRolWerks.Join(_dbContext.ViewModuleRoles,
                            x => new { x.ModuleGuid, RoleGuid = x.RolGuid },
                            y => new { y.ModuleGuid, y.RoleGuid },
                            (x, y) => new
                            {
                                x.LoginGuid,
                                x.WerkGuid,
                                y.ModuleName,
                                y.RoleName,
                                y.Active,
                                x.Actief
                            }).Any(x => x.LoginGuid == userId && x.Active == true && x.Actief == true
                            && x.ModuleName == LoginModules.BuyerGuide && x.WerkGuid == projectId
                            && roleNames.Any(y => y == x.RoleName));
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool IsUserModuleBuyerGuide(string projectId, string userId)
        {
            try
            {
                userId = string.IsNullOrEmpty(userId) ? _httpContextAccessor.HttpContext?.User?.Identity?.Name : userId;
                return BuildQueryForAvailableModulesRolesForUser(userId, projectId).Any();
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// check if user has buyer guide role for dossier
        /// </summary>
        /// <param name="dossierId">dossier GUID</param>
        /// <returns></returns>
        public bool IsUserBuyerGuideForDossier(string dossierId, string userId = null)
        {
            try
            {
                userId = string.IsNullOrEmpty(userId) ? _httpContextAccessor.HttpContext?.User?.Identity?.Name : userId;
                return BuildQueryForAvailableModulesRolesForUser(userId, null, dossierId).Select(x => x.RoleName).Any(x => x == LoginRoles.BuyersGuide);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// check if user has buyer guide role for dossier
        /// </summary>
        /// <param name="dossierId">dossier GUID</param>
        /// <returns></returns>
        private bool IsUserBuyerForDossier(string dossierId, string userId = null)
        {
            try
            {
                userId = string.IsNullOrEmpty(userId) ? _httpContextAccessor.HttpContext?.User?.Identity?.Name : userId;
                return BuildQueryForAvailableModulesRolesForUser(userId, null, dossierId).Select(x => x.RoleName).Any(x => x == LoginRoles.BuyerOrRenter);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// update backgroud image for dossier
        /// </summary>
        /// <param name="dossierId"></param>
        /// <param name="backgroundImagePath"></param>
        /// <returns></returns>
        public bool UpdateBackgroundImage(string dossierId, string backgroundImagePath)
        {
            if (!string.IsNullOrWhiteSpace(dossierId))
            {
                var dossierData = _dbContext.Dossiers.Find(dossierId);
                if (dossierData != null)
                {
                    dossierData.Afbeelding = backgroundImagePath;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// check if building visibility is true for buyer role
        /// </summary>
        /// <param name="dossierId"></param>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        public bool BuildingIsVisibleToBuyer(string dossierId, string buildingId)
        {
            return _dbContext.DossierGebouws.Include(x => x.DossierGu).Any(x => x.DossierGuid == dossierId && x.GebouwGuid == buildingId && x.DossierGu.Extern);
        }

        /// <summary>
        /// get all buildings for project id and dossiers related to each building
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IEnumerable<BuildingOverviewModel> GetBuildingListWithDossiers(string projectId)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            var availableModuleWithRoles = BuildQueryForAvailableModulesRolesForUser(currentUserId, projectId);//.ToList();
            var filteredDossiers = BuildQueryForDossiers(projectId)
                .Where(x => (x.Status == (int)DossierStatus.Closed || x.Status == (int)DossierStatus.Open) && !x.Gearchiveerd);

            var isBuyer = availableModuleWithRoles.Any(x => x.RoleName.Equals(LoginRoles.BuyerOrRenter));
            var buildings = availableModuleWithRoles.Select(x => x.BuildingId).Distinct();

            var dossiersForBuildings = _dbContext.DossierGebouws.Include(x => x.DossierGu).ThenInclude(x => x.DossierLoginLaatstGelezens).Include(x => x.DossierGu.DossierVolgordes)
                .Where(y => filteredDossiers.Select(x => x.Guid).Contains(y.DossierGuid) && y.Actief && (buildings.Any(z => z == null) || buildings.Contains(y.GebouwGuid)) && (!isBuyer || y.DossierGu.Extern))
                .Select(x => new
                {
                    x.GebouwGuid,
                    x.DossierGuid,
                    x.DossierGu.WerkGuid,
                    x.Deadline,
                    x.DossierGu.Naam,
                    x.Status,
                    x.DossierGu.Extern,
                    x.DossierGu.AlgemeneBestanden,
                    x.DossierGu.ObjectgebondenBestanden,
                    HasUpdates = !x.DossierGu.DossierLoginLaatstGelezens.Any(y => y.LoginGuid == currentUserId && y.GebouwGuid == x.GebouwGuid && y.LaatstGelezen >= x.GewijzigdOp),
                    Order = x.DossierGu.DossierVolgordes.Any() ? x.DossierGu.DossierVolgordes.Min(x => x.Volgorde) : 0
                })
                .Distinct()
                .ToList()
                .GroupBy(x => x.GebouwGuid)
            .Select(x => new
            {
                BuildingId = x.Key,
                DossierList = x.Select(y => new
                {
                    Id = y.DossierGuid,
                    ProjectId = y.WerkGuid,
                    Deadline = y.Deadline,
                    Name = y.Naam,
                    Status = (DossierStatus)y.Status,
                    IsExternal = y.Extern,
                    HasGeneralFiles = y.AlgemeneBestanden,
                    HasObjectBoundFiles = y.ObjectgebondenBestanden,
                    y.HasUpdates,
                    y.Order
                })
                .OrderBy(x => x.Order)
                .ThenBy(x => x.Deadline)
            }).ToList();

            var result = new List<BuildingOverviewModel>();

            foreach (var building in dossiersForBuildings)
            {
                var buildingToAdd = new BuildingOverviewModel
                {
                    BuildingId = building.BuildingId
                };

                var dossierList = new List<DossierOverviewModel>();
                foreach (var buildingDossier in building.DossierList)
                {
                    var hasUpdates = buildingDossier.HasUpdates;
                    if (!hasUpdates && buildingDossier.HasGeneralFiles)
                    {
                        if (!isBuyer)
                        {
                            hasUpdates = BuildQueryForDossierAttachment(buildingDossier.Id, buildingDossier.ProjectId, null, true)
                                        .Any(x => x.HasUpdates && (x.HasRights || (!x.IsArchived && !x.IsDeleted)));
                        }
                        if (buildingDossier.IsExternal && !buildingDossier.HasUpdates)
                        {
                            hasUpdates = BuildQueryForDossierAttachment(buildingDossier.Id, buildingDossier.ProjectId, null, false)
                                .Any(x => x.HasUpdates && (x.HasRights || (!x.IsArchived && !x.IsDeleted)));
                        }
                    }
                    if (!hasUpdates && buildingDossier.HasObjectBoundFiles)
                    {
                        if (!isBuyer)
                        {
                            hasUpdates = BuildQueryForDossierAttachment(buildingDossier.Id, buildingDossier.ProjectId, new List<string> { building.BuildingId }, true)
                                    .Any(x => x.HasUpdates && (x.HasRights || (!x.IsArchived && !x.IsDeleted)));
                        }
                        if (buildingDossier.IsExternal && !buildingDossier.HasUpdates)
                        {
                            hasUpdates = BuildQueryForDossierAttachment(buildingDossier.Id, buildingDossier.ProjectId, new List<string> { building.BuildingId }, false)
                                .Any(x => x.HasUpdates && (x.HasRights || (!x.IsArchived && !x.IsDeleted)));
                        }
                    }
                    dossierList.Add(new DossierOverviewModel
                    {
                        Id = buildingDossier.Id,
                        Deadline = buildingDossier.Deadline,
                        Name = buildingDossier.Name,
                        Status = buildingDossier.Status,
                        IsExternal = buildingDossier.IsExternal,
                        HasUpdates = hasUpdates
                    });
                }

                buildingToAdd.DossierList = dossierList;
                result.Add(buildingToAdd);
            }

            return result;
        }

        public DossierUserInfoModel GetUsersByDossierId(string dossierId, string buildingId = null)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            var availableModuleWithRoles = BuildQueryForAvailableModulesRolesForUser(currentUserId, null, dossierId);
            var buildings = availableModuleWithRoles.Select(x => x.BuildingId);
            var isBuyer = availableModuleWithRoles.Any(x => x.RoleName.Equals(LoginRoles.BuyerOrRenter));

            var dossierUserData = BuildQueryForDossiers(null, buildingId, dossierId).Select(x => new DossierUserInfoModel
            {
                DossierId = x.Guid,
                BuyerContactInfo = _dbContext.DossierGebouws.Include(x => x.GebouwGu)
                                                .ThenInclude(x => x.KoperHuurderGu)
                                                .Include(y => y.GebouwGu.KoperHuurderGu.Login)
                                                .Include(y => y.GebouwGu.KoperHuurderGu.Persoon1Gu)
                                                .Include(y => y.GebouwGu.KoperHuurderGu.Persoon2Gu)
                                                .Include(y => y.GebouwGu.KoperHuurderGu.OrganisatieGu)
                                                .Include(y => y.GebouwGu.KoperHuurderGu.RelatieGu)
                                                .Include(y => y.GebouwGu.KoperHuurderGu.RelatieGu.PersoonGu)
                                                .Include(y => y.GebouwGu.KoperHuurderGu.RelatieGu.FunctieGu)
                                                .Include(y => y.GebouwGu.KoperHuurderGu.RelatieGu.AfdelingGu)
                                                .Where(y => y.DossierGuid == dossierId && (buildingId == null || buildingId == y.GebouwGuid) && y.Actief && (buildings.Any(z => z == null) || buildings.Contains(y.GebouwGuid)) && (!isBuyer || x.Extern))
                                                .Select(y => buildingId != null && !string.IsNullOrWhiteSpace(y.GebouwGu.KoperHuurderGuid) && x.Extern ?
                                                new BuyersInfoApiModel(y.GebouwGu.KoperHuurderGu) : null
                                                ).ToList(),
            }).FirstOrDefault();
            if (dossierUserData != null)
            {
                dossierUserData.UsersList = BuildQueryForDossierUsersWithRights(dossierId, null, buildingId).ToList()
                    .Select(x => new DossierUserApiModel
                    {
                        DossierRightId = x.DossierRightId,
                        Email = _dbContext.ViewLogins.Where(y => y.Guid == x.LoginId).Select(x => x.Email).SingleOrDefault(),
                        RoleId = x.RoleId,
                        RoleName = x.RoleName,
                        LoginId = x.LoginId,
                        Name = x.Name
                    }).ToList();
                return dossierUserData;
            }

            return null;
        }

        public string CheckDuplicateDossierFile(string originalFileName, DossierFileApiModel fileModel)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
            var fileExtension = Path.GetExtension(originalFileName);
            var dossierData = _dbContext.Dossiers.Where(x => x.Guid == fileModel.DossierId)
                                 .Include(x => x.BijlageDossiers).SingleOrDefault();

            if (dossierData != null)
            {
                var fileData = _dbContext.BijlageDossiers.Include(x => x.BijlageGu)
                                      .Where(x => x.DossierGuid == fileModel.DossierId
                                       && x.BijlageGu.Omschrijving == fileNameWithoutExtension
                                       && x.BijlageGu.Publiceren
                                       && !x.Verwijderd
                                       && !x.Gearchiveerd
                                       && x.BijlageGu.WerkGuid == dossierData.WerkGuid
                                       && x.BijlageGu.GebouwGuid == null
                                       && x.BijlageGu.KoppelenAan == (int)AttachmentLinkedTo.Dossier
                                       && x.BijlageGu.Bijlage1 != null
                                       && x.Intern == fileModel.IsInternal)
                                      .Select(x => new { x.BijlageGuid, x.BijlageGu.Bijlage1 })
                                      .FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(fileModel.BuildingId))
                {
                    fileData = _dbContext.BijlageDossiers.Include(x => x.BijlageGu)
                                          .Where(x => x.DossierGuid == fileModel.DossierId
                                           && x.BijlageGu.Omschrijving == fileNameWithoutExtension
                                           && x.BijlageGu.Publiceren
                                           && !x.Verwijderd
                                           && !x.Gearchiveerd
                                           && x.BijlageGu.WerkGuid == dossierData.WerkGuid
                                           && x.BijlageGu.GebouwGuid == fileModel.BuildingId
                                           && x.BijlageGu.KoppelenAan == (int)AttachmentLinkedTo.Dossier
                                           && x.BijlageGu.Bijlage1 != null
                                           && x.Intern == fileModel.IsInternal)
                                          .Select(x => new { x.BijlageGuid, x.BijlageGu.Bijlage1 })
                                          .FirstOrDefault();
                }

                if (fileData != null && !string.IsNullOrWhiteSpace(fileData.BijlageGuid) && Path.GetExtension(fileData.Bijlage1) == fileExtension)
                {
                    var dossierFileData = dossierData.BijlageDossiers.Where(x => x.BijlageGuid == fileData.BijlageGuid && x.GebouwGuid == null)
                                                                   .FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(fileModel.BuildingId))
                    {
                        dossierFileData = dossierData.BijlageDossiers.Where(x => x.BijlageGuid == fileData.BijlageGuid && x.GebouwGuid == fileModel.BuildingId)
                                                                   .FirstOrDefault();
                    }
                    if (dossierFileData != null)
                    {
                        dossierFileData.Gearchiveerd = true;
                        return dossierFileData.Guid;
                    }
                }
            }
            return null;
        }

        public bool UpdateDossierDeadline(string dossierId, DateTime? deadlineDate, bool isUpdateBuildings)
        {
            var dossierData = _dbContext.Dossiers.Where(x => x.Guid == dossierId).Include(x => x.DossierGebouws).SingleOrDefault();
            if (dossierData != null && (dossierData.Status == (int)DossierStatus.Open))
            {
                if (IsUserBuyerGuideForDossier(dossierId))
                {
                    dossierData.Deadline = deadlineDate ?? null;
                    if (isUpdateBuildings)
                    {
                        foreach (var building in dossierData.DossierGebouws)
                        {
                            building.Deadline = deadlineDate ?? null;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public string UpdateDossierLastView(string dossierId, DossierLastViewModel viewModel)
        {
            var dossierData = _dbContext.Dossiers.Find(dossierId);
            if (dossierData != null)
            {
                var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                var existingDossierView = _dbContext.DossierLoginLaatstGelezens.Where(x => x.LoginGuid == currentUserId
                                             && x.DossierGuid == dossierId && x.GebouwGuid == viewModel.BuildingId
                                             && x.BijlageDossierGuid == viewModel.DossierFileId).OrderBy(x => x.LaatstGelezen).FirstOrDefault();
                if (existingDossierView != null)
                {
                    existingDossierView.LaatstGelezen = viewModel.LastViewDate ?? DateTime.Now;
                    return existingDossierView.Guid;
                }
                else
                {
                    var dossierView = new DossierLoginLaatstGelezen
                    {
                        Guid = Guid.NewGuid().ToUpperString(),
                        DossierGuid = dossierId,
                        GebouwGuid = viewModel.BuildingId,
                        BijlageDossierGuid = viewModel.DossierFileId,
                        LaatstGelezen = viewModel.LastViewDate ?? DateTime.Now,
                        LoginGuid = currentUserId
                    };
                    _dbContext.DossierLoginLaatstGelezens.Add(dossierView);
                    return dossierView.Guid;
                }
            }
            return null;
        }

        /// <summary>
        /// get Org info for dossier user list
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="relationId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        private OrgInfoApiModel GetUserContactInfo(string organizationId, string relationId, string projectId)
        {
            try
            {
                var org = _dbContext.Organisatie.Find(organizationId);
                if (org != null)
                {
                    var relation = _dbContext.Relatie.Where(x => x.Guid.Equals(relationId)).Include(x => x.PersoonGu).Include(x => x.AfdelingGu).Select(x => new { x.Guid, x.PersoonGu.Geslacht, x.PersoonGu.Naam, x.PersoonGuid, x.AfdelingGu.Afdeling1 }).SingleOrDefault();
                    var projectRelation = _dbContext.BetrokkeneRelatie.Include(x => x.FunctieGu).Where(x => x.RelatieGuid.Equals(relation.Guid) && x.BetrokkeneGu.WerkGuid.Equals(projectId)).Select(x => new { x.Doorkiesnummer, x.Mobiel, x.EmailZakelijk, x.FunctieGu.Functie1 }).FirstOrDefault();
                    return new OrgInfoApiModel()
                    {
                        OrganisatonId = org.Guid,
                        Name = org.NaamOnderdeel ?? string.Empty,
                        Email = org.Email,
                        Telephone = org.Telefoon,
                        RelationId = relation.Guid,
                        RelationName = relation.Naam == "N.v.t." ? relation.Afdeling1 : relation.Naam ?? string.Empty,
                        RelationTelephone = projectRelation?.Doorkiesnummer ?? string.Empty,
                        RelationMobile = projectRelation?.Mobiel ?? string.Empty,
                        RelationEmail = projectRelation?.EmailZakelijk ?? string.Empty,
                        RelationPersonId = relation.PersoonGuid,
                        RelationFunctionName = projectRelation?.Functie1 ?? string.Empty,
                        RelationPersonSex = relation.Geslacht
                    };
                }
                return new OrgInfoApiModel();
            }
            catch
            {
                return null;
            }
        }

        public void OrderDossier(string dossierId, string previousDossierId)
        {
            short order = 10;
            var dossierData = _dbContext.Dossiers.Find(dossierId);
            if (dossierData != null)
            {
                if (IsUserBuyerGuideForDossier(dossierId))
                {
                    var projectId = dossierData.WerkGuid;
                    var dossierOrder = _dbContext.DossierVolgordes.Where(x => x.WerkGuid == projectId).OrderBy(x => x.Volgorde).ToList();
                    if (dossierOrder.Any())
                    {
                        if (!string.IsNullOrWhiteSpace(previousDossierId))
                        {
                            var previousDossierData = _dbContext.Dossiers.Find(previousDossierId);
                            if (previousDossierData != null && previousDossierData.WerkGuid == projectId)
                            {
                                var previousOrder = dossierOrder.Where(x => x.DossierGuid == previousDossierId).SingleOrDefault();
                                // Set mid order value to in the range of 10,20....
                                short previousOrderValue = previousOrder.Volgorde;
                                if (previousOrderValue % 10 != 0)
                                {
                                    previousOrderValue /= 10;
                                    previousOrderValue = (short)((previousOrderValue * 10) + 10);
                                    previousOrder.Volgorde = previousOrderValue;
                                }
                                order = (short)(previousOrderValue + 10);
                            }
                            else
                            {
                                throw new Exception(string.Format("Different project id for Dossier Ids: {0} and {1}", dossierId, previousDossierId));
                            }
                        }
                        ReorderDossiers(dossierId, previousDossierId, dossierOrder, order);
                    }
                    else
                    {
                        var dossiers = _dbContext.Dossiers.Where(x => x.Status != (int)DossierStatus.Draft && x.WerkGuid == projectId && !x.Gearchiveerd)
                            .OrderBy(x => x.Deadline).ToList();
                        int index = -1;
                        dossiers = dossiers.Where(x => x.Guid != dossierId).ToList();
                        if (!string.IsNullOrWhiteSpace(previousDossierId))
                        {
                            var previousDossierData = _dbContext.Dossiers.Find(previousDossierId);
                            if (previousDossierData != null && previousDossierData.WerkGuid == projectId)
                            {
                                index = dossiers.FindIndex(x => x.Guid == previousDossierId);
                                if (index != -1)
                                {
                                    var filterDossiers = dossiers.GetRange(0, index + 1);
                                    foreach (var dossier in filterDossiers)
                                    {
                                        AddDossierOrder(dossier.Guid, dossier.WerkGuid, order);
                                        order += 10;
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception(string.Format("Different project id for Dossier Ids: {0} and {1}", dossierId, previousDossierId));
                            }
                        }
                        // add current dossier order
                        AddDossierOrder(dossierId, projectId, order);
                        index++;
                        var dossierOrders = dossiers.GetRange(index, dossiers.Count - index);
                        foreach (var dossier in dossierOrders)
                        {
                            order += 10;
                            AddDossierOrder(dossier.Guid, dossier.WerkGuid, order);
                        }
                        dossiers = _dbContext.Dossiers.Where(x => x.Status != (int)DossierStatus.Draft && x.WerkGuid == projectId && x.Gearchiveerd)
                            .OrderBy(x => x.Deadline).ToList();
                        foreach (var dossier in dossiers)
                        {
                            order += 10;
                            AddDossierOrder(dossier.Guid, dossier.WerkGuid, order);
                        }
                    }
                }
            }
        }

        private void AddDossierOrder(string dossierId, string projectId, short order)
        {
            _dbContext.DossierVolgordes.Add(new DossierVolgorde
            {
                Guid = Guid.NewGuid().ToUpperString(),
                DossierGuid = dossierId,
                WerkGuid = projectId,
                Volgorde = order
            });
        }

        private void ReorderDossiers(string dossierId, string previousDossierId, List<DossierVolgorde> dossierOrdersList, short order)
        {
            if (!string.IsNullOrWhiteSpace(dossierId))
            {
                var dossierOrder = dossierOrdersList.Where(x => x.DossierGuid == dossierId).SingleOrDefault();
                int previousIndex = dossierOrdersList.FindIndex(x => x.DossierGuid == previousDossierId);
                int index = dossierOrdersList.FindIndex(x => x.DossierGuid == dossierId);
                if (previousIndex != -1 && index != -1)
                {
                    if (index > previousIndex)
                    {
                        dossierOrdersList = dossierOrdersList.Where(x => x.DossierGuid != dossierId).ToList();
                        previousIndex++;
                        dossierOrdersList = dossierOrdersList.GetRange(previousIndex, dossierOrdersList.Count - previousIndex);
                        if (dossierOrder != null)
                        {
                            dossierOrder.Volgorde = order;
                        }
                    }
                    else
                    {
                        var orderValue = dossierOrdersList.Where(x => x.DossierGuid == dossierId).Select(x => x.Volgorde)
                            .SingleOrDefault();
                        index++;
                        previousIndex++;
                        dossierOrdersList = dossierOrdersList.GetRange(index, (previousIndex - index));
                        if (dossierOrder != null)
                        {
                            dossierOrder.Volgorde = (short)(order - 10);
                        }
                        order = (short)(orderValue - 10);
                    }
                }
                else
                {
                    if (dossierOrder != null)
                    {
                        dossierOrder.Volgorde = order;
                    }
                    dossierOrdersList = dossierOrdersList.Where(x => x.DossierGuid != dossierId).ToList();
                }
            }
            foreach (var item in dossierOrdersList)
            {
                order += 10;
                item.Volgorde = order;
            }
        }

        public void MarkWholeDossierAsViewed(string dossierId)
        {
            var dossierData = _dbContext.Dossiers.Find(dossierId);
            if (dossierData != null)
            {
                var lastViewData = _dbContext.DossierLoginLaatstGelezens.Where(x => x.DossierGuid == dossierId);
                foreach (var item in lastViewData)
                {
                    item.LaatstGelezen = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// create zip for all selected dossier files
        /// </summary>
        /// <param name="downloadDossierFiles">model for creating folder structure inside zip and which general and object files to add in zip</param>
        /// <returns>zip file name</returns>
        public string CreateZipForDownload(DownloadFilesModel downloadDossierFiles)
        {
            ZipArchive zip = null;
            try
            {
                string zipFileName = string.Empty;

                bool isExternArchivedFilesReq, isExternDeletedFilesReq, isInternDeletedFilesReq, isInternArchivedFilesReq;

                Dictionary<string, int> dossierNames = new Dictionary<string, int>();
                if (downloadDossierFiles.DossierDownloadFiles.Any())
                {
                    var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

                    if (!Directory.Exists("Temp"))
                        Directory.CreateDirectory("Temp");

                    zipFileName = Path.Combine("Temp", "dossierdl-" + DateTime.Now.ToString("ddMMyyyy-HHmmssfff") + ".zip");
                    zip = ZipFile.Open(zipFileName, ZipArchiveMode.Create);
                    if (downloadDossierFiles.IsDossierFolderFormat)
                    {
                        foreach (var dossierId in downloadDossierFiles.DossierDownloadFiles.Select(x => x.DossierId).Distinct())
                        {
                            var isBuyerGuide = IsUserBuyerGuideForDossier(dossierId, currentUserId);

                            //fetch archive and deleted files only for buyerguide
                            isExternArchivedFilesReq = isBuyerGuide && downloadDossierFiles.IsExternArchivedFilesReq;
                            isExternDeletedFilesReq = isBuyerGuide && downloadDossierFiles.IsExternDeletedFilesReq;
                            isInternDeletedFilesReq = isBuyerGuide && downloadDossierFiles.IsInternDeletedFilesReq;
                            isInternArchivedFilesReq = isBuyerGuide && downloadDossierFiles.IsInternArchivedFilesReq;

                            var dossier = _dbContext.Dossiers.Find(dossierId);
                            string dossierName = dossier.Naam;
                            if (dossierNames.ContainsKey(dossierName.ToLower()))
                            {
                                dossierNames[dossierName.ToLower()] = ++dossierNames[dossierName.ToLower()];
                                dossierName += " (" + dossierNames[dossierName.ToLower()] + ")";
                            }
                            else
                                dossierNames.Add(dossierName.ToLower(), 0);

                            if (dossier.AlgemeneBestanden)
                            {
                                //get general files
                                var genFiles = GetFilesForDownload(dossierId).Where(x => (dossier.Extern || x.IsInternal) && ((!x.IsArchived && !x.IsDeleted) || isBuyerGuide)
                                 && ((isExternArchivedFilesReq && !x.IsInternal) || (isInternArchivedFilesReq && x.IsInternal) || !x.IsArchived)
                                 && ((isExternDeletedFilesReq && !x.IsInternal) || (isInternDeletedFilesReq && x.IsInternal) || !x.IsDeleted)).Select(x => x.FilePath).Distinct().ToList();

                                foreach (var file in genFiles)
                                {
                                    zip.CreateEntryFromFile(file, dossierName + "/" + Path.GetFileName(file));
                                }

                            }
                            if (dossier.ObjectgebondenBestanden)
                            {
                                foreach (var buildingId in downloadDossierFiles.DossierDownloadFiles.Where(x => x.DossierId == dossierId).SelectMany(x => x.BuildingIds))
                                {
                                    string buildingNumber = _dbContext.Gebouw.Find(buildingId).BouwnummerExtern;
                                    //obj files
                                    var objFiles = GetFilesForDownload(dossierId, buildingId).Where(x => (dossier.Extern || x.IsInternal) && ((!x.IsArchived && !x.IsDeleted) || isBuyerGuide)
                                    && ((isExternArchivedFilesReq && !x.IsInternal) || (isInternArchivedFilesReq && x.IsInternal) || !x.IsArchived)
                                    && ((isExternDeletedFilesReq && !x.IsInternal) || (isInternDeletedFilesReq && x.IsInternal) || !x.IsDeleted)).Select(x => x.FilePath).Distinct().ToList();

                                    foreach (var file in objFiles)
                                    {
                                        zip.CreateEntryFromFile(file, dossierName + "/" + buildingNumber + "/" + Path.GetFileName(file));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        List<string> filePaths = new List<string>();
                        foreach (var buildingId in downloadDossierFiles.DossierDownloadFiles.SelectMany(x => x.BuildingIds).Distinct())
                        {
                            string buildingNumber = _dbContext.Gebouw.Find(buildingId).BouwnummerExtern;
                            dossierNames.Clear();

                            foreach (var dossierId in downloadDossierFiles.DossierDownloadFiles.Where(x => x.BuildingIds.Contains(buildingId)).Select(x => x.DossierId))
                            {
                                var isBuyerGuide = IsUserBuyerGuideForDossier(dossierId, currentUserId);

                                //fetch archive and deleted files only for buyerguide
                                isExternArchivedFilesReq = isBuyerGuide && downloadDossierFiles.IsExternArchivedFilesReq;
                                isExternDeletedFilesReq = isBuyerGuide && downloadDossierFiles.IsExternDeletedFilesReq;
                                isInternDeletedFilesReq = isBuyerGuide && downloadDossierFiles.IsInternDeletedFilesReq;
                                isInternArchivedFilesReq = isBuyerGuide && downloadDossierFiles.IsInternArchivedFilesReq;

                                var dossier = _dbContext.Dossiers.Find(dossierId);
                                string dossierName = dossier.Naam;
                                if (dossierNames.ContainsKey(dossierName.ToLower()))
                                {
                                    dossierNames[dossierName.ToLower()] = ++dossierNames[dossierName.ToLower()];
                                    dossierName += " (" + dossierNames[dossierName.ToLower()] + ")";
                                }
                                else
                                    dossierNames.Add(dossierName.ToLower(), 0);


                                if (dossier.AlgemeneBestanden)
                                {
                                    //get general files
                                    filePaths = GetFilesForDownload(dossierId).Where(x => (dossier.Extern || x.IsInternal) && ((!x.IsArchived && !x.IsDeleted) || isBuyerGuide)
                                 && ((isExternArchivedFilesReq && !x.IsInternal) || (isInternArchivedFilesReq && x.IsInternal) || !x.IsArchived)
                                 && ((isExternDeletedFilesReq && !x.IsInternal) || (isInternDeletedFilesReq && x.IsInternal) || !x.IsDeleted)).Select(x => x.FilePath).ToList();

                                }
                                if (dossier.ObjectgebondenBestanden)
                                {
                                    filePaths.AddRange(GetFilesForDownload(dossierId, buildingId).Where(x => (dossier.Extern || x.IsInternal) && ((!x.IsArchived && !x.IsDeleted) || isBuyerGuide)
                                && ((isExternArchivedFilesReq && !x.IsInternal) || (isInternArchivedFilesReq && x.IsInternal) || !x.IsArchived)
                                && ((isExternDeletedFilesReq && !x.IsInternal) || (isInternDeletedFilesReq && x.IsInternal) || !x.IsDeleted)).Select(x => x.FilePath).ToList());

                                }

                                foreach (var file in filePaths.Distinct())
                                {
                                    zip.CreateEntryFromFile(file, buildingNumber + "/" + dossierName + "/" + Path.GetFileName(file));
                                }

                            }
                        }
                    }
                }
                return Path.GetFileName(zipFileName);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (zip != null)
                    zip.Dispose();
            }
        }

        private IQueryable<DossierAttachmentModelForDownload> GetFilesForDownload(string dossierId, string buildingId = null)
        {
            try
            {
                return _dbContext.BijlageDossiers.Include(x => x.DossierGu).Include(x => x.BijlageGu)
                    .Where(x => x.DossierGuid == dossierId && x.GebouwGuid == buildingId)
                    .Select(x => new DossierAttachmentModelForDownload
                    {
                        Name = x.BijlageGu.Omschrijving,
                        FilePath = x.BijlageGu.Bijlage1,
                        IsInternal = x.Intern,
                        IsArchived = x.Gearchiveerd,
                        IsDeleted = x.Verwijderd
                    }
                );
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool IsUserNotOnlySpectator(string dossierId, string buildingId = null)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            var projectId = _dbContext.Dossiers.Find(dossierId)?.WerkGuid;
            if (!string.IsNullOrWhiteSpace(projectId))
            {
                var availableModuleWithRoles = BuildQueryForAvailableModulesRolesForUser(currentUserId, projectId, dossierId);
                if (availableModuleWithRoles.Any(x => x.RoleName != LoginRoles.Spectator && x.ProjectId == projectId && (string.IsNullOrWhiteSpace(x.BuildingId) || x.BuildingId == buildingId)))
                {
                    return true;
                }
            }
            return false;
        }
    }
}


