using Portal.JPDS.AppCore.Models;
using Portal.JPDS.Domain.Common;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class DossierApiModel
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public string GeneralInformation { get; set; }
        public bool HasBackground { get; set; }
        public string BackgroundImagePath { get; set; }
        public string BackgroundImageName { get; set; }
        public DateTime? Deadline { get; set; }
        public DossierStatus Status { get; set; }
        public bool IsArchived { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ClosedOn { get; set; }
        public bool HasGeneralFiles { get; set; }
        public bool HasObjectBoundFiles { get; set; }
        public List<DossiersBuildingApiModel> BuildingInfoList { get; set; }
        public List<DossierUserApiModel> UserList { get; set; }
        public DossierFilesList InternalFiles { get; set; }
        public DossierFilesList ExternalFiles { get; set; }
        public bool IsOverdue { get; set; }
        public bool Is48hoursReminder { get; set; }
        public bool IsExternal { get; set; }
        public bool HasUpdates { get; set; }
    }

    public class DossierRoleswithUserApiModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public List<DossierUserListModel> UsersList { get; set; }
    }

    public class DossierUserListModel
    {
        public string LoginId { get; set; }
        public string Name { get; set; }
    }

    public class DossierUserApiModel
    {
        public string DossierRightId { get; set; }
        public string LoginId { get; set; }
        public string ModuleId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsExternal { get; set; }
        public bool IsExternalVisible { get; set; }
        public bool HasExternalEditRights { get; set; }
        public bool IsInternal { get; set; }        
        public bool IsInternalVisible { get; set; }
        public bool HasInternalEditRights { get; set; }
        public OrgInfoApiModel UserContactInfo { get; set; }
    }

    public class DossiersBuildingApiModel
    {
        public string BuildingId { get; set; }
        public DateTime? Deadline { get; set; }
        public DossierStatus Status { get; set; }
        public bool IsActive { get; set; }
        public DossierFilesList InternalObjectFiles { get; set; }
        public DossierFilesList ExternalObjectFiles { get; set; }
        public bool IsOverdue { get; set; }
        public bool Is48hoursReminder { get; set; }
        public BuyersInfoApiModel BuyerContactInfo { get; set; }
        public DateTime? ClosedOn { get; set; }
        public bool HasUpdates { get; set; }
    }

    public class DossierFileApiModel
    {
        public string DossierId { get; set; }
        public string BuildingId { get; set; }
        public bool IsInternal { get; set; }
        public bool IsArchived { get; set; }
        public bool IsDeleted { get; set; }
        public List<DossierFileListModel> DossierFileList { get; set; }
    }

    public class DossierFileListModel
    {
        public string DossierFileId { get; set; }
        public string AttachmentId { get; set; }
    }

    public class DossierNotificationModel
    {
        public string DossierId { get; set; }
        public List<string> BuildingIds { get; set; }
        public string Message { get; set; }
        public List<string> ToUserIdList { get; set; }
        public List<string> BuyerBuildingIds { get; set; }
    }

    public class NewDossierApiModel
    {
        public string DossierId { get; set; }
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public string GeneralInformation { get; set; }
        public FileModel BackgroundImage { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsDraft { get; set; }
        public bool HasGeneralFiles { get; set; }
        public bool HasObjectBoundFiles { get; set; }
        public bool IsExternal { get; set; }
        public List<NewDossiersBuildingApiModel> BuildingInfoList { get; set; }
        public List<NewDossierUserRightApiModel> UserList { get; set; }
    }

    public class NewDossiersBuildingApiModel
    {
        public string BuildingId { get; set; }
        public bool IsActive { get; set; }

    }

    public class NewDossierUserRightApiModel
    {
        public string LoginId { get; set; }
        public string ModuleId { get; set; }
        public string RoleId { get; set; }
        public bool IsExternal { get; set; }
        public bool HasExternalEditRights { get; set; }
        public bool IsExternalVisible { get; set; }
        public bool IsInternal { get; set; }
        public bool HasInternalEditRights { get; set; }
        public bool IsInternalVisible { get; set; }
    }

    public class DossiersList
    {
        public List<DossierOverviewModel> OpenOrClosedDossiers { get; set; }
        public List<DossierOverviewModel> DraftDossiers { get; set; }
        public List<DossierOverviewModel> ArchiveDossiers { get; set; }
    }

    public class DossierOverviewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? Deadline { get; set; }
        public DossierStatus Status { get; set; }
        public bool IsExternal { get; set; }
        public bool HasUpdates { get; set; }
    }

    public class DossierFilesList
    {
        public DossierFilesList()
        {
            UploadedFiles = new List<DossierFileModel>();
            ArchivedFiles = new List<DossierFileModel>();
            DeletedFiles = new List<DossierFileModel>();
        }
        public List<DossierFileModel> UploadedFiles { get; set; }
        public List<DossierFileModel> ArchivedFiles { get; set; }
        public List<DossierFileModel> DeletedFiles { get; set; }
    }

    public class DossierFileModel
    {
        public string FileId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public string UploadedBy { get; set; }
        public bool HasRights { get; set; }
        public bool HasUpdates { get; set; }
        public string AttachmentId { get; set; }
    }

    public class DossiersBuildingUpdateModel
    {
        public string BuildingId { get; set; }
        public bool IsActive { get; set; }
    }

    public class DossierUserRightsModel
    {
        public string DossierId { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ProjectId { get; set; }
        public string BuildingId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsExternal { get; set; }
        public bool HasExternalEditRights { get; set; }
        public bool IsExternalVisible { get; set; }
        public bool IsInternal { get; set; }
        public bool HasInternalEditRights { get; set; }
        public bool IsInternalVisible { get; set; }
    }

    public class LoginDossierUserRightsModel
    {
        public string DossierId { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string DossierRightId { get; set; }
        public string LoginId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string LoginOrgId { get; set; }
        public string LoginRelationId { get; set; }
        public bool IsInternal { get; set; }
        public bool HasInternalEditRights { get; set; }
        public bool IsInternalVisible { get; set; }
        public bool IsExternal { get; set; }
        public bool HasExternalEditRights { get; set; }
        public bool IsExternalVisible { get; set; }
    }


    public class BuildingOverviewModel
    {
        public string BuildingId { get; set; }
        public IEnumerable<DossierOverviewModel> DossierList { get; set; }
    }

    public class DossierUserInfoModel
    {
        public string DossierId { get; set; }
        public List<BuyersInfoApiModel> BuyerContactInfo { get; set; }
        public List<DossierUserApiModel> UsersList { get; set; }
    }

    public class DossierLastViewModel
    {
        public string BuildingId { get; set; }
        public string DossierFileId { get; set; }
        public DateTime? LastViewDate { get; set; }
    }

    public class DossierAttachmentModel
    {
        public string Guid { get; set; }
        public string BuildingId { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public string UploadedBy { get; set; }
        public string UploadedByGuid { get; set; }
        public bool HasRights { get; set; }
        public bool HasUpdates { get; set; }
        public bool IsArchived { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class DownloadFilesModel
    {
        public List<DossierDownloadFilesModel> DossierDownloadFiles { get; set; }
        public bool IsDossierFolderFormat { get; set; }
        public bool IsInternMainFileReq { get; set; }
        public bool IsInternArchivedFilesReq { get; set; }
        public bool IsInternDeletedFilesReq { get; set; }
        public bool IsExternMainFileReq { get; set; }
        public bool IsExternArchivedFilesReq { get; set; }
        public bool IsExternDeletedFilesReq { get; set; }
    }

    public class DossierDownloadFilesModel
    {
        public string DossierId { get; set; }
        public List<string> BuildingIds { get; set; }
    }
    public class DossierAttachmentModelForDownload
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public bool IsArchived { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsInternal { get; set; }
    }
}