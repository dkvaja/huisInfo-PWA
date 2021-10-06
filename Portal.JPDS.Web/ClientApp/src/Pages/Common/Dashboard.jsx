import React from "react";
import { connect } from "react-redux";
import { Link } from "react-router-dom";
import { withStyles } from "@material-ui/core/styles";
import { withTranslation } from 'react-i18next';
import { userAccountTypeConstants, appsInfo } from "../../_constants";
import { Card, CardActionArea, CardMedia, CardContent, Container, Typography, Grid, CssBaseline, Fab, Divider, Box } from "@material-ui/core";
import { commonActions } from "../../_actions";
import { history, getCommonArray } from "../../_helpers";
import { ExitToApp } from "@material-ui/icons";

const styles = theme => ({
    wrapper: {
        backgroundSize: "cover",
        backgroundAttachment: "fixed",
        backgroundPosition: "center"
    },
    container: {
        backgroundColor: "rgba(255,255,255,0.9)",
        borderRadius: theme.spacing(1),
        padding: theme.spacing(4),
        width: 750,
        maxWidth: "100%"
    },
    logoContainer: {
        padding: theme.spacing(1),
        margin: theme.spacing(0, 0, 2),
        borderRadius: theme.spacing(1),
        backgroundColor: theme.palette.common.white,
        '& > img': {
            width: '100%',
            maxWidth: 500,
            maxHeight: 150,
            minWidth: 100,
            minHeight: 60
        }
    },
    grow: {
        flexGrow: 1
    },
    bold: {
        fontWeight: "bold"
    },
    actionItems: {
        position: "fixed",
        padding: theme.spacing(2),
        top: 0,
        right: 0
    },
    welcomePanel: {
        padding: theme.spacing(5, 0)
    },
    card: {
        width: 120,
        margin: theme.spacing(2),
        background: 'none',
        boxShadow: 'none',
        '& .MuiCardMedia-root': {
            backgroundSize: 'contain',
            width: '100%',
            height: 0,
            padding: '50% 0'
        }
    },
    mediaContainer: {
        padding: theme.spacing(1),
        margin: theme.spacing(-1),
        borderRadius: theme.spacing(1),
        backgroundColor: theme.palette.common.white,
    },
    appName: {
        margin: theme.spacing(2, -2, -1)
    }
});

class Page extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            huisinfo: false,
            repairRequest: false
        };
    }

    componentDidMount() {
        const { user, dispatch } = this.props;

        //set app not selected
        dispatch(commonActions.selectApp(null));

        //check if only one app user has access to then go to that app
        if (user.availableApps) {
            var availableAppsForUser = getCommonArray(user.availableApps, appsInfo.map(x => x.appId))
            if (availableAppsForUser.length === 1) {
                var info = appsInfo.find(x => x.appId === availableAppsForUser[0]);
                if (info && info.path)
                    this.handleSelectApp(info);
            }
        }
    }

    handleSelectApp(app) {
        const { dispatch } = this.props;
        dispatch(commonActions.selectApp(app.appId));
        history.push(app.path)
    }

    render() {
        const { t, classes, user } = this.props;
        const { huisinfo, repairRequest } = this.state;
        const { webApiUrl } = window.appConfig;
        const isBuyer = user.type === userAccountTypeConstants.buyer;

        return (
            <Grid
                container
                spacing={0}
                direction="column"
                alignItems="center"
                justify="center"
                className={classes.wrapper}
                style={{ minHeight: "100vh", backgroundImage: "url(" + webApiUrl + "api/Config/WebBackground)" }}
            >
                <CssBaseline />
                <div className={classes.actionItems}>
                    <Fab onClick={() => { if (user.viewOnly !== true) history.push('/login') }} size="medium" color="secondary" aria-label="logout" title={t('layout.navbar.logout')}>
                        <ExitToApp />
                    </Fab>
                </div>
                <Grid item className={classes.container}>
                    <Grid container spacing={2}
                        direction="column"
                        alignItems="center"
                        justify="center">
                        {
                            <Grid item className={classes.logoContainer}>
                                <img src={webApiUrl + "api/Config/WebLogo"} alt="JPDS" />
                            </Grid>
                        }
                        {
                            //    <Grid item xs={12} className={classes.welcomePanel}>
                            //    <Typography component="h1" variant="h4" gutterBottom align="center">{user.name}, {isBuyer ? t('dashboard.welcome.text.buyer') : t('dashboard.welcome.text')}</Typography>
                            //</Grid>
                        }
                        <Grid item container justify="center" alignItems="center" alignContent="center">
                            <Grid item className={classes.grow}>
                                <Divider />
                            </Grid>
                            <Typography color="textSecondary" variant="h5" align="center">&nbsp;&nbsp; {t('Apps')} &nbsp;&nbsp;</Typography>
                            <Grid item className={classes.grow}>
                                <Divider />
                            </Grid>
                        </Grid>
                        <Grid item container justify="center" alignItems="center" spacing={5}>
                            {
                                appsInfo.map((app, index) => {
                                    var appName = t(app.nameKey + (isBuyer && app.isDifferentNameForBuyer === true ? '.buyer' : ''))
                                    return user.availableApps.includes(app.appId) &&
                                        <Card key={index} className={classes.card}>
                                            <CardActionArea onClick={() => this.handleSelectApp(app)}>
                                                <CardContent>
                                                    <Box boxShadow={1} className={classes.mediaContainer}>
                                                        <CardMedia
                                                            alt={appName}
                                                            image={app.icon}
                                                            title={appName}
                                                        />
                                                    </Box>
                                                    <Typography className={classes.appName} color="textSecondary" variant="body1" align="center" title={appName} noWrap>{appName}</Typography>
                                                </CardContent>
                                            </CardActionArea>
                                        </Card>
                                })
                            }
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
        );
    }
}

function mapStateToProps(state) {
    const { authentication, buildings, dashboardCount } = state;
    const { user } = authentication;
    const { selected } = buildings;
    return {
        user,
        selected,
        dashboardCount
    };
}

const connectedLayout = connect(mapStateToProps)(withTranslation()(withStyles(styles)(Page)));
export { connectedLayout as Dashboard }
