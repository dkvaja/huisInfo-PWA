import React, { useEffect, useState } from "react";
import {
    Button,
    CircularProgress,
    Dialog,
    DialogContent,
    DialogTitle,
    Grid,
    makeStyles,
    Switch,
    Typography,
    useMediaQuery,
    useTheme,
    Slide
} from "@material-ui/core";
import { Edit, Visibility } from "@material-ui/icons";
import { useTranslation } from "react-i18next";
import { useSelector } from "react-redux";
import clsx from "clsx";
import { Hidden } from "@material-ui/core";

const Transition = React.forwardRef(function Transition(props, ref) {
    return <Slide direction="up" ref={ref} {...props} />;
})

export default function DossierRights(props) {
    const { open, onUpdate, onClose, availableRoles: roles, selectedRoles, isDossierExternal, isReadOnly, isNewDossier = false, ...rest } = props;
    const { t } = useTranslation();
    const classes = useStyles();
    const [availableRoles, setAvailableRoles] = useState([]);
    const [changedRoles, setChangedRoles] = useState([]);
    const [isUpdated, setUpdated] = useState(false);
    const { isUpdatingRights } = useSelector(state => state.dossier);
    const { user } = useSelector(state => state.authentication);
    const theme = useTheme();
    const fullScreen = useMediaQuery(theme.breakpoints.down('sm'));


    useEffect(() => {
        if (isUpdatingRights) setUpdated(true);
        if (!isUpdatingRights && isUpdated) onClose();
    }, [isUpdatingRights]);

    useEffect(() => {
        if (roles && roles.length) {
            setAvailableRoles(roles.map(r => ({
                ...r,
                usersList: r.usersList.map(u => {
                    const right = selectedRoles && selectedRoles
                        .find(p => p.loginId === u.loginId && r.roleId === p.roleId && r.moduleId === p.moduleId);
                    if (u.loginId === user.id) {
                        u.isExternal = true;
                        u.hasExternalEditRights = true;
                        if (isNewDossier) {
                            u.isInternal = true;
                            u.isInternalVisible = true;
                            u.isExternalVisible = true;
                        }
                        if (!right) changedRoles.push(u);
                    }
                    if (right) return { ...right, ...u };
                    return u;
                })
            })));
        }
    }, [roles, selectedRoles]);

    const handleUpdateObjects = () => {
        if (changedRoles && changedRoles.length) {
            onUpdate({
                roles,
                changedRoles,
                selectedRoles,
                availableRoles: availableRoles.map(r => r.usersList
                    .filter(u => u.isExternal !== undefined || u.isExternalVisible !== undefined || u.hasExternalEditRights !== undefined)
                    .map(u => ({
                        ...u,
                        roleId: r.roleId,
                        moduleId: r.moduleId,
                        roleName: r.roleName,
                    }))).flat()
            });
        }
        // onClose();
    };

    const handleChangeRights = (parentIndex, index, key) => {
        const allRoles = Object.assign([], availableRoles);
        let allChangedRoles = Object.assign([], changedRoles);
        let selectedRole = allRoles[parentIndex].usersList[index];
        selectedRole = {
            ...selectedRole,
            [key]: !selectedRole[key]
        };
        if ((!key.includes('Visible') || !key.includes('EditRights')) && !selectedRole[key]) {
            selectedRole[key + 'Visible'] = false;
            selectedRole[key === 'isExternal' ? 'hasExternalEditRights' :'hasInternalEditRights'] = false;
        }
        const row = {
            dossierRightId: selectedRole.dossierRightId,
            loginId: selectedRole.loginId,
            isExternal: selectedRole.isExternal,
            isExternalVisible: selectedRole.isExternalVisible,
            hasExternalEditRights: selectedRole.hasExternalEditRights,
            isInternal: selectedRole.isInternal,
            isInternalVisible: selectedRole.isInternalVisible,
            hasInternalEditRights: selectedRole.hasInternalEditRights,
            roleId: allRoles[parentIndex].roleId,
            moduleId: allRoles[parentIndex].moduleId,
        };
        allRoles[parentIndex].usersList[index] = selectedRole;
        const isExistingIndex = changedRoles
            .findIndex(r => row.dossierRightId
                ? row.dossierRightId === r.dossierRightId
                : (r.loginId === row.loginId && r.roleId === row.roleId && r.moduleId === row.moduleId));
        if (isExistingIndex >= 0)
            allChangedRoles[isExistingIndex] = row;
        else
            allChangedRoles.push(row);
        setChangedRoles(allChangedRoles)
        setAvailableRoles(allRoles);
    }

    return (
        open &&
        <Dialog
            TransitionComponent={Transition}
            keepMounted
            fullScreen={fullScreen} maxWidth={isDossierExternal ? 'lg' : 'md'} open={open} onClose={isUpdatingRights ? () => { } : onClose} aria-labelledby="form-dialog-title"
            scroll="paper">
            <DialogTitle className={classes.dialogTitle} id="dialog-objects-title" disableTypography>
                <Grid className={classes.dialogTitleContent} container spacing={1}>
                    <Grid item className={classes.grow}>
                        <Typography variant="h5"
                            className={classes.dialogTitleTypo}>{t('dossier.objects.rights.title')}</Typography>
                    </Grid>
                    <Grid item >
                        <div className={classes.buttonContainer}>
                            <Button className={classes.button} size='small' disabled={isUpdatingRights} variant="outlined" onClick={onClose}>
                                <Typography className={classes.buttonTitle}>
                                    {t('Annuleer')}
                                </Typography>
                            </Button>
                            <Button className={classes.button} size='small' disabled={isUpdatingRights || !changedRoles.length || isReadOnly} variant="outlined" onClick={handleUpdateObjects} >
                                {isUpdatingRights ? <CircularProgress style={{ color: 'white' }} size={20} /> : <Typography className={classes.buttonTitle}>{t('Opslaan')}</Typography>}
                            </Button>
                        </div>
                    </Grid>
                </Grid>
            </DialogTitle>
            <DialogContent className={classes.dialogContent}>
                <Grid container spacing={1}>
                    {
                        availableRoles && availableRoles.map((p, index) => (
                            <Grid className={index && classes.border} container spacing={1}>
                                <Grid className={classes.borderBottom} container xs={12}>
                                    <Grid className={classes.tableTitles} item xs={12} sm={isDossierExternal ? 5 : 7} md={isDossierExternal ? 5 : 8}>
                                        <Typography nowrap className={classes.tableTitlesTypo} variant='h5'>{t(p.roleName)}</Typography>
                                    </Grid>
                                    <Hidden smDown={isDossierExternal} xsDown={!isDossierExternal}>
                                        <Grid container className={classes.tableTitles} item xs={12} sm={isDossierExternal ? 7 : 5} md={isDossierExternal ? 7 : 4}>
                                            <Grid item xs={isDossierExternal ? 3 : 12} className={classes.gridTitle}>
                                                <Typography className={classes.subtile2}
                                                    variant='subtitle2'>{t("dossier.objects.rights.internal.title")}</Typography>
                                                <Edit />
                                            </Grid>
                                            {
                                                isDossierExternal &&
                                                <>
                                                    <Grid item xs={3} className={classes.gridTitle}>
                                                        <Typography className={classes.subtile2}
                                                            variant='subtitle2'>{t("dossier.objects.rights.external.title")}</Typography>
                                                        <Edit />
                                                    </Grid>

                                                    <Grid item xs={3} className={classes.gridTitle}>
                                                        <Typography className={classes.subtile2}
                                                            variant='subtitle2'>{t("dossier.objects.rights.subTitle.1")}</Typography>
                                                        <Visibility />
                                                    </Grid>
                                                    <Grid item xs={3} className={classes.gridTitle}>
                                                        <Typography className={classes.subtile2}
                                                            variant='subtitle2'>{t("dossier.objects.rights.subTitle.2")}</Typography>
                                                        <Edit />
                                                    </Grid>
                                                </>
                                            }
                                        </Grid>
                                    </Hidden>
                                </Grid>
                                <Grid container xs={12}>
                                    {
                                        p.usersList.map((u, i) => (
                                            <Grid className={classes.userContainer} container xs={12}>
                                                <Grid item xs={12} sm={isDossierExternal ? 12 : 7} md={isDossierExternal ? 5 : 8}>
                                                    <Typography variant='h6' className={classes.userNameTypo}>{t(u.name)}</Typography>
                                                </Grid>
                                                <Hidden mdUp={isDossierExternal} smUp={!isDossierExternal}>
                                                    <Grid container className={classes.tableTitles} item xs={12} sm={isDossierExternal ? 12 : 5}>
                                                        <Grid item xs={6} sm={3} className={clsx(classes.gridTitle, classes.labelText)}>
                                                            <Typography className={classes.subtile2}
                                                                variant='subtitle2'>{t("dossier.objects.rights.internal.title")}</Typography>
                                                        </Grid>
                                                        <Grid item xs={6} sm={3} className={classes.switchContainer}>
                                                            <Switch
                                                                size='small'
                                                                disabled={(user.id === u.loginId && u.isInternal) || isReadOnly}
                                                                onChange={() => handleChangeRights(index, i, 'isInternal')}
                                                                color="primary"
                                                                classes={{ disabled: u.isInternal && classes.disabledSwitch }}
                                                                checked={u.isInternal} />
                                                        </Grid>
                                                        {
                                                            isDossierExternal &&
                                                            <>
                                                                <Grid item xs={6} sm={3} className={clsx(classes.gridTitle, classes.labelText)}>
                                                                    <Typography className={classes.subtile2}
                                                                        variant='subtitle2'>{t("dossier.objects.rights.external.title")}</Typography>
                                                                </Grid>
                                                                <Grid item xs={6} sm={3} className={classes.switchContainer}>
                                                                    <Switch
                                                                        size='small'
                                                                        disabled={(user.id === u.loginId && u.isExternal) || isReadOnly}
                                                                        onChange={() => handleChangeRights(index, i, 'isExternal')}
                                                                        color="primary"
                                                                        classes={{ disabled: u.isExternal && classes.disabledSwitch }}
                                                                        checked={u.isExternal} />
                                                                </Grid>

                                                                <Grid item xs={6} sm={3} className={clsx(classes.gridTitle, classes.labelText)}>
                                                                    <Typography className={classes.subtile2}
                                                                        variant='subtitle2'>{t("dossier.objects.rights.subTitle.1")}</Typography>
                                                                </Grid>
                                                                <Grid item xs={6} sm={3} className={classes.switchContainer}>
                                                                    <Switch
                                                                        size='small'
                                                                        disabled={u.isExternal !== true || isReadOnly}
                                                                        onChange={() => handleChangeRights(index, i, 'isExternalVisible')}
                                                                        color="primary"
                                                                        classes={{ disabled: u.isExternalVisible && classes.disabledSwitch }}
                                                                        checked={u.isExternalVisible} />
                                                                </Grid>
                                                                <Grid item xs={6} sm={3} className={clsx(classes.gridTitle, classes.labelText)}>
                                                                    <Typography className={classes.subtile2}
                                                                        variant='subtitle2'>{t("dossier.objects.rights.subTitle.2")}</Typography>
                                                                </Grid>
                                                                <Grid item xs={6} sm={3} className={classes.switchContainer}>
                                                                    <Switch
                                                                        size='small'
                                                                        disabled={
                                                                            u.isExternal !== true || (user.id === u.loginId && u.hasExternalEditRights) || isReadOnly
                                                                        }
                                                                        onChange={() => handleChangeRights(index, i, 'hasExternalEditRights')}
                                                                        color="primary"
                                                                        classes={{ disabled: u.hasExternalEditRights && classes.disabledSwitch }}
                                                                        checked={u.hasExternalEditRights} />
                                                                </Grid>
                                                            </>
                                                        }
                                                    </Grid>
                                                </Hidden>
                                                <Hidden smDown={isDossierExternal} xsDown={!isDossierExternal}>
                                                    <Grid container xs={12} sm={isDossierExternal ? 12 : 5} md={isDossierExternal ? 7 : 4}>
                                                        <Grid className={classes.gridTitle} item xs={isDossierExternal ? 3 : 12}>
                                                            <Switch disabled={(user.id === u.loginId && u.isInternal) || isReadOnly}
                                                                onChange={() => handleChangeRights(index, i, 'isInternal')}
                                                                color="primary"
                                                                classes={{ disabled: u.isInternal && classes.disabledSwitch }}
                                                                checked={u.isInternal} />
                                                        </Grid>
                                                        {
                                                            isDossierExternal &&
                                                            <>
                                                                <Grid className={classes.gridTitle} item xs={3}>
                                                                    <Switch disabled={(user.id === u.loginId && u.isExternal) || isReadOnly}
                                                                        onChange={() => handleChangeRights(index, i, 'isExternal')}
                                                                        color="primary"
                                                                        classes={{ disabled: u.isExternal && classes.disabledSwitch }}
                                                                        checked={u.isExternal} />
                                                                </Grid>

                                                                <Grid className={classes.gridTitle} item xs={3}>
                                                                    <Switch disabled={u.isExternal !== true || isReadOnly}
                                                                        onChange={() => handleChangeRights(index, i, 'isExternalVisible')}
                                                                        color="primary"
                                                                        classes={{ disabled: u.isExternalVisible && classes.disabledSwitch }}
                                                                        checked={u.isExternalVisible} />
                                                                </Grid>
                                                                <Grid className={classes.gridTitle} item xs={3}>
                                                                    <Switch
                                                                        disabled={
                                                                            u.isExternal !== true || (user.id === u.loginId && u.hasExternalEditRights) || isReadOnly
                                                                        }
                                                                        onChange={() => handleChangeRights(index, i, 'hasExternalEditRights')}
                                                                        color="primary"
                                                                        classes={{ disabled: u.hasExternalEditRights && classes.disabledSwitch }}
                                                                        checked={u.hasExternalEditRights} />
                                                                </Grid>
                                                            </>
                                                        }
                                                    </Grid>
                                                </Hidden>
                                            </Grid>
                                        ))
                                    }
                                </Grid>
                            </Grid>
                        ))
                    }
                </Grid>
            </DialogContent>
        </Dialog >
    );
}


const useStyles = makeStyles((theme) => ({
    grow: {
        flexGrow: 1,
        display: 'flex',
        alignItems: 'center'
    },
    border: {
        borderTop: `1px solid ${theme.palette.grey[400]}`,
        marginTop: 20,
    },
    dialogContent: {
        padding: theme.spacing(1.25)
    },
    dialogTitle: {
        padding: theme.spacing(0.625),
        background: theme.palette.primary.main,
    },
    dialogTitleTypo: {
        color: theme.palette.common.white,
        fontSize: 25,
        [theme.breakpoints.down('sm')]: {
            fontSize: 17,
        },
    },
    dialogTitleContent: {
        padding: theme.spacing(1.25)
    },
    iconStyle: {
        width: '2rem',
        height: '2rem',
        fill: theme.palette.common.white,
    },
    button: {
        margin: '0 10px',
        color: theme.palette.common.white,
        borderColor: theme.palette.common.white,
        [theme.breakpoints.down("xs")]: {
            minWidth: 0,
            width: 50,
            margin: '0 3px',
            padding: 2
        }
    },
    buttonTitle: {
        [theme.breakpoints.down("xs")]: {
            fontSize: 10,
        }
    },
    userNameTypo: {
        paddingLeft: theme.spacing(1.25),
        margin: 5,
        ['@media screen and (max-width:600px)']: {
            fontSize: 12,
            fontWeight: 'bold',
        },
    },
    tableTitles: {
        display: 'flex',
    },
    gridTitle: {
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        flexDirection: 'column',
        padding: 5
    },
    tableTitlesTypo: {
        alignSelf: 'center',
        width: '100%',
        overflow: 'hidden',
        textOverflow: 'ellipsis',
        paddingLeft: theme.spacing(0.625),
        ['@media screen and (max-width:600px)']: {
            fontSize: 20
        },
    },
    borderBottom: {
        borderBottom: `1px solid ${theme.palette.grey[400]}`,
        background: theme.palette.grey[200]
    },
    subtitles: {
        fontSize: '0.696rem',
        marginTop: 10,
        textAlign: 'center',
        marginBottom: -2,
    },
    subtile2: {
        fontSize: '0.686rem',
        textAlign: 'center',
        [theme.breakpoints.down("sm")]: {
            textAlign: 'left',
        }
    },
    userContainer: {
        marginTop: 13,
    },
    disabledSwitch: {
        color: `${theme.palette.primary.light} !important`,
    },
    buttonContainer: {
        flexGrow: 1,
        display: 'flex',
        justifyContent: 'flex-end',
        [theme.breakpoints.down('xs')]: {
            justifyContent: 'flex-start',
        }
    },
    labelText: {
        flexDirection: 'row',
        paddingLeft: theme.spacing(4.375),
        justifyContent: 'flex-start',
    },
    switchContainer: {
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center'
    },
}));
