import React from "react";
import { connect } from "react-redux";
import { Link as RouterLink, Redirect } from "react-router-dom";
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
import Modal from '@material-ui/core/Modal';
import CircularProgress from '@material-ui/core/CircularProgress';
import Autorenew from "@material-ui/icons/Autorenew";
import LockOutlinedIcon from "@material-ui/icons/LockOutlined";
import Typography from "@material-ui/core/Typography";
import { makeStyles } from "@material-ui/core/styles";
import Container from "@material-ui/core/Container";
import { userActions } from "../_actions";
import { withTranslation } from 'react-i18next';

import "./login.css";

class LoginPage extends React.Component {
    constructor(props) {
        super(props);

        // reset login status
        this.props.dispatch(userActions.logout());

        this.state = {
            username: "",
            password: "",
            remember: false,
            submitted: false
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleCheckUncheck = this.handleCheckUncheck.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange(e) {
        const { name, value } = e.target;
        this.setState({ [name]: value });
    }

    handleCheckUncheck(e) {
        this.setState({ remember: !this.state.remember });
    }

    handleSubmit(e) {
        e.preventDefault();

        this.setState({ submitted: true });
        const { username, password, remember } = this.state;
        const { dispatch } = this.props;
        if (username && password) {
            dispatch(userActions.login(username, password, remember));
        }
    }

    render() {
        const { loggingIn, loggedIn, alert, t, history } = this.props;
        const { username, password, remember, submitted } = this.state;
        const { webApiUrl } = window.appConfig;
        const { cookieEnabled } = navigator;

        const referer = (history.location.state && history.location.state.from) || '/';
        if (loggedIn)
            return <Redirect to={referer} />;

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
                        {
                            //<img src="/Content/Images/Logos/Logo-JPDS-transparant.svg" width="50" alt="JPDS" />
                            <img src={webApiUrl + "api/Config/WebLogo"} style={{ maxWidth: 200, maxHeight: 50 }} alt="JPDS" />
                            //<img src="/Content/Images/Logos/logo_home4u.jpg" width="200" alt="JPDS" />
                        }
                    </Grid>

                    {
                        cookieEnabled !== true ?
                            <Box my={1} className={`alert alert-danger`}>{t('login.cookie.error')}</Box>
                            :
                            <React.Fragment>
                                <Typography component="h1" variant="h5" align="center">{t('login.title')}</Typography>

                                {
                                    alert.message && (
                                        <Box my={1} className={`alert ${alert.type}`}>{t(alert.message)}</Box>
                                    )
                                }
                                <form noValidate onSubmit={this.handleSubmit}>
                                    <TextField
                                        error={submitted && !username}
                                        required
                                        variant="standard"
                                        margin="normal"
                                        required
                                        fullWidth
                                        id="email"
                                        label={t('login.email.label')}
                                        name="username"
                                        autoComplete="email"
                                        autoFocus
                                        value={username}
                                        onChange={this.handleChange} disabled={loggingIn}
                                    />
                                    <TextField
                                        error={submitted && !password}
                                        required
                                        variant="standard"
                                        margin="normal"
                                        required
                                        fullWidth
                                        name="password"
                                        label={t('login.password.label')}
                                        type="password"
                                        id="password"
                                        autoComplete="current-password"
                                        value={password}
                                        onChange={this.handleChange} disabled={loggingIn}
                                    />
                                    <Grid container alignItems="center">
                                        <Grid item sm={6}>
                                            <FormControlLabel
                                                control={<Checkbox name="remember" color="primary" checked={remember} onChange={this.handleCheckUncheck} disabled={loggingIn} />}
                                                label={t('login.rememberme.label')}
                                                className="remember-me"
                                            />
                                        </Grid>
                                        <Grid item sm={6} align="right">
                                            <Link component={RouterLink} to="/forgot" variant="body1">{t('login.forgotpassword.text')}</Link>
                                        </Grid>
                                    </Grid>
                                    <Grid item xs={12} align="center">
                                        <Box mt={2}>
                                            <Button type="submit" variant="contained" color="primary" disabled={loggingIn}>{t('login.button')}</Button>
                                        </Box>
                                    </Grid>
                                </form>
                            </React.Fragment>
                    }
                </Grid>
                <Modal open={loggingIn === true}>
                    <Grid container direction="column" alignItems="center" justify="center" style={{ minHeight: "100vh" }}>
                        <Grid item>
                            <CircularProgress />
                        </Grid>
                    </Grid>
                </Modal>
            </Grid>
        );
    }
}

function mapStateToProps(state) {
    const { loggingIn, loggedIn } = state.authentication;
    const { alert } = state;
    return {
        loggingIn,
        loggedIn,
        alert
    };
}

const connectedLoginPage = connect(mapStateToProps)(withTranslation()(LoginPage));
export { connectedLoginPage as LoginPage };
