import React from "react";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import {
    Container,
    Grid,
    Typography, ExpansionPanel, ExpansionPanelSummary, AppBar, TextField, Button, Toolbar, Table, TableBody, TableRow, TableCell, Avatar, IconButton
} from "@material-ui/core";
import { withStyles, withTheme } from '@material-ui/core/styles';
import { Search, Person, ArrowBack } from "@material-ui/icons"
import { withTranslation } from 'react-i18next';
import clsx from "clsx";
import NumberFormat from "react-number-format";
import { history, nl2br, formatDate, authHeader } from '../../_helpers';

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
    halfWidth: {
        width: '50%',
        verticalAlign: 'top'
    },
    button: {
        '&:hover': {
            color: theme.palette.primary.contrastText
        }
    },
    thumbnail: {
        width: 50,
        height: 50,
        backgroundRepeat: 'no-repeat',
        backgroundPosition: 'center',
        backgroundSize: 'contain',
        backgroundColor: 'rgba(0,0,0,0.1)',
        '&.big': {
            width: '100%',
            height: 0,
            padding: '50%',
            borderRadius: theme.spacing(1)
        }
    }
});


class Page extends React.Component {
    state = {
        repairRequest: null
    };

    componentDidMount() {
        this.GetRepairRequestDetails();
    }

    componentDidUpdate(prevProps, prevState) {
        if (!prevProps.selected || prevProps.selected.buildingId !== this.props.selected.buildingId) {
            this.GetRepairRequestDetails();
        }
    }

    GetRepairRequestDetails() {
        let { repairRequestId } = this.props.match.params;
        const { selected } = this.props;
        if (selected && repairRequestId) {
            const url = webApiUrl + 'api/RepairRequest/GetRepairRequestDetails/' + encodeURI(repairRequestId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            this.setState({
                repairRequest: null,
            });

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    if (findResponse.number) {
                        this.setState({ repairRequest: findResponse });
                    }
                });
        }
    }

    render() {
        const { t, classes, selected } = this.props;
        const { repairRequest } = this.state;
        return (
            <Container className={classes.mainContainer}>
                <Grid container>
                    <Grid item container xs={12} className={classes.container}>
                        <AppBar position="sticky">
                            <Toolbar variant="dense">
                                <IconButton
                                    color="inherit"
                                    edge="start"
                                    aria-label="go back"
                                    component="span"
                                    onClick={() => history.push('/nazorg')}
                                >
                                    <ArrowBack />
                                </IconButton>
                                <Typography className={classes.bold}>{t('Details van melding ') + (repairRequest ? repairRequest.number : '')}</Typography>
                            </Toolbar>
                        </AppBar>
                        {
                            repairRequest &&
                            <Grid container spacing={2} item xs={12} className={classes.innerContainer} alignItems="flex-start">
                                <Grid item xs={12} md={6}>
                                    <Typography variant="h6">{t('Algemene informatie')}</Typography>
                                    <Table>
                                        <TableBody>
                                            <TableRow>
                                                <TableCell component="th" scope="row" className={classes.halfWidth}>
                                                    {t('Datum') + ':'}
                                                </TableCell>
                                                <TableCell>{repairRequest.date && formatDate(new Date(repairRequest.date))}</TableCell>
                                            </TableRow>
                                            <TableRow>
                                                <TableCell component="th" scope="row">
                                                    {t('Soort') + ':'}
                                                </TableCell>
                                                <TableCell>{repairRequest.repairRequestType && repairRequest.repairRequestType}</TableCell>
                                            </TableRow>
                                            <TableRow>
                                                <TableCell component="th" scope="row">
                                                    {t('Status') + ':'}
                                                </TableCell>
                                                <TableCell>{repairRequest.status && repairRequest.status}</TableCell>
                                            </TableRow>
                                        </TableBody>
                                    </Table>
                                </Grid>
                                <Grid item xs={12} md={6}>
                                    <Typography variant="h6">{t('Adres')}</Typography>
                                    <Table>
                                        <TableBody>
                                            <TableRow>
                                                <TableCell component="th" scope="row" className={classes.halfWidth}>
                                                    {t('Straat en huisnummer') + ':'}
                                                </TableCell>
                                                <TableCell>{repairRequest.address && (repairRequest.address.street ? repairRequest.address.street : "") + " " + (repairRequest.address.houseNo ? repairRequest.address.houseNo : "")}</TableCell>
                                            </TableRow>
                                            <TableRow>
                                                <TableCell component="th" scope="row">
                                                    {t('Postcode') + ':'}
                                                </TableCell>
                                                <TableCell>{repairRequest.address && repairRequest.address.postcode && repairRequest.address.postcode}</TableCell>
                                            </TableRow>
                                            <TableRow>
                                                <TableCell component="th" scope="row">
                                                    {t('Plaats') + ':'}
                                                </TableCell>
                                                <TableCell>{repairRequest.address && repairRequest.address.place && repairRequest.address.place}</TableCell>
                                            </TableRow>
                                        </TableBody>
                                    </Table>
                                </Grid>

                                <Grid item xs={12} md={6}>
                                    <Typography variant="h6">{t('Toelichting melding')}</Typography>
                                    <Table>
                                        <TableBody>
                                            <TableRow>
                                                <TableCell component="th" scope="row" className={classes.halfWidth}>
                                                    {t('Locatie') + ':'}
                                                </TableCell>
                                                <TableCell>{repairRequest.location && repairRequest.location}</TableCell>
                                            </TableRow>
                                            <TableRow>
                                                <TableCell component="th" scope="row">
                                                    {t('Omschrijving') + ':'}
                                                </TableCell>
                                                <TableCell>{repairRequest.desc && repairRequest.desc}</TableCell>
                                            </TableRow>
                                            <TableRow>
                                                <TableCell component="th" scope="row">
                                                    {t('Toelichting') + ':'}
                                                </TableCell>
                                                <TableCell>{nl2br(repairRequest.detailDesc)}</TableCell>
                                            </TableRow>
                                        </TableBody>
                                    </Table>
                                </Grid>
                                {
                                    repairRequest.attachments && repairRequest.attachments.length > 0 &&
                                    <Grid item container xs={12} md={6} spacing={1}>
                                        <Grid item xs={12}>
                                            <Typography variant="h6">{t('Afbeeldingen')}</Typography>
                                        </Grid>
                                        {
                                            repairRequest.attachments.map((attachment, index) =>
                                                <Grid item xs={6}>
                                                    <div key={index} class={clsx(classes.thumbnail, 'big')} style={{
                                                        backgroundImage: 'url(/api/home/getattachment/' + attachment.attachmentId + ')'
                                                    }}></div>
                                                </Grid>
                                            )
                                        }
                                    </Grid>
                                }
                                {
                                    repairRequest.resolvers &&
                                    <Grid item xs={12}>
                                        <Typography variant="h6">{t('Oplossers')}</Typography>
                                        <div style={{ overflow: 'auto' }}>
                                            <Table>
                                                <TableBody>
                                                    <TableRow>
                                                        <TableCell component="th" scope="row">
                                                            {t('Logo')}
                                                        </TableCell>
                                                        <TableCell component="th" scope="row">
                                                            {t('Organisatie')}
                                                        </TableCell>
                                                        <TableCell component="th" scope="row">
                                                            {t('Contactpersoon')}
                                                        </TableCell>
                                                        <TableCell component="th" scope="row">
                                                            {t('Telefoonnummer')}
                                                        </TableCell>
                                                        <TableCell component="th" scope="row">
                                                            {t('Ingelicht op')}
                                                        </TableCell>
                                                    </TableRow>
                                                    {
                                                        repairRequest.resolvers.map((resolver, index) => (
                                                            <TableRow key={index}>
                                                                <TableCell>
                                                                    <div class={classes.thumbnail} style={{
                                                                        backgroundImage: ('url(/api/organisation/GetOrganisationLogo/' + resolver.organisatonId + ')')
                                                                    }}></div>
                                                                </TableCell>
                                                                <TableCell>{resolver.name && resolver.name}</TableCell>
                                                                <TableCell>{resolver.relationName && resolver.relationName}</TableCell>
                                                                <TableCell>{resolver.telephone && resolver.telephone}</TableCell>
                                                                <TableCell>{resolver.dateNotified && formatDate(new Date(resolver.dateNotified))}</TableCell>
                                                            </TableRow>
                                                        ))
                                                    }
                                                </TableBody>
                                            </Table>
                                        </div>
                                    </Grid>
                                }
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
export { connectedPage as RepairRequestDetailsPage };