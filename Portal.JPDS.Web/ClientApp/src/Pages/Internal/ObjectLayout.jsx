import React from "react";
import { connect } from "react-redux";
import { Container } from "@material-ui/core";
import { withStyles, withTheme } from '@material-ui/core/styles';
import { withTranslation } from 'react-i18next';
import { commonActions } from "../../_actions";
import { history as History } from "../../_helpers";

const { webApiUrl } = window.appConfig;

const styles = theme => ({
    grow: {
        flexGrow: 1
    },
    bold: {
        fontWeight: "bold"
    },
    mainContainer: {
        height: '100%',
        width: '100%',
        overflow: 'auto',
        padding: 0
    },
});


class Layout extends React.Component {
    state = {
        actions: [],
        plannings: [],
        messages: []
    };

    componentDidMount() {
        const { user, selected } = this.props;

        this.UpdateSelectedBuilding()

        var intervalId = setInterval(this.timer, 10000);
        // store intervalId in the state so it can be accessed later:
        this.setState({ intervalId: intervalId });
    }

    componentWillUnmount() {
        // use intervalId from the state to clear the interval
        clearInterval(this.state.intervalId);
    }

    componentDidUpdate(prevProps, prevState) {
        const { selected, match, history } = this.props;
        const { buildingNoInternal } = match.params;
        if (!prevProps.selected || prevProps.selected.projectId !== selected.projectId) {
            this.UpdateSelectedBuilding();
        }
        if (!!prevProps.selected && prevProps.selected.buildingId !== selected.buildingId) {
            var url = match.url + '/';
            url = url.replace('/object/' + prevProps.selected.buildingNoIntern + '/', '/object/' + selected.buildingNoIntern + '/');
            url = url.substr(0, url.length - 1);

            if (!!history && !!history.location && history.location.pathname === url) {
                History.push(url, history.location.state);
            }
            else {
                History.push(url);
            }
        }
    }

    timer = () => {
        //this.UpdateBuildings();
    }

    UpdateSelectedBuilding() {
        const { dispatch, selected, buildings, match } = this.props;
        const { buildingNoInternal } = match.params;

        if (selected && selected.buildingNoIntern.toUpperCase() != buildingNoInternal.toUpperCase()) {
            var selectedItem = buildings.find(x => x.buildingNoIntern.toUpperCase() == buildingNoInternal.toUpperCase())
            if (selectedItem) {
                dispatch(commonActions.selectBuilding(selectedItem));
            }
            else {
                History.push('/werk/' + selected.projectNo + '/objecten')
            }
        }
    }


    render() {
        const { classes } = this.props;
        const { } = this.state;

        return (
            <Container maxWidth={false} className={classes.mainContainer}>
                {this.props.children}
            </Container >
        );
    }
}

function mapStateToProps(state) {
    const { authentication, buildings, dashboardCount } = state;
    const { user } = authentication;
    const { selected, all } = buildings;
    const { messageCountPerBuilding } = dashboardCount;
    return {
        user,
        selected,
        buildings: all,
        messageCountPerBuilding
    };
}

const connectedLayout = connect(mapStateToProps)(withTranslation()(withStyles(styles)(Layout)));
export { connectedLayout as ObjectLayout };