import { AppBar, Checkbox, CircularProgress, Container, Grid, IconButton, makeStyles, Toolbar, Tooltip, Typography, Box } from '@material-ui/core';
import { ArrowBack, Search, Share, SystemUpdateAlt, FilterList } from '@material-ui/icons';
import React, { useEffect, useRef, useState } from 'react';
import clsx from "clsx";
import { useHistory } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useDispatch, useSelector } from 'react-redux';
import { formatDate } from '../../_helpers';
import Chip from '@material-ui/core/Chip';
import Paper from '@material-ui/core/Paper';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import DossierSelectionDialog from './components/DossierSelectionDialog';
import { dossiersActions } from '../../_actions/dossiers.actions';
import { groupBy } from '../../_helpers';
import DeadLineCellInfo from './components/DeadLineCellInfo';
import ShareDossier from '../Dossiers/Common/ShareDossier';
import { getUsersForDossierShare } from '../../apis/dossiersApi';
import BuildingCells from './components/BuildingCell';
import DownloadMenu from './components/DownloadMenu';
import FilterMenu from './components/FilterMenu';

export const DossierDeadline = (props) => {
    const dispatch = useDispatch();
    const { allDossiers, deadlineDossierLoading, filteredAllDossiers } = useSelector(state => state.dossier);
    const { selected, all: allBuildings, loading: mainLoading, rights } = useSelector(state => state.buildings);
    const [buildings, setBuildings] = useState([]);
    const classes = useStyles();
    const filterRef = useRef();
    const history = useHistory();
    const { t } = useTranslation();
    const [mainFilterData, setMainFilterData] = useState({
        startDate: null,
        endDate: null,
        status: 0,
        is48hoursReminder: false,
        isOverdue: false,
        hasUpdates: false
    });
    const [isFilterOpen, setIsFilterOpen] = useState(false);
    const [isOpenDossierSelection, setIsOpenDossierSelection] = useState(false);
    const [openShareDossierMenu, setOpenShareDossierMenu] = useState(false);
    const [selectedDossierUsersList, setSelectedDossierUsersList] = useState(null);
    const [isLoadingUsers, setIsLoadingUsers] = useState(false);
    const [sharedDossier, setSharedDossier] = useState(false);
    const [isOpenDownloadModal, setIsOpenDownloadModal] = useState(null);

    const isFilterActive = !!Object.keys(mainFilterData).some(filter => !!mainFilterData[filter])
    const finalAllDossiers = isFilterActive ? filteredAllDossiers : allDossiers;

    useEffect(() => {
        if (!!window.localStorage.getItem('filterDataForDossierDeadline')) {
            setMainFilterData(JSON.parse(window.localStorage.getItem('filterDataForDossierDeadline')));
        }
    }, [])

    useEffect(() => {
        dispatch(dossiersActions.filterDossiers(mainFilterData))
    }, [allDossiers])

    useEffect(() => {
        window.localStorage.setItem('filterDataForDossierDeadline', JSON.stringify(mainFilterData))
        dispatch(dossiersActions.filterDossiers(mainFilterData));
        buildings && buildings.map(b => dispatch(dossiersActions.selectAllBuildingColumn(b.buildingId, false)))
    }, [mainFilterData])

    useEffect(() => {
        if (selected.projectId) {
            dispatch(dossiersActions.getDossiers(selected.projectId))
            setBuildings(allBuildings.filter(b => b.projectId === selected.projectId))
        }
    }, [selected.projectId]);

    useEffect(() => {
        const selectedDossiers = finalAllDossiers.filter(p => (p.isSelectedAllBuilding || p.isSelectedAnyBuilding));
        if (selectedDossiers.length === 1) {
            if (!selectedDossierUsersList || selectedDossierUsersList.dossierId !== selectedDossiers[0].id) {
                setSelectedDossierUsersList(null);
                setIsLoadingUsers(true);
                getUsersForDossierShare(selectedDossiers[0].id).then(res => {
                    if (selectedDossiers[0].id === res.data.dossierId) {
                        setSelectedDossierUsersList(res.data);
                    }
                    setIsLoadingUsers(false);
                }).catch(er => {
                    setSharedDossier(false)
                    setIsLoadingUsers(false)
                })
            }
            setSharedDossier({ ...selectedDossiers[0], buildingInfoList: Object.values(selectedDossiers[0].buildingInfoList) })
        } else {
            setSharedDossier(false)
        }
    }, [finalAllDossiers]);

    useEffect(() => {
        if (finalAllDossiers.length && allBuildings.length) {
            const buildingsData = groupBy(finalAllDossiers.flatMap(p => Object.values(p.buildingInfoList)), 'buildingId')
            const allBuildingsData = allBuildings.filter(b => b.projectId === selected.projectId).map(p => {
                const isSelectedAllBuilding = buildingsData[p.buildingId] && buildingsData[p.buildingId].every(b => !!b.isSelected);
                const isSelectedAnyBuilding = !isSelectedAllBuilding && buildingsData[p.buildingId] && buildingsData[p.buildingId].some(b => !!b.isSelected);
                return {
                    ...p,
                    isSelectedAnyBuilding,
                    isSelectedAllBuilding,
                }
            });
            setBuildings(allBuildingsData);
        }
    }, [finalAllDossiers, allBuildings])

    const handleFilterData = filters => {
        setMainFilterData(filters);
        dispatch(dossiersActions.filterDossiers(mainFilterData));
    };

    const handleDeleteFilterChip = filter => {
        const nonDeletedFilterChips = { ...mainFilterData };
        if (filter === 'startDate') nonDeletedFilterChips['startDate'] = nonDeletedFilterChips['endDate'] = null;
        nonDeletedFilterChips[filter] = filter === 'status' ? 0 : filter === 'is48hoursReminder' || filter === 'isOverdue' || filter === 'hasUpdates' ? false : null
        setMainFilterData(nonDeletedFilterChips)
    }

    const handleSelectAllColumn = (buildingId, { target: { checked } }) => {
        dispatch(dossiersActions.selectAllBuildingColumn(buildingId, checked));
    }

    const navigateToDetails = (e, dossier) => {
        e.stopPropagation();
        if (!['checkbox', 'button'].includes(e.target.name))
            history.push('/werk/' + selected.projectNo + '/dossier/' + dossier.id, { dossier });
    };

    if (mainLoading || deadlineDossierLoading) return (<div className={classes.loadingContainer}>
        <CircularProgress size={55} color={'primary'} />
    </div>);

    const labelSetter = (filter) => {
        switch (filter) {
            case 'startDate':
                if (!!mainFilterData.startDate && !!mainFilterData.endDate) return `${formatDate(new Date(mainFilterData[filter]))} ${t('dossierdeadline.filterChip.date.label')} ${formatDate(new Date(mainFilterData['endDate']))}`
                break;
            case 'status':
                return t('dossier.status.' + mainFilterData[filter])
                break;
            case 'is48hoursReminder':
                return t("dossierdeadline.filterMenu.checkBox.is48hoursReminder.label")
                break;
            case 'isOverdue':
                return t("dossierdeadline.filterMenu.checkBox.isOverdue.label")
                break;
            case 'hasUpdates':
                return t("dossierdeadline.filterMenu.checkBox.hasUpdates.label")
                break;
            default:
                break;
        }
    }


    return (
        <Container className={classes.mainContainer} maxWidth={false}>
            <DossierSelectionDialog open={isOpenDossierSelection} onClose={() => setIsOpenDossierSelection(false)} />
            <Grid container className={classes.container}>
                <AppBar position="sticky">
                    <Toolbar variant="dense">
                        {
                            <React.Fragment>
                                <IconButton edge="start" aria-label="GoBack" color="inherit" onClick={props.history.goBack}>
                                    <ArrowBack />
                                </IconButton>
                                <Typography className={clsx(classes.grow, classes.bold)} noWrap>
                                    {t("dossierdeadline.title")}
                                </Typography>
                                {
                                    !!finalAllDossiers.filter(p => (p.isSelectedAllBuilding || p.isSelectedAnyBuilding)).length &&
                                    <>
                                        <Tooltip title={t('Download')}>
                                            {
                                                <IconButton aria-describedby={'download-popover'} aria-label="Do" edge="end" color="inherit" onClick={(e) => setIsOpenDownloadModal(true)}>
                                                    <SystemUpdateAlt />
                                                </IconButton>
                                            }
                                        </Tooltip>
                                        {isOpenDownloadModal && <DownloadMenu
                                            isFilterActive={isFilterActive}
                                            isOpenDownloadModal={isOpenDownloadModal}
                                            onClose={() => setIsOpenDownloadModal(false)} />}
                                    </>
                                }
                                <Tooltip title={t('Delen')}>
                                    <IconButton disabled={isLoadingUsers || !sharedDossier || !selectedDossierUsersList} onClick={() => setOpenShareDossierMenu(true)} aria-label="Share" edge="end" color="inherit">
                                        {
                                            isLoadingUsers ? <CircularProgress color="inherit" size={24} /> : <Share />
                                        }
                                    </IconButton>
                                </Tooltip>
                                <Tooltip title={t('search')}>
                                    <IconButton aria-label="Search" color="inherit">
                                        <Search />
                                    </IconButton>
                                </Tooltip>
                                <Tooltip title={t('filter')}>
                                    <IconButton aria-label="filter" edge="start" color="inherit" onClick={() => setIsFilterOpen(!isFilterOpen)}
                                        ref={filterRef}>
                                        <FilterList />
                                    </IconButton>
                                </Tooltip>
                            </React.Fragment>
                        }
                    </Toolbar>
                    <FilterMenu
                        open={isFilterOpen}
                        anchorEl={filterRef.current}
                        mainFilterData={mainFilterData}
                        onClickAway={() => setIsFilterOpen(false)}
                        onClickListItem={() => setIsFilterOpen(false)}
                        handleFilterData={handleFilterData}
                    />
                </AppBar>
                <Paper className={classes.root}>
                    {
                        isFilterActive &&
                        <Box px={2}>
                            {
                                Object.keys(mainFilterData).filter(filter => !!mainFilterData[filter]).map((filter, index) => {
                                    if (filter !== 'endDate')
                                        return <Chip
                                            key={index}
                                            label={labelSetter(filter)}
                                            className={classes.filterChip}
                                            onDelete={() => handleDeleteFilterChip(filter)}
                                        />
                                })
                            }
                        </Box>
                    }
                    {
                        finalAllDossiers.length > 0 ?
                            <TableContainer className={classes.container}>
                                <Table stickyHeader aria-label="sticky table">
                                    <TableHead>
                                        <TableRow>
                                            <TableCell className={classes.head}
                                                style={{ zIndex: finalAllDossiers ? finalAllDossiers.length + 1 : 1 }}>
                                                {t('Dossiers')}
                                            </TableCell>
                                            {buildings && buildings.map((column) => {
                                                return (
                                                    <TableCell className={classes.head} key={column.buildingId} >
                                                        <Checkbox
                                                            onChange={(val) => handleSelectAllColumn(column.buildingId, val)}
                                                            indeterminate={!column.isSelectedAllBuilding && !!column.isSelectedAnyBuilding}
                                                            checked={!!column.isSelectedAllBuilding}
                                                            color='default' name="checkbox" />
                                                        {t(column.buildingNoExtern)}
                                                    </TableCell>
                                                )
                                            })}
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {
                                            finalAllDossiers.map((row, i) => {
                                                return (
                                                    <TableRow className={classes.row} hover role="checkbox" tabIndex={-1} key={row.id}>
                                                        <TableCell className={classes.dossierHeader}
                                                            style={{ zIndex: finalAllDossiers.length - i, cursor: 'pointer' }}
                                                            onClick={e => navigateToDetails(e, row)}>
                                                            <DeadLineCellInfo
                                                                dossier={row}
                                                                indeterminate={!row.isSelectedAllBuilding && row.isSelectedAnyBuilding}
                                                                isSelected={!!row.isSelectedAllBuilding}
                                                                classes={classes} />
                                                        </TableCell>
                                                        <BuildingCells
                                                            buildings={buildings}
                                                            dossier={row}
                                                        />
                                                    </TableRow>
                                                );
                                            })}
                                    </TableBody>
                                </Table>
                            </TableContainer>
                            :
                            <Typography className={classes.noDataText} variant='body1' align='center' noWrap>{t('dossierdeadline.nodata')}</Typography>
                    }
                </Paper>
            </Grid>
            {
                openShareDossierMenu && sharedDossier && <ShareDossier
                    isReadOnly={!rights['dossier.canShare']}
                    selectedDossier={selectedDossierUsersList ? { ...sharedDossier, userList: selectedDossierUsersList.usersList } : sharedDossier}
                    buildingIds={sharedDossier.buildingInfoList.filter(b => b.isSelected).map(b => b.buildingId)}
                    open={openShareDossierMenu}
                    onClose={() => {
                        setOpenShareDossierMenu(false);
                    }}
                />
            }
        </Container>
    )
};

const useStyles = makeStyles((theme) => ({
    mAuto: {
        margin: 'auto'
    },
    loadingContainer: {
        height: '100%',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center'
    },
    root: {
        width: '100%',
        height: 'calc(100% - 48px)',
        // padding: 20,
        paddingTop: 10,
        display: 'flex',
        flexDirection: 'column',
    },
    head: {
        backgroundColor: theme.palette.background.paper,
        boxShadow: theme.shadows[0],
        textAlign: 'center',
        padding: 0,
        '&:first-child': {
            padding: theme.spacing(0, 2),
            textAlign: 'left'
        }
        // left: 225
    },

    mainContainer: {
        height: '100%',
        width: '100%',
        overflow: 'auto',
        padding: 0
    },
    container: {
        backgroundColor: theme.palette.background.paper,
        [theme.breakpoints.down("xs")]: {
            marginTop: theme.spacing(0)
        },
        height: '100%',
    },
    noDataText: {
        width: '100%',
    },
    filterChip: {
        margin: theme.spacing(1, 1, 1, 0),
    },
    grow: {
        flexGrow: 1
    },
    bold: {
        fontWeight: "bold"
    },
    innerContainer: {
        position: 'relative',
        height: 'calc(100% - 48px)',
        overflow: 'hidden',
        zIndex: 1
    },
    row: {
        // height: 150,
    },
    dossierHeader: {
        position: 'sticky',
        left: 0,
        zIndex: 1,
        background: '#fff',
        // boxShadow: '3px -3px 6px #ccc',
        padding: theme.spacing(0, 2),
        borderRight: '1px solid #ccc',
        minWidth: 220,
    },
    dossierName: {
        whiteSpace: 'nowrap',
        overflow: 'hidden',
        textOverflow: 'ellipsis',
        maxWidth: '200px'
    },
    icon: {
        fontSize: '1.4em'
    },
    checkBox: {
        '& svg': {
            fontSize: '1.4rem',
        },
        padding: 0,
        //paddingLeft: 0
        // position: 'absolute',
        // left: -7,
        // top: -8
    },
    colorSuccess: {
        color: theme.palette.success.main
    },
    dossierHeaderTitles: {
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center'
    },
    buildingCell: {
        minWidth: 100,
        borderRight: '1px solid #ccc'
    },
    objectDetails: {
        position: 'relative',
        minWidth: 100,
        padding: 0
        // textAlign: 'center'
    },
    warning: {
        fill: '#ffc107'
    },
    pending: {
        fill: '#dc3545'
    },
    statusContainer: {
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center'
    }
}));