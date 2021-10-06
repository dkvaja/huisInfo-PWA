import React from "react";
import { connect } from "react-redux";
import {
    Container,
    Grid,
    Typography,
    AppBar,
    Toolbar,
    IconButton,
    CircularProgress,
    FormControl,
    InputLabel,
    Input,
    InputAdornment,
    MenuList,
    Select,
    MenuItem,
    Modal,
    Fade,
    Card,
    Backdrop,
    CardMedia,
    Collapse,
    Button,
    Icon,
    TextField,
    FormControlLabel,
    Checkbox,
    isWidthUp,
    withWidth,
    Divider,
    Popover,
    Tooltip,
    FormLabel,
    FormGroup,
    FormHelperText,
    RadioGroup,
    Radio,
    Menu,
    Snackbar,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle
} from "@material-ui/core";
import { Alert, AlertTitle } from '@material-ui/lab';
import { withStyles, withTheme } from '@material-ui/core/styles';
import { ArrowBack, AttachFile, Close, Clear, Build, Home, FileCopyOutlined, Mail, Share, Assignment, Schedule, Edit, Delete, Contacts, FilterList, Search } from "@material-ui/icons"
import { withTranslation } from 'react-i18next';
import clsx from "clsx";
import NumberFormat from "react-number-format";
import { history, nl2br, formatDate, authHeader, validateFile, getNameInitials, getDataTableTextLabels, formatTime } from '../../_helpers';
import { useState } from "react";
import MUIDataTable from "mui-datatables";
import { Link } from "react-router-dom";
import { DatePicker, MuiPickersUtilsProvider } from "@material-ui/pickers";
import Dropzone from "../../components/Dropzone";
import Autocomplete from '@material-ui/lab/Autocomplete';
import DateFnsUtils from "@date-io/date-fns";
import nlLocale from "date-fns/locale/nl";
import ContactCard from "./RepairRequestContactCard"
import SelectOrganisationPopover from "./SelectOrganisationPopover";

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
        copiedMessage: "",
        customEmails: [],
        customEmailsValid: true
    };

    componentDidMount() {
        this.GetRepairRequestCarryOutAsTypeList();
        this.GetWorkOrderDetails();
        this.GetProductServices();
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
                    history.push('/werk/' + this.props.selected.projectNo + '/kwaliteitsborging');
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

    GetRepairRequestCarryOutAsTypeList() {
        const url = webApiUrl + 'api/RepairRequest/GetRepairRequestCarryOutAsTypeList';
        const requestOptions = {
            method: 'GET',
            headers: authHeader()
        };

        this.setState({
            types: null,
        });

        fetch(url, requestOptions)
            .then(Response => Response.json())
            .then(findResponse => {
                this.setState({ carryOutAsTypeList: findResponse });
            });
    }

    GetResolverRelations() {
        const { workOrder } = this.state;
        if (workOrder) {
            const url = webApiUrl + 'api/Organisation/GetRelationsbyOrganisationId/' + encodeURI(workOrder.organisatonId);

            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({ resolverRelations: findResponse });
                });
        }
    }

    GetEmailsForWorkOrderResolver() {
        const { t } = this.props;
        const { workOrder, isTemporary } = this.state;
        if (workOrder) {
            const url = webApiUrl + 'api/RepairRequest/GetEmailsForWorkOrderResolver/' + encodeURI(workOrder.resolverId);

            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(emails => {
                    let toEmail = null;
                    let ccEmails = [];
                    if (isTemporary === true) {
                        if (emails.resolverEmails && emails.resolverEmails.length > 0) {
                            toEmail = 'resolver';
                        }
                    }
                    else if (workOrder.isHandled !== true && emails.reporterEmails && emails.reporterEmails.length > 0) {
                        toEmail = 'reporter';
                    }

                    const emailModel = [
                        { key: 'reporter', title: t('Melder'), emails: emails.reporterEmails },
                        { key: 'buyer', title: t('Objectgebruiker'), emails: emails.buyerEmails },
                        { key: 'client', title: t('Oprachtgever'), emails: emails.clientEmails },
                        { key: 'vve', title: t('VvE'), emails: emails.vvEEmails },
                        { key: 'vveadmin', title: t('VvE beheerder'), emails: emails.vvEAdministratorEmails },
                        { key: 'propertymanager', title: t('Vastgoedbeheerder'), emails: emails.propertyManagerEmails },
                        { key: 'resolver', title: t('Oplosser'), emails: emails.resolverEmails },
                    ];
                    this.setState({ emails, emailModel, toEmail, ccEmails });
                });
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
                        let reworkOrganizationName = workOrder.name || null;
                        let reworkOrganizationId = workOrder.organisatonId || null;
                        workOrder.completed = workOrder.status === 3 || workOrder.status === 4;
                        this.setState({
                            reworkOrganizationName,
                            reworkOrganizationId,
                            workOrder,
                            isTemporary: (workOrder.status == 0 && !workOrder.workOrderNumber),
                            completeOrRejectionText: workOrder.status === 3 ? workOrder.RejectionText : (workOrder.status === 4 ? workOrder.solutionText : this.state.completeOrRejectionText)
                        });
                        this.GetResolverRelations();
                        if (workOrder.isHandled !== true) {
                            this.GetEmailsForWorkOrderResolver()
                        }
                    }
                });
        }
    }

    updateWorkOrder(key, value) {
        const { t } = this.props;
        const { workOrder } = this.state;

        if (workOrder && workOrder.completed !== true) {
            this.setState({ updating: key })

            const url = webApiUrl + `api/RepairRequest/UpdateWorkOrder/` + encodeURI(workOrder.resolverId);
            const requestOptions = {
                method: 'PATCH',
                headers: authHeader('application/json'),
                body: JSON.stringify([{
                    id: key,
                    name: value.toString()
                }])
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(success => {
                    if (success) {
                        this.setState({ updating: null, edit: null });
                        this.GetWorkOrderDetails(true);
                    }
                    else {
                        this.setState({ updating: null });
                        alert(t('general.api.error'));
                    }
                })
                .catch(e => {
                    this.setState({ updating: null });
                    alert(t('general.api.error'));
                })
        }
    }

    updateWorkOrderRelation(relationId) {
        const { t } = this.props;
        const { workOrder } = this.state;

        if (workOrder && workOrder.completed !== true) {
            this.setState({ updating: 'relation' })

            const url = webApiUrl + `api/RepairRequest/UpdateRepairRequestResolverRelation/` + encodeURI(workOrder.resolverId) + '?relationId=' + relationId;
            const requestOptions = {
                method: 'PATCH',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(success => {
                    if (success) {
                        this.setState({ updating: null, edit: null });
                        this.GetWorkOrderDetails(true);
                    }
                    else {
                        this.setState({ updating: null });
                        alert(t('general.api.error'));
                    }
                })
                .catch(e => {
                    this.setState({ updating: null });
                    alert(t('general.api.error'));
                })
        }
    }

    deleteResolver = () => {
        const { t, selected } = this.props;
        const { workOrder, isTemporary } = this.state;

        if (workOrder && isTemporary === true) {
            this.setState({ deleting: true })

            const url = webApiUrl + `api/RepairRequest/DeleteRepairRequestResolver/` + encodeURI(workOrder.resolverId);
            const requestOptions = {
                method: 'DELETE',
                headers: authHeader('application/json')
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(success => {
                    if (success) {
                        this.setState({ deleting: false });
                        history.push("/werk/" + selected.projectNo + "/kwaliteitsborging/" + workOrder.repairRequestId)
                    }
                    else {
                        this.setState({ deleting: false });
                        alert(t('general.api.error'));
                    }
                })
                .catch(e => {
                    this.setState({ deleting: false });
                    alert(t('general.api.error'));
                })
        }
    }

    initiateWorkOrder = e => {
        const { workOrder, isTemporary } = this.state;
        if (workOrder && isTemporary === true) {
            if (workOrder.explanation && workOrder.explanation.trim() !== '') {
                this.setState({ anchorElCreateWorkOrder: e.currentTarget, customEmails: [], customEmailsValid: true });
            }
            else {
                this.setState({ showCreateWorkOrderErrors: true });
            }
        }
    }

    createWorkOrder = () => {
        const { t, selected } = this.props;
        const { workOrder, isTemporary } = this.state;
        if (workOrder && isTemporary === true) {
            if (workOrder.explanation && workOrder.explanation.trim() !== '') {
                this.setState({ creating: true })

                let model = this.createModelForEmailNotify();

                const url = webApiUrl + `api/RepairRequest/CreateWorkOrder/` + encodeURI(workOrder.resolverId);
                const requestOptions = {
                    method: 'POST',
                    headers: authHeader('application/json'),
                    body: JSON.stringify(model)
                };

                fetch(url, requestOptions)
                    .then(Response => Response.json())
                    .then(success => {
                        if (success) {
                            this.setState({ creating: false });
                            history.push("/werk/" + selected.projectNo + "/kwaliteitsborging/" + workOrder.repairRequestId)
                        }
                        else {
                            this.setState({ creating: false });
                            alert(t('general.api.error'));
                        }
                    })
                    .catch(e => {
                        this.setState({ creating: false });
                        alert(t('general.api.error'));
                    })
            }
            else {
                this.setState({ showCreateWorkOrderErrors: true });
            }
        }
    }

    initiateHandle = (e, handleType) => {
        if (['complete', 'reject', 'rework'].includes(handleType)) {
            const { workOrder, isTemporary, completeOrRejectionText } = this.state;
            if (workOrder && isTemporary !== true && workOrder.isHandled !== true) {
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
        const { workOrder, reworkOrganizationId, isTemporary, handleType, completeOrRejectionText } = this.state;
        if (workOrder && isTemporary !== true && workOrder.isHandled !== true) {
            if (completeOrRejectionText && completeOrRejectionText.trim() !== '' && ['complete', 'reject', 'rework'].includes(handleType)) {
                this.setState({ handling: true })

                let notification = this.createModelForEmailNotify();
                let isComplete = false;
                let isCompleteRepairRequest = false;
                switch (handleType) {
                    case 'complete':
                        isComplete = true;
                        isCompleteRepairRequest = workOrder.isOnlyOrAllOthersWorkOrderCompleted === true && this.state.isCompleteRepairRequest === true
                        break;
                    case 'reject': isComplete = false; break;
                    case 'rework': isComplete = (workOrder.status !== 3); break;
                    default: break;
                }

                const model = {
                    resolverId: workOrder.resolverId,
                    completeOrRejectionText,
                    isComplete,
                    continuedWorkOrder: (handleType === 'rework'),
                    organisationId: reworkOrganizationId,
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
                            if (handleType === 'rework') {
                                history.push("/werk/" + selected.projectNo + "/werkbon/" + success.reworkResolverId)
                            }
                            else {
                                history.push("/werk/" + selected.projectNo + "/kwaliteitsborging/" + workOrder.repairRequestId)
                            }
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
        const { isNotify, toEmail, ccEmails, emails, customEmails } = this.state;
        const model = {
            isNotify: isNotify === true,
            toEmails: {
                reporterEmails: toEmail === 'reporter' ? emails.reporterEmails : null,
                buyerEmails: toEmail === 'buyer' ? emails.buyerEmails : null,
                clientEmails: toEmail === 'client' ? emails.clientEmails : null,
                vvEEmails: toEmail === 'vve' ? emails.vvEEmails : null,
                vvEAdministratorEmails: toEmail === 'vveadmin' ? emails.vvEAdministratorEmails : null,
                propertyManagerEmails: toEmail === 'propertymanager' ? emails.propertyManagerEmails : null,
                resolverEmails: toEmail === 'resolver' ? emails.resolverEmails : null,
            },
            cCEmails: {
                reporterEmails: ccEmails.includes('reporter') ? emails.reporterEmails : null,
                buyerEmails: ccEmails.includes('buyer') ? emails.buyerEmails : null,
                clientEmails: ccEmails.includes('client') ? emails.clientEmails : null,
                vvEEmails: ccEmails.includes('vve') ? emails.vvEEmails : null,
                vvEAdministratorEmails: ccEmails.includes('vveadmin') ? emails.vvEAdministratorEmails : null,
                propertyManagerEmails: ccEmails.includes('propertymanager') ? emails.propertyManagerEmails : null,
                resolverEmails: ccEmails.includes('resolver') ? emails.resolverEmails : null,
                customEmails: customEmails
            }
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

            if (workOrder && workOrder.isHandled !== true) {
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
        if (workOrder && workOrder.isHandled !== true) {
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


    renderEditTextbox(title, key, value, multi = false) {
        const { classes, t } = this.props;
        const { workOrder, updating } = this.state;
        return (
            <div style={{ position: 'relative' }}>
                {value && nl2br(value)}
                {
                    workOrder.completed !== true &&
                    <>
                        {
                            updating === key ?
                                <Icon color="inherit" fontSize="small" style={{ position: 'absolute', right: -15, top: 0 }}>
                                    <CircularProgress size="small" />
                                </Icon>
                                :
                                <>
                                    <Tooltip title={t('Tekst bewerken')}>
                                        <IconButton
                                            aria-describedby={'edit-' + key}
                                            color="inherit"
                                            aria-label="edit"
                                            component="span"
                                            size="small"
                                            edge="end"
                                            style={{ position: 'absolute', right: -15, top: -5 }}
                                            onClick={e => this.setState({ edit: { key, value, anchorEl: e.currentTarget } })}
                                        >
                                            <Edit />
                                        </IconButton>
                                    </Tooltip>
                                    {
                                        this.state.edit && this.state.edit.key === key &&
                                        <Popover open={true}
                                            transformOrigin={{
                                                vertical: 'top',
                                                horizontal: 'right',
                                            }}
                                            id={'edit-' + key}
                                            anchorEl={this.state.edit.anchorEl}
                                            onClose={() => { this.setState({ edit: null }) }}
                                        >
                                            <div style={{ padding: '16px' }}>
                                                <Grid container spacing={1} direction="column">
                                                    <Grid item>
                                                        <Typography variant="h6">{title}</Typography>
                                                    </Grid>
                                                    <Grid item>
                                                        <TextField
                                                            className={classes.textField}
                                                            value={this.state.edit.value}
                                                            onChange={e => this.setState({ edit: { anchorEl: this.state.edit.anchorEl, key, value: e.target.value } })}
                                                            margin="dense"
                                                            variant="outlined"
                                                            multiline={multi}
                                                            rows={10}
                                                            fullWidth
                                                            autoFocus
                                                            disabled={workOrder.completed}
                                                        />
                                                    </Grid>
                                                    <Grid item>
                                                        <Grid container spacing={1} justify="flex-end">
                                                            <Grid item>
                                                                <Button
                                                                    variant="outlined"
                                                                    onClick={() => { this.setState({ edit: null }) }}
                                                                >
                                                                    {t('Annuleer')}
                                                                </Button>
                                                            </Grid>
                                                            <Grid item>
                                                                <Button
                                                                    variant="outlined"
                                                                    onClick={() => this.updateWorkOrder(key, this.state.edit.value)}
                                                                >
                                                                    {t('Opslaan')}
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
                    </>
                }
            </div>
        )
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
                {(workOrder && (workOrder.workOrderNumber ? workOrder.workOrderNumber : workOrder.repairRequestNo + '.##'))}
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
                    {
                        isTemporary !== true && workOrder.relationName &&
                        <>
                            <Grid item xs={12}>
                                <Grid container className={classes.infoGridRow}>
                                    <Grid item xs={6}>{t('Relatie') + ':'}</Grid>
                                    <Grid item xs={6}>{workOrder.relationName}</Grid>
                                </Grid>
                            </Grid>
                            <Grid item xs={12} >
                                <Divider />
                            </Grid>
                        </>
                    }
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
                    {
                        isTemporary !== true &&
                        <>
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
                        </>
                    }
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
        const { workOrder, isTemporary, carryOutAsTypeList, resolverRelations, uploading, showCreateWorkOrderErrors } = this.state;
        const completionRejectionLabel = (workOrder.isHandled === true && workOrder.status == 3 ? t('Reden afwijzing') : (workOrder.isHandled === true && workOrder.status == 4 ? t('Oplossing') : t('Uitgevoerde werkzaamheden')));

        return (
            <div className={classes.block}>
                <Typography component="h2" variant="h6" className={classes.subHeader} >
                    <Assignment color="primary" /> &nbsp;
                                                {t('Aanvullende informatie')}
                </Typography>
                <Grid container>
                    {
                        isTemporary === true &&
                        <>
                            <Grid item xs={12}>
                                <Grid container className={classes.infoGridRow}>
                                    <Grid item xs={6}>{t('Soort') + ':'}</Grid>
                                    <Grid item xs={6}>
                                        <FormControl
                                            variant="outlined"
                                            margin="dense"
                                            fullWidth
                                            disabled={workOrder.completed}
                                        >
                                            <InputLabel id="carry-out-as-type-select-label">{t('Soort')}</InputLabel>
                                            <Select
                                                labelId="carry-out-as-type-select-label"
                                                id="carry-out-as-type-select"
                                                value={workOrder.carryOutAsTypeId}
                                                onChange={e => this.updateWorkOrder('carryoutastypeid', e.target.value)}
                                                label={t('Soort')}
                                            >
                                                {
                                                    !carryOutAsTypeList || carryOutAsTypeList.filter(x => x.id === workOrder.carryOutAsTypeId).length === 0 &&
                                                    <MenuItem value="">&nbsp;</MenuItem>
                                                }
                                                {
                                                    carryOutAsTypeList && carryOutAsTypeList.map((t, index) => (
                                                        <MenuItem key={index} value={t.id}>{t.name}</MenuItem>
                                                    ))
                                                }
                                            </Select>
                                        </FormControl>
                                    </Grid>
                                </Grid>
                            </Grid>
                            <Grid item xs={12} >
                                <Divider />
                            </Grid>
                            <Grid item xs={12}>
                                <MuiPickersUtilsProvider utils={DateFnsUtils} locale={nlLocale}>
                                    <Grid container className={classes.infoGridRow}>
                                        <Grid item xs={6}>{t('Streefdatum afhandeling') + ':'}</Grid>
                                        <Grid item xs={6}>
                                            <DatePicker
                                                variant="inline"
                                                margin="dense"
                                                id="date-time-picker"
                                                label={t('Streefdatum afhandeling')}
                                                format="dd-MM-yyyy"
                                                minDate={new Date()}
                                                helperText={""}
                                                value={workOrder.targetCompletionDate}
                                                onChange={(date) => this.updateWorkOrder('targetcompletiondate', date.toJSON())}
                                                inputVariant="outlined"
                                                autoOk
                                                ampm={false}
                                                fullWidth
                                                required
                                            />
                                        </Grid>
                                    </Grid>
                                </MuiPickersUtilsProvider>
                            </Grid>
                            <Grid item xs={12} >
                                <Divider />
                            </Grid>
                            <Grid item xs={12}>
                                <Grid container className={classes.infoGridRow}>
                                    <Grid item xs={6}>{t('Relatie') + ':'}</Grid>
                                    <Grid item xs={6}>
                                        <FormControl
                                            variant="outlined"
                                            margin="dense"
                                            fullWidth
                                            disabled={workOrder.completed}
                                        >
                                            <InputLabel id="relation-select-label">{t('Relatie')}</InputLabel>
                                            <Select
                                                labelId="relation-select-label"
                                                id="relation-select"
                                                value={workOrder.relationId}
                                                onChange={e => this.updateWorkOrderRelation(e.target.value)}
                                                label={t('Relatie')}
                                            >
                                                {
                                                    !resolverRelations || resolverRelations.filter(x => x.id === workOrder.relationId).length === 0 &&
                                                    <MenuItem value="">&nbsp;</MenuItem>
                                                }
                                                {
                                                    resolverRelations && resolverRelations.map((t, index) => (
                                                        <MenuItem key={index} value={t.id}>{t.name}</MenuItem>
                                                    ))
                                                }
                                            </Select>
                                        </FormControl>
                                    </Grid>
                                </Grid>
                            </Grid>
                            <Grid item xs={12} >
                                <Divider />
                            </Grid>
                            <Grid item xs={12}>
                                <Grid container className={classes.infoGridRow}>
                                    <Grid item xs={6}>
                                        <Typography variant="body2" color={workOrder && showCreateWorkOrderErrors === true && (!workOrder.explanation || workOrder.explanation.trim() === '') ? "error" : "inherit"}>
                                            {t('Toelichting') + '*:'}
                                        </Typography>
                                    </Grid>
                                    <Grid item xs={6}>
                                        {this.renderEditTextbox(t('Toelichting'), 'explanation', workOrder.explanation, true)}
                                    </Grid>
                                </Grid>
                            </Grid>
                            <Grid item xs={12} >
                                <Divider />
                            </Grid>
                        </>
                    }
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
                                            disabled={workOrder.isHandled === true}
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
                                                workOrder.isHandled !== true &&
                                                <div className={classes.dropzoneContainer}>
                                                    <Dropzone onFilesAdded={this.uploadAttachment} disabled={workOrder.isHandled === true} uploading={uploading} accept="image/*" />
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
                                                        workOrder.isHandled !== true &&
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
                            <Grid item xs={12}>
                                <ContactCard hidePointOfContactCheckbox object={workOrder} isOrg pointOfContactType={-1} subTitle={'Oplosser - Werkbon ' + (!workOrder.workOrderNumber ? '(Concept)' : workOrder.workOrderNumber)} />
                            </Grid>
                        </>
                    }
                </Grid>
            </div>
        )
    }

    updateCustomEmails = value => {
        let { customEmails } = this.state;
        var customEmailsValid = true;
        customEmails = value.trim() !== '' ? value.replace(/\s/g, '').split(",") : [];
        var regex = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

        for (var i = 0; i < customEmails.length; i++) {
            if (customEmails[i] == "" || !regex.test(customEmails[i])) {
                customEmailsValid = false;
                break;
            }
        }
        this.setState({
            customEmails,
            customEmailsValid
        });
    }

    SendReminderToResolver = () => {
        const { t } = this.props;
        const { workOrder, customEmails, customEmailsValid } = this.state;

        if (customEmailsValid === true && this.canSendReminderToWorkorder() === true) {
            this.setState({ sendingReminder: true })
            let sendReminderModel = {
                resolverIdList: [workOrder.resolverId],
                ccEmails: customEmails
            }
            const url = webApiUrl + `api/RepairRequest/SendReminder/`;
            const requestOptions = {
                method: 'POST',
                headers: authHeader('application/json'),
                body: JSON.stringify(sendReminderModel)
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(success => {
                    if (success) {
                        this.setState({ sendingReminder: false, customEmails: [], customEmailsValid: true, sendReminderAnchorEl: null });
                        this.GetWorkOrderDetails(true);
                    }
                    else {
                        this.setState({ sendingReminder: false });
                        alert(t('general.api.error'));
                    }
                })
                .catch(e => {
                    this.setState({ sendingReminder: false });
                    alert(t('general.api.error'));
                })
        }
    }

    canSendReminderToWorkorder() {
        if (this.state.workOrder != null) {
            if (this.state.workOrder.status === 3 || this.state.workOrder.status === 4 || (this.state.workOrder.status === 0 && !this.state.workOrder.workOrderNumber)) {
                return false;
            } else {
                return true;
            }
        }
        return false;
    }

    renderProjectSelectPopup() {
        const { workOrder, productServices, searchResolversResult,selectProjectLoading } = this.state;
        const { t, classes,selected } = this.props;
        return (
            <Popover
                open={!selectProjectLoading && true}
                anchorOrigin={{
                    vertical: "top",
                    horizontal: "right",
                }}
                transformOrigin={{
                    vertical: "top",
                    horizontal: "right",
                }}
                id={"add-workorder"}
                anchorEl={this.state.selectProjectAnchorEl}
                onClose={() => {
                    this.setState({ selectProjectAnchorEl: null });
                }}
            >
                <div style={{ padding: "16px" }}>
                    <Grid container spacing={1} direction="column">
                        {/* <Grid item>
                            <Grid container direction="row">
                                <Grid item className={classes.grow}>
                                    {this.state.selectProjectSearchTerm == null ? (
                                        <Typography variant="h6">
                                            {this.state.selectProjectMethod >= 0 && t("Oplosser")}
                                        </Typography>
                                    ) : (
                                            <Input
                                                onFocus={(event) => event.stopPropagation()}
                                                //className={classes.inputBox}
                                                autoFocus
                                                type="search"
                                                value={this.state.selectProjectSearchTerm}
                                                onChange={(e) =>
                                                    this.setState({
                                                        selectProjectSearchTerm: e.target.value,
                                                    })
                                                }
                                                endAdornment={
                                                    this.state.selectProjectSearchTerm !== "" && (
                                                        <InputAdornment position="end">
                                                            <IconButton
                                                                color="inherit"
                                                                size="small"
                                                                onClick={() =>
                                                                    this.setState({
                                                                        selectProjectSearchTerm: null,
                                                                    })
                                                                }
                                                            >
                                                                <Close />
                                                            </IconButton>
                                                        </InputAdornment>
                                                    )
                                                }
                                            />
                                        )}
                                </Grid>
                                <Grid item>
                                    <IconButton
                                        color="inherit"
                                        aria-label="search"
                                        component="span"
                                        style={{ marginTop: -8, marginBottom: -8 }}
                                        onClick={(e) => {
                                            this.setState({
                                                selectProjectSearchTerm:
                                                    this.state.selectProjectSearchTerm == null
                                                        ? ""
                                                        : null,
                                            });
                                        }}
                                    >
                                        <Search />
                                    </IconButton>
                                </Grid>
                                <Grid item>
                                    <IconButton
                                        aria-describedby={"filter-list"}
                                        color="inherit"
                                        aria-label="filter-resolvers"
                                        component="span"
                                        style={{ marginTop: -8, marginBottom: -8 }}
                                        onClick={(e) => {
                                            this.setState({ editFilterAnchorEl: e.currentTarget });
                                        }}
                                    >
                                        <FilterList />
                                    </IconButton>
                                    {!!this.state.editFilterAnchorEl && (
                                        <Popover
                                            open={true}
                                            anchorOrigin={{
                                                vertical: "top",
                                                horizontal: "right",
                                            }}
                                            transformOrigin={{
                                                vertical: "top",
                                                horizontal: "right",
                                            }}
                                            id={"filter-list"}
                                            anchorEl={this.state.editFilterAnchorEl}
                                            onClose={() => {
                                                this.setState({ editFilterAnchorEl: null });
                                            }}
                                        >
                                            <div style={{ padding: "16px" }}>
                                                <Grid container spacing={1}>
                                                    <Grid item xs={12}>
                                                        <Grid container>
                                                            <Grid item className={classes.grow}>
                                                                {t("datatable.label.filter.title")}
                                                            </Grid>
                                                            <Grid item>
                                                                <IconButton
                                                                    color="inherit"
                                                                    component="span"
                                                                    edge="end"
                                                                    style={{ marginTop: -8, marginBottom: -8 }}
                                                                    onClick={() => {
                                                                        this.setState({
                                                                            editFilterAnchorEl: null,
                                                                        });
                                                                    }}
                                                                >
                                                                    <Close />
                                                                </IconButton>
                                                            </Grid>
                                                        </Grid>
                                                    </Grid>
                                                    <Grid item xs={12} sm={6}>
                                                        <FormControl
                                                            variant="outlined"
                                                            margin="dense"
                                                            fullWidth
                                                            disabled={workOrder.isHandled}
                                                        //style={{ marginTop: -10, marginBottom: -10 }}
                                                        >
                                                            <InputLabel id="change-method-select-label">
                                                                {t("Methode")}
                                                            </InputLabel>
                                                            <Select
                                                                labelId="change-method-select-label"
                                                                id="change-method-select"
                                                                value={this.state.selectProjectMethod}
                                                                onChange={(e) =>
                                                                    this.setState({
                                                                        selectProjectMethod: e.target.value,
                                                                    })
                                                                }
                                                                label={t("Methode")}
                                                            >
                                                                {[0, 1, 2].map((item, index) => (
                                                                    <MenuItem key={index} value={item}>
                                                                        {t(
                                                                            "workorder.organisation.search.method." +
                                                                            item
                                                                        )}
                                                                    </MenuItem>
                                                                ))}
                                                            </Select>
                                                        </FormControl>
                                                    </Grid>
                                                    <Grid item xs={12} sm={6} className='test'>
                                                        <Autocomplete
                                                            id="product-service-method-select"
                                                            fullWidth
                                                            disabled={workOrder.isHandled}
                                                            options={productServices && productServices}
                                                            value={this.state.selectProductServiceId}
                                                            onChange={(e) =>
                                                                this.setState({
                                                                    selectProductServiceId: e.target.value,
                                                                })
                                                            }
                                                            getOptionLabel={(option) => option.code + ' - ' + option.description}
                                                            renderInput={(params) => <TextField  {...params} label={t('Product/Dienst')} variant="outlined" margin="dense" />}
                                                        />
                                                    </Grid>
                                                </Grid>
                                            </div>
                                        </Popover>
                                    )}
                                </Grid>
                                <Grid item>
                                    <IconButton
                                        color="inherit"
                                        component="span"
                                        edge="end"
                                        style={{ marginTop: -8, marginBottom: -8 }}
                                        onClick={() => {
                                            this.setState({ selectProjectAnchorEl: null });
                                        }}
                                    >
                                        <Close />
                                    </IconButton>
                                </Grid>
                            </Grid>
                        </Grid> */}
                        <Grid item>
                            <SelectOrganisationPopover
                                projectId={selected && selected.projectId}
                                productServices={productServices}
                                // disabled={repairRequest.completed}
                                setLoading={(val)=>this.setState({selectProjectLoading:val})}
                                setSelectOrgAnchorEl={v => this.setState({ selectProjectAnchorEl: v })}
                                onSelect={item => {
                                    this.setState(
                                        {reworkOrganizationName: item.name,reworkOrganizationId: item.id},
                                                    () => {this.setState({ selectProjectAnchorEl: null})}
                                                );
                                }}
                                />
                            {/* <MenuList style={{ margin: "0 -16px -12px" }}>
                                {!!searchResolversResult &&
                                    searchResolversResult.map((item, index) => (
                                        <MenuItem
                                            key={index}
                                            onClick={(event) => {
                                                event.preventDefault();
                                                this.setState(
                                                    {
                                                        reworkOrganizationName: item.name,
                                                        reworkOrganizationId: item.id

                                                    },
                                                    () => {
                                                        this.setState(
                                                            { selectProjectAnchorEl: null, }
                                                        )

                                                    }
                                                );
                                            }}
                                        >
                                            {item.name}
                                        </MenuItem>
                                    ))}
                            </MenuList> */}
                        </Grid>
                    </Grid>
                </div>
            </Popover>
        );
    }

    GetProductServices() {
        const url = webApiUrl + "api/Home/GetProductServices";
        const requestOptions = {
            method: "GET",
            headers: authHeader(),
        };

        this.setState({
            productServices: null,
        });

        fetch(url, requestOptions)
            .then((Response) => Response.json())
            .then((findResponse) => {
                console.log(findResponse)
                this.setState({ productServices: findResponse });
            });
    }

    handleShare(e) {
        this.setState({
            shareAnchorEl: e.currentTarget
        })
    }

    copyWorkOrder(organisationId) {
        const { t, selected } = this.props;
        const { workOrder } = this.state;
        if (workOrder && workOrder.repairRequestCompleted !== true) {
            const url = webApiUrl + `api/RepairRequest/CopyWorkOrder/${organisationId}/${workOrder.resolverId}`;
            const requestOptions = {
                method: 'POST',
                headers: authHeader('application/json')
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(res => {
                    if (res.success === true && !!res.resolverId) {
                        history.push("/werk/" + selected.projectNo + "/werkbon/" + res.resolverId);
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


    render() {
        const { t, classes, selected, width } = this.props;
        const { workOrder, productServices, attachmentIndex, openAlert, copiedMessage, reworkOrganizationName, shareAnchorEl, resolverAttachmentIndex, isTemporary, isNotify, emails, emailModel, toEmail, ccEmails } = this.state;
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
                                    onClick={() =>
                                        !!workOrder
                                            ? history.push(
                                                "/werk/" +
                                                selected.projectNo +
                                                "/kwaliteitsborging/" +
                                                workOrder.repairRequestId
                                            )
                                            : history.goBack()
                                    }
                                >
                                    <ArrowBack />
                                </IconButton>
                                <Typography
                                    className={clsx(classes.bold, classes.grow)}
                                    noWrap
                                >
                                    {t("Werkbon ") +
                                        (workOrder
                                            ? workOrder.workOrderNumber
                                                ? workOrder.workOrderNumber
                                                : workOrder.repairRequestNo + ".##"
                                            : "")}
                                </Typography>
                                {
                                    workOrder && workOrder.repairRequestCompleted !== true &&
                                    <>
                                        <Tooltip title={t('Kopieer werkbon')}>
                                            <IconButton
                                                color="inherit"
                                                aria-label="CopyWorkOrder"
                                                component="span"
                                                onClick={(e) => {
                                                    this.setState({
                                                        copyWorkOrderAnchorEl: e.currentTarget
                                                    })

                                                }}>
                                                <FileCopyOutlined />
                                            </IconButton>
                                        </Tooltip>
                                        {
                                            <Popover
                                                open={!!this.state.copyWorkOrderAnchorEl}
                                                onClose={() => {
                                                    this.setState({
                                                        copyWorkOrderAnchorEl: null
                                                    })
                                                }}
                                                anchorOrigin={{
                                                    vertical: 'center',
                                                    horizontal: 'center',
                                                }}
                                                transformOrigin={{
                                                    vertical: 'top',
                                                    horizontal: 'center',
                                                }}
                                                anchorEl={this.state.copyWorkOrderAnchorEl}
                                                onClose={() => this.setState({ copyWorkOrderAnchorEl: null })}
                                            >
                                                <SelectOrganisationPopover
                                                    projectId={selected && selected.projectId}
                                                    productServices={productServices}
                                                    disabled={workOrder.repairRequestCompleted}
                                                    selectOrgAnchorEl={this.state.copyWorkOrderAnchorEl}
                                                    setSelectOrgAnchorEl={v => this.setState({ copyWorkOrderAnchorEl: v })}
                                                    onSelect={item => this.copyWorkOrder(item.id)} />
                                            </Popover>
                                        }
                                    </>
                                }
                                {workOrder && !!workOrder.workOrderNumber && (
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
                                                <span>{t("Afmeldlink")}</span>
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
                                                <span>{t("Werkbonlink")}</span>
                                            </MenuItem>
                                            {
                                                this.canSendReminderToWorkorder() &&
                                                <MenuItem onClick={(e) => {
                                                    this.setState({ shareAnchorEl: null, sendReminderAnchorEl: e.currentTarget })
                                                }}>
                                                    <Mail />
                                                    &nbsp;
                                                    <span>{t("Herinnering versturen")}</span>
                                                </MenuItem>
                                            }
                                        </Menu>
                                        {
                                            !!this.state.sendReminderAnchorEl &&
                                            <Dialog open={true}
                                                onClose={() => this.setState({
                                                    customEmails: [], customEmailsValid: true, sendReminderAnchorEl: null
                                                })}
                                                aria-labelledby="form-dialog-title">
                                                <DialogTitle id="send_reminder">{t('Herinnering versturen')}</DialogTitle>
                                                <DialogContent>
                                                    <TextField
                                                        autoFocus
                                                        disabled={this.state.sendingReminder === true}
                                                        error={this.state.customEmailsValid !== true}
                                                        rows={2}
                                                        variant="outlined"
                                                        onChange={(e) => this.updateCustomEmails(e.target.value)}
                                                        margin="dense"
                                                        id="ccemail"
                                                        label="CC"
                                                        type="email"
                                                        fullWidth
                                                    />
                                                </DialogContent>
                                                <DialogActions>
                                                    <Button
                                                        disabled={this.state.sendingReminder === true}
                                                        onClick={() => {
                                                            this.setState({
                                                                customEmails: [], customEmailsValid: true, sendReminderAnchorEl: null
                                                            })
                                                        }}
                                                        color="primary"
                                                    >
                                                        {t('Annuleer')}
                                                    </Button>
                                                    <Button
                                                        disabled={this.state.customEmailsValid !== true || this.state.sendingReminder === true}
                                                        onClick={() => { this.SendReminderToResolver() }}
                                                        color="primary"
                                                    >
                                                        {t('Herinnering versturen')}
                                                    </Button>
                                                </DialogActions>
                                            </Dialog>
                                        }
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
                                )}
                                {!!workOrder && isTemporary === true && (
                                    <>
                                        <Tooltip
                                            title={t("datatable.label.selectedrows.delete")}
                                        >
                                            <IconButton
                                                color="inherit"
                                                aria-label="Delete"
                                                component="span"
                                                onClick={this.deleteResolver}
                                            >
                                                <Delete />
                                            </IconButton>
                                        </Tooltip>
                                        <Button
                                            aria-describedby="saveDraft"
                                            variant="outlined"
                                            color="inherit"
                                            style={{ marginLeft: 12 }}
                                            onClick={() =>
                                                !!workOrder
                                                    ? history.push(
                                                        "/werk/" +
                                                        selected.projectNo +
                                                        "/kwaliteitsborging/" +
                                                        workOrder.repairRequestId
                                                    )
                                                    : history.goBack()
                                            }
                                        >
                                            {t("Concept opslaan")}
                                        </Button>
                                        <Button
                                            aria-describedby="createWorkOrderPopup"
                                            variant="outlined"
                                            color="inherit"
                                            style={{ marginLeft: 12 }}
                                            onClick={this.initiateWorkOrder}
                                        >
                                            {t("Maak werkbon")}
                                        </Button>
                                        {!!this.state.anchorElCreateWorkOrder && (
                                            <Popover
                                                open={true}
                                                transformOrigin={{
                                                    vertical: "top",
                                                    horizontal: "right",
                                                }}
                                                id={"createWorkOrderPopup"}
                                                anchorEl={this.state.anchorElCreateWorkOrder}
                                                onClose={() => {
                                                    this.setState({ anchorElCreateWorkOrder: null });
                                                }}
                                            >
                                                <div style={{ padding: "16px" }}>
                                                    <Grid container spacing={1} direction="column">
                                                        <Grid item>
                                                            <Typography variant="h6">
                                                                {t("Maak werkbon")}
                                                            </Typography>
                                                        </Grid>
                                                        <Grid item>
                                                            <FormControlLabel
                                                                control={
                                                                    <Checkbox
                                                                        checked={isNotify === true}
                                                                        onChange={(e) =>
                                                                            this.setState({
                                                                                isNotify: e.target.checked === true,
                                                                            })
                                                                        }
                                                                        name="notify"
                                                                        color="primary"
                                                                    />
                                                                }
                                                                label={t("Oplosser nu inlichten")}
                                                            />
                                                        </Grid>
                                                        <Collapse
                                                            in={isNotify}
                                                            timeout="auto"
                                                            unmountOnExit
                                                        >
                                                            <Grid item>
                                                                <FormControl
                                                                    component="fieldset"
                                                                    className={classes.formControlCheckList}
                                                                    required
                                                                    error={toEmail !== "resolver"}
                                                                >
                                                                    <FormLabel component="legend">{t("Aan")}:</FormLabel>
                                                                    {toEmail !== "resolver" && (
                                                                        <FormHelperText>
                                                                            {t("Geen emailadres bekend")}
                                                                        </FormHelperText>
                                                                    )}
                                                                    <FormGroup>
                                                                        {toEmail === "resolver" && (
                                                                            <FormControlLabel
                                                                                control={
                                                                                    <Checkbox
                                                                                        checked={true}
                                                                                        name="to-resolver"
                                                                                    />
                                                                                }
                                                                                label={
                                                                                    <>
                                                                                        {t("Oplosser")}
                                                                                        <br />
                                                                                        <Typography variant="caption">
                                                                                            {emails &&
                                                                                                emails.resolverEmails &&
                                                                                                emails.resolverEmails.join(
                                                                                                    "; "
                                                                                                )}
                                                                                        </Typography>
                                                                                    </>
                                                                                }
                                                                            />
                                                                        )}
                                                                    </FormGroup>
                                                                </FormControl>
                                                            </Grid>
                                                            <Grid item>
                                                                <FormControl
                                                                    component="fieldset"
                                                                    className={classes.formControlCheckList}
                                                                >
                                                                    <FormLabel component="legend">
                                                                        {t("CC")}
                                                                    </FormLabel>
                                                                    <FormGroup>
                                                                        {emailModel &&
                                                                            emailModel
                                                                                .filter(
                                                                                    (x) =>
                                                                                        x.key !== toEmail &&
                                                                                        x.emails &&
                                                                                        x.emails.length > 0
                                                                                )
                                                                                .map((item, index) => (
                                                                                    <FormControlLabel
                                                                                        key={index}
                                                                                        control={
                                                                                            <Checkbox
                                                                                                checked={ccEmails.includes(
                                                                                                    item.key
                                                                                                )}
                                                                                                color="primary"
                                                                                                onChange={(e) => {
                                                                                                    if (
                                                                                                        e.target.checked ===
                                                                                                        true
                                                                                                    ) {
                                                                                                        let ccEmailsToUpdate = ccEmails.slice();
                                                                                                        ccEmailsToUpdate.push(
                                                                                                            item.key
                                                                                                        );
                                                                                                        this.setState({
                                                                                                            ccEmails: ccEmailsToUpdate,
                                                                                                        });
                                                                                                    } else {
                                                                                                        this.setState({
                                                                                                            ccEmails: ccEmails.filter(
                                                                                                                (x) =>
                                                                                                                    x !== item.key
                                                                                                            ),
                                                                                                        });
                                                                                                    }
                                                                                                }}
                                                                                                name={"cc-" + item.key}
                                                                                            />
                                                                                        }
                                                                                        label={
                                                                                            <>
                                                                                                {item.title}
                                                                                                {ccEmails.includes(
                                                                                                    item.key
                                                                                                ) && (
                                                                                                        <>
                                                                                                            {item.emails &&
                                                                                                                item.emails.map(
                                                                                                                    (email, index1) => (
                                                                                                                        <React.Fragment
                                                                                                                            key={index1}
                                                                                                                        >
                                                                                                                            <br />
                                                                                                                            <Typography variant="caption">
                                                                                                                                {email}
                                                                                                                            </Typography>
                                                                                                                        </React.Fragment>
                                                                                                                    )
                                                                                                                )}
                                                                                                        </>
                                                                                                    )}
                                                                                            </>
                                                                                        }
                                                                                    />
                                                                                ))}
                                                                        <TextField
                                                                            autoFocus
                                                                            error={this.state.customEmailsValid !== true}
                                                                            rows={2}
                                                                            variant="outlined"
                                                                            onChange={(e) => this.updateCustomEmails(e.target.value)}
                                                                            margin="dense"
                                                                            id="ccemail"
                                                                            label="Extra CC"
                                                                            type="email"
                                                                            fullWidth
                                                                        />
                                                                    </FormGroup>
                                                                </FormControl>
                                                            </Grid>
                                                        </Collapse>
                                                        <Grid item>
                                                            <Grid
                                                                container
                                                                spacing={1}
                                                                justify="flex-end"
                                                            >
                                                                <Grid item>
                                                                    <Button
                                                                        variant="outlined"
                                                                        onClick={() => {
                                                                            this.setState({
                                                                                anchorElCreateWorkOrder: null,
                                                                            });
                                                                        }}
                                                                    >
                                                                        {t("Annuleer")}
                                                                    </Button>
                                                                </Grid>
                                                                <Grid item>
                                                                    <Button
                                                                        variant="outlined"
                                                                        disabled={
                                                                            (isNotify === true && toEmail !== "resolver")
                                                                            || this.state.customEmailsValid !== true
                                                                        }
                                                                        onClick={() => this.createWorkOrder()}
                                                                    >
                                                                        {t("Maak werkbon")}
                                                                    </Button>
                                                                </Grid>
                                                            </Grid>
                                                        </Grid>
                                                    </Grid>
                                                </div>
                                            </Popover>
                                        )}
                                    </>
                                )}
                                {!!workOrder &&
                                    isTemporary !== true &&
                                    workOrder.isHandled !== true && (
                                        <>
                                            {
                                                //workOrder.status !== 4 &&//completed=4
                                                <Button
                                                    aria-describedby="handleWorkOrderPopup"
                                                    variant="outlined"
                                                    color="inherit"
                                                    style={{ marginLeft: 12 }}
                                                    onClick={(e) => this.initiateHandle(e, "reject")}
                                                >
                                                    {t("workorder.handle.reject")}
                                                </Button>
                                            }
                                            {
                                                <Button
                                                    aria-describedby="handleWorkOrderPopup"
                                                    variant="outlined"
                                                    color="inherit"
                                                    style={{ marginLeft: 12 }}
                                                    onClick={(e) => this.initiateHandle(e, "rework")}
                                                >
                                                    {t("workorder.handle.rework")}
                                                </Button>
                                            }
                                            {
                                                //workOrder.status !== 3 &&//rejected=3
                                                <Button
                                                    aria-describedby="handleWorkOrderPopup"
                                                    variant="outlined"
                                                    color="inherit"
                                                    style={{ marginLeft: 12 }}
                                                    onClick={(e) =>
                                                        this.initiateHandle(e, "complete")
                                                    }
                                                >
                                                    {t("workorder.handle.complete")}
                                                </Button>
                                            }
                                            {!!this.state.anchorElHandleWorkOrder &&
                                                this.state.handleType && (
                                                    <Popover
                                                        open={true}
                                                        transformOrigin={{
                                                            vertical: "top",
                                                            horizontal: "right",
                                                        }}
                                                        id={"handleWorkOrderPopup"}
                                                        anchorEl={this.state.anchorElHandleWorkOrder}
                                                        onClose={() => {
                                                            this.setState({
                                                                anchorElHandleWorkOrder: null,
                                                            });
                                                        }}
                                                    >
                                                        <div style={{ padding: "16px" }}>
                                                            <Grid
                                                                container
                                                                spacing={1}
                                                                direction="column"
                                                            >
                                                                <Grid item>
                                                                    <Typography variant="h6">
                                                                        {t(
                                                                            "workorder.handle." +
                                                                            this.state.handleType
                                                                        )}
                                                                    </Typography>
                                                                    {this.state.handleType == "rework" && (
                                                                        <Typography noWrap variant="caption">
                                                                            {reworkOrganizationName}
                                                                            <IconButton
                                                                                color="inherit"
                                                                                aria-label="edit"
                                                                                component="span"
                                                                                size="small"
                                                                                edge="end"
                                                                                onClick={(e) => {
                                                                                    const selectProjectMethod = !this
                                                                                        .state.selectProjectMethod
                                                                                        ? 0
                                                                                        : this.state
                                                                                            .selectProjectMethod;
                                                                                    const selectProjectSearchTerm = !this
                                                                                        .state.selectProjectSearchTerm
                                                                                        ? null
                                                                                        : this.state
                                                                                            .selectProjectSearchTerm;
                                                                                    this.setState({
                                                                                        selectProjectAnchorEl:
                                                                                            e.currentTarget,
                                                                                        selectProjectMethod,
                                                                                        selectProjectSearchTerm,
                                                                                    });
                                                                                    // if (!workOrder.productServiceId || !workOrder.carryOutAsTypeId)
                                                                                    //     this.setState({ highlightErrors: true });
                                                                                    // else {
                                                                                    //     const selectProjectMethod = !this.state.selectProjectMethod ? 0 : this.state.selectProjectMethod;
                                                                                    //     const selectProjectSearchTerm = !this.state.selectProjectSearchTerm ? null : this.state.selectProjectSearchTerm;
                                                                                    //     this.setState({ selectProjectAnchorEl: e.currentTarget, selectProjectMethod, selectProjectSearchTerm, anchorElHandleWorkOrder:null,handleType:null })
                                                                                    // }
                                                                                }}
                                                                            >
                                                                                {this.state.selectProjectLoading?<CircularProgress size={20}/>:<Edit fontSize="small" />}
                                                                            </IconButton>
                                                                        </Typography>
                                                                    )}
                                                                    {!!this.state.selectProjectAnchorEl && this.renderProjectSelectPopup()}
                                                                </Grid>
                                                                <Grid item>
                                                                    <FormControlLabel
                                                                        control={
                                                                            <Checkbox
                                                                                checked={isNotify === true}
                                                                                onChange={(e) =>
                                                                                    this.setState({
                                                                                        isNotify:
                                                                                            e.target.checked === true,
                                                                                    })
                                                                                }
                                                                                name="notify"
                                                                                color="primary"
                                                                            />
                                                                        }
                                                                        label={t("Nu inlichten")}
                                                                    />
                                                                </Grid>
                                                                <Collapse
                                                                    in={isNotify}
                                                                    timeout="auto"
                                                                    unmountOnExit
                                                                >
                                                                    <Grid item>
                                                                        <FormControl
                                                                            component="fieldset"
                                                                            className={
                                                                                classes.formControlCheckList
                                                                            }
                                                                            required
                                                                            error={
                                                                                !toEmail || toEmail.trim() === ""
                                                                            }
                                                                        >
                                                                            <FormLabel component="legend">
                                                                                {t("Aan")}
                                                                            </FormLabel>
                                                                            {!toEmail ||
                                                                                (toEmail.trim() === "" && (
                                                                                    <FormHelperText>
                                                                                        {t("Geen emailadres bekend")}
                                                                                    </FormHelperText>
                                                                                ))}
                                                                            <RadioGroup
                                                                                aria-label="toEmail"
                                                                                name="toEmail"
                                                                                value={toEmail}
                                                                                onChange={(e) => {
                                                                                    this.setState({
                                                                                        toEmail: e.target.value,
                                                                                        ccEmails: ccEmails.filter(
                                                                                            (x) => x !== e.target.value
                                                                                        ),
                                                                                    });
                                                                                }}
                                                                            >
                                                                                {emailModel &&
                                                                                    emailModel
                                                                                        .filter(
                                                                                            (x) =>
                                                                                                x.emails &&
                                                                                                x.emails.length > 0
                                                                                        )
                                                                                        .map((item, index) => (
                                                                                            <FormControlLabel
                                                                                                key={index}
                                                                                                value={item.key}
                                                                                                control={<Radio />}
                                                                                                label={
                                                                                                    <>
                                                                                                        {item.title}
                                                                                                        {toEmail ===
                                                                                                            item.key && (
                                                                                                                <>
                                                                                                                    {item.emails &&
                                                                                                                        item.emails.map(
                                                                                                                            (
                                                                                                                                email,
                                                                                                                                index1
                                                                                                                            ) => (
                                                                                                                                    <React.Fragment
                                                                                                                                        key={index1}
                                                                                                                                    >
                                                                                                                                        <br />
                                                                                                                                        <Typography variant="caption">
                                                                                                                                            {email}
                                                                                                                                        </Typography>
                                                                                                                                    </React.Fragment>
                                                                                                                                )
                                                                                                                        )}
                                                                                                                </>
                                                                                                            )}
                                                                                                    </>
                                                                                                }
                                                                                            />
                                                                                        ))}
                                                                            </RadioGroup>
                                                                        </FormControl>
                                                                    </Grid>
                                                                    <Grid item>
                                                                        <FormControl
                                                                            component="fieldset"
                                                                            className={
                                                                                classes.formControlCheckList
                                                                            }
                                                                        >
                                                                            <FormLabel component="legend">
                                                                                {t("CC")}
                                                                            </FormLabel>
                                                                            <FormGroup>
                                                                                {emailModel &&
                                                                                    emailModel
                                                                                        .filter(
                                                                                            (x) =>
                                                                                                x.key !== toEmail &&
                                                                                                x.emails &&
                                                                                                x.emails.length > 0
                                                                                        )
                                                                                        .map((item, index) => (
                                                                                            <FormControlLabel
                                                                                                key={index}
                                                                                                control={
                                                                                                    <Checkbox
                                                                                                        checked={ccEmails.includes(
                                                                                                            item.key
                                                                                                        )}
                                                                                                        color="primary"
                                                                                                        onChange={(e) => {
                                                                                                            if (
                                                                                                                e.target.checked ===
                                                                                                                true
                                                                                                            ) {
                                                                                                                let ccEmailsToUpdate = ccEmails.slice();
                                                                                                                ccEmailsToUpdate.push(
                                                                                                                    item.key
                                                                                                                );
                                                                                                                this.setState({
                                                                                                                    ccEmails: ccEmailsToUpdate,
                                                                                                                });
                                                                                                            } else {
                                                                                                                this.setState({
                                                                                                                    ccEmails: ccEmails.filter(
                                                                                                                        (x) =>
                                                                                                                            x !== item.key
                                                                                                                    ),
                                                                                                                });
                                                                                                            }
                                                                                                        }}
                                                                                                        name={"cc-" + item.key}
                                                                                                    />
                                                                                                }
                                                                                                label={
                                                                                                    <>
                                                                                                        {item.title}
                                                                                                        {ccEmails.includes(
                                                                                                            item.key
                                                                                                        ) && (
                                                                                                                <>
                                                                                                                    {item.emails &&
                                                                                                                        item.emails.map(
                                                                                                                            (
                                                                                                                                email,
                                                                                                                                index1
                                                                                                                            ) => (
                                                                                                                                    <React.Fragment
                                                                                                                                        key={index1}
                                                                                                                                    >
                                                                                                                                        <br />
                                                                                                                                        <Typography variant="caption">
                                                                                                                                            {email}
                                                                                                                                        </Typography>
                                                                                                                                    </React.Fragment>
                                                                                                                                )
                                                                                                                        )}
                                                                                                                </>
                                                                                                            )}
                                                                                                    </>
                                                                                                }
                                                                                            />
                                                                                        ))}
                                                                            </FormGroup>

                                                                        </FormControl>
                                                                    </Grid>
                                                                </Collapse>
                                                                {
                                                                    this.state.handleType === "complete" && workOrder.isOnlyOrAllOthersWorkOrderCompleted === true &&
                                                                    (
                                                                        <Grid item>
                                                                            <FormControlLabel
                                                                                control={
                                                                                    <Checkbox
                                                                                        checked={
                                                                                            this.state
                                                                                                .isCompleteRepairRequest ===
                                                                                            true
                                                                                        }
                                                                                        onChange={(e) =>
                                                                                            this.setState({
                                                                                                isCompleteRepairRequest:
                                                                                                    e.target.checked === true,
                                                                                            })
                                                                                        }
                                                                                        name="notify"
                                                                                        color="primary"
                                                                                    />
                                                                                }
                                                                                label={t(
                                                                                    "workorder.handle.completerepairrequest"
                                                                                )}
                                                                            />
                                                                        </Grid>
                                                                    )
                                                                }
                                                                <Grid item>
                                                                    <Grid
                                                                        container
                                                                        spacing={1}
                                                                        justify="flex-end"
                                                                    >
                                                                        <Grid item>
                                                                            <Button
                                                                                variant="outlined"
                                                                                onClick={() => {
                                                                                    this.setState({
                                                                                        anchorElHandleWorkOrder: null,
                                                                                    });
                                                                                }}
                                                                            >
                                                                                {t("Annuleer")}
                                                                            </Button>
                                                                        </Grid>
                                                                        <Grid item>
                                                                            <Button
                                                                                variant="outlined"
                                                                                disabled={
                                                                                    isNotify === true &&
                                                                                    (!toEmail ||
                                                                                        toEmail.trim() === "")
                                                                                }
                                                                                onClick={() =>
                                                                                    this.handleWorkOrder()
                                                                                }
                                                                            >
                                                                                {t(
                                                                                    "workorder.handle." +
                                                                                    this.state.handleType
                                                                                )}
                                                                            </Button>
                                                                        </Grid>
                                                                    </Grid>
                                                                </Grid>
                                                            </Grid>
                                                        </div>
                                                    </Popover>
                                                )}
                                        </>
                                    )}
                            </Toolbar>
                        </AppBar>
                        {workOrder && (
                            <Grid item xs={12}>
                                <Grid
                                    container
                                    spacing={2}
                                    className={classes.innerContainer}
                                    alignItems="flex-start"
                                >
                                    {
                                        //<Grid item xs={12}>
                                        //    <Typography variant="h6">{t('Algemene informatie')}</Typography>
                                        //</Grid>
                                    }
                                    <Grid item xs={12} md={6} lg={4}>
                                        {this.blockGeneral()}
                                    </Grid>
                                    {isLargeScreen && (
                                        <Grid item xs={4}>
                                            <Grid container spacing={2}>
                                                <Grid item xs={12}>
                                                    {this.blockWorkOrder()}
                                                </Grid>
                                            </Grid>
                                        </Grid>
                                    )}

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
                                        //<Grid item xs={12}>
                                        //    <Typography variant="h6">{t('Extra benodigde informatie')}</Typography>
                                        //</Grid>
                                    }
                                    {!isLargeScreen && (
                                        <>
                                            <Grid item xs={12} md={6}>
                                                {this.blockWorkOrder()}
                                            </Grid>
                                        </>
                                    )}
                                </Grid>
                            </Grid>
                        )}
                    </Grid>
                </Grid>
                {workOrder &&
                    workOrder.repairRequestImages &&
                    attachmentIndex != null &&
                    attachmentIndex >= 0 &&
                    attachmentIndex < workOrder.repairRequestImages.length && (
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
                                <Card style={{ position: "relative" }}>
                                    <IconButton
                                        style={{ position: "absolute", right: "0" }}
                                        onClick={this.handleImageModalClose}
                                    >
                                        <Close />
                                    </IconButton>
                                    <CardMedia
                                        component="img"
                                        alt={attachmentIndex + 1}
                                        title={attachmentIndex + 1}
                                        image={
                                            webApiUrl +
                                            "api/home/getattachment/" +
                                            encodeURI(
                                                workOrder.repairRequestImages[attachmentIndex]
                                                    ? workOrder.repairRequestImages[attachmentIndex]
                                                        .attachmentId
                                                    : ""
                                            )
                                        }
                                        style={{ maxHeight: "100vh", maxWidth: "100%" }}
                                    />
                                </Card>
                            </Fade>
                        </Modal>
                    )}
                {workOrder &&
                    workOrder.solutionImages &&
                    resolverAttachmentIndex != null &&
                    resolverAttachmentIndex >= 0 &&
                    resolverAttachmentIndex < workOrder.solutionImages.length && (
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
                                <Card style={{ position: "relative" }}>
                                    <IconButton
                                        style={{ position: "absolute", right: "0" }}
                                        onClick={this.handleImageModal2Close}
                                    >
                                        <Close />
                                    </IconButton>
                                    <CardMedia
                                        component="img"
                                        alt={resolverAttachmentIndex + 1}
                                        title={resolverAttachmentIndex + 1}
                                        image={
                                            webApiUrl +
                                            "api/home/getattachment/" +
                                            encodeURI(
                                                workOrder.solutionImages[resolverAttachmentIndex]
                                                    ? workOrder.solutionImages[
                                                        resolverAttachmentIndex
                                                    ].attachmentId
                                                    : ""
                                            )
                                        }
                                        style={{ maxHeight: "100vh", maxWidth: "100%" }}
                                    />
                                </Card>
                            </Fade>
                        </Modal>
                    )}
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
export { connectedPage as WorkOrderDetailsPage };
