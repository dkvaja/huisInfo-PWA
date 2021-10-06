import React, { useEffect, useRef, useState } from "react";
import { Button, CircularProgress, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, Divider, Grid, Icon, IconButton, makeStyles, Popover, Switch, TextField, Tooltip, Typography } from "@material-ui/core";
import { useTranslation } from "react-i18next";
import { Archive, Clear, CloudUpload, Description, Edit, Unarchive, Visibility } from "@material-ui/icons";
import { DatePicker, MuiPickersUtilsProvider } from "@material-ui/pickers";
import DateFnsUtils from "@date-io/date-fns";
import nlLocale from "date-fns/locale/nl";
import { formatDate, nl2br, toBase64, validateFile } from "../../../_helpers";
import Markdown from "../../../components/Markdown";
import RichTextEditor from "../../Chat/RichTextEditor";
import { useDispatch, useSelector } from "react-redux";
import { dossiersActions } from "../../../_actions/dossiers.actions";
import { userAccountTypeConstants } from "../../../_constants";

export const GeneralSection = ({
  buildingId, accessRights, selectedDossierBuilding, buyerContactInfo, selectedDossierContacts,
  dossierUpdateType, selected, selectedDossier, updating, buildings, handleUpdateDossierClose,
  handleGetAllImages, handlePreviewOfFiles, handleSelectFiles, updateDossier, isReadOnly: readOnly,
  openDossierRights, openEditObjects, uploadingBackground, edit, setEdit, updateStatus, markFileAsViewed, ...props
}) => {
  const { t } = useTranslation();
  const fileInputRef = useRef()
  const classes = useStyles();
  const [isReadOnly, setIsReadOnly] = useState(readOnly);
  const params = new URLSearchParams(window.location.search)
  const [isUpdating, setIsUpdating] = useState(false);
  const [deadlineConfirmationDialog, setDeadlineConfirmationDialog] = useState({ open: false, date: null });
  const { updateLoading, isDeadLineUpdating } = useSelector(state => state.dossier);
  const { user } = useSelector(state => state.authentication);
  const { rights } = useSelector(state => state.buildings);
  const dispatch = useDispatch();
  const status = selectedDossierBuilding.buildingId ? selectedDossierBuilding.status : selectedDossier.status;
  const closedOn = selectedDossierBuilding.buildingId ? selectedDossierBuilding.closedOn : selectedDossier.closedOn;
  const deadline = selectedDossierBuilding.buildingId ? selectedDossierBuilding.deadline : selectedDossier.deadline;
  const isBuyer = user.type === userAccountTypeConstants.buyer;
  const canEdit = rights['dossier.generalInformation.write'] || rights['selected.object.write'];
  const canShowOptions = rights['dossier.generalInformation.read'];

  useEffect(() => {
    const buildingIdParam = params.get('buildingId');
    const dossierId = params.get('dossierId');
    if (buildingIdParam || dossierId) {
      setIsReadOnly(true);
    } else {
      setIsReadOnly(readOnly);
    }
  }, [params]);

  useEffect(() => {
    if (updateLoading) setIsUpdating(true);
    if (!updateLoading && isUpdating) {
      handleUpdateDossierClose()
    }
  }, [updateLoading]);

  const handleUploadBackgroundImage = async ({ target: { files } }) => {
    if (files.length) {
      let file = files[0];
      if (validateFile(file, true) === true) {
        let currenturl = await toBase64(file)
        file.url = currenturl;
        const backgroundImage = {
          content: file.url.split(',')[1],
          name: file.name
        }
        updateDossier('backgroundImage', backgroundImage)
      }
    }
  }

  const handleDossierDeadlineUpdate = (deadlineDate, isUpdateBuildings) => {
    dispatch(dossiersActions.dossierDeadlineUpdate({
      dossierId: selectedDossier.id,
      deadlineDate,
      isUpdateBuildings,
    }));
  }

  const renderEditTextbox = (title, key, value, multi = false, rich = false) => {
    return (
      <div style={{ position: 'relative', minHeight: 20 }}>
        {value && <div>{rich ? <Markdown value={value} /> : nl2br(value)}</div>}
        {selectedDossier.status !== 2 &&
          <div>{updating === key ?
            <Icon color="inherit" fontSize="small" style={{ position: 'absolute', right: -15, top: 0 }}>
              <CircularProgress size="small" />
            </Icon> :
            <div>
              {!isReadOnly && <Tooltip title={t('Tekst bewerken')}>
                <IconButton
                  aria-describedby={'edit-' + key} color="inherit" aria-label="edit" component="span" size="small"
                  edge="end" style={{ position: 'absolute', right: -15, top: -5 }}
                  disabled={isReadOnly || !canEdit}
                  onClick={e => setEdit({ key, value, anchorEl: e.currentTarget })}>
                  <Edit />
                </IconButton>
              </Tooltip>}
              {edit && edit.key === key &&
                <Popover open={true}
                  transformOrigin={{ vertical: 'top', horizontal: 'right' }}
                  id={'edit-' + key}
                  anchorEl={edit.anchorEl}
                  onClose={() => setEdit(null)}>
                  <div className={classes.generalInfoText}>
                    <Grid container spacing={1} direction="column">
                      <Grid item>
                        <Typography variant="h6">{title}</Typography>
                      </Grid>
                      <Grid item>
                        {rich ? <RichTextEditor label={t('Algemene informatie')}
                          showToolbar={true} value={edit.value}
                          onChange={value => setEdit({ anchorEl: edit.anchorEl, key, value })} />
                          :
                          <TextField className={classes.textField} value={edit.value}
                            onChange={e => setEdit({ anchorEl: edit.anchorEl, key, value: e.target.value })}
                            margin="dense" variant="outlined" multiline={multi} rows={10} fullWidth
                            autoFocus disabled={selectedDossier.status === 2} />}
                      </Grid>
                      <Grid item>
                        <Grid container spacing={1} justify="flex-end">
                          <Grid item>
                            <Button
                              disabled={updateLoading && dossierUpdateType === key}
                              variant="outlined"
                              onClick={() => setEdit(null)}>
                              {t('Annuleer')}
                            </Button>
                          </Grid>
                          <Grid item>
                            <Button variant="outlined"
                              disabled={updateLoading && dossierUpdateType === key}
                              onClick={() => updateDossier(key, edit.value)}>
                              {
                                updateLoading && dossierUpdateType === key ? <CircularProgress size={20} /> : 'Opslaan'
                              }
                            </Button>
                          </Grid>
                        </Grid>
                      </Grid>
                    </Grid>
                  </div>
                </Popover>
              }
            </div>
          }
          </div>
        }
      </div>
    )
  };

  return (
    <Grid item xs={12} >
      <input
        ref={fileInputRef}
        className={classes.fileInput}
        type="file"
        accept={"image/x-png,image/gif,image/jpeg"}
        onChange={handleUploadBackgroundImage}
      />
      <div className={classes.block}>
        <Grid container spacing={1}>
          <Grid item xs={12}>
            <Typography component="h2" variant="h6" className={classes.subHeader}>
              <Description color="primary" /> &nbsp;
              {isBuyer ? `Dossier - ${selectedDossier.name}` : (!selectedDossierBuilding.buildingId ? t('Dossier instellingen') : t('Instellingen per object'))}
            </Typography>
            <Grid container spacing={2}>
              <Grid item xs={12} md={8}>
                <Grid container>
                  <Grid container className={classes.infoGridRow}>
                    <Grid item xs={12}>{t('Algemene tekst') + ':'}</Grid>
                    <Grid item xs={12}>
                      {isBuyer ? (selectedDossier.generalInformation && <Markdown
                        value={selectedDossier.generalInformation} />) : renderEditTextbox(t('Algemene tekst'), 'generalinformation', selectedDossier.generalInformation, true, true)}
                    </Grid>
                  </Grid>
                  <Grid item xs={12}>
                    <Divider />
                  </Grid>
                </Grid>
              </Grid>
              <Grid item xs={12} md={4}>
                <Grid container>
                  {canShowOptions && !isReadOnly &&
                    <Grid item xs={12}>
                      <Grid container>
                        <Grid item xs={12} sm={6} md={12}>
                          <Grid container className={classes.infoGridRow}>
                            <Grid item xs={4}>{t('Kopersdossier') + ':'}</Grid>
                            <Grid item xs={8}>
                              {
                                updateLoading && dossierUpdateType == 'extern' ?
                                  <CircularProgress size={24} />
                                  :
                                  <Switch
                                    color="primary"
                                    disabled={!canEdit}
                                    checked={selectedDossier.isExternal === true || isReadOnly}
                                    onChange={e => { updateDossier('extern', (e.target.checked).toString()) }}
                                  />
                              }
                            </Grid>
                            <Grid item xs={12}><Divider /></Grid>
                          </Grid>
                        </Grid>
                        {canEdit && <Grid item xs={12} sm={6} md={12}>
                          <Grid container className={classes.infoGridRow}>
                            <Grid item xs={4}>{t('Achtergrond afbeelding') + ':'}</Grid>
                            <Grid item xs={8}>
                              <React.Fragment>
                                <Button
                                  variant="contained"
                                  color="primary"
                                  className={classes.button}
                                  onClick={() => {
                                    fileInputRef.current.click()
                                  }}
                                  startIcon={<CloudUpload />}
                                  component="span"
                                  disabled={uploadingBackground || isReadOnly}>
                                  {uploadingBackground ?
                                    <CircularProgress color="primary" size={24} /> : t('dossier.general.uploadImageButton.title')}
                                </Button>
                              </React.Fragment>
                            </Grid>
                            <Grid item xs={12}>
                              <Divider />
                            </Grid>
                          </Grid>
                        </Grid>}
                        <Grid item xs={12} sm={6} md={12}>
                          <Grid container className={classes.infoGridRow}>
                            <Grid item xs={4}>{t('Objecten') + ':'}</Grid>
                            <Grid item xs={8}>
                              <Tooltip title={t('Objecten bewerken')}>
                                <IconButton
                                  aria-describedby={'edit-objects'}
                                  color="inherit"
                                  aria-label="edit"
                                  component="span"
                                  size="small"
                                  edge="end"
                                  onClick={openEditObjects}
                                  disabled={isReadOnly}>
                                  {!canEdit ? <Visibility /> : <Edit />}
                                </IconButton>
                              </Tooltip>
                            </Grid>
                            <Grid item xs={12}><Divider /></Grid>
                          </Grid>
                        </Grid>
                        <Grid item xs={12} sm={6} md={12}>
                          <Grid container className={classes.infoGridRow}>
                            <Grid item xs={4}>{t('Rechten') + ':'}</Grid>
                            <Grid item xs={8}>
                              <Tooltip title={t('Rechten bewerken')}>
                                <IconButton
                                  aria-describedby={'edit-objects'}
                                  color="inherit"
                                  aria-label="edit"
                                  component="span"
                                  size="small"
                                  disabled={(updateLoading && dossierUpdateType === 'rights') || isReadOnly}
                                  edge="end"
                                  onClick={openDossierRights}>
                                  {!canEdit ? <Visibility /> : <Edit />}
                                </IconButton>
                              </Tooltip>
                            </Grid>
                            <Grid item xs={12}>
                              <Divider />
                            </Grid>
                          </Grid>
                        </Grid>
                      </Grid>
                    </Grid>}
                  <Grid item xs={12}>
                    <Grid container>
                      <Grid container alignItems="center" className={classes.infoGridRow} item xs={12} sm={6} md={12}>
                        <Grid item xs={4}>{t('dossier.general.status.title') + ':'}</Grid>
                        <Grid item xs={8}>
                          {canEdit && !selectedDossier.isArchived && ((selectedDossier.buildingInfoList && selectedDossier.buildingInfoList.length === 0) || (selectedDossierBuilding.buildingId)) ?
                            <Grid component="label" container alignItems="center" spacing={1}>
                              <Grid item>{t('dossier.general.status.value.1')}</Grid>
                              <Grid item>
                                {props.isUpdateStatus ? <CircularProgress size={24} /> :
                                  <Switch
                                    onChange={() => updateStatus(status === 1)}
                                    color="primary"
                                    checked={status !== 1} />}
                              </Grid>
                              <Grid item>{t('dossier.general.status.value.2')}</Grid>
                            </Grid> : t('dossier.general.status.value.' + status)}
                        </Grid>
                        <Grid item xs={12}>
                          <Divider />
                        </Grid>
                      </Grid>
                      {canShowOptions && (!selectedDossierBuilding.buildingId) && <>
                        <Grid container alignItems="center" className={classes.infoGridRow} item xs={12} sm={6} md={12}>
                          <Grid item xs={4}>{t('Archiveren') + ':'}</Grid>
                          <Grid item xs={8}>
                            <Grid component="label" container alignItems="center" spacing={1}>
                              <Grid item xs={12}>
                                {canEdit ? <Tooltip
                                  title={t(!selectedDossier.isArchived ? 'Archiveren' : 'Dearchiveren')}>
                                  <IconButton
                                    style={{ paddingLeft: 0 }}
                                    disabled={(updateLoading && dossierUpdateType === 'archive')}
                                    aria-label="Archive" color="inherit"
                                    onClick={() => updateDossier('archive', (!selectedDossier.isArchived).toString())}>
                                    {updateLoading && dossierUpdateType == 'archive' ? <CircularProgress size={24} /> :
                                      selectedDossier.isArchived ? <Unarchive color={"primary"} /> : <Archive color={"primary"} />
                                    }
                                  </IconButton>
                                </Tooltip> : t(selectedDossier.isArchived ? 'Archiveren' : 'Dearchiveren')}
                              </Grid>
                            </Grid>
                          </Grid>
                          <Grid item xs={12}>
                            <Divider />
                          </Grid>
                        </Grid>
                      </>}
                      <Grid item xs={12} sm={6} md={12}>
                        <MuiPickersUtilsProvider utils={DateFnsUtils} locale={nlLocale}>
                          <Dialog open={deadlineConfirmationDialog.open}>
                            <DialogTitle>{t('confirmation.text')}</DialogTitle>
                            <DialogContent>
                              <DialogContentText> {t('dossier.deadline.confirmation.message')} </DialogContentText>
                            </DialogContent>
                            <DialogActions>
                              <Button color="primary" onClick={() => {
                                setDeadlineConfirmationDialog({ date: null, open: false })
                                handleDossierDeadlineUpdate(deadlineConfirmationDialog.date || '', false)
                              }}>{t('button.action.no')}</Button>
                              <Button color="primary" autoFocus onClick={() => {
                                setDeadlineConfirmationDialog({ date: null, open: false })
                                handleDossierDeadlineUpdate(deadlineConfirmationDialog.date || '', true)
                              }}>{t("button.action.yes")}</Button>
                            </DialogActions>
                          </Dialog>
                          <Grid container className={classes.infoGridRow}>
                            <Grid item xs={4}>{t('Deadline') + ':'}</Grid>
                            <Grid item xs={8}>
                              {(!readOnly && canEdit) ? <DatePicker
                                variant="inline"
                                margin="dense"
                                id="date-time-picker"
                                label={t('dossier.general.deadline.title')}
                                format="dd-MM-yyyy"
                                helperText={!deadline ? t('dossier.nodeadline') : ''}
                                value={deadline}
                                onChange={(date) => {
                                  if (selectedDossierBuilding.buildingId || !selectedDossier.buildingInfoList.length)
                                    updateDossier('deadline', date.toJSON())
                                  else
                                    setDeadlineConfirmationDialog({ open: true, date: date.toJSON() })
                                }}
                                inputVariant="outlined"
                                autoOk
                                ampm={false}
                                fullWidth
                                disabled={readOnly || (updateLoading && dossierUpdateType === 'deadline') || isDeadLineUpdating}
                                clearable
                                InputProps={{
                                  endAdornment: (deadline || (updateLoading && dossierUpdateType === 'deadline')) && (
                                    <IconButton
                                      disabled={readOnly || (updateLoading && dossierUpdateType === 'deadline') || isDeadLineUpdating}
                                      style={{ padding: 0 }} onClick={(e) => {
                                        e.stopPropagation()
                                        if (selectedDossierBuilding.buildingId || !selectedDossier.buildingInfoList.length)
                                          updateDossier('deadline', null)
                                        else
                                          setDeadlineConfirmationDialog({ open: true, date: null })
                                      }}>
                                      {(updateLoading && dossierUpdateType === 'deadline') || isDeadLineUpdating ?
                                        <CircularProgress size={20} /> :
                                        <Clear />
                                      }
                                    </IconButton>
                                  ),
                                  className: classes.datePicker
                                }}
                              /> : deadline ? formatDate(new Date(deadline)) : t('dossier.nodeadline')}
                            </Grid>
                            <Grid item xs={12}>
                              <Divider />
                            </Grid>
                          </Grid>
                        </MuiPickersUtilsProvider>
                      </Grid>
                      {status === 2 &&
                        <>
                          <Grid container className={classes.infoGridRow} item xs={12} sm={6} md={12}>
                            <Grid item xs={4}>{t('dossier.general.close.title') + ':'}</Grid>
                            <Grid item xs={8}>
                              {
                                closedOn && formatDate(new Date(closedOn))
                              }
                            </Grid>
                            <Grid item xs={12}>
                              <Divider />
                            </Grid>
                          </Grid>
                        </>
                      }
                    </Grid>
                  </Grid>
                </Grid>
              </Grid>
            </Grid>
          </Grid>
        </Grid>
      </div>
    </Grid>
  );
};


const useStyles = makeStyles((theme) => ({
  grow: {
    flexGrow: 1
  },
  filesContainer: {
    maxHeight: 410,
    overflow: 'auto'
  },
  attachmentContainer: {
    padding: theme.spacing(0.5)
  },
  block: {
    width: '100%',
    backgroundColor: theme.palette.grey[100],
    padding: theme.spacing(1, 1, 1),
  },

  button: {
    '&:hover': {
      color: theme.palette.primary.contrastText
    }
  },
  caption: {
    width: '100%',
    textAlign: 'center'
  },
  subHeader: {
    padding: theme.spacing(2),
    '& svg': {
      fontSize: 30
    }
  },
  infoGridRow: {
    '& > div': {
      padding: theme.spacing(0.5, 2),
    },
    '&:hover': {
      backgroundColor: theme.palette.action.hover,
      "& $allSelectCheckbox": {
        opacity: 1
      }
    },
    '& .MuiInputLabel-outlined': {
      whiteSpace: 'nowrap',
      maxWidth: '100%',
      overflow: 'hidden',
      textOverflow: 'ellipsis'
    }
  },
  thumbnail: {
    backgroundPosition: 'center',
    backgroundSize: 'contain',
    backgroundRepeat: 'no-repeat',
    height: 80,
    // padding: '35% 0',
    width: '100%',
    display: 'block',

  },
  checked: {
    fill: theme.palette.common.white
  },
  overlay: {
    position: 'absolute',
    bottom: 0,
    background: 'rgba(0, 0, 0, 0.5)', /* Black see-through */
    color: ' #f1f1f1',
    width: '100%',
    transition: '.5s ease',
    opacity: 0,
    fontSize: '20px',
    textAlign: 'center',
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    height: '100%',
  },
  thumnailContainer: {
    position: 'relative',
    cursor: 'pointer',
    height: '120px',
    width: '120px',
    minHeight: 120,
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    justifyContent: 'space-evenly',
    '&:hover': {
      "& $overlay": {
        opacity: 1
      },
      "& $attachmentStatus": {
        opacity: 1,
        right: 10
      },
    }
  },
  imagePreviewIcon: {
    fill: theme.palette.common.white,
    width: '1.5em',
    height: '1.5em'
  },
  datePicker: {
    paddingRight: `${theme.spacing(0.5)} !important`
  },
  allSelectCheckbox: {
    opacity: 0,
    transition: '0.5s all'
  },
  attachmentStatus: {
    position: 'fixed',
    backgroundColor: '#fff',
    boxShadow: '1px 1px 6px #eee',
    bottom: 10,
    padding: theme.spacing(1.25),
    marginTop: '23px',
    right: '-100%',
    opacity: 0,
    minWidth: 300,
    transition: '1s all',
    zIndex: 2,
    ['@media screen and (max-width:668px)']: {
      bottom: 66,
    },
  },
  fileInput: {
    display: 'none'
  },
  rightAlignCheckBox: {
    position: 'absolute',
    right: 0,
    top: 0,
    padding: 0
  },
  generalInfoText: {
    padding: theme.spacing(2)
  }
}));
