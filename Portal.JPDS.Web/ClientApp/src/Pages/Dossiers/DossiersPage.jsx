import React, { createRef } from "react";
import { connect } from "react-redux";
import {
	AppBar,
	CircularProgress,
	Container,
	Grid,
	IconButton,
	isWidthUp,
	Slide,
	Snackbar,
	Toolbar,
	Tooltip,
	Typography,
	withWidth,
} from "@material-ui/core";
import { withStyles } from '@material-ui/core/styles';
import { ArrowBack, ArrowRight, Share, SwapHoriz } from "@material-ui/icons"
import { withTranslation } from 'react-i18next';
import clsx from "clsx";
import { totalFilesSizeIsValid, formatDate, getMimeTypefromString, history, isValidFiles, downloadFile } from "../../_helpers";
import AddDossier from "./AddDossier";
import EditDossierObjects from "./EditDossierObjects";
import DossierRights from "./components/DossierRights";
import DocumentViewer from "../../components/DocumentViewer";
import DossiersList from "./components/DossiersList";
import DraftDossiers from "./components/DraftDossiers";
import ArchivedDossiers from "./components/ArchivedDossiers";
import ObjectList from "./components/ObjectList";
import ShareDossier from "./Common/ShareDossier";
import { dossiersActions } from "../../_actions/dossiers.actions";
import { commonActions } from "../../_actions/common.actions";
import { Route, Switch, generatePath } from 'react-router-dom';
import DossierInformation from "./components/DossierInformation";
import { uploadBackgroundImage } from "../../apis/dossiersApi";
import { URLS } from "../../apis/urls";
import { userAccountTypeConstants } from "../../_constants";
import { Alert } from "@material-ui/lab";


const { webApiUrl } = window.appConfig;

const styles = theme => ({
	mAuto: {
		margin: 'auto'
	},
	bold: {
		fontWeight: 'bold'
	},
	grow: {
		flexGrow: 1
	},
	mainContainer: {
		height: '100%',
		width: '100%',
		overflow: 'auto',
		padding: 0
	},
	container: {
		height: '100%',
		backgroundColor: theme.palette.background.paper,
		[theme.breakpoints.down("xs")]: {
			marginTop: theme.spacing(0)
		}
	},
	loadingContainer: {
		height: '100%',
		display: 'flex',
		alignItems: 'center',
		justifyContent: 'center'
	},
	innerContainer: {
		position: 'relative',
		height: 'calc(100% - 48px)',
		overflow: 'hidden',
		zIndex: 1
	},
	dossiersContainer: {
		backgroundColor: theme.palette.background.paper,
		height: '100%',
		position: 'relative',
		zIndex: 1
	},
	dossiersWrapper: {
		height: '100%',
		width: '100%',
		overflowX: 'hidden',
		overflowY: 'auto'
	},
	switch: {
		position: 'absolute',
		zIndex: 9999999,
		right: -10
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
	buildingList: {
		flexGrow: 1,
		maxHeight: 'calc(100% - 48px)',
		overflowX: 'hidden',
		overflowY: 'auto',
		width: '100%',
		zIndex: 1,
	},
	blocksContainer: {
		backgroundColor: theme.palette.background.paper,
		backgroundPosition: 'center',
		backgroundSize: 'cover',
		height: '100%',
		width: '100%',
		overflow: 'auto',
		padding: theme.spacing(4),
		[theme.breakpoints.down("md")]: {
			position: 'absolute',
			right: 0
		},
		[theme.breakpoints.down("xs")]: {
			padding: 0
		},
	},
	block: {
		width: '100%',
		backgroundColor: theme.palette.grey[100],
		padding: theme.spacing(1, 1, 3),
	},
});


class Page extends React.Component {
	state = {
		dossiers: [],
		drafts: [],
		archived: []
	};

	openShareDossierMenuRef = createRef();

	componentDidMount() {
		let viewAsParam = this.props.match.params.viewType;
		this.setState({ viewAs: viewAsParam })
		if (this.props.selected) {
			if (this.props.user.type === userAccountTypeConstants.buyer) {
				this.props.dispatch(dossiersActions.getDossiersByBuildingId(this.props.selected.buildingId));
			} else
				this.props.dispatch(dossiersActions.getAllDossiersByProject(this.props.selected.projectId));
			this.props.dispatch(dossiersActions.getRoles(this.props.selected.projectId));
		}
	}

	componentDidUpdate(prevProps, prevState) {
		const { viewAs } = this.state;
		let viewAsParam = this.props.match.params.viewType;
		if (viewAs && viewAs !== viewAsParam) this.setState({ viewAs: viewAsParam });
		if (viewAsParam === 'dossier' && (JSON.stringify(prevProps.dossiers) !== JSON.stringify(this.props.dossiers) || prevProps.match.url !== this.props.match.url)) {
			const { dossiers } = this.props;
			if (this.props.user.type === userAccountTypeConstants.buyer && dossiers && !dossiers.length) {
				history.push('/home');
				return;
			}
			let dossierId = this.props.match.params.selectedDataId;
			if (!this.props.loadingDossiersList && (dossiers && dossiers.length || dossierId)) {
				if (this.props.user.type === userAccountTypeConstants.buyer && this.props.selected) {
					dossierId = dossierId || dossiers[0].id;
					this.props.dispatch(dossiersActions.getSelectedDossierInfo(dossierId, this.props.selected.buildingId, viewAs));
					history.push(this.generatePath({ selectedDataId: dossierId }) + '?buildingId=' + this.props.selected.buildingId);
				} else {
					if (dossierId) {
						const params = new URLSearchParams(window.location.search)
						const buildingId = params.get('buildingId');
						this.props.dispatch(dossiersActions.getSelectedDossierInfo(dossierId, buildingId));
						if (!buildingId) {
							this.setState({ buildingId, openBuildingSelection: true })
							history.push(`${this.props.match.url}`);
						} else {
							this.setState({ openBuildingSelection: true })
						}
					} else {
						this.setState({ selectedDossier: null });
						this.props.dispatch(dossiersActions.removeSelectedDossier())
					}
				}
			}
		}
		if (viewAs === 'building' && (this.props.buildingList.length && JSON.stringify(prevProps.buildingList.map(x => x.buildingId)) !== JSON.stringify(this.props.buildingList.map(x => x.buildingId)))) {
			const { buildingList } = this.props;
			const params = new URLSearchParams(window.location.search)
			const buildingId = this.props.match.params.selectedDataId;
			if (buildingId) {
				const dossierId = params.get('dossierId') || buildingList.find(b => b.buildingId === buildingId).dossierList[0].id;
				this.props.dispatch(dossiersActions.getSelectedDossierBuildingInfo({ dossierId, buildingId }, viewAs));
				this.setState({ buildingId })
				history.push(`${this.generatePath({ selectedDataId: buildingId })}?dossierId=${dossierId}`);
			}
		}

		if (prevProps.selectedDossier !== this.props.selectedDossier) {
			const { selectedDossier } = this.props;
			const params = new URLSearchParams(window.location.search)
			const buildingId = params.get('buildingId');
			const dossierId = params.get('dossierId');
			if (buildingId || dossierId) {
				let selectedDossierBuilding = {};
				if (selectedDossier && selectedDossier.buildingInfoList) {
					selectedDossierBuilding = selectedDossier.buildingInfoList.find(p => p.buildingId === (buildingId || this.state.buildingId));
				}
				this.setState({ selectedDossierBuilding, buildingId: buildingId || this.state.buildingId })
				this.props.dispatch(commonActions.updateRights(buildingId || this.state.buildingId))
			}
			this.setState({ selectedDossier })
		}

		if ((!prevProps.selected && this.props.selected)
			|| (prevProps.selected && this.props.selected && prevProps.selected.projectId.toUpperCase() !== this.props.selected.projectId.toUpperCase())) {
			this.setState({ selectedDossier: null, selectedDossierBuilding: {} });
			this.props.dispatch(dossiersActions.removeSelectedDossier())
			if (prevProps.selected && prevProps.selected.projectId && this.props.selected && this.props.selected.projectId) {
				history.replace(`/werk/${this.props.selected.projectNo}/${this.state.viewAs}`);
			} else {
				if (this.state.viewAs === 'dossier') {
					if (this.props.user.type === userAccountTypeConstants.buyer) {
						this.props.dispatch(dossiersActions.getDossiersByBuildingId(this.props.selected.buildingId));
					} else
						this.props.dispatch(dossiersActions.getAllDossiersByProject(this.props.selected.projectId));
				} else if (this.state.viewAs === 'building') {
					this.props.dispatch(dossiersActions.getBuildingWithDossiers(this.props.selected.projectId));
				}
			}
			this.props.dispatch(dossiersActions.getRoles(this.props.selected.projectId));
		}
	}

	generatePath = (params) => {
		if (this.props.match.params.selectedDataId)
			return generatePath(this.props.match.path, { ...this.props.match.params, ...params })
		return `${this.props.match.url}/${params.selectedDataId}`;
	}

	handleSelectDossier = (dossier, selectedDossierType, building) => {
		if (this.state.viewAs === 'dossier') {
			if (!this.props.selectedDossier || (this.props.selectedDossier.id !== dossier.id)) {
				if (this.props.user.type === userAccountTypeConstants.buyer && this.props.selected) {
					history.push(this.generatePath({ selectedDataId: dossier.id }) + '?buildingId=' + this.props.selected.buildingId);
					this.props.dispatch(dossiersActions.getSelectedDossierInfo(dossier.id, this.props.selected.buildingId, this.state.viewAs));
				} else {
					history.push(this.generatePath({ selectedDataId: dossier.id }));
					this.setState({
						selectedDossierType,
						selectedDossierBuilding: {},
						buildingId: null,
						openBuildingSelection: true,
						openDossier: false
					});
				}
			} else {
				this.setState({ buildingId: null, selectedDossierBuilding: {}, openBuildingSelection: true })
			}
		} else {
			const dossierId = dossier ? dossier.id : building.dossierList[0].id;
			const buildingId = building ? building.buildingId : this.state.buildingId;
			this.props.dispatch(dossiersActions.getSelectedDossierBuildingInfo({ dossierId, buildingId }, this.state.viewAs));
			history.push(`${this.generatePath({ selectedDataId: buildingId })}?dossierId=${dossierId}`);
		}
	}

	handleOpenUpdateDossier = (dossier) => {
		this.setState({ isUpdateDossier: true, updatingDossierId: dossier.id });
	}

	goBack(matchesWidthUpLg, matchesWidthUpSm) {
		const { openDossier, openBuildingSelection, buildingId, viewAs } = this.state;
		const { selected } = this.props;
		var goToHome = false;
		if (matchesWidthUpLg === true) {
			goToHome = true;
		} else if (matchesWidthUpSm === true) {
			if (openDossier === true) {
				this.setState({ openDossier: false })
			} else {
				goToHome = true;
			}
		} else {
			if (openDossier === true) {
				this.setState({ openDossier: false })
			} else if (openBuildingSelection === true) {
				this.setState({ openBuildingSelection: false })
			} else {
				goToHome = true;
			}
		}
		if (buildingId && viewAs === 'dossier') {
			this.setState({ buildingId: null })
			history.push(`${this.props.match.url}`)
		} if (!buildingId && viewAs === 'dossier' && this.state.selectedDossier) {
			this.setState({ selectedDossier: null })
			this.props.dispatch(dossiersActions.removeSelectedDossier())
			history.push(this.props.match.url.replace('/' + this.props.match.params.selectedDataId, ''))
			return
		}
		if (this.state.selectedDossierBuilding && viewAs === 'building') {
			this.setState({ buildingId: null, isOpenDossierBuildingInfo: false, selectedDossierBuilding: null, selectedDossier: null })
			this.props.dispatch(dossiersActions.removeSelectedDossier())
			history.push(`/werk/${selected.projectNo}/building`)
			return;
		}
		if (selected && goToHome === true) {
			history.push(this.props.match.url)
		}
	}

	handleDeleteDraft = dossier => {
		this.props.dispatch(dossiersActions.deleteDraftDossier(dossier.id));
	}

	updateDossier = (key, value, isUpdateAllDeadline) => {
		let selectedDossier = this.state.selectedDossier;
		const buildingId = key !== 'archive' && this.state.buildingId;
		this.setState({ dossierUpdateType: key });
		if (key === 'backgroundImage') {
			this.setState({ uploadingBackground: true })
			uploadBackgroundImage({ dossierId: selectedDossier.id, ...value }).then(res => {
				this.props.dispatch(dossiersActions.getBackground(selectedDossier.id));
				this.setState({ uploadingBackground: false })
			}).catch(er => {
				this.setState({ uploadingBackground: false })
			})
			return;
		}
		this.props.dispatch(dossiersActions.updateDossierInformation({
			[key]: value,
			id: selectedDossier.id,
			buildingId
		}, !!buildingId));
	};
	updateStatus = (isClosed) => {
		let selectedDossier = this.state.selectedDossier;
		this.props.dispatch(dossiersActions.updateStatus({
			dossierId: selectedDossier.id,
			isClosed,
			buildingId: this.state.buildingId || '',
			type: this.state.viewAs
		}));
	};

	handleUpdateDossierClose = () => {
		this.setState({ updating: null, edit: null, });
	}

	handleSaveObjects = (objects, changedObjects) => {
		let selectedDossier = this.state.selectedDossier;
		changedObjects.length &&
			this.props.dispatch(dossiersActions.updateDossierBuilding({ dossierId: selectedDossier.id, changedObjects, objects }))
	}

	handleCloseObjects = () => {
		this.setState({ openEditObjects: false });
	}

	handleUpdateDossierRights = ({ availableRoles, changedRoles }) => {
		this.props.dispatch(dossiersActions.updateDossiersRights(this.props.selectedDossier.id, changedRoles, availableRoles))
	}
	handleCloseDossierRights = () => {
		this.setState({ openDossierRights: false });
	}

	handleSelectFiles = (files, fileData, type) => {
		if (!isValidFiles(files)) {
			this.setState({
				filesSizeIsInValid: true, fileUploadErrorMessage: 'files.total.type.notValid'
			})
			return;
		}
		if (totalFilesSizeIsValid(files, 25)) {
			let formData = new FormData();
			formData.append('dossierId', this.state.selectedDossier.id);
			formData.append('buildingId', this.state.buildingId || "");
			formData.append('isInternal', fileData.type === 'internal');
			formData.append('DossierFileList', []);
			formData.append('isArchived', !!fileData.isArchived);
			formData.append('isDeleted', !!fileData.isDeleted);
			files.forEach(f => formData.append('files', f));
			this.props.dispatch(dossiersActions.uploadFiles(formData, {
				key: fileData.type === 'internal' ? this.state.buildingId ? 'internalObjectFiles' : 'internalFiles' : this.state.buildingId ? 'externalObjectFiles' : 'externalFiles',
				buildingId: this.state.buildingId
			}));
		} else this.setState({
			filesSizeIsInValid: true, fileUploadErrorMessage: 'files.total.size.notValid'
		})
	};

	handlePreviewAndDownloadImages = (type, files) => {
		if (files && files.length) {
			let documents = [];
			type === 'preview' && this.setState({ isLoadingDocument: true, isOpenImageLightBox: true, });
			files.forEach(file => {
				if (type === 'download') {
					try {
						downloadFile(URLS.GET_UPLOADED_DOSSIER_IMAGE + file.fileId, file.name + file.extension);
					} catch (er) {
						this.setState({ isLoadingDocument: false, document: [] });
					}
				} else if (type === 'preview') {
					documents.push({
						uri: (webApiUrl === '/' ? window.location.origin : '') + URLS.GET_UPLOADED_DOSSIER_IMAGE + file.fileId,
						fileType: getMimeTypefromString(file.extension),
						name: file.name + file.extension,
						description: `${file.uploadedBy} uploaded ${file.name} on ${formatDate(new Date(file.lastModifiedOn))}`
					});
					if (documents.length === files.length)
						this.setState({ isLoadingDocument: false, document: documents })
				}
			});
		}
	}


	handlePreviewOfFiles = (files) => {
		if (files && files.length) {
			let documents = [];
			this.setState({ isLoadingPreviewOfFile: true, isOpenFilePreviewModal: true })
			files.map(file => {
				documents.push(
					{
						uri: (webApiUrl === '/' ? window.location.origin : '') + URLS.GET_ATTACHMENT + file.fileId,
						fileType: getMimeTypefromString(file.extension),
						name: file.name + file.extension,
						description: `${file.uploadedBy} uploaded ${file.name} on ${formatDate(new Date(file.lastModifiedOn))}`
					}
				);
			})
			if (documents.length === files.length)
				this.setState({ isLoadingPreviewOfFile: false, previewDocument: documents })
		}
	}



	redirectSwitchHandler = () => {
		const type = this.state.viewAs === 'dossier' ? 'building' : 'dossier';
		const { buildingId, selectedDossier } = this.state;
		if (type === 'building') {
			this.props.dispatch(dossiersActions.getBuildingWithDossiers(this.props.selected.projectId));
			this.setState({ selectedDossier: null, buildingId: null })
			this.props.dispatch(dossiersActions.removeSelectedDossier())
			history.push(`/werk/${this.props.selected.projectNo}/${type}/${buildingId}?dossierId=${selectedDossier.id}`)
		}
		else if (type === 'dossier') {
			history.push(`/werk/${this.props.selected.projectNo}/${type}/${selectedDossier.id}` + (buildingId ? `?buildingId=${buildingId}` : ''));
		}
	}

	handleSwitcher = () => {
		const viewAs = this.state.viewAs === 'dossier' ? 'building' : 'dossier';
		if (viewAs === 'building') {
			this.props.dispatch(dossiersActions.getBuildingWithDossiers(this.props.selected.projectId));
		}
		this.setState({ selectedDossier: null, buildingId: null })
		this.props.dispatch(dossiersActions.removeSelectedDossier())
		history.push(`/werk/${this.props.selected.projectNo}/${viewAs}`);
	}

	handleSelectBuilding(data) {
		this.setState(data);
		if (!data.buildingId)
			this.props.dispatch(commonActions.updateRights())
	}

	render() {
		const { t, loadingDossiersList, mainLoading, allBuildings, dossierLoading,
			classes, buildingList, width, selected, buildings,
			user, isUpdateStatus, backgroundImage, loadingDossierInfo, dossiers,
			archived, drafts, rights } = this.props;
		const { viewAs, selectedDossier, buildingId, document, previewDocument, selectedDossierBuilding = {} } = this.state;
		const matchesWidthUpLg = isWidthUp('lg', width);
		const matchesWidthUpSm = isWidthUp('sm', width);
		const loggedInUserFromRights = selectedDossier && (selectedDossier.userList || []).find(u => u.loginId === user.id);
		const isBuyer = user.type === userAccountTypeConstants.buyer;
		const isExternal = selectedDossier && selectedDossier.isExternal && (loggedInUserFromRights || isBuyer);
		const isInternal = loggedInUserFromRights && loggedInUserFromRights.isInternal;
		const hasExternalEditRights = isExternal && (isBuyer || (loggedInUserFromRights && loggedInUserFromRights.hasExternalEditRights))
		const accessRights = { isExternal, isInternal, hasExternalEditRights };
		const selectedBuilding = buildingId && allBuildings.find(b => b.buildingId === buildingId);
		const selectedTitleName = selectedBuilding || (selectedBuilding && viewAs === 'building') ? selectedBuilding.buildingNoExtern : t('Algemeen');
		const canSwitchView = rights['dossier.canSwitchView'] && !isBuyer && selectedDossier && !selectedDossier.isArchived && !loadingDossiersList;
		if (mainLoading) return (<div className={classes.loadingContainer}>
			<CircularProgress size={55} color={'primary'} />
		</div>);

		return (
			<Container className={classes.mainContainer} maxWidth={false}>
				<Snackbar
					style={{ top: 120 }}
					TransitionComponent={(props) => <Slide {...props} direction="left" />}
					anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
					open={this.state.filesSizeIsInValid}
					onClose={() => this.setState({ filesSizeIsInValid: false })}
					autoHideDuration={3000} key={'file-size-validation'}>
					<Alert elevation={6} variant="filled" severity="error">
						{t(this.state.fileUploadErrorMessage)}
					</Alert>
				</Snackbar>
				<DocumentViewer loader={this.state.isLoadingDocument} open={this.state.isOpenImageLightBox}
					onClose={() => this.setState({ isOpenImageLightBox: false, document: [] })} documents={document} />

				<DocumentViewer loader={this.state.isLoadingPreviewOfFile} open={this.state.isOpenFilePreviewModal}
					onClose={() => this.setState({ isOpenFilePreviewModal: false, previewDocument: [] })} documents={previewDocument} />

				<Grid container className={classes.container}>
					<AppBar position="sticky">
						<Toolbar variant="dense">
							{
								<React.Fragment>
									<Grid item lg={isBuyer ? 2 : 3}>
										<IconButton edge="start" aria-label="GoBack" color="inherit"
											onClick={() => this.goBack(matchesWidthUpLg, matchesWidthUpSm)}>
											<ArrowBack />
										</IconButton>
										{rights['dossier.canCreateDossier'] && <AddDossier selectedBuilding={selected}
											roles={this.props.availableRoles}
											updatingDossierId={this.state.updatingDossierId}
											isUpdateDossier={this.state.isUpdateDossier}
											onCancelUpdating={() => this.setState({
												isUpdateDossier: false,
												updatingDossierId: null,
											})}
											buildings={buildings.filter(x => x.projectId === selected.projectId)} />
										}
									</Grid>
									{(!loadingDossiersList && selectedDossier) &&
										<>
											<div className={clsx(classes.grow, classes.bold)} style={{ display: 'flex' }} noWrap>
												{(this.state.openDossier || matchesWidthUpSm || buildingId) && !dossierLoading && selectedDossier && (viewAs === 'dossier' ? (
													<div style={{ display: 'flex' }}>
														<Typography>{selectedDossier.name}</Typography>
														{
															isBuyer !== true &&
															<>
																<ArrowRight />
																<Typography onClick={() => (canSwitchView && selectedBuilding) ? this.redirectSwitchHandler() : null} style={{ cursor: canSwitchView && selectedBuilding && 'pointer' }}>{selectedTitleName}</Typography>
															</>
														}
													</div>
												) : (
													<div style={{ display: 'flex' }}>
														<Typography>{selectedTitleName}</Typography> <ArrowRight />
														<Typography onClick={canSwitchView && selectedBuilding && this.redirectSwitchHandler} style={{ cursor: canSwitchView && selectedBuilding && 'pointer' }}>{selectedDossier.name}</Typography>
													</div>))}
											</div>

											{!isBuyer && !loadingDossierInfo && selectedDossier &&
												<Tooltip title={t('Delen')}>
													<IconButton aria-label="Delen" edge="end" color="inherit"
														onClick={() => this.setState({ openShareDossierMenu: true })}>
														<Share />
													</IconButton>
												</Tooltip>}
										</>
									}
								</React.Fragment>
							}
						</Toolbar>
					</AppBar>
					{<Grid item xs={12} container className={classes.innerContainer}>
						{viewAs === 'dossier' ? <Grid item xs={12} sm={isBuyer ? 12 : 9} md={isBuyer && 12} lg={2} container
							className={classes.dossiersContainer} style={{ zIndex: 2 }} direction="column">
							{
								selectedDossier && canSwitchView && viewAs === 'dossier' &&
								<IconButton className={classes.switch} aria-label="Filter" color="inherit" edge="end"
									onClick={this.handleSwitcher}>
									<SwapHoriz style={{ fill: 'white' }} />
								</IconButton>
							}
							<div className={classes.dossiersWrapper}>
								<DossiersList viewAs={viewAs} dossiers={dossiers} handleSwitcher={this.handleSwitcher}
									selectedDossier={selectedDossier} isBuyer={isBuyer}
									handleSelectDossier={this.handleSelectDossier} />
								{
									rights['dossier.draftList.read'] && <DraftDossiers
										dossiers={drafts} selectedDossier={selectedDossier}
										handleDeleteDraft={this.handleDeleteDraft}
										handleUpdateDraft={this.handleOpenUpdateDossier}
										canEdit={rights['dossier.draftList.write']}
									/>
								}
								{
									rights['dossier.archiveList.read'] && <ArchivedDossiers dossiers={archived} selectedDossier={selectedDossier}
										handleSelectDossier={this.handleSelectDossier} />
								}
							</div>
						</Grid> :
							!isBuyer && <ObjectList
								match={this.props.match}
								generatePath={this.generatePath}
								handleSwitcher={this.handleSwitcher}
								handleSelectDossier={this.handleSelectDossier}
								selectedDossier={selectedDossier} buildingId={buildingId}
								show
								openDossier={this.state.openDossier}
								handleSelectBuilding={data => this.handleSelectBuilding(data)} />
						}
						{
							<>
								{
									!!selectedDossier && <>
										{viewAs === 'dossier' ? (!isBuyer && <ObjectList
											match={this.props.match}
											generatePath={this.generatePath}
											matchesWidthUpSm={matchesWidthUpSm}
											selectedDossier={selectedDossier} buildingId={buildingId}
											show={this.state.openBuildingSelection}
											openDossier={this.state.openDossier}
											handleSelectBuilding={data => this.handleSelectBuilding(data)} />)
											: <Grid item xs={12} sm={9} lg={2} container className={classes.dossiersContainer}
												direction="column">
												<div className={classes.dossiersWrapper}>
													<DossiersList
														isFullHeight viewAs={viewAs} isBuyer={isBuyer}
														dossiers={buildingId && buildingList.length && buildingList.find(b => b.buildingId === buildingId).dossierList}
														handleSwitcher={this.handleSwitcher} selectedDossier={selectedDossier}
														handleSelectDossier={this.handleSelectDossier} />
												</div>
											</Grid>
										}
									</>
								}
								<Slide direction="left" in={matchesWidthUpLg || this.state.openDossier || buildingId} mountOnEnter unmountOnExit>
									<Grid item xs={12} lg={isBuyer || !selectedDossier || loadingDossiersList ? viewAs === 'building' ? 11 : 10 : 9}
										style={{
											display: loadingDossierInfo || loadingDossiersList ? 'flex' : null,
											zIndex: (this.state.openDossier || buildingId) && 1103,
											backgroundImage: `url(${backgroundImage}), url(${webApiUrl}api/home/ProjectBackground/${selected.projectId}), url(${webApiUrl}api/Config/WebBackground)`
										}}
										className={classes.blocksContainer}>
										{loadingDossierInfo || loadingDossiersList ?
											<CircularProgress className={classes.mAuto} />
											:
											!!selectedDossier && <Grid container >
												<Switch>
													<Route
														exact path={`${this.props.match.path}`}
														render={(props) => <DossierInformation
															{...props}
															buildingId={buildingId}
															dossierUpdateType={this.state.dossierUpdateType}
															handleGetAllImages={this.handlePreviewAndDownloadImages}
															handlePreviewOfFiles={this.handlePreviewOfFiles}
															updateStatus={this.updateStatus}
															isUpdateStatus={isUpdateStatus}
															handleUpdateDossierClose={this.handleUpdateDossierClose}
															handleSelectFiles={this.handleSelectFiles}
															selectedDossierBuilding={selectedDossierBuilding} selectedDossier={selectedDossier}
															accessRights={accessRights}
															selectedDossierType={this.state.selectedDossierType}
															edit={this.state.edit} setEdit={edit => this.setState({ edit })}
															openDossierRights={() => this.setState({
																openDossierRights: true,
																dossierUpdateType: 'rights'
															})}
															openEditObjects={() => this.setState({ openEditObjects: true })}
															updateDossier={this.updateDossier} updating={this.state.updating}
															uploadingBackground={this.state.uploadingBackground}
															selected={selected} buildings={buildings} fileUploading={this.state.fileUploading}
															loggedInUserFromRights={loggedInUserFromRights}
														/>} />
												</Switch>
											</Grid>
										}
									</Grid>
								</Slide>
							</>
						}
					</Grid>
					}
				</Grid>
				{
					this.state.openEditObjects &&
					<EditDossierObjects
						isReadOnly={!rights["dossier.generalInformation.write"]}
						open={this.state.openEditObjects}
						buildings={buildings.filter(x => x.projectId === selected.projectId)}
						selectedObjects={selectedDossier.buildingInfoList}
						onSave={this.handleSaveObjects}
						onClose={this.handleCloseObjects}
					/>
				}
				{
					this.state.openDossierRights &&
					<DossierRights
						isReadOnly={!rights["dossier.generalInformation.write"]}
						open={this.state.openDossierRights}
						selectedRoles={selectedDossier.userList}
						availableRoles={this.props.availableRoles}
						onClose={this.handleCloseDossierRights}
						onUpdate={this.handleUpdateDossierRights}
						isDossierExternal={selectedDossier.isExternal}
					/>
				}
				{
					this.state.openShareDossierMenu && <ShareDossier
						selectedDossier={selectedDossier}
						buildingId={buildingId}
						isReadOnly={!rights["selected.object.write"] && !rights["dossier.canShare"]}
						open={this.state.openShareDossierMenu}
						onClose={() => {
							this.setState({ openShareDossierMenu: false })
						}}
					/>
				}
			</Container >
		);
	}
}

function mapStateToProps(state) {
	const { authentication, buildings, dossier } = state;
	const { user } = authentication;
	const { selected, all, buyerGuidesBuildings, loading, rights } = buildings;
	const { dossiers, selectedDossier, selectedBackground, availableRoles, selectedLoading, isUpdateStatus, buildingList, updateLoading, dossierLoading } = dossier;
	return {
		user,
		selected,
		allBuildings: all,
		buildings: buyerGuidesBuildings || [],
		rights,
		mainLoading: loading,
		selectedDossier,
		isUpdateStatus,
		buildingList,
		updateLoading,
		availableRoles,
		dossiers: dossiers.openOrClosedDossiers,
		dossierLoading,
		archived: dossiers.archiveDossiers,
		drafts: dossiers.draftDossiers,
		loadingDossierInfo: selectedLoading,
		loadingDossiersList: dossier.loading,
		backgroundImage: selectedBackground,
	};
}

const connectedPage = connect(mapStateToProps)(withWidth()(withTranslation()(withStyles(styles)(Page))));
export { connectedPage as DossiersPage };
