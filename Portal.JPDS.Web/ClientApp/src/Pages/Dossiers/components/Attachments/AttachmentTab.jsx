import { Checkbox, Grid, IconButton, makeStyles, Tooltip, Typography, useTheme, withStyles } from '@material-ui/core';
import React, { useRef, useState } from 'react'
import { fileUploadConfig } from '../../../../_helpers/fileUploadConfig';
import Dropzone from "../../../../components/Dropzone";
import { CloudDownload, Delete, PriorityHigh, RestoreFromTrash, Unarchive, Visibility } from '@material-ui/icons';
import { formatDate } from '../../../../_helpers';
import { URLS } from '../../../../apis/urls';
import { useTranslation } from 'react-i18next';
import { useDrag, useDrop } from 'react-dnd';
import clsx from 'clsx';

const WhiteCheckbox = withStyles((theme) => ({
    root: {
        color: theme.palette.common.white,
        fill: theme.palette.common.white,
        '&$checked': {
            color: theme.palette.common.white,
        },
    },
    checked: {},
}))((props) => {
    return <Checkbox classes={{}} color="default" {...props} />
});
let timeOutId = null;
const MovableItem = ({
    item, handleGetAllImages, canStartDragging, setCanStartDragging,
    handleUpdateFiles, index, currentTab, moveCardHandler,
    buildingId, attachmentType, markFileAsViewed, canMoveFiles,
    handleSelect, isSelected, isReadOnly, viewType, isBuyer, ...props
}) => {
    const ref = useRef(null);
    const classes = useStyles();
    const { t } = useTranslation();
    const [_, drop] = useDrop({
        accept: "moveAttachments",
        hover(dragItem, monitor) {
            if (!ref.current) return;
            const dragIndex = dragItem.index;
            const hoverIndex = index;
            if (dragIndex === hoverIndex) return;
            const hoverBoundingRect = ref.current && ref.current.getBoundingClientRect();
            const hoverMiddleY = (hoverBoundingRect.bottom - hoverBoundingRect.top) / 2;
            const clientOffset = monitor.getClientOffset();
            const hoverClientY = clientOffset.y - hoverBoundingRect.top;
            if (dragIndex < hoverIndex && hoverClientY < hoverMiddleY) return;
            if (dragIndex > hoverIndex && hoverClientY > hoverMiddleY) return;
            dragItem.index = hoverIndex;
        }
    });

    const [{ isDragging }, drag] = useDrag({
        type: "moveAttachments",
        item: { index, item, viewType, type: 'moveAttachments' },
        canDrag() {
            return (!canMoveFiles && !isBuyer);
        },
        begin(dragItem, monitor) {
            timeOutId = setTimeout(() => {
                setCanStartDragging(true);
            }, [100])
        },
        end: (dragItem, monitor) => {
            const dropResult = monitor.getDropResult();
            if (dropResult) {
                const isExist = dropResult.attachmentData && dropResult.attachmentData.find(a => a.fileId === dragItem.item.fileId);
                if (!isExist) {
                    let data;
                    data = dropResult.type === props.type && dropResult.subType === 'uploaded' ?
                        { isActive: true } :
                        dropResult.subType === 'archived' ?
                            { isArchived: true } :
                            dropResult.subType === 'removed' ?
                                { isDeleted: true } : {}
                    handleUpdateFiles(
                        data, dropResult.type === 'internal',
                        dragItem.item, dropResult.type !== props.type ? {
                            moveTo: dropResult.type,
                        } : {}, { moveToViewType: dropResult.viewType, viewType: dragItem.viewType });
                }
            }
            clearTimeout(timeOutId)
            setCanStartDragging(false);
        },
        collect: (monitor) => ({
            isDragging: monitor.isDragging()
        })
    });
    const opacity = isDragging ? 0.4 : 1;
    drag(drop(ref));
    return (
        <Grid
            className={classes.movableItemContainer}
            onClick={() => { if (item.hasUpdates) markFileAsViewed(item.fileId, buildingId, props.type, attachmentType, item.isGeneralFile) }}
            onMouseEnter={() => { if (item.hasUpdates) markFileAsViewed(item.fileId, buildingId, props.type, attachmentType, item.isGeneralFile) }}
            item>
            {!canStartDragging &&
                <div className={classes.attachmentStatus}>
                    <Typography variant={'h6'} className={classes.modificationHeader}>
                        {t("dossier.attachments.statusBar.title")}
                    </Typography>
                    <div style={{ padding: 10 }}>
                        <Typography variant={'caption'} className={classes.caption}>
                            Nieuw of gewijzigd bestand: "{item.name}" door "{item.lastModifiedBy}" op "{formatDate(new Date(item.lastModifiedOn), true)}".
                        </Typography>
                    </div>
                </div>
            }
            <div style={{ padding: 5, paddingTop: 5, position: 'relative', opacity, transform: 'translate3d(0, 0, 0)' }} ref={ref}>
                {!canStartDragging && <div className={classes.overlay}>
                    <IconButton onClick={() => handleGetAllImages('preview', [item])}
                        aria-label="image-preview" className={classes.noOutline}>
                        <Visibility className={classes.icons} />
                    </IconButton>
                    <WhiteCheckbox
                        checked={isSelected}
                        classes={{ root: classes.checkbox }}
                        onChange={() => handleSelect(null, attachmentType, item)}
                        className={clsx(classes.isLeft, classes.whiteCheckbox)} />
                    <div className={classes.isRight}>
                        <Tooltip title={t('Downloaden')}>
                            <IconButton onClick={() => handleGetAllImages('download', [item])} className={classes.noOutline} size={'small'} >
                                <CloudDownload className={classes.icons} size={'small'} />
                            </IconButton>
                        </Tooltip>
                        {(item.hasRights && !isReadOnly) &&
                            <>
                                {currentTab === 0 &&
                                    <Tooltip title={t('Verwijderen')}>
                                        <IconButton
                                            onClick={() => handleUpdateFiles({ isDeleted: true }, props.type === 'internal', item)}
                                            className={classes.noOutline} size={'small'} >
                                            <Delete className={classes.icons} size={'small'} />
                                        </IconButton>
                                    </Tooltip>}
                                {currentTab === 1 &&
                                    <Tooltip title={t('Dearchiveren')}>
                                        <IconButton
                                            onClick={() => handleUpdateFiles({ isActive: true }, props.type === 'internal', item)}
                                            className={classes.noOutline} size={'small'} >
                                            <Unarchive className={classes.icons} size={'small'} />
                                        </IconButton>
                                    </Tooltip>}
                                {currentTab === 2 &&
                                    <Tooltip title={t('Herstellen')}>
                                        <IconButton
                                            onClick={() => handleUpdateFiles({ isActive: true }, props.type === 'internal', item)}
                                            className={classes.noOutline} size={'small'} >
                                            <RestoreFromTrash className={classes.icons} size={'small'} />
                                        </IconButton>
                                    </Tooltip>
                                }
                            </>
                        }
                    </div>
                </div>
                }
                <Grid item>
                    <Grid
                        className={classes.thumnailContainer}
                        container xs={12} justify="center">
                        <Checkbox
                            color={'primary'}
                            classes={{ root: classes.checkbox }}
                            checked={isSelected}
                            onChange={() => handleSelect(null, attachmentType, item)}
                            className={classes.isLeft} />
                        {
                            item.hasUpdates &&
                            <Tooltip title={t('Er is een aanpassing gedaan in het dossier.')}>
                                <PriorityHigh color="error" style={{ position: 'absolute', left: -18, top: -4 }} />
                            </Tooltip>
                        }
                        <div className={classes.thumbnail}
                            style={{ backgroundImage: `url("${URLS.GET_UPLOADED_DOSSIER_FILE_THUMBNAIL}${item.fileId}")` }} />
                        <Typography variant="caption" className={classes.caption}
                            noWrap>{item.name}</Typography>
                        <Typography variant="caption" className={classes.caption}
                            noWrap>{formatDate(new Date(item.lastModifiedOn))}</Typography>
                    </Grid>
                </Grid>
            </div>
        </Grid >
    )
}

const AttachmentTab = (props) => {
    const {
        isRootFolder, buildings, selected, selectedDossier,
        currentTab, activeTab, attachmentData, handleMoveExistingFiles,
        handleSelectFiles, isReadOnly, viewType, fileUploading,
        selectedFiles, attachmentType, handlePreviewOfFiles } = props;
    const classes = useStyles();
    const theme = useTheme();
    const [{ isOver, canDrop }, drop] = useDrop({
        accept: "moveAttachments",
        drop: () => ({ attachmentData, name: attachmentType, type: props.type, subType: attachmentType, viewType }),
        collect: (monitor) => ({
            isOver: monitor.isOver(),
            canDrop: monitor.canDrop()
        }),
    });
    const getBackgroundColor = () => {
        if (isOver) {
            if (canDrop) {
                return theme.palette.info.light;
            } else if (!canDrop) {
                return theme.palette.error.light;
            }
        } else {
            return "";
        }
    };


    return (
        <div ref={drop} style={{ padding: 5, backgroundColor: getBackgroundColor() }}
            role="tabpanel" hidden={activeTab !== currentTab}
            id={`wrapped-tabpanel-${currentTab}`} aria-labelledby={`wrapped-tab-${currentTab}`}>
            {
                currentTab === activeTab && (
                    <Grid item xs={12} className={classes.filesContainer}>
                        <Grid container className={classes.gridContainer}>
                            {isRootFolder &&
                                <Grid item style={{ margin: '0 auto' }}>
                                    <Grid className={classes.thumnailContainer} container justify="center">
                                        <Dropzone
                                            className={classes.dropzone}
                                            handlePreviewOfFiles={handlePreviewOfFiles}
                                            accept={fileUploadConfig.allowedMimeTypes.map(f => f.mime).join()}
                                            selectedObjects={selectedDossier.buildingInfoList}
                                            buildings={buildings.filter(x => x.projectId === selected.projectId)}
                                            withUploadFileDialog={!!buildings.length}
                                            handleSelectExistingImages={(data) => handleMoveExistingFiles(data)}
                                            onFilesAdded={(data) => handleSelectFiles(data, { type: props.type, isActive: true })}
                                            disabled={isReadOnly || canDrop}
                                            uploading={fileUploading} />
                                    </Grid>
                                </Grid>
                            }

                            {attachmentData && attachmentData.map((item, index) => {
                                const isSelected = !!selectedFiles.find(f => f.fileId === item.fileId);
                                return <MovableItem key={index} {...props} index={index} isSelected={isSelected} item={item} />
                            })}
                        </Grid>
                    </Grid>)}
        </div>
    )
};

const useStyles = makeStyles((theme) => ({
    noOutline: {
        outline: 'none !important'
    },
    block: {
        width: '100%',
        backgroundColor: theme.palette.grey[100],
        padding: theme.spacing(1, 1, 3),
        '&.buyer': {
            padding: theme.spacing(0, 0, 2),
        }
    },
    filesContainer: {
        maxHeight: 410,
        overflow: 'auto',
        padding: theme.spacing(1.25)
    },
    thumbnail: {
        backgroundPosition: 'center',
        backgroundSize: 'contain',
        backgroundRepeat: 'no-repeat',
        height: 80,
        width: '100%',
        display: 'block',

    },
    overlay: {
        position: 'absolute',
        borderRadius: 5,
        background: 'rgba(0, 0, 0, 0.5)',
        color: ' #f1f1f1',
        transition: '.5s ease',
        opacity: 0,
        fontSize: '20px',
        textAlign: 'center',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        width: '100%',
        height: 135,
        zIndex: 1,
        top: 0,
        left: 0
    },
    caption: {
        width: '100%',
        textAlign: 'center',
        [theme.breakpoints.down("xs")]: {
            fontSize: 10,
        },
    },
    thumnailContainer: {
        width: 135,
        height: 135,
        cursor: 'pointer',
        minHeight: 120,
        display: 'flex',
        position: 'relative',
        alignItems: 'center',
        flexDirection: 'column',
        justifyContent: 'space-evenly',

    },
    movableItemContainer: {
        position: 'relative',
        background: theme.palette.common.white,
        boxShadow: '3px 3px 4px #eee',
        height: 135,
        borderRadius: 5,
        '&:hover': {
            "& $overlay": {
                opacity: 1
            },
            "& $attachmentStatus": {
                opacity: 1,
                right: 10
            },
        },
    },
    checkbox: {
        [theme.breakpoints.down("xs")]: {
            width: '0.8em',
            height: '0.8em',
        },
    },
    isLeft: {
        position: 'absolute',
        top: -3,
        left: -3,
        padding: 0,
    },
    icons: {
        fill: theme.palette.common.white,
        width: '1.2em',
        height: '1.2em',
    },
    attachmentStatus: {
        maxWidth: '80%',
        position: 'fixed',
        backgroundColor: theme.palette.common.white,
        boxShadow: '1px 1px 6px #eee',
        bottom: 10,
        padding: '10px',
        marginTop: '23px',
        right: '-100%',
        opacity: 0,
        minWidth: 300,
        transition: '1s all',
        zIndex: 2,
        ['@media screen and (max-width:821px)']: {
            bottom: 66,
        },
    },
    modificationHeader: {
        fontSize: '1rem',
    },
    isRight: {
        position: 'absolute',
        right: 0,
        top: -4,
        padding: 0,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
    },
    gridContainer: {
        justifyContent: 'space-around',
        gridTemplateColumns: 'repeat(auto-fill, 135px)',
        display: 'grid',
        gridGap: 10
    },
    dropzone: {
        padding: 0,
        height: '100%',
    },
    whiteCheckbox: {
        top: 2,
        left: 2
    }
}));


export default AttachmentTab;

