import { commonConstants } from '../_constants';
import { commonService } from '../_services';
// import { history } from '../_helpers';
// import { dashboardCount } from '../_reducers/common.reducer';

export const commonActions = {
    getBuildings,
    selectApp,
    selectBuilding,
    getDashboardCount,
    updateRights,
    meldingenTableFilter
};

function getBuildings(app) {
    return dispatch => {
        dispatch(request());
        commonService.getBuildings(app)
            .then(
                buildings => dispatch(success(buildings)),
                error => dispatch(failure(error))
            );
    };

    function request() { return { type: commonConstants.GETALL_REQUEST } }
    function success(buildings) { return { type: commonConstants.GETALL_SUCCESS, buildings } }
    function failure(error) { return { type: commonConstants.GETALL_FAILURE, error } }
}

function selectApp(app) {
    return {
        type: commonConstants.CHANGE_APP_SUCCESS,
        app: app
    };
}

function meldingenTableFilter(list) {
    return {
        type: commonConstants.CHANGE_MELDINGEN_FILTER_SUCCESS,
        list
    };
}

function selectBuilding(building) {
    return {
        type: commonConstants.CHANGE_SUCCESS,
        building: building
    };
}

function updateRights(buildingId = null) {
    return {
        type: commonConstants.UPDATE_RIGHTS_SUCCESS,
        buildingId
    };
}

function getDashboardCount(app, building) {
    return dispatch => {
        commonService.getDashboardCount(app, building)
            .then(
                dashboardCount => {
                    if (dashboardCount) {
                        dispatch(success(dashboardCount))
                    }
                    else {
                        dispatch(failure('Undefined'))
                    }
                },
                error => dispatch(failure(error))
            );
    };

    function success(dashboardCount) { return { type: commonConstants.GETCOUNT_SUCCESS, dashboardCount } }
    function failure(error) {
        return { type: commonConstants.GETCOUNT_ERROR, error }
    }
}
