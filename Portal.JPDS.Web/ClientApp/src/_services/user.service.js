import { getUser, authHeader, removeUser } from "../_helpers";

export const userService = {
    login,
    logout,
    getLoggedInUser,
    getViewAsUser,
    register,
    getAll,
    getById,
    update,
    delete: _delete,
    forgotPassword,
    resetPassword,
    changePassword
};

const { webApiUrl } = window.appConfig;

function login(email, password, remember) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password, remember })
    };
    return fetch(webApiUrl + 'api/users/authenticate', requestOptions)
        .then(handleResponse, handleError)
        .then(user => {
            if (user) {
                localStorage.setItem('user', JSON.stringify(user));
            }
            return user;
        });
}

function logout() {
    const requestOptions = {
        method: 'GET',
        headers: authHeader('application/json'),
    };
    fetch(webApiUrl + 'api/users/signout', requestOptions)

    removeUser();
}

function getLoggedInUser() {
    var localUser = getUser();
    let userFromStorage = localUser && JSON.parse(localUser);

    const requestOptions = {
        method: 'GET',
        headers: authHeader('application/json')
    };
    return fetch(webApiUrl + 'api/users/GetLoggedInUser', requestOptions)
        .then(handleResponse, handleError)
        .then(user => {
            if (user) {
                user.token = userFromStorage.token;
                if (userFromStorage.viewOnly !== true) {
                    localStorage.setItem('user', JSON.stringify(user));
                }
                else {
                    user.viewOnly = true;
                    sessionStorage.setItem('user', JSON.stringify(user));
                }
            }
            return user;
        });
}

function getViewAsUser(loginId) {
    const requestOptions = {
        method: 'GET',
        headers: authHeader('application/json')
    };
    return fetch(webApiUrl + 'api/users/GetViewAsUserInfo/' + loginId, requestOptions)
        .then(handleResponse, handleError)
        .then(user => {
            if (user) {
                user.viewOnly = true;
                sessionStorage.setItem('user', JSON.stringify(user));
            }
            return user;
        });
}

function getAll() {
    const requestOptions = {
        method: 'GET',

    };

    return fetch(webApiUrl + 'api/users', requestOptions).then(handleResponse, handleError);
}

function getById(id) {
    const requestOptions = {
        method: 'GET',

    };

    return fetch(webApiUrl + 'api/users/' + id, requestOptions).then(handleResponse, handleError);
}

function register(user) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(user)
    };

    return fetch(webApiUrl + 'api/users/register', requestOptions).then(handleResponse, handleError);
}

function update(user) {
    const requestOptions = {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(user)
    };

    return fetch(webApiUrl + 'api/users/' + user.id, requestOptions).then(handleResponse, handleError);
}

// prefixed function name with underscore because delete is a reserved word in javascript
function _delete(id) {
    const requestOptions = {
        method: 'DELETE',

    };

    return fetch(webApiUrl + 'api/users/' + id, requestOptions).then(handleResponse, handleError);
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
            // return error message from response body
            response.text().then(text => reject(text));
        }
    });
}

function handleError(error) {
    return Promise.reject(error && error.message);
}

function forgotPassword(email) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email })
    };
    return fetch(webApiUrl + 'api/users/RequestPasswordReset', requestOptions)
        .then(handleResponse, handleError)
        .then(emailSent => {
            return emailSent;
        });
}

function resetPassword(token, password) {
    var headers = {
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
    };

    const requestOptions = {
        method: 'POST',
        headers,
        body: JSON.stringify(password)
    };

    return fetch(webApiUrl + 'api/users/UpdatePassword', requestOptions)
        .then(handleResponse, handleError);
}

function changePassword(userPassword) {
    const requestOptions = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(userPassword),
    };
    return fetch(webApiUrl + "api/users/ChangePassword", requestOptions).then(
      handleResponseStatus,
      handleErrorStatus
    );
}

function handleResponseStatus(response) {
    return new Promise((resolve, reject) => {
      if (response.ok) {
        var contentType = response.headers.get("content-type");
        if (contentType && contentType.includes("application/json")) {
          response.json().then((json) => {
            resolve({ status: response.status, data: json });
          });
        } else {
          resolve({ status: response.status });
        }
      } else {
        // return error message from response body
        response.text().then((text) => reject(text));
      }
    });
}
  
function handleErrorStatus(response) {
    //return Promise.reject(error && error.message);
    return new Promise((resolve, reject) => {
        if (!response.ok) {
        resolve({ status: response.status });
        }
    });
}

