import { Checkbox, Tooltip, Typography } from '@material-ui/core';
import { CheckCircle, History, VisibilityOff, PriorityHigh, } from '@material-ui/icons';
import React from 'react';
import clsx from "clsx";
import { useTranslation } from 'react-i18next';
import { formatDate } from '../../../_helpers';
import { useDispatch } from 'react-redux';
import { dossiersActions } from '../../../_actions/dossiers.actions';

const DeadLineCellInfo = ({ selectedDossier, selectedBuildings, dossier, classes, indeterminate, isSelected, ...props }) => {
    const { t } = useTranslation();
    const dispatch = useDispatch();

    const handleSelectDossier = (e, dossierId) => {
        e.stopPropagation();
        dispatch(dossiersActions.selectAllDossiersBuilding(dossierId));
    }

    return (
        <>
            <div className={classes.dossierHeaderTitles} style={{ justifyContent: 'flex-start' }}>
                <Checkbox
                    indeterminate={indeterminate}
                    checked={isSelected}
                    onChange={(e) => handleSelectDossier(e, dossier.id)} name="checkbox"
                    color='default' className={classes.checkBox} />
                <Typography className={`${classes.dossierName} ${classes.grow}`} title={dossier.name}>
                    &nbsp;
                    {dossier.name}
                    &nbsp;
                    {
                        dossier.status === 2 &&
                        <Tooltip title={t('Afgerond')}><CheckCircle fontSize="default" className={classes.colorSuccess} /></Tooltip>
                    }
                    {
                        dossier.status !== 2 && (dossier.isOverdue || dossier.is48hoursReminder) &&
                        <Tooltip
                            title={dossier.is48hoursReminder ? t("Binnen 48 uur") : dossier.isOverdue ? t("Te laat") : ''}>
                            <History
                                className={clsx(dossier.is48hoursReminder && classes.warning, dossier.isOverdue && classes.pending, classes.icon)} />
                        </Tooltip>
                    }
                    {
                        dossier.hasUpdates &&
                        <Tooltip title={t('Er is een aanpassing gedaan in het dossier.')}>
                            <PriorityHigh color="error" />
                        </Tooltip>
                    }
                </Typography>
                {
                    dossier.isExternal !== true &&
                    <Tooltip title={t('Onzichtbaar voor kopers')}><VisibilityOff fontSize="default" color="action" /></Tooltip>
                }
            </div>
            <div className={classes.dossierHeaderTitles}>
                <Typography variant="body2" color="textSecondary">{dossier.deadline && formatDate(new Date(dossier.deadline))}</Typography>
            </div>
        </>
    );
}

export default DeadLineCellInfo;