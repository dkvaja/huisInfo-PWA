import React from "react";
import { connect } from "react-redux";
import { withStyles } from "@material-ui/core/styles";
import { withTranslation } from 'react-i18next';
import { Container, Typography, Grid, CssBaseline, Box, TextField, Button, IconButton, List, isWidthUp, withWidth, AppBar, Toolbar } from "@material-ui/core";
import { history, formatFileSize, validateFile, authHeader, toBase64 } from "../../_helpers";
import Dropzone from "../../components/Dropzone";
import { AttachFile, Clear } from "@material-ui/icons";
import { MuiPickersUtilsProvider } from "@material-ui/pickers";
import DateFnsUtils from "@date-io/date-fns";
import nlLocale from "date-fns/locale/nl";
import clsx from "clsx";
import { isAbsolute, relative } from "draftjs-md-converter";

const { webApiUrl } = window.appConfig;

const styles = theme => ({
    wrapper: {
        backgroundSize: "cover",
        backgroundAttachment: "fixed",
        backgroundPosition: "center"
    },
    mainContainer: {
        [theme.breakpoints.down("xs")]: {
            padding: theme.spacing(0)
        }
    },
    container: {
        backgroundColor: theme.palette.common.white,
        maxWidth: "100%"
    },
    innerContainer: {
        padding: theme.spacing(2)
    },
    dropzoneContainer: {
        width: "40%",
        float: "left",
        padding: 8,
        margin: "-2px auto -4px",
        [theme.breakpoints.down("md")]: {
            width: '40%',
        },
        [theme.breakpoints.down("sm")]: {
            width: '50%',
        }

    },
    changedirection: {
        flexDirection: "column"
    },
    uploadcontainer: {
        display: "block",
        width: "100%",
        "&:after": {
            content: '',
            clear: "both",
            display: "block",
        }
    },

    listview: {
        width: "20%",
        float: "left",
        padding: 8,
        position: "relative",

        '& > button': {
            position: "absolute",
            top: 3,
            right: 3
        },
        "& > div": {
            width: "100%",
            padding: "50% 0px",
            height: 0,
            backgroundSize: 'cover',
            backgroundPosition: 'center',

        },
        [theme.breakpoints.down("md")]: {
            width: "20%",
        },
        [theme.breakpoints.down("sm")]: {
            width: "25%",
        },
        [theme.breakpoints.down("xs")]: {
            width: '50%',
        }

    },

    logoContainer: {
        //backgroundColor: theme.palette.common.white,
        '& > img': {
            margin: 'auto',
            maxWidth: 200,
            maxHeight: 50,
            minWidth: 100
        }
    },
    alert: {
        margin: theme.spacing(2, 0)
    },
    grow: {
        flexGrow: 1
    },
    bold: {
        fontWeight: "bold"
    },
    thumbnail: {
        maxHeight: 50, maxWidth: 50,
        margin: theme.spacing(0.5, 0.5, 0.5, 0)
    }
});

class Page extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            alert: null,
            workOrderDetails: null,
            locationId: null,
            date: new Date(),
            desc: '',
            formSubmittedOnce: false,
            locations: [],
            uploading: false,
            files: [],
            success: false
        };
        this.handleSelectFiles = this.handleSelectFiles.bind(this);
    }


    componentDidMount() {
        let { resolverId } = this.props.match.params;
        if (resolverId && resolverId.trim() !== '')
            this.GetResolverDetailsForWorkOrder();
        else
            history.push('/');
    }

    GetResolverDetailsForWorkOrder() {
        const { t } = this.props;
        let { resolverId } = this.props.match.params;
        const url = webApiUrl + 'api/RepairRequest/GetResolverDetailsForWorkOrder/' + resolverId;
        const requestOptions = {
            method: 'GET',
            headers: authHeader()
        };

        this.setState({
            workOrderDetails: null,
        });

        fetch(url, requestOptions)
            .then(res => {
                if (res.ok)
                    return res.json();
                else
                    throw res;
            })
            .then(findResponse => {
                this.setState({ workOrderDetails: findResponse });
            })
            .catch(err => {
                if (err.status === 408) {
                    this.GetResolverDetailsForWorkOrder();
                }
                else if (err.status === 400) {
                    const alert = {
                        type: 'alert-danger',
                        message: t('Link is verlopen.')
                    };
                    this.setState({ alert });
                }
                else {
                    alert(t('general.api.error'));
                }
            });
    }

    validateForm() {
        const { desc, date } = this.state;
        this.setState({ formSubmittedOnce: true });
        return (
            this.validateField(desc, true)

        );
    }

    validateField(field, validateForm = false) {
        return (!validateForm && !this.state.formSubmittedOnce) || (field && field.trim() !== '');
    }

    handleRequest = (event, IsComplete) => {
        const { t } = this.props;
        let { resolverId } = this.props.match.params;
        const { files, desc } = this.state;
        if (this.validateForm()) {
            this.setState({ uploading: true })

            const formData = new FormData()

            formData.append('resolverId', resolverId);
            formData.append('desc', desc);
            formData.append('IsComplete', IsComplete);

            for (var i = 0; i < files.length; i++) {
                formData.append('files', files[i])
            }

            fetch(webApiUrl + 'api/RepairRequest/UpdateWorkOrderByDirectLink', {
                method: 'POST',
                headers: authHeader(),
                body: formData
            })
                .then(Response => Response.json())
                .then(res => {
                    if (res.status === true) {
                        const alert = {
                            type: 'alert-success',
                            message: t('Uw werkorder is afgehandeld.')
                        };
                        this.setState({ alert, success: true, uploading: false });
                    }
                    else throw res;
                })
                .catch(e => {
                    this.setState({ uploading: false, files: [] });
                    alert(t('general.api.error'));
                })
        }
        else {
            event.preventDefault();
        }
    }

    handleChangeTextField = name => event => {
        this.setState({
            [name]: event.target.value
        })
    }

    handleDateChange = date => {
        this.setState({ date });
    }

    handleChangeSendEmail = () => {
        this.setState({
            sendEmail: !this.state.sendEmail
        })
    }

    async handleSelectFiles(selectedFiles) {

        let files = this.state.files.slice();
        for (var i = 0; i < selectedFiles.length; i++) {
            if (validateFile(selectedFiles[i], true) === true) {
                let currenturl = await toBase64(selectedFiles[i])
                selectedFiles[i].url = currenturl;
                files.push(selectedFiles[i]);
            }
        }
        this.setState({ files });
    }

    handleRemoveFile = index => {
        let files = this.state.files.slice();
        files.splice(index, 1);
        this.setState({ files });
    }

    renderImageSelector() {
        const { t, classes } = this.props;
        const { files, uploading, success } = this.state;
        const disabled = uploading === true || success === true;

        return (
            <Grid container item xs={12}>
                <Grid item xs={12}>
                    <Typography>{t('Afbeeldingen')}</Typography>
                    <Typography variant="body2" color="textSecondary">{t('Bestand toevoegen')}</Typography>
                </Grid>
                <Grid container item xs={12}>
                    <div className={classes.uploadcontainer}>
                        {
                            <div className={classes.dropzoneContainer}>
                                <Dropzone onFilesAdded={this.handleSelectFiles} disabled={disabled} uploading={uploading} accept="image/*" />
                            </div>
                        }
                        {files.map((file, index) => (
                            <div className={classes.listview}>
                                <div
                                    style={{
                                        backgroundImage: "url(" + file.url + ")",
                                    }}
                                ></div>
                                <IconButton
                                    aria-label="delete"
                                    size="small"
                                    disabled={disabled}
                                    onClick={() => this.handleRemoveFile(index)}
                                >
                                    <Clear />
                                </IconButton>
                            </div>
                        ))}
                    </div>


                </Grid>


            </Grid>
        );
    }

    render() {
        const { t, classes, width } = this.props;
        const { workOrderDetails, uploading, date, desc, alert, success } = this.state;
        const matches = isWidthUp('md', width);

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
                <Container className={classes.mainContainer}>
                    <Grid container className={classes.container}>
                        <AppBar position="sticky">
                            <Toolbar variant="dense">
                                <Typography className={`${classes.bold} ${classes.grow}`}>{t('Werkbon')}&nbsp;{workOrderDetails && workOrderDetails.workOrderNumber}</Typography>
                                {alert == null &&
                                    <>
                                        <Button
                                            aria-describedby="handleWorkOrderPopup"
                                            variant="outlined"
                                            color="inherit"
                                            style={{ marginLeft: 12 }}
                                            onClick={e => this.handleRequest(e, false)}
                                        >
                                            {t('workorder.handle.reject')}
                                        </Button>
                                        <Button
                                            aria-describedby="handleWorkOrderPopup"
                                            variant="outlined"
                                            color="inherit"
                                            style={{ marginLeft: 12 }}
                                            onClick={e => this.handleRequest(e, true)}
                                        >
                                            {t('workorder.handle.complete')}
                                        </Button>
                                    </>
                                }
                            </Toolbar>
                        </AppBar>
                        <Grid container className={classes.innerContainer} item xs={12}>
                            {
                                <Grid container alignContent="center" item xs={12} className={classes.logoContainer}>
                                    <img src={webApiUrl + "api/Config/WebLogo"} alt="JPDS" />
                                </Grid>
                            }
                            {
                                alert && (
                                    <Grid container justify="center" item xs={12}>
                                        <Box component="span" className={clsx("alert", alert.type, classes.alert)}>{alert.message}</Box>
                                    </Grid>
                                )
                            }
                            {
                                workOrderDetails &&
                                <Grid item xs={12}>
                                    <MuiPickersUtilsProvider utils={DateFnsUtils} locale={nlLocale}>
                                        <Grid container spacing={1} alignItems="flex-start">
                                            <Grid item xs={12}>
                                                <Typography>{t('Object Adres')}</Typography>
                                                <Typography variant="body2" color="textSecondary">
                                                    {workOrderDetails.address && workOrderDetails.address}
                                                </Typography>
                                            </Grid>
                                            <Grid container spacing={1} item xs={12} md={6}>
                                                <Grid item xs={12}>
                                                    <Typography>{t('Oplosser')}</Typography>
                                                    <Typography variant="body2" color="textSecondary">
                                                        {
                                                            workOrderDetails.organisationHasLogo === true &&
                                                            <img alt="" className={classes.thumbnail} src={'/api/organisation/GetOrganisationLogo/' + workOrderDetails.organisationId} />
                                                        }
                                                        {workOrderDetails.organisationName}
                                                    </Typography>
                                                </Grid>
                                                <Grid item xs={12} >
                                                    <TextField
                                                        error={!this.validateField(desc)}
                                                        label={t('Uitgevoerde werkzaamheden')}
                                                        className={classes.textField}
                                                        value={desc}
                                                        onChange={this.handleChangeTextField('desc')}
                                                        margin="dense"
                                                        variant="outlined"
                                                        rows={3}
                                                        rowsMax={7}
                                                        multiline
                                                        fullWidth
                                                        disabled={uploading}
                                                    />

                                                </Grid>
                                                {
                                                    !matches &&
                                                    <Grid item xs={12}>
                                                        {this.renderImageSelector()}
                                                    </Grid>
                                                }
                                            </Grid>
                                            {
                                                matches &&
                                                <Grid container spacing={1} item xs={12} md={6}>
                                                    {this.renderImageSelector()}
                                                </Grid>
                                            }
                                        </Grid>
                                    </MuiPickersUtilsProvider>
                                </Grid>
                            }
                        </Grid>
                    </Grid>
                </Container>
            </Grid >
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

const connectedPage = connect(mapStateToProps)(withWidth()(withTranslation()(withStyles(styles)(Page))));
export { connectedPage as WorkOrderStatusUpdatePage }