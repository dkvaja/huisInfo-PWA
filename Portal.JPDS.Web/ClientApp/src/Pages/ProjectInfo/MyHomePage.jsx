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
import NumberFormat from "react-number-format";
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
    halfWidth: {
        width: '50%'
    },
    button: {
        '&:hover': {
            color: theme.palette.primary.contrastText
        }
    }
});


class Page extends React.Component {
    state = {
        building: null
    };

    componentDidMount() {
        this.UpdateBuildingInfo();
    }

    componentDidUpdate(prevProps, prevState) {
        if (!prevProps.selected || prevProps.selected.buildingId !== this.props.selected.buildingId) {
            this.UpdateBuildingInfo();
        }
    }

    UpdateBuildingInfo() {
        const { selected } = this.props;
        if (selected) {
            const url = webApiUrl + 'api/home/GetBuildingInfo/' + encodeURI(selected.buildingId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            this.setState({
                building: null,
            });

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({ building: findResponse });
                });
        }
    }

    render() {
        const { t, classes } = this.props;
        const { building } = this.state;
        return (
            <Container className={classes.mainContainer}>
                <Grid container>
                    <Grid item container xs={12} className={classes.container}>
                        <AppBar position="sticky">
                            <Toolbar variant="dense">
                                <Typography className={classes.bold}>{t('Mijn woning')}</Typography>
                            </Toolbar>
                        </AppBar>
                        {
                            building &&
                            <Grid container spacing={2} item xs={12} className={classes.innerContainer} alignItems="flex-start">
                                <Grid item xs={12} md={6}>
                                    <Typography variant="h6">{t('Algemene informatie')}</Typography>
                                    <Table>
                                        <TableBody>
                                            <TableRow>
                                                <TableCell component="th" scope="row" className={classes.halfWidth}>
                                                    {t('Bouwnummer (extern)') + ':'}
                                                </TableCell>
                                                <TableCell>{building.buildingNoExtern && building.buildingNoExtern}</TableCell>
                                            </TableRow>
                                            <TableRow>
                                                <TableCell component="th" scope="row">
                                                    {t('Woning type') + ':'}
                                                </TableCell>
                                                <TableCell>{building.propertyType && building.propertyType}</TableCell>
                                            </TableRow>
                                            <TableRow>
                                                <TableCell component="th" scope="row">
                                                    {t('Soort') + ':'}
                                                </TableCell>
                                                <TableCell>{building.buildingType && building.buildingType}</TableCell>
                                            </TableRow>
                                            <TableRow>
                                                <TableCell component="th" scope="row">
                                                    {t('Vrij Op Naam prijs (V.O.N. prijs)') + ':'}
                                                </TableCell>
                                                <TableCell>
                                                    {
                                                        building.freeInNamePriceInclVAT &&
                                                        <NumberFormat
                                                            prefix="&euro;&nbsp;"
                                                            displayType="text"
                                                            decimalScale={2}
                                                            fixedDecimalScale={true}
                                                            thousandSeparator="."
                                                            decimalSeparator=","
                                                            value={building.freeInNamePriceInclVAT}
                                                        />
                                                    }
                                                </TableCell>
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
                                                <TableCell>{building.street && building.street}&nbsp;{building.houseNo && building.houseNo}{building.houseNoAddition && building.houseNoAddition}</TableCell>
                                            </TableRow>
                                            <TableRow>
                                                <TableCell component="th" scope="row">
                                                    {t('Postcode') + ':'}
                                                </TableCell>
                                                <TableCell>{building.postcode && building.postcode}</TableCell>
                                            </TableRow>
                                            <TableRow>
                                                <TableCell component="th" scope="row">
                                                    {t('Plaats') + ':'}
                                                </TableCell>
                                                <TableCell>{building.place && building.place}</TableCell>
                                            </TableRow>
                                        </TableBody>
                                    </Table>
                                </Grid>

                                <Grid item xs={12} md={6}>
                                    <Typography variant="h6">{t('Energie')}</Typography>
                                    <Table>
                                        <TableBody>
                                            <TableRow>
                                                <TableCell component="th" scope="row" className={classes.halfWidth}>
                                                    {t('EAN code elektra') + ':'}
                                                </TableCell>
                                                <TableCell>{building.eanElectricity && building.eanElectricity}</TableCell>
                                            </TableRow>
                                            <TableRow>
                                                <TableCell component="th" scope="row">
                                                    {t('EAN code gas') + ':'}
                                                </TableCell>
                                                <TableCell>{building.eanGas && building.eanGas}</TableCell>
                                            </TableRow>
                                        </TableBody>
                                    </Table>
                                </Grid>

                                <Grid item xs={12} md={6}>
                                    <Typography variant="h6">{t('Garantie')}</Typography>
                                    <Table>
                                        <TableBody>
                                            <TableRow>
                                                <TableCell component="th" scope="row" className={classes.halfWidth}>
                                                    {t('Garantieregeling') + ':'}
                                                </TableCell>
                                                <TableCell>{building.guaranteeScheme && building.guaranteeScheme}</TableCell>
                                            </TableRow>
                                            <TableRow>
                                                <TableCell component="th" scope="row">
                                                    {t('Einde onderhoudstermijn') + ':'}
                                                </TableCell>
                                                <TableCell>{building.dateEndMaintenancePeriod && formatDate(new Date(building.dateEndMaintenancePeriod))}</TableCell>
                                            </TableRow>
                                            <TableRow>
                                                <TableCell component="th" scope="row">
                                                    {t('Garantiecertificaatnummer') + ':'}
                                                </TableCell>
                                                <TableCell>{building.guaranteeCertNo && building.guaranteeCertNo}</TableCell>
                                            </TableRow>
                                            <TableRow>
                                                <TableCell component="th" scope="row">
                                                    {t('Geldig vanaf') + ':'}
                                                </TableCell>
                                                <TableCell>{building.guaranteeCertValidFrom && formatDate(new Date(building.guaranteeCertValidFrom))}</TableCell>
                                            </TableRow>
                                            <TableRow>
                                                <TableCell component="th" scope="row">
                                                    {t('Geldig t/m') + ':'}
                                                </TableCell>
                                                <TableCell>{building.guaranteeCertValidUntil && formatDate(new Date(building.guaranteeCertValidUntil))}</TableCell>
                                            </TableRow>
                                        </TableBody>
                                    </Table>
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
export { connectedPage as MyHomePage };