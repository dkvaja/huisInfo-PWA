import axios from 'axios';
import {
	createAurUpdateDossier,
	deleteDossier,
	getAllDossiers, getAllDossiersByBuildingId,
	getAllDossiersByProjectId,
	getAvailableUsersAndRolesByProjectId,
	getBackgroundImage,
	getBuildingListWithDossiers,
	getDossierBuildingInfo,
	getDossierGeneralInfo,
	getExistingImagesForProjects,
	linkFilesToDossier,
	moveDossierFiles,
	updateDossierBuildingDataByKey,
	updateDossierBuildings,
	updateDossierBuildingStatus,
	updateDossierDataByKey,
	updateDossierRight,
	uploadDossierFiles,
	updateDossierLastView,
	updateDossierDeadline,
} from '../apis/dossiersApi';
import { dossiersConstants } from '../_constants';

export const dossiersActions = {
	getAllDossiersByProject,
	getDossiers,
	getSelectedDossierInfo,
	uploadFiles,
	getRoles,
	updateDossiersRights,
	getDossiersByBuildingId,
	addUpdateDossier,
	filterDossiers,
	updateDossierInformation,
	getSelectedDossierBuildingInfo,
	deleteDraftDossier,
	getBackground,
	linkFiles,
	updateDossierBuilding,
	moveFiles,
	updateStatus,
	getBuildingWithDossiers,
	updateDossierLastRead,
	dossierDeadlineUpdate,
	selectAllDossiersBuilding,
	selectDossiersBuilding,
	selectAllBuildingColumn,
	removeSelectedDossier
};

var dossierInfoController = null;
var buildingInfoController = null;

function removeSelectedDossier() {
	return dispatch => {
		dispatch(getBackgroundImageSuccess(null));
		dispatch(getSelectedDossiersSuccess(null));
	};
}

function filterDossiers(filters) {
	return dispatch => {
		dispatch(getFilteredDossiers(filters))
	};
}

function selectAllDossiersBuilding(dossierId) {
	return dispatch => {
		dispatch(selectAllDossiersBuildingSuccess(dossierId));
	};
};

function selectDossiersBuilding(dossierId, buildingId) {
	return dispatch => {
		dispatch(selectDossiersBuildingSuccess(dossierId, buildingId));
	};
};

function selectAllBuildingColumn(buildingId, isChecked) {
	return dispatch => {
		dispatch(selectAllBuildingColumnSuccess(buildingId, isChecked));
	};
};

function getAllDossiersByProject(projectId) {
	return dispatch => {
		dispatch(getDossierLoading());
		getAllDossiersByProjectId(projectId).then(res => {
			dispatch(getImages(projectId));
			dispatch(getDossiersSuccess(res.data));
		}).catch(error => {
			dispatch(getDossiersError(error));
		})
	};
};

function getBuildingWithDossiers(projectId) {
	return dispatch => {
		dispatch(getBuildingWithDossiersLoading());
		getBuildingListWithDossiers(projectId).then(res => {
			// dispatch(getImages(projectId));
			dispatch(getBuildingWithDossiersSuccess(res.data));
		}).catch(error => {
			dispatch(getBuildingWithDossiersError(error));
		})
	};
};

function getBackground(dossierId) {
	return dispatch => {
		dispatch(getBackgroundImageLoading());
		getBackgroundImage(dossierId).then(res => {
			const image = window.URL.createObjectURL(res.data);
			dispatch(getBackgroundImageSuccess(image));
		}).catch(error => {
			dispatch(getBackgroundImageError(error));
		})
	};
};

function getDossiers(projectId) {
	return dispatch => {
		dispatch(getAllDossierLoading());
		getAllDossiers(projectId).then(res => {
			dispatch(getAllDossiersSuccess(res.data));
		}).catch(error => {
			dispatch(getDeadLineDossiersError(error));
		})
	};
};

function getDossiersByBuildingId(buildingId) {
	return dispatch => {
		dispatch(getDossierLoading());
		getAllDossiersByBuildingId(buildingId).then(res => {
			dispatch(getDossiersSuccess({ openOrClosedDossiers: res.data }));
		}).catch(error => {
			dispatch(getAllDossiersError(error));
		})
	};
};
function getImages(projectId) {
	return dispatch => {
		dispatch(getImagesLoading());
		getExistingImagesForProjects(projectId).then(res => {
			dispatch(getImagesSuccess(res.data));
		}).catch(error => {
			dispatch(getImagesError(error));
		})
	};
};

function getSelectedDossierInfo(id, buildingId, viewAs) {
	return dispatch => {

		if (dossierInfoController)
			dossierInfoController.cancel('canceled');
		dossierInfoController = axios.CancelToken.source();
		dispatch(getSelectedDossierLoading());
		getDossierGeneralInfo(id, dossierInfoController).then(async res => {
			dispatch(getBackground(id));
			dispatch(getSelectedDossiersSuccess(res.data));
			if (buildingId) dispatch(getSelectedDossierBuildingInfo({ dossierId: id, buildingId }, viewAs))
		}).catch(error => {
			console.log(error)
			dispatch(getSelectedDossiersError(error));
		})
	};
};

function deleteDraftDossier(id) {
	return dispatch => {
		dispatch(deleteDossierLoading());
		deleteDossier({ id }).then(res => {
			dispatch(deleteDossierSuccess({ id }));
		}).catch(error => {
			dispatch(deleteDossierError(error));
		})
	};
};

function getSelectedDossierBuildingInfo(data, viewAs) {
	return dispatch => {
		if (buildingInfoController)
			buildingInfoController.cancel('canceled');
		buildingInfoController = axios.CancelToken.source();

		dispatch(getSelectedDossierBuildingLoading());
		getDossierBuildingInfo(data, buildingInfoController).then(res => {
			if (viewAs === 'building')
				dispatch(getBackground(res.data.id));
			dispatch(getSelectedDossiersBuildingSuccess(data, res.data, viewAs));
		}).catch(error => {
			dispatch(getSelectedDossiersBuildingError(error));
		})
	};
};

function getRoles(id) {
	return dispatch => {
		dispatch(getAvailableRolesLoading());
		getAvailableUsersAndRolesByProjectId(id).then(res => {
			dispatch(getAvailableRolesSuccess(res.data));
		}).catch(error => {
			dispatch(getAvailableRolesError(error));
		})
	};
};

function updateDossiersRights(id, changedRoles) {
	return dispatch => {
		dispatch(updateDossierRightsLoading());
		updateDossierRight(id, changedRoles).then(async res => {
			const { data } = await getDossierGeneralInfo(id)
			dispatch(updateDossierRightsSuccess(data.userList));
		}).catch(error => {
			dispatch(updateDossierRightsError(error));
		})
	};
}

function updateStatus(data) {
	return dispatch => {
		dispatch(updateStatusLoading());
		updateDossierBuildingStatus(data).then(res => {
			dispatch(updateStatusSuccess(data));
		}).catch(error => {
			dispatch(updateStatusError(error));
		})
	};
}

function updateDossierInformation(data, isBuilding) {
	return dispatch => {
		dispatch(updateDossierInformationLoading());
		(isBuilding && data.id !== 'archive' && data.id !== 'extern' ? updateDossierBuildingDataByKey : updateDossierDataByKey)(data).then(res => {
			dispatch(updateDossierInformationSuccess(data, isBuilding));
		}).catch(error => {
			dispatch(updateDossierInformationError(error));
		})
	};
}

function uploadFiles(data, location) {
	return dispatch => {
		uploadDossierFiles(data).then(res => {
			dispatch(uploadFilesSuccess(res.data.uploadedFiles, location));
		}).catch(error => {
			dispatch(uploadFilesError(error));
		})
	};
}

function linkFiles(data, location) {
	return dispatch => {
		linkFilesToDossier(data).then(res => {
			dispatch(uploadFilesSuccess(res.data.linkedFiles, location));
		}).catch(error => {
			dispatch(linkFilesError(error));
		})
	};
}

function moveFiles(data, location) {
	return dispatch => {
		dispatch(moveFilesLoading(data));
		moveDossierFiles(data).then(res => {
			dispatch(moveFilesSuccess(data, location));
		}).catch(error => {
			dispatch(moveFilesError(error, data, location));
		})
	};
}

function updateDossierLastRead(data, location) {
	return dispatch => {
		dispatch(updateDossierLastReadLoading(data));
		updateDossierLastView(data).then(res => {
			dispatch(updateDossierLastReadSuccess(data, location));
		}).catch(error => {
			dispatch(updateDossierLastReadError(error));
		})
	};
}

function addUpdateDossier(data) {
	return dispatch => {
		dispatch(addDossierLoading());
		createAurUpdateDossier(data).then(res => {
			dispatch(addDossierSuccess({ ...data, id: res.data }));
		}).catch(error => {
			dispatch(addDossierError(error));
		})
	};
}

function updateDossierBuilding({ objects, ...data }) {
	return dispatch => {
		dispatch(updateDossierBuildingLoading());
		updateDossierBuildings(data).then(res => {
			dispatch(updateDossierBuildingSuccess(objects));
		}).catch(error => {
			dispatch(updateDossierBuildingError(error));
		})
	};
}

function dossierDeadlineUpdate(data) {
	return dispatch => {
		dispatch(updateDossierDeadlineLoading());
		updateDossierDeadline(data).then(res => {
			dispatch(updateDossierDeadlineSuccess(data));
		}).catch(error => {
			dispatch(updateDossierDeadlineError(error));
		})
	};
}



const selectAllDossiersBuildingSuccess = (dossierId) => {
	return { type: dossiersConstants.SELECT_ALL_DOSSIER_BUILDINGS, dossierId }
};

const selectDossiersBuildingSuccess = (dossierId, buildingId) => {
	return { type: dossiersConstants.SELECT_DOSSIER_BUILDING, dossierId, buildingId }
};

const selectAllBuildingColumnSuccess = (buildingId, isChecked) => {
	return { type: dossiersConstants.SELECT_ALL_BUILDINGS_COLUMN, buildingId, isChecked }
};

const getDossierLoading = () => {
	return { type: dossiersConstants.GET_DOSSIER_LOADING }
};

const getAllDossierLoading = () => {
	return { type: dossiersConstants.GET_ALL_DOSSIER_LOADING }
};
const getBuildingWithDossiersSuccess = (buildings) => {
	return { type: dossiersConstants.GET_BUILDING_LIST_WITH_DOSSIERS_SUCCESS, buildings }
};

const getBuildingWithDossiersError = (error) => {
	return { type: dossiersConstants.GET_BUILDING_LIST_WITH_DOSSIERS_ERROR, error }
};

const getBuildingWithDossiersLoading = () => {
	return { type: dossiersConstants.GET_BUILDING_LIST_WITH_DOSSIERS_LOADING }
};

const getDossiersSuccess = (dossiers) => {
	return { type: dossiersConstants.GET_DOSSIERS_SUCCESS, dossiers };
};

const getDossiersError = (error) => {
	return { type: dossiersConstants.GET_DOSSIERS_ERROR, error };
};

const getAllDossiersSuccess = (dossiers) => {
	return { type: dossiersConstants.GET_ALL_DOSSIERS_SUCCESS, dossiers };
};

const getAllDossiersError = (error) => {
	return { type: dossiersConstants.GET_ALL_DOSSIERS_ERROR, error };
};

const getDeadLineDossiersError = (error) => {
	return { type: dossiersConstants.GET_DEADLINE_DOSSIERS_ERROR, error };
};

const getSelectedDossierLoading = () => {
	return { type: dossiersConstants.GET_SELECTED_DOSSIER_LOADING }
};

const getSelectedDossiersSuccess = (dossiers) => {
	return { type: dossiersConstants.GET_SELECTED_DOSSIERS_SUCCESS, dossiers };
};

const getSelectedDossiersError = (error) => {
	return { type: dossiersConstants.GET_SELECTED_DOSSIERS_ERROR, error };
};
const getSelectedDossierBuildingLoading = () => {
	return { type: dossiersConstants.GET_SELECTED_DOSSIER_BUILDING_LOADING }
};

const getSelectedDossiersBuildingSuccess = (data, dossiers, viewAs) => {
	return { type: dossiersConstants.GET_SELECTED_DOSSIERS_BUILDING_SUCCESS, data, dossiers, viewAs };
};

const getSelectedDossiersBuildingError = (error) => {
	return { type: dossiersConstants.GET_SELECTED_DOSSIERS_BUILDING_ERROR, error };
};

const getAvailableRolesLoading = () => {
	return { type: dossiersConstants.GET_AVAILABLE_ROLES_LOADING }
};

const getAvailableRolesSuccess = (roles) => {
	return { type: dossiersConstants.GET_AVAILABLE_ROLES_SUCCESS, roles };
};

const getAvailableRolesError = (error) => {
	return { type: dossiersConstants.GET_AVAILABLE_ROLES_ERROR, error };
};

const getFilteredDossiers = (filters) => {
	return { type: dossiersConstants.FILTER_DOSSIERS, filters }
}

const addDossierLoading = () => {
	return { type: dossiersConstants.ADD_DOSSIER_LOADING };
};

const updateDossierInformationLoading = () => {
	return { type: dossiersConstants.UPDATE_DOSSIER_INFORMATION_LOADING };
};

const updateDossierInformationError = (error) => {
	return { type: dossiersConstants.UPDATE_DOSSIER_INFORMATION_ERROR, error };
};

const updateDossierInformationSuccess = (dossier) => {
	return { type: dossiersConstants.UPDATE_DOSSIER_INFORMATION_SUCCESS, dossier };
};
const addDossierSuccess = (dossier) => {
	return { type: dossiersConstants.ADD_DOSSIER_SUCCESS, dossier };
};
const addDossierError = (error) => {
	return { type: dossiersConstants.ADD_DOSSIER_ERROR, error };
};

const deleteDossierLoading = () => {
	return { type: dossiersConstants.DELETE_DOSSIER_LOADING };
};

const deleteDossierSuccess = (dossier) => {
	return { type: dossiersConstants.DELETE_DOSSIER_SUCCESS, dossier };
};
const deleteDossierError = (error) => {
	return { type: dossiersConstants.DELETE_DOSSIER_ERROR, error };
};

const getImagesLoading = () => {
	return { type: dossiersConstants.GET_IMAGES_LOADING };
};

const getImagesSuccess = (images) => {
	return { type: dossiersConstants.GET_IMAGES_SUCCESS, images };
};
const getImagesError = (error) => {
	return { type: dossiersConstants.GET_IMAGES_ERROR, error };
};

const updateDossierRightsLoading = () => {
	return { type: dossiersConstants.UPDATE_DOSSIER_RIGHTS_LOADING }
};

const updateDossierRightsSuccess = (userList) => {
	return { type: dossiersConstants.UPDATE_DOSSIER_RIGHTS_SUCCESS, userList };
};

const updateDossierRightsError = (error) => {
	return { type: dossiersConstants.UPDATE_DOSSIER_RIGHTS_ERROR, error };
};

const updateStatusLoading = () => {
	return { type: dossiersConstants.UPDATE_STATUS_LOADING }
};

const updateStatusSuccess = (status) => {
	return { type: dossiersConstants.UPDATE_STATUS_SUCCESS, status };
};

const updateStatusError = (error) => {
	return { type: dossiersConstants.UPDATE_STATUS_ERROR, error };
};

const getBackgroundImageSuccess = (image) => {
	return {
		type: dossiersConstants.GET_BACKGROUND_IMAGE_SUCCESS,
		image
	}
};

const getBackgroundImageLoading = () => {
	return {
		type: dossiersConstants.GET_BACKGROUND_IMAGE_LOADING,
	}
};

const getBackgroundImageError = (error) => {
	return {
		type: dossiersConstants.GET_BACKGROUND_IMAGE_ERROR,
		error,
	}
}
const linkFilesError = (error) => {
	return {
		type: dossiersConstants.LINK_FILE_ERROR,
		error,
	}
}
const linkFilesSuccess = (files) => {
	return {
		type: dossiersConstants.LINK_FILE_SUCCESS,
		files
	}
}
const uploadFilesError = (error) => {
	return {
		type: dossiersConstants.UPLOAD_FILES_ERROR,
		error,
	}
};
const uploadFilesSuccess = (files, location) => {
	return {
		type: dossiersConstants.UPLOAD_FILES_SUCCESS,
		files,
		location
	}
};

const updateDossierBuildingError = (error) => {
	return {
		type: dossiersConstants.UPDATE_DOSSIER_BUILDING_ERROR,
		error
	}
}

const updateDossierBuildingLoading = (error) => {
	return {
		type: dossiersConstants.UPDATE_DOSSIER_BUILDING_LOADING,
		error
	}
}

const updateDossierBuildingSuccess = (objects) => {
	return {
		type: dossiersConstants.UPDATE_DOSSIER_BUILDING_SUCCESS,
		objects
	}
}

const moveFilesLoading = () => {
	return {
		type: dossiersConstants.MOVE_FILES_LOADING,
	}
}

const moveFilesError = (error, files, location) => {
	return {
		type: dossiersConstants.MOVE_FILES_ERROR,
		error, files, location
	}
}

const moveFilesSuccess = (files, location) => {
	return {
		type: dossiersConstants.MOVE_FILES_SUCCESS,
		files, location
	}
}


const updateDossierLastReadLoading = () => {
	return {
		type: dossiersConstants.UPDATE_DOSSIER_LAST_VIEW_LOADING,
	}
}

const updateDossierLastReadError = (error) => {
	return {
		type: dossiersConstants.UPDATE_DOSSIER_LAST_VIEW_ERROR,
		error
	}
}

const updateDossierLastReadSuccess = (data, location) => {
	return {
		type: dossiersConstants.UPDATE_DOSSIER_LAST_VIEW_SUCCESS,
		data, location
	}
}

const updateDossierDeadlineLoading = () => {
	return {
		type: dossiersConstants.UPDATE_DOSSIER_DEADLINE_LOADING,
	}
}

const updateDossierDeadlineError = (error) => {
	return {
		type: dossiersConstants.UPDATE_DOSSIER_DEADLINE_ERROR,
		error
	}
}

const updateDossierDeadlineSuccess = (data) => {
	return {
		type: dossiersConstants.UPDATE_DOSSIER_DEADLINE_SUCCESS,
		data
	}
}
