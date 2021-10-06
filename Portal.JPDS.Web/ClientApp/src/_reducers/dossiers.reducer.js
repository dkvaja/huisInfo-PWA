import { dossiersConstants } from '../_constants';
import { getUser } from '../_helpers';

const initialState = {
	allDossiers: [],
	filteredAllDossiers: [],
	dossiers: {},
	selectedDossier: null,
	error: '',
	selectedError: '',
	dossierLoading: false,
	loading: false,
	selectedLoading: false,
	availableRoles: [],
	addLoading: false,
	selectedBackground: '',
	buildingList: [],
	getBackgroundLoading: false,
	selectedDossierBuilding: {}
}

export function dossier(state = initialState, action) {
	switch (action.type) {
		case dossiersConstants.GET_DOSSIERS_SUCCESS:
			return {
				...state,
				dossiers: action.dossiers,
				error: '',
				loading: false,
				addLoading: false
			};
		case dossiersConstants.GET_DOSSIERS_ERROR:
			return {
				...state,
				error: action.error,
				dossiers: {},
				loading: false,
			};
		case dossiersConstants.GET_BUILDING_LIST_WITH_DOSSIERS_SUCCESS:
			return {
				...state,
				buildingList: action.buildings,
				// dossiers: {
				// 	...state.dossiers,
				// 	openOrClosedDossiers: []
				// },
				error: '',
				getBuildingLoading: false,
				addLoading: false
			};
		case dossiersConstants.GET_BUILDING_LIST_WITH_DOSSIERS_ERROR:
			return {
				...state,
				buildingList: [],
				// buildingList: action.buildings,
				error: action.error,
				getBuildingLoading: false,
				addLoading: false
			};
		case dossiersConstants.GET_BUILDING_LIST_WITH_DOSSIERS_LOADING:
			return {
				...state,
				buildingList: [],
				selectedBackground: null,
				getBuildingLoading: true,
				addLoading: false
			};
		case dossiersConstants.GET_ALL_DOSSIERS_SUCCESS:
			const { dossiers } = action;
			let dossierData = [];
			if (dossiers.length) {
				dossierData = dossiers.map(p => {
					const obj = p.buildingInfoList.reduce(function (acc, cur, i) {
						acc[cur.buildingId] = cur;
						return acc;
					}, {});
					return { ...p, buildingInfoList: obj }
				});
			}
			return {
				...state,
				allDossiers: dossierData,
				error: null,
				selectedBackground: '',
				deadlineDossierLoading: false,
			};
		case dossiersConstants.FILTER_DOSSIERS:
			const { filters } = action;
			const dossiersForFilter = [...state.allDossiers];
			const filteredAllDossiers = dossiersForFilter.filter(row => {
				if (!!filters.startDate && !!filters.endDate) return (!!row.deadline && (new Date(filters.startDate).getTime() <= new Date(row.deadline).getTime()) && (new Date(row.deadline).getTime() <= new Date(filters.endDate).getTime()));
				return true
			})
				.filter(row => {
					if (!!filters.status) return row.status && row.status === filters.status;
					return true
				})
				.filter(row => {
					if (filters.is48hoursReminder) return row.is48hoursReminder === filters.is48hoursReminder;
					return true
				}).filter(row => {
					if (filters.isOverdue) return row.isOverdue === filters.isOverdue;
					return true
				}).filter(row => {
					if (filters.hasUpdates) return row.hasUpdates === filters.hasUpdates;
					return true
				})
				.map(row => row)
			return {
				...state,
				filteredAllDossiers
			}
		case dossiersConstants.SELECT_ALL_DOSSIER_BUILDINGS:
			var allDossiersData = [...state.allDossiers];
			var dossierIndex = allDossiersData.findIndex(d => d.id === action.dossierId);
			if (dossierIndex >= 0) {
				const isChecked = !allDossiersData[dossierIndex].isSelectedAllBuilding;
				const buildingInfoList = { ...allDossiersData[dossierIndex].buildingInfoList };
				for (let buildingId in buildingInfoList) {
					buildingInfoList[buildingId] = {
						...buildingInfoList[buildingId],
						isSelected: isChecked,
						dossierId: action.dossierId
					};
				}
				allDossiersData[dossierIndex] = {
					...allDossiersData[dossierIndex],
					buildingInfoList,
					isSelectedAllBuilding: isChecked,
					isSelectedAnyBuilding: false
				}
			}

			const allfilteredDossiersDataOfBuildings = [...state.filteredAllDossiers];
			const filteredDossierIndexForAllBuildings = allfilteredDossiersDataOfBuildings.findIndex(d => d.id === action.dossierId);
			if (filteredDossierIndexForAllBuildings >= 0) {
				const isChecked = !allfilteredDossiersDataOfBuildings[filteredDossierIndexForAllBuildings].isSelectedAllBuilding;
				const buildingInfoList = { ...allfilteredDossiersDataOfBuildings[filteredDossierIndexForAllBuildings].buildingInfoList };
				for (let buildingId in buildingInfoList) {
					buildingInfoList[buildingId] = {
						...buildingInfoList[buildingId],
						isSelected: isChecked,
						dossierId: action.dossierId
					};
				}
				allfilteredDossiersDataOfBuildings[filteredDossierIndexForAllBuildings] = {
					...allfilteredDossiersDataOfBuildings[filteredDossierIndexForAllBuildings],
					buildingInfoList,
					isSelectedAllBuilding: isChecked,
					isSelectedAnyBuilding: false
				}
			}
			return {
				...state,
				allDossiers: allDossiersData,
				filteredAllDossiers: allfilteredDossiersDataOfBuildings
			}

		case dossiersConstants.SELECT_DOSSIER_BUILDING:
			var allDossiersData = [...state.allDossiers];
			var dossierIndex = allDossiersData.findIndex(p => p.id === action.dossierId);
			if (dossierIndex >= 0) {
				const buildingInfoList = { ...allDossiersData[dossierIndex].buildingInfoList };
				buildingInfoList[action.buildingId] = {
					...buildingInfoList[action.buildingId],
					isSelected: !buildingInfoList[action.buildingId].isSelected
				};
				const buildingInfoListArray = Object.values(buildingInfoList);
				const isSelectedAllBuilding = buildingInfoListArray.every(p => !!p.isSelected);
				allDossiersData[dossierIndex] = {
					...allDossiersData[dossierIndex],
					buildingInfoList,
					isSelectedAnyBuilding: !isSelectedAllBuilding && buildingInfoListArray.some(p => !!p.isSelected),
					isSelectedAllBuilding
				}
			}

			const allfilteredDossiersData = [...state.filteredAllDossiers];
			const filteredDossierIndex = allfilteredDossiersData.findIndex(p => p.id === action.dossierId);
			if (filteredDossierIndex >= 0) {
				const buildingInfoList = { ...allfilteredDossiersData[filteredDossierIndex].buildingInfoList };
				buildingInfoList[action.buildingId] = {
					...buildingInfoList[action.buildingId],
					isSelected: !buildingInfoList[action.buildingId].isSelected
				};
				const buildingInfoListArray = Object.values(buildingInfoList);
				const isSelectedAllBuilding = buildingInfoListArray.every(p => !!p.isSelected);
				allfilteredDossiersData[filteredDossierIndex] = {
					...allfilteredDossiersData[filteredDossierIndex],
					buildingInfoList,
					isSelectedAnyBuilding: !isSelectedAllBuilding && buildingInfoListArray.some(p => !!p.isSelected),
					isSelectedAllBuilding
				}
			}
			return {
				...state,
				allDossiers: allDossiersData,
				filteredAllDossiers: allfilteredDossiersData
			}

		case dossiersConstants.SELECT_ALL_BUILDINGS_COLUMN:
			const modifiedDossiers = state.allDossiers.map(p => {
				const selectedBuilding = p.buildingInfoList[action.buildingId]
				if (selectedBuilding) {
					p.buildingInfoList[action.buildingId] = {
						...selectedBuilding,
						isSelected: action.isChecked,
						dossierId: p.id
					};
					const selectedDossierBuilding = Object.values(p.buildingInfoList);
					const isSelectedAllBuilding = selectedDossierBuilding.every(b => !!b.isSelected);
					const isSelectedAnyBuilding = !isSelectedAllBuilding && selectedDossierBuilding.some(b => !!b.isSelected);
					p = {
						...p,
						isSelectedAnyBuilding,
						isSelectedAllBuilding
					}
				}
				return p;
			});
			const modifiedfilteredDossiers = state.filteredAllDossiers.map(p => {
				const selectedBuilding = p.buildingInfoList[action.buildingId]
				if (selectedBuilding) {
					p.buildingInfoList[action.buildingId] = {
						...selectedBuilding,
						isSelected: action.isChecked,
						dossierId: p.id
					};
					const selectedDossierBuilding = Object.values(p.buildingInfoList);
					const isSelectedAllBuilding = selectedDossierBuilding.every(b => !!b.isSelected);
					const isSelectedAnyBuilding = !isSelectedAllBuilding && selectedDossierBuilding.some(b => !!b.isSelected);
					p = {
						...p,
						isSelectedAnyBuilding,
						isSelectedAllBuilding
					}
				}
				return p;
			});
			return {
				...state,
				allDossiers: modifiedDossiers,
				filteredAllDossiers: modifiedfilteredDossiers
			}
		case dossiersConstants.GET_ALL_DOSSIERS_ERROR:
			return {
				...state,
				error: action.error,
				addLoading: false
			};
		case dossiersConstants.GET_DEADLINE_DOSSIERS_ERROR:
			return {
				...state,
				error: action.error,
				deadlineDossierLoading: false,
			};
		case dossiersConstants.GET_ALL_DOSSIER_LOADING:
			return {
				...state,
				selectedBackground: null,
				deadlineDossierLoading: true
			};
		case dossiersConstants.GET_DOSSIER_LOADING:
			return {
				...state,
				selectedDossier: null,
				selectedBackground: null,
				selectedDossierBuilding: {},
				dossiers: {},
				loading: true
			};
		case dossiersConstants.GET_SELECTED_DOSSIER_LOADING:
			return {
				...state,
				selectedBackground: null,
				selectedDossier: null,
				dossierLoading: true,
				selectedLoading: true
			};
		case dossiersConstants.GET_SELECTED_DOSSIERS_ERROR:
			if (action.error.message === 'canceled') {
				return {
					...state,
					selectedDossier: null,
				}
			}
			return {
				...state,
				dossierLoading: false,
				selectedDossier: null,
				selectedLoading: false,
				selectedError: action.error
			};
		case dossiersConstants.GET_SELECTED_DOSSIERS_SUCCESS:
			return {
				...state,
				selectedDossier: action.dossiers,
				dossierLoading: false,
				selectedLoading: false,
				selectedError: null
			};
		case dossiersConstants.GET_SELECTED_DOSSIER_BUILDING_LOADING:
			return {
				...state,
				selectedDossierBuilding: {},
				selectedLoading: true
			};
		case dossiersConstants.GET_SELECTED_DOSSIERS_BUILDING_ERROR:
			if (action.error.message === 'canceled') {
				return {
					...state,
					selectedDossierBuilding: {},
				}
			}
			return {
				...state,
				selectedDossierBuilding: {},
				selectedLoading: false,
				selectedError: action.error
			};
		case dossiersConstants.GET_SELECTED_DOSSIERS_BUILDING_SUCCESS:
			let selectedDossier = Object.assign({}, state.selectedDossier);

			if (!selectedDossier.id || action.viewAs === 'building') {
				const buildingIndex = state.buildingList.findIndex(b => b.buildingId === action.data.buildingId);
				const selectedDossierIndex = state.buildingList[buildingIndex].dossierList.findIndex(d => d.id === action.dossiers.id);
				if (selectedDossierIndex >= 0)
					state.buildingList[buildingIndex].dossierList[selectedDossierIndex].status = action.dossiers.buildingInfoList[0].status;
				return {
					...state,
					selectedDossier: action.dossiers,
					selectedLoading: false,
					selectedError: null
				}
			}
			const index = selectedDossier.buildingInfoList.findIndex(b => b.buildingId === action.data.buildingId);
			if (index < 0) return {
				...state,
				selectedDossier,
				selectedLoading: false,
				selectedError: null
			};
			selectedDossier = {
				...selectedDossier,
			};
			selectedDossier.buildingInfoList[index] = { ...action.dossiers.buildingInfoList[0] };
			return {
				...state,
				selectedDossier,
				selectedDossierBuilding: action.dossiers.buildingInfoList[0],
				selectedLoading: false,
				selectedError: null
			};
		case dossiersConstants.GET_AVAILABLE_ROLES_ERROR:
			return {
				...state,
				availableRoles: [],
				rolesLoading: false,
				rolesError: action.error
			};
		case dossiersConstants.GET_AVAILABLE_ROLES_LOADING:
			return {
				...state,
				availableRoles: [],
				rolesLoading: true,
				rolesError: null
			};
		case dossiersConstants.GET_AVAILABLE_ROLES_SUCCESS:
			return {
				...state,
				availableRoles: action.roles,
				rolesLoading: false,
				rolesError: null
			};
		case dossiersConstants.ADD_DOSSIER_LOADING:
			return {
				...state,
				addLoading: true,
			};
		case dossiersConstants.ADD_DOSSIER_ERROR:
			return {
				...state,
				addLoading: false,
			};
		case dossiersConstants.ADD_DOSSIER_SUCCESS:
			const allDossiers = Object.assign({}, state.dossiers);
			let dossier = action.dossier;
			const remove = (key) => {
				allDossiers[key] = allDossiers[key].filter(p => p.id !== dossier.id);
			};
			const addOrUpdate = (key) => {
				let dossierIndex = allDossiers[key].findIndex(p => p.id === dossier.id);
				if (dossierIndex >= 0) {
					allDossiers[key][dossierIndex] = {
						...allDossiers[key][dossierIndex],
						...dossier,
					}
				} else allDossiers[key].unshift(dossier);
			};
			const key = !dossier.isDraft ? 'openOrClosedDossiers' : 'draftDossiers';
			addOrUpdate(key);
			if (key === 'openOrClosedDossiers') remove('draftDossiers');
			else remove('openOrClosedDossiers');
			return {
				...state,
				dossiers: allDossiers,
				addLoading: false,
			};
		case dossiersConstants.UPDATE_STATUS_LOADING:
			return {
				...state,
				isUpdateStatus: true,
			};
		case dossiersConstants.UPDATE_STATUS_ERROR:
			console.log(action)
			return {
				...state,
				isUpdateStatus: false,
			};
		case dossiersConstants.UPDATE_STATUS_SUCCESS:
			const indexOfDossier = state.dossiers.openOrClosedDossiers.findIndex(d => d.id === action.status.dossierId);
			const status = action.status.isClosed ? 2 : 1;
			const closedOn = status === 2 ? new Date() : null;
			if (indexOfDossier >= 0 && !action.status.buildingId) {
				state.dossiers.openOrClosedDossiers[indexOfDossier] = {
					...state.dossiers.openOrClosedDossiers[indexOfDossier],
					status,
				}
				state.selectedDossier = {
					...state.selectedDossier,
					status,
					closedOn,
				}
			} else if (action.status.buildingId) {
				// const  selectedOpenOrClosedDossier
				const indexOfBuilding = state.selectedDossier.buildingInfoList.findIndex(b => b.buildingId === action.status.buildingId);
				if (indexOfBuilding >= 0) {
					state.selectedDossier.buildingInfoList[indexOfBuilding] = {
						...state.selectedDossier.buildingInfoList[indexOfBuilding],
						status,
						closedOn
					}
				}
				if (action.status.type === 'building') {
					const buildingIndex = state.buildingList.findIndex(b => b.buildingId === action.status.buildingId);
					const selectedDossierIndex = state.buildingList[buildingIndex].dossierList.findIndex(d => d.id === state.selectedDossier.id);
					if (selectedDossierIndex >= 0)
						state.buildingList[buildingIndex].dossierList[selectedDossierIndex].status = status;
				}
				const isOpenDossier = state.selectedDossier.buildingInfoList.some(b => b.status === 1);
				state.selectedDossier = {
					...state.selectedDossier,
					status: !isOpenDossier ? 2 : 1
				}
				state.dossiers.openOrClosedDossiers[indexOfDossier] = {
					...state.dossiers.openOrClosedDossiers[indexOfDossier],
					status: !isOpenDossier ? 2 : 1
				}

			}
			return {
				...state,
				// dossiers: {
				// 	...state.dossiers,
				// 	openOrClosedDossiers
				// },
				selectedDossier: { ...state.selectedDossier },
				isUpdateStatus: false,
			};
		case dossiersConstants.UPDATE_DOSSIER_INFORMATION_LOADING:
			return {
				...state,
				updateLoading: true,
			};
		case dossiersConstants.UPDATE_DOSSIER_INFORMATION_ERROR:
			return {
				...state,
				updateLoading: false,
			};
		case dossiersConstants.UPDATE_DOSSIER_INFORMATION_SUCCESS:
			const keys = {
				generalinformation: 'generalInformation',
				status: 'status',
				archive: 'isArchived',
				deadline: 'deadline',
				buildingId: 'buildingId',
				extern: 'extern',
			};
			const values = Object.keys(action.dossier).reduce((p, c) => ({ ...p, [keys[c]]: action.dossier[c] }), {});
			if (action.dossier.buildingId) {
				let selectedDossierData = { ...state.selectedDossier };
				let startIndex = selectedDossierData.buildingInfoList.findIndex(b => b.buildingId === action.dossier.buildingId);
				if (startIndex >= 0) {
					selectedDossierData.buildingInfoList[startIndex] = {
						...selectedDossierData.buildingInfoList[startIndex],
						...values
					}
					return {
						...state,
						updateLoading: false,
						selectedDossier: selectedDossierData
					}
				}
			}

			let selectedDossierData = { ...state.selectedDossier, ...values };
			if (Object.keys(action.dossier).includes(keys.deadline)) {

				let startIndex = state.dossiers.openOrClosedDossiers.findIndex(p => p.id === selectedDossierData.id);
				if (startIndex >= 0)
					state.dossiers.openOrClosedDossiers[startIndex] = {
						...state.dossiers.openOrClosedDossiers[startIndex],
						deadline: action.dossier.deadline
					}

			}
			if (Object.keys(action.dossier).includes(keys.extern)) {
				values.isExternal = action.dossier.extern === 'true';
				let startIndex = state.dossiers.openOrClosedDossiers.findIndex(p => p.id === selectedDossierData.id);
				if (startIndex >= 0)
					state.dossiers.openOrClosedDossiers[startIndex] = {
						...state.dossiers.openOrClosedDossiers[startIndex],
						isExternal: values.isExternal
					}
			}
			if (action.dossier.archive) {
				values.isArchived = action.dossier.archive === 'true';
				let key = action.dossier.archive === 'true' ? 'openOrClosedDossiers' : 'archiveDossiers';
				let startIndex = state.dossiers[key].findIndex(p => p.id === selectedDossierData.id);
				let dossierData = state.dossiers[key][startIndex];
				if (action.dossier.archive === 'true') {
					if (startIndex >= 0) {
						state.dossiers.archiveDossiers.push(dossierData);
						state.dossiers.openOrClosedDossiers.splice(startIndex, 1)
					}
				} else if (action.dossier.archive === 'false') {
					state.dossiers.openOrClosedDossiers.push(dossierData);
					state.dossiers.archiveDossiers.splice(startIndex, 1)
				}
			}
			return {
				...state,
				updateLoading: false,
				selectedDossier: { ...state.selectedDossier, ...values }
			};
		case dossiersConstants.DELETE_DOSSIER_ERROR:
			return {
				...state,
				deleteError: action.error,
				deleteLoading: false
			};
		case dossiersConstants.DELETE_DOSSIER_LOADING:
			return {
				...state,
				deleteLoading: true
			};
		case dossiersConstants.DELETE_DOSSIER_SUCCESS:
			const draftDossiers = state.dossiers.draftDossiers.filter(d => d.id !== action.dossier.id);
			return {
				...state,
				dossiers: {
					...state.dossiers, draftDossiers
				},
				deleteLoading: false
			};
		case dossiersConstants.GET_IMAGES_LOADING:
			return {
				...state,
				isGettingImages: true
			};
		case dossiersConstants.GET_IMAGES_SUCCESS:
			return {
				...state,
				images: action.images,
				isGettingImages: false
			};
		case dossiersConstants.GET_IMAGES_ERROR:
			return {
				...state,
				images: [],
				imageError: action.error,
				isGettingImages: false
			};
		case dossiersConstants.UPDATE_DOSSIER_RIGHTS_LOADING:
			return {
				...state,
				isUpdatingRights: true
			};
		case dossiersConstants.UPDATE_DOSSIER_RIGHTS_SUCCESS:
			state.selectedDossier.userList = action.userList;
			return {
				...state,
				isUpdatingRights: false
			};
		case dossiersConstants.UPDATE_DOSSIER_RIGHTS_ERROR:
			return {
				...state,
				isUpdatingError: action.error,
				isUpdatingRights: false
			};
		case dossiersConstants.GET_BACKGROUND_IMAGE_LOADING:
			return {
				...state,
				selectedBackground: '',
				getBackgroundLoading: true
			};
		case dossiersConstants.GET_BACKGROUND_IMAGE_ERROR:
			return {
				...state,
				getBackgroundLoading: false,
				selectedBackground: '',
			};
		case dossiersConstants.UPDATE_DOSSIER_BUILDING_ERROR:
			return {
				...state,
				updateLoading: false,
			};
		case dossiersConstants.UPDATE_DOSSIER_BUILDING_LOADING:
			return {
				...state,
				updateLoading: true
			};
		case dossiersConstants.UPDATE_DOSSIER_BUILDING_SUCCESS:
			state.selectedDossier.buildingInfoList = action.objects;
			//let dossierIndex = state.dossiers.openOrClosedDossiers.findIndex(d => d.id === state.selectedDossier.id);
			//const isVisibleToBuyer = state.selectedDossier.buildingInfoList.some(b => b.isVisibleToBuyer);
			//if (dossierIndex >= 0) {
			//	state.dossiers.openOrClosedDossiers[dossierIndex] = {
			//		...state.dossiers.openOrClosedDossiers[dossierIndex],
			//		isVisibleToBuyer
			//	}
			//}
			return {
				...state,
				updateLoading: false
			};
		case dossiersConstants.GET_BACKGROUND_IMAGE_SUCCESS:
			return {
				...state,
				getBackgroundLoading: false,
				selectedBackground: action.image || [],
			};
		case dossiersConstants.MOVE_FILES_SUCCESS:
			const user = JSON.parse(getUser());
			const fileKey = action.location.key === 'isArchived' ? 'archivedFiles' : action.location.key === 'isDeleted' ? 'deletedFiles' : 'uploadedFiles';
			const prevFileKey = action.location.subType === 'uploaded' ? 'uploadedFiles' : action.location.subType === 'archived' ? 'archivedFiles' : 'deletedFiles';
			const dossiersData = Object.assign({}, state.selectedDossier);
			if (action.location.viewType !== action.location.moveToViewType) {
				if (action.location.moveTo) {
					if (action.location.moveToViewType === 'object') {
						let objectParentKey = action.location.moveTo === 'internal' ? 'internalObjectFiles' : 'externalObjectFiles';
						let dossierKey = action.location.type === 'internal' ? 'internalFiles' : 'externalFiles';
						const buildingIndex = dossiersData.buildingInfoList.findIndex(b => b.buildingId === action.location.buildingId);
						if (buildingIndex >= 0) {
							action.files.dossierFileList.map((file, i) => {
								let isExist = dossiersData[dossierKey][prevFileKey].findIndex(f => f.fileId === file.dossierFileId);
								if (!dossiersData.buildingInfoList[buildingIndex][objectParentKey]) {
									dossiersData.buildingInfoList[buildingIndex][objectParentKey] = {
										[fileKey]: [{ ...dossiersData[dossierKey][prevFileKey][isExist] }],
										hasUpdates: true
									}
									dossiersData[dossierKey][prevFileKey].splice(isExist, 1);

								}
								if (isExist >= 0) {
									dossiersData.buildingInfoList[buildingIndex][objectParentKey][fileKey].push({
										...dossiersData[dossierKey][prevFileKey][isExist],
										lastModifiedBy: user && user.name,
										lastModifiedOn: (new Date()).toJSON(),
										hasUpdates: true
									});
									dossiersData[dossierKey][prevFileKey].splice(isExist, 1);
								}
							})
						}
					}
					else {
						let objectParentKey = action.location.type === 'internal' ? 'internalObjectFiles' : 'externalObjectFiles';
						let dossierKey = action.location.moveTo === 'internal' ? 'internalFiles' : 'externalFiles';
						const buildingIndex = dossiersData.buildingInfoList.findIndex(b => b.buildingId === action.location.buildingId);
						if (buildingIndex >= 0) {
							action.files.dossierFileList.map((file, i) => {
								let isExist = dossiersData.buildingInfoList[buildingIndex][objectParentKey][prevFileKey].findIndex(f => f.fileId === file.dossierFileId);
								if (!dossiersData[dossierKey]) {
									dossiersData[dossierKey] = {
										[fileKey]: [{ ...dossiersData.buildingInfoList[buildingIndex][objectParentKey][prevFileKey][isExist] }],
										hasUpdates: true
									}
									dossiersData.buildingInfoList[buildingIndex][objectParentKey][prevFileKey].splice(isExist, 1);

								}
								if (isExist >= 0) {
									dossiersData[dossierKey][fileKey].push({
										...dossiersData.buildingInfoList[buildingIndex][objectParentKey][prevFileKey][isExist],
										lastModifiedBy: user && user.name,
										lastModifiedOn: (new Date()).toJSON(),
										hasUpdates: true
									});
									dossiersData.buildingInfoList[buildingIndex][objectParentKey][prevFileKey].splice(isExist, 1);
								}
							})
						}
					}
				} else {
					if (action.location.moveToViewType === 'object') {
						let objectParentKey = action.location.type === 'internal' ? 'internalObjectFiles' : 'externalObjectFiles';
						let dossierKey = action.location.type === 'internal' ? 'internalFiles' : 'externalFiles';
						const buildingIndex = dossiersData.buildingInfoList.findIndex(b => b.buildingId === action.location.buildingId);
						if (buildingIndex >= 0) {
							action.files.dossierFileList.map((file, i) => {
								let isExist = dossiersData[dossierKey][prevFileKey].findIndex(f => f.fileId === file.dossierFileId);
								if (!dossiersData.buildingInfoList[buildingIndex][objectParentKey]) {
									dossiersData.buildingInfoList[buildingIndex][objectParentKey] = {
										[fileKey]: [{ ...dossiersData[dossierKey][prevFileKey][isExist] }],
										hasUpdates: true
									}
									dossiersData[dossierKey][prevFileKey].splice(isExist, 1);

								}
								if (isExist >= 0) {
									dossiersData.buildingInfoList[buildingIndex][objectParentKey][fileKey].push({
										...dossiersData[dossierKey][prevFileKey][isExist],
										lastModifiedBy: user && user.name,
										lastModifiedOn: (new Date()).toJSON(),
										hasUpdates: true
									});
									dossiersData[dossierKey][prevFileKey].splice(isExist, 1);
								}
							})
						}
					}
					else {
						let objectParentKey = action.location.type === 'internal' ? 'internalObjectFiles' : 'externalObjectFiles';
						let dossierKey = action.location.type === 'internal' ? 'internalFiles' : 'externalFiles';
						const buildingIndex = dossiersData.buildingInfoList.findIndex(b => b.buildingId === action.location.buildingId);
						if (buildingIndex >= 0) {
							action.files.dossierFileList.map((file, i) => {
								let isExist = dossiersData.buildingInfoList[buildingIndex][objectParentKey][prevFileKey].findIndex(f => f.fileId === file.dossierFileId);
								if (!dossiersData[dossierKey]) {
									dossiersData[dossierKey] = {
										[fileKey]: [{ ...dossiersData.buildingInfoList[buildingIndex][objectParentKey][prevFileKey][isExist] }],
										hasUpdates: true
									}
									dossiersData.buildingInfoList[buildingIndex][objectParentKey][prevFileKey].splice(isExist, 1);

								}
								if (isExist >= 0) {
									dossiersData[dossierKey][fileKey].push({
										...dossiersData.buildingInfoList[buildingIndex][objectParentKey][prevFileKey][isExist],
										lastModifiedBy: user && user.name,
										lastModifiedOn: (new Date()).toJSON(),
										hasUpdates: true
									});
									dossiersData.buildingInfoList[buildingIndex][objectParentKey][prevFileKey].splice(isExist, 1);
								}
							})
						}
					}
					// if (action.location.moveToViewType === 'object') {
					// 	let objectParentKey = action.location.type === 'internal' ? 'internalObjectFiles' : 'externalObjectFiles';
					// 	let dossierKey = action.location.type === 'internal' ? 'internalFiles' : 'externalFiles';
					// 	const buildingIndex = dossiersData.buildingInfoList.findIndex(b => b.buildingId === action.location.buildingId);
					// 	console.log(buildingIndex,objectParentKey,dossierKey)
					// 	if (buildingIndex >= 0) {
					// 		action.files.dossierFileList.map((file, i) => {
					// 			let isExist = dossiersData[dossierKey][prevFileKey].findIndex(f => f.fileId === file.dossierFileId);
					// 			if (isExist>=0) {
					// 				if (!dossiersData.buildingInfoList[buildingIndex][objectParentKey]) {
					// 					dossiersData.buildingInfoList[buildingIndex][objectParentKey] = {
					// 						[fileKey]: [{ ...dossiersData[dossierKey][prevFileKey][isExist] }]
					// 					}
					// 					dossiersData[dossierKey][prevFileKey].splice(isExist, 1);
					// 				}
					// 			}
					// 		})
					// 	}
					// }
				}

			}
			else if (action.location.buildingId) {
				const buildingIndex = dossiersData.buildingInfoList.findIndex(b => b.buildingId === action.location.buildingId);
				if (action.location.moveTo) {
					let parentKey = action.location.moveTo === 'internal' ? 'internalObjectFiles' : 'externalObjectFiles';
					let prevParentKey = parentKey === 'internalObjectFiles' ? 'externalObjectFiles' : 'internalObjectFiles';

					const buildingIndex = dossiersData.buildingInfoList.findIndex(b => b.buildingId === action.location.buildingId);
					if (buildingIndex >= 0) {
						action.files.dossierFileList.map((file, i) => {
							let isExist = dossiersData.buildingInfoList[buildingIndex][prevParentKey][prevFileKey].findIndex(f => f.fileId === file.dossierFileId);
							if (!dossiersData.buildingInfoList[buildingIndex][parentKey]) {
								dossiersData.buildingInfoList[buildingIndex][parentKey] = {
									[fileKey]: [{ ...dossiersData.buildingInfoList[buildingIndex][prevParentKey][prevFileKey][isExist] }],
									hasUpdates: true
								}
								dossiersData.buildingInfoList[buildingIndex][prevParentKey][prevFileKey].splice(isExist, 1);

							}
							if (isExist >= 0) {
								dossiersData.buildingInfoList[buildingIndex][parentKey][fileKey].push({
									...dossiersData.buildingInfoList[buildingIndex][prevParentKey][prevFileKey][isExist],
									lastModifiedBy: user && user.name,
									lastModifiedOn: (new Date()).toJSON(),
									hasUpdates: true
								});
								dossiersData.buildingInfoList[buildingIndex][prevParentKey][prevFileKey].splice(isExist, 1);
							}
						});
					}
				}
				else if (action.location.key) {
					let parentKey = action.location.type === 'external' ? 'externalObjectFiles' : 'internalObjectFiles';
					action.files.dossierFileList.map((file, i) => {
						const buildingIndex = dossiersData.buildingInfoList.findIndex(b => b.buildingId === action.location.buildingId);
						if (buildingIndex >= 0) {
							let isExist = dossiersData.buildingInfoList[buildingIndex][parentKey][prevFileKey].findIndex(f => f.fileId === file.dossierFileId);
							if (isExist >= 0) {
								dossiersData.buildingInfoList[buildingIndex][parentKey][fileKey].push({
									...dossiersData.buildingInfoList[buildingIndex][parentKey][prevFileKey][isExist],
									lastModifiedBy: user && user.name,
									lastModifiedOn: (new Date()).toJSON(),
									hasUpdates: true
								});
								dossiersData.buildingInfoList[buildingIndex][parentKey][prevFileKey].splice(isExist, 1);
							}
						}
					});
				} else {
					let parentKey = action.location.type === 'external' ? 'internalObjectFiles' : 'externalObjectFiles';
					let prevParentKey = parentKey === 'internalObjectFiles' ? 'externalObjectFiles' : 'internalObjectFiles';
					action.files.dossierFileList.map((file, i) => {
						if (buildingIndex >= 0) {
							let isExist = dossiersData.buildingInfoList[buildingIndex][prevParentKey].uploadedFiles.findIndex(f => f.fileId === file.dossierFileId);
							if (!dossiersData.buildingInfoList[buildingIndex][parentKey]) {
								dossiersData.buildingInfoList[buildingIndex][parentKey] = {
									uploadedFiles: [{ ...dossiersData.buildingInfoList[buildingIndex][prevParentKey].uploadedFiles[isExist] }],
									hasUpdates: true
								}
								dossiersData.buildingInfoList[buildingIndex][prevParentKey].uploadedFiles.splice(isExist, 1);

							}
							if (isExist >= 0) {
								dossiersData.buildingInfoList[buildingIndex][parentKey].uploadedFiles.push({
									...dossiersData.buildingInfoList[buildingIndex][prevParentKey].uploadedFiles[isExist],
									lastModifiedBy: user && user.name,
									lastModifiedOn: (new Date()).toJSON(),
									hasUpdates: true
								});
								dossiersData.buildingInfoList[buildingIndex][prevParentKey].uploadedFiles.splice(isExist, 1);
							}
						}
					});
				}
				dossiersData.buildingInfoList[buildingIndex].hasUpdates = true;
			}
			else {
				if (action.location.moveTo) {
					let parentKey = action.location.moveTo === 'internal' ? 'internalFiles' : 'externalFiles';
					let prevParentKey = parentKey === 'internalFiles' ? 'externalFiles' : 'internalFiles';
					action.files.dossierFileList.forEach((file, i) => {
						let isExist = dossiersData[prevParentKey][prevFileKey].findIndex(f => f.fileId === file.dossierFileId);
						if (!dossiersData[parentKey]) {
							dossiersData[parentKey] = {
								[fileKey]: [{ ...dossiersData[prevParentKey][prevFileKey][isExist] }],
								hasUpdates: true
							}
							dossiersData[prevParentKey][prevFileKey].splice(isExist, 1);

						} else {
							if (isExist >= 0) {
								dossiersData[parentKey][fileKey].push({
									...dossiersData[prevParentKey][prevFileKey][isExist],
									lastModifiedBy: user && user.name,
									lastModifiedOn: (new Date()).toJSON(),
									hasUpdates: true
								});
								dossiersData[prevParentKey][prevFileKey].splice(isExist, 1);
							}
						}

					})
				}
				else if (action.location.key) {
					let parentKey = action.location.type === 'external' ? 'externalFiles' : 'internalFiles';
					action.files.dossierFileList.forEach((file, i) => {
						let isExist = dossiersData[parentKey][prevFileKey].findIndex(f => f.fileId === file.dossierFileId);
						if (isExist >= 0) {
							dossiersData[parentKey][fileKey].push({
								...dossiersData[parentKey][prevFileKey][isExist],
								lastModifiedBy: user && user.name,
								lastModifiedOn: (new Date()).toJSON(),
								hasUpdates: true
							});
							dossiersData[parentKey][prevFileKey].splice(isExist, 1);
						}
					});
				} else {
					let parentKey = action.location.type === 'external' ? 'internalFiles' : 'externalFiles';
					let prevParentKey = parentKey === 'internalFiles' ? 'externalFiles' : 'internalFiles';
					action.files.dossierFileList.forEach((file, i) => {
						let isExist = dossiersData[prevParentKey].uploadedFiles.findIndex(f => f.fileId === file.dossierFileId);
						if (!dossiersData[parentKey]) {
							dossiersData[parentKey] = {
								uploadedFiles: [{ ...dossiersData[prevParentKey].uploadedFiles[isExist] }],
								hasUpdates: true
							}
							dossiersData[prevParentKey].uploadedFiles.splice(isExist, 1);

						} else {
							if (isExist >= 0) {
								dossiersData[parentKey].uploadedFiles.push({
									...dossiersData[prevParentKey].uploadedFiles[isExist],
									lastModifiedBy: user && user.name,
									lastModifiedOn: (new Date()).toJSON(),
									hasUpdates: true
								});
								dossiersData[prevParentKey].uploadedFiles.splice(isExist, 1);
							}
						}

					})
				}
			}
			const selectedDossierIndex = state.dossiers.openOrClosedDossiers.findIndex(p => p.id === state.selectedDossier.id);
			if (selectedDossierIndex >= 0)
				state.dossiers.openOrClosedDossiers[selectedDossierIndex].hasUpdates = true;
			return {
				...state,
				selectedDossier: dossiersData,
				isMoving: false
			};
		case dossiersConstants.LINK_FILE_ERROR:

			return {
				...state,
			};
		case dossiersConstants.MOVE_FILES_LOADING:
			return {
				...state,
				isMoving: true
			};
		case dossiersConstants.MOVE_FILES_ERROR:
			return {
				...state,
				isMoveError: action.error,
				isMoving: false
			};
		case dossiersConstants.UPLOAD_FILES_LOADING:
			return {
				...state,
				isUploading: true
			};
		case dossiersConstants.UPLOAD_FILES_ERROR:
			return {
				...state,
				isUploading: false
			};
		case dossiersConstants.UPLOAD_FILES_SUCCESS: {
			const selectedDossier = Object.assign({}, state.selectedDossier);
			if (action.location.buildingId) {
				const building = selectedDossier.buildingInfoList.find(b => b.buildingId === action.location.buildingId);
				if (building) {
					if (!building[action.location.key]) {
						building[action.location.key] = { uploadedFiles: action.files };
					} else {
						building[action.location.key].uploadedFiles = building[action.location.key].uploadedFiles.concat(action.files);
					}
					building.hasUpdates = true;
				}
			} else if (!selectedDossier[action.location.key]) {
				selectedDossier[action.location.key] = { uploadedFiles: action.files };
			} else {
				selectedDossier[action.location.key].uploadedFiles = selectedDossier[action.location.key].uploadedFiles.concat(action.files);
			}
			const openDossierIndex = state.dossiers.openOrClosedDossiers.findIndex(p => p.id === selectedDossier.id);
			if (openDossierIndex >= 0)
				state.dossiers.openOrClosedDossiers[openDossierIndex].hasUpdates = true;
			return {
				...state,
				selectedDossier,
				isUploading: false
			};
		}
		case dossiersConstants.UPDATE_DOSSIER_LAST_VIEW_LOADING:
			return {
				...state,
				//updatingRead: true
			};
		case dossiersConstants.UPDATE_DOSSIER_LAST_VIEW_ERROR:
			return {
				...state,
				//updatingRead: false
			};
		case dossiersConstants.UPDATE_DOSSIER_LAST_VIEW_SUCCESS: {
			const dossierData = Object.assign({}, state.selectedDossier);
			const dossierBuilding = Object.assign({}, state.selectedDossierBuilding);
			if (action.data.dossierId == dossierData.id) {
				if (action.data.dossierFileId) {
					const fileKey = action.location.subType === 'uploaded' ? 'uploadedFiles' : action.location.subType === 'archived' ? 'archivedFiles' : 'deletedFiles';
					if (action.data.buildingId) {
						let parentKey = action.location.type === 'external' ? 'externalObjectFiles' : 'internalObjectFiles';
						let dossierParentKey = action.location.type === 'external' ? 'externalFiles' : 'externalFiles';
						const buildingIndex = dossierData.buildingInfoList.findIndex(b => b.buildingId === action.data.buildingId);
						if (buildingIndex >= 0) {
							let isExist = dossierData.buildingInfoList[buildingIndex][parentKey][fileKey].findIndex(f => f.fileId === action.data.dossierFileId);
							if (action.location.isBuyer && action.location.isGeneralFile) {
								isExist = dossierData[dossierParentKey][fileKey].findIndex(f => f.fileId === action.data.dossierFileId);
								if (isExist >= 0) dossierData[dossierParentKey][fileKey][isExist].hasUpdates = false;
							}
							else if (isExist >= 0) dossierData.buildingInfoList[buildingIndex][parentKey][fileKey][isExist].hasUpdates = false;
							const { internalFiles, externalFiles } = dossierData;
							const { internalObjectFiles, externalObjectFiles } = dossierData.buildingInfoList[buildingIndex];
							var othersHaveUpdates =
								(
									internalObjectFiles
									&& (
										(internalObjectFiles.uploadedFiles && internalObjectFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
										|| (internalObjectFiles.uploadedFiles && internalObjectFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
										|| (internalObjectFiles.uploadedFiles && internalObjectFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
									)
								)
								||
								(
									externalObjectFiles
									&& (
										(externalObjectFiles.uploadedFiles && externalObjectFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
										|| (externalObjectFiles.uploadedFiles && externalObjectFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
										|| (externalObjectFiles.uploadedFiles && externalObjectFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
									)
								)
								||
								(
									internalFiles
									&& (
										(internalFiles.uploadedFiles && internalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
										|| (internalFiles.uploadedFiles && internalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
										|| (internalFiles.uploadedFiles && internalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
									)
								)
								||
								(
									externalFiles
									&& (
										(externalFiles.uploadedFiles && externalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
										|| (externalFiles.uploadedFiles && externalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
										|| (externalFiles.uploadedFiles && externalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
									)
								);

							if (!othersHaveUpdates) {
								dossierData.buildingInfoList[buildingIndex].hasUpdates = false;
							}
						}
					}
					else {
						let parentKey = action.location.type === 'external' ? 'externalFiles' : 'internalFiles';

						let isExist = dossierData[parentKey][fileKey].findIndex(f => f.fileId === action.data.dossierFileId);
						if (isExist >= 0) {
							dossierData[parentKey][fileKey][isExist].hasUpdates = false;

							const { internalFiles, externalFiles } = dossierData;

							const buildingIndex = dossierData.buildingInfoList.findIndex(b => dossierBuilding && b.buildingId === dossierBuilding.buildingId && b.buildingId === action.location.buildingId);
							if (buildingIndex >= 0) {
								const { internalObjectFiles, externalObjectFiles } = dossierData.buildingInfoList[buildingIndex];
								var othersHaveUpdates =
									(
										internalObjectFiles
										&& (
											(internalObjectFiles.uploadedFiles && internalObjectFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
											|| (internalObjectFiles.uploadedFiles && internalObjectFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
											|| (internalObjectFiles.uploadedFiles && internalObjectFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
										)
									)
									||
									(
										externalObjectFiles
										&& (
											(externalObjectFiles.uploadedFiles && externalObjectFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
											|| (externalObjectFiles.uploadedFiles && externalObjectFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
											|| (externalObjectFiles.uploadedFiles && externalObjectFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
										)
									)
									||
									(
										internalFiles
										&& (
											(internalFiles.uploadedFiles && internalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
											|| (internalFiles.uploadedFiles && internalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
											|| (internalFiles.uploadedFiles && internalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
										)
									)
									||
									(
										externalFiles
										&& (
											(externalFiles.uploadedFiles && externalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
											|| (externalFiles.uploadedFiles && externalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
											|| (externalFiles.uploadedFiles && externalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
										)
									);

								if (!othersHaveUpdates) {
									dossierData.buildingInfoList[buildingIndex].hasUpdates = false;
								}
							}
							else {
								var othersHaveUpdates =
									(
										internalFiles
										&& (
											(internalFiles.uploadedFiles && internalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
											|| (internalFiles.uploadedFiles && internalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
											|| (internalFiles.uploadedFiles && internalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
										)
									)
									||
									(
										externalFiles
										&& (
											(externalFiles.uploadedFiles && externalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
											|| (externalFiles.uploadedFiles && externalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
											|| (externalFiles.uploadedFiles && externalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
										)
									);

								if (!othersHaveUpdates) {
									dossierData.hasUpdates = false;
								}
							}
						}
					}
				}
				else if (action.data.buildingId) {
					const buildingIndex = dossierData.buildingInfoList.findIndex(b => b.buildingId === action.data.buildingId);
					if (buildingIndex >= 0) {
						const { internalFiles, externalFiles } = dossierData;
						const { internalObjectFiles, externalObjectFiles } = dossierData.buildingInfoList[buildingIndex];
						var othersHaveUpdates =
							(
								internalObjectFiles
								&& (
									(internalObjectFiles.uploadedFiles && internalObjectFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
									|| (internalObjectFiles.uploadedFiles && internalObjectFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
									|| (internalObjectFiles.uploadedFiles && internalObjectFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
								)
							)
							||
							(
								externalObjectFiles
								&& (
									(externalObjectFiles.uploadedFiles && externalObjectFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
									|| (externalObjectFiles.uploadedFiles && externalObjectFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
									|| (externalObjectFiles.uploadedFiles && externalObjectFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
								)
							)
							||
							(
								internalFiles
								&& (
									(internalFiles.uploadedFiles && internalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
									|| (internalFiles.uploadedFiles && internalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
									|| (internalFiles.uploadedFiles && internalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
								)
							)
							||
							(
								externalFiles
								&& (
									(externalFiles.uploadedFiles && externalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
									|| (externalFiles.uploadedFiles && externalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
									|| (externalFiles.uploadedFiles && externalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
								)
							);

						if (!othersHaveUpdates) {
							dossierData.buildingInfoList[buildingIndex].hasUpdates = false;
						}
					}
				}
				else if (action.data.dossierId) {
					const { internalFiles, externalFiles } = dossierData;
					var othersHaveUpdates =
						(
							internalFiles
							&& (
								(internalFiles.uploadedFiles && internalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
								|| (internalFiles.uploadedFiles && internalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
								|| (internalFiles.uploadedFiles && internalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
							)
						)
						||
						(
							externalFiles
							&& (
								(externalFiles.uploadedFiles && externalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
								|| (externalFiles.uploadedFiles && externalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
								|| (externalFiles.uploadedFiles && externalFiles.uploadedFiles.filter(x => x.hasUpdates === true).length > 0)
							)
						);

					if (!othersHaveUpdates) {
						dossierData.hasUpdates = false;
					}
				}
			}

			var hasUpdates = dossierData.buildingInfoList && dossierData.buildingInfoList.filter(x => x.hasUpdates).length > 0;
			const dossiers = Object.assign({}, state.dossiers);
			if ((action.location.isBuyer || !dossierData.hasUpdates) && !hasUpdates) {
				if (dossiers && dossiers.openOrClosedDossiers && dossiers.openOrClosedDossiers.length >= 0) {
					var dossierIndex = dossiers.openOrClosedDossiers.findIndex(x => x.id == dossierData.id);
					if (dossierIndex >= 0) {
						dossierData.hasUpdates = false;
						dossiers.openOrClosedDossiers[dossierIndex].hasUpdates = false;
					}
				}
			}

			hasUpdates = dossierData.buildingInfoList && dossierData.buildingInfoList.filter(x => x.buildingId === action.location.buildingId && x.hasUpdates).length > 0;
			const buildingList = state.buildingList && state.buildingList.slice();
			if (!hasUpdates && buildingList && buildingList.length > 0 && action.location.buildingId) {
				var buildingIndex = buildingList.findIndex(b => b.buildingId === action.location.buildingId);
				if (buildingIndex >= 0 && buildingList[buildingIndex].dossierList) {
					var dossierIndex = buildingList[buildingIndex].dossierList.findIndex(x => x.id == dossierData.id);
					if (dossierIndex >= 0) {
						buildingList[buildingIndex].dossierList[dossierIndex].hasUpdates = false;
					}
				}
			}

			return {
				...state,
				dossiers,
				buildingList,
				selectedDossier: dossierData,
				//updatingRead: false
			};
		}
		case dossiersConstants.UPDATE_DOSSIER_DEADLINE_LOADING:
			return {
				...state,
				isDeadLineUpdating: true
			};
		case dossiersConstants.UPDATE_DOSSIER_DEADLINE_ERROR:
			return {
				...state,
				isDeadLineUpdating: false
			};
		case dossiersConstants.UPDATE_DOSSIER_DEADLINE_SUCCESS:
			const indexOfOpenDossier = state.dossiers.openOrClosedDossiers.findIndex(p => p.id === action.data.dossierId);
			if (indexOfOpenDossier >= 0)
				state.dossiers.openOrClosedDossiers[indexOfOpenDossier] = {
					...state.dossiers.openOrClosedDossiers[indexOfOpenDossier],
					deadline: action.data.deadlineDate
				}
			return {
				...state,
				selectedDossier: {
					...state.selectedDossier,
					deadline: action.data.deadlineDate,
				},
				isDeadLineUpdating: false
			};
		default:
			return state
	}
}
