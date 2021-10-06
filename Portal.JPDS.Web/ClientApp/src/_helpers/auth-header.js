import { getUser } from './get-user';

export function authHeader(contentType) {
    // return authorization header with jwt token
    let user = JSON.parse(getUser());

    if (user && user.token) {
        if (contentType) {
            return {
                'Authorization': 'Bearer ' + user.token,
                'Content-Type': contentType
            };
        }
        return {
            'Authorization': 'Bearer ' + user.token
        };
    }
    else if (contentType) {
        return {
            'Content-Type': contentType
        };
    }
    else {
        return {};
    }
}