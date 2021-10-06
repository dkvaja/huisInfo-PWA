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
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle
} from "@material-ui/core";
import { withStyles } from '@material-ui/core/styles';
import {   ArrowBack, ExpandMore, Clear, Close, Build, Home, Contacts, Assignment,  Schedule, ListAlt, ExpandLess, Edit, Save, Add, Delete, Mail, PriorityHigh, FileCopyOutlined } from "@material-ui/icons"
import Autocomplete from '@material-ui/lab/Autocomplete';
import { withTranslation } from 'react-i18next';
import clsx from "clsx";
import Dropzone from "../../components/Dropzone";
import { history, nl2br, formatDate, authHeader, validateFile, getDataTableTextLabels } from '../../_helpers';
import MUIDataTable from "mui-datatables";
import { Link } from "react-router-dom";
import ContactCard from "./RepairRequestContactCard";
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
    infoGrid: {
        //padding: theme.spacing(0.5, 2),

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
    warning: {
        color: theme.palette.warning.main
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
        position: 'relative',
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
    mainLoaderContainer: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
        display: 'flex',
        padding: theme.spacing(1.25),
        height: '50vh'
    }
});


class Page extends React.Component {
    state = {
        attachmentIndex: null,
        repairRequest: null,
        building: null,
        sendReminderCCEmailList: [],
        sendReminderCCEmailValid: true,
        workorderPopoverOpenLoader: false,
    };

    componentDidMount() {
        this.GetRepairRequestLocations();
        this.GetRepairRequestCarryOutAsTypeList();
        this.GetProductServices();
        this.GetSettlementTerm();
        this.GetRepairRequestTypeList();
        this.GetRepairRequestNatureList();
        this.GetRepairRequestCauseList();
        this.GetRepairRequestCauserList();
        this.GetOrganisations();
        this.GetRepairRequestDetails();
    }

    componentDidUpdate(prevProps, prevState) {
        if (!prevProps.selected || prevProps.selected.buildingId !== this.props.selected.buildingId) {
            this.GetRepairRequestDetails();
        }
        if (!prevProps.selected || prevProps.selected.projectId !== this.props.selected.projectId) {
            this.GetOrganisations();
        }
        if (
            (!prevState.repairRequest && !!this.state.repairRequest)
            ||
            (!!prevState.repairRequest && !!this.state.repairRequest && prevState.repairRequest.requestId !== this.state.repairRequest.requestId)
        ) {
            if (this.props.selected && this.props.selected.buildingId.toUpperCase() !== this.state.repairRequest.buildingId.toUpperCase()) {
                var selectedItem = this.props.allBuildings.find(x => x.buildingId.toUpperCase() === this.state.repairRequest.buildingId.toUpperCase());
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

    GetRepairRequestLocations() {
        const url = webApiUrl + 'api/RepairRequest/GetRepairRequestLocations';
        const requestOptions = {
            method: 'GET',
            headers: authHeader()
        };

        this.setState({
            locations: null,
        });

        fetch(url, requestOptions)
            .then(Response => Response.json())
            .then(findResponse => {
                this.setState({ locations: findResponse });
            });
    }

    GetProductServices() {
        const url = webApiUrl + 'api/Home/GetProductServices';
        const requestOptions = {
            method: 'GET',
            headers: authHeader()
        };

        this.setState({
            productServices: null,
        });

        fetch(url, requestOptions)
            .then(Response => Response.json())
            .then(findResponse => {
                this.setState({ productServices: findResponse });
            });
    }

    GetSettlementTerm() {
        const url = webApiUrl + 'api/RepairRequest/GetSettlementTerm';
        const requestOptions = {
            method: 'GET',
            headers: authHeader()
        };

        this.setState({
            terms: null,
        });

        fetch(url, requestOptions)
            .then(Response => Response.json())
            .then(findResponse => {
                this.setState({ terms: findResponse });
            });
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

    GetRepairRequestTypeList() {
        const url = webApiUrl + 'api/RepairRequest/GetRepairRequestTypeList';
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
                this.setState({ typeList: findResponse });
            });
    }

    GetRepairRequestNatureList() {
        const url = webApiUrl + 'api/RepairRequest/GetRepairRequestNatureList';
        const requestOptions = {
            method: 'GET',
            headers: authHeader()
        };

        this.setState({
            natureList: null,
        });

        fetch(url, requestOptions)
            .then(Response => Response.json())
            .then(findResponse => {
                this.setState({ natureList: findResponse });
            });
    }

    GetRepairRequestCauseList() {
        const url = webApiUrl + 'api/RepairRequest/GetRepairRequestCauseList';
        const requestOptions = {
            method: 'GET',
            headers: authHeader()
        };

        this.setState({
            causeList: null,
        });

        fetch(url, requestOptions)
            .then(Response => Response.json())
            .then(findResponse => {
                this.setState({ causeList: findResponse });
            });
    }

    GetRepairRequestCauserList() {
        const url = webApiUrl + 'api/RepairRequest/GetRepairRequestCauserList';
        const requestOptions = {
            method: 'GET',
            headers: authHeader()
        };

        this.setState({
            causerList: null,
        });

        fetch(url, requestOptions)
            .then(Response => Response.json())
            .then(findResponse => {
                this.setState({ causerList: findResponse });
            });
    }

    GetOrganisations() {
        const { selected } = this.props;
        if (selected) {
            const url = webApiUrl + 'api/Organisation/GetOrganisationsByProject/' + encodeURI(selected.projectId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({
                        organisationList: findResponse
                    });
                });
        }
    }

    GetEmailsForRepairRequest() {
        const { t } = this.props;
        const { repairRequest } = this.state;
        if (repairRequest && repairRequest.completed !== true && (repairRequest.isAllWorkOrderCompleted === true || repairRequest.isAllWorkOrderDeclined === true)) {
            const url = webApiUrl + 'api/RepairRequest/GetEmailsForRepairRequest/' + encodeURI(repairRequest.requestId);

            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(emails => {
                    let toEmail = null;
                    let ccEmails = [];
                    if (emails.reporterEmails && emails.reporterEmails.length > 0) {
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

    GetRepairRequestDetails(update = false) {
        const { repairRequestId } = this.props.match.params;
        const { selected } = this.props;
        if (selected && repairRequestId) {
            this.setState({ repairRequestLoading: true });
            const url = webApiUrl + 'api/RepairRequest/GetRepairRequestDetails/' + encodeURI(repairRequestId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            if (update === false) {
                this.setState({
                    repairRequest: null,
                });
            }

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    if (findResponse.number) {
                        this.setState({ repairRequest: findResponse, completionTextToEdit: findResponse.completionText, rejectionTextToEdit: findResponse.rejectionText, repairRequestLoading: false });
                        //this.GetRepairRequestContactInfo();
                        this.GetEmailsForRepairRequest();
                    }
                }).catch(er => {
                    this.setState({ repairRequestLoading: false })
                });
        }
    }

    //GetRepairRequestContactInfo() {
    //    const { repairRequest } = this.state;
    //    if (repairRequest) {
    //        const url = webApiUrl + 'api/RepairRequest/GetRepairRequestContactInfo/' + encodeURI(repairRequest.requestId);
    //        const requestOptions = {
    //            method: 'GET',
    //            headers: authHeader()
    //        };

    //        fetch(url, requestOptions)
    //            .then(Response => Response.json())
    //            .then(findResponse => {
    //                this.setState({
    //                    repairRequestContacts: findResponse
    //                });
    //            });
    //    }
    //}

    updateRepairRequest(key, value) {
        const { t } = this.props;
        const { repairRequest } = this.state;

        if (repairRequest && repairRequest.completed !== true) {
            this.setState({ updating: key })

            const url = webApiUrl + `api/RepairRequest/UpdateRepairRequestByKey/` + encodeURI(repairRequest.requestId);
            const requestOptions = {
                method: 'POST',
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
                        this.GetRepairRequestDetails(true);
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

    updatePointOfContact = value => {
        this.updateRepairRequest("pointofcontact", value);
    }

    updateCCEmails = value => {
        let { sendReminderCCEmailList } = this.state;
        var sendReminderCCEmailValid = true;
        sendReminderCCEmailList = value.trim() !== '' ? value.replace(/\s/g, '').split(",") : [];
        var regex = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

        for (var i = 0; i < sendReminderCCEmailList.length; i++) {
            if (sendReminderCCEmailList[i] == "" || !regex.test(sendReminderCCEmailList[i])) {
                sendReminderCCEmailValid = false;
                break;
            }
        }
        this.setState({
            sendReminderCCEmailList,
            sendReminderCCEmailValid
        });
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
            const { repairRequest } = this.state;
            this.setState({
                uploading: true
            })

            if (repairRequest && repairRequest.completed !== true) {
                const formData = new FormData()

                for (var i = 0; i < filesToUpload.length; i++) {
                    formData.append('files', filesToUpload[i])
                }

                const url = webApiUrl + `api/RepairRequest/AddRepairRequestAttachments/` + encodeURI(repairRequest.requestId);
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
                            this.GetRepairRequestDetails(true);
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

    AddRepairRequestResolver = organisationId => {
        const { t, selected } = this.props;
        const { repairRequest } = this.state;

        if (repairRequest && repairRequest.completed !== true) {
            const url = webApiUrl + `api/RepairRequest/AddRepairRequestResolver/` + encodeURI(repairRequest.requestId);

            const formData = new FormData()
            formData.append('organisationId', organisationId)
            const requestOptions = {
                method: 'POST',
                headers: authHeader(),
                body: formData
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(res => {
                    if (res.success === true && !!res.resolverId) {
                        history.push("/werk/" + selected.projectNo + "/werkbon/" + res.resolverId);
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
    }

    copyWorkOrder(organisationId) {
        const { t, selected } = this.props;
        const { repairRequest, selectedResolverIds } = this.state;
        if (selectedResolverIds && selectedResolverIds.length === 1 && repairRequest && repairRequest.completed !== true) {
            const url = webApiUrl + `api/RepairRequest/CopyWorkOrder/${organisationId}/${selectedResolverIds[0]}`;
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

    DeleteRepairRequestResolvers = () => {
        const { t } = this.props;
        const { selectedResolverIds } = this.state;

        if (selectedResolverIds && selectedResolverIds.length > 0 && this.canDeleteSelectedWorkorders() === true) {
            this.setState({ deletingResolvers: true })

            const url = webApiUrl + `api/RepairRequest/DeleteResolvers/`;
            const requestOptions = {
                method: 'DELETE',
                headers: authHeader('application/json'),
                body: JSON.stringify(selectedResolverIds)
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(success => {
                    if (success) {
                        this.setState({ deletingResolvers: false });
                        this.GetRepairRequestDetails(true);
                    }
                    else {
                        this.setState({ deletingResolvers: false });
                        alert(t('general.api.error'));
                    }
                })
                .catch(e => {
                    this.setState({ deletingResolvers: false });
                    alert(t('general.api.error'));
                })
        }
    }

    SendReminderToResolvers = () => {
        const { t } = this.props;
        const { selectedResolverIds, sendReminderCCEmailList, sendReminderCCEmailValid } = this.state;


        if (selectedResolverIds && selectedResolverIds.length > 0 && sendReminderCCEmailValid === true && this.canSendReminderSelectedWorkorders() === true) {
            this.setState({ sendingReminder: true })
            let sendReminderModel = {
                resolverIdList: selectedResolverIds,
                ccEmails: sendReminderCCEmailList
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
                        this.setState({ sendingReminder: false, sendReminderCCEmailList: [], sendReminderCCEmailValid: true, sendReminderAnchorEl: null });
                        this.GetRepairRequestDetails(true);
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

    canDeleteSelectedWorkorders() {
        if (this.state.repairRequest && this.state.selectedResolverIds) {
            const resolvers = this.state.repairRequest.resolvers.filter(x => this.state.selectedResolverIds.includes(x.resolverId));
            for (var i = 0; i < resolvers.length; i++) {
                const resolver = resolvers[i];

                if (resolver.status !== 0 || (resolver.status === 0 && !!resolver.workOrderNumber)) {
                    return false;
                }
            }
        }
        return true;
    }

    canSendReminderSelectedWorkorders() {
        if (this.state.repairRequest && this.state.selectedResolverIds) {
            const resolvers = this.state.repairRequest.resolvers.filter(x => this.state.selectedResolverIds.includes(x.resolverId));
            for (var i = 0; i < resolvers.length; i++) {
                const resolver = resolvers[i];
                if (resolver.status === 3 || resolver.status === 4 || (resolver.status === 0 && !resolver.workOrderNumber)) {
                    return false;
                }
            }
        }
        return true;
    }

    initiateHandle = (e, handleType) => {
        if (['complete', 'turndown'].includes(handleType)) {
            const { repairRequest } = this.state;
            if (!!repairRequest
                && (
                    (
                        //handleType == 'complete' &&
                        repairRequest.isAllWorkOrderCompleted === true
                    )
                    ||
                    (
                        //handleType == 'turndown' &&
                        repairRequest.isAllWorkOrderDeclined === true
                    )
                )
            ) {
                this.setState({ anchorElHandleRepairRequest: e.currentTarget, handleType });
            }
        }
    }

    handleRepairRequest = () => {
        const { t, selected } = this.props;
        const { repairRequest, handleType, completionTextToEdit, rejectionTextToEdit } = this.state;
        const completeOrRejectionText = handleType === 'complete' ? completionTextToEdit : rejectionTextToEdit;
        if (!!repairRequest
            && ['complete', 'turndown'].includes(handleType)
            && (repairRequest.isAllWorkOrderCompleted === true || repairRequest.isAllWorkOrderDeclined === true)
            && (!!completeOrRejectionText && completeOrRejectionText.trim() !== '')
        ) {
            this.setState({ handling: true })
            let notification = this.createModelForEmailNotify();
            const isComplete = handleType === 'complete';
            let model = {
                isComplete,
                completeOrRejectionText,
                notification
            }

            const url = webApiUrl + `api/RepairRequest/UpdateRepairRequestStatus/` + encodeURI(repairRequest.requestId);
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
                        this.GetRepairRequestDetails(true);
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
    }

    createModelForEmailNotify = () => {
        const { isNotify, toEmail, ccEmails, emails } = this.state;
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
            }
        };

        return model;
    }

    handleImageModalOpen = index => {
        this.setState({ attachmentIndex: index });
    }

    handleImageModalClose = () => {
        this.setState({ attachmentIndex: null });
    }

    isProductServiceRequired() {
        const carryOutAsTypeData = this.state.carryOutAsTypeList.find(p => p.id === this.state.repairRequest.carryOutAsTypeId)
        return !(["Inspectie", "Voorschouw", "Opleverpunt", "Onderhoudstermijn"].some(p => p === carryOutAsTypeData.name));
    }

    renderEditTextbox(title, key, value, multi = false) {
        const { classes, t } = this.props;
        const { repairRequest, updating } = this.state;
        return (
            <div style={{ position: 'relative' }}>
                {value && nl2br(value)}
                {
                    repairRequest.completed !== true &&
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
                                                            disabled={repairRequest.completed}
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
                                                                    onClick={() => this.updateRepairRequest(key, this.state.edit.value)}
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

    handleRemoveAttachFile(attachmentId) {
        const { t } = this.props;
        const { repairRequest } = this.state;
        if (repairRequest && repairRequest.completed !== true) {
            const url = webApiUrl + `api/RepairRequest/DeleteRepairRequestAttachment/${repairRequest.requestId}/${attachmentId}`;
            const requestOptions = {
                method: 'DELETE',
                headers: authHeader('application/json')
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(res => {
                    if (res.success === true) {
                        this.GetRepairRequestDetails(true);
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

    blockGeneralRepairRequest() {
        const { t, classes } = this.props;
        const { repairRequest, uploading, locations, updating } = this.state;
        return (
            <div className={classes.block}>
                <Typography component="h2" variant="h6" className={classes.subHeader} >
                    <Build color="primary" /> &nbsp;
                    {t('Melding')}
                    &nbsp;
                    {(repairRequest ? repairRequest.number : '')}
                </Typography>
                <Grid container>
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={6}>{t('Datum') + ':'}</Grid>
                            <Grid item xs={6}>{repairRequest.date && formatDate(new Date(repairRequest.date))}</Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                    {
                        (isNaN(repairRequest.receivedVia) !== true || repairRequest.receivedVia !== 2) &&
                        <>
                            <Grid item xs={12}>
                                <Grid container className={classes.infoGridRow}>
                                    <Grid item xs={6}>{t('Aangenomen door') + ':'}</Grid>
                                    <Grid item xs={6}>{repairRequest.adoptedBy && repairRequest.adoptedBy}</Grid>
                                </Grid>
                            </Grid>
                            <Grid item xs={12} >
                                <Divider />
                            </Grid>
                        </>
                    }
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={6}>{t('Melder') + ':'}</Grid>
                            <Grid item xs={6}>{repairRequest.reporter && repairRequest.reporter.name}</Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={6}>{t('Melding ontvangen via') + ':'}</Grid>
                            <Grid item xs={6}>{repairRequest.receivedVia !== null && repairRequest.receivedVia >= 0 && repairRequest.receivedVia <= 7 && t('repairrequest.receivedvia.' + repairRequest.receivedVia)}</Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={6}>{t('Omschrijving') + ':'}</Grid>
                            <Grid item xs={6}>
                                {this.renderEditTextbox(t('Omschrijving'), 'desc', repairRequest.desc)}
                            </Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={6}>{t('Oorspronkelijke melding') + ':'}</Grid>
                            <Grid item xs={6}>{nl2br(repairRequest.detailDesc)}</Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                    {
                        !!repairRequest.preferredAppointmentTime && repairRequest.preferredAppointmentTime.trim() !== '' &&
                        <>
                            <Grid item xs={12}>
                                <Grid container className={classes.infoGridRow}>
                                    <Grid item xs={6}>{t('Voorkeurstijdstip afspraak') + ':'}</Grid>
                                    <Grid item xs={6}>{repairRequest.preferredAppointmentTime}</Grid>
                                </Grid>
                            </Grid>
                            <Grid item xs={12} >
                                <Divider />
                            </Grid>
                        </>
                    }
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={6}>{t('Locatie') + ':'}</Grid>
                            <Grid item xs={6}>
                                <FormControl
                                    variant="outlined"
                                    margin="dense"
                                    fullWidth
                                    disabled={repairRequest.completed}
                                //style={{ margin: 0 }}
                                >
                                    <InputLabel id="demo-simple-select-label">{t('Locatie')}</InputLabel>
                                    <Select
                                        labelId="demo-simple-select-label"
                                        id="demo-simple-select"
                                        value={repairRequest.locationId}
                                        onChange={e => this.updateRepairRequest('locationid', e.target.value)}
                                        label={t('Locatie')}
                                    >
                                        {
                                            !locations || locations.filter(x => x.id === repairRequest.locationId).length === 0 &&
                                            <MenuItem value="">&nbsp;</MenuItem>
                                        }
                                        {
                                            locations && locations.map((location, index) => (
                                                <MenuItem key={index} value={location.id}>{location.name}</MenuItem>
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
                            <Grid item xs={12}>{t('Afbeeldingen') + ':'}</Grid>
                        </Grid>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={12}>
                                <div className={classes.imageGallery}>
                                    {
                                        repairRequest.completed !== true &&
                                        <div className={classes.dropzoneContainer}>
                                            <Dropzone onFilesAdded={this.uploadAttachment} disabled={repairRequest.completed === true} uploading={uploading} accept="image/*" />
                                        </div>
                                    }
                                    {repairRequest.attachments && repairRequest.attachments.length > 0 && repairRequest.attachments.map((file, index) => (
                                        <div className={classes.galleryImageTile}>
                                            <div
                                                onClick={() => this.handleImageModalOpen(index)}
                                                style={{
                                                    backgroundImage: 'url(' + webApiUrl + 'api/home/getattachment/' + file.attachmentId + ')'
                                                }}
                                            ></div>
                                            {
                                                repairRequest.completed !== true &&
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
                </Grid>
            </div>
        )
    }

    blockStatus() {
        const { t, classes } = this.props;
        const { repairRequest, terms } = this.state;
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
                            <Grid item xs={6}>{repairRequest.status && repairRequest.status}</Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={6}>{t('Afgehandeld op') + ':'}</Grid>
                            <Grid item xs={6}>{repairRequest.settledOn && formatDate(new Date(repairRequest.settledOn))}</Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                    {
                        repairRequest.completed === true &&
                        <>
                            {
                                !!repairRequest.completionText && repairRequest.completionText.trim() !== '' &&
                                <>
                                    <Grid item xs={12}>
                                        <Grid container className={classes.infoGridRow}>
                                            <Grid item xs={6}>{t('Oplossing') + ':'}</Grid>
                                            <Grid item xs={6}>{nl2br(repairRequest.completionText)}</Grid>
                                        </Grid>
                                    </Grid>
                                    <Grid item xs={12}>
                                        <Divider />
                                    </Grid>
                                </>
                            }
                            {
                                !!repairRequest.rejectionText && repairRequest.rejectionText.trim() !== '' &&
                                <>
                                    <Grid item xs={12}>
                                        <Grid container className={classes.infoGridRow}>
                                            <Grid item xs={6}>{t('Reden afwijzing') + ':'}</Grid>
                                            <Grid item xs={6}>{nl2br(repairRequest.rejectionText)}</Grid>
                                        </Grid>
                                    </Grid>
                                    <Grid item xs={12}>
                                        <Divider />
                                    </Grid>
                                </>
                            }
                        </>
                    }
                </Grid>
            </div>
        )
    }

    blockObjectInfo() {
        const { t, classes } = this.props;
        const { repairRequest, building } = this.state;
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
                                <Grid item xs={6}>{repairRequest.address && (repairRequest.address.street ? repairRequest.address.street : "") + " " + (repairRequest.address.houseNo ? repairRequest.address.houseNo : "")}</Grid>
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
                                        repairRequest.address &&
                                        <>
                                            {repairRequest.address.postcode ? repairRequest.address.postcode + " " : ""}&nbsp;{repairRequest.address.place ? repairRequest.address.place : ""}
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
                                <Grid item xs={6}>{repairRequest.completionDate && formatDate(new Date(repairRequest.completionDate))}</Grid>
                            </Grid>
                        </Grid>
                        <Grid item xs={12} >
                            <Divider />
                        </Grid>
                        <Grid item xs={12}>
                            <Grid container className={classes.infoGridRow}>
                                <Grid item xs={6}>{t('Datum tweede handtekening') + ':'}</Grid>
                                <Grid item xs={6}>{repairRequest.secondSignatureDate && formatDate(new Date(repairRequest.secondSignatureDate))}</Grid>
                            </Grid>
                        </Grid>
                        <Grid item xs={12} >
                            <Divider />
                        </Grid>
                        <Grid item xs={12}>
                            <Grid container className={classes.infoGridRow}>
                                <Grid item xs={6}>{t('Einde onderhoudstermijn') + ':'}</Grid>
                                <Grid item xs={6}>{repairRequest.maintenanceEndDate && formatDate(new Date(repairRequest.maintenanceEndDate))}</Grid>
                            </Grid>
                        </Grid>
                        <Grid item xs={12} >
                            <Divider />
                        </Grid>
                        <Grid item xs={12}>
                            <Grid container className={classes.infoGridRow}>
                                <Grid item xs={6}>{t('Einde garantietermijn') + ':'}</Grid>
                                <Grid item xs={6}>{repairRequest.warrantyEndDate && formatDate(new Date(repairRequest.warrantyEndDate))}</Grid>
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

    blockContactInfo() {
        const { t, classes } = this.props;
        const { repairRequest } = this.state;
        return (
            <div className={classes.block}>
                <Typography component="h2" variant="h6" className={classes.subHeader} >
                    <Contacts color="primary" /> &nbsp;
                    {t('Contactgegevens')}
                </Typography>
                <Grid container spacing={1}>
                    {
                        repairRequest && repairRequest.contactInfo &&
                        <>
                            {
                                repairRequest.contactInfo.buyer &&
                                <>
                                    {
                                        repairRequest.contactInfo.buyer.type === 0 &&
                                        <>
                                            <Grid item xs={12}>
                                                <ContactCard hidePointOfContactCheckbox={repairRequest.completed} updatePointOfContact={this.updatePointOfContact} object={repairRequest.contactInfo.buyer.p1} pointOfContactType={0} selectedPointOfContactType={repairRequest.pointOfContact} subTitle="Koper 1" />
                                            </Grid>
                                            {
                                                repairRequest.contactInfo.buyer.p2 &&
                                                <Grid item xs={12}>
                                                    <ContactCard hidePointOfContactCheckbox={repairRequest.completed} updatePointOfContact={this.updatePointOfContact} object={repairRequest.contactInfo.buyer.p2} pointOfContactType={0} selectedPointOfContactType={repairRequest.pointOfContact} subTitle="Koper 2" />
                                                </Grid>
                                            }
                                        </>
                                    }
                                    {
                                        repairRequest.contactInfo.buyer.type === 1 &&
                                        <Grid item xs={12}>
                                            <ContactCard hidePointOfContactCheckbox={repairRequest.completed} updatePointOfContact={this.updatePointOfContact} object={repairRequest.contactInfo.buyer.org} isOrg pointOfContactType={0} selectedPointOfContactType={repairRequest.pointOfContact} subTitle="Koper organisatie" />
                                        </Grid>
                                    }
                                </>
                            }
                            {
                                repairRequest.contactInfo.client &&
                                <Grid item xs={12}>
                                    <ContactCard hidePointOfContactCheckbox={repairRequest.completed} updatePointOfContact={this.updatePointOfContact} object={repairRequest.contactInfo.client} isOrg pointOfContactType={1} selectedPointOfContactType={repairRequest.pointOfContact} subTitle="Opdrachtgever" />
                                </Grid>
                            }
                            {
                                repairRequest.contactInfo.vvE &&
                                <Grid item xs={12}>
                                    <ContactCard hidePointOfContactCheckbox={repairRequest.completed} updatePointOfContact={this.updatePointOfContact} object={repairRequest.contactInfo.vvE} isOrg pointOfContactType={2} selectedPointOfContactType={repairRequest.pointOfContact} subTitle="VvE" />
                                </Grid>
                            }
                            {
                                repairRequest.contactInfo.vvEAdministrator &&
                                <Grid item xs={12}>
                                    <ContactCard hidePointOfContactCheckbox={repairRequest.completed} updatePointOfContact={this.updatePointOfContact} object={repairRequest.contactInfo.vvEAdministrator} isOrg pointOfContactType={3} selectedPointOfContactType={repairRequest.pointOfContact} subTitle="VvE beheerder" />
                                </Grid>
                            }
                            {
                                repairRequest.contactInfo.propertyManager &&
                                <Grid item xs={12}>
                                    <ContactCard hidePointOfContactCheckbox={repairRequest.completed} updatePointOfContact={this.updatePointOfContact} object={repairRequest.contactInfo.propertyManager} isOrg pointOfContactType={4} selectedPointOfContactType={repairRequest.pointOfContact} subTitle="Vastgoedbeheerder" />
                                </Grid>
                            }
                            {
                                repairRequest.contactInfo.employee &&
                                <Grid item xs={12}>
                                    <ContactCard hidePointOfContactCheckbox={repairRequest.completed} updatePointOfContact={this.updatePointOfContact} object={repairRequest.contactInfo.employee} isOrg pointOfContactType={5} selectedPointOfContactType={repairRequest.pointOfContact} subTitle="Medewerker" />
                                </Grid>
                            }
                            {
                                repairRequest.resolvers && repairRequest.resolvers.map((resolver, index) => (
                                    <Grid key={index} item xs={12}>
                                        <ContactCard hidePointOfContactCheckbox object={resolver} isOrg pointOfContactType={-1} subTitle={'Oplosser - Werkbon ' + (!resolver.workOrderNumber ? '(Concept)' : resolver.workOrderNumber)} />
                                    </Grid>
                                ))
                            }
                        </>
                    }
                </Grid>
            </div>
        )
    }

    blockRepairRequestWorkOrder() {
        const { t, classes } = this.props;
        const { repairRequest, productServices, carryOutAsTypeList, terms, highlightErrors } = this.state;

        const selectedProductService = productServices && repairRequest && productServices.find(x => x.id === repairRequest.productServiceId);
        const selectedSubProductService = selectedProductService && selectedProductService.subProductServiceList && selectedProductService.subProductServiceList.find(x => x.id === repairRequest.subProductService1Id);
        const selectedSubProductService1 = selectedSubProductService && selectedSubProductService.subProductServiceList && selectedSubProductService.subProductServiceList.find(x => x.id === repairRequest.subProductService2Id);

        return (
            <div className={classes.block}>
                <Typography component="h2" variant="h6" className={classes.subHeader} >
                    <Assignment color="primary" /> &nbsp;
                    {t('Aanvullende informatie')}
                </Typography>
                <Grid container>
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={6}>{t('Product/Dienst') + ':'}</Grid>
                            <Grid item xs={6}>
                                <Autocomplete
                                    id="product-service-select"
                                    fullWidth
                                    disabled={repairRequest.completed}
                                    options={productServices && productServices}
                                    value={productServices && selectedProductService}
                                    onChange={(event, newValue) => { if (!!newValue) { this.updateRepairRequest('productserviceid', newValue.id) } }}
                                    getOptionLabel={(option) => option.code + ' - ' + option.description}
                                    renderInput={(params) =>
                                        <TextField  {...params} label={t('Product/Dienst')} variant="outlined" margin="dense"
                                            error={highlightErrors === true && this.isProductServiceRequired() && !selectedProductService}
                                        />
                                    }
                                />
                            </Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                    {
                        selectedProductService && !!selectedProductService.subProductServiceList && selectedProductService.subProductServiceList.length > 0 &&
                        <>
                            <Grid item xs={12}>
                                <Grid container className={classes.infoGridRow}>
                                    <Grid item xs={6}>{t('Product/Dienst Sub 1') + ':'}</Grid>
                                    <Grid item xs={6}>
                                        <Autocomplete
                                            id="product-subservice1-select"
                                            fullWidth
                                            disabled={repairRequest.completed}
                                            value={selectedProductService && selectedSubProductService}
                                            options={selectedProductService.subProductServiceList}
                                            onChange={(event, newValue) => { if (!!newValue) { this.updateRepairRequest('subproductservice1id', newValue.id) } }}
                                            getOptionLabel={(option) => option.description}
                                            renderInput={(params) => <TextField  {...params} label={t('Product/Dienst Sub 1')} variant="outlined" margin="dense" />}
                                        />
                                    </Grid>
                                </Grid>
                            </Grid>
                            <Grid item xs={12} >
                                <Divider />
                            </Grid>
                            {
                                selectedSubProductService && selectedSubProductService.subProductServiceList && selectedSubProductService.subProductServiceList.length > 0 &&
                                <>
                                    <Grid item xs={12}>
                                        <Grid container className={classes.infoGridRow}>
                                            <Grid item xs={6}>{t('Product/Dienst Sub 2') + ':'}</Grid>
                                            <Grid item xs={6}>
                                                <Autocomplete
                                                    id="product-subservice2-select"
                                                    fullWidth
                                                    disabled={repairRequest.completed}
                                                    value={selectedSubProductService && selectedSubProductService1}
                                                    options={selectedSubProductService.subProductServiceList}
                                                    onChange={(event, newValue) => { if (!!newValue) { this.updateRepairRequest('subproductservice2id', newValue.id) } }}
                                                    getOptionLabel={(option) => `${option.description}`}
                                                    renderInput={(params) => <TextField  {...params} label={t('Product/Dienst Sub 2')} variant="outlined" margin="dense" />}
                                                />
                                            </Grid>
                                        </Grid>
                                    </Grid>
                                    <Grid item xs={12} >
                                        <Divider />
                                    </Grid>
                                </>
                            }
                        </>
                    }
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={6}>{t('Soort') + ':'}</Grid>
                            <Grid item xs={6}>
                                <FormControl
                                    variant="outlined"
                                    margin="dense"
                                    fullWidth
                                    disabled={repairRequest.completed || (repairRequest.surveyType === 0 || repairRequest.surveyType === 1 || repairRequest.surveyType === 2)}/*Disable in case of "Inspectie", "Voorschouw" or "Oplevering"*/
                                >
                                    <InputLabel id="carry-out-as-type-select-label">{t('Soort')}</InputLabel>
                                    <Select
                                        labelId="carry-out-as-type-select-label"
                                        id="carry-out-as-type-select"
                                        value={repairRequest.carryOutAsTypeId}
                                        onChange={e => this.updateRepairRequest('carryoutastypeid', e.target.value)}
                                        label={t('Soort')}
                                        error={highlightErrors === true && !repairRequest.carryOutAsTypeId}
                                    >
                                        {
                                            !carryOutAsTypeList || carryOutAsTypeList.filter(x => x.id === repairRequest.carryOutAsTypeId).length === 0 &&
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
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={6}>{t('Werkbontekst') + ':'}</Grid>
                            <Grid item xs={6}>
                                {this.renderEditTextbox(t('Werkbontekst'), 'workordertext', repairRequest.workOrderText, true)}
                            </Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                    <Grid item xs={12}>
                        <Grid container className={classes.infoGridRow}>
                            <Grid item xs={6}>{t('Prioriteit') + ':'}</Grid>
                            <Grid item xs={6}>
                                <FormControl
                                    variant="outlined"
                                    margin="dense"
                                    fullWidth
                                    disabled={repairRequest.completed}
                                //style={{ margin: 0 }}
                                >
                                    <InputLabel id="priority-select-label">{t('Prioriteit')}</InputLabel>
                                    <Select
                                        labelId="priority-select-label"
                                        id="priority-select"
                                        value={repairRequest.priority}
                                        onChange={e => this.updateRepairRequest('priority', e.target.value)}
                                        label={t('Prioriteit')}
                                    >
                                        {
                                            (!repairRequest.priority || repairRequest.priority < 0 || repairRequest.priority > 2) &&
                                            <MenuItem value="">&nbsp;</MenuItem>
                                        }

                                        <MenuItem value={0}>{t("repairrequest.priority.0")}</MenuItem>
                                        <MenuItem value={1}>{t("repairrequest.priority.1")}</MenuItem>
                                        <MenuItem value={2}>{t("repairrequest.priority.2")}</MenuItem>

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
                            <Grid item xs={6}>{t('Afhandelingstermijn') + ':'}</Grid>
                            <Grid item xs={6}>
                                <FormControl
                                    variant="outlined"
                                    margin="dense"
                                    fullWidth
                                    disabled={repairRequest.completed}
                                //style={{ marginTop: -10, marginBottom: -10 }}
                                >
                                    <InputLabel id="terms-select-label">{t('Afhandelingstermijn')}</InputLabel>
                                    <Select
                                        labelId="terms-select-label"
                                        id="terms-select"
                                        value={repairRequest.completionTermId}
                                        onChange={e => this.updateRepairRequest('completiontermid', e.target.value)}
                                        label={t('Afhandelingstermijn')}
                                    >
                                        {
                                            !terms || terms.filter(x => x.termId === repairRequest.completionTermId).length === 0 &&
                                            <MenuItem value="">&nbsp;</MenuItem>
                                        }
                                        {
                                            terms && terms.map((t, index) => (
                                                <MenuItem key={index} value={t.termId}>{t.description + ' (' + t.noOfDays + ' ' + (t.workingDays === true ? 'werkdagen' : 'kalenderdagen') + ')'}</MenuItem>
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
                            <Grid item xs={6}>{t('Streefdatum afhandeling') + ':'}</Grid>
                            <Grid item xs={6}>{repairRequest.targetCompletionDate && formatDate(new Date(repairRequest.targetCompletionDate))}</Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12} >
                        <Divider />
                    </Grid>
                </Grid>
            </div>
        )
    }

    blockExtraInfo() {
        const { t, classes } = this.props;
        const { repairRequest, typeList, natureList, causeList, causerList, organisationList, expandExtraInfo } = this.state;
        return (
            <div className={expandExtraInfo !== true ? clsx(classes.block, 'collapsed') : classes.block}>
                <Grid container alignItems="center">
                    <Grid item className={classes.grow}>
                        <Typography component="h2" variant="h6" className={classes.subHeader} >
                            <ListAlt color="primary" /> &nbsp; {t('Extra informatie')}
                        </Typography>
                    </Grid>
                    <Grid item>
                        <Tooltip title={expandExtraInfo === true ? t('Inklappen') : t('Uitklappen')}>
                            <IconButton aria-label="expand" onClick={() => this.setState({ expandExtraInfo: !expandExtraInfo })}>
                                {
                                    expandExtraInfo === true ?
                                        <ExpandLess />
                                        :
                                        <ExpandMore />
                                }
                            </IconButton>
                        </Tooltip>
                    </Grid>
                </Grid>
                <Collapse in={expandExtraInfo === true} timeout="auto" unmountOnExit>
                    <Grid container>
                        <Grid item xs={12}>
                            <Grid container className={classes.infoGridRow}>
                                <Grid item xs={6}>{t('Type') + ':'}</Grid>
                                <Grid item xs={6}>
                                    <FormControl
                                        variant="outlined"
                                        margin="dense"
                                        fullWidth
                                        disabled={repairRequest.completed}
                                    >
                                        <InputLabel id="type-select-label">{t('Type')}</InputLabel>
                                        <Select
                                            labelId="type-select-label"
                                            id="type-select"
                                            value={repairRequest.typeId}
                                            onChange={e => this.updateRepairRequest('typeid', e.target.value)}
                                            label={t('Type')}
                                        >
                                            {
                                                !typeList || typeList.filter(x => x.id === repairRequest.typeId).length === 0 &&
                                                <MenuItem value="">&nbsp;</MenuItem>
                                            }
                                            {
                                                typeList && typeList.map((t, index) => (
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
                                <Grid item xs={6}>{t('Aard') + ':'}</Grid>
                                <Grid item xs={6}>
                                    <FormControl
                                        variant="outlined"
                                        margin="dense"
                                        fullWidth
                                        disabled={repairRequest.completed}
                                    >
                                        <InputLabel id="nature-select-label">{t('Aard')}</InputLabel>
                                        <Select
                                            labelId="nature-select-label"
                                            id="nature-select"
                                            value={repairRequest.natureId}
                                            onChange={e => this.updateRepairRequest('natureid', e.target.value)}
                                            label={t('Aard')}
                                        >
                                            {
                                                !natureList || natureList.filter(x => x.id === repairRequest.natureId).length === 0 &&
                                                <MenuItem value="">&nbsp;</MenuItem>
                                            }
                                            {
                                                natureList && natureList.map((t, index) => (
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
                                <Grid item xs={6}>{t('Oorzaak') + ':'}</Grid>
                                <Grid item xs={6}>
                                    <FormControl
                                        variant="outlined"
                                        margin="dense"
                                        fullWidth
                                        disabled={repairRequest.completed}
                                    >
                                        <InputLabel id="cause-select-label">{t('Oorzaak')}</InputLabel>
                                        <Select
                                            labelId="cause-select-label"
                                            id="cause-select"
                                            value={repairRequest.causeId}
                                            onChange={e => this.updateRepairRequest('causeid', e.target.value)}
                                            label={t('Oorzaak')}
                                        >
                                            {
                                                !causeList || causeList.filter(x => x.id === repairRequest.causeId).length === 0 &&
                                                <MenuItem value="">&nbsp;</MenuItem>
                                            }
                                            {
                                                causeList && causeList.map((t, index) => (
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
                                <Grid item xs={6}>{t('Veroorzaker') + ':'}</Grid>
                                <Grid item xs={6}>
                                    <FormControl
                                        variant="outlined"
                                        margin="dense"
                                        fullWidth
                                        disabled={repairRequest.completed}
                                    >
                                        <InputLabel id="causer-select-label">{t('Veroorzaker')}</InputLabel>
                                        <Select
                                            labelId="causer-select-label"
                                            id="causer-select"
                                            value={repairRequest.causerId}
                                            onChange={e => this.updateRepairRequest('causerid', e.target.value)}
                                            label={t('Veroorzaker')}
                                        >
                                            {
                                                !causerList || causerList.filter(x => x.id === repairRequest.causerId).length === 0 &&
                                                <MenuItem value="">&nbsp;</MenuItem>
                                            }
                                            {
                                                causerList && causerList.map((t, index) => (
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
                        {
                            repairRequest.isCauserOrganisationVisible === true &&
                            <>
                                <Grid item xs={12}>
                                    <Grid container className={classes.infoGridRow}>
                                        <Grid item xs={6}>{t('Organisatie') + ':'}</Grid>
                                        <Grid item xs={6}>
                                            <FormControl
                                                variant="outlined"
                                                margin="dense"
                                                fullWidth
                                                disabled={repairRequest.completed}
                                            >
                                                <InputLabel id="organisation-select-label">{t('Organisatie')}</InputLabel>
                                                <Select
                                                    labelId="organisation-select-label"
                                                    id="organisation-select"
                                                    value={repairRequest.causerOrganisationId}
                                                    onChange={e => this.updateRepairRequest('causerorganisationid', e.target.value)}
                                                    label={t('Organisatie')}
                                                >
                                                    {
                                                        !organisationList || organisationList.filter(x => x.id === repairRequest.causerOrganisationId).length === 0 &&
                                                        <MenuItem value="">&nbsp;</MenuItem>
                                                    }
                                                    {
                                                        organisationList && organisationList.map((t, index) => (
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
                            </>
                        }
                        <Grid item xs={12}>
                            <Grid container className={classes.infoGridRow}>
                                <Grid item xs={6}>{t('Inboektermijn') + ':'}</Grid>
                                <Grid item xs={6}>{repairRequest.bookingPeriod && repairRequest.bookingPeriod}</Grid>
                            </Grid>
                        </Grid>
                        <Grid item xs={12} >
                            <Divider />
                        </Grid>
                        <Grid item xs={12}>
                            <Grid container className={classes.infoGridRow}>
                                <Grid item xs={6}>{t('Doorlooptijd') + ':'}</Grid>
                                <Grid item xs={6}>{repairRequest.leadTime && repairRequest.leadTime}</Grid>
                            </Grid>
                        </Grid>
                        <Grid item xs={12} >
                            <Divider />
                        </Grid>
                        <Grid item xs={12}>
                            <Grid container className={classes.infoGridRow}>
                                <Grid item xs={6}>{t('Referentie opdrachtgever') + ':'}</Grid>
                                <Grid item xs={6}>
                                    {this.renderEditTextbox(t('Referentie opdrachtgever'), 'clientreference', repairRequest.clientReference)}
                                </Grid>
                            </Grid>
                        </Grid>
                        <Grid item xs={12} >
                            <Divider />
                        </Grid>
                        {
                            (repairRequest.surveyType === 0 || repairRequest.surveyType === 1 || repairRequest.surveyType === 2) &&
                            <>
                                <Grid item xs={12}>
                                    <Grid container className={classes.infoGridRow}>
                                        <Grid item xs={6}>{t('Opname') + ':'}</Grid>
                                        <Grid item xs={6}>{t('survey.type.' + repairRequest.surveyType)}</Grid>
                                    </Grid>
                                </Grid>
                                <Grid item xs={12} >
                                    <Divider />
                                </Grid>
                            </>
                        }
                    </Grid>
                </Collapse>
            </div>
        )
    }

    blockWorkorder() {
        const { t, classes, selected } = this.props;
        const { repairRequest, productServices, highlightErrors } = this.state;
        const hasErrors = (this.isProductServiceRequired() && !repairRequest.productServiceId) || !repairRequest.carryOutAsTypeId;

        const columns = [
            {
                name: 'resolverId',
                options: {
                    display: 'excluded',
                    filter: false,
                }
            },
            {
                name: 'isRequiredHandling',
                label: ' ',
                options: {
                    filter: false,
                    customBodyRender: (value, tableMeta, updateValue) => {
                        var resolverId = tableMeta.rowData[0];
                        var resolver = !!repairRequest && repairRequest.resolvers.find(x => x.resolverId === resolverId);
                        return (
                            <Typography noWrap>
                                {value === true && (
                                    <Tooltip title={t("Actie vereist")}>
                                        <PriorityHigh color="secondary" size="small" />
                                    </Tooltip>

                                )}
                                {
                                    resolver.overdue ?
                                        (
                                            <Tooltip title={t("Te laat")}>
                                                <Schedule fontSize="small" color="error" />
                                            </Tooltip>
                                        )
                                        : resolver.is48HoursReminder ?
                                            (
                                                <Tooltip title={t("Binnen 48 uur")}>
                                                    <Schedule
                                                        fontSize="small"
                                                        className={classes.warning}
                                                    />
                                                </Tooltip>
                                            )
                                            :
                                            ("")
                                }
                            </Typography>
                        );
                    }
                }
            },
            {
                name: 'targetCompletionDate',
                label: t('Streefdatum'),
                options: {
                    filter: true,
                    customBodyRender: v => v && formatDate(new Date(v))
                }
            },
            {
                name: 'workOrderNumber',
                label: t('Werkbonnummer'),
                options: {
                    filter: true,
                    customBodyRender: (value, tableMeta, updateValue) => {
                        var resolverId = tableMeta.rowData[0];
                        return <Button style={{ padding: 0, minWidth: "auto" }} component={Link} to={"/werk/" + selected.projectNo + "/werkbon/" + resolverId} color="primary">{!value ? t('Concept') : value}</Button>
                    },
                }
            },
            {
                name: 'name',
                label: t('Oplosser'),
                options: {
                    filter: true,
                }
            },
            {
                name: 'status',
                label: t('Status'),
                options: {
                    filter: true,
                    customBodyRender: v => v >= 0 && v <= 4 && t('resolver.status.' + v)
                }
            }
        ];

        const options = {
            //filterType: 'dropdown',
            //responsive: 'vertical',
            download: false,
            // filter: true,
            print: false,
            search: false,
            viewColumns: false,
            pagination: false,
            customToolbar: () => (
                <>
                    <Tooltip title={t('Maak werkbon')}>
                        <IconButton
                            aria-describedby={'select-organisation'}
                            color="inherit"
                            aria-label="Add"
                            component="span"
                            disabled={(hasErrors && highlightErrors) || this.state.workorderPopoverOpenLoader || repairRequest.completed === true}
                            onClick={
                                ({ currentTarget }) => {
                                    if (!!hasErrors)
                                        this.setState({ highlightErrors: true });
                                    else {
                                        this.setState({ addWorkOrderAnchorEl: currentTarget })
                                    }
                                }
                            }
                        >
                            {this.state.workorderPopoverOpenLoader ? <CircularProgress size={20} /> : <Add />}
                        </IconButton>
                    </Tooltip>
                    {
                        <Popover
                            open={!this.state.workorderPopoverOpenLoader && !!this.state.addWorkOrderAnchorEl}
                            anchorOrigin={{
                                vertical: 'top',
                                horizontal: 'center',
                            }}
                            transformOrigin={{
                                vertical: 'bottom',
                                horizontal: 'left',
                            }}
                            id={'select-organisation'}
                            anchorEl={this.state.addWorkOrderAnchorEl}
                            onClose={() => this.setState({ addWorkOrderAnchorEl: null })}
                        >
                            <SelectOrganisationPopover
                                setLoading={(val) => this.setState({ workorderPopoverOpenLoader: val })}
                                projectId={selected && selected.projectId}
                                productServices={productServices}
                                disabled={repairRequest.completed}
                                selectOrgAnchorEl={this.state.addWorkOrderAnchorEl}
                                setSelectOrgAnchorEl={v => this.setState({ addWorkOrderAnchorEl: v })}
                                onSelect={item => this.AddRepairRequestResolver(item.id)} />
                        </Popover>
                    }
                </>
            ),
            customToolbarSelect: (selectedRows, displayData, setSelectedRows) => (
                <div style={{ paddingRight: 4 }}>
                    {
                        repairRequest && repairRequest.completed !== true && this.state.selectedResolverIds && this.state.selectedResolverIds.length == 1 &&
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
                                        disabled={repairRequest.completed}
                                        selectOrgAnchorEl={this.state.copyWorkOrderAnchorEl}
                                        setSelectOrgAnchorEl={v => this.setState({ copyWorkOrderAnchorEl: v })}
                                        onSelect={item => this.copyWorkOrder(item.id)} />
                                </Popover>
                            }
                        </>
                    }

                    <Tooltip title={t('Herinnering versturen')}>
                        <IconButton
                            color="inherit"
                            aria-label="Reminder"
                            component="span"
                            disabled={repairRequest.completed === true || this.canSendReminderSelectedWorkorders() === false}
                            onClick={(e) => {
                                this.setState({ sendReminderAnchorEl: e.currentTarget });
                            }}
                        >
                            <Mail />
                        </IconButton>
                    </Tooltip>
                    {
                        !!this.state.sendReminderAnchorEl &&
                        <Dialog open={true}
                            onClose={() => this.setState({
                                sendReminderCCEmailList: [], sendReminderCCEmailValid: true, sendReminderAnchorEl: null
                            })}
                            aria-labelledby="form-dialog-title">
                            <DialogTitle id="send_reminder">{t('Herinnering versturen')}</DialogTitle>
                            <DialogContent>
                                <TextField
                                    autoFocus
                                    disabled={this.state.sendingReminder === true}
                                    error={this.state.sendReminderCCEmailValid !== true}
                                    rows={2}
                                    variant="outlined"
                                    onChange={(e) => this.updateCCEmails(e.target.value)}
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
                                            sendReminderCCEmailList: [], sendReminderCCEmailValid: true, sendReminderAnchorEl: null
                                        })
                                    }}
                                    color="primary"
                                >
                                    {t('Annuleer')}
                                </Button>
                                <Button
                                    disabled={this.state.sendReminderCCEmailValid !== true || this.state.sendingReminder === true}
                                    onClick={() => { this.SendReminderToResolvers() }}
                                    color="primary"
                                >
                                    {t('Herinnering versturen')}
                                </Button>
                            </DialogActions>
                        </Dialog>
                    }
                    <Tooltip title={t('datatable.label.selectedrows.delete')}>
                        <IconButton
                            color="inherit"
                            aria-label="Delete"
                            component="span"
                            disabled={repairRequest.completed === true || this.canDeleteSelectedWorkorders() === false}
                            onClick={() => { this.DeleteRepairRequestResolvers(); setSelectedRows([]); }}
                        >
                            <Delete />
                        </IconButton>
                    </Tooltip>
                </div>
            ),
            onRowSelectionChange: (currentRowsSelected, allRowsSelected, rowsSelected) => {
                let selectedResolverIds = [];
                for (var i = 0; i < rowsSelected.length; i++) {
                    const resolver = repairRequest.resolvers[rowsSelected[i]];
                    selectedResolverIds.push(resolver.resolverId);
                }
                this.setState({ selectedResolverIds })
            },
            textLabels: getDataTableTextLabels(t),
        };

        return (
            repairRequest &&
            <div className={classes.block}>
                <Grid container>
                    <Grid item xs={12}>
                        <MUIDataTable
                            className={classes.dataTable}
                            title={
                                (
                                    <Typography component="h2" variant="h6" className="header">
                                        <Build color="primary" /> &nbsp; {t('Werkbonnen')}
                                    </Typography>
                                )
                            }
                            data={!!repairRequest.resolvers ? repairRequest.resolvers : []}
                            columns={columns}
                            options={options}
                        />
                    </Grid>
                </Grid>
            </div>
        )
    }

    render() {
        const { t, classes, selected, width } = this.props;
        const { repairRequest, attachmentIndex, isNotify, toEmail, ccEmails, emailModel, repairRequestLoading } = this.state;
        const textBoxValue = this.state.handleType === 'complete' ? this.state.completionTextToEdit : this.state.rejectionTextToEdit;
        const textBoxLabel = this.state.handleType === 'complete' ? t('Oplossing') : t('Reden afwijzing');
        const isLargeScreen = isWidthUp('lg', width);
        return (
            <Container maxWidth={false} className={classes.mainContainer}>
                <Grid container>
                    <Grid item container xs={12} className={classes.container}>
                        <AppBar position="sticky">
                            <Toolbar variant="dense">
                                {
                                    selected &&
                                    <IconButton
                                        color="inherit"
                                        edge="start"
                                        aria-label="go back"
                                        component="span"
                                        onClick={() => history.push('/werk/' + selected.projectNo + '/kwaliteitsborging')}
                                    >
                                        <ArrowBack />
                                    </IconButton>
                                }
                                <Typography className={clsx(classes.bold, classes.grow)}>{t('Details van melding ') + (repairRequest ? repairRequest.number : '')}</Typography>
                                {
                                    repairRequest && repairRequest.completed !== true &&
                                    <>
                                        {
                                            (!repairRequest.resolvers || (repairRequest.resolvers.length > 0 && (repairRequest.isAllWorkOrderDeclined || repairRequest.isAllWorkOrderCompleted))) &&
                                            <>
                                                <Button
                                                    aria-describedby="handleRepairRequestPopup"
                                                    variant="outlined"
                                                    color="inherit"
                                                    style={{ marginLeft: 12 }}
                                                    onClick={e => this.initiateHandle(e, 'turndown')}
                                                >
                                                    {t('repairrequest.button.turndown')}
                                                </Button>
                                                <Button
                                                    aria-describedby="handleRepairRequestPopup"
                                                    variant="outlined"
                                                    color="inherit"
                                                    style={{ marginLeft: 12 }}
                                                    onClick={e => this.initiateHandle(e, 'complete')}
                                                >
                                                    {t('repairrequest.button.complete')}
                                                </Button>
                                            </>
                                        }
                                        {
                                            !!this.state.anchorElHandleRepairRequest && this.state.handleType &&
                                            <Popover open={true}
                                                transformOrigin={{
                                                    vertical: 'top',
                                                    horizontal: 'right',
                                                }}
                                                id={'handleRepairRequestPopup'}
                                                anchorEl={this.state.anchorElHandleRepairRequest}
                                                onClose={() => { this.setState({ anchorElHandleRepairRequest: null }) }}
                                            >
                                                <div style={{ padding: '16px' }}>
                                                    <Grid container spacing={1} direction="column">
                                                        <Grid item>
                                                            <Typography variant="h6">{t('repairrequest.button.' + this.state.handleType)}</Typography>
                                                        </Grid>
                                                        <Grid item>
                                                            <TextField
                                                                label={textBoxLabel}
                                                                className={classes.textField}
                                                                value={textBoxValue}
                                                                onChange={e => {
                                                                    if (this.state.handleType === 'complete')
                                                                        this.setState({ completionTextToEdit: e.target.value })
                                                                    else
                                                                        this.setState({ rejectionTextToEdit: e.target.value })
                                                                }}
                                                                margin="dense"
                                                                variant="outlined"
                                                                multiline
                                                                rows={10}
                                                                fullWidth
                                                                autoFocus
                                                                error={!textBoxValue || textBoxValue.trim() === ''}
                                                                disabled={repairRequest.completed}
                                                            />
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
                                                                label={t('Nu inlichten')}
                                                            />
                                                        </Grid>
                                                        <Collapse in={isNotify} timeout="auto" unmountOnExit>
                                                            <Grid item>
                                                                <FormControl
                                                                    component="fieldset"
                                                                    className={classes.formControlCheckList}
                                                                    required
                                                                    error={!toEmail || toEmail.trim() === ''}
                                                                >
                                                                    <FormLabel component="legend">{t('Aan')}</FormLabel>
                                                                    {
                                                                        !toEmail || toEmail.trim() === '' &&
                                                                        <FormHelperText>{t('Geen emailadres bekend')}</FormHelperText>
                                                                    }
                                                                    <RadioGroup
                                                                        aria-label="toEmail"
                                                                        name="toEmail"
                                                                        value={toEmail}
                                                                        onChange={e => {
                                                                            this.setState({ toEmail: e.target.value, ccEmails: ccEmails.filter(x => x !== e.target.value) });
                                                                        }}
                                                                    >
                                                                        {
                                                                            emailModel && emailModel.filter(x => x.emails && x.emails.length > 0 && x.key !== 'resolver').map(
                                                                                (item, index) => (
                                                                                    <FormControlLabel key={index}
                                                                                        value={item.key}
                                                                                        control={<Radio />}
                                                                                        label=
                                                                                        {
                                                                                            <>
                                                                                                {item.title}
                                                                                                {
                                                                                                    toEmail === item.key &&
                                                                                                    <>
                                                                                                        {
                                                                                                            item.emails && item.emails.map(
                                                                                                                (email, index1) => (
                                                                                                                    <React.Fragment key={index1}>
                                                                                                                        <br />
                                                                                                                        <Typography variant="caption">{email}</Typography>
                                                                                                                    </React.Fragment>
                                                                                                                )
                                                                                                            )
                                                                                                        }
                                                                                                    </>
                                                                                                }
                                                                                            </>
                                                                                        }
                                                                                    />
                                                                                )
                                                                            )
                                                                        }
                                                                    </RadioGroup>
                                                                </FormControl>
                                                            </Grid>
                                                            <Grid item>
                                                                <FormControl component="fieldset" className={classes.formControlCheckList}>
                                                                    <FormLabel component="legend">{t('CC')}</FormLabel>
                                                                    <FormGroup>
                                                                        {
                                                                            emailModel && emailModel.filter(x => x.key !== toEmail && x.emails && x.emails.length > 0).map(
                                                                                (item, index) => (
                                                                                    <FormControlLabel key={index}
                                                                                        control={
                                                                                            <Checkbox checked={ccEmails.includes(item.key)}
                                                                                                color="primary"
                                                                                                onChange={
                                                                                                    e => {
                                                                                                        if (e.target.checked === true) {
                                                                                                            let ccEmailsToUpdate = ccEmails.slice();
                                                                                                            ccEmailsToUpdate.push(item.key);
                                                                                                            this.setState({ ccEmails: ccEmailsToUpdate });
                                                                                                        }
                                                                                                        else {
                                                                                                            this.setState({ ccEmails: ccEmails.filter(x => x !== item.key) });
                                                                                                        }
                                                                                                    }}
                                                                                                name={'cc-' + item.key}
                                                                                            />
                                                                                        }
                                                                                        label=
                                                                                        {
                                                                                            <>
                                                                                                {item.title}
                                                                                                {
                                                                                                    ccEmails.includes(item.key) &&
                                                                                                    <>
                                                                                                        {
                                                                                                            item.emails && item.emails.map(
                                                                                                                (email, index1) => (
                                                                                                                    <React.Fragment key={index1}>
                                                                                                                        <br />
                                                                                                                        <Typography variant="caption">{email}</Typography>
                                                                                                                    </React.Fragment>
                                                                                                                )
                                                                                                            )
                                                                                                        }
                                                                                                    </>
                                                                                                }
                                                                                            </>
                                                                                        }
                                                                                    />
                                                                                )
                                                                            )
                                                                        }
                                                                    </FormGroup>
                                                                </FormControl>
                                                            </Grid>
                                                        </Collapse>
                                                        <Grid item>
                                                            <Grid container spacing={1} justify="flex-end">
                                                                <Grid item>
                                                                    <Button
                                                                        variant="outlined"
                                                                        onClick={() => { this.setState({ anchorElHandleRepairRequest: null }) }}
                                                                    >
                                                                        {t('Annuleer')}
                                                                    </Button>
                                                                </Grid>
                                                                <Grid item>
                                                                    <Button
                                                                        variant="outlined"
                                                                        disabled={(isNotify === true && (!toEmail || toEmail.trim() === '')) || (!textBoxValue || textBoxValue.trim() === '')}
                                                                        onClick={() => this.handleRepairRequest()}
                                                                    >
                                                                        {t('repairrequest.button.' + this.state.handleType)}
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
                            repairRequestLoading ?
                                <div className={classes.mainLoaderContainer}>
                                    <CircularProgress size={30} />
                                </div> :
                                repairRequest &&
                                <Grid item xs={12}>
                                    <Grid container spacing={2} className={classes.innerContainer} alignItems="flex-start">
                                        {
                                            //<Grid item xs={12}>
                                            //    <Typography variant="h6">{t('Algemene informatie')}</Typography>
                                            //</Grid>
                                        }
                                        <Grid item xs={12} md={6} lg={4}>
                                            {this.blockGeneralRepairRequest()}
                                        </Grid>
                                        {
                                            isLargeScreen &&
                                            <Grid item xs={4}>
                                                <Grid container spacing={2}>
                                                    <Grid item xs={12}>
                                                        {this.blockRepairRequestWorkOrder()}
                                                    </Grid>
                                                    <Grid item xs={12}>
                                                        {this.blockExtraInfo()}
                                                    </Grid>
                                                    <Grid item xs={12}>
                                                        {this.blockWorkorder()}
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
                                            //<Grid item xs={12}>
                                            //    <Typography variant="h6">{t('Extra benodigde informatie')}</Typography>
                                            //</Grid>
                                        }
                                        {
                                            !isLargeScreen &&
                                            <>
                                                <Grid item xs={12} md={6}>
                                                    {this.blockRepairRequestWorkOrder()}
                                                </Grid>
                                                <Grid item xs={12} md={6}>
                                                    {this.blockExtraInfo()}
                                                </Grid>
                                                <Grid item xs={12} md={6}>
                                                    {this.blockWorkorder()}
                                                </Grid>
                                            </>
                                        }
                                    </Grid>

                                </Grid>
                        }
                    </Grid>
                </Grid>
                {
                    repairRequest && repairRequest.attachments && attachmentIndex != null && (attachmentIndex >= 0 && attachmentIndex < repairRequest.attachments.length) &&
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
                                    image={webApiUrl + 'api/home/getattachment/' + encodeURI(repairRequest.attachments[attachmentIndex] ? repairRequest.attachments[attachmentIndex].attachmentId : '')}
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
export { connectedPage as RepairRequestWorkOrdersPage };
