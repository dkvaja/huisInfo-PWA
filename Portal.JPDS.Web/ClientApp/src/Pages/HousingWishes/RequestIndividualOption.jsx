import React, { useState, useEffect } from "react";
import {
    IconButton,
    makeStyles,
    Tooltip,
    Dialog,
    DialogTitle,
    DialogContent,
    TextField,
    Button,
    Typography,
    Grid,
    CircularProgress,
    CardHeader,
    List,
    ListItem,
    ListItemAvatar,
    Avatar,
    ListItemSecondaryAction,
    ListItemText
} from "@material-ui/core";
import { Add, Close, AttachFile, Clear } from "@material-ui/icons";
import { useTranslation } from "react-i18next";
import { history, validateFile, formatFileSize, authHeader } from "../../_helpers";

const { webApiUrl } = window.appConfig;

const useStyles = makeStyles((theme) => ({
    grow: {
        flexGrow: 1
    },
    dialogTitle: {
        padding: 0,
        backgroundColor: theme.palette.primary.main,
        color: theme.palette.primary.contrastText,
        '& .MuiCardHeader-content': {
            overflow: 'hidden'
        },
        '& .MuiCardHeader-action': {
            marginBottom: -8
        }
    },
    dialogContent: {
        //padding: 0
    },
}));

export default function RequestIndividualOption(props) {
    const { selectedBuilding, user, ...rest } = props;
    const { t } = useTranslation();
    const classes = useStyles();
    const [openDialog, setOpenDialog] = React.useState(false);
    const [desc, setDesc] = React.useState('');
    const [uploading, setUploading] = React.useState(false);
    const [files, setFiles] = React.useState([]);

    useEffect(() => {
    }, [selectedBuilding]);

    const handleClick = () => {
        setOpenDialog(true);
    };

    const handleClose = () => {
        setOpenDialog(false);
    };

    const handleRequestOption = event => {
        if (desc && desc.trim() !== '') {
            setUploading(true);

            const formData = new FormData()

            formData.append('desc', desc);

            for (var i = 0; i < files.length; i++) {
                formData.append('files', files[i])
            }

            fetch(webApiUrl + 'api/shopping/RequestIndividualOption/' + encodeURI(selectedBuilding.buildingId), {
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
                    setUploading(false);
                    alert(t('general.api.error'));
                })
        }
        else {
            event.preventDefault();
        }
    }

    const handleChangeDesc = event => {
        setDesc(event.target.value);
    }

    const handleSelectFiles = e => {
        const selectedFiles = Array.from(e.target.files);
        let fls = files.slice();
        for (var i = 0; i < selectedFiles.length; i++) {
            if (validateFile(selectedFiles[i]) === true) {
                fls.push(selectedFiles[i]);
            }
        }
        setFiles(fls);
    }

    const handleRemoveFile = index => {
        let fls = files.slice();
        fls.splice(index, 1);
        setFiles(fls);
    }

    return (
        <React.Fragment>
            <Tooltip title={t("Individuele optie")}>
                <IconButton color="inherit" onClick={handleClick}>
                    <Add />
                </IconButton>
            </Tooltip>
            <Dialog open={openDialog} onClose={handleClose} aria-labelledby="form-dialog-title" maxWidth='sm' fullWidth>
                <DialogTitle id="simple-dialog-title" className={classes.dialogTitle} disableTypography>
                    <CardHeader id="transition-dialog-title"
                        title={
                            <Typography variant="h6" noWrap>{t('Individuele optie')}</Typography>
                        }
                        action={
                            <React.Fragment>
                                <IconButton color="inherit" aria-label="close" onClick={handleClose}>
                                    <Close />
                                </IconButton>
                            </React.Fragment>
                        } />
                </DialogTitle>
                <DialogContent className={classes.dialogContent}>
                    <div className={classes.formData}>
                        <Grid container spacing={1} direction="column" >
                            <Grid item xs>
                                <Typography variant="body2" color="textSecondary">
                                    {t('Hier kunt u een optie voor uw woning aanvragen die niet standaard is. Wij hebben van u een uitgebreide beschrijving nodig.')}
                                </Typography>
                            </Grid>
                            <Grid item xs>
                                <TextField
                                    label={t('Uitgebreide omschrijving')}
                                    className={classes.textField}
                                    value={desc}
                                    onChange={handleChangeDesc}
                                    margin="dense"
                                    variant="outlined"
                                    multiline
                                    fullWidth
                                    disabled={uploading || user.viewOnly === true}
                                />

                            </Grid>
                            <Grid item xs>
                                <Grid container item xs={12}>
                                    <Grid container item xs={12} alignItems="center">
                                        <Typography variant="body2" color="textSecondary">{t('Bestand toevoegen')}</Typography>
                                        <input accept="*" style={{ display: 'none' }} disabled={uploading || user.viewOnly === true} id="icon-button-file" type="file" multiple onChange={handleSelectFiles} />
                                        <label htmlFor="icon-button-file" style={{ margin: 0 }}>
                                            {
                                                uploading ?
                                                    <CircularProgress color="inherit" size={24} />
                                                    :
                                                    <IconButton
                                                        color="inherit"
                                                        aria-label="upload"
                                                        component="span"
                                                        disabled={user.viewOnly === true}
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
                                                                <IconButton edge="end" aria-label="delete" disabled={uploading} onClick={() => handleRemoveFile(index)}>
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
                            <Grid item xs>
                                <Button color="primary" variant="contained" disabled={uploading || user.viewOnly === true} fullWidth onClick={handleRequestOption} >{t('Aanvragen')}</Button>
                            </Grid>
                        </Grid>
                    </div>
                </DialogContent>
            </Dialog>
        </React.Fragment>
    );
}