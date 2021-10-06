import { userConstants } from '../_constants';
import { getUser } from '../_helpers';

var localUser = getUser();
let user = localUser && JSON.parse(localUser);
const initialState = user ? { loggedIn: true, user } : {};

export function authentication(state = initialState, action) {
    switch (action.type) {
        case userConstants.LOGIN_REQUEST:
            return {
                loggingIn: true,
                username: action.user
            };
        case userConstants.LOGIN_SUCCESS:
            return {
                loggedIn: true,
                user: action.user
            };
        case userConstants.LOGIN_FAILURE:
            return {};
        case userConstants.LOGOUT:
            return {};
        case userConstants.VIEW_AS_REQUEST:
            return {
                ...initialState,
                viewAsRequest: true
            };
        case userConstants.VIEW_AS_SUCCESS:
            return {
                loggedIn: true,
                user: action.user
            };
        case userConstants.VIEW_AS_FAILURE:
            return {
                ...initialState,
                viewAsFailed: true
            };
        case userConstants.FORGOTPASSWORD_REQUEST:
            return {
                loading: true,
                emailSent: false,
                noError: true
            };
        case userConstants.FORGOTPASSWORD_SUCCESS:
            return {
                emailSent: true
            };
        case userConstants.FORGOTPASSWORD_FAILURE:
            return {};
        case userConstants.UPDATEPASSWORD_REQUEST:
            return {
                loading: true,
                resetSuccessful: false,
                noError: true
            };
        case userConstants.UPDATEPASSWORD_SUCCESS:
            return {
                resetSuccessful: true
            };
        case userConstants.UPDATEPASSWORD_FAILURE:
            return {};
        default:
            return state
    }
}