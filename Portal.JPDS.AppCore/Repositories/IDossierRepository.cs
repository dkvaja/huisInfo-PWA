using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.Domain.Common;
using Portal.JPDS.AppCore.Models;
using Portal.JPDS.Domain.Entities;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Portal.JPDS.AppCore.Repositories
{
    public interface IDossierRepository
    {
        IEnumerable<DossierApiModel> GetAllDossiersByProjectId(string projectId, bool shouldHaveDeadline);
        DossierApiModel GetDossierBuildingInfo(string dossierId, string buildingId);
        DossierApiModel GetDossierGeneralInfo(string dossierId);
        IEnumerable<string> GetObjectsForDossier(string projectId);
        IEnumerable<DossierRoleswithUserApiModel> GetAvailableUsersAndRolesByProjectId(string projectId);
        string GetUploadLocationForDossierFiles(string dossierId, string buildingId);
        string CreateFile(DossierFileApiModel dossierFileApiModel, string attachmentId);
        bool UpdateDossierDataKeyValue(string dossierId, CommonKeyValueApiModel lstKeyValues);
        bool UpdateBuildingDossierDataKeyValue(string dossierId, string buildingId, CommonKeyValueApiModel lstKeyValues);
        bool DeleteDossier(string dossierId);
        bool UpdateDossierUserRights(string dossierId, IEnumerable<NewDossierUserRightApiModel> newUserApiModel);
        string CreateOrUpdateDossier(NewDossierApiModel newDossier);
        IEnumerable<DossierFileModel> GetExistingFilesForProject(string projectId);
        DossiersList GetDossiersListByProjectId(string projectId);
        Dictionary<string, string> GetDefaultNotificationTokens(string dossierId, List<string> buildingId = null, string employeeId = null);
        bool UpdateDossierBuildingStatus(string dossierId, bool isClosed, string buildingId);
        DossierFileModel GetUploadedDossierFile(string dossierFileId, bool filePathOnly = false);
        DossierResponseTypes MoveDossierBuildingsFiles(DossierFileApiModel dossierFileApiModel);
        DossierResponseTypes UpdateDossierBuildings(string dossierId, List<DossiersBuildingUpdateModel> dossiersBuildingUpdateModels);
        Dictionary<string, string> GetUserEmailAddress(List<string> userIdList);
        DossierApiModel GetDossierInfo(string dossierId);
        bool HasRolesForProject(string projectId, string userId, List<string> roleNames);
        bool IsUserModuleBuyerGuide(string projectId, string userId);
        string UploadBackgroundImageforDossier(string dossierId, string projectId, FileModel backgroundImageModel);
        bool UpdateBackgroundImage(string dossierId, string backgroundImagePath);
        List<DossierOverviewModel> GetDossiersListByBuildingId(string buildingId, bool shouldHaveDeadline);
        bool BuildingIsVisibleToBuyer(string dossierId, string buildingId);
        IEnumerable<BuildingOverviewModel> GetBuildingListWithDossiers(string projectId);
        DossierUserInfoModel GetUsersByDossierId(string dossierId, string buildingId);
        string CheckDuplicateDossierFile(string originalFileName, DossierFileApiModel fileModel);
        bool UpdateDossierDeadline(string dossierId, DateTime? deadlineDate, bool isUpdateBuildings);
        bool IsUserBuyerGuideForDossier(string dossierId, string userId = null);
        string UpdateDossierLastView(string dossierId, DossierLastViewModel viewModel);
        bool HasEditRightsToSection(string dossierId, string projectId, string buildingId, bool isInternal);
        void MarkWholeDossierAsViewed(string dossierId);
        void OrderDossier(string dossierId, string previousDossierId);
        string CreateZipForDownload(DownloadFilesModel downloadDossierFiles);
        bool IsUserNotOnlySpectator(string dossierId, string buildingId = null);
    }
}
