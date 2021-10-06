import React from "react";
import { connect } from "react-redux";
import { Link as RouterLink } from "react-router-dom";
import { Button, CssBaseline, TextField, Link, Grid, Box, Modal, CircularProgress, Typography } from "@material-ui/core";
import { userActions } from "../_actions";
import { withTranslation } from 'react-i18next';
import "./login.css";
import queryString from 'query-string';

const { webApiUrl } = window.appConfig;

class ResetPage extends React.Component {
    constructor(props) {
        super(props);
        this.props.dispatch(userActions.logout());
        this.state = {
            isValidLink: false,
            email: "",
            newPassword: "",
            confirmPassword: "",
            submitted: false,
            AccessToken: ""
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
        this.setState({ submitted: true });
        const { email, newPassword, confirmPassword } = this.state;
        const { dispatch } = this.props;
        if ((newPassword && confirmPassword) && (newPassword == confirmPassword)) {
            dispatch(userActions.resetPassword(this.state.AccessToken, newPassword));
        }
    }

    componentDidMount() {
        const values = queryString.parse(this.props.location.search);
        this.state.AccessToken = values.AccessToken;
        this.GetUserNameForResetPassword();
    }

    GetUserNameForResetPassword() {
        var headers = {
            'Authorization': 'Bearer ' + this.state.AccessToken,
            'Content-Type': 'application/json'
        };

        const url = webApiUrl + 'api/users/GetUserNameForResetPassword';
        const requestOptions = {
            method: 'GET',
            headers
        };

        fetch(url, requestOptions)
            .then(Response => Response.json())
            .then(findResponse => {
                if (findResponse.userName != null && findResponse.userName != "") {
                    this.setState({
                        isValidLink: true,
                        email: findResponse.userName
                    })
                }
                else {
                    this.setState({
                        isValidLink: false
                    })
                }
            });
    }

    render() {
        const { t, resetSuccessful, alert, noError } = this.props;
        const { isValidLink, email, newPassword, confirmPassword, submitted } = this.state;

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
                            <img src={webApiUrl + "api/Config/WebLogo"} style={{ maxWidth: 200, maxHeight: 50 }} alt="JPDS" />
                        }
                    </Grid>
                    {
                        (!isValidLink && !resetSuccessful) &&
                        <Box my={1} className={`alert alert-danger`}>{t('reset.invalid.error')}</Box>

                    }
                    {
                        (isValidLink && !resetSuccessful) &&
                        <React.Fragment>
                            <Typography component="h1" variant="h5" align="center">{t('reset.title')}</Typography>
                            {
                                alert.message && (
                                    <Box my={1} className={`alert ${alert.type}`}>{t(alert.message)}</Box>
                                )
                            }
                            <form noValidate onSubmit={this.handleSubmit}>
                                <TextField
                                    variant="standard"
                                    margin="normal"
                                    required
                                    fullWidth
                                    id="email"
                                    label={t('login.email.label')}
                                    name="email"
                                    autoComplete="email"
                                    autoFocus
                                    value={email}
                                    disabled={true}
                                />
                                <TextField
                                    error={submitted && !newPassword}
                                    required
                                    variant="standard"
                                    margin="normal"
                                    required
                                    fullWidth
                                    id="newPassword"
                                    label={t('reset.newPassword.label')}
                                    type="password"
                                    name="newPassword"
                                    autoComplete="newPassword"
                                    autoFocus
                                    value={newPassword}
                                    onChange={this.handleChange} disabled={noError}
                                />
                                <TextField
                                    error={(submitted && !confirmPassword) || (newPassword != confirmPassword)}
                                    required
                                    variant="standard"
                                    margin="normal"
                                    required
                                    fullWidth
                                    name="confirmPassword"
                                    label={t('reset.confirmPassword.label')}
                                    type="password"
                                    id="confirmPassword"
                                    value={confirmPassword}
                                    onChange={this.handleChange} disabled={noError}
                                />
                                <Grid item xs={12} align="center">
                                    <Box mt={2}>
                                        <Button type="submit" variant="contained" color="primary" disabled={noError}>{t('reset.button')}</Button>
                                    </Box>
                                </Grid>
                            </form>
                        </React.Fragment>
                    }
                    {
                        resetSuccessful &&
                        (<Grid item xs={12} align="center">
                            <Box mt={2}>
                                <label>{t('reset.change.success')}</label>
                                <br />
                                <Link component={RouterLink} to="/login" variant="body1">{t('login.button')}</Link>
                            </Box>
                        </Grid>)
                    }

                    {
                        submitted &&
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
    const { alert } = state;
    const { resetSuccessful, noError } = state.authentication;
    return {
        alert,
        resetSuccessful,
        noError
    };
}

const connectedResetPage = connect(mapStateToProps)(withTranslation()(ResetPage));
export { connectedResetPage as ResetPage };
