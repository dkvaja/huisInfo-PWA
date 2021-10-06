import React, { useEffect, useState } from 'react';
import { Divider, Grid, makeStyles, Tabs, Tooltip } from '@material-ui/core';
import { useTranslation } from 'react-i18next';
import { useDispatch, useSelector } from 'react-redux';
import { dossiersActions } from '../../../_actions/dossiers.actions';
import { userAccountTypeConstants } from '../../../_constants';
import ContactList from '../Common/ContactList';
import { Attachments } from './Attachments';
import { GeneralSection } from './GeneralSection';
import { DndProvider } from 'react-dnd';
import { HTML5Backend } from 'react-dnd-html5-backend';
import TabButton from '../Common/TabButton';
import { PriorityHigh } from '@material-ui/icons';

const isShow = (type, activeTab, selectedDossier, isBuyer, externalRights, internalRights) => {
    if (type === 'external')
        return ((activeTab === 0 && externalRights) || (isBuyer && selectedDossier.isExternal))
    else if (type === 'internal')
        return ((activeTab === 1 && externalRights || !externalRights && activeTab === 0) && internalRights && !isBuyer);
}

const DossierInformation = (props) => {
    const { selectedDossierBuilding, accessRights, selectedDossier,
        buildings, selected, fileUploading, handleSelectFiles,
        handlePreviewOfFiles, loggedInUserFromRights,
        handleGetAllImages, buildingId } = props;
    const { t } = useTranslation();
    const { user } = useSelector(state => state.authentication);
    const isBuyer = user.type === userAccountTypeConstants.buyer;
    const dispatch = useDispatch();
    const classes = useStyles();
    const [activeTab, setActiveTab] = useState(0);
    const internalRights = accessRights && accessRights.isInternal && (buildingId ? selectedDossier.hasObjectBoundFiles : selectedDossier.hasGeneralFiles);
    const externalRights = accessRights && accessRights.isExternal && (buildingId ? selectedDossier.hasObjectBoundFiles : selectedDossier.hasGeneralFiles);
    const isReadOnlyExternal = !accessRights.hasExternalEditRights || selectedDossier.isArchived || selectedDossier.status === 2 || selectedDossierBuilding.status === 2
    const isReadOnlyInternal = selectedDossier.isArchived || selectedDossier.status === 2 || selectedDossierBuilding.status === 2;
    const [sortingType, setSortingType] = useState('date');
    const [canStartDragging, setCanStartDragging] = useState(false)

    useEffect(() => {
        if (selectedDossier.id && (!selectedDossierBuilding || !selectedDossierBuilding.buildingId) && selectedDossier.hasUpdates) {
            markFileAsViewed(null, null, null, null);
        }
    }, [selectedDossier.id]);

    useEffect(() => {
        if (selectedDossierBuilding.buildingId && selectedDossierBuilding.hasUpdates) {
            markFileAsViewed(null, selectedDossierBuilding.buildingId, null, null);
        }
    }, [selectedDossierBuilding.buildingId, selectedDossier.id]);

    const markFileAsViewed = (dossierFileId, objId, type, subType,isGeneralFile) => {
        const data = {
            dossierId: selectedDossier.id,
            buildingId: objId,
            dossierFileId
        };
        dispatch(dossiersActions.updateDossierLastRead(data, {
            type: type,
            subType,
            dossierId: selectedDossier.id,
            buildingId: objId ? objId : buildingId,
            isBuyer,
            isGeneralFile
        }));
    };

    const exclamationIconRender = (type) => {
        const attachments = buildingId ? selectedDossierBuilding : selectedDossier;
        let key = type === 'internal' ? 'internalFiles' : 'externalFiles';;
        if (buildingId)
            key = type === 'internal' ? 'internalObjectFiles' : 'externalObjectFiles'

        const isUpdate = attachments[key] && [].concat(attachments[key].uploadedFiles || [], attachments[key].archivedFiles || [], attachments[key].deletedFiles || []).some(p => p.hasUpdates)

        return (
            isUpdate &&
            <Tooltip Tooltip title={t('Er is een aanpassing gedaan in het dossier.')}>
                <PriorityHigh color="error" size={'small'} style={{ position: 'relative', left: 6, marginBottom: 0 }} />
            </Tooltip>
        )
    }

    const getFiles = (type, key) => {
        const mainDossierKey = type === 'internal' ? 'internalFiles' : 'externalFiles';
        const mainObjectKey = type === 'internal' ? 'internalObjectFiles' : 'externalObjectFiles';
        if (isBuyer) {
            const generalFiles = ((selectedDossier[mainDossierKey] || {})[key] || []).map(p => ({ ...p, isGeneralFile: true }));
            return [].concat(
                buildingId && selectedDossier.hasObjectBoundFiles ?
                    (selectedDossierBuilding[mainObjectKey] || {})[key] || [] : [],
                (generalFiles)).sort((a, b) => new Date(b.lastModifiedOn) - new Date(a.lastModifiedOn));
        }

        return ((buildingId ? selectedDossierBuilding[mainObjectKey] : selectedDossier[mainDossierKey]) || {})[key];
    }

    return (
        <Grid container className={classes.block}>
            <GeneralSection
                {...props}
                isReadOnly={selectedDossier.isArchived || selectedDossier.status === 2 || selectedDossierBuilding.status === 2}
                selectedDossier={selectedDossier}
                selectedDossierBuilding={selectedDossierBuilding}
                buildingId={buildingId}
                markFileAsViewed={markFileAsViewed}
                buyerContactInfo={selectedDossierBuilding.buyerContactInfo}
                selectedDossierContacts={selectedDossier.userList} />

            <DndProvider backend={HTML5Backend}>
                <Grid container className={classes.attachmentsBlock}>
                    {!isBuyer && <Grid xs={12}>
                        <Tabs value={activeTab} indicatorColor="primary" textColor="primary" onChange={(e, tab) => setActiveTab(tab)}>
                            {externalRights && <TabButton
                                activeTab={activeTab}
                                classes={{ wrapper: classes.tabWrapper }}
                                setActiveTab={(tab) => setActiveTab(tab)}
                                viewType={buildingId ? 'object' : 'dossier'}
                                className={classes.tabs} icon={exclamationIconRender('external')}
                                tab={0} label={t('Kopers Dossier')} />}
                            {internalRights && <TabButton
                                activeTab={activeTab}
                                classes={{ wrapper: classes.tabWrapper }}
                                setActiveTab={(tab) => setActiveTab(tab)}
                                viewType={buildingId ? 'object' : 'dossier'}
                                className={classes.tabs} icon={exclamationIconRender('internal')}
                                tab={1} label={t('Betrokkenen Dossier')} />}
                        </Tabs>
                        <Divider />
                    </Grid>}
                    <Grid item xs={12} md={8}>
                        {['external', 'internal'].map(p => {
                            const show = isShow(p, activeTab, selectedDossier, isBuyer, externalRights, internalRights);
                            return show && (
                                <Attachments key={p} currentTab={activeTab} buildingId={buildingId}
                                    canStartDragging={canStartDragging}
                                    setCanStartDragging={(value) => setCanStartDragging(value)}
                                    selectedDossier={selectedDossier} handlePreviewOfFiles={handlePreviewOfFiles}
                                    selected={selected} isObjectView={!!buildingId} fileUploading={fileUploading}
                                    viewType={buildingId ? 'object' : 'dossier'} buildings={buildings}
                                    active={getFiles(p, 'uploadedFiles')} archived={getFiles(p, 'archivedFiles')}
                                    removed={getFiles(p, 'deletedFiles')} handleSelectFiles={handleSelectFiles}
                                    isBuyer={isBuyer} handleGetAllImages={handleGetAllImages}
                                    isReadOnly={p === 'external' ? isReadOnlyExternal : isReadOnlyInternal}
                                    markFileAsViewed={markFileAsViewed} type={p}
                                    sortingType={sortingType} setSortingType={(type) => setSortingType(type)}
                                />
                            )
                        })}
                    </Grid>
                    <Grid item xs={12} md={4}>
                        <ContactList
                            loggedInUserFromRights={loggedInUserFromRights}
                            selectedDossierContacts={selectedDossier.userList}
                            buyerContactInfo={selectedDossierBuilding.buyerContactInfo} />
                    </Grid>
                </Grid>
            </DndProvider>
        </Grid>
    )
};

const useStyles = makeStyles((theme) => ({
    block: {
        width: '100%',
        backgroundColor: theme.palette.grey[100],
    },
    attachmentsBlock: {
        padding: theme.spacing(1, 1, 3),
    },
    tabs: {
        minHeight: 'auto !important',
        padding: theme.spacing(1.5),
        [theme.breakpoints.down("sm")]: {
            flexGrow: 1,
        },
        [theme.breakpoints.down("xs")]: {
            fontSize: 12
        },
    },
    tabWrapper: {
        flexDirection: 'row'
    }
}));

export default DossierInformation;
