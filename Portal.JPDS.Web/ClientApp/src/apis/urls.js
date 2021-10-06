const { webApiUrl } = window.appConfig;

export const URLS = {
    GET_ALL_DOSSIERS_BY_PROJECT_ID: webApiUrl + "api/Dossier/GetDossiersListByProjectId/",
    GET_ALL_DOSSIERS_BY_BUILDING_ID: webApiUrl + "api/Dossier/GetDossiersListByBuildingId/",
    GET_ALL_DOSSIERS: webApiUrl + "api/Dossier/GetAllDossiersByProjectId/",
    GET_DOSSIER_BUILDING_INFO: webApiUrl + "api/Dossier/GetDossierBuildingInfo/",
    GET_DOSSIER_GENERAL_INFO: webApiUrl + "api/Dossier/GetDossierGeneralInfo/",
    GET_USERS_FOR_DOSSIER_SHARE: webApiUrl + "api/Dossier/GetUsersForDossierDeadline/",
    GET_AVAILABLE_USERS_ROLE_BY_PROJECT_ID: webApiUrl + "api/Dossier/GetAvailableUsersAndRolesByProjectId/",
    UPDATE_DOSSIER_RIGHTS: webApiUrl + "api/Dossier/UpdateDossierUserRights/",
    UPLOAD_DOSSIER_FILES: webApiUrl + "api/Dossier/UploadDossierFiles/",
    ADD_DOSSIER: webApiUrl + "api/Dossier/CreateOrUpdateDossier/",
    UPDATE_DOSSIER_INFORMATION: webApiUrl + "api/Dossier/UpdateDossierDataByKey/",
    DELETE_DOSSIER: webApiUrl + "api/Dossier/DeleteDossier/",
    SEND_DOSSIER_NOTIFICATION: webApiUrl + "api/Dossier/DossierNotifications/",
    GET_EXISTING_IMAGES_FOR_PROJECTS: webApiUrl + "api/Dossier/GetExistingFilesForProject/",
    GET_BACKGROUND_IMAGE: webApiUrl + "api/Dossier/GetBackgroundImage/",
    GET_UPLOADED_DOSSIER_IMAGE: webApiUrl + "api/Dossier/GetUploadedDossierFile/",
    GET_UPLOADED_DOSSIER_FILE_THUMBNAIL: webApiUrl + "api/Dossier/GetFileThumbnail/",
    GET_ATTACHMENT: webApiUrl + "api/Home/GetAttachment/",
    GET_ATTACHMENT_THUMBNAIL: webApiUrl + "api/Home/GetAttachmentThumbnail/",
    UPDATE_DOSSIER_BUILDING_DATA_BY_KEY: webApiUrl + "api/Dossier/UpdateBuildingDossierDataKeyValue/",
    LINK_FILE_TO_DOSSIER: webApiUrl + "api/Dossier/LinkFilesToDossier/",
    UPDATE_DOSSIER_BUILDING: webApiUrl + "api/Dossier/UpdateDossierBuildings/",
    UPDATE_DOSSIER_BUILDING_STATUS: webApiUrl + "api/Dossier/UpdateDossierBuildingStatus/",
    MOVE_DOSSIER_FILES: webApiUrl + "api/Dossier/MoveDossierBuildingsFiles/",
    UPLOAD_BACKGROUND_IMAGE: webApiUrl + "api/Dossier/UploadBackgroundImage/",
    GET_BUILDING_LIST_WITH_DOSSIERS: webApiUrl + "api/Dossier/GetBuildingListWithDossiers/",
    UPDATE_DOSSIER_LAST_VIEW: webApiUrl + "api/Dossier/UpdateDossierLastView/",
    CREATE_DOWNLOAD_ZIP: webApiUrl + "api/Dossier/CreateZipForDownload/",
    DOWNLOAD_ZIP_FILE: webApiUrl + "api/Dossier/DownloadZip/",
    ORDER_DOSSIERS: webApiUrl + "api/Dossier/OrderDossier/",
    UPDATE_DOSSIER_DEADLINE: webApiUrl + "api/Dossier/UpdateDossierDeadline/"
};
