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
    ListItemSecondaryAction
} from "@material-ui/core";
import { withStyles, withTheme } from '@material-ui/core/styles';
import { Search, Add, AttachFile, Clear } from "@material-ui/icons"
import { withTranslation } from 'react-i18next';
import clsx from "clsx";
import { history, validateFile, formatFileSize, authHeader } from '../../_helpers';

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
        desc: '',
        uploading: false,
        files: []
    };


    handleRequestOption = event => {
        const { selected, t, user } = this.props;
        const { files, desc } = this.state;
        if (desc && desc.trim() !== '') {
            this.setState({ uploading: true })

            const formData = new FormData()

            formData.append('desc', desc);

            for (var i = 0; i < files.length; i++) {
                formData.append('files', files[i])
            }

            fetch(webApiUrl + 'api/shopping/RequestIndividualOption/' + encodeURI(selected.buildingId), {
                method: 'POST',
                headers: authHeader(),
                body: formData
            })
                .then(Response => Response.json())
                .then(res => {
                    alert(t('Bedankt voor uw aanvraag.'));
                    history.push(
                        '/berichten',
                        {
                            selectedChatId: res.chatId
                        }
                    );
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

    handleSelectFiles = e => {
        const selectedFiles = Array.from(e.target.files);
        let files = this.state.files.slice();
        for (var i = 0; i < selectedFiles.length; i++) {
            if (validateFile(selectedFiles[i]) === true) {
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

    render() {
        const { t, classes } = this.props;
        const { files, uploading } = this.state;
        return (
            <Container className={classes.mainContainer}>
                <Grid container>
                    <Grid item xs={12} className={classes.container}>
                        <AppBar position="sticky" expanded={false}>
                            <Toolbar variant="dense">
                                <Typography className={classes.bold}>{t('Individuele optie')}</Typography>
                            </Toolbar>
                        </AppBar>
                        <div className={classes.formData}>
                            <Grid container spacing={1} direction="column" >
                                <Grid item xs>
                                    <Typography variant="body2" color="textSecondary">
                                        {t('Hier kunt u een optie voor uw woning aanvragen die niet standaard is. Wij hebben van u een uitgebreide beschrijving nodig.')}
                                    </Typography>
                                </Grid>
                                {
                                    //<Grid item xs md={7}>
                                    //    <TextField
                                    //        label={t('Korte omschrijving')}
                                    //        className={classes.textField}
                                    //        value={this.state.title}
                                    //        onChange={this.handleChangeTextField('title')}
                                    //        margin="dense"
                                    //        variant="outlined"
                                    //        fullWidth
                                    //    />
                                    //</Grid>
                                }
                                <Grid item xs md={7}>
                                    <TextField
                                        label={t('Uitgebreide omschrijving')}
                                        className={classes.textField}
                                        value={this.state.desc}
                                        onChange={this.handleChangeTextField('desc')}
                                        margin="dense"
                                        variant="outlined"
                                        multiline
                                        fullWidth
                                        disabled={uploading}
                                    />

                                </Grid>
                                <Grid item xs md={7}>
                                    <Grid container item xs={12}>
                                        <Grid container item xs={12} alignItems="center">
                                            <Typography variant="body2" color="textSecondary">{t('Bestand toevoegen')}</Typography>
                                            <input accept="*" style={{ display: 'none' }} id="icon-button-file" type="file" multiple onChange={this.handleSelectFiles} />
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
                                </Grid>
                                <Grid item xs md={7}>
                                    <Button color="primary" variant="contained" disabled={uploading} fullWidth onClick={this.handleRequestOption} >{t('Aanvragen')}</Button>
                                </Grid>
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

const connectedPage = connect(mapStateToProps)(withTranslation()(withStyles(styles)(Page)));
export { connectedPage as RequestIndividualOptionPage };