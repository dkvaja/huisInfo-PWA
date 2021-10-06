import React, { Fragment } from "react";
import { connect } from "react-redux";
import Header from "./Header";
import { CssBaseline } from "@material-ui/core";
import { withStyles } from '@material-ui/core/styles';
import './layout.css';
import { commonActions } from "../_actions";
import { withTranslation } from "react-i18next";
import { getUser, history } from "../_helpers";
import { apps, userAccountTypeConstants } from "../_constants";
import { dossiersActions } from "../_actions/dossiers.actions";

const { webApiUrl } = window.appConfig;

const styles = theme => ({
    //common for most pages with fixed height
    mainElement: {
        height: "calc(100% - 96px)",
        position: "fixed",
        overflowX: "hidden",
        overflowY: "auto",
        width: "100%",
        [theme.breakpoints.down("sm")]: {
            height: "calc(100% - 64px)",
        },
        [theme.breakpoints.down("xs")]: {
            height: "calc(100% - 56px)",
        },
        [`${theme.breakpoints.down("xs")} and (orientation: landscape)`]: {
            height: "calc(100% - 48px)"
        }
    },
})

class Layout extends React.Component {
    state = {};

    componentDidMount() {
        const { dispatch, user, app } = this.props;
        if (user && user.id && app !== null) {
            dispatch(commonActions.getBuildings(app));
        }

        //Change document title here to customize based on user
        //document.title = "Custom title here";
        var intervalId = setInterval(this.timer, 10000);
        // store intervalId in the state so it can be accessed later:
        this.setState({ intervalId: intervalId });
    }

    componentWillUnmount() {
        // use intervalId from the state to clear the interval
        clearInterval(this.state.intervalId);
    }

    componentDidUpdate(prevProps, prevState) {
        const { dispatch, user, app, selected } = this.props;
        if (user && user.id && app !== null && (!prevProps.user || prevProps.user.id !== user.id || prevProps.app !== app)) {
            dispatch(commonActions.getBuildings(app));
        }
        if (!prevProps.selected || prevProps.selected.buildingId !== this.props.selected.buildingId) {
            if (user.type === userAccountTypeConstants.buyer && selected) {
                this.props.dispatch(dossiersActions.getDossiersByBuildingId(selected.buildingId));
            } else if (selected)
                this.props.dispatch(dossiersActions.getAllDossiersByProject(selected.projectId));
            this.UpdateDashboardCount()
        }
    }

    timer = () => {
        if (!!this.state.intervalId) {
            this.UpdateDashboardCount();
            this.CheckLoginStatus();
        }
    }

    CheckLoginStatus() {
        var localUser = getUser();
        if (!localUser) {
            history.push('/login');
        }
    }

    UpdateDashboardCount() {
        const { selected, user, app, dispatch } = this.props;
        if (!!selected && !!user && app != null) {
            dispatch(commonActions.getDashboardCount(app, selected));
        }
    }

    onScroll = () => {
        window.scrollTo(0, 1)
    }

    render() {
        const { alert, selected, user, classes, dashboardCount, app } = this.props;
        return (
            user ?
                //Id layout-root is used for scroll position
                <div id="layout-root" className="layout-root" style={{
                    backgroundImage:
                        (
                            selected
                                ?
                                'url(' + webApiUrl + 'api/home/ProjectBackground/' + selected.projectId + '), '
                                :
                                ''
                        )
                        + 'url(' + webApiUrl + 'api/Config/WebBackground)'
                }}>
                    <CssBaseline />
                    <Header user={user} selectedBuilding={selected} app={app} dashboardCount={dashboardCount} />
                    <main className={classes.mainElement} onScroll={this.onScroll}>
                        {this.props.children}
                    </main>
                </div>
                :
                this.props.children
        );
    }
}

function mapStateToProps(state) {
    const { alert, buildings, dashboardCount, app } = state;
    const { selected } = buildings;
    return {
        alert,
        selected,
        dashboardCount,
        app
    };
}

const connectedLayout = connect(mapStateToProps)(withTranslation()(withStyles(styles)(Layout)));
export { connectedLayout as Layout }
