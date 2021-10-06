import React, { useEffect, useState } from 'react'
import { Button, Checkbox, CircularProgress, Dialog, DialogActions, DialogContent, FormControlLabel, Grid, IconButton, ListItem, makeStyles, Typography } from '@material-ui/core';
import { useTranslation } from 'react-i18next';
import { Close } from '@material-ui/icons';
import { createDownloadZip } from '../../../apis/dossiersApi';
import { URLS } from '../../../apis/urls';
import { compareTwoObjects } from '../../../_helpers';
import { useSelector } from 'react-redux';

const CustomCheckBox = ({ label, controlStyle = {}, ...props }) => {
    const classes = useStyles();

    return (
        <FormControlLabel
            classes={{
                root: classes.formControl,
                label: classes.formControlLabel,
                ...controlStyle
            }}
            control={
                <Checkbox {...props} color="primary" />
            }
            label={
                <ListItem button disabled={props.disabled} className={classes.listContainer}>
                    {label}
                </ListItem>
            }
            labelPlacement="end"
        />
    );
}


export default function DownloadMenu({ onClose, isOpenDownloadModal, isFilterActive, onDownload, ...props }) {
    const { t } = useTranslation();
    const classes = useStyles();
    const { allDossiers: dossiers, filteredAllDossiers } = useSelector((s) => s.dossier);
    const [isDisabledDownload, setIsDisabledDownload] = useState(false)
    const [isDownloadLoading, setIsDownloadLoading] = useState(false)
    const [isStartDownloading, setIsStartDownloading] = useState(false);
    const [selectedDossiers, setSelectedDossiers] = useState();
    const [isExternalRights, setIsExternalRights] = useState(false);
    const [previousZipFileName, setPreviousZipFileName] = useState(null);
    const [prevDownloadState, setPrevDownloadState] = useState(null);
    const [state, setState] = useState({
        folderType: 1,
        external: {
            isAll: false, isAny: false,
            folders: { isExternMainFileReq: false, isExternArchivedFilesReq: false, isExternDeletedFilesReq: false }
        },
        internal: {
            isAll: false, isAny: false,
            folders: { isInternMainFileReq: false, isInternArchivedFilesReq: false, isInternDeletedFilesReq: false }
        }
    });

    const finalAllDossiers = isFilterActive ? filteredAllDossiers : dossiers;

    useEffect(() => {
        if (isOpenDownloadModal && finalAllDossiers.length) {
            const modifiedDossiers = finalAllDossiers.map(p => ({
                ...p,
                buildingInfoList: Object.values(p.buildingInfoList)
            })).filter(p => (p.isSelectedAllBuilding || p.isSelectedAnyBuilding));
            setSelectedDossiers(modifiedDossiers);
            setIsExternalRights(modifiedDossiers.some(b => b.isExternal));
        }
    }, [finalAllDossiers, isOpenDownloadModal])

    useEffect(() => {
        const isSelected = ['internal', 'external'].reduce((p, c) => (p || Object.values(state[c].folders).some(b => b)), false);
        const isSameDownloadState = compareTwoObjects(prevDownloadState, state);
        setIsDisabledDownload(isSelected)
        setIsStartDownloading(isSameDownloadState);
    }, [state]);


    const handleChange = ({ target: { name, checked } }, type) => {
        const selectedType = { ...state[type] };
        if (name !== 'all') {
            selectedType.folders[name] = checked;
            const folderValues = Object.values(selectedType.folders);
            selectedType.isAll = folderValues.every(p => p);
            selectedType.isAny = !selectedType.isAll && folderValues.some(p => p);
        } else {
            selectedType.isAll = checked;
            selectedType.isAny = false;
            for (const folderName in selectedType.folders)
                selectedType.folders[folderName] = checked;
        }
        setState(p => ({ ...p, [type]: { ...selectedType } }))
    }

    const documentsDownloadHandler = () => {
        setIsDownloadLoading(true)
        const dossierDownloadFiles = selectedDossiers.map(p => ({
            dossierId: p.id,
            buildingIds: p.buildingInfoList.filter(b => b.isSelected).map(b => b.buildingId)
        }));
        const data = {
            dossierDownloadFiles,
            isDossierFolderFormat: state.folderType === 1,
            ...state.internal.folders,
            ...state.external.folders,
        }
        createDownloadZip(data).then(({ data: fileName }) => {
            downloadCreatedZipFile(encodeURIComponent(fileName))
            setPreviousZipFileName(encodeURIComponent(fileName))
            setIsStartDownloading(true);
            setIsDownloadLoading(false)
            setPrevDownloadState(JSON.parse(JSON.stringify(state)))
            // onClose();
        }).catch(er => {
            setIsDownloadLoading(false)
            // onClose();
        })
    }

    const downloadCreatedZipFile = (fileName) => {
        if (fileName) {
            let link = document.createElement("a");
            link.href = URLS.DOWNLOAD_ZIP_FILE + fileName;
            link.download = fileName;
            document.body.appendChild(link).click();
        }
    }

    return (
        <Dialog open={isOpenDownloadModal} onClose={onClose} >
            <DialogContent className={classes.container}>
                <div className={classes.header}>
                    <Typography variant='h5' style={{ color: '#fff' }}>{t('download.modal.title')}:</Typography>
                    <IconButton onClick={onClose}>
                        <Close />
                    </IconButton>
                </div>
                <DialogContent style={{ padding: '5px 36px' }}>
                    <div>
                        <Typography variant={'h6'} style={{ marginLeft: 5 }}>
                            {t(`download.modal.folderStructure.title`)}:
                        </Typography>
                        <CustomCheckBox
                            disabled={isDownloadLoading}
                            checked={state.folderType === 1}
                            onChange={() => setState((p) => ({ ...p, folderType: 1 }))}
                            label={t(`download.modal.folderStructure.1`)} />
                        <CustomCheckBox
                            disabled={isDownloadLoading}
                            checked={state.folderType === 2}
                            onChange={() => setState((p) => ({ ...p, folderType: 2 }))}
                            label={t(`download.modal.folderStructure.2`)} />
                    </div>
                    <div>
                        <Typography variant={'h6'} style={{ marginLeft: 5 }}>
                            {t(`download.modal.downloadType.title`)}
                        </Typography>
                        <CustomCheckBox
                            disabled={isDownloadLoading}
                            name='all'
                            onChange={(e) => handleChange(e, 'internal')}
                            indeterminate={!state.internal.isAll && state.internal.isAny}
                            checked={state.internal.isAll}
                            label={t('download.modal.downloadType.subTitle.1')} />
                        <Grid container className={classes.usersContainer}>
                            <Grid item xs={12}>
                                <CustomCheckBox
                                    disabled={isDownloadLoading}
                                    onChange={(e) => handleChange(e, 'internal')}
                                    name='isInternMainFileReq'
                                    checked={state.internal.folders.isInternMainFileReq}
                                    label={t('download.modal.downloadType.1')} />
                            </Grid>
                            <Grid item xs={12}>
                                <CustomCheckBox
                                    disabled={isDownloadLoading}
                                    onChange={(e) => handleChange(e, 'internal')}
                                    name='isInternArchivedFilesReq'
                                    checked={state.internal.folders.isInternArchivedFilesReq}
                                    label={t('download.modal.downloadType.2')} />
                            </Grid>
                            <Grid item xs={12}>
                                <CustomCheckBox
                                    disabled={isDownloadLoading}
                                    onChange={(e) => handleChange(e, 'internal')}
                                    name='isInternDeletedFilesReq'
                                    checked={state.internal.folders.isInternDeletedFilesReq}
                                    label={t('download.modal.downloadType.3')} />
                            </Grid>
                        </Grid>

                        {isExternalRights && (
                            <>
                                <CustomCheckBox
                                    disabled={isDownloadLoading}
                                    onChange={(e) => handleChange(e, 'external')}
                                    name='all'
                                    indeterminate={!state.external.isAll && state.external.isAny}
                                    checked={state.external.isAll}
                                    label={t(`download.modal.downloadType.subTitle.2`)} />
                                <Grid container className={classes.usersContainer}>
                                    <Grid item xs={12}>
                                        <CustomCheckBox
                                            disabled={isDownloadLoading}
                                            onChange={(e) => handleChange(e, 'external')}
                                            name='isExternMainFileReq'
                                            checked={state.external.folders.isExternMainFileReq}
                                            label={t('download.modal.downloadType.1')} />
                                    </Grid>
                                    <Grid item xs={12}>
                                        <CustomCheckBox
                                            disabled={isDownloadLoading}
                                            onChange={(e) => handleChange(e, 'external')}
                                            name='isExternArchivedFilesReq'
                                            checked={state.external.folders.isExternArchivedFilesReq}
                                            label={t('download.modal.downloadType.2')} />
                                    </Grid>
                                    <Grid item xs={12}>
                                        <CustomCheckBox
                                            disabled={isDownloadLoading}
                                            onChange={(e) => handleChange(e, 'external')}
                                            name='isExternDeletedFilesReq'
                                            checked={state.external.folders.isExternDeletedFilesReq}
                                            label={t('download.modal.downloadType.3')} />
                                    </Grid>
                                </Grid>
                            </>
                        )}
                    </div>
                </DialogContent>
                <DialogActions>
                    <Grid container spacing={1} justify="flex-end">
                        {isStartDownloading && <Grid item xs={12}>
                            <Typography style={{ textAlign: 'center' }}>
                                {t('files.download.started.text')}
                            </Typography>
                        </Grid>}
                        <Grid item>
                            <Button
                                disabled={!isDisabledDownload || isDownloadLoading}
                                onClick={
                                    isStartDownloading ? () => downloadCreatedZipFile(previousZipFileName) : documentsDownloadHandler
                                }
                                variant="outlined" color="primary">
                                {
                                    isDownloadLoading ?
                                        <>
                                            <CircularProgress color='primary' size={20} />
                                            <Typography className={classes.informationText}>
                                                {t('files.download.createZip.text')}
                                            </Typography>
                                        </> : t('Download')
                                }
                            </Button>
                        </Grid>
                    </Grid>
                </DialogActions>
            </DialogContent>
        </Dialog >
    )
}


const useStyles = makeStyles((theme) => ({
    listContainer: {
        flexWrap: 'wrap',
    },
    header: {
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        marginBottom: 5,
        padding: theme.spacing(0.625),
        paddingLeft: theme.spacing(1.25),
        background: theme.palette.primary.main
    },
    listItemTitle: {
        flexGrow: 1,
        width: '100%',
        display: 'flex',
        justifyContent: 'space-between'
    },
    usersContainer: {
        paddingLeft: theme.spacing(5.625),
        flexGrow: 1,
    },
    formControl: {
        width: '100%',
        marginLeft: 20
    },
    formControlLabel: {
        flexGrow: 1,
    },
    container: {
        padding: '0 !important',
        position: 'relative'
    },
    informationText: {
        textAlign: 'center',
        marginLeft: 15,
        color: theme.palette.common.black
    }
}));