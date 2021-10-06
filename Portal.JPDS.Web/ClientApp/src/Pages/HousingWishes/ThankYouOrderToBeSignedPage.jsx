import React from "react";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import {
    Container,
    Grid,
    Typography, ExpansionPanel, ExpansionPanelSummary, AppBar, TextField, Button
} from "@material-ui/core";
import { withStyles, withTheme } from '@material-ui/core/styles';
import { Search } from "@material-ui/icons"
import { withTranslation } from 'react-i18next';
import clsx from "clsx";
import SelectedOptionCard from './SelectedOptionCard';

const styles = theme => ({
    mainContainer: {
        [theme.breakpoints.down("xs")]: {
            padding: theme.spacing(0)
        }
    },
    container: {
        backgroundColor: theme.palette.background.paper,
        margin: theme.spacing(5, 0, 6),
        padding: theme.spacing(5),
        [theme.breakpoints.down("xs")]: {
            marginTop: theme.spacing(0)
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
        title: '',
        desc: '',
        files: []
    };



    handleChangeTextField = name => event => {
        this.setState({
            [name]: event.target.value
        })
    }

    render() {
        const { t, classes } = this.props;
        return (
                <Container className={classes.mainContainer}>
                    <Grid container justify="center">
                        <Grid item container spacing={2} xs={12} md={7} className={classes.container}>
                            <Grid item container justify="center" xs={12}>
                                <Typography variant="h4">{t('Bedankt voor uw bestelling')}</Typography>
                            </Grid>
                            <Grid item xs={12}>
                                <Typography align="center" paragraph>
                                    Nog de laatste stap. U ontvangt van ons binnen enkele minuten een <strong>email</strong> met een link naar de pagina waar u uw offerte <strong>digitaal kunt ondertekenen.</strong>
                                    </Typography>
                            </Grid>
                            <Grid item xs={12}>
                                <Typography align="center">{t('U kunt de status van uw bestelling binnen enkele minuten zien door hier te klikken')}:</Typography>
                            </Grid>
                            <Grid item container justify="center" xs={12}>
                                <Button component={Link} to="/home" variant="contained" color="primary" className={classes.button}>{t('Te ondertekenen offertes')}</Button>
                            </Grid>
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
export { connectedPage as ThankYouOrderToBeSignedPage };