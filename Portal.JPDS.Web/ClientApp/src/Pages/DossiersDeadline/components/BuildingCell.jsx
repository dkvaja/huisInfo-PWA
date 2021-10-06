import React, { useEffect } from 'react'
import { Checkbox, Grid, makeStyles, TableCell, Tooltip, Typography } from '@material-ui/core';
import { Done, History, PriorityHigh } from '@material-ui/icons';
import { useTranslation } from 'react-i18next';
import { formatDate } from '../../../_helpers';
import clsx from 'clsx';
import { useHistory } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { dossiersActions } from '../../../_actions/dossiers.actions';

const BuildingCell = React.memo(({ dossier, column, handleObjectPageRedirect, selectedBuilding }) => {
    const { t } = useTranslation();
    const classes = useStyles();
    const dispatch = useDispatch();

    const checkDate = (dossierDate, buildingDate) => {
        return dossierDate === buildingDate || (formatDate(new Date(dossierDate)) === formatDate(new Date(buildingDate)));
    }

    const handleSelectBuilding = (dossierId, buildingId) => {
        dispatch(dossiersActions.selectDossiersBuilding(dossierId, buildingId));
    }

    return (
        <TableCell className={`${classes.buildingCell} ${classes.objectDetails}`}>
            {selectedBuilding ?
                <div style={{ padding: 4, cursor: 'pointer' }} onClick={(e) => {
                    handleObjectPageRedirect(e, dossier.id, selectedBuilding.buildingId)
                }}>
                    <Grid container alignItems="center" style={{ textAlign: 'center' }}>
                        <Grid item xs={4}>
                            <Checkbox
                                checked={!!selectedBuilding.isSelected}
                                color='default'
                                name="checkbox"
                                className={classes.checkBox}
                                onChange={(e) => {
                                    e.stopPropagation();
                                    handleSelectBuilding(dossier.id, selectedBuilding.buildingId)
                                }} />
                        </Grid>
                        <Grid item xs={4}>
                            {
                                selectedBuilding.status === 2 &&
                                <Tooltip title={t('dossier.general.status.value.' + selectedBuilding.status)}>
                                    <Done className={classes.colorSuccess} />
                                </Tooltip>
                            }
                            {
                                selectedBuilding.status !== 2 && (selectedBuilding.isOverdue || selectedBuilding.is48hoursReminder) &&
                                <Tooltip
                                    title={selectedBuilding.is48hoursReminder ? t("Binnen 48 uur") : selectedBuilding.isOverdue ? t("Te laat") : ''}>
                                    <History
                                        className={clsx(selectedBuilding.is48hoursReminder && classes.warning, selectedBuilding.isOverdue && classes.pending, classes.icon)} />
                                </Tooltip>
                            }
                        </Grid>
                        {
                            selectedBuilding.hasUpdates &&
                            <Grid item xs={4}>
                                <Tooltip title={t('Er is een aanpassing gedaan in het dossier.')}>
                                    <PriorityHigh color="error" />
                                </Tooltip>
                            </Grid>
                        }
                    </Grid>
                    <Tooltip title={dossier.deadline && checkDate(dossier.deadline, selectedBuilding.deadline) ? formatDate(new Date(selectedBuilding.deadline)) : selectedBuilding.deadline ? '' : t('dossier.nodeadline')}>
                        <Typography style={{ paddingTop: 1, fontSize: 12, textAlign: 'center' }}>
                            {dossier.deadline && checkDate(dossier.deadline, selectedBuilding.deadline) ? column.buildingNoExtern : selectedBuilding.deadline ? formatDate(new Date(selectedBuilding.deadline)) : column.buildingNoExtern}
                        </Typography>
                    </Tooltip>
                </div>
                : null
            }
        </TableCell>
    )
}, (prevProps, nextProps) => {
    if (prevProps.selectedBuilding === undefined || nextProps.selectedBuilding === undefined) return true;
    return prevProps.selectedBuilding.isSelected === nextProps.selectedBuilding.isSelected;
});

const BuildingCells = ({ buildings, handleSelectBuildings, dossier, ...props }) => {
    const history = useHistory();
    const { selected } = useSelector(state => state.buildings);

    const handleObjectPageRedirect = (e, dossierId, buildingId) => {
        if (!['checkbox', 'button'].includes(e.target.name))
            history.push(`/werk/${selected.projectNo}/dossier/${dossierId}?buildingId=${buildingId}`);
    }

    return (
        buildings ? buildings.map((column) => {
            const selectedBuilding = dossier.buildingInfoList[column.buildingId];
            return (<BuildingCell
                key={dossier.id + column.buildingId}
                dossier={dossier}
                handleObjectPageRedirect={handleObjectPageRedirect}
                column={column}
                selectedBuilding={selectedBuilding} />)
        }) : ''
    )
}
export default (BuildingCells);

const useStyles = makeStyles((theme) => ({
    buildingCell: {
        minWidth: 100,
        borderRight: '1px solid #ccc'
    },
    objectDetails: {
        position: 'relative',
        minWidth: 100,
        padding: 0
    },
    checkBox: {
        '& svg': {
            fontSize: '1.4rem',
        },
        padding: 0,
    },
    icon: {
        fontSize: '1.4em'
    },
    colorSuccess: {
        color: theme.palette.success.main
    },
    warning: {
        fill: theme.palette.warning.light
    },
    pending: {
        fill: theme.palette.error.dark
    },
}));