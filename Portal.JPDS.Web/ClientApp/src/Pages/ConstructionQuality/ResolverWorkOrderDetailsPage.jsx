import React from "react";
import { connect } from "react-redux";
import {
    Container,
    Grid,
    Typography,
    AppBar,
    Toolbar,
    IconButton,
    MenuItem,
    Modal,
    Fade,
    Card,
    Backdrop,
    CardMedia,
    Button,
    TextField,
    FormControlLabel,
    Checkbox,
    isWidthUp,
    withWidth,
    Divider,
    Popover,
    Menu,
    Snackbar,
    Tooltip
} from "@material-ui/core";
import { withStyles } from '@material-ui/core/styles';
import { ArrowBack, Clear, Close, Build, Home, Contacts, Assignment, Schedule, Share, FileCopyOutlined } from "@material-ui/icons"
import { withTranslation } from 'react-i18next';
import clsx from "clsx";
import { history, nl2br, formatDate, authHeader, validateFile, formatTime } from '../../_helpers';
import ContactCard from "./RepairRequestContactCard";
import { Alert } from "@material-ui/lab";
import Dropzone from "../../components/Dropzone";

const { webApiUrl } = window.appConfig;

const styles = theme => ({
    bold: {
        fontWeight: "bold"
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
        backgroundColor: theme.palette.background.paper,
        overflow: 'hidden',
        [theme.breakpoints.down("xs")]: {
            marginTop: theme.spacing(0)
        }
    },
    innerContainer: {
        padding: theme.spacing(2, 3, 4)
    },
    block: {
        backgroundColor: theme.palette.grey[100],
        padding: theme.spacing(1, 1, 3),
        '&.collapsed': {
            paddingBottom: theme.spacing(1)
        }
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
            backgroundColor: theme.palette.action.hover
        },
        '& .MuiInputLabel-outlined': {
            whiteSpace: 'nowrap',
            maxWidth: '100%',
            overflow: 'hidden',
            textOverflow: 'ellipsis'
        }
    },
    imageGallery: {
        width: '100%',
        maxHeight: '15vw',
        height: 'calc(100% + 4px)',
        overflowX: 'hidden',
        overflowY: 'auto',
        [theme.breakpoints.down("xl")]: {
            maxHeight: '19vw',
        },
        [theme.breakpoints.down("lg")]: {
            maxHeight: '19vw',
        },
        [theme.breakpoints.down("md")]: {
            maxHeight: '28vw',
        },
        [theme.breakpoints.down("sm")]: {
            maxHeight: '56vw',
        },
        [theme.breakpoints.down("xs")]: {
            maxHeight: '85vw',
        }
    },
    dropzoneContainer: {
        width: "25%",
        float: "left",
        padding: theme.spacing(0.5),
        margin: "-2px auto -4px",
        [theme.breakpoints.down("lg")]: {
            width: '33.33%',
        },
        [theme.breakpoints.down("xs")]: {
            width: "50%",
        },
    },
    galleryImageTile: {
        width: "25%",
        float: "left",
        padding: theme.spacing(0.5),
        position: "relative",

        '& > button': {
            position: "absolute",
            top: 0,
            right: 0
        },
        "& > div": {
            width: "100%",
            padding: "50% 0px",
            height: 0,
            backgroundSize: 'cover',
            backgroundPosition: 'center',

        },
        [theme.breakpoints.down("lg")]: {
            width: "33.33%",
        },
        [theme.breakpoints.down("xs")]: {
            width: "50%",
        },
    },
    bigAvatar: {
        margin: 'auto',
        width: 120,
        height: 120
    },
    halfWidth: {
        width: '50%',
        verticalAlign: 'top'
    },
    button: {
        '&:hover': {
            color: theme.palette.primary.contrastText
        }
    },
    textField: {
        marginBottom: theme.spacing(1),
        [theme.breakpoints.up("xs")]: {
            width: 350,
            maxWidth: '100%',
        }
    },
    thumbnail: {
        width: 50,
        height: 50,
        backgroundRepeat: 'no-repeat',
        backgroundPosition: 'center',
        backgroundSize: 'cover',
        backgroundColor: 'rgba(0,0,0,0.1)',
        '&.big': {
            width: '100%',
            height: 0,
            padding: '50%',
            cursor: 'pointer',
            //borderRadius: theme.spacing(1)
        }
    },
    thumbnailOrg: {
        width: 'calc(100% - 16px)',
        margin: theme.spacing(-1, 0),
        height: 50,
        backgroundRepeat: 'no-repeat',
        backgroundPosition: 'left center',
        backgroundSize: 'contain',
    },
    modal: {
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
    },
    dataTable: {
        height: '100%',
        display: 'flex',
        flexDirection: 'column',
        //overflow: 'hidden',
        background: 'none',
        boxShadow: 'none',
        '& > .MuiPaper-root': {
            flex: '0 1 auto',
        },
        '& > .MuiToolbar-root.MuiToolbar-regular': {
            display: 'flex',
            padding: theme.spacing(0, 0, 0, 2),
            '& .header svg': {
                fontSize: 30
            },
            '& > div': {
                padding: 0,
                textAlign: 'right',
                flex: '1 0 auto',
                '& .MuiTypography-root': {
                    textAlign: 'left'
                }
            }
        },
        '& .MuiTable-root': {
            '& caption': {
                display: 'none'
            }
            //marginBottom: theme.spacing(0.5)
        },
        '& .MuiTableCell-root': {
            padding: theme.spacing(0, 0.5, 0, 0),
            '&.MuiTableCell-paddingCheckbox': {
                paddingLeft: theme.spacing(0.5),
                '& > div': {
                    justifyContent: 'center'
                },
                '& .MuiCheckbox-root': {
                    padding: theme.spacing(0.25)
                }
            },
            '&.MuiTableCell-head': {
                fontWeight: 'bold',
                backgroundColor: theme.palette.grey[100],
            }
        }
    },
    formControlCheckList: {
        paddingLeft: theme.spacing(0.5),
        '& .MuiTypography-body1': {
            lineHeight: 1
        }
    }
});


class Page extends React.Component {
    state = {
        attachmentIndex: null,
        resolverAttachmentIndex: null,
        workOrder: null,
        building: null,
        shareAnchorEl: null,
        openAlert: false,
        copiedMessage: ""
    };

    componentDidMount() {
        this.GetWorkOrderDetails();
    }

    componentDidUpdate(prevProps, prevState) {
        if (!prevProps.selected || prevProps.selected.buildingId.toUpperCase() !== this.props.selected.buildingId.toUpperCase()) {
            //this.GetWorkOrderDetails();
        }
        if (
            (!prevState.workOrder && !!this.state.workOrder)
            ||
            (!!prevState.workOrder && !!this.state.workOrder && prevState.workOrder.resolverId !== this.state.workOrder.resolverId)
        ) {
            if (this.props.selected && this.props.selected.buildingId.toUpperCase() !== this.state.workOrder.buildingId.toUpperCase()) {
                var selectedItem = this.props.allBuildings.find(x => x.buildingId.toUpperCase() === this.state.workOrder.buildingId.toUpperCase());
                if (selectedItem.projectId !== this.props.selected.projectId) {
                    history.push('/werk/' + this.props.selected.projectNo + '/werkbonnen');
                }
                else {
                    this.setState({ building: selectedItem });
                }
            }
            else {
                this.setState({ building: this.props.selected });
            }
        }
    }

    GetWorkOrderDetails(update = false) {
        const { resolverId } = this.props.match.params;
        if (resolverId) {
            const url = webApiUrl + 'api/RepairRequest/GetWorkOrderDetails/' + encodeURI(resolverId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            if (update === false) {
                this.setState({
                    workOrder: null,
                });
            }

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(workOrder => {
                    if (!!workOrder.resolverId) {
                        workOrder.completed = workOrder.status === 3 || workOrder.status === 4;
                        this.setState({
                            workOrder,
                            isTemporary: (workOrder.status == 0 && !workOrder.workOrderNumber),
                            completeOrRejectionText: workOrder.status === 3 ? workOrder.rejectionText : (workOrder.status === 4 ? workOrder.solutionText : this.state.completeOrRejectionText)
                        });
                        this.updateWorkOrderAsInformed();
                    }
                });
        }
    }

    updateWorkOrderAsInformed() {
        const { t } = this.props;
        const { workOrder } = this.state;

        if (!!workOrder && workOrder.status === 0) {
            this.setState({ updating: 'status' })

            const url = webApiUrl + `api/RepairRequest/UpdateResolverStatusAsInformed/` + encodeURI(workOrder.resolverId);
            const requestOptions = {
                method: 'POST',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(success => {
                    if (success) {
                        this.setState({ updating: null });
                        this.GetWorkOrderDetails(true);
                    }
                    else {
                        this.setState({ updating: null });
                    }
                })
                .catch(e => {
                    this.setState({ updating: null });
                    alert(t('general.api.error'));
                })
        }
    }

    initiateHandle = (e, handleType) => {
        if (['complete', 'reject'].includes(handleType)) {
            const { workOrder, isTemporary, completeOrRejectionText } = this.state;
            if (workOrder && isTemporary !== true && workOrder.completed !== true) {
                if (completeOrRejectionText && completeOrRejectionText.trim() !== '') {
                    this.setState({ anchorElHandleWorkOrder: e.currentTarget, handleType });
                }
                else {
                    this.setState({ completeOrRejectionTextError: true });
                }
            }
        }
    }

    handleWorkOrder = () => {
        const { t, selected } = this.props;
        const { workOrder, isTemporary, handleType, completeOrRejectionText } = this.state;
        if (workOrder && isTemporary !== true && workOrder.completed !== true) {
            if (completeOrRejectionText && completeOrRejectionText.trim() !== '' && ['complete', 'reject'].includes(handleType)) {
                this.setState({ handling: true })

                let notification = this.createModelForEmailNotify();
                let isComplete = false;
                let isCompleteRepairRequest = false;
                switch (handleType) {
                    case 'complete': isComplete = true; break;
                    case 'reject': isComplete = false; break;
                    default: break;
                }

                const model = {
                    resolverId: workOrder.resolverId,
                    completeOrRejectionText,
                    isComplete,
                    continuedWorkOrder: false,
                    isCompleteRepairRequest,
                    notification
                };


                const url = webApiUrl + `api/RepairRequest/UpdateWorkOrderStatus`;
                const requestOptions = {
                    method: 'POST',
                    headers: authHeader('application/json'),
                    body: JSON.stringify(model)
                };

                fetch(url, requestOptions)
                    .then(Response => Response.json())
                    .then(success => {
                        if (success) {
                            this.setState({ handling: false });
                            history.push("/werk/" + selected.projectNo + "/werkbonnen")
                        }
                        else {
                            this.setState({ handling: false });
                            alert(t('general.api.error'));
                        }
                    })
                    .catch(e => {
                        this.setState({ handling: false });
                        alert(t('general.api.error'));
                    })
            }
            else {
                this.setState({ showCreateWorkOrderErrors: true });
            }
        }
    }

    createModelForEmailNotify = () => {
        const { isNotify } = this.state;
        const model = {
            isNotify: isNotify === true
        };

        return model;
    }

    uploadAttachment = (files) => {
        let filesToUpload = [];
        for (let i = 0; i < files.length; i++) {
            if (validateFile(files[i], true) === true) {
                filesToUpload.push(files[i]);
            }
        }

        if (filesToUpload.length > 0) {
            const { t } = this.props;
            const { workOrder } = this.state;
            this.setState({
                uploading: true
            })

            if (workOrder && workOrder.completed !== true) {
                const formData = new FormData()

                for (var i = 0; i < filesToUpload.length; i++) {
                    formData.append('files', filesToUpload[i])
                }

                const url = webApiUrl + `api/RepairRequest/UploadWorkOrderImages/` + encodeURI(workOrder.resolverId);
                fetch(url, {
                    method: 'POST',
                    headers: authHeader(),
                    body: formData
                })
                    .then(res => res.json())
                    .then(res => {
                        if (res.success === true) {
                            this.setState({
                                uploading: false
                            })
                            this.GetWorkOrderDetails(true);
                        }
                        else {
                            this.setState({ uploading: false });
                            alert(t('general.api.error'));
                        }
                    })
                    .catch(e => {
                        this.setState({ uploading: false });
                        alert(t('general.api.error'));
                    })
            }
        }
    }

    handleRemoveAttachFile(attachmentId) {
        const { t } = this.props;
        const { workOrder } = this.state;
        if (workOrder && workOrder.completed !== true) {
            const url = webApiUrl + `api/RepairRequest/DeleteRepairRequestAttachment/${workOrder.repairRequestId}/${attachmentId}`;
            const requestOptions = {
                method: 'DELETE',
                headers: authHeader('application/json')
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(res => {
                    if (res.success === true) {
                        this.GetWorkOrderDetails(true);
                    }
                    else {
                        alert(t('general.api.error'));
                    }
                })
                .catch(e => {
                    alert(t('general.api.error'));
                })
        }
    }

    handleImageModalOpen = index => {
        this.setState({ attachmentIndex: index });
    }

    handleImageModalClose = () => {
        this.setState({ attachmentIndex: null });
    }

    handleImageModal2Open = index => {
        this.setState({ resolverAttachmentIndex: index });
    }

    handleImageModal2Close = () => {
        this.setState({ resolverAttachmentIndex: null });
    }

    handleShare(e) {
        this.setState({
            shareAnchorEl: e.currentTarget
        })
    }

    blockGeneral() {
        const { t, classes } = this.props;
        const { workOrder, isTemporary } = this.state;
        return (
            <div className={classes.block}>
                <Typography component="h2" variant="h6" className={classes.subHeader} >
                    <Build color="primary" /> &nbsp;
                {t('Werkbon')}
                    &nbsp;
                {(workOrder && (workOrder.workOrderNumber ? workOrder.workOrderNumber : workOrder.repairRequestNo + '-##'))}
                </Typography>
                <Grid container>
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={6}>{t('Oplosser') + ':'}</Grid>
                            <Grid item xs={6}>{workOrder.name && workOrder.name}</Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={6}>{t('Melding') + ':'}</Grid>
                            <Grid item xs={6}>{workOrder.repairRequestNo && workOrder.repairRequestNo}</Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={6}>{t('Relatie') + ':'}</Grid>
                            <Grid item xs={6}>{workOrder.relationName}</Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={6}>{t('Omschrijving') + ':'}</Grid>
                            <Grid item xs={6}>{workOrder.description && workOrder.description}</Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                    {
                        workOrder && workOrder.previousWorkOrderDetails &&
                        <>
                            <Grid item xs={12}>
                                <Grid container className={classes.infoGridRow}>
                                    <Grid item xs={6}>{t('Werkbon oud') + ': '}</Grid>
                                    <Grid item xs={6}>{workOrder.previousWorkOrderDetails.workOrderNumber}</Grid>
                                </Grid>
                            </Grid>
                            <Grid item xs={12} >
                                <Divider />
                            </Grid>
                            <Grid item xs={12}>
                                <Grid container className={classes.infoGridRow}>
                                    <Grid item xs={6}>{t('Afhandeldatum oud') + ':'}</Grid>
                                    <Grid item xs={6}>{workOrder.previousWorkOrderDetails.handledDate && formatDate(new Date(workOrder.previousWorkOrderDetails.handledDate))}</Grid>
                                </Grid>
                            </Grid>
                            <Grid item xs={12} >
                                <Divider />
                            </Grid>
                            <Grid item xs={12}>
                                <Grid container className={classes.infoGridRow}>
                                    <Grid item xs={6}>{t('Oplossing oud') + ':'}</Grid>
                                    <Grid item xs={6}>{nl2br(workOrder.previousWorkOrderDetails.solution)}</Grid>
                                </Grid>
                            </Grid>
                            <Grid item xs={12} >
                                <Divider />
                            </Grid>
                            <Grid item xs={12}>
                                <Grid container className={classes.infoGridRow}>
                                    <Grid item xs={6}>{t('Toelichting oud') + ':'}</Grid>
                                    <Grid item xs={6}>{nl2br(workOrder.previousWorkOrderDetails.explantion)}</Grid>
                                </Grid>
                            </Grid>
                            <Grid item xs={12} >
                                <Divider />
                            </Grid>
                            <Grid item xs={12}>
                                <Grid container className={classes.infoGridRow}>
                                    <Grid item xs={6}>{t('Werkbontekst oud') + ':'}</Grid>
                                    <Grid item xs={6}>{nl2br(workOrder.prevWorkOrderText)}</Grid>
                                </Grid>
                            </Grid>
                            <Grid item xs={12} >
                                <Divider />
                            </Grid>
                        </>
                    }
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={6}>{t('Werkbontekst') + ':'}</Grid>
                            <Grid item xs={6}>{nl2br(workOrder.workOrderText)}</Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={6}>{t('Toelichting') + ':'}</Grid>
                            <Grid item xs={6}>{nl2br(workOrder.explanation)}</Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={6}>{t('Soort') + ':'}</Grid>
                            <Grid item xs={6}>{workOrder.carryOutAsType}</Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={6}>{t('Melding datum') + ':'}</Grid>
                            <Grid item xs={6}>{workOrder.repairRequestDate && formatDate(new Date(workOrder.repairRequestDate))}</Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={6}>{t('Datum ingelicht') + ':'}</Grid>
                            <Grid item xs={6}>{workOrder.dateNotified && formatDate(new Date(workOrder.dateNotified))}</Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={6}>{t('Streefdatum afhandeling') + ':'}</Grid>
                            <Grid item xs={6}>{workOrder.targetCompletionDate && formatDate(new Date(workOrder.targetCompletionDate))}</Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={12}>{t('Melding afbeeldingen') + ':'}</Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={12}>
                                {
                                    workOrder.repairRequestImages && workOrder.repairRequestImages.length > 0 &&
                                    <div className={classes.imageGallery}>
                                        <Grid container spacing={1}>
                                            {
                                                workOrder.repairRequestImages.map((attachment, index) =>
                                                    <Grid key={index} item xs={6} sm={4} xl={3}>
                                                        <div
                                                            key={index}
                                                            class={clsx(classes.thumbnail, 'big')}
                                                            onClick={() => this.handleImageModalOpen(index)}
                                                            style={{
                                                                backgroundImage: 'url(/api/home/getattachment/' + attachment.attachmentId + ')'
                                                            }} />
                                                    </Grid>
                                                )
                                            }
                                        </Grid>
                                    </div>
                                }
                            </Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                </Grid>
            </div>
        )
    }

    blockStatus() {
        const { t, classes } = this.props;
        const { workOrder, terms } = this.state;
        return (
            <div className={classes.block}>
                <Typography component="h2" variant="h6" className={classes.subHeader} >
                    <Schedule color="primary" /> &nbsp;
                                                {t('Status')}
                </Typography>
                <Grid container>
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={6}>{t('Status') + ':'}</Grid>
                            <Grid item xs={6}>{workOrder.status >= 0 && t('resolver.status.' + workOrder.status)}</Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={6}>{t('Afgehandeld op') + ':'}</Grid>
                            <Grid item xs={6}>{workOrder.dateHandled && formatDate(new Date(workOrder.dateHandled))}</Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                    {
                        workOrder.isHandled === true && <> <Grid item xs={12}>
                            <Grid container className={classes.infoGridRow}>
                                <Grid item xs={6}>{t('Gecontroleerd door') + ':'}</Grid>
                                <Grid item xs={6}>{workOrder.handledBy}</Grid>
                            </Grid>
                        </Grid>
                            <Grid item xs={12} >
                                <Divider />
                            </Grid></>
                    }

                    {
                        workOrder.isHandled === true && <> <Grid item xs={12}>
                            <Grid container className={classes.infoGridRow}>
                                <Grid item xs={6}>{t('Gecontroleerd op ') + ':'}</Grid>
                                <Grid item xs={6}>{workOrder.handledOn && (formatDate(new Date(workOrder.handledOn)) + ' ' + formatTime(new Date(workOrder.handledOn)))}</Grid>
                            </Grid>
                        </Grid>
                            <Grid item xs={12} >
                                <Divider />
                            </Grid></>
                    }

                </Grid>
            </div>
        )
    }

    blockObjectInfo() {
        const { t, classes } = this.props;
        const { workOrder, building } = this.state;
        return (
            <div className={classes.block}>
                <Typography component="h2" variant="h6" className={classes.subHeader} >
                    <Home color="primary" /> &nbsp;
                                                {t('Object informatie')}
                </Typography>
                {
                    <Grid container>
                        <Grid item xs={12}>
                            <Grid container className={classes.infoGridRow}>
                                <Grid item xs={6}>{t('Straat en huisnummer') + ':'}</Grid>
                                <Grid item xs={6}>{workOrder.address && (workOrder.address.street ? workOrder.address.street : "") + " " + (workOrder.address.houseNo ? workOrder.address.houseNo : "")}</Grid>
                            </Grid>
                        </Grid>
                        <Grid item xs={12} >
                            <Divider />
                        </Grid>
                        <Grid item xs={12}>
                            <Grid container className={classes.infoGridRow}>
                                <Grid item xs={6}>{t('Postcode en plaats') + ':'}</Grid>
                                <Grid item xs={6}>
                                    {
                                        workOrder.address &&
                                        <>
                                            {workOrder.address.postcode ? workOrder.address.postcode + " " : ""}&nbsp;{workOrder.address.place ? workOrder.address.place : ""}
                                        </>
                                    }
                                </Grid>
                            </Grid>
                        </Grid>
                        <Grid item xs={12} >
                            <Divider />
                        </Grid>

                        <Grid item xs={12}>
                            <Grid container className={classes.infoGridRow}>
                                <Grid item xs={6}>{t('Bouwnummer') + ':'}</Grid>
                                <Grid item xs={6}>{building && building.buildingNoIntern}</Grid>
                            </Grid>
                        </Grid>
                        <Grid item xs={12} >
                            <Divider />
                        </Grid>
                        <Grid item xs={12}>
                            <Grid container className={classes.infoGridRow}>
                                <Grid item xs={6}>{t('Datum oplevering') + ':'}</Grid>
                                <Grid item xs={6}>{workOrder.completionDate && formatDate(new Date(workOrder.completionDate))}</Grid>
                            </Grid>
                        </Grid>
                        <Grid item xs={12} >
                            <Divider />
                        </Grid>
                        <Grid item xs={12}>
                            <Grid container className={classes.infoGridRow}>
                                <Grid item xs={6}>{t('Datum tweede handtekening') + ':'}</Grid>
                                <Grid item xs={6}>{workOrder.secondSignatureDate && formatDate(new Date(workOrder.secondSignatureDate))}</Grid>
                            </Grid>
                        </Grid>
                        <Grid item xs={12} >
                            <Divider />
                        </Grid>
                        <Grid item xs={12}>
                            <Grid container className={classes.infoGridRow}>
                                <Grid item xs={6}>{t('Einde onderhoudstermijn') + ':'}</Grid>
                                <Grid item xs={6}>{workOrder.maintenanceEndDate && formatDate(new Date(workOrder.maintenanceEndDate))}</Grid>
                            </Grid>
                        </Grid>
                        <Grid item xs={12} >
                            <Divider />
                        </Grid>
                        <Grid item xs={12}>
                            <Grid container className={classes.infoGridRow}>
                                <Grid item xs={6}>{t('Einde garantietermijn') + ':'}</Grid>
                                <Grid item xs={6}>{workOrder.warrantyEndDate && formatDate(new Date(workOrder.warrantyEndDate))}</Grid>
                            </Grid>
                        </Grid>
                        <Grid item xs={12} >
                            <Divider />
                        </Grid>
                    </Grid>
                }
            </div>
        )
    }

    blockWorkOrder() {
        const { t, classes } = this.props;
        const { workOrder, isTemporary, uploading, showCreateWorkOrderErrors } = this.state;
        const completionRejectionLabel = (workOrder.status == 3 ? t('Reden afwijzing') : (workOrder.status == 4 ? t('Oplossing') : t('Uitgevoerde werkzaamheden')));

        return (
            <div className={classes.block}>
                <Typography component="h2" variant="h6" className={classes.subHeader} >
                    <Assignment color="primary" /> &nbsp;
                                                {t('Aanvullende informatie')}
                </Typography>
                <Grid container>
                    {
                        isTemporary !== true &&
                        <>
                            <Grid item xs={12}>
                                <Grid container className={classes.infoGridRow}>
                                    <Grid item xs={12}>
                                        <TextField
                                            label={completionRejectionLabel}
                                            value={this.state.completeOrRejectionText}
                                            onChange={e => this.setState({ completeOrRejectionText: e.target.value })}
                                            margin="dense"
                                            variant="outlined"
                                            multiline
                                            rows={10}
                                            required
                                            error={this.state.completeOrRejectionTextError === true && (!this.state.completeOrRejectionText || this.state.completeOrRejectionText.trim() === '')}
                                            fullWidth
                                            autoFocus
                                            disabled={workOrder.completed === true}
                                        />
                                    </Grid>
                                </Grid>
                            </Grid>
                            <Grid item xs={12} >
                                <Divider />
                            </Grid>
                        </>
                    }
                    {
                        (!!workOrder.workOrderNumber || (workOrder.solutionImages && workOrder.solutionImages.length > 0)) &&
                        <>
                            <Grid item xs={12}>
                                <Grid container className={classes.infoGridRow}>
                                    <Grid item xs={12}>{t('Afbeeldingen') + ':'}</Grid>
                                </Grid>
                                <Grid container className={classes.infoGridRow}>
                                    <Grid item xs={12}>
                                        <div className={classes.imageGallery}>
                                            {
                                                workOrder.completed !== true &&
                                                <div className={classes.dropzoneContainer}>
                                                    <Dropzone onFilesAdded={this.uploadAttachment} disabled={workOrder.completed === true} uploading={uploading} accept="image/*" />
                                                </div>
                                            }
                                            {workOrder.solutionImages && workOrder.solutionImages.length > 0 && workOrder.solutionImages.map((file, index) => (
                                                <div className={classes.galleryImageTile}>
                                                    <div
                                                        onClick={() => this.handleImageModal2Open(index)}
                                                        style={{
                                                            backgroundImage: 'url(' + webApiUrl + 'api/home/getattachment/' + file.attachmentId + ')'
                                                        }}
                                                    ></div>
                                                    {
                                                        workOrder.completed !== true &&
                                                        <Tooltip title={t('Verwijderen')}>
                                                            <IconButton
                                                                aria-label="delete"
                                                                size="small"
                                                                disabled={uploading}
                                                                onClick={() => this.handleRemoveAttachFile(file.attachmentId)}
                                                            >
                                                                <Clear />
                                                            </IconButton>
                                                        </Tooltip>
                                                    }

                                                </div>
                                            ))}
                                        </div>
                                    </Grid>
                                </Grid>
                            </Grid>
                            <Grid item xs={12} >
                                <Divider />
                            </Grid>
                        </>
                    }
                </Grid>
            </div>
        )
    }

    blockContactInfo() {
        const { t, classes } = this.props;
        const { workOrder } = this.state;
        return (
            <div className={classes.block}>
                <Typography component="h2" variant="h6" className={classes.subHeader} >
                    <Contacts color="primary" /> &nbsp;
                                                {t('Contactgegevens')}
                </Typography>
                <Grid container spacing={1}>
                    {
                        workOrder && workOrder.contactInfo &&
                        <>
                            {
                                workOrder.contactInfo.buyer &&
                                <>
                                    {
                                        workOrder.contactInfo.buyer.type === 0 &&
                                        <>
                                            <Grid item xs={12}>
                                                <ContactCard hidePointOfContactCheckbox={true} object={workOrder.contactInfo.buyer.p1} pointOfContactType={0} selectedPointOfContactType={workOrder.pointOfContact} subTitle="Koper 1" />
                                            </Grid>
                                            {
                                                workOrder.contactInfo.buyer.p2 &&
                                                <Grid item xs={12}>
                                                    <ContactCard hidePointOfContactCheckbox={true} object={workOrder.contactInfo.buyer.p2} pointOfContactType={0} selectedPointOfContactType={workOrder.pointOfContact} subTitle="Koper 2" />
                                                </Grid>
                                            }
                                        </>
                                    }
                                    {
                                        workOrder.contactInfo.buyer.type === 1 &&
                                        <Grid item xs={12}>
                                            <ContactCard hidePointOfContactCheckbox={true} object={workOrder.contactInfo.buyer.org} isOrg pointOfContactType={0} selectedPointOfContactType={workOrder.pointOfContact} subTitle="Koper organisatie" />
                                        </Grid>
                                    }
                                </>
                            }
                            {
                                workOrder.contactInfo.client &&
                                <Grid item xs={12}>
                                    <ContactCard hidePointOfContactCheckbox={true} object={workOrder.contactInfo.client} isOrg pointOfContactType={1} selectedPointOfContactType={workOrder.pointOfContact} subTitle="Opdrachtgever" />
                                </Grid>
                            }
                            {
                                workOrder.contactInfo.vvE &&
                                <Grid item xs={12}>
                                    <ContactCard hidePointOfContactCheckbox={true} object={workOrder.contactInfo.vvE} isOrg pointOfContactType={2} selectedPointOfContactType={workOrder.pointOfContact} subTitle="VvE" />
                                </Grid>
                            }
                            {
                                workOrder.contactInfo.vvEAdministrator &&
                                <Grid item xs={12}>
                                    <ContactCard hidePointOfContactCheckbox={true} object={workOrder.contactInfo.vvEAdministrator} isOrg pointOfContactType={3} selectedPointOfContactType={workOrder.pointOfContact} subTitle="VvE beheerder" />
                                </Grid>
                            }
                            {
                                workOrder.contactInfo.propertyManager &&
                                <Grid item xs={12}>
                                    <ContactCard hidePointOfContactCheckbox={true} object={workOrder.contactInfo.propertyManager} isOrg pointOfContactType={4} selectedPointOfContactType={workOrder.pointOfContact} subTitle="Vastgoedbeheerder" />
                                </Grid>
                            }
                            {
                                workOrder.contactInfo.employee &&
                                <Grid item xs={12}>
                                    <ContactCard hidePointOfContactCheckbox={true} object={workOrder.contactInfo.employee} isOrg pointOfContactType={5} selectedPointOfContactType={workOrder.pointOfContact} subTitle="Medewerker" />
                                </Grid>
                            }
                        </>
                    }
                </Grid>
            </div>
        )
    }

    render() {
        const { t, classes, selected, width } = this.props;
        const { workOrder, attachmentIndex, openAlert, copiedMessage, shareAnchorEl, resolverAttachmentIndex, isTemporary, isNotify } = this.state;
        const isLargeScreen = isWidthUp('lg', width);

        return (
            <Container maxWidth={false} className={classes.mainContainer}>
                <Grid container>
                    <Grid item container xs={12} className={classes.container}>
                        <AppBar position="sticky">
                            <Toolbar variant="dense">
                                <IconButton
                                    color="inherit"
                                    edge="start"
                                    aria-label="go back"
                                    component="span"
                                    onClick={() => history.push("/werk/" + selected.projectNo + "/werkbonnen")}
                                >
                                    <ArrowBack />
                                </IconButton>
                                <Typography className={clsx(classes.bold, classes.grow)} noWrap>{t('Werkbon ') + (workOrder ? (workOrder.workOrderNumber ? workOrder.workOrderNumber : workOrder.repairRequestNo + '-##') : '')}</Typography>
                                {
                                    workOrder && !!workOrder.workOrderNumber &&
                                    (
                                        <>
                                            <Tooltip title={t('Delen')}>
                                                <IconButton
                                                    color="inherit"
                                                    aria-label="Share"
                                                    component="span"
                                                    onClick={(e) => {
                                                        this.handleShare(e)
                                                    }}
                                                >
                                                    <Share />
                                                </IconButton>
                                            </Tooltip>
                                            <Menu
                                                id="share-menu"
                                                anchorEl={shareAnchorEl}
                                                keepMounted
                                                open={Boolean(shareAnchorEl)}
                                                onClose={() => {
                                                    this.setState({ shareAnchorEl: null });
                                                }}
                                            >
                                                {
                                                    workOrder.completed !== true &&
                                                    <MenuItem onClick={() => {
                                                        let Unsubscribelink = `${window.location.origin}/werkbon/${this.props.match.params.resolverId}`;
                                                        navigator.clipboard.writeText(Unsubscribelink);
                                                        this.setState({
                                                            openAlert: true,
                                                            shareAnchorEl: null,
                                                            copiedMessage: t("Afmeldlink gekopieerd")
                                                        })
                                                    }}>
                                                        <FileCopyOutlined />
                                                &nbsp;
                                                <span>{t("afmeldlink")}</span>
                                                    </MenuItem>
                                                }
                                                <MenuItem onClick={() => {
                                                    let Workorderlink = window.location.href;
                                                    navigator.clipboard.writeText(Workorderlink);
                                                    this.setState({
                                                        openAlert: true,
                                                        shareAnchorEl: null,
                                                        copiedMessage: t("Werkbonlink gekopieerd")
                                                    })

                                                }}>
                                                    <FileCopyOutlined />
                                                &nbsp;
                                                <span>{t("werkbonlink")}</span>
                                                </MenuItem>
                                            </Menu>
                                            <Snackbar open={openAlert}
                                                autoHideDuration={6000}
                                                onClose={() => this.setState({
                                                    openAlert: false,
                                                    copiedMessage: ""
                                                })}
                                                anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
                                            >
                                                <Alert onClose={() => this.setState({
                                                    openAlert: false,
                                                    copiedMessage: ""
                                                })} severity="success">
                                                    {copiedMessage}
                                                </Alert>
                                            </Snackbar>
                                        </>
                                    )
                                }
                                {
                                    !!workOrder && isTemporary !== true && workOrder.completed !== true &&
                                    <>
                                        {
                                            <Button
                                                aria-describedby="handleWorkOrderPopup"
                                                variant="outlined"
                                                color="inherit"
                                                style={{ marginLeft: 12 }}
                                                onClick={e => this.initiateHandle(e, 'complete')}
                                            >
                                                {t('workorder.handle.complete')}
                                            </Button>
                                        }
                                        {
                                            <Button
                                                aria-describedby="handleWorkOrderPopup"
                                                variant="outlined"
                                                color="inherit"
                                                style={{ marginLeft: 12 }}
                                                onClick={e => this.initiateHandle(e, 'reject')}
                                            >
                                                {t('workorder.handle.reject')}
                                            </Button>
                                        }
                                        {
                                            !!this.state.anchorElHandleWorkOrder && this.state.handleType &&
                                            <Popover open={true}
                                                transformOrigin={{
                                                    vertical: 'top',
                                                    horizontal: 'right',
                                                }}
                                                id={'handleWorkOrderPopup'}
                                                anchorEl={this.state.anchorElHandleWorkOrder}
                                                onClose={() => { this.setState({ anchorElHandleWorkOrder: null }) }}
                                            >
                                                <div style={{ padding: '16px' }}>
                                                    <Grid container spacing={1} direction="column">
                                                        <Grid item>
                                                            <Typography variant="h6">{t('workorder.handle.' + this.state.handleType)}</Typography>
                                                        </Grid>
                                                        <Grid item>
                                                            <FormControlLabel
                                                                control={
                                                                    <Checkbox
                                                                        checked={isNotify === true}
                                                                        onChange={e => this.setState({ isNotify: (e.target.checked === true) })}
                                                                        name="notify"
                                                                        color="primary"
                                                                    />
                                                                }
                                                                label={t('Nu uitvoerder inlichten')}
                                                            />
                                                        </Grid>
                                                        <Grid item>
                                                            <Grid container spacing={1} justify="flex-end">
                                                                <Grid item>
                                                                    <Button
                                                                        variant="outlined"
                                                                        onClick={() => { this.setState({ anchorElHandleWorkOrder: null }) }}
                                                                    >
                                                                        {t('Annuleer')}
                                                                    </Button>
                                                                </Grid>
                                                                <Grid item>
                                                                    <Button
                                                                        variant="outlined"
                                                                        disabled={workOrder.completed === true}
                                                                        onClick={() => this.handleWorkOrder()}
                                                                    >
                                                                        {t('workorder.handle.' + this.state.handleType)}
                                                                    </Button>
                                                                </Grid>
                                                            </Grid>
                                                        </Grid>
                                                    </Grid>
                                                </div>
                                            </Popover>
                                        }
                                    </>
                                }
                            </Toolbar>
                        </AppBar>
                        {
                            workOrder &&
                            <Grid item xs={12}>
                                <Grid container spacing={2} className={classes.innerContainer} alignItems="flex-start">
                                    <Grid item xs={12} md={6} lg={4}>
                                        {this.blockGeneral()}
                                    </Grid>
                                    {
                                        isLargeScreen &&
                                        <Grid item xs={4}>
                                            <Grid container spacing={2}>
                                                <Grid item xs={12}>
                                                    {this.blockWorkOrder()}
                                                </Grid>
                                            </Grid>
                                        </Grid>
                                    }

                                    <Grid item xs={12} md={6} lg={4}>
                                        <Grid container spacing={2}>
                                            <Grid item xs={12}>
                                                {this.blockStatus()}
                                            </Grid>
                                            <Grid item xs={12}>
                                                {this.blockObjectInfo()}
                                            </Grid>
                                            <Grid item xs={12}>
                                                {this.blockContactInfo()}
                                            </Grid>
                                        </Grid>
                                    </Grid>
                                    {
                                        !isLargeScreen &&
                                        <>
                                            <Grid item xs={12} md={6}>
                                                {this.blockWorkOrder()}
                                            </Grid>
                                        </>
                                    }
                                </Grid>

                            </Grid>
                        }
                    </Grid>
                </Grid>
                {
                    workOrder && workOrder.repairRequestImages && attachmentIndex != null && (attachmentIndex >= 0 && attachmentIndex < workOrder.repairRequestImages.length) &&
                    <Modal
                        aria-labelledby="transition-modal-title"
                        aria-describedby="transition-modal-description"
                        className={classes.modal}
                        open={attachmentIndex != null}
                        onClose={this.handleImageModalClose}
                        closeAfterTransition
                        BackdropComponent={Backdrop}
                        BackdropProps={{
                            timeout: 500,
                        }}
                    >
                        <Fade in={true}>
                            <Card style={{ position: 'relative' }}>
                                <IconButton style={{ position: 'absolute', right: '0' }} onClick={this.handleImageModalClose}><Close /></IconButton>
                                <CardMedia
                                    component="img"
                                    alt={(attachmentIndex + 1)}
                                    title={(attachmentIndex + 1)}
                                    image={webApiUrl + 'api/home/getattachment/' + encodeURI(workOrder.repairRequestImages[attachmentIndex] ? workOrder.repairRequestImages[attachmentIndex].attachmentId : '')}
                                    style={{ maxHeight: '100vh', maxWidth: '100%' }}
                                />
                            </Card>
                        </Fade>
                    </Modal>
                }
                {
                    workOrder && workOrder.solutionImages && resolverAttachmentIndex != null && (resolverAttachmentIndex >= 0 && resolverAttachmentIndex < workOrder.solutionImages.length) &&
                    <Modal
                        aria-labelledby="transition-modal1-title"
                        aria-describedby="transition-modal1-description"
                        className={classes.modal}
                        open={resolverAttachmentIndex != null}
                        onClose={this.handleImageModal2Close}
                        closeAfterTransition
                        BackdropComponent={Backdrop}
                        BackdropProps={{
                            timeout: 500,
                        }}
                    >
                        <Fade in={true}>
                            <Card style={{ position: 'relative' }}>
                                <IconButton style={{ position: 'absolute', right: '0' }} onClick={this.handleImageModal2Close}><Close /></IconButton>
                                <CardMedia
                                    component="img"
                                    alt={(resolverAttachmentIndex + 1)}
                                    title={(resolverAttachmentIndex + 1)}
                                    image={webApiUrl + 'api/home/getattachment/' + encodeURI(workOrder.solutionImages[resolverAttachmentIndex] ? workOrder.solutionImages[resolverAttachmentIndex].attachmentId : '')}
                                    style={{ maxHeight: '100vh', maxWidth: '100%' }}
                                />
                            </Card>
                        </Fade>
                    </Modal>
                }
            </Container>
        );
    }
}

function mapStateToProps(state) {
    const { authentication, buildings } = state;
    const { user } = authentication;
    const { selected, all } = buildings;
    const allBuildings = all;
    return {
        user,
        selected,
        allBuildings
    };
}

const connectedPage = connect(mapStateToProps)(withWidth()(withTranslation()(withStyles(styles)(Page))));
export { connectedPage as ResolverWorkOrderDetailsPage };