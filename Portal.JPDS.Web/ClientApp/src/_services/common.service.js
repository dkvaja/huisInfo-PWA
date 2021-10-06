import { history, authHeader, getUser } from '../_helpers';

export const commonService = {
    getBuildings,
    getDashboardCount
};

const { webApiUrl } = window.appConfig;

function getBuildings(app) {
    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };

    return fetch(webApiUrl + 'api/home/GetUserObjects/' + app, requestOptions).then(handleResponse, handleError);
}

function getDashboardCount(app, building) {
    const url = webApiUrl + 'api/home/GetDashboardCount?app=' + app + '&buildingId=' + building.buildingId + '&projectId=' + building.projectId;

    if (this.countAbortController && this.countAbortController.signal.aborted !== true) {
        this.countAbortController.abort();
    }

    this.countAbortController = new window.AbortController();
    const requestOptions = {
        method: 'GET',
        headers: authHeader(),
        signal: this.countAbortController.signal
    };

    return fetch(url, requestOptions).then(handleResponse, handleError).catch(err => {
        if (err.name === 'AbortError') {
            //handle aborterror here
        }
    });
}

function handleResponse(response) {
    return new Promise((resolve, reject) => {
        if (response.ok) {
            // return json if it was returned in the response
            var contentType = response.headers.get("content-type");
            if (contentType && contentType.includes("application/json")) {
                response.json().then(json => resolve(json));
            } else {
                resolve();
            }
        } else {
            if (response.status === 401) {
                var user = JSON.parse(getUser());
                if (user.viewOnly === true) {
                    alert('Deze sessie is verlopen.');
                    window.close();
                }
                else {
                    history.push('/login?session_timeout');
                }
            }
            // return error message from response body
            response.text().then(text => reject(text));
        }
    });
}

function handleError(error) {
    return Promise.reject(error && error.message);
}