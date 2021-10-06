import React from "react";
import { connect } from "react-redux";
import {
    Container,
    Grid,
    Typography,
    AppBar,
    TextField,
    Button,
    Toolbar,

    CircularProgress,
    IconButton,
    List,
    ListItem,
    ListItemAvatar,
    ListItemText,
    Avatar,
    ListItemSecondaryAction,

    isWidthUp,
    withWidth,
    FormControlLabel,
    Checkbox,
    FormControl,
    InputLabel,
    Select,
    MenuItem
} from "@material-ui/core";
import { withStyles, withTheme } from '@material-ui/core/styles';
import { Search, Add, AttachFile, Clear } from "@material-ui/icons"
import { withTranslation } from 'react-i18next';
import clsx from "clsx";
import { history, validateFile, formatFileSize, authHeader, nl2br } from '../../_helpers';

const { webApiUrl } = window.appConfig;

const styles = theme => ({
    heading: {
        color: theme.palette.primary.contrastText,
        backgroundColor: theme.palette.primary.main,
        cursor: 'default !important'
    },
    bold: {
        fontWeight: "bold"
    },
    card: {
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
    formData: {
        padding: theme.spacing(2),
    },
    fullWidth: {
        width: '100%'
    },
    stickyAppBar: {
        background: 'none',
        boxShadow: 'none'
    }
});


class Page extends React.Component {
    state = {
        locationId: null,
        desc: '',
        detailedDesc: '',
        preferredAppointmentTime: '',
        sendEmail: false,
        formSubmittedOnce: false,
        locations: [],
        warningText: '',
        uploading: false,
        files: []
    };

    componentDidMount() {
        this.GetRepairRequestLocations();
        this.GetRepairRequestAddWarningText();
    }

    GetRepairRequestLocations() {
        const url = webApiUrl + 'api/RepairRequest/GetRepairRequestLocations';
        const requestOptions = {
            method: 'GET',
            headers: authHeader()
        };

        this.setState({
            locations: null,
        });

        fetch(url, requestOptions)
            .then(Response => Response.json())
            .then(findResponse => {
                this.setState({ locations: findResponse });
            });
    }

    GetRepairRequestAddWarningText() {
        const url = webApiUrl + 'api/RepairRequest/GetRepairRequestAddWarningText';
        const requestOptions = {
            method: 'GET',
            headers: authHeader()
        };

        this.setState({
            locations: null,
        });

        fetch(url, requestOptions)
            .then(Response => Response.json())
            .then(findResponse => {
                this.setState({ warningText: findResponse.warningText });
            });
    }

    validateForm() {
        const { selected } = this.props;
        const { files, locationId, desc, detailedDesc, preferredAppointmentTime } = this.state;
        this.setState({ formSubmittedOnce: true });
        return (
            selected
            && this.validateField(desc, true)
            && this.validateField(detailedDesc, true)
            && this.validateField(locationId, true)
            && this.validateField(preferredAppointmentTime, true)
        );
    }

    validateField(field, validateForm = false) {
        return (!validateForm && !this.state.formSubmittedOnce) || (field && field.trim() !== '');
    }

    handleRequest = event => {
        const { selected, t, user } = this.props;
        const { files, locationId, desc, detailedDesc, preferredAppointmentTime, sendEmail } = this.state;
        if (this.validateForm()) {
            this.setState({ uploading: true })

            const formData = new FormData()

            formData.append('buildingId', selected.buildingId);
            formData.append('locationId', locationId);
            formData.append('desc', desc);
            formData.append('detailedDesc', detailedDesc);
            formData.append('preferredAppointmentTime', preferredAppointmentTime);
            formData.append('desc', desc);
            formData.append('SendMailToReporter', sendEmail);

            for (var i = 0; i < files.length; i++) {
                formData.append('files', files[i])
            }

            fetch(webApiUrl + 'api/repairRequest/AddRepairRequest', {
                method: 'POST',
                headers: authHeader(),
                body: formData
            })
                .then(Response => Response.json())
                .then(res => {
                    alert(t('We hebben uw melding in goede orde ontvangen. Via het nazorg portaal houden we u op de hoogte van de voortgang.'));
                    history.push('/nazorg/details/' + res.requestId);
                })
                .catch(e => {
                    this.setState({ uploading: false });
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

    handleChangeSendEmail = () => {
        this.setState({
            sendEmail: !this.state.sendEmail
        })
    }

    handleSelectFiles = e => {
        const selectedFiles = Array.from(e.target.files);
        let files = this.state.files.slice();
        for (var i = 0; i < selectedFiles.length; i++) {
            if (validateFile(selectedFiles[i], true) === true) {
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
        const { files, uploading } = this.state;
        return (
            <Grid container item xs={12}>
                <Grid item xs={12}>
                    <Typography>{t('Afbeeldingen')}</Typography>
                </Grid>
                <Grid container item xs={12} alignItems="center">
                    <Typography variant="body2" color="textSecondary">{t('Bestand toevoegen')}</Typography>
                    <input accept="image/*" style={{ display: 'none' }} id="icon-button-file" type="file" multiple onChange={this.handleSelectFiles} />
                    <label htmlFor="icon-button-file" style={{ margin: 0 }}>
                        {
                            uploading ?
                                <CircularProgress color="inherit" size={24} />
                                :
                                <IconButton
                                    color="inherit"
                                    aria-label="upload"
                                    component="span"
                                >
                                    <Add />
                                </IconButton>
                        }
                    </label>
                </Grid>
                <Grid container item xs={12}>
                    {
                        <List dense className={classes.grow}>
                            {
                                files.map((file, index) => (
                                    <ListItem key={index}>
                                        <ListItemAvatar>
                                            <Avatar>
                                                <AttachFile />
                                            </Avatar>
                                        </ListItemAvatar>
                                        <ListItemText
                                            primary={file.name}
                                            secondary={formatFileSize(file.size)}
                                        />
                                        <ListItemSecondaryAction>
                                            <IconButton edge="end" aria-label="delete" disabled={uploading} onClick={() => this.handleRemoveFile(index)}>
                                                <Clear />
                                            </IconButton>
                                        </ListItemSecondaryAction>
                                    </ListItem>
                                ))
                            }
                        </List>
                    }
                </Grid>
            </Grid>
        );
    }

    render() {
        const { t, classes, selected, width } = this.props;
        const { locations, uploading, locationId, desc, detailedDesc, preferredAppointmentTime, sendEmail, warningText } = this.state;
        const matches = isWidthUp('md', width);

        return (
            <Container className={classes.mainContainer}>
                <Grid container>
                    <Grid item xs={12} className={classes.container}>
                        <AppBar position="sticky" expanded={false}>
                            <Toolbar variant="dense">
                                <Typography className={classes.bold}>{t('Nieuwe melding')}</Typography>
                            </Toolbar>
                        </AppBar>
                        <div className={classes.formData}>
                            <Grid container spacing={1} alignItems="flex-start">
                                <Grid item xs={12}>
                                    <Typography>{t('Object Adres')}</Typography>
                                    <Typography variant="body2" color="textSecondary">
                                        {selected && selected.address && selected.address}
                                    </Typography>
                                </Grid>
                                <Grid container spacing={1} item xs={12} md={6}>
                                    <Grid item xs={12}>
                                        <Typography>{t('Melding')}</Typography>
                                    </Grid>
                                    <Grid item xs={12}>
                                        <FormControl
                                            variant="outlined"
                                            margin="dense"
                                            fullWidth
                                            disabled={uploading}
                                        >
                                            <InputLabel id="demo-simple-select-label">{t('Locatie')}</InputLabel>
                                            <Select
                                                error={!this.validateField(locationId)}
                                                labelId="demo-simple-select-label"
                                                id="demo-simple-select"
                                                value={locationId}
                                                onChange={this.handleChangeTextField('locationId')}
                                                label={t('Locatie')}
                                            >
                                                <MenuItem value="">&nbsp;</MenuItem>
                                                {
                                                    locations && locations.map((location, index) => (
                                                        <MenuItem key={index} value={location.id}>{location.name}</MenuItem>
                                                    ))
                                                }
                                            </Select>
                                        </FormControl>
                                    </Grid>
                                    {
                                        <Grid item xs={12}>
                                            <TextField
                                                error={!this.validateField(desc)}
                                                label={t('Omschrijving')}
                                                className={classes.textField}
                                                value={desc}
                                                onChange={this.handleChangeTextField('desc')}
                                                margin="dense"
                                                variant="outlined"
                                                fullWidth
                                                disabled={uploading}
                                            />
                                        </Grid>
                                    }
                                    <Grid item xs={12} >
                                        <TextField
                                            error={!this.validateField(detailedDesc)}
                                            label={t('Toelichting')}
                                            className={classes.textField}
                                            value={detailedDesc}
                                            onChange={this.handleChangeTextField('detailedDesc')}
                                            margin="dense"
                                            variant="outlined"
                                            rows={2}
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
                                    <Grid item xs={12}>
                                        <TextField
                                            error={!this.validateField(preferredAppointmentTime)}
                                            label={t(' Voorkeurstijdstip afspraak')}
                                            className={classes.textField}
                                            value={preferredAppointmentTime}
                                            onChange={this.handleChangeTextField('preferredAppointmentTime')}
                                            margin="dense"
                                            variant="outlined"
                                            fullWidth
                                            disabled={uploading}
                                        />
                                    </Grid>
                                    <Grid item xs={12}>
                                        <FormControlLabel
                                            control={
                                                <Checkbox
                                                    checked={sendEmail}
                                                    onChange={this.handleChangeSendEmail}
                                                    value="checkedB"
                                                    color="primary"
                                                    size="small"
                                                />
                                            }
                                            label={
                                                <Typography variant="body2">{t('Stuur een kopie van deze melding naar mijn e-mailadres')}</Typography>
                                            }
                                        />
                                    </Grid>
                                    <Grid item xs={12}>
                                        <Typography variant="body2">{nl2br(warningText)}</Typography>
                                    </Grid>
                                    <Grid item xs md={12}>
                                        <Button color="primary" variant="contained" disabled={uploading} fullWidth onClick={this.handleRequest} >{t('Versturen')}</Button>
                                    </Grid>
                                </Grid>
                                {
                                    matches &&
                                    <Grid container spacing={1} item xs={12} md={6}>
                                        {this.renderImageSelector()}
                                    </Grid>
                                }
                            </Grid>
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

const connectedPage = connect(mapStateToProps)(withWidth()(withTranslation()(withStyles(styles)(Page))));
export { connectedPage as AddNewRepairRequestPage };