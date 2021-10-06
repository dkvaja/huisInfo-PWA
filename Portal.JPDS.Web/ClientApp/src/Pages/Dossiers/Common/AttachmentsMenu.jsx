import React, { useState } from 'react';
import { ClickAwayListener, makeStyles, Grow, MenuItem, MenuList, Paper, Popper, Collapse, Typography, Container } from '@material-ui/core';
import { ExpandLess, ExpandMore, Visibility, Archive, Delete, Description, CloudDownload, Unarchive } from '@material-ui/icons';
import { useTranslation } from 'react-i18next';

const AttachmentsMenu = ({
    open, selectedTab, moveAndUploadRights, disabledOptions, canMoveFiles,
    handleSelectExistingImages, handleUpdateFiles, onClickAway, handleGetAllImages,
    handleSelectFiles, anchorEl, onClickListItem, onKeyDown,
    selectedObjects, buildings, isReadOnly, ...props
}) => {
    const classes = useStyles();
    const { t } = useTranslation();
    const [isOpenFilesSubMenu, setIsOpenFilesSubMenu] = useState(false);

    const handleOpenFilesSubMenuToggler = () => {
        setIsOpenFilesSubMenu(!isOpenFilesSubMenu);
    }


    return (
        <React.Fragment>
            <Popper style={{ zIndex: 2 }} open={open} anchorEl={anchorEl} role={undefined} transition disablePortal>
                {({ TransitionProps, placement }) => (
                    <Grow
                        {...TransitionProps}
                        style={{ transformOrigin: placement === 'bottom' ? 'center top' : 'center bottom' }}
                    >
                        <Paper>
                            <ClickAwayListener onClickAway={onClickAway}>
                                <MenuList autoFocusItem={open} id="menu-list-grow" onKeyDown={onKeyDown}>
                                    <MenuItem disabled={disabledOptions} className={classes.menuListItem} onClick={() => handleGetAllImages('preview')}>
                                        {t("dossier.external-internal.menu.label.1")}
                                        <Visibility className={classes.icon} />
                                    </MenuItem>
                                    <MenuItem disabled={disabledOptions} className={classes.menuListItem} onClick={() => handleGetAllImages('download')}>
                                        {t("dossier.external-internal.menu.label.2")}
                                        <CloudDownload className={classes.icon} />
                                    </MenuItem>
                                    {!isReadOnly && !disabledOptions && <MenuItem style={{ display: 'block' }} onClick={handleOpenFilesSubMenuToggler}>
                                        <div className={classes.menuListItem}>
                                            {t("dossier.external-internal.menu.label.3")}
                                            {isOpenFilesSubMenu ? <ExpandLess className={classes.icon} /> : <ExpandMore className={classes.icon} />}
                                        </div>
                                        <Collapse className={classes.collapseContainer} in={isOpenFilesSubMenu} timeout="auto" unmountOnExit>
                                            {
                                                selectedTab === 'uploaded' &&
                                                <>
                                                    <Container style={{ padding: 0 }}>
                                                        <Typography variant={'h6'}>{t(`${props.type === 'internal' ? 'dossier.objects.rights.internal.title' : 'dossier.objects.rights.external.title'}`)}</Typography>
                                                        <div>
                                                            {
                                                                canMoveFiles &&
                                                                <MenuItem className={classes.subMenuListItem} onClick={() => handleUpdateFiles({ isArchived: true })}>
                                                                    Archiveer
                                                                    <Archive className={classes.icon} />
                                                                </MenuItem>
                                                            }
                                                            <MenuItem className={classes.subMenuListItem} onClick={() => handleUpdateFiles({ isDeleted: true })}>
                                                                {t('datatable.label.selectedrows.delete')}
                                                                <Delete className={classes.icon} />
                                                            </MenuItem>
                                                        </div>
                                                    </Container>
                                                    <Container style={{ padding: 0 }}>
                                                        <Typography variant={'h6'} >{t(`${props.type === 'internal' ? 'dossier.objects.rights.external.title' : 'dossier.objects.rights.internal.title'}`)}</Typography>
                                                        <div>
                                                            <MenuItem className={classes.subMenuListItem}
                                                                onClick={() => handleUpdateFiles({}, props.type !== 'internal')}>
                                                                Hoofdmap
                                                                <Description className={classes.icon} />
                                                            </MenuItem>
                                                        </div>
                                                    </Container>
                                                </>
                                            }
                                            {
                                                selectedTab === 'archived' && (
                                                    <Container style={{ padding: 0 }}>
                                                        <Typography variant={'h6'}>{t(`${props.type === 'internal' ? 'dossier.objects.rights.internal.title' : 'dossier.objects.rights.external.title'}`)}</Typography>
                                                        <div>
                                                            <MenuItem className={classes.subMenuListItem} onClick={() => handleUpdateFiles({ isActive: true })}>
                                                                {t('Hoofdmap')}
                                                                <Unarchive className={classes.icon} />
                                                            </MenuItem>
                                                            <MenuItem className={classes.subMenuListItem} onClick={() => handleUpdateFiles({ isDeleted: true })}>
                                                                {t('datatable.label.selectedrows.delete')}
                                                                <Delete className={classes.icon} />
                                                            </MenuItem>
                                                        </div>
                                                    </Container>)
                                            }
                                            {
                                                selectedTab === 'removed' && (
                                                    <Container style={{ padding: 0 }}>
                                                        <Typography variant={'h6'}>{t(`${props.type === 'internal' ? 'dossier.objects.rights.internal.title' : 'dossier.objects.rights.external.title'}`)}</Typography>
                                                        <div>
                                                            <MenuItem className={classes.subMenuListItem} onClick={() => handleUpdateFiles({ isActive: true })}>
                                                                {t('Hoofdmap')}
                                                            </MenuItem>
                                                            {
                                                                canMoveFiles &&
                                                                <MenuItem className={classes.subMenuListItem} onClick={() => handleUpdateFiles({ isArchived: true })}>
                                                                    Archiveer
                                                                    <Archive className={classes.icon} />
                                                                </MenuItem>
                                                            }
                                                        </div>
                                                    </Container>
                                                )
                                            }
                                        </Collapse>
                                    </MenuItem>}
                                </MenuList>
                            </ClickAwayListener>
                        </Paper>
                    </Grow>
                )}
            </Popper>
        </React.Fragment>
    )
}

export default AttachmentsMenu;

const useStyles = makeStyles((theme) => ({
    menuListItem: {
        minWidth: '200px',
        display: 'flex',
        justifyContent: 'space-between'
    },
    subMenuListItem: {
        display: 'flex',
        justifyContent: 'space-between'
    },
    icon: {
        fill: theme.palette.primary.main
    },
    collapseContainer: {
        padding: theme.spacing(1.25),
    }
}));
