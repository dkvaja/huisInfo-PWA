import React, { useRef, useState } from 'react';
import { makeStyles, Typography, Dialog, DialogContent, Grid, FormControlLabel, Checkbox, IconButton } from '@material-ui/core';
import { Close, CreateNewFolder, Folder } from '@material-ui/icons';
import EditDossierObjects from '../EditDossierObjects';
import { useTranslation } from 'react-i18next';
import ExistingImageModal from './ExistingImageModal';

const UploadFiles = ({ open, onClose, buildings, handleSelectExistingImages, handleSubmit, handlePreviewOfFiles, selectedDossier, isSingle, ...props }) => {
    const [openEditObjects, setOpenEditObjects] = useState(false);
    const [isObjectsRightsAssign, setIsObjectsRightsAssign] = useState(false);
    const [isExistingImageModal, setIsExistingImageModal] = useState(false);
    const { t } = useTranslation()
    const classes = useStyles();
    const fileInputRef = useRef();

    const handleSaveObjects = (object) => {
        setIsObjectsRightsAssign(object)
        setOpenEditObjects(false)
    }
    const handleCloseObjects = (object) => {
        setOpenEditObjects(false)
    }
    const onFilesAdded = (evt) => {
        if (props.disabled) return
        const files = evt.target.files
        if (handleSubmit) {
            const array = fileListToArray(files)
            handleSubmit(array)
        }
        onClose();
    }

    const fileListToArray = (list) => {
        const array = []
        for (var i = 0; i < list.length; i++) {
            array.push(list.item(i))
        }
        return array
    }


    const openFileDialog = () => {
        fileInputRef.current.click();
    }

    return (
        <Dialog open={open} onClose={onClose}>
            <ExistingImageModal onselect={handleSelectExistingImages} handlePreviewOfFiles={handlePreviewOfFiles} isSingle={isSingle} open={isExistingImageModal} handleClose={() => setIsExistingImageModal(false)} />
            <DialogContent className={classes.container}>
                <IconButton className={classes.closeIcon} component="span" size="small" onClick={onClose} >
                    <Close />
                </IconButton>
                <Grid container spacing={1}>
                    <Grid container xs={12} spacing={2} className={classes.mainContent}>
                        <Grid container xs={12} className={classes.grid}>
                            <Grid item xs={6} className={classes.content}>
                                <Typography variant={'h4'} className={classes.title}>Upload </Typography>
                            </Grid>
                            <Grid item xs={6} className={classes.content}>
                                <Typography variant={'h4'} className={classes.title}>{t("dossier.file.upload.title.1")}</Typography>
                            </Grid>
                        </Grid>
                        <Grid container xs={12} className={classes.grid}>
                            <Grid item xs={6} className={classes.content}>
                                <input
                                    ref={fileInputRef}
                                    className={classes.fileInput}
                                    type="file"
                                    accept={props.accept ? props.accept : "*"}
                                    multiple={!isSingle}
                                    onChange={onFilesAdded}
                                />
                                <IconButton
                                    component="span"
                                    size="small"
                                    onClick={openFileDialog}
                                >
                                    <CreateNewFolder className={classes.icons} />
                                </IconButton>
                            </Grid>
                            <Grid item xs={6} className={classes.content}>
                                <IconButton
                                    component="span"
                                    size="small"
                                    onClick={() => setIsExistingImageModal(true)}
                                >
                                    <Folder className={classes.icons} />
                                </IconButton>
                            </Grid>
                        </Grid>
                    </Grid>
                    {false && <Grid item xs={12} className={classes.mainContent}>
                        <FormControlLabel
                            control={<Checkbox checked={isObjectsRightsAssign} color="primary" onChange={() => setOpenEditObjects(!openEditObjects)} />}
                            label={t("dossier.file.upload.subtitle")}
                            labelPlacement="end"
                        />
                    </Grid>}
                </Grid>
            </DialogContent>
            <EditDossierObjects
                open={openEditObjects}
                buildings={buildings}
                selectedObjects={selectedDossier}
                onSave={handleSaveObjects}
                onClose={handleCloseObjects}
            />
        </Dialog>
    );
}

export default UploadFiles;

const useStyles = makeStyles((theme) => ({
    title: {
        textAlign: 'center',
        color: theme.palette.primary.main,
        [theme.breakpoints.down('xs')]: {
            fontSize: '1.7rem',
        }
    },
    icons: {
        width: '3em',
        height: '3em',
        fill: theme.palette.primary.main
    },
    container: {
        padding: theme.spacing(3.125),
        position: 'relative'
    },
    content: {
        display: 'flex',
        flexDirection: 'row',
        alignItems: 'center',
        justifyContent: 'center'
    },
    mainContent: {
        marginTop: 10,
        marginBottom: 10,
    },
    grid: {
        marginTop: 15,
        marginBottom: 15,
    },
    fileInput: {
        display: 'none'
    },
    closeIcon: {
        position: 'absolute',
        right: 10,
        top: 10
    }
}));
