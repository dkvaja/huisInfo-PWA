import { combineReducers } from 'redux';

import { authentication } from './authentication.reducer';
import { registration } from './registration.reducer';
import { users } from './users.reducer';
import { buildings, dashboardCount, app, meldingenFilter } from './common.reducer';
import { alert } from './alert.reducer';
import { dossier } from './dossiers.reducer';
import { routerReducer } from 'react-router-redux';

const rootReducer = combineReducers({
    authentication,
    registration,
    users,
    alert,
    buildings,
    dashboardCount,
    app,
    dossier,
    routing: routerReducer,
    meldingenFilter
});

export default rootReducer;