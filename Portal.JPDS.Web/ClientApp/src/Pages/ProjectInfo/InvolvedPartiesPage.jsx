import React from "react";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import {
    Container,
    Grid,
    Typography, ExpansionPanel, ExpansionPanelSummary, AppBar, TextField, Button, Toolbar, Table, TableBody, TableRow, TableCell, Avatar, ExpansionPanelDetails, IconButton
} from "@material-ui/core";
import { withStyles, withTheme } from '@material-ui/core/styles';
import { Search, Person, ArrowDropDown, CloudDownload } from "@material-ui/icons"
import { userAccountTypeConstants } from "../../_constants"
import { withTranslation } from 'react-i18next';
import clsx from "clsx";
import { authHeader } from "../../_helpers";

const { webApiUrl } = window.appConfig;

const styles = theme => ({
    grow: {
        flexGrow: 1
    },
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
    expansionPanel: {
        width: '100%',
        '& .MuiExpansionPanelSummary-root.Mui-expanded': {
            minHeight: 48,
        },
        '& .MuiExpansionPanelSummary-content.Mui-expanded': {
            margin: '12px 0'
        }
    },
    documentHeading: {
        color: theme.palette.primary.contrastText,
        backgroundColor: theme.palette.primary.light,
        '& .MuiExpansionPanelSummary-content': {
            display: 'block',
            overflow: 'hidden',
        },
        '& .MuiIconButton-root': {
            color: "inherit"
        }
    },
    qaHeaderDetails: {
        display: 'block',
        padding: 0
    },
    qaQuestion: {
        backgroundColor: theme.palette.grey[100],
    },
    expansionPanelDetails: {
        padding: theme.spacing(1, 3)
    },
    button: {
        '&:hover': {
            color: theme.palette.primary.contrastText
        }
    }
});


class Page extends React.Component {
    state = {
        involvedParties: []
    };

    componentDidMount() {
        this.UpdateInvolvedPartiesInfo();
    }

    componentDidUpdate(prevProps, prevState) {
        if (!prevProps.selected || !this.props.selected || prevProps.selected.projectId.toUpperCase() !== this.props.selected.projectId.toUpperCase()) {
            this.UpdateInvolvedPartiesInfo();
        }
    }

    UpdateInvolvedPartiesInfo() {
        const { selected } = this.props;
        if (selected) {
            const url = webApiUrl + 'api/home/GetInvolvedParties/' + encodeURI(selected.projectId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({ involvedParties: findResponse });
                });
        }
    }

    render() {
        const { t, classes, user } = this.props;
        const { involvedParties } = this.state;
        const isBuyer = user.type === userAccountTypeConstants.buyer;
        return (
            <Container className={classes.mainContainer}>
                <Grid container>
                    <Grid item container xs={12} className={classes.container}>
                        <AppBar position="sticky">
                            <Toolbar variant="dense">
                                <Typography className={classes.bold}>{t('Betrokken partijen')}</Typography>
                            </Toolbar>
                        </AppBar>
                        <div style={{ width: '100%' }}>
                            {
                                involvedParties.map((party, indexHeader) => (
                                    <ExpansionPanel key={indexHeader} className={classes.expansionPanel} defaultExpanded={true}>
                                        <ExpansionPanelSummary
                                            expandIcon={<ArrowDropDown />}
                                            aria-controls={'panel-cat-' + indexHeader + '-content'}
                                            id={'panel-cat-' + indexHeader + '-header'} className={classes.documentHeading}
                                        >
                                            <Typography className={classes.bold} noWrap>{party.organisationNamePart}</Typography>
                                            <Typography component="p" variant="caption" noWrap>{(isBuyer === false ? '(' + party.productCode + ') ' : '') + party.productDescription}</Typography>
                                        </ExpansionPanelSummary>
                                        <ExpansionPanelDetails className={classes.documentHeaderDetails}>
                                            <div>
                                                {
                                                    <Table size="small">
                                                        <TableBody>
                                                            <TableRow >
                                                                <TableCell component="th" scope="row">
                                                                    {t('Postadres') + ':'}
                                                                </TableCell>
                                                                <TableCell>{party.organisationPostAdress_StreeNumberAddition}</TableCell>
                                                            </TableRow>
                                                            <TableRow>
                                                                <TableCell component="th" scope="row">
                                                                    {t('Postcode') + ':'}
                                                                </TableCell>
                                                                <TableCell>{party.organisationPostAdress_Postcode}</TableCell>
                                                            </TableRow>
                                                            <TableRow>
                                                                <TableCell component="th" scope="row">
                                                                    {t('Plaats') + ':'}
                                                                </TableCell>
                                                                <TableCell>{party.organisationPostAdress_Place}</TableCell>
                                                            </TableRow>
                                                            <TableRow>
                                                                <TableCell component="th" scope="row">
                                                                    {t('Telefoon') + ':'}
                                                                </TableCell>
                                                                <TableCell>{party.organisationTelephone}</TableCell>
                                                            </TableRow>
                                                            <TableRow>
                                                                <TableCell component="th" scope="row">
                                                                    {t('Website') + ':'}
                                                                </TableCell>
                                                                <TableCell>{party.organisationWebsite}</TableCell>
                                                            </TableRow>
                                                            <TableRow>
                                                                <TableCell component="th" scope="row">
                                                                    {t('E-mailadres') + ':'}
                                                                </TableCell>
                                                                <TableCell>{party.organisationEmail}</TableCell>
                                                            </TableRow>
                                                        </TableBody>
                                                    </Table>
                                                }
                                            </div>
                                        </ExpansionPanelDetails>
                                    </ExpansionPanel>
                                ))
                            }
                        </div>
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
export { connectedPage as InvolvedPartiesPage };