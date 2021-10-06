import React from "react";
import { connect } from "react-redux";
import {
    Grid,
    Typography,
    Button,
    IconButton,
    TextField,
    DialogContent,
    Box,
} from "@material-ui/core";
import { alertActions } from "../_actions";
import { history } from "../_helpers";
import { withStyles, withTheme } from "@material-ui/core/styles";
import { withTranslation } from "react-i18next";
import { userService } from "../_services";
import Dialog from "@material-ui/core/Dialog";
import MuiDialogTitle from "@material-ui/core/DialogTitle";
import CloseIcon from "@material-ui/icons/Close";
import MuiAlert from "@material-ui/lab/Alert";
import LockRoundedIcon from "@material-ui/icons/LockRounded";
import LockOpenRoundedIcon from "@material-ui/icons/LockOpenRounded";
import DialogActions from "@material-ui/core/DialogActions";

const { webApiUrl } = window.appConfig;

const styles = (theme) => ({
    heading: {
        color: theme.palette.primary.contrastText,
        backgroundColor: theme.palette.primary.main,
        cursor: "default !important",
    },
    bold: {
        fontWeight: "bold",
    },
    fullWidth: {
        width: "100%",
    },
    button: {
        margin: theme.spacing(1, 2),
    },
    formElementGrid: {
        flexGrow: 1,
        maxWidth:'calc(100% - 32px)'
    },
    closeButton: {
        position: "absolute",
        right: theme.spacing(1),
        top: theme.spacing(0.5),
    },
    dialogTitle: {
        padding: theme.spacing(2, 4),
        backgroundColor: theme.palette.primary.main,
        color: theme.palette.primary.contrastText,
        "& .MuiCardHeader-content": {
            overflow: "hidden",
        },
        "& .MuiCardHeader-action": {
            marginBottom: -8,
        },
    },
    dialogContent: {
        padding: 0,
        justifyContent: "center",
    },
    imgAlign: {
        marginTop: theme.spacing(3.5),
    },
});

const DialogTitle = withStyles(styles)((props) => {
    const { children, classes, onClose, ...other } = props;
    return (
        <MuiDialogTitle disableTypography className={classes.root} {...other}>
            <Typography variant="h6">{children}</Typography>
            {onClose ? (
                <IconButton
                    aria-label="close"
                    color="inherit"
                    className={classes.closeButton}
                    onClick={onClose}
                >
                    <CloseIcon />
                </IconButton>
            ) : null}
        </MuiDialogTitle>
    );
});

function Alert(props) {
    return <MuiAlert elevation={6} variant="filled" {...props} />;
}

class Page extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            changePasswordReq: {
                oldPassword: "",
                newPassword: "",
                confirmPassword: "",
            },
            isOpen: this.props.changePwdOpen,
            submitted: false,
            oldError: false,
            newError: false,
            oldPasswordErrorCode: "",
            newPasswordErrorCode: "",
        };

        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);
    }

    async handleChange(event) {
        const { name, value } = event.target;
        const { changePasswordReq } = this.state;
        if (name === "oldPassword") {
            this.setState({ oldError: false });
        } else if (name === "newPassword") {
            this.setState({ newError: false });
        }
        this.setState({
            changePasswordReq: {
                ...changePasswordReq,
                [name]: value,
            },
        });
    }

    handleSubmit = (event) => {
        event.preventDefault();
        this.setState({ submitted: true, oldError: false, newError: false });
        const { changePasswordReq } = this.state;
        var regularCheck = new RegExp(
            "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])(?=.{8,})"
        );

        let newPasswordRegCheck = regularCheck.test(changePasswordReq.newPassword);

        const { dispatch } = this.props;

        if (
            changePasswordReq.oldPassword &&
            changePasswordReq.newPassword &&
            changePasswordReq.confirmPassword &&
            changePasswordReq.newPassword == changePasswordReq.confirmPassword &&
            newPasswordRegCheck
        ) {
            userService.changePassword(changePasswordReq).then(
                (response) => {
                    if (response.status === 200) {
                        history.push("/login");
                        dispatch(alertActions.success("Uw wachtwoord is succesvol gewijzigd."));
                    }
                },
                (error) => {
                    if (error === "Old password and new password is same") {
                        this.setState({
                            newError: true,
                            newPasswordErrorCode: "Uw wachtwoord moet verschillen van de vorige twee wachtwoorden"
                        });
                    } else if (error === "Old password is incorrect") {
                        this.setState({
                            oldError: true,
                            oldPasswordErrorCode: "Het oude wachtwoord is onjuist, voer het juiste wachtwoord in"
                        });
                    }
                }
            );
        }
    };

    render() {
        const { alert, t, classes } = this.props;
        const { changePasswordReq, submitted } = this.state;

        const handleClose = () => {
            this.setState({
                isOpen: false,
            });
            this.props.onChangePasswordClose(false);
        };

        var regularCheck = new RegExp(
            "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])(?=.{8,})"
        );

        return (
            <div>
                <Dialog
                    onClose={handleClose}
                    aria-labelledby="max-width-dialog-title"
                    fullWidth={true}
                    maxWidth="xs"
                    open={this.props.changePwdOpen}
                //disableBackdropClick="true"
                >
                    <DialogTitle
                        id="customized-dialog-title"
                        className={classes.dialogTitle}
                        onClose={handleClose}
                    >
                        <Typography className={classes.bold}>
                            {t("layout.navbar.changepassword")}
                        </Typography>
                    </DialogTitle>

                    <DialogContent>

                        <Grid container alignItems="flex-start" spacing={1}>
                            <Grid item sm={1}>
                                <LockRoundedIcon className={classes.imgAlign} />
                            </Grid>
                            <Grid item sm={11} className={classes.formElementGrid}>
                                <TextField
                                    id="password"
                                    error={
                                        (submitted && !changePasswordReq.oldPassword) ||
                                        (submitted && this.state.oldError)
                                    }
                                    label={t("Huidige wachtwoord")}
                                    helperText={
                                        submitted && !changePasswordReq.oldPassword
                                            ? "Huidige wachtwoord is vereist"
                                            : (submitted && this.state.oldError)
                                                ? this.state.oldPasswordErrorCode
                                                : ""
                                    }
                                    autoFocus
                                    margin="dense"
                                    type="password"
                                    name="oldPassword"
                                    fullWidth
                                    onChange={this.handleChange}
                                />
                            </Grid>
                        </Grid>


                        <Grid container alignItems="flex-start" spacing={1}>
                            <Grid item sm={1}>
                                <LockOpenRoundedIcon className={classes.imgAlign} />
                            </Grid>
                            <Grid item sm={11} className={classes.formElementGrid}>
                                <TextField
                                    id="newPassword"
                                    error={
                                        submitted &&
                                        (changePasswordReq.newPassword.length === 0 ||
                                            !regularCheck.test(changePasswordReq.newPassword) ||
                                            (submitted && this.state.newError)
                                        )
                                    }
                                    label={t("Nieuwe wachtwoord")}
                                    margin="dense"
                                    type="password"
                                    name="newPassword"
                                    fullWidth
                                    onChange={this.handleChange}
                                    helperText={
                                        submitted && changePasswordReq.newPassword.length === 0
                                            ? "Nieuwe wachtwoord is vereist"
                                            : submitted &&
                                                !regularCheck.test(changePasswordReq.newPassword)
                                                ? "Wachtwoord moet één hoofdletter, één kleine letter, één cijfer en één speciaal teken bevatten en de lengte moet 8 tekens zijn"
                                                : this.state.newError ? this.state.newPasswordErrorCode : ""
                                    }
                                />
                            </Grid>
                        </Grid>


                        <Grid container alignItems="flex-start" spacing={1}>
                            <Grid item sm={1}>
                                <LockOpenRoundedIcon className={classes.imgAlign} />
                            </Grid>
                            <Grid item sm={11} className={classes.formElementGrid}>
                                <TextField
                                    id="confirmPassword"
                                    error={
                                        (submitted &&
                                            changePasswordReq.confirmPassword.length === 0) ||
                                        (submitted &&
                                            changePasswordReq.confirmPassword &&
                                            changePasswordReq.confirmPassword !==
                                            changePasswordReq.newPassword)
                                    }
                                    label={t("Bevestigen nieuwe wachtwoord")}
                                    margin="dense"
                                    type="password"
                                    name="confirmPassword"
                                    fullWidth
                                    onChange={this.handleChange}
                                    helperText={
                                        submitted &&
                                            changePasswordReq.confirmPassword.length === 0
                                            ? "Bevestig wachtwoord is vereist"
                                            : submitted &&
                                                changePasswordReq.confirmPassword &&
                                                changePasswordReq.confirmPassword !==
                                                changePasswordReq.newPassword
                                                ? "Bevestig wachtwoord komt niet overeen met het nieuwe wachtwoord"
                                                : ""
                                    }
                                />
                            </Grid>
                        </Grid>
                    </DialogContent>

                    <DialogActions>
                        <Button
                            color="primary"
                            variant="contained"
                            className={classes.button}
                            onClick={this.handleSubmit}
                        >
                            {t("Opslaan")}
                        </Button>
                        {/* <Button
              color="primary"
              variant="outlined"
              className={classes.margin}
              color="primary"
              onClick={() => history.push("/home")}
            >
              {t("Annuleren")}
            </Button> */}
                    </DialogActions>
                </Dialog>
            </div>
        );
    }
}

function mapStateToProps(state) {
    const { authentication, buildings, profiles } = state;
    const { user } = authentication;
    const { selected } = buildings;
    const { alert } = state;
    return {
        profiles,
        user,
        selected,
        alert,
    };
}

const connectedPage = connect(mapStateToProps)(
    withTranslation()(withStyles(styles)(Page))
);
export { connectedPage as ChangePasswordModal };
