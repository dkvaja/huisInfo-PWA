import { userService } from "../_services";

export function getUser() {
    var userFromLocal = localStorage.getItem('user');
    var userFromSession = sessionStorage.getItem('user');
    if (!!userFromSession) {
        //check whether main user logged out then logout session user as well
        if (!userFromLocal) {
            sessionStorage.removeItem('user');
            return null;
        }

        return userFromSession;
    }
    return userFromLocal;
}

export function removeUser() {
    var userFromSession = sessionStorage.getItem('user');
    if (!!userFromSession) {
        sessionStorage.removeItem('user');
    }
    else {
        localStorage.removeItem('user');
    }
}