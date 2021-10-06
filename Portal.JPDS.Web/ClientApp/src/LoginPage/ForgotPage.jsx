import React from "react";
import { connect } from "react-redux";
import { Link as RouterLink } from "react-router-dom";
import Avatar from "@material-ui/core/Avatar";
import Button from "@material-ui/core/Button";
import Fab from "@material-ui/core/Fab";
import CssBaseline from "@material-ui/core/CssBaseline";
import TextField from "@material-ui/core/TextField";
import FormControlLabel from "@material-ui/core/FormControlLabel";
import Checkbox from "@material-ui/core/Checkbox";
import Link from "@material-ui/core/Link";
import Grid from "@material-ui/core/Grid";
import Box from "@material-ui/core/Box";
import Autorenew from "@material-ui/icons/Autorenew";
import LockOutlinedIcon from "@material-ui/icons/LockOutlined";
import Typography from "@material-ui/core/Typography";
import { makeStyles } from "@material-ui/core/styles";
import Container from "@material-ui/core/Container";
import { userActions } from "../_actions";
import "./login.css";
import { withTranslation } from 'react-i18next';
import Modal from '@material-ui/core/Modal';
import CircularProgress from '@material-ui/core/CircularProgress';
import { history } from "../_helpers";

class ForgotPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            email: "",
            requestSubmitted: false
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange(e) {
        const { name, value } = e.target;
        this.setState({ [name]: value });
    }

    handleSubmit(e) {
        e.preventDefault();

        this.setState({ requestSubmitted: true });
        const { email } = this.state;
        const { dispatch } = this.props;
        if (email) {
            dispatch(userActions.forgotPassword(email));
        }
    }

    render() {
        const { emailSent, alert, t, noError } = this.props;
        const { email, requestSubmitted } = this.state;
        const { webApiUrl } = window.appConfig;
        return (
            <Grid
                container
                spacing={0}
                direction="column"
                alignItems="center"
                justify="center"
                className="login-grid"
                style={{ minHeight: "100vh", backgroundImage: "url(" + webApiUrl + "api/Config/WebBackground)" }}
            >
                <CssBaseline />
                <Grid item className="login-container">
                    <Grid container spacing={0}
                        direction="column"
                        alignItems="center"
                        justify="center">
                        <img src={webApiUrl + "api/Config/WebLogo"} style={{ maxWidth: 200, maxHeight: 50 }} alt="JPDS" />
                    </Grid>
                    <Typography component="h1" variant="h5" align="center">{t('login.forgotpassword.text')}</Typography>

                    {
                        !noError && !emailSent && alert.message &&
                        (<Box my={1} className={`alert ${alert.type}`}>{t(alert.message)}</Box>)

                    }
                    {
                        !emailSent &&
                        <form noValidate onSubmit={this.handleSubmit}>
                            <TextField
                                error={requestSubmitted && !email}
                                required
                                variant="standard"
                                margin="normal"
                                required
                                fullWidth
                                id="email"
                                label="E-mailadres"
                                name="email"
                                autoComplete="email"
                                autoFocus
                                value={email}
                                onChange={this.handleChange}
                                disabled={noError}
                            />
                            <Grid container alignItems="center">
                                <Grid item sm={12} align="right">
                                    <Link component={RouterLink} to="/login" variant="body1">{t('general.back')}</Link>
                                </Grid>
                            </Grid>
                            <Grid item xs={12} align="center">
                                <Box mt={2}>
                                    <Button type="submit" variant="contained" color="primary" disabled={noError}>{t('general.send')}</Button>
                                </Box>
                            </Grid>
                        </form>
                    }

                    {
                        emailSent && (
                            <React.Fragment>
                                <Grid item xs={12} align="center">
                                    <Box mt={2}>
                                        <label>{t('reset.email.success')}</label>
                                    </Box>
                                </Grid><Grid item xs={12} align="center">
                                    <Box mt={2}>
                                        <Button type="submit" variant="contained" color="primary" onClick={() => history.push('/login')}>{t('general.ok')}</Button>
                                    </Box>
                                </Grid>
                            </React.Fragment>)
                    }

                    {
                        <Modal open={noError === true}>
                            <Grid container direction="column" alignItems="center" justify="center" style={{ minHeight: "100vh" }}>
                                <Grid item>
                                    <CircularProgress />
                                </Grid>
                            </Grid>
                        </Modal>
                    }
                </Grid>
            </Grid>
        );
    }
}

function mapStateToProps(state) {
    const { emailSent, noError } = state.authentication;
    const { alert } = state;
    return {
        emailSent,
        alert,
        noError
    };
}

const connectedForgotPage = connect(mapStateToProps)(withTranslation()(ForgotPage));
export { connectedForgotPage as ForgotPage };
