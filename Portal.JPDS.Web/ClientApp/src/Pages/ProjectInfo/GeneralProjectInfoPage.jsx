import React from "react";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import {
    Container,
    Grid,
    Typography, ExpansionPanel, ExpansionPanelSummary, AppBar, TextField, Button, Toolbar, Table, TableBody, TableRow, TableCell, Avatar
} from "@material-ui/core";
import { withStyles, withTheme } from '@material-ui/core/styles';
import { Search, Person } from "@material-ui/icons"
import { withTranslation } from 'react-i18next';
import clsx from "clsx";
import { formatDate, authHeader } from "../../_helpers";

const { webApiUrl } = window.appConfig;

const styles = theme => ({
    bold: {
        fontWeight: "bold"
    },
    mainContainer: {
        [theme.breakpoints.down("xs")]: {
            padding: theme.spacing(0)
        }
    },
    container: {
        backgroundColor: theme.palette.background.paper,
        margin: theme.spacing(5, 0, 6),
        [theme.breakpoints.down("xs")]: {
            marginTop: theme.spacing(0)
        }
    },
    innerContainer: {
        padding: theme.spacing(2, 3, 4)
    },
    bigAvatar: {
        margin: 'auto',
        width: 120,
        height: 120
    },
    divWithHtmlContent: {
        maxWidth: '100%',
        overflowX: 'auto',
        '& *': {
            maxWidth: '100%'
        }
    },
    button: {
        '&:hover': {
            color: theme.palette.primary.contrastText
        }
    }
});


class Page extends React.Component {
    state = {
        projectInfo: null,
        relaties: []
    };

    componentDidMount() {
        this.UpdateProjectInfo();
        this.UpdateRelatiesInfo();
    }

    componentDidUpdate(prevProps, prevState) {
        if (!prevProps.selected || !this.props.selected || prevProps.selected.projectId.toUpperCase() !== this.props.selected.projectId.toUpperCase()) {
            this.UpdateProjectInfo();
            this.UpdateRelatiesInfo();
        }
    }

    UpdateProjectInfo() {
        const { selected } = this.props;
        if (selected) {
            const url = webApiUrl + 'api/home/GetProjectInfo/' + encodeURI(selected.projectId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            this.setState({
                projectInfo: null,
                relaties: []
            });

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({ projectInfo: findResponse });
                });
        }
    }

    UpdateRelatiesInfo() {
        const { selected } = this.props;
        if (selected) {
            const url = webApiUrl + 'api/home/GetRelationsByProject/' + encodeURI(selected.projectId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            this.setState({
                projectInfo: null,
                relaties: []
            });

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({ relaties: findResponse });
                });
        }
    }


    render() {
        const { t, classes } = this.props;
        const { projectInfo, relaties } = this.state;
        return (
            <Container className={classes.mainContainer}>
                <Grid container>
                    <Grid item container xs={12} className={classes.container}>
                        <AppBar position="sticky">
                            <Toolbar variant="dense">
                                <Typography className={classes.bold}>{projectInfo ? projectInfo.projectName : ''}</Typography>
                            </Toolbar>
                        </AppBar>
                        {
                            projectInfo &&
                            <Grid container spacing={2} item xs={12} direction="row-reverse" className={classes.innerContainer}>
                                <Grid container item xs={12} md={4} alignContent="flex-start">
                                    {
                                    //    <Grid item xs={12}>
                                    //    <Typography variant="h6">{t('Kopersbegeleider')}</Typography>
                                    //</Grid>
                                    }
                                    <Grid item xs={12}>
                                        <Table>
                                            <TableBody>
                                                {
                                                    relaties.map((relatie, index) => (
                                                        <React.Fragment key={index}>

                                                            <TableRow>
                                                                <TableCell component="th" colSpan={2} align="center">
                                                                    {
                                                                        relatie.hasPhoto === true ?
                                                                            <Avatar alt={relatie.fullName} src={webApiUrl +"api/home/GetPersonPhoto/" + relatie.personId} className={classes.bigAvatar} />
                                                                    :
                                                                            <Avatar className={classes.bigAvatar} >
                                                                                <Person fontSize="large" />
                                                                            </Avatar>
                                                                    }
                                                                </TableCell>
                                                            </TableRow>
                                                            {
                                                                relatie.fullName && relatie.fullName.trim() !== '' &&
                                                                <TableRow>
                                                                    <TableCell component="th">
                                                                        {t('Naam') + ':'}
                                                                    </TableCell>
                                                                    <TableCell>{relatie.fullName}</TableCell>
                                                                </TableRow>
                                                            }
                                                            {
                                                                relatie.function && relatie.function.trim() !== '' &&
                                                                <TableRow>
                                                                    <TableCell component="th">
                                                                        {t('Functie') + ':'}
                                                                    </TableCell>
                                                                    <TableCell>{relatie.function}</TableCell>
                                                                </TableRow>
                                                            }
                                                            {
                                                                relatie.email && relatie.email.trim() !== '' &&
                                                                <TableRow>
                                                                    <TableCell component="th" scope="row">
                                                                        {t('E-mail') + ':'}
                                                                    </TableCell>
                                                                    <TableCell>{relatie.email}</TableCell>
                                                                </TableRow>
                                                            }{
                                                                relatie.telephone && relatie.telephone.trim() !== '' &&
                                                                <TableRow>
                                                                    <TableCell component="th" scope="row">
                                                                        {t('Telefoon') + ':'}
                                                                    </TableCell>
                                                                    <TableCell>{relatie.telephone}</TableCell>
                                                                </TableRow>
                                                            }{
                                                                relatie.mobile && relatie.mobile.trim() !== '' &&
                                                                <TableRow>
                                                                    <TableCell component="th" scope="row">
                                                                        {t('Mobiel') + ':'}
                                                                    </TableCell>
                                                                    <TableCell>{relatie.mobile}</TableCell>
                                                                </TableRow>
                                                            }
                                                        </React.Fragment>
                                                    ))
                                                }
                                            </TableBody>
                                        </Table>
                                    </Grid>
                                </Grid>
                                <Grid container item xs={12} md={8}>
                                    <Grid item xs={12}>
                                        <Typography variant="h6">{t('Algemene informatie')}</Typography>
                                        <Table>
                                            <TableBody>
                                                {
                                                    projectInfo.projectNo && projectInfo.projectNo.trim() !== '' &&
                                                    <TableRow>
                                                        <TableCell component="th" scope="row">
                                                            {t('Werknummer') + ':'}
                                                        </TableCell>
                                                        <TableCell>{projectInfo.projectNo}</TableCell>
                                                    </TableRow>
                                                }
                                                {
                                                    projectInfo.projectName && projectInfo.projectName.trim() !== '' &&
                                                    <TableRow>
                                                        <TableCell component="th" scope="row">
                                                            {t('Werknaam') + ':'}
                                                        </TableCell>
                                                        <TableCell>{projectInfo.projectName}</TableCell>
                                                    </TableRow>
                                                }
                                                {
                                                    projectInfo.place && projectInfo.place.trim() !== '' &&
                                                    <TableRow>
                                                        <TableCell component="th" scope="row">
                                                            {t('Plaats') + ':'}
                                                        </TableCell>
                                                        <TableCell>{projectInfo.place}</TableCell>
                                                    </TableRow>
                                                }
                                                {
                                                    projectInfo.objectsCount &&
                                                    <TableRow>
                                                        <TableCell component="th" scope="row">
                                                            {t('Aantal objecten') + ':'}
                                                        </TableCell>
                                                        <TableCell>{projectInfo.objectsCount}</TableCell>
                                                    </TableRow>
                                                }
                                                {
                                                    projectInfo.projectType && projectInfo.projectType.trim() !== '' &&
                                                    <TableRow>
                                                        <TableCell component="th" scope="row">
                                                            {t('Type project') + ':'}
                                                        </TableCell>
                                                        <TableCell>{projectInfo.projectType}</TableCell>
                                                    </TableRow>
                                                }
                                                {
                                                    projectInfo.projectConstructionType && projectInfo.projectConstructionType.trim() !== '' &&
                                                    <TableRow>
                                                        <TableCell component="th" scope="row">
                                                            {t('Soort project') + ':'}
                                                        </TableCell>
                                                        <TableCell>{projectInfo.projectConstructionType}</TableCell>
                                                    </TableRow>
                                                }
                                                {
                                                    projectInfo.projectPhase && projectInfo.projectPhase.trim() !== '' &&
                                                    <TableRow>
                                                        <TableCell component="th" scope="row">
                                                            {t('Fase') + ':'}
                                                        </TableCell>
                                                        <TableCell>{projectInfo.projectPhase}</TableCell>
                                                    </TableRow>
                                                }
                                                {
                                                    projectInfo.saleStartDate && projectInfo.saleStartDate.trim() !== '' &&
                                                    <TableRow>
                                                        <TableCell component="th" scope="row">
                                                            {t('Startdatum verkoop') + ':'}
                                                        </TableCell>
                                                        <TableCell>{formatDate(new Date(projectInfo.saleStartDate))}</TableCell>
                                                    </TableRow>
                                                }
                                                {
                                                    projectInfo.dateStartConstruction && projectInfo.dateStartConstruction.trim() !== '' &&
                                                    <TableRow>
                                                        <TableCell component="th" scope="row">
                                                            {t('Startdatum bouw') + ':'}
                                                        </TableCell>
                                                        <TableCell>{formatDate(new Date(projectInfo.dateStartConstruction))}</TableCell>
                                                    </TableRow>
                                                }
                                                {
                                                    projectInfo.dateEndConstruction && projectInfo.dateEndConstruction.trim() !== '' &&
                                                    <TableRow>
                                                        <TableCell component="th" scope="row">
                                                            {t('Einddatum bouw') + ':'}
                                                        </TableCell>
                                                        <TableCell>{formatDate(new Date(projectInfo.dateEndConstruction))}</TableCell>
                                                    </TableRow>
                                                }
                                            </TableBody>
                                        </Table>
                                    </Grid>

                                    <Grid item xs={12}>
                                        <br />
                                        <Typography variant="h6">{t('Overige algemene informatie')}</Typography>
                                        <div className={ classes.divWithHtmlContent} dangerouslySetInnerHTML={{ __html: projectInfo.generalInfo }} />
                                    </Grid>
                                </Grid>
                            </Grid>
                        }
                    </Grid>
                </Grid>
            </Container>
        );
    }
}

function mapStateToProps(state) {
    const { authentication, buildings } = state;
    const { user } = authentication;
    const { selected } = buildings;
    return {
        user,
        selected
    };
}

const connectedPage = connect(mapStateToProps)(withTranslation()(withStyles(styles)(Page)));
export { connectedPage as GeneralProjectInfoPage };