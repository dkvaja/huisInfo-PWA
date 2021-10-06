import React, { useEffect, useState } from "react";
import {
    AppBar,
    Divider,
    Grid,
    IconButton,
    List,
    ListItem,
    makeStyles,
    Slide,
    Toolbar,
    Tooltip,
    Typography
} from "@material-ui/core";
import { useTranslation } from "react-i18next";
import { Settings, SwapHoriz, Done, PriorityHigh } from "@material-ui/icons";
import clsx from "clsx";
import { useHistory, useParams } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import { dossiersActions } from "../../../_actions/dossiers.actions";
import { CircularProgress } from "@material-ui/core";
import { userAccountTypeConstants } from "../../../_constants";


export default function ObjectList({ openDossier, generatePath, buildingId, match, show, matchesWidthUpSm, handleSwitcher, handleSelectBuilding, handleSelectDossier }) {
    const { t } = useTranslation();
    const classes = useStyles();
    const history = useHistory();
    const { viewType } = useParams();
    const dispatch = useDispatch();
    const { all, rights } = useSelector(state => state.buildings);
    const { user } = useSelector(state => state.authentication);
    const { selectedDossier, buildingList, getBuildingLoading, dossierLoading, selectedLoading } = useSelector(state => state.dossier);
    const [buildingInfoList, setBuildingInfoList] = useState([]);
    const isBuyer = user.type === userAccountTypeConstants.buyer;

    const objectViewHandler = (building) => {
        if (buildingId !== building.buildingId) {
            if (viewType === 'dossier') {
                dispatch(dossiersActions.getSelectedDossierBuildingInfo({
                    buildingId: building.buildingId,
                    dossierId: selectedDossier.id
                }))
                handleSelectBuilding({
                    buildingId: building.buildingId,
                    openDossier: true,
                });
                history.push(`${generatePath({ selectedDataId: selectedDossier.id })}?buildingId=${building.buildingId}`)
            } else {
                handleSelectBuilding({
                    buildingId: building.buildingId,
                    openDossier: true
                });
                handleSelectDossier(null, 'dossier main', building)
                history.push(`${generatePath({ selectedDataId: building.buildingId })}?dossierId=${building.dossierList[0].id}`)
            }
        }
    }
    useEffect(() => {
        if (selectedDossier && selectedDossier.buildingInfoList && viewType === 'dossier')
            setBuildingInfoList(selectedDossier.buildingInfoList.map(b => {
                const { buildingNoExtern } = all.find(a => a.buildingId === b.buildingId);
                return { ...b, buildingNoExtern }
            }).sort((a, b) => a.buildingNoExtern > b.buildingNoExtern ? 1 : -1));
        if (viewType === 'building' && buildingList && all && all.length) {
            setBuildingInfoList(buildingList.map(b => {
                const { buildingNoExtern } = all.find(a => a.buildingId === b.buildingId);
                return { ...b, buildingNoExtern }
            }).sort((a, b) => a.buildingNoExtern > b.buildingNoExtern ? 1 : -1));

        }
    }, [all, selectedDossier, buildingList]);

    if (!selectedDossier && viewType === 'dossier') return null;
    return (
        <Slide direction="left" in={show} mountOnEnter
            unmountOnExit>
            <Grid item xs={12} sm={3} lg={1} container className={clsx(classes.buildingContainer, viewType === 'dossier' && matchesWidthUpSm && classes.zIndex)} direction="column">
                <AppBar position="static">
                    <Toolbar variant="dense" className={classes.toolbar} >
                        <Typography className={clsx(classes.grow, classes.bold)} align="center" noWrap>
                            {t('Objecten')}
                        </Typography>
                        {!isBuyer && rights['dossier.canSwitchView'] && !selectedLoading && buildingId && !!selectedDossier && viewType === 'building' && <IconButton className={classes.switch} aria-label="Filter" color="inherit" edge="end" onClick={handleSwitcher}>
                            <SwapHoriz />
                        </IconButton>}
                    </Toolbar>
                </AppBar>
                <List className={classes.buildingList}>
                    {viewType !== 'building' && !dossierLoading && <Tooltip title={t('Instellingen')}>
                        <ListItem
                            button
                            className={classes.buildingListItem}
                            selected={!buildingId}
                            onClick={() => {
                                if (buildingId || !openDossier) {
                                    handleSelectBuilding({ selectedDossierBuilding: {}, buildingId: null, openDossier: true })
                                    history.push(`${match.url}`)
                                }
                            }}>
                            <Settings color="action" />
                            {
                                selectedDossier && ['internalFiles', 'externalFiles'].find(d =>
                                    selectedDossier[d] && [].concat(
                                        selectedDossier[d].uploadedFiles || [],
                                        selectedDossier[d].archivedFiles || [],
                                        selectedDossier[d].deletedFiles || []
                                    ).find(p => p.hasUpdates)) &&
                                <Tooltip title={t('Er is een aanpassing gedaan in het dossier.')}>
                                    <PriorityHigh fontSize="small" color="error" />
                                </Tooltip>
                            }
                        </ListItem>
                    </Tooltip>}
                    {getBuildingLoading || dossierLoading ? (
                        <CircularProgress size={30} className={classes.mAuto} />
                    ) : null}
                    <Divider component="li" />
                    {!getBuildingLoading && !dossierLoading && !!buildingInfoList && buildingInfoList.map((building, index) => (
                        <React.Fragment key={index}>
                            <ListItem
                                button
                                className={clsx(classes.buildingListItem, classes.listItem)}
                                selected={buildingId === building.buildingId}
                                onClick={() => objectViewHandler(building)}>
                                <Grid container>
                                    <Grid item xs={12} className={classes.center}>
                                        <Typography variant={''}>
                                            {building.buildingNoExtern}
                                            {
                                                building.hasUpdates &&
                                                <Tooltip title={t('Er is een aanpassing gedaan in het dossier.')}>
                                                    <PriorityHigh fontSize="small" color="error" />
                                                </Tooltip>
                                            }
                                            {
                                                viewType !== 'building' && building.status === 2 &&
                                                <Tooltip title={t('Afgerond')} style={{ position: 'absolute', right: 16, top: -3 }}>
                                                    <Done className={classes.colorSuccess} />
                                                </Tooltip>
                                            }
                                        </Typography>
                                    </Grid>
                                </Grid>
                            </ListItem>
                            <Divider component="li" />
                        </React.Fragment>
                    ))
                    }
                </List>
            </Grid>
        </Slide >
    );
}


const useStyles = makeStyles((theme) => ({
    buildingListItem: {
        padding: 0,
        justifyContent: 'center',
        alignItems: 'center'
    },
    dossierList: {
        flexGrow: 1,
        maxHeight: 'calc(100% - 144px)',
        overflowX: 'hidden',
        overflowY: 'auto',
        width: '100%',
        '&.full': {
            maxHeight: 'calc(100% - 48px)',
        },
        '&.others': {
            height: 'auto',
            maxHeight: 'unset'
        }
    },
    colorSuccess: {
        color: theme.palette.success.main
    },
    center: {
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center'
    },
    dossierListItem: {
        paddingTop: 0,
        paddingBottom: 0,
    }, grow: {
        flexGrow: 1
    },
    bold: {
        fontWeight: "bold"
    },

    buildingContainer: {
        backgroundColor: theme.palette.background.paper,
        height: '100%',
        [theme.breakpoints.down("xs")]: {
            position: 'absolute',
            zIndex: 1102,
            right: 0
        }
    },
    zIndex: {
        zIndex: 1,
    },
    buildingList: {
        flexGrow: 1,
        display: 'flex',
        flexDirection: 'column',
        maxHeight: 'calc(100% - 48px)',
        overflowX: 'hidden',
        overflowY: 'auto',
        width: '100%',
        zIndex: 1,
    },
    mAuto: {
        margin: '5px auto 5px auto'
    },
    switch: {
        position: 'absolute',
        right: -12,
    },
    listItem: {
        display: 'flex',
        justifyContent: 'space-between',
        padding: theme.spacing(0, 0.625, 0, 0.625),
        alignItems: 'center',
        position: 'relative'
    },
    toolbar: {
        padding: theme.spacing(0.625)
    }
}));
