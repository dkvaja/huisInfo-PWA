import { commonConstants } from '../_constants';
import { getRights, getUser } from '../_helpers';
const initialState = {
    all: [],
    selected: '',
    loading: true,
    rights: {}
}

export function buildings(state = initialState, action) {
    const user = JSON.parse(getUser());

    switch (action.type) {
        case commonConstants.GETALL_REQUEST:
            return {
                ...state,
                all: [],
                selected: '',
                loading: true,
                buyerGuidesBuildings: []
            };
        case commonConstants.GETALL_SUCCESS:
            var prevSelected = localStorage.getItem('building');
            const building = prevSelected && prevSelected !== 'undefined' && JSON.parse(prevSelected);
            let selectedBuilding = null;
            const selectedBuildings = action.buildings.filter(x => building && x.buildingId === building.buildingId);
            if (selectedBuildings && selectedBuildings.length > 0) {
                selectedBuilding = selectedBuildings.find(b => b.roles && b.roles.some(r => r === 'BuyersGuide' || r === 'Spectator')) || selectedBuildings[0];
            } else {
                selectedBuilding = action.buildings.find(b => b.roles && b.roles.some(r => r === 'BuyersGuide' || r === 'Spectator')) || action.buildings[0];
            }
            const roles = (selectedBuilding && selectedBuilding.roles) ? selectedBuilding.roles : [user.type === 1 ? 'Buyer' : ''];
            const dossierRights = getRights(roles);
            return {
                ...state,
                all: action.buildings,
                selected: selectedBuilding,
                loading: false,
                rights: { ...state.rights, ...dossierRights },
                buyerGuidesBuildings: action.buildings.filter(b => b.roles && b.roles.some(r => r === 'BuyersGuide' || r === 'Spectator'))
            };
        case commonConstants.GETALL_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false
            };
        case commonConstants.CHANGE_SUCCESS: {
            const selectedBuilding = action.building;
            localStorage.setItem('building', JSON.stringify(selectedBuilding));
            const roles = selectedBuilding.roles ? selectedBuilding.roles : [user.type === 1 ? 'Buyer' : ''];
            const dossierRights = getRights(roles);
            return {
                ...state,
                selected: selectedBuilding,
                loading: false,
                rights: { ...state.rights, ...dossierRights }

            };
        }
        case commonConstants.UPDATE_RIGHTS_SUCCESS: {
            const { selected } = state;
            let selectedBuilding = null;
            if (!action.buildingId) {
                const selectedBuildings = state.all.filter(x => selected && x.buildingId === selected.buildingId);
                if (selectedBuildings && selectedBuildings.length > 0) {
                    selectedBuilding = selectedBuildings.find(b => b.roles && b.roles.some(r => r === 'BuyersGuide' || r === 'Spectator')) || selectedBuildings[0];
                } else {
                    selectedBuilding = state.all.find(b => b.roles && b.roles.some(r => r === 'BuyersGuide' || r === 'Spectator')) || action.buildings[0];
                }
            } else {
                selectedBuilding = state.all.find(x => x.buildingId === action.buildingId);
            }
            if (selectedBuilding) {
                const roles = selectedBuilding.roles ? selectedBuilding.roles : [user.type === 1 ? 'Buyer' : ''];
                const rights = getRights(roles, 'selected.object.write');
                return { ...state, rights: { ...state.rights, ...rights } }
            }
            return { ...state }
        }
        default:
            return state
    }
}


export function dashboardCount(state = {}, action) {
    switch (action.type) {
        case commonConstants.GETCOUNT_SUCCESS:
            return action.dashboardCount;
        default:
            return state
    }
}

export function app(state = null, action) {
    switch (action.type) {
        case commonConstants.CHANGE_APP_SUCCESS:
            localStorage.setItem('app', JSON.stringify(action.app));
            return action.app;
        default:
            return state;
    }
}

export function meldingenFilter(state = {}, action) {
    switch (action.type) {
        case commonConstants.CHANGE_MELDINGEN_FILTER_SUCCESS:
            return action.list;
        default:
            return state;
    }
}

