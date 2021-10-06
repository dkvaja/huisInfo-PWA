import { userConstants } from '../_constants';
import { userService } from '../_services';
import { alertActions } from './';
import { history } from '../_helpers';

export const userActions = {
    login,
    logout,
    getLoggedInUser,
    getViewAsUser,
    register,
    getAll,
    delete: _delete,
    forgotPassword,
    resetPassword
};

function login(email, password, remember) {
    return dispatch => {
        dispatch(request({ email }));

        userService.login(email, password, remember)
            .then(
                user => { 
                    dispatch(success(user));
                    //history.push('/');
                },
                error => {
                    dispatch(failure(error));
                    dispatch(alertActions.error(error));
                }
            );
    };

    function request(user) { return { type: userConstants.LOGIN_REQUEST, user } }
    function success(user) { return { type: userConstants.LOGIN_SUCCESS, user } }
    function failure(error) { return { type: userConstants.LOGIN_FAILURE, error } }
}

function logout() {
    userService.logout();
    return { type: userConstants.LOGOUT };
}

function getLoggedInUser() {
    return dispatch => {
        dispatch(request());
        userService.getLoggedInUser()
            .then(
                user => {
                    dispatch(success(user));
                },
                error => {
                    dispatch(failure(error));
                }
            );
    };

    function request(user) { return { type: userConstants.LOGIN_REQUEST, user } }
    function success(user) { return { type: userConstants.LOGIN_SUCCESS, user } }
    function failure(error) { return { type: userConstants.LOGIN_FAILURE, error } }
}

function getViewAsUser(loginId) {
    return dispatch => {
        dispatch(request());
        userService.getViewAsUser(loginId)
            .then(
                user => {
                    dispatch(success(user));
                },
                error => {
                    dispatch(failure(error));
                }
            );
    };

    function request(user) { return { type: userConstants.VIEW_AS_REQUEST, user } }
    function success(user) { return { type: userConstants.VIEW_AS_SUCCESS, user } }
    function failure(error) { return { type: userConstants.VIEW_AS_FAILURE, error } }
}

function register(user) {
    return dispatch => {
        dispatch(request(user));

        userService.register(user)
            .then(
                () => { 
                    dispatch(success());
                    history.push('/login');
                    dispatch(alertActions.success('Registration successful'));
                },
                error => {
                    dispatch(failure(error));
                    dispatch(alertActions.error(error));
                }
            );
    };

    function request(user) { return { type: userConstants.REGISTER_REQUEST, user } }
    function success(user) { return { type: userConstants.REGISTER_SUCCESS, user } }
    function failure(error) { return { type: userConstants.REGISTER_FAILURE, error } }
}

function getAll() {
    return dispatch => {
        dispatch(request());

        userService.getAll()
            .then(
                users => dispatch(success(users)),
                error => dispatch(failure(error))
            );
    };

    function request() { return { type: userConstants.GETALL_REQUEST } }
    function success(users) { return { type: userConstants.GETALL_SUCCESS, users } }
    function failure(error) { return { type: userConstants.GETALL_FAILURE, error } }
}

// prefixed function name with underscore because delete is a reserved word in javascript
function _delete(id) {
    return dispatch => {
        dispatch(request(id));

        userService.delete(id)
            .then(
                () => { 
                    dispatch(success(id));
                },
                error => {
                    dispatch(failure(id, error));
                }
            );
    };

    function request(id) { return { type: userConstants.DELETE_REQUEST, id } }
    function success(id) { return { type: userConstants.DELETE_SUCCESS, id } }
    function failure(id, error) { return { type: userConstants.DELETE_FAILURE, id, error } }
}

function forgotPassword(email) {
    return dispatch => {
        dispatch(request(email));

        userService.forgotPassword(email)
            .then(
                emailSent => {
                    dispatch(success(emailSent));
                },
                error => {
                    dispatch(failure(error));
                    dispatch(alertActions.error(error));
                }
            );
    };

    function request(email) { return { type: userConstants.FORGOTPASSWORD_REQUEST, email } }
    function success(emailSent) { return { type: userConstants.FORGOTPASSWORD_SUCCESS, emailSent } }
    function failure(error) { return { type: userConstants.FORGOTPASSWORD_FAILURE, error } }
}

function resetPassword(token,password) {
    return dispatch => {
        dispatch(request(password));

        userService.resetPassword(token,password)
            .then(resetSuccessful => {
                dispatch(success(resetSuccessful));
            },
                error => {
                    dispatch(failure(error));
                    dispatch(alertActions.error(error));
                });
    };

    function request(password) { return { type: userConstants.UPDATEPASSWORD_REQUEST, password } }
    function success(resetSuccessful) { return { type: userConstants.UPDATEPASSWORD_SUCCESS, resetSuccessful } }
    function failure(error) { return { type: userConstants.UPDATEPASSWORD_FAILURE, error } }
}