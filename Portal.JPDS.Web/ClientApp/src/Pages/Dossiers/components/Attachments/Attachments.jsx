import React, { useEffect, useRef, useState } from "react";
import { Checkbox, Grid, IconButton, ListItemText, makeStyles, MenuItem, MenuList, Paper, Popover, Tooltip, } from "@material-ui/core";
import { useTranslation } from "react-i18next";
import { Folder, ImportExport, MoreVert, PriorityHigh } from "@material-ui/icons";
import AttachmentsMenu from "../../Common/AttachmentsMenu";
import { useDispatch, useSelector } from "react-redux";
import { dossiersActions } from "../../../../_actions/dossiers.actions";
import AttachmentTab from "./AttachmentTab";
import { ListItem } from "@material-ui/core";

const selectedTab = {
  0: 'uploaded',
  1: 'archived',
  2: 'removed',
}

export const Attachments = (props) => {
  const {
    buildingId, selectedDossier, handleGetAllImages, sortingType,
    setSortingType, isBuyer, buildings, selected, isReadOnly,
    active, archived, removed, currentTab } = props;
  const dispatch = useDispatch()
  const { t } = useTranslation();
  const classes = useStyles();
  if (!selectedDossier) return null;
  const attachmentAnchorRef = useRef();
  const [selectedFiles, setSelectedFiles] = useState({ uploaded: [], removed: [], archived: [] });
  const [selectedFileReadOnly, setSelectedFileReadOnly] = useState({ uploaded: false, removed: false, archived: false });
  const [openAttachmentsMenu, setOpenAttachmentsMenu] = useState(false);
  const { rights } = useSelector(state => state.buildings);
  const { isMoving } = useSelector(state => state.dossier);
  const [isMovingFileKey, setIsMovingFileKey] = useState(false);
  const [activeTab, setActiveTab] = React.useState(0);
  const [folderSelectionOpenerEl, setFolderSelectionOpenerEl] = React.useState(0);
  const [sortingEl, setSortingEl] = useState();
  const canMoveFiles = rights['dossier.canDrag'] || rights['selected.object.write'];
  const canEdit = rights['dossier.files.write'] || rights['selected.object.write'];

  useEffect(() => {
    setActiveTab(0);
  }, [currentTab])

  useEffect(() => {
    handleSortAttachments(sortingType);
  }, [sortingType, active, archived, removed]);

  useEffect(() => {
    if (!isMoving && isMovingFileKey) {
      setSelectedFiles(p => ({ ...p, [isMovingFileKey.subType]: [] }))
      setIsMovingFileKey(false);
    }
  }, [isMoving])

  const handleUpdateFiles = ({ isActive = false, ...type }, subType, isInternal = props.type === 'internal', singleItem, moveTo = {}, moveTypes = {}) => {
    const data = {
      dossierId: selectedDossier.id,
      buildingId: (singleItem && moveTypes.viewType === 'dossier' && moveTypes.moveToViewTypes === 'dossier') ? null : buildingId,
      isInternal,
      isArchived: false,
      isDeleted: false,
      ...type,
      dossierFileList: (singleItem && !selectedFiles[subType].length) ? [{ dossierFileId: singleItem.fileId }] : selectedFiles[subType].map(f => ({ dossierFileId: f.fileId }))
    };
    (selectedFiles[subType].length || singleItem) &&
      dispatch(dossiersActions.moveFiles(data, {
        type: props.type,
        key: isActive || Object.keys(type)[0],
        subType,
        dossierId: selectedDossier.id,
        buildingId: (singleItem && moveTypes.viewType === 'dossier' && moveTypes.moveToViewTypes === 'dossier') ? null : buildingId,
        ...moveTypes,
        ...moveTo
      }));
    setOpenAttachmentsMenu(false);
    setIsMovingFileKey({ subType, type: props.type })
  };

  const handleSelect = (isAll, key, file) => {
    let data = Object.assign({}, selectedFiles);
    if (!isAll) {
      const isExist = data[key].findIndex(f => f.fileId === file.fileId);
      if (isExist >= 0)
        data[key].splice(isExist, 1);
      else data[key].push(file)
    } else {
      data[key] = data[key].length === file.length ? [] : [...file]
    }
    setSelectedFiles(p => ({ ...p, [key]: data[key] }));
    setSelectedFileReadOnly(s => ({ ...s, [key]: data[key].some(p => !p.hasRights) }));
  };

  const handleMoveExistingFiles = (images) => {
    const data = {
      dossierId: selectedDossier.id,
      buildingId: buildingId || '',
      isInternal: props.type === 'internal',
      dossierFileList: images.map(image => ({
        dossierFileId: image.fileId,
        attachmentId: image.fileId,
        isArchived: false,
        isDeleted: false
      }))
    };
    dispatch(dossiersActions.linkFiles(data, {
      key: props.type === 'internal' ? buildingId ? 'internalObjectFiles' : 'internalFiles' : buildingId ? 'externalObjectFiles' : 'externalFiles',
      buildingId
    }));
  };

  const handleChangeTab = (tab) => {
    setActiveTab(tab);
    setFolderSelectionOpenerEl(null)
  }

  const getCurrentSelectedFiles = (type = null) => {
    if (type === 'length') {
      switch (activeTab) {
        case 0:
          return active ? active.length : 0;
        case 1:
          return archived ? archived.length : 0;
        case 2:
          return removed ? removed.length : 0;
        default:
          return 0;
      }
    }
    return activeTab === 0 ? active : activeTab === 1 ? archived : removed;

  }

  const handleSortAttachments = (type) => {
    const data = getCurrentSelectedFiles() || [];
    data.sort(function (a, b) {
      if (type === 'date') return new Date(b.lastModifiedOn) - new Date(a.lastModifiedOn);

      const prevFileName = a.name.toUpperCase();
      const currentFileName = b.name.toUpperCase();
      if (prevFileName < currentFileName) return -1;
      if (prevFileName > currentFileName) return 1;
      return 0;
    });
    setSortingEl(null);
  }

  const exclamationIconRender = (type, isMenuItem = false) => {
    const hasUpdatesFiles = {
      uploaded: active && active.some(a => a.hasUpdates),
      archived: archived && archived.some(a => a.hasUpdates),
      removed: removed && removed.some(a => a.hasUpdates),
    };

    const isUpdate = !isMenuItem && hasUpdatesFiles && Object.values(selectedTab).filter(p => {
      if (isBuyer) return (p !== selectedTab[activeTab] && p !== 'archived');
      return p !== selectedTab[activeTab];
    }).some(p => hasUpdatesFiles[p]);

    return (
      hasUpdatesFiles && (isMenuItem ? hasUpdatesFiles[type] : isUpdate) &&
      <Tooltip Tooltip title={t('Er is een aanpassing gedaan in het dossier.')}>
        <PriorityHigh color="error" size={'small'} style={{ position: 'absolute', left: isMenuItem ? -4 : -10, marginBottom: 0 }} />
      </Tooltip>
    )
  }

  return (
    <Grid item xs={12}>
      <div className={`${classes.block} ${isBuyer ? 'buyer' : ''}`}>
        <Grid container>
          <Grid item xs={12}>
            <div className={classes.headerContainer}>
              <Popover
                id={'select-folder'}
                open={!!folderSelectionOpenerEl}
                anchorEl={folderSelectionOpenerEl}
                onClose={() => setFolderSelectionOpenerEl(null)}
                anchorOrigin={{ vertical: 'bottom', horizontal: 'left' }}
                transformOrigin={{ vertical: 'top', horizontal: 'left' }} >
                <Paper className={classes.paper}>
                  <MenuList>
                    <MenuItem className={classes.relative} selected={activeTab === 0} onClick={() => handleChangeTab(0)}>
                      {exclamationIconRender('uploaded', true)}
                      {t('Hoofdmap')}
                    </MenuItem>
                    {!isBuyer && <MenuItem className={classes.relative} selected={activeTab === 1} onClick={() => handleChangeTab(1)}>
                      {exclamationIconRender('archived', true)}
                      {t('Archief')}
                    </MenuItem>}
                    <MenuItem className={classes.relative} selected={activeTab === 2} onClick={() => handleChangeTab(2)}>
                      {exclamationIconRender('removed', true)}
                      {t('Verwijderd')}
                    </MenuItem>
                  </MenuList>
                </Paper>
              </Popover>
              <ListItem button className={classes.subHeader} onClick={(e) => setFolderSelectionOpenerEl(e.currentTarget)}>
                {exclamationIconRender()}
                <Folder color="primary" />
                <ListItemText
                  classes={{ primary: classes.subHeaderText }}
                  primary={t(activeTab === 0 ? 'Hoofdmap' : activeTab === 1 ? 'Archief' : 'Verwijderd')} />
              </ListItem>
              <IconButton aria-label="sort" onClick={(e) => setSortingEl(e.currentTarget)}>
                <ImportExport />
              </IconButton>
              <Popover
                id={'select-sort'}
                open={!!sortingEl}
                anchorEl={sortingEl}
                onClose={() => setSortingEl(null)}
                anchorOrigin={{ vertical: 'bottom', horizontal: 'left' }}
                transformOrigin={{ vertical: 'top', horizontal: 'left' }}
                className={classes.filterPopover}
                classes={{
                  root: classes.filterPopover
                }}>
                <Paper className={classes.paper}>
                  <MenuList>
                    <MenuItem
                      className={classes.relative}
                      selected={sortingType === 'date'}
                      onClick={() => {
                        setSortingType('date');
                      }}>
                      {t('Datum')}
                    </MenuItem>
                    <MenuItem
                      className={classes.relative}
                      selected={sortingType === 'alpha'}
                      onClick={() => {
                        setSortingType('alpha');
                      }}>
                      {t('Alfabetisch')}
                    </MenuItem>
                  </MenuList>
                </Paper>
              </Popover>
              <Checkbox
                indeterminate={(!!getCurrentSelectedFiles('length') && selectedFiles[selectedTab[activeTab]].length && selectedFiles[selectedTab[activeTab]].length !== getCurrentSelectedFiles('length'))}
                checked={getCurrentSelectedFiles('length') && selectedFiles[selectedTab[activeTab]].length === getCurrentSelectedFiles('length')}
                onChange={() => handleSelect(true, selectedTab[activeTab], getCurrentSelectedFiles())}
                color="primary" inputProps={{ 'aria-label': 'secondary checkbox' }} />
              <IconButton ref={attachmentAnchorRef}
                onClick={() => setOpenAttachmentsMenu(true)} aria-label="settings">
                <MoreVert />
              </IconButton>
              <AttachmentsMenu
                {...props}
                handleGetAllImages={(type) => handleGetAllImages(type, selectedFiles[selectedTab[activeTab]])}
                handleUpdateFiles={(data, isInternal) => handleUpdateFiles(data, selectedTab[activeTab], isInternal)}
                handleSelectExistingImages={(data) => handleMoveExistingFiles(data)}
                selectedTab={selectedTab[activeTab]}
                canMoveFiles={canMoveFiles}
                isReadOnly={!canMoveFiles && (isReadOnly || selectedFileReadOnly[selectedTab[activeTab]])}
                selectedObjects={selectedDossier.buildingInfoList}
                disabledOptions={!selectedFiles[selectedTab[activeTab]].length}
                buildings={buildings.filter(x => x.projectId === selected.projectId)}
                open={openAttachmentsMenu}
                anchorEl={attachmentAnchorRef.current}
                onClickAway={() => setOpenAttachmentsMenu(false)}
                onClickListItem={() => setOpenAttachmentsMenu(false)}
              />
            </div>
            {
              [...Array(3)].map((p, i) => (
                <AttachmentTab
                  {...props}
                  isRootFolder={selectedTab[i] === 'uploaded'}
                  handleUpdateFiles={(data, isInternal, item, moveTo, viewType) => {
                    handleUpdateFiles(data, selectedTab[activeTab], isInternal, item, moveTo, viewType)
                  }}
                  isBuyer={isBuyer}
                  isReadOnly={isReadOnly || !canEdit}
                  canMoveFiles={isReadOnly || !canMoveFiles}
                  handleSelect={handleSelect}
                  selectedFiles={selectedFiles[selectedTab[i]]}
                  handleMoveExistingFiles={handleMoveExistingFiles}
                  attachmentType={selectedTab[i]}
                  selectedDossier={selectedDossier}
                  attachmentData={(i === 0 ? active : i === 1 ? archived : removed) || []}
                  currentTab={i}
                  activeTab={activeTab}
                />
              ))
            }
          </Grid>
        </Grid>
      </div >
    </Grid >
  );
}

const useStyles = makeStyles((theme) => ({
  relative: {
    position: 'relative'
  },
  block: {
    width: '100%',
    backgroundColor: theme.palette.grey[100],
    padding: theme.spacing(1, 1, 1),
    '&.buyer': {
      padding: theme.spacing(0, 0, 0),
    }
  },
  headerContainer: {
    display: 'flex',
    alignItems: 'center'
  },
  subHeaderText: {
    fontSize: 20,
    fontWeight: 500,
  },
  subHeader: {
    position: 'relative',
    flexGrow: 1,
    paddingLeft: theme.spacing(1.25),
    '& svg': {
      fontSize: 30,
      marginRight: 10
    },
  },
  filterPopover: {
    zIndex: 2
  }
}));
