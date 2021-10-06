import { URLS } from "./urls";
import Axios from "axios";

export const getAllDossiers = projectId => Axios.get(URLS.GET_ALL_DOSSIERS + projectId);
export const getBuildingListWithDossiers = projectId => Axios.get(URLS.GET_BUILDING_LIST_WITH_DOSSIERS + projectId);
export const getAllDossiersByProjectId = projectId => Axios.get(URLS.GET_ALL_DOSSIERS_BY_PROJECT_ID + projectId);
export const getAllDossiersByBuildingId = buildingId => Axios.get(URLS.GET_ALL_DOSSIERS_BY_BUILDING_ID + buildingId);
export const getDossierGeneralInfo = (dossierId, dossierInfoController) => Axios.get(URLS.GET_DOSSIER_GENERAL_INFO + dossierId, dossierInfoController && { cancelToken: dossierInfoController.token });
// export const getDossierGeneralInfo = dossierId => Axios.post(URLS.GET_DOSSIER_GENERAL_INFO, { id:dossierId });
export const getDossierBuildingInfo = ({ dossierId, buildingId }, buildingInfoController) => Axios.get(URLS.GET_DOSSIER_BUILDING_INFO + dossierId + '/' + buildingId, { cancelToken: buildingInfoController.token });
export const getUsersForDossierShare = dossierId => Axios.get(URLS.GET_USERS_FOR_DOSSIER_SHARE + dossierId);
// export const getDossierBuildingInfo = data => Axios.post(URLS.GET_DOSSIER_BUILDING_INFO, data);
export const getBackgroundImage = dossierId => Axios.get(URLS.GET_BACKGROUND_IMAGE + dossierId, { responseType: 'blob' });
export const getAvailableUsersAndRolesByProjectId = projectId => Axios.get(URLS.GET_AVAILABLE_USERS_ROLE_BY_PROJECT_ID + projectId);
export const updateDossierRight = (dossierId, data) => Axios.post(URLS.UPDATE_DOSSIER_RIGHTS + dossierId, data);
export const uploadDossierFiles = data => Axios.post(URLS.UPLOAD_DOSSIER_FILES, data);
export const createAurUpdateDossier = data => Axios.post(URLS.ADD_DOSSIER, data);

export const updateDossierDataByKey = ({ id, buildingId, ...data }) =>
	Axios.post(URLS.UPDATE_DOSSIER_INFORMATION + id, Object.keys(data).map(id => ({ id, name: data[id] })));

export const updateDossierBuildingDataByKey = ({ id, buildingId, ...data }) =>
	Axios.post(URLS.UPDATE_DOSSIER_BUILDING_DATA_BY_KEY + id + '/' + buildingId, Object.keys(data).map(id => ({
		id,
		name: data[id]
	})));

export const deleteDossier = ({ id }) => Axios.delete(URLS.DELETE_DOSSIER + id);
export const sendDossierNotification = data => Axios.post(URLS.SEND_DOSSIER_NOTIFICATION, data);
export const getExistingImagesForProjects = projectId => Axios.get(URLS.GET_EXISTING_IMAGES_FOR_PROJECTS + projectId)
export const getUploadedDossierImage = dossierImageId => Axios.get(URLS.GET_UPLOADED_DOSSIER_IMAGE + dossierImageId, { responseType: 'blob' })
export const linkFilesToDossier = data => Axios.post(URLS.LINK_FILE_TO_DOSSIER, data);
export const updateDossierBuildings = ({ dossierId, changedObjects }) => Axios.post(URLS.UPDATE_DOSSIER_BUILDING + dossierId, changedObjects);
export const updateDossierBuildingStatus = ({ dossierId, isClosed, buildingId }) => Axios.post(URLS.UPDATE_DOSSIER_BUILDING_STATUS + dossierId + `?isClosed=${isClosed}${buildingId ? `&buildingId=${buildingId}` : ''}`)
export const moveDossierFiles = data => Axios.post(URLS.MOVE_DOSSIER_FILES, data);
export const uploadBackgroundImage = ({ dossierId, ...data }) => Axios.post(URLS.UPLOAD_BACKGROUND_IMAGE + dossierId, data)
export const updateDossierLastView = ({ dossierId, ...data }) => Axios.post(URLS.UPDATE_DOSSIER_LAST_VIEW + dossierId, data)
export const updateDossierDeadline = ({ dossierId, deadlineDate, isUpdateBuildings, ...data }) => Axios.post(URLS.UPDATE_DOSSIER_DEADLINE + dossierId + `?deadlineDate=${deadlineDate}&isUpdateBuildings=${isUpdateBuildings}`)
export const createDownloadZip = data => Axios.post(URLS.CREATE_DOWNLOAD_ZIP, data);
export const downloadZip = fileName => Axios.get(URLS.DOWNLOAD_ZIP_FILE + fileName);
export const orderDossiers = ({ dossierId, previousDossierId, ...data }) => Axios.post(URLS.ORDER_DOSSIERS + dossierId + `?previousDossierId=${previousDossierId}`);
