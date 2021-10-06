import React from "react";
import { connect } from "react-redux";
import { Container, Grid, Typography } from "@material-ui/core";
import { withStyles } from '@material-ui/core/styles';
import { withTranslation } from 'react-i18next';
import { history } from "../../_helpers";
import { apps } from "../../_constants";

const { webApiUrl } = window.appConfig;

const styles = theme => ({
    wrapper: {
        backgroundSize: 'cover',
        backgroundAttachment: 'fixed',
        backgroundPosition: 'center bottom',
        height: '100%',
        width: '100%',
        overflowX: 'hidden',
        overflowY: 'auto'
    },
    welcomePanel: {
        color: theme.palette.common.white,
        height: '40vh',
        position: 'relative',
        padding: theme.spacing(5, 0),
        '& h1': {
            textShadow: '0 0 10px rgb(0,0,0)'
        }
    },
});


class Page extends React.Component {
    state = {
        actions: [],
        plannings: [],
        messages: []
    };

    componentDidMount() {
        const { selected, app, buyerGuidesBuildings } = this.props;
        if (selected) {
            var url = '/werk/' + selected.projectNo;
            if (app === apps.constructionQuality)
                url += '/kwaliteitsborging';
            else if (app === apps.resolverModule)
                url += '/werkbonnen';
            else if (buyerGuidesBuildings.length === 0)
                url += '/dossier';
            history.push(url)
        }
    }

    componentWillUnmount() {
    }

    componentDidUpdate(prevProps, prevState) {
        const { selected, app, buyerGuidesBuildings } = this.props;
        if (selected) {
            var url = '/werk/' + selected.projectNo;
            if (app === apps.constructionQuality)
                url += '/kwaliteitsborging';
            else if (app === apps.resolverModule)
                url += '/werkbonnen';
            else if (buyerGuidesBuildings.length === 0)
                url += '/dossier';
            history.push(url)
        }
    }

    timer = () => {
        this.UpdateMessages(false);
    }

    render() {
        const { user, t, classes, selected } = this.props;

        return (
            <div className={classes.wrapper} style={{
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
                <Container>
                    <Grid container direction="row" justify="center" alignItems="center" className={classes.welcomePanel}>
                        <Grid item xs>
                            <Typography component="h1" variant="h4" gutterBottom align="center">{user.name}, {t('dashboard.welcome.text')}</Typography>
                        </Grid>
                    </Grid>
                </Container>
            </div>
        );
    }
}

function mapStateToProps(state) {
    const { authentication, buildings, dashboardCount, app } = state;
    const { user } = authentication;
    const { selected, all, buyerGuidesBuildings } = buildings;
    const { quotationsCount } = dashboardCount;
    return {
        user,
        selected,
        buildings: all, buyerGuidesBuildings,
        quotationsCount,
        app
    };
}

const connectedPage = connect(mapStateToProps)(withTranslation()(withStyles(styles)(Page)));
export { connectedPage as HomeInternal };
