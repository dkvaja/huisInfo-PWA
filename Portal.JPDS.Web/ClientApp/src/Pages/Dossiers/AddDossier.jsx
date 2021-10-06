import React, { useEffect, useState } from "react";
import {
	Button,
	CardHeader,
	CircularProgress,
	Container,
	Dialog,
	DialogContent,
	DialogTitle,
	FormControlLabel,
	Grid,
	IconButton,
	List,
	ListItem,
	ListItemAvatar,
	ListItemSecondaryAction,
	ListItemText,
	makeStyles,
	Switch,
	TextField,
	Tooltip,
	Typography
} from "@material-ui/core";
import { Add, Clear, Close, Edit } from "@material-ui/icons";
import { useTranslation } from "react-i18next";
import { formatFileSize, toBase64, validateFile } from "../../_helpers";
import { DatePicker, MuiPickersUtilsProvider } from "@material-ui/pickers";
import nlLocale from "date-fns/locale/nl";
import DateFnsUtils from "@date-io/date-fns";
import RichTextEditor from "../Chat/RichTextEditor";
import EditDossierObjects from "./EditDossierObjects";
import DossierRights from "./components/DossierRights";
import { useDispatch, useSelector } from "react-redux";
import { dossiersActions } from "../../_actions/dossiers.actions";
import { DialogActions } from "@material-ui/core";
import { getBackgroundImage, getDossierGeneralInfo } from "../../apis/dossiersApi";

const useStyles = makeStyles((theme) => ({
	grow: {
		flexGrow: 1
	},
	dialogTitle: {
		padding: 0,
		backgroundColor: theme.palette.primary.main,
		color: theme.palette.primary.contrastText,
		'& .MuiCardHeader-content': {
			overflow: 'hidden'
		},
		'& .MuiCardHeader-action': {
			marginBottom: -8
		}
	},
	dialogContent: {
		//padding: 0
	},
	loaderContainer: {
		height: 500,
		maxHeight: '100%',
		display: 'flex',
		justifyContent: 'center',
		alignItems: 'center'
	}
}));

export default function AddDossier(props) {
	const { selectedBuilding, updatingDossierId, isUpdateDossier, roles, buildings, onCancelUpdating, ...rest } = props;
	const dossierProperties = {
		projectId: selectedBuilding && selectedBuilding.projectId,
		name: "",
		status: 1,
		generalInformation: "",
		backgroundImage: null,
		deadline: null,
		isDraft: false,
		hasGeneralFiles: true,
		hasObjectBoundFiles: true,
		buildingInfoList: [],
		userList: []
	}
	const { t } = useTranslation();
	const classes = useStyles();
	const [openDialog, setOpenDialog] = useState(isUpdateDossier || false);
	const [isLoading, setIsLoading] = useState(false);
	const [openEditObjects, setOpenEditObjects] = useState(null);
	const [openRightsDialog, setOpenRightsDialog] = useState(null);
	const [isCreating, setIsCreating] = useState(false);
	const [getBackgroundLoading, setGetBackgroundLoading] = useState();
	const { selectedLoading, addLoading } = useSelector(state => state.dossier);
	const [hasDeadline, setHasDeadline] = useState(true);
	const [uploading, setUploading] = useState(false);
	const [selectedDossier, setSelectedDossier] = useState();
	const [disabledBuildingSwitches, setDisabledBuildingSwitches] = useState(false);
	const dispatch = useDispatch();
	const [isUpdated, setIsUpdated] = useState(false);
	const [changedRights, setChangedRights] = useState([]);

	useEffect(() => {
		if (!addLoading && isUpdated) {
			handleClose();
			setIsUpdated(false)
		}
		addLoading && setIsUpdated(true);
	}, [addLoading]);

	const [dossier, setDossier] = useState(dossierProperties);
	const [isValidData, setIsValidData] = useState(true);
	const { user } = useSelector(state => state.authentication);

	useEffect(() => {
		if (isUpdateDossier && updatingDossierId) {
			setIsLoading(true);
			getDossierGeneralInfo(updatingDossierId).then(({ data }) => {
				const { hasBackground, backgroundImageName } = data;
				if (data) {
					setDossier({
						...data,
						backgroundImage: hasBackground && { name: backgroundImageName }
					});
				}
				setSelectedDossier(data);
				setGetBackgroundLoading(true)
				getBackgroundImage(updatingDossierId).then(({ data: image }) => {
					// const url = window.URL.createObjectURL(image)
					const blob = new Blob([image])
					const size = blob && blob.size;
					blob && toBase64(blob).then(res => {
						setDossier(p => ({ ...p, backgroundImage: { ...p.backgroundImage, url: res, size } }))
					})
					// setDossier(p => ({ ...p, backgroundImage: { ...p.backgroundImage, url, size: size, } }))
					setGetBackgroundLoading(false)
				}).catch(er => {
					setGetBackgroundLoading(false)
				});
				setIsLoading(false)
			}).catch(er => {
				setIsLoading(false)
			})
		}
	}, [isUpdateDossier, updatingDossierId])

	useEffect(() => {
		if (!isUpdateDossier) {
			const userList = roles.map(({ usersList, ...r }) => usersList.filter(u => u.loginId === user.id).map(u => {
				return { ...u, hasRights: true, hasExternalEditRights: true, ...r, };
			})).flat();
			setDossier({ ...dossierProperties, userList });
		}
	}, [selectedDossier, isUpdateDossier]);

	useEffect(() => {
		const userList = roles.map(({ usersList, ...r }) => usersList.filter(u => u.loginId === user.id).map(u => {
			return { ...u, hasRights: true, hasExternalEditRights: true, ...r, };
		})).flat();
		dossierProperties.userList = userList;
		setDossier(p => ({ ...p, userList }));
	}, [roles]);

	useEffect(() => {
		isValid();
	}, [dossier.name, dossier.deadline, hasDeadline]);

	const handleClick = () => {
		setOpenDialog(true);
	};

	const handleClose = () => {
		setDossier(dossierProperties)
		setOpenDialog(false);
		isUpdateDossier && onCancelUpdating()
	};

	const handleCloseObjects = () => {
		setOpenEditObjects(false);
	};

	const handleSaveObjects = (objects) => {
		// setSelectedObjects(objects);
		setDossier(p => ({
			...p,
			buildingInfoList: objects.map(p => ({
				buildingId: p.buildingId,
				isActive: true
			})),
		}));
		handleCloseObjects();
	}
	const handleCloseRightsDialog = rights => {
		setDossier(p => ({ ...p, userList: rights.availableRoles }));
		isUpdateDossier && setChangedRights(rights.changedRoles);
		setOpenRightsDialog(false);
	};

	const handleAddDossier = create => {
		setIsCreating(create);
		dossier.isDraft = !create;
		dossier.status = create ? 1 : 0;
		dispatch(dossiersActions.addUpdateDossier({
			...dossier,
			dossierId: dossier.id,
			projectId: selectedBuilding.projectId,
			userList: isUpdateDossier ? changedRights : dossier.userList,
			backgroundImage: dossier.backgroundImage ? {
				content: dossier.backgroundImage.url.split(',')[1],
				name: dossier.backgroundImage.name
			} : null
		}));
	};

	const handleChangeName = ({ target: { value } }) => {
		setDossier(p => ({ ...p, name: value }));
	}

	async function handleSelectFile(e) {
		const selectedFiles = Array.from(e.target.files);
		if (selectedFiles.length === 1) {
			let file = selectedFiles[0];
			if (validateFile(file, true) === true) {
				let currenturl = await toBase64(file)
				file.url = currenturl;
				setDossier(p => ({ ...p, backgroundImage: file }));
				// setFile(file);
			}
		}
	}

	const handleRemoveFile = () => {
		// setFile(null);
		setDossier(p => ({ ...p, backgroundImage: null }));
	}

	const isValid = () => {
		let valid = dossier.name && dossier.name.length >= 2 && dossier.name.length <= 100;
		if (hasDeadline) valid = valid && dossier.deadline;
		setIsValidData(valid)
	}


	return (
		<>
			{selectedBuilding &&
				<Tooltip title={t("Nieuw dossier")}>
					<IconButton color="inherit" onClick={handleClick}>
						<Add />
					</IconButton>
				</Tooltip>
			}
			<Dialog open={openDialog || (isUpdateDossier && !selectedLoading)} onClose={handleClose}
				aria-labelledby="form-dialog-title"
				maxWidth='xs' fullWidth >
				<DialogTitle id="simple-dialog-title" className={classes.dialogTitle} disableTypography>
					<CardHeader id="transition-dialog-title"
						title={
							<Typography variant="h6" noWrap>{selectedBuilding.projectName}</Typography>
						}
						action={
							<div>
								<IconButton color="inherit" aria-label="close" onClick={handleClose}>
									<Close />
								</IconButton>
							</div>
						} />
				</DialogTitle>
				{isLoading ?
					<Container className={classes.loaderContainer}>
						<CircularProgress size={25} />
					</Container>
					: <>
						<DialogContent className={classes.dialogContent}>
							<div className={classes.formData}>
								<Grid container spacing={1}>
									<Grid item xs={12}>
										<TextField
											error={dossier.name.length > 100}
											label={t('Naam')}
											helperText={dossier.name.length > 100 && t('dossier.name.maxLength.message')}
											className={classes.textField}
											value={dossier.name}
											onChange={handleChangeName}
											margin="dense"
											variant="outlined"
											fullWidth
											disabled={uploading}
										/>

									</Grid>
									<Grid item xs={12}>
										<FormControlLabel
											style={{ marginLeft: 0 }}
											value={dossier.hasGeneralFiles}
											control={
												<Switch
													color="primary"
													checked={dossier.isExternal}
													onChange={e => {
														setDossier(p => ({ ...p, isExternal: e.target.checked }))
													}
													}
												/>
											}
											label={t('Kopersdossier') + ':'}
											labelPlacement="start"
										/>
									</Grid>
									<Grid item xs={12}>
										{t('Objecten') + ':'}
										&nbsp;
										<Tooltip title={t('Objecten bewerken')}>
											<IconButton
												aria-describedby={'edit-objects'}
												color="inherit"
												aria-label="edit"
												component="span"
												//size="small"
												edge="end"
												onClick={e => setOpenEditObjects(true)}
											>
												<Edit />
											</IconButton>
										</Tooltip>
									</Grid>
									<Grid item xs={12}>
										{t('Achtergrond afbeelding') + ':'} &nbsp;
										<input accept="image/*" style={{ display: 'none' }} disabled={uploading} id="icon-button-file" type="file"
											onChange={handleSelectFile} />
										<label htmlFor="icon-button-file" style={{ margin: 0 }}>
											{
												uploading ? <CircularProgress color="inherit" size={24} />
													:
													<IconButton color="inherit" aria-label="upload" component="span">
														{!!dossier.backgroundImage ? <Edit /> : <Add />}
													</IconButton>
											}
										</label>
										{
											!!dossier.backgroundImage &&
											<List dense>
												{
													<ListItem>
														<ListItemAvatar>
															{
																getBackgroundLoading ? <CircularProgress size={20} /> :
																	<img src={dossier.backgroundImage.url} style={{ maxHeight: 50, marginRight: 8 }} />
															}
														</ListItemAvatar>
														<ListItemText
															primary={dossier.backgroundImage.name}
															secondary={formatFileSize(dossier.backgroundImage.size)}
														/>
														<ListItemSecondaryAction>
															<IconButton edge="end" aria-label="delete" disabled={uploading} onClick={handleRemoveFile}>
																<Clear />
															</IconButton>
														</ListItemSecondaryAction>
													</ListItem>
												}
											</List>
										}
									</Grid>
									<Grid item xs={6}>
										<FormControlLabel
											style={{ marginLeft: 0 }}
											value={hasDeadline}
											control={
												<Switch
													color="primary"
													checked={hasDeadline}
													onChange={e => setHasDeadline(e.target.checked)}
												/>
											}
											label={t('Deadline') + ':'}
											labelPlacement="start"
										/>
									</Grid>
									{
										hasDeadline &&
										<>
											<Grid item xs={6}>
												<MuiPickersUtilsProvider utils={DateFnsUtils} locale={nlLocale}>
													<DatePicker
														variant="inline"
														margin="dense"
														id="date-time-picker"
														label={t('Deadline datum')}
														format="dd-MM-yyyy"
														minDate={new Date()}
														helperText={""}
														value={dossier.deadline}
														onChange={(date) => {
															setDossier(p => ({ ...p, deadline: date.toJSON() }))
															// setDeadline(date.toJSON())
														}}
														inputVariant="outlined"
														autoOk
														ampm={false}
														fullWidth
														required
													/>
												</MuiPickersUtilsProvider>
											</Grid>
										</>
									}
									<Grid item xs={12}>
										{t('Rechten') + ':'} &nbsp;
										<Tooltip title={t('Rechten bewerken')}>
											<IconButton
												aria-describedby={'edit-rights'}
												color="inherit"
												aria-label="edit"
												component="span"
												//size="small"
												edge="end"
												onClick={() => setOpenRightsDialog(true)}
											>
												<Edit />
											</IconButton>
										</Tooltip>
									</Grid>
									<Grid item xs={12}>
										{t('Algemeen informatie') + ':'} &nbsp;
										<RichTextEditor
											label={t('Algemeen informatie')}
											showToolbar={true}
											onChange={data => {
												setDossier(p => ({ ...p, generalInformation: data }))
												// setGeneralInfo(data)
											}}
											value={dossier.generalInformation}
										/>
									</Grid>
									<Grid item xs={12}>
										<FormControlLabel
											style={{ marginLeft: 0 }}
											disabled={disabledBuildingSwitches && dossier.hasGeneralFiles}
											value={dossier.hasGeneralFiles}
											control={
												<Switch
													disabled={disabledBuildingSwitches && dossier.hasGeneralFiles}
													color="primary"
													checked={dossier.hasGeneralFiles}
													onChange={e => {
														setDossier(p => ({ ...p, hasGeneralFiles: e.target.checked }))
														setDisabledBuildingSwitches(!e.target.checked)
													}
													}
												/>
											}
											label={t('Algemene bestanden') + ':'}
											labelPlacement="start"
										/>
									</Grid>
									<Grid item xs={12}>
										<FormControlLabel
											disabled={disabledBuildingSwitches && dossier.hasObjectBoundFiles}
											style={{ marginLeft: 0 }}
											value={dossier.hasObjectBoundFiles}
											control={
												<Switch
													color="primary"
													disabled={disabledBuildingSwitches && dossier.hasObjectBoundFiles}
													checked={dossier.hasObjectBoundFiles}
													onChange={e => {
														setDossier(p => ({ ...p, hasObjectBoundFiles: e.target.checked }))
														setDisabledBuildingSwitches(!e.target.checked)
													}}
												/>
											}
											label={t('Objectgebonden bestanden') + ':'}
											labelPlacement="start"
										/>
									</Grid>

								</Grid>
							</div>
						</DialogContent>
						<DialogActions>
							{/* <Grid item xs> */}
							<Grid container spacing={1} justify="flex-end">
								<Grid item>
									<Button
										disabled={addLoading || !isValidData}
										variant="outlined"
										onClick={() => handleAddDossier(false)}
									>
										{addLoading && !isCreating ? <CircularProgress size={20} /> :
											<Typography variant={'p'}>{t('Concept opslaan')}</Typography>}
									</Button>
								</Grid>
								<Grid item>
									<Button
										disabled={addLoading || !isValidData}
										variant="outlined"
										onClick={() => handleAddDossier(true)}>
										{addLoading && isCreating ? <CircularProgress size={20} /> :
											<Typography variant={'p'}>{t('Maak dossier')}</Typography>}
									</Button>
								</Grid>
							</Grid>
							{/* </Grid> */}
						</DialogActions></>}

			</Dialog>
			{
				openEditObjects &&
				<EditDossierObjects open={openEditObjects} buildings={buildings}
					selectedObjects={dossier.buildingInfoList || []} onSave={handleSaveObjects}
					onClose={handleCloseObjects} />
			}
			{
				openRightsDialog &&
				<DossierRights
					availableRoles={roles}
					selectedRoles={dossier.userList}
					open={openRightsDialog}
					onClose={() => setOpenRightsDialog(false)}
					onUpdate={handleCloseRightsDialog}
					isDossierExternal={true}
					isNewDossier={true} />
			}
		</>
	);
}
