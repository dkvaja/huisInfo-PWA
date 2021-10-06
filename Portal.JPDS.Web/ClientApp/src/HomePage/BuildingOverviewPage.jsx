import React from "react";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import {
    Avatar,
    Container,
    Grid,
    Typography,
    Divider,
    Card,
    CardContent,
    CardHeader,
    List,
    ListItem,
    ListItemText,
    ListItemAvatar,
    Badge,
    Modal,
    Backdrop,
    Fade,
    IconButton,
    CardMedia,
    Dialog,
    DialogContent,
    DialogTitle,
    Box,
    Tabs,
    Tab,

    CircularProgress,
    Table,
    TableBody,
    TableCell,
    TableRow,
    ExpansionPanel,
    ExpansionPanelSummary,
    ExpansionPanelDetails,

    ListItemSecondaryAction,

    TextField,
    MenuItem,
    Button,

    Tooltip,

    ListItemIcon
} from "@material-ui/core";
import { withStyles, withTheme } from '@material-ui/core/styles';
import { Schedule, Chat, LibraryBooks, Assignment, Close, ArrowBackIos, ArrowForwardIos, KeyboardArrowLeft, KeyboardArrowRight, FormatQuote, Description, AttachFile, People, ArrowDropDown, CloudDownload, Notifications, ShoppingBasket, AssignmentInd, LocalOffer, CheckCircleOutline, CheckCircle, Add, AssignmentOutlined, AssignmentTurnedIn, Block, AssignmentLate, CreateOutlined, Create, LocalOfferOutlined, Bookmarks, Bookmark, InfoOutlined, Airplay, TimerOff } from "@material-ui/icons"
import DateFnsUtils from '@date-io/date-fns';
import nlLocale from "date-fns/locale/nl";
import enLocale from "date-fns/locale/en-US";
import {
    MuiPickersUtilsProvider,
    KeyboardTimePicker,
    KeyboardDatePicker,
    KeyboardDateTimePicker,
    DateTimePicker,
} from '@material-ui/pickers';
import { history, validateFile, getDateText, nl2br, md2plaintext, formatDate, formatTime, authHeader, getRights } from "../_helpers"
import { userAccountTypeConstants } from "../_constants"
//import './home.css';
import { withTranslation } from 'react-i18next';
import NumberFormat from "react-number-format";

const { webApiUrl } = window.appConfig;

const styles = theme => ({
    grow: { flexGrow: 1 },
    fullHeight: {
        height: '100%'
    },
    cardContainer: {
        maxHeight: '50%',
        height: '50%',
        minHeight: 280,
        [theme.breakpoints.down("sm")]: {
            height: 'auto',
            minHeight: 'auto'
        }
    },
    card: {
        height: '100%',
        margin: 'auto'
    },
    cardHeader: {
        backgroundColor: theme.palette.primary.main,
        color: theme.palette.primary.contrastText,
        '& .MuiCardHeader-action': {
            margin: theme.spacing(-1)
        }
    },
    cardHeaderMessages: {
        backgroundColor: theme.palette.primary.main,
        color: theme.palette.primary.contrastText,
        padding: theme.spacing(1),
        '& .MuiTab-root': {
            minWidth: 'auto'
        }
    },
    cardHeaderOpties: {
        backgroundColor: theme.palette.primary.main,
        color: theme.palette.primary.contrastText,
        padding: theme.spacing(1),
        '& .MuiCardHeader-content': {
            width: '100%'
        },
        '& .MuiTab-root': {
            minWidth: 56,
            flexGrow: 1
        }
    },
    fullWidth: {
        width: '100%'
    },
    list: {
        overflow: 'auto',
        maxHeight: 'calc(100% - 64px)',
    },
    chatListIcon: {
        right: -3,
        top: '50%',
        position: 'absolute',
        marginTop: -12
    },
    chatListDate: {
        position: 'absolute',
        right: 18,
        top: 6
    },
    chatListCount: {
        position: 'absolute',
        right: 28,
        bottom: 16
    },
    chatListItem: {
        paddingTop: 0,
        paddingBottom: 0,
        '& .MuiListItemText-root': {
            marginRight: '60px'
        }
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
        padding: 0
    },
    divWithHtmlContent: {
        '& *': {
            maxWidth: '100%'
        }
    },
    expansionPanel: {
        width: '100%',
        '& .MuiExpansionPanelSummary-root.Mui-expanded': {
            minHeight: 48,
        },
        '& .MuiExpansionPanelSummary-content.Mui-expanded': {
            margin: '12px 0'
        }
    },
    documentHeading: {
        color: theme.palette.primary.contrastText,
        backgroundColor: theme.palette.primary.light,
        '& .MuiIconButton-root': {
            color: "inherit"
        }
    },
    expansionPanelDetails: {
        padding: theme.spacing(1, 3)
    },
    importantChatHeader: {
        backgroundColor: theme.palette.background.default,
        paddingTop: 0,
        paddingBottom: 0,
        '&.MuiListItem-root.Mui-selected': {
            backgroundColor: theme.palette.primary.light,
            color: theme.palette.primary.contrastText,
            '& .MuiTypography-root': {
                fontWeight: 'bold'
            }
        }
    },
    importantChatItem: {
        paddingTop: 0,
        paddingBottom: 0,
        paddingRight: 80,
        height: 48,
        '& .MuiListItemText-root': {
            maxHeight: 40,
            overflow: 'hidden',
            marginTop: 0,
            marginBottom: 0
        }
    },
    modal: {
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
    },
    modalCard: {
        maxWidth: 500
    },
    modalCardHeader: {
        backgroundColor: theme.palette.primary.main,
        color: theme.palette.primary.contrastText
    },
    thumbnail: {
        width: 'calc(100% - 16px)',
        margin: theme.spacing(-1, 0),
        height: 50,
        backgroundRepeat: 'no-repeat',
        backgroundPosition: 'left center',
        backgroundSize: 'contain',
    },
    loadingData:{
        display:'flex',
        justifyContent:'center',
        alignItems:'center',
        marginTop:12
    }
});


class BuildingOverviewPage extends React.Component {
    state = {
        actions: [],
        plannings: [],
        news: [],
        messageTabValue: 0,
        messages: [],
        importantMessages: [],
        optionTabValue: 0,
        groupedOptions: null,
        documentHeaders: [],
        uploading: false,
        buyer: null,
        employees: [],
        rights: {}
    };

    componentDidMount() {
        this.UpdatePlannings();
        this.UpdateMessages();
        this.UpdateImportantMessages();
        this.UpdateActions();
        this.UpdateNews();
        this.UpdateOptions();
        this.UpdateDocuments();
        this.UpdateBuyer();
        this.UpdateActionEmployess();
        this.getSelectedBuildingRights();
        var intervalId = setInterval(this.timer, 10000);
        // store intervalId in the state so it can be accessed later:
        this.setState({ intervalId: intervalId });
    }

    componentWillUnmount() {
        // use intervalId from the state to clear the interval
        clearInterval(this.state.intervalId);
    }

    componentDidUpdate(prevProps, prevState) {
        if (!prevProps.selected || prevProps.selected.buildingId !== this.props.selected.buildingId) {
            this.UpdatePlannings();
            this.UpdateMessages();
            this.UpdateImportantMessages();
            this.UpdateActions();
            this.UpdateNews();
            this.UpdateOptions();
            this.UpdateDocuments();
            this.UpdateBuyer();
            this.UpdateActionEmployess();
            this.getSelectedBuildingRights();
        }
        if(prevProps.selected && this.props.selected && prevProps.selected.buildingId!==this.props.selected.buildingId){
            this.setState({buyer:null});
        }
    }

    timer = () => {
        this.UpdateMessages(false);
        this.UpdateImportantMessages();
    }

    getSelectedBuildingRights() {
        const { selected } = this.props;
        if (selected) {
            const rights = getRights(selected.roles);
            this.setState({ rights })
        }
    }

    UpdateActions() {
        const { selected, user } = this.props;
        if (selected) {
            this.setState({getActionsLoading:true})
            const url = webApiUrl + 'api/home/GetActionsByBuildingId/' + encodeURI(selected.buildingId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({
                        actions: findResponse,
                        getActionsLoading:false
                    });
                }).catch(er=>{
                    this.setState({getActionsLoading:false})
                });
        }
    }

    UpdatePlannings() {
        const { selected, user } = this.props;
        if (selected) {
            this.setState({getPlanningsLoading:true})
            const url = webApiUrl + 'api/home/GetPlanningsByBuildingId/' + selected.buildingId;
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({
                        plannings: findResponse,
                        getPlanningsLoading:false
                    });
                }).catch(er=>{
                     this.setState({getPlanningsLoading:false})
                });
        }
    }

    UpdateNews() {
        const { selected } = this.props;
        if (selected) {
            this.setState({getNewsLoading:true})
            const url = webApiUrl + 'api/home/GetNewsByProjectId/' + encodeURI(selected.projectId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({
                        news: findResponse,
                        getNewsLoading:false
                    });
                }).catch(er=>{
                    this.setState({getNewsLoading:false})
                });
        }
    }

    UpdateMessages(refresh = true) {
        const { selected, user } = this.props;
        const { messages } = this.state;
        if (selected) {
            if(refresh){
                 this.setState({getMessagesLoading:true})
            }

             if (refresh)  clearInterval(this.state.intervalId)

            if (this.messageAbortController && this.messageAbortController.signal.aborted !== true) {
                this.messageAbortController.abort();
            }

             this.messageAbortController = new window.AbortController();

            const url = webApiUrl + 'api/chat/GetChatsByBuilding/' + selected.buildingId
                + (messages.length > 0 && !refresh ? '?dateTime=' + encodeURIComponent(messages[0].dateTime) : '');
            const requestOptions = {
                method: 'GET',
                headers: authHeader(),
                signal: this.messageAbortController.signal
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    var allChats = refresh === true ? [] : messages.slice();

                    if (refresh) {
                        clearInterval(this.state.intervalId);
                        const intervalId = setInterval(this.timer, 10000);
                        this.setState({ intervalId })
                    }

                    for (var i = 0; i < findResponse.length; i++) {
                        var existingChat = allChats.find(x => x.chatId === findResponse[i].chatId);
                        if (existingChat) {
                            existingChat.dateTime = findResponse[i].dateTime;
                            existingChat.lastChatMessagePartialText = findResponse[i].lastChatMessagePartialText;
                            existingChat.isSender = findResponse[i].isSender;
                            existingChat.senderName = findResponse[i].senderName;
                            existingChat.unreadMessagesCount = findResponse[i].unreadMessagesCount;
                        }
                        else {
                            allChats.push(findResponse[i]);
                        }
                    }

                    allChats.sort(function (a, b) { return new Date(b.dateTime) - new Date(a.dateTime); });

                    this.setState({
                        messages: allChats,
                        getMessagesLoading:false
                    });
                }).catch(err=>{
                   if(err.code !== 20)
                        this.setState({getMessagesLoading:false})
                });
        }
    }

    UpdateImportantMessages() {
        const { selected, user } = this.props;
        if (selected) {
            const url = webApiUrl + 'api/chat/GetImportantMessagesByBuilding/' + selected.buildingId;

            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({
                        importantMessages: findResponse
                    });
                });
        }
    }

    UpdateBuyer() {
        const { selected } = this.props;
        if (selected) {
            if (this.buyerAbortController && this.buyerAbortController.signal.aborted !== true) {
                this.buyerAbortController.abort();
            }

             this.buyerAbortController = new window.AbortController();
            this.setState({getBuyersLoading:true})
            const url = webApiUrl + 'api/home/GetBuyerInfo/' + encodeURI(selected.buildingBuyerRenterId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader(),
                 signal: this.buyerAbortController.signal
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({
                        buyer: findResponse,
                        getBuyersLoading:false
                    });
                }).catch(er=>{
                    this.setState({getBuyersLoading:false})
                });
        }
    }

    UpdateOptions() {
        const { selected } = this.props;
        if (selected) {
            this.setState({getOptionsLoading:true})
            const url = webApiUrl + 'api/shopping/GetOptionsOverview/' + encodeURI(selected.buildingId)
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            this.setState({
                groupedOptions: null
            });

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({
                        groupedOptions: findResponse,
                        getOptionsLoading:false
                    });
                }).catch(er=>{
                    this.setState({getOptionsLoading:false})
                });
        }
    }

    UpdateDocuments() {
        const { selected } = this.props;
        if (selected) {
            this.setState({getDocumentsLoading:true})
            const url = webApiUrl + 'api/home/GetBuildingDocuments/' + encodeURI(selected.buildingId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({ documentHeaders: findResponse,getDocumentsLoading:false });
                }).catch(er=>{
            this.setState({getDocumentsLoading:false})
                });
        }
    }

    UpdateActionEmployess() {
        const { selected, user } = this.props;
        if (selected) {
            const url = webApiUrl + 'api/home/GetEmployees';

            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({
                        employees: findResponse
                    });
                });
        }
    }

    uploadDocument = e => {
        const { selected, t } = this.props;
        const files = Array.from(e.target.files);

        if (validateFile(files[0]) === true) {
            this.setState({ uploading: true })

            const formData = new FormData()

            formData.append('file', files[0])

            const url = webApiUrl + `api/home/UploadDocument/` + encodeURI(selected.buildingId);
            fetch(url, {
                method: 'POST',
                headers: authHeader(),
                body: formData
            })
                .then(res => res.json())
                .then(res => {
                    this.setState({
                        uploading: false
                    })
                    this.UpdateDocuments();
                })
                .catch(e => {
                    this.setState({ uploading: false });
                    alert(t('general.api.error'));
                })
        }
    }

    getChatSubTitle = chat => {
        const { user } = this.props;
        if (user.type !== userAccountTypeConstants.buyer) {
            return chat.buildingNoExtern
        }
        return chat.organisationName
    }

    handleChangeMessagesTab = (event, newValue) => {
        this.setState({ messageTabValue: newValue });
    };

    handleActionDialogOpen = actionIndex => {
        this.setState({ actionIndex });
    }

    changeActionItemIndex = i => {
        this.setState({ actionIndex: (this.state.actionIndex + i) });
    }

    handleActionDialogClose = () => {
        this.setState({ actionIndex: -1 });
    }


    handleNewsDialogOpen = newsIndex => {
        this.setState({ newsIndex });
    }

    changeNewsItemIndex = i => {
        this.setState({ newsIndex: (this.state.newsIndex + i) });
    }

    handleNewsDialogClose = () => {
        this.setState({ newsIndex: -1 });
    }

    handleChangeOptionsTab = (event, newValue) => {
        this.setState({ optionTabValue: newValue });
    };

    handleUnmarkImportantChatMessage(message, chatId) {
        const url = webApiUrl + 'api/chat/MarkUnmarkChatMessageImportant/' + message.chatMessageId + '?isMark=' + false;
        const requestOptions = {
            method: 'GET',
            headers: authHeader()
        };

        fetch(url, requestOptions)
            .then(Response => {
                var importantMessages = this.state.importantMessages.slice();

                var chatToUpdate = importantMessages.find(x => x.chatId.toUpperCase() === chatId.toUpperCase());
                if (chatToUpdate) {
                    chatToUpdate.messages = chatToUpdate.messages.filter(x => x.chatMessageId.toUpperCase() !== message.chatMessageId.toUpperCase());
                    this.setState({
                        importantMessages
                    });
                }
            })
            .catch(error => {
                console.log('Error while removing');
            });
    }

    handleNewActionModalOpen = () => {
        const newAction = {
            employeeId: '',
            description: '',
            descriptionExtended: '',
            dateTime: Date.now()
        }
        this.setState({ newAction });
    }

    handleNewActionModalClose = () => {
        this.setState({ newAction: null });
    }

    handleModalDateChange = date => {
        const newAction = Object.assign({}, this.state.newAction);
        newAction.date = date;
        this.setState({ newAction });
    }

    handleModalChangeTextField = name => event => {
        const newAction = Object.assign({}, this.state.newAction);
        switch (name) {
            case 'employeeId': newAction.employeeId = event.target.value; break;
            case 'description': newAction.description = event.target.value; break;
            case 'descriptionExtended': newAction.descriptionExtended = event.target.value; break;
            default: return;
        }
        this.setState({ newAction });
    }

    handleModalActionSubmit = event => {
        event.preventDefault();
        const newAction = Object.assign({}, this.state.newAction);
        const { selected, t } = this.props;

        if (selected && event.target.checkValidity()) {
            newAction.submitting = true;
            this.setState({ newAction });

            const url = webApiUrl + 'api/home/AddNewAction';
            const requestOptions = {
                method: 'POST',
                headers: authHeader('application/json'),
                body: JSON.stringify({
                    buildingId: selected.buildingId,
                    employeeId: newAction.employeeId,
                    description: newAction.description,
                    descriptionExtended: newAction.descriptionExtended,
                    dateTime: new Date(newAction.date).toJSON()
                })
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(success => {
                    if (success) {
                        alert(t('Uw actie is toevoegd.'));
                        this.handleNewActionModalClose();
                        this.UpdateActions();
                    }
                })
                .catch(error => {
                    alert(t('general.api.error'));
                    newAction.submitting = false;
                    this.setState({ newAction });
                });
        }
        else {
            newAction.submitted = true;
            this.setState({ newAction });
        }
    }

    renderActions() {
        const { user, t, classes } = this.props;
        const { rights,getActionsLoading } = this.state;

        return (
            <Card className={classes.card}>
                <CardHeader title={
                    <Typography variant="h6">{t('dashboard.actions.title')}</Typography>
                } avatar={<Assignment />}
                    action={
                        <IconButton
                            disabled={!rights['selected.object.write']}
                            color="inherit"
                            aria-label="new Action"
                            component="span"
                            onClick={this.handleNewActionModalOpen}
                        >
                            <Add />
                        </IconButton>
                    }
                    className={classes.cardHeader} />
                    {getActionsLoading?<div className={classes.loadingData}><CircularProgress size={25} /></div>:
                <List className={classes.list}>
                    {
                        this.state.actions.length > 0 ?
                            this.state.actions.map((data, index) => (
                                <React.Fragment key={index}>
                                    {index !== 0 && <Divider component="li" />}
                                    <ListItem title={data.description} button onClick={() => this.handleActionDialogOpen(index)}>
                                        <ListItemText primary={
                                            <Typography noWrap>{data.description}</Typography>
                                        } secondary={t('general.date') + ': ' + formatDate(new Date(data.actionDate)) + ' ' + t('general.time') + ': ' + formatTime(data.startTime) + ' uur'} />
                                    </ListItem>
                                </React.Fragment>
                            ))
                            : <ListItem><ListItemText secondary={t('dashboard.actions.nodata')} /></ListItem>
                    }
                </List>}
            </Card>
        )
    }

    renderPlannings() {
        const { user, t, classes, selected } = this.props;
        const { getPlanningsLoading } = this.state;
        const baseUrl = `/werk/${selected.projectNo}/dossier/`;
        const currentDateTime = Date.now();

        return (
            <Card className={classes.card}>
                <CardHeader title={
                    <Typography variant="h6">{t('dashboard.planning.title')}</Typography>
                } avatar={<Schedule />} className={classes.cardHeader} />
                {getPlanningsLoading?<div className={classes.loadingData}><CircularProgress size={25} /></div>:
                <List className={classes.list}>
                    {
                        this.state.plannings.length > 0 ?
                            this.state.plannings.map((data, index) => {
                                const date = new Date(data.date);
                                const differenceInDays = Math.ceil((date - currentDateTime) / (1000 * 3600 * 24)) + 1;
                                return (
                                    <React.Fragment key={index}>
                                        {index !== 0 && <Divider component="li" />}
                                        <ListItem title={data.description} button component={Link}
                                            to={{
                                                pathname: data.dossierId ? `${baseUrl}${data.dossierId}` : '',
                                                search: data.dossierId ? `buildingId=${selected.buildingId}` : ''
                                            }}
                                            style={{
                                                cursor: data.dossierId ? 'pointer' : 'default'
                                            }}

                                        >
                                            <ListItemText
                                                primary={
                                                    <Typography noWrap color="textPrimary">{data.description}</Typography>
                                                }
                                                secondary={
                                                    t('general.date') + ': ' + formatDate(date)
                                                    + (data.startTime ? (' ' + t('general.time') + ': ' + data.startTime + ' uur') : '')
                                                }
                                            />
                                            <ListItemIcon style={{ justifyContent: 'center' }}>
                                                {
                                                    differenceInDays <= 0 ?
                                                        <TimerOff fontSize="large" />
                                                        :
                                                        <Badge badgeContent={differenceInDays <= 5 ? differenceInDays : 0} color="secondary" >
                                                            <Schedule fontSize="large" />
                                                        </Badge>
                                                }
                                            </ListItemIcon>
                                        </ListItem>
                                    </React.Fragment>
                                )
                            })
                            : <ListItem><ListItemText secondary={t('dashboard.planning.nodata')} /></ListItem>
                    }
                </List>}
            </Card>
        )
    }

    renderMessages() {
        const { user, t, classes, selected } = this.props;
        const { messageTabValue, messages, importantMessages, rights,getMessagesLoading } = this.state;
        const isUserBuyer = user.type === userAccountTypeConstants.buyer;
        return (
            <Card className={classes.card}>
                <CardHeader title={
                    <React.Fragment>
                        <Grid container alignItems="center">
                            <Grid item>
                                <Tabs value={messageTabValue} onChange={this.handleChangeMessagesTab} aria-label="message tabs">
                                    <Tab icon={<Chat />} id="message-tab-0" aria-controls="message-tabpanel-0" />
                                    <Tab icon={<Bookmarks />} id="message-tab-1" aria-controls="message-tabpanel-1" />
                                </Tabs>
                            </Grid>
                            <Grid item className={classes.grow}>
                                <Typography variant="h6">{t('dashboard.messages.title')}</Typography>
                            </Grid>
                        </Grid>
                    </React.Fragment>
                } className={classes.cardHeaderMessages} />
                {getMessagesLoading?<div className={classes.loadingData}><CircularProgress size={25} /></div>:
                <>
                <Typography
                    component="div"
                    role="tabpanel"
                    hidden={messageTabValue !== 0}
                    id={`message-tabpanel-0`}
                    aria-labelledby={`message-tab-0`}
                >
                    <List className={classes.list}>
                        {
                            messages.length > 0 ?
                                messages.map((chat, index) => {
                                    const chatTitle = this.getChatSubTitle(chat);
                                    const chatTitleInitial = chatTitle && chatTitle.length > 0
                                        ?
                                        (
                                            isUserBuyer ? chatTitle[0] :
                                                (
                                                    chatTitle.length <= 0 ?
                                                        chatTitle :
                                                        chatTitle.substr(chatTitle.length - 3)
                                                )
                                        ) : '-';
                                    return (
                                        <React.Fragment key={index}>
                                            {index !== 0 && <Divider component="li" />}
                                            <ListItem className={classes.chatListItem} title={chatTitle} button component={Link}
                                                to={selected && {
                                                    pathname: '/object/' + selected.buildingNoIntern + '/berichten',
                                                    state: {
                                                        selectedChatId: chat.chatId
                                                    }
                                                }}
                                            >
                                                <ListItemAvatar>
                                                    <Avatar>
                                                        {chatTitleInitial}
                                                    </Avatar>
                                                </ListItemAvatar>
                                                <ListItemText
                                                    primary={
                                                        <Typography noWrap color="textPrimary">{chatTitle}</Typography>
                                                    }
                                                    secondary={
                                                        <Typography variant="body2" color="textSecondary" noWrap>
                                                            {chat.hasAttachment && <AttachFile fontSize="small" style={{ marginLeft: -5 }} />}
                                                            {
                                                                (
                                                                    chat.lastChatMessagePartialText ?
                                                                        <React.Fragment>
                                                                            {
                                                                                chat.isSender !== true
                                                                                &&
                                                                                <React.Fragment>
                                                                                    {
                                                                                        chat.senderName === null ?
                                                                                            <React.Fragment><InfoOutlined style={{ marginTop: '-3px' }} fontSize="small" />&nbsp;</React.Fragment>
                                                                                            :
                                                                                            <React.Fragment>{chat.senderName}:&nbsp;</React.Fragment>
                                                                                    }
                                                                                </React.Fragment>
                                                                            }
                                                                            {md2plaintext(chat.lastChatMessagePartialText)}
                                                                        </React.Fragment>
                                                                        :
                                                                        '...'
                                                                )
                                                            }
                                                        </Typography>
                                                    }
                                                    secondaryTypographyProps={{ component: "div" }}
                                                />
                                                <Typography variant="caption" color="textPrimary" className={classes.chatListDate}>{getDateText(new Date(chat.dateTime))}</Typography>
                                                {
                                                    chat.unreadMessagesCount > 0 &&
                                                    <Badge className={classes.chatListCount} badgeContent={chat.unreadMessagesCount} color="primary" ><span></span></Badge>
                                                }
                                            </ListItem>
                                        </React.Fragment>
                                    )
                                })
                                : <ListItem><ListItemText secondary={t('dashboard.messages.nodata')} /></ListItem>
                        }
                    </List>
                </Typography>

                <Typography
                    component="div"
                    role="tabpanel"
                    hidden={messageTabValue !== 1}
                    id={`message-tabpanel-1`}
                    aria-labelledby={`message-tab-1`}
                >
                    <List className={classes.list}>
                        {
                            importantMessages.map((chat, indexCat) => (
                                chat.messages.length > 0 &&
                                <React.Fragment key={indexCat}>
                                    <Divider component="li" />
                                    <ListItem className={classes.importantChatHeader}>
                                        <ListItemText primary={
                                            <React.Fragment>
                                                <Typography noWrap>{this.getChatSubTitle(chat)}</Typography>
                                            </React.Fragment>
                                        } />
                                    </ListItem>
                                    <List component="div" disablePadding>
                                        {
                                            chat.messages.map((message, indexMessage) => (
                                                <React.Fragment key={indexMessage}>
                                                    <Divider component="li" />
                                                    <ListItem button className={classes.importantChatItem} component={Link}
                                                        to={selected && {
                                                            pathname: '/object/' + selected.buildingNoIntern + '/berichten',
                                                            state: {
                                                                selectedChatId: chat.chatId
                                                            }
                                                        }}>
                                                        <ListItemText primary={
                                                            <Typography variant="body2" color="textPrimary">
                                                                {message.isFile && <AttachFile fontSize="small" style={{ marginLeft: -5 }} />}
                                                                {md2plaintext(message.message)}</Typography>
                                                        }
                                                        />
                                                        <ListItemSecondaryAction>
                                                            <Grid container direction="column" alignItems="flex-end">
                                                                <Typography color="textPrimary" variant="caption" className={classes.impChatListDate}>{getDateText(new Date(message.dateTime))}</Typography>
                                                                <IconButton disabled={!rights['selected.object.write']} edge="end" aria-label="important" size="small" onClick={() => this.handleUnmarkImportantChatMessage(message, chat.chatId)}>
                                                                    <Bookmark color="primary" />
                                                                </IconButton>
                                                            </Grid>
                                                        </ListItemSecondaryAction>
                                                    </ListItem>
                                                </React.Fragment>
                                            ))
                                        }
                                    </List>
                                </React.Fragment>
                            ))
                        }
                    </List>
                </Typography>
                </>
                }
            </Card>
        )
    }

    renderNews() {
        const { user, t, classes } = this.props;
        const { getNewsLoading } = this.state;

        return (
            <Card className={classes.card}>
                <CardHeader title={
                    <Typography variant="h6">{t('dashboard.news.title')}</Typography>
                } avatar={<LibraryBooks />} className={classes.cardHeader} />
                {getNewsLoading?<div className={classes.loadingData}><CircularProgress size={25} /></div>:
                <List className={classes.list}>
                    {
                        this.state.news.length > 0 ?
                            this.state.news.map((data, index) => (
                                <React.Fragment key={index}>
                                    {index !== 0 && <Divider component="li" />}
                                    <ListItem title={data.description} button onClick={() => this.handleNewsDialogOpen(index)}>
                                        <ListItemText primary={
                                            <Typography noWrap>{data.description}</Typography>
                                        } secondary={t('general.date') + ': ' + formatDate(new Date(data.date))} />
                                    </ListItem>
                                </React.Fragment>
                            ))
                            : <ListItem><ListItemText secondary={t('dashboard.news.nodata')} /></ListItem>
                    }
                </List>}
            </Card>
        )
    }

    renderBuyers() {
        const { user, t, classes } = this.props;
        const { buyer,getBuyersLoading } = this.state;
        return (
            <Card className={classes.card}>
                <CardHeader title={
                    <Typography variant="h6">{t('Kopers info')}</Typography>
                } avatar={<People />} className={classes.cardHeader} />
                {getBuyersLoading?<div className={classes.loadingData}><CircularProgress size={25} /></div>:
                <div className={classes.list}>
                    {
                        buyer && buyer.type === 0 &&
                        <Table>
                            <TableBody>
                                <TableRow hover>
                                    <TableCell component="th" scope="row">
                                        <strong>{t('Koper 1') + ':'}</strong>
                                    </TableCell>
                                    <TableCell component="th" scope="row">
                                        {
                                            !!buyer.p1.loginId &&
                                            <Tooltip title={t("Bekijken als koper/huurder")}>
                                                <IconButton
                                                    color="primary"
                                                    aria-label="new Action"
                                                    component="span"
                                                    edge="start"
                                                    style={{ margin: -16, float: 'right' }}
                                                    onClick={() => window.open('/viewasbuyer/' + buyer.p1.loginId)}
                                                >
                                                    <Airplay />
                                                </IconButton>
                                            </Tooltip>
                                        }
                                    </TableCell>
                                </TableRow>
                                <TableRow hover>
                                    <TableCell component="th" scope="row">
                                        {t('Voornaam') + ':'}
                                    </TableCell>
                                    <TableCell>{buyer.p1.firstName}</TableCell>
                                </TableRow>
                                <TableRow hover>
                                    <TableCell component="th" scope="row">
                                        {t('Achternaam') + ':'}
                                    </TableCell>
                                    <TableCell>{buyer.p1.lastName}</TableCell>
                                </TableRow>
                                {
                                    buyer.p1.telephonePrivate &&
                                    <TableRow hover>
                                        <TableCell component="th" scope="row">
                                            {t('Telefoon prive') + ':'}
                                        </TableCell>
                                        <TableCell>{buyer.p1.telephonePrivate}</TableCell>
                                    </TableRow>
                                }
                                {
                                    buyer.p1.mobile &&
                                    <TableRow hover>
                                        <TableCell component="th" scope="row">
                                            {t('Mobiel') + ':'}
                                        </TableCell>
                                        <TableCell>{buyer.p1.mobile}</TableCell>
                                    </TableRow>
                                }
                                {
                                    buyer.p1.telephoneWork &&
                                    <TableRow hover>
                                        <TableCell component="th" scope="row">
                                            {t('Telefoon werk') + ':'}
                                        </TableCell>
                                        <TableCell>{buyer.p1.telephoneWork}</TableCell>
                                    </TableRow>
                                }
                                {
                                    buyer.p1.emailPrivate &&
                                    <TableRow hover>
                                        <TableCell component="th" scope="row">
                                            {t('E-mail prive') + ':'}
                                        </TableCell>
                                        <TableCell>{buyer.p1.emailPrivate}</TableCell>
                                    </TableRow>
                                }
                                {
                                    buyer.p1.emailWork &&
                                    <TableRow hover>
                                        <TableCell component="th" scope="row">
                                            {t('E-mail werk') + ':'}
                                        </TableCell>
                                        <TableCell>{buyer.p1.emailWork}</TableCell>
                                    </TableRow>
                                }
                                {
                                    buyer.p2 &&
                                    <React.Fragment>
                                        <TableRow hover>
                                            <TableCell component="th" scope="row">
                                                <strong>{t('Koper 2') + ':'}</strong>
                                            </TableCell>
                                            <TableCell component="th" scope="row">
                                                {
                                                    !!buyer.p2.loginId &&
                                                    <Tooltip title={t("Bekijken als koper/huurder")}>
                                                        <IconButton
                                                            color="primary"
                                                            aria-label="new Action"
                                                            component="span"
                                                            edge="start"
                                                            style={{ margin: -16, float: 'right' }}
                                                            onClick={() => window.open('/viewasbuyer/' + buyer.p2.loginId)}
                                                        >
                                                            <Airplay />
                                                        </IconButton>
                                                    </Tooltip>
                                                }
                                            </TableCell>
                                        </TableRow>
                                        <TableRow hover>
                                            <TableCell component="th" scope="row">
                                                {t('Voornaam') + ':'}
                                            </TableCell>
                                            <TableCell>{buyer.p2.firstName}</TableCell>
                                        </TableRow>
                                        <TableRow hover>
                                            <TableCell component="th" scope="row">
                                                {t('Achternaam') + ':'}
                                            </TableCell>
                                            <TableCell>{buyer.p2.lastName}</TableCell>
                                        </TableRow>
                                        {
                                            buyer.p2.telephonePrivate &&
                                            <TableRow hover>
                                                <TableCell component="th" scope="row">
                                                    {t('Telefoon prive') + ':'}
                                                </TableCell>
                                                <TableCell>{buyer.p2.telephonePrivate}</TableCell>
                                            </TableRow>
                                        }
                                        {
                                            buyer.p2.mobile &&
                                            <TableRow hover>
                                                <TableCell component="th" scope="row">
                                                    {t('Mobiel') + ':'}
                                                </TableCell>
                                                <TableCell>{buyer.p2.mobile}</TableCell>
                                            </TableRow>
                                        }
                                        {
                                            buyer.p2.telephoneWork &&
                                            <TableRow hover>
                                                <TableCell component="th" scope="row">
                                                    {t('Telefoon werk') + ':'}
                                                </TableCell>
                                                <TableCell>{buyer.p2.telephoneWork}</TableCell>
                                            </TableRow>
                                        }
                                        {
                                            buyer.p2.emailPrivate &&
                                            <TableRow hover>
                                                <TableCell component="th" scope="row">
                                                    {t('E-mail prive') + ':'}
                                                </TableCell>
                                                <TableCell>{buyer.p2.emailPrivate}</TableCell>
                                            </TableRow>
                                        }
                                        {
                                            buyer.p2.emailWork &&
                                            <TableRow hover>
                                                <TableCell component="th" scope="row">
                                                    {t('E-mail werk') + ':'}
                                                </TableCell>
                                                <TableCell>{buyer.p2.emailWork}</TableCell>
                                            </TableRow>
                                        }
                                    </React.Fragment>
                                }
                            </TableBody>
                        </Table>
                    }
                    {
                        buyer && buyer.type === 1 &&
                        <Table>
                            <TableBody>
                                <TableRow hover>
                                    <TableCell component="th" scope="row" colspan={2}>
                                        <Grid container alignItems="center">
                                            <Grid item className={classes.grow}>
                                                <div className={classes.thumbnail} style={{ backgroundImage: 'url(/api/organisation/GetOrganisationLogo/' + buyer.org.organisatonId + ')' }} />
                                            </Grid>
                                            <Grid item>
                                                {
                                                    !!buyer.org.loginId &&
                                                    <Tooltip title={t("Bekijken als koper/huurder")}>
                                                        <IconButton
                                                            color="primary"
                                                            aria-label="new Action"
                                                            component="span"
                                                            edge="start"
                                                            style={{ margin: -16, float: 'right' }}
                                                            onClick={() => window.open('/viewasbuyer/' + buyer.org.loginId)}
                                                        >
                                                            <Airplay />
                                                        </IconButton>
                                                    </Tooltip>
                                                }
                                            </Grid>
                                        </Grid>
                                    </TableCell>
                                </TableRow>
                                <TableRow hover>
                                    <TableCell component="th" scope="row">
                                        {t('Organisatie') + ':'}
                                    </TableCell>
                                    <TableCell>{buyer.org.name && buyer.org.name}</TableCell>
                                </TableRow>
                                {
                                    buyer.org.telephone &&
                                    <TableRow hover>
                                        <TableCell component="th" scope="row">
                                            {t('Telefoon') + ':'}
                                        </TableCell>
                                        <TableCell>{buyer.org.telephone}</TableCell>
                                    </TableRow>
                                }
                                {
                                    buyer.org.email &&
                                    <TableRow hover>
                                        <TableCell component="th" scope="row">
                                            {t('Email') + ':'}
                                        </TableCell>
                                        <TableCell>{buyer.org.email}</TableCell>
                                    </TableRow>
                                }
                                {
                                    buyer.org.relationName &&
                                    <TableRow hover>
                                        <TableCell component="th" scope="row">
                                            {t('Contactpersoon') + ':'}
                                        </TableCell>
                                        <TableCell>{buyer.org.relationName}</TableCell>
                                    </TableRow>
                                }
                                {
                                    buyer.org.relationTelephone &&
                                    <TableRow hover>
                                        <TableCell component="th" scope="row">
                                            {t('Telefoon') + ':'}
                                        </TableCell>
                                        <TableCell>{buyer.org.relationTelephone}</TableCell>
                                    </TableRow>
                                }
                                {
                                    buyer.org.relationMobile &&
                                    <TableRow hover>
                                        <TableCell component="th" scope="row">
                                            {t('Mobiel') + ':'}
                                        </TableCell>
                                        <TableCell>{buyer.org.relationMobile}</TableCell>
                                    </TableRow>
                                }
                                {
                                    buyer.org.relationEmail &&
                                    <TableRow hover>
                                        <TableCell component="th" scope="row">
                                            {t('E-mail zakelijk') + ':'}
                                        </TableCell>
                                        <TableCell>{buyer.org.relationEmail}</TableCell>
                                    </TableRow>
                                }
                            </TableBody>
                        </Table>
                    }
                </div>}
            </Card>
        )
    }

    a11yProps(index) {
        return {
            id: `simple-tab-${index}`,
            'aria-controls': `simple-tabpanel-${index}`,
        };
    }

    getCountOfOptions(groupedItem) {
        return groupedItem ? groupedItem.reduce((prev, next) => prev + next.options.length, 0) : 0;
    }

    renderQuotedGroupedOptions(groupedQuotations) {
        const { t, classes } = this.props;

        return (
            groupedQuotations &&
            groupedQuotations.map((quote, quoteIndex) => (
                <React.Fragment key={quoteIndex}>
                    {
                        quoteIndex > 0 &&
                        <React.Fragment>
                            <br />
                            <Divider />
                            <br />
                        </React.Fragment>
                    }
                    <Grid container>
                        <Typography paragraph className={classes.grow}>{t('Offerte') + ' - ' + quote.quoteNo}</Typography>
                        <Typography>{quote.closingDate && (t('Sluitingsdatum') + ': ' + formatDate(new Date(quote.closingDate)))}</Typography>
                    </Grid>
                    {
                        this.renderGroupedOptions(quote.options)
                    }
                </React.Fragment>
            ))
        )
    }

    renderGroupedOptions(options) {
        var headerText = '';
        return (
            <Grid container spacing={2}>
                {
                    options && options.map((option, indexOption) => {
                        let currentHeader = option.category + ' - ' + option.header;
                        if (headerText !== currentHeader) {
                            headerText = currentHeader;
                        }
                        else {
                            currentHeader = '';
                        }
                        return (
                            <React.Fragment key={indexOption}>
                                {
                                    currentHeader !== '' &&
                                    <Grid item xs={12}>
                                        <Typography variant="body2">{currentHeader}</Typography>
                                    </Grid>
                                }
                                <Grid container item xs={12} alignItems="flex-start">
                                    <Grid item xs={2} md={1}>
                                        <Typography variant="body2">
                                            <NumberFormat
                                                displayType="text"
                                                decimalScale={option.unitDecimalPlaces}
                                                fixedDecimalScale={true}
                                                thousandSeparator="."
                                                decimalSeparator=","
                                                value={option.quantity}
                                                suffix="&nbsp; "
                                            />
                                            {option.unit}
                                        </Typography>
                                    </Grid>
                                    <Grid item xs={7} md={9}>
                                        <Typography variant="body2" component="div" >{option.optionNo + ' - ' + option.description}</Typography>
                                        <Typography variant="body2" component="div" color="textSecondary">{nl2br(option.commercialDescription)}</Typography>
                                        <Typography variant="body2" component="div" color="textSecondary">
                                            <FormatQuote fontSize="small" />
                                            {option.additionalDescription}
                                        </Typography>
                                    </Grid>
                                    <Grid item container xs={3} md={2} alignItems="center" justify="flex-end">
                                        <Grid item xs={12} sm md={6}>
                                            <Typography variant="body2" component="div" align="right">
                                                {
                                                    (!option.salesPriceEstimated && !option.salesPriceToBeDetermined) ?
                                                        <NumberFormat
                                                            prefix="&euro;&nbsp;"
                                                            displayType="text"
                                                            decimalScale={2}
                                                            fixedDecimalScale={true}
                                                            thousandSeparator="."
                                                            decimalSeparator=","
                                                            value={option.quantity * option.salesPriceInclVAT}
                                                        />
                                                        :
                                                        option.salesPriceInclVAT_Text
                                                }
                                            </Typography>
                                        </Grid>
                                    </Grid>
                                </Grid>
                            </React.Fragment>
                        )
                    })
                }
            </Grid>
        )
    }

    renderOptions() {
        const { user, t, classes } = this.props;
        const { optionTabValue, groupedOptions,getOptionsLoading} = this.state


        return (
            <Card className={classes.card}>
                <CardHeader title={
                    <Tabs value={optionTabValue} onChange={this.handleChangeOptionsTab} aria-label="simple tabs example" variant="scrollable">
                        <Tab label={t('Opties')} {...this.a11yProps(0)} />
                        <Tab icon={
                            <Badge badgeContent={groupedOptions ? groupedOptions.availableIndividualOptions.length : 0} color="secondary">
                                <AssignmentInd />
                            </Badge>
                        } {...this.a11yProps(1)} />
                        <Tab icon={
                            <Badge badgeContent={groupedOptions ? groupedOptions.optionsInShoppingCart.length : 0} color="secondary">
                                <ShoppingBasket />
                            </Badge>
                        } {...this.a11yProps(2)} />
                        <Tab icon={
                            <Badge badgeContent={groupedOptions ? groupedOptions.requestedToBeJudged.length : 0} color="secondary">
                                {<LocalOfferOutlined />}
                            </Badge>
                        } {...this.a11yProps(3)} />
                        <Tab icon={
                            <Badge badgeContent={groupedOptions ? groupedOptions.availableQuotations.length : 0} color="secondary">
                                <LocalOffer />
                            </Badge>
                        } {...this.a11yProps(4)} />
                        <Tab icon={
                            <Badge badgeContent={groupedOptions ? groupedOptions.orderedOnlineButNotSentToBeSigned.length : 0} color="secondary">
                                <AssignmentLate />
                            </Badge>
                        } {...this.a11yProps(5)} />
                        <Tab icon={
                            <Badge badgeContent={groupedOptions ? groupedOptions.orderedOnlineAndSentToBeSigned.length : 0} color="secondary">
                                <CreateOutlined />
                            </Badge>
                        } {...this.a11yProps(6)} />
                        <Tab icon={
                            <Badge badgeContent={groupedOptions ? groupedOptions.signedToBeReviewed.length : 0} color="secondary">
                                <Create />
                            </Badge>
                        } {...this.a11yProps(7)} />
                        <Tab icon={
                            <Badge badgeContent={groupedOptions ? groupedOptions.definite.length : 0} color="secondary">
                                <AssignmentTurnedIn />
                            </Badge>
                        } {...this.a11yProps(8)} />
                        <Tab icon={
                            <Badge badgeContent={groupedOptions ? groupedOptions.cancelled.length : 0} color="secondary">
                                <Block />
                            </Badge>
                        } {...this.a11yProps(9)} />
                    </Tabs>
                } className={classes.cardHeaderOpties} />
                {getOptionsLoading?<div className={classes.loadingData}><CircularProgress size={25} /></div>:
                <div className={classes.list}>
                    <Typography
                        component="div"
                        role="tabpanel"
                        hidden={optionTabValue !== 0}
                        id={`simple-tabpanel-0`}
                        aria-labelledby={`simple-tab-0`}
                    >
                        <Box p={3}>
                            <Table size="small">
                                <TableBody>
                                    <TableRow hover onClick={e => this.handleChangeOptionsTab(e, 1)}>
                                        <TableCell component="th" scope="row">
                                            <AssignmentInd color="action" />
                                            &nbsp;&nbsp;
                                            {t('Openstaande individuele opties') + ':'}
                                        </TableCell>
                                        <TableCell>{groupedOptions ? groupedOptions.availableIndividualOptions.length : 0}</TableCell>
                                    </TableRow>
                                    <TableRow hover onClick={e => this.handleChangeOptionsTab(e, 2)}>
                                        <TableCell component="th" scope="row">
                                            <ShoppingBasket color="action" />
                                            &nbsp;&nbsp;
                                            {t('Mijn voorlopige opties') + ':'}
                                        </TableCell>
                                        <TableCell>{groupedOptions ? groupedOptions.optionsInShoppingCart.length : 0}</TableCell>
                                    </TableRow>
                                    <TableRow hover onClick={e => this.handleChangeOptionsTab(e, 3)}>
                                        <TableCell component="th" scope="row">
                                            <LocalOfferOutlined color="action" />
                                            &nbsp;&nbsp;
                                            {t('Aangevraagde opties') + ':'}
                                        </TableCell>
                                        <TableCell>{groupedOptions ? groupedOptions.requestedToBeJudged.length : 0}</TableCell>
                                    </TableRow>
                                    <TableRow hover onClick={e => this.handleChangeOptionsTab(e, 4)}>
                                        <TableCell component="th" scope="row">
                                            <LocalOffer color="action" />
                                            &nbsp;&nbsp;
                                            {t('Openstaande offertes') + ':'}
                                        </TableCell>
                                        <TableCell>{groupedOptions ? groupedOptions.availableQuotations.length : 0}</TableCell>
                                    </TableRow>
                                    <TableRow hover onClick={e => this.handleChangeOptionsTab(e, 5)}>
                                        <TableCell component="th" scope="row">
                                            <AssignmentLate color="action" />
                                            &nbsp;&nbsp;
                                            {t('Besteld, hangt in proces') + ':'}
                                        </TableCell>
                                        <TableCell>{groupedOptions ? groupedOptions.orderedOnlineButNotSentToBeSigned.length : 0}</TableCell>
                                    </TableRow>
                                    <TableRow hover onClick={e => this.handleChangeOptionsTab(e, 6)}>
                                        <TableCell component="th" scope="row">
                                            <CreateOutlined color="action" />
                                            &nbsp;&nbsp;
                                            {t('Digitaal te ondertekenen') + ':'}
                                        </TableCell>
                                        <TableCell>{groupedOptions ? groupedOptions.orderedOnlineAndSentToBeSigned.length : 0}</TableCell>
                                    </TableRow>
                                    <TableRow hover onClick={e => this.handleChangeOptionsTab(e, 7)}>
                                        <TableCell component="th" scope="row">
                                            <Create color="action" />
                                            &nbsp;&nbsp;
                                            {t('Handtekening beoordelen') + ':'}
                                        </TableCell>
                                        <TableCell>{groupedOptions ? groupedOptions.signedToBeReviewed.length : 0}</TableCell>
                                    </TableRow>
                                    <TableRow hover onClick={e => this.handleChangeOptionsTab(e, 8)}>
                                        <TableCell component="th" scope="row">
                                            <AssignmentTurnedIn color="action" />
                                            &nbsp;&nbsp;
                                            {t('Definitieve opties') + ':'}
                                        </TableCell>
                                        <TableCell>{groupedOptions ? groupedOptions.definite.length : 0}</TableCell>
                                    </TableRow>
                                    <TableRow hover onClick={e => this.handleChangeOptionsTab(e, 9)}>
                                        <TableCell component="th" scope="row">
                                            <Block color="action" />
                                            &nbsp;&nbsp;
                                            {t('Afgewezen opties') + ':'}
                                        </TableCell>
                                        <TableCell>{groupedOptions ? groupedOptions.cancelled.length : 0}</TableCell>
                                    </TableRow>
                                </TableBody>
                            </Table>
                        </Box>
                    </Typography>
                    <Typography
                        component="div"
                        role="tabpanel"
                        hidden={optionTabValue !== 1}
                        id={`simple-tabpanel-1`}
                        aria-labelledby={`simple-tab-1`}
                    >
                        <Box p={3}>{this.renderGroupedOptions(groupedOptions && groupedOptions.availableIndividualOptions)}</Box>
                    </Typography>
                    <Typography
                        component="div"
                        role="tabpanel"
                        hidden={optionTabValue !== 2}
                        id={`simple-tabpanel-2`}
                        aria-labelledby={`simple-tab-2`}
                    >
                        <Box p={3}>{this.renderGroupedOptions(groupedOptions && groupedOptions.optionsInShoppingCart)}</Box>
                    </Typography>
                    <Typography
                        component="div"
                        role="tabpanel"
                        hidden={optionTabValue !== 3}
                        id={`simple-tabpanel-3`}
                        aria-labelledby={`simple-tab-3`}
                    >
                        <Box p={3}>{this.renderGroupedOptions(groupedOptions && groupedOptions.requestedToBeJudged)}</Box>
                    </Typography>
                    <Typography
                        component="div"
                        role="tabpanel"
                        hidden={optionTabValue !== 4}
                        id={`simple-tabpanel-4`}
                        aria-labelledby={`simple-tab-4`}
                    >
                        <Box p={3}>{this.renderQuotedGroupedOptions(groupedOptions && groupedOptions.availableQuotations)}</Box>
                    </Typography>
                    <Typography
                        component="div"
                        role="tabpanel"
                        hidden={optionTabValue !== 5}
                        id={`simple-tabpanel-5`}
                        aria-labelledby={`simple-tab-5`}
                    >
                        <Box p={3}>{this.renderQuotedGroupedOptions(groupedOptions && groupedOptions.orderedOnlineButNotSentToBeSigned)}</Box>
                    </Typography>
                    <Typography
                        component="div"
                        role="tabpanel"
                        hidden={optionTabValue !== 6}
                        id={`simple-tabpanel-6`}
                        aria-labelledby={`simple-tab-6`}
                    >
                        <Box p={3}>{this.renderQuotedGroupedOptions(groupedOptions && groupedOptions.orderedOnlineAndSentToBeSigned)}</Box>
                    </Typography>
                    <Typography
                        component="div"
                        role="tabpanel"
                        hidden={optionTabValue !== 7}
                        id={`simple-tabpanel-7`}
                        aria-labelledby={`simple-tab-7`}
                    >
                        <Box p={3}>{this.renderQuotedGroupedOptions(groupedOptions && groupedOptions.signedToBeReviewed)}</Box>
                    </Typography>
                    <Typography
                        component="div"
                        role="tabpanel"
                        hidden={optionTabValue !== 8}
                        id={`simple-tabpanel-8`}
                        aria-labelledby={`simple-tab-8`}
                    >
                        <Box p={3}>{this.renderGroupedOptions(groupedOptions && groupedOptions.definite)}</Box>
                    </Typography>
                    <Typography
                        component="div"
                        role="tabpanel"
                        hidden={optionTabValue !== 9}
                        id={`simple-tabpanel-9`}
                        aria-labelledby={`simple-tab-9`}
                    >
                        <Box p={3}>{this.renderQuotedGroupedOptions(groupedOptions && groupedOptions.cancelled)}</Box>
                    </Typography>
                </div>}
            </Card>
        )
    }

    renderDocuments() {
        const { user, selected, t, classes } = this.props;
        const { documentHeaders, uploading, rights,getDocumentsLoading } = this.state;

        return (
            <Card className={classes.card}>
                <CardHeader title={
                    <Typography variant="h6">{t('Documenten')}</Typography>
                }
                    avatar={<Description />} className={classes.cardHeader}
                    action={
                        <React.Fragment>
                            <input disabled={!rights['selected.object.write']} accept="*" style={{ display: 'none' }} id="icon-button-file" type="file" onChange={this.uploadDocument} />
                            <label htmlFor={rights['selected.object.write'] && "icon-button-file"} style={{ margin: 0 }}>
                                {
                                    uploading ?
                                        <CircularProgress color="inherit" />
                                        :
                                        <IconButton
                                            color="inherit"
                                            aria-label="upload"
                                            component="span"
                                            disabled={!selected || !rights['selected.object.write']}
                                        >
                                            <AttachFile />
                                        </IconButton>
                                }
                            </label>
                        </React.Fragment>
                    }
                />
                {getDocumentsLoading?<div className={classes.loadingData}><CircularProgress size={25} /></div>:
                <div className={classes.list}>
                    {
                        documentHeaders.map((header, indexHeader) => (
                            <ExpansionPanel key={indexHeader} className={classes.expansionPanel} defaultExpanded={true}>
                                <ExpansionPanelSummary
                                    expandIcon={<ArrowDropDown />}
                                    aria-controls={'panel-cat-' + indexHeader + '-content'}
                                    id={'panel-cat-' + indexHeader + '-header'} className={classes.documentHeading}
                                >
                                    <Typography className={classes.bold}>{header.header}</Typography>
                                </ExpansionPanelSummary>
                                <ExpansionPanelDetails className={classes.documentHeaderDetails}>
                                    <Grid container>
                                        {
                                            header.attachments.map((document, index) => (
                                                <Grid key={index} container item xs={12} alignItems="center" justify="flex-end">
                                                    <Typography className={classes.grow} noWrap>{document.description}</Typography>
                                                    <Grid item>
                                                        <Typography>{document.dateTime && formatDate(new Date(document.dateTime))}</Typography>
                                                    </Grid>
                                                    <Grid item>
                                                        <IconButton href={webApiUrl + 'api/home/GetAttachment/' + encodeURI(document.id)} download>
                                                            <CloudDownload />
                                                        </IconButton>
                                                    </Grid>
                                                </Grid>
                                            ))
                                        }
                                    </Grid>
                                </ExpansionPanelDetails>
                            </ExpansionPanel>
                        ))
                    }
                </div>}
            </Card>
        )
    }


    render() {
        const { user, t, classes, selected, buildings } = this.props;
        const { actions, actionIndex, news, newsIndex, newAction, actionSubmitting } = this.state;
        const openActionPopup = actionIndex >= 0 && actionIndex < actions.length;
        const selectedAction = openActionPopup ? actions[actionIndex] : null;
        if (selectedAction) {
            const building = buildings.find(x => x.buildingId === selectedAction.buildingId);
        }

        const openNewsPopup = newsIndex >= 0 && newsIndex < news.length;
        const selectedNews = openNewsPopup ? news[newsIndex] : null;


        return (
            <React.Fragment>
                <Box p={1} className={classes.fullHeight}>
                    <Grid container spacing={1} className={classes.fullHeight}>
                        <Grid item xs={12} sm={6} md={3} className={classes.cardContainer}>
                            {this.renderActions()}
                        </Grid>
                        <Grid item xs={12} sm={6} md={3} className={classes.cardContainer}>
                            {this.renderMessages()}
                        </Grid>
                        <Grid item xs={12} sm={6} md={3} className={classes.cardContainer}>
                            {this.renderPlannings()}
                        </Grid>
                        <Grid item xs={12} sm={6} md={3} className={classes.cardContainer}>
                            {this.renderBuyers()}
                        </Grid>
                        <Grid item xs={12} md={6} className={classes.cardContainer}>
                            {this.renderOptions()}
                        </Grid>
                        <Grid item xs={12} sm={6} md={3} className={classes.cardContainer}>
                            {this.renderDocuments()}
                        </Grid>
                        <Grid item xs={12} sm={6} md={3} className={classes.cardContainer}>
                            {this.renderNews()}
                        </Grid>
                    </Grid>
                </Box>
                {
                    selectedAction &&
                    <Dialog onClose={this.handleActionDialogClose} aria-labelledby="simple-dialog-title" open={openActionPopup} fullWidth={true} maxWidth="sm">
                        <DialogTitle id="simple-dialog-title" className={classes.dialogTitle} disableTypography>
                            <CardHeader id="transition-dialog-title"
                                title={
                                    <Typography variant="h6" noWrap>{selectedAction.description}</Typography>
                                }
                                action={
                                    <React.Fragment>
                                        <IconButton color="inherit" aria-label="previous" disabled={actionIndex <= 0} onClick={() => this.changeActionItemIndex(-1)}>
                                            <KeyboardArrowLeft />
                                        </IconButton>
                                        <IconButton color="inherit" aria-label="next" disabled={actionIndex >= (actions.length - 1)} onClick={() => this.changeActionItemIndex(1)}>
                                            <KeyboardArrowRight />
                                        </IconButton>
                                        <IconButton color="inherit" aria-label="close" onClick={this.handleActionDialogClose}>
                                            <Close />
                                        </IconButton>
                                    </React.Fragment>
                                } />
                        </DialogTitle>
                        <DialogContent className={classes.dialogContent}>
                            <Box p={2}>
                                <Typography paragraph>
                                    <strong>{t('general.date')}:&nbsp;</strong>
                                    {formatDate(new Date(selectedAction.actionDate))}
                                    <strong>&nbsp;{t('general.time')}:&nbsp;</strong>
                                    {formatTime(selectedAction.startTime) + ' uur'}
                                </Typography>
                                <Typography paragraph>
                                    <strong>{t('Selectie')}:&nbsp;</strong>
                                    {selectedAction.buildingNoExtern + '/' + selected.projectName}
                                </Typography>
                                <Typography>
                                    <strong>{t('Omschrijving')}:</strong>
                                </Typography>
                                <Typography>
                                    {selectedAction.descriptionExtended}
                                </Typography>
                            </Box>
                        </DialogContent>
                    </Dialog>
                }
                {
                    selectedNews &&
                    <Dialog onClose={this.handleNewsDialogClose} aria-labelledby="simple-dialog-title" open={openNewsPopup} fullWidth={true} maxWidth="sm">
                        <DialogTitle id="simple-dialog-title" className={classes.dialogTitle} disableTypography>
                            <CardHeader id="transition-dialog-title"
                                title={
                                    <Typography variant="h6" noWrap>{selectedNews.description}</Typography>
                                }
                                action={
                                    <React.Fragment>
                                        <IconButton color="inherit" aria-label="previous" disabled={newsIndex <= 0} onClick={() => this.changeNewsItemIndex(-1)}>
                                            <KeyboardArrowLeft />
                                        </IconButton>
                                        <IconButton color="inherit" aria-label="next" disabled={newsIndex >= (news.length - 1)} onClick={() => this.changeNewsItemIndex(1)}>
                                            <KeyboardArrowRight />
                                        </IconButton>
                                        <IconButton color="inherit" aria-label="close" onClick={this.handleNewsDialogClose}>
                                            <Close />
                                        </IconButton>
                                    </React.Fragment>
                                } />
                        </DialogTitle>
                        <DialogContent className={classes.dialogContent}>
                            {
                                selectedNews.hasImage === true &&
                                <CardMedia
                                    component="img"
                                    alt={selectedNews.description}
                                    className={classes.media}
                                    image={webApiUrl + 'api/home/getnewsimage/' + selectedNews.newsId}
                                    title={selectedNews.description}
                                />
                            }
                            <Box p={2}>
                                <Typography paragraph>
                                    <strong>{t('general.date')}:&nbsp;</strong>
                                    {formatDate(new Date(selectedNews.date))}
                                </Typography>
                                <div className={classes.divWithHtmlContent} dangerouslySetInnerHTML={{ __html: selectedNews.newsItem }} />
                            </Box>
                        </DialogContent>
                    </Dialog>
                }
                <Modal
                    aria-labelledby="transition-modal-title"
                    aria-describedby="transition-modal-description"
                    className={classes.modal}
                    open={newAction != undefined}
                    onClose={this.handleNewActionModalClose}
                    closeAfterTransition
                    BackdropComponent={Backdrop}
                    BackdropProps={{
                        timeout: 500,
                    }}
                >
                    <Fade in={newAction != undefined}>
                        <Card className={classes.modalCard}>
                            <CardHeader id="transition-modal-title" title={
                                <Typography variant="h6">{t('Actie toevoegen')}</Typography>
                            } className={classes.modalCardHeader} />
                            <CardContent id="transition-modal-description">
                                {
                                    newAction &&
                                    <form noValidate onSubmit={this.handleModalActionSubmit} disabled={newAction.submitting}>
                                        <MuiPickersUtilsProvider utils={DateFnsUtils} locale={nlLocale}>
                                            <Grid container spacing={1} justify="space-around">
                                                <Grid item xs={12}>
                                                    <DateTimePicker
                                                        variant="inline"
                                                        margin="dense"
                                                        id="date-time-picker"
                                                        label={t('Startdatumtijd')}
                                                        format="dd/MM/yyyy HH:mm"
                                                        value={newAction.date}
                                                        onChange={(date) => this.handleModalDateChange(date)}
                                                        inputVariant="outlined"
                                                        autoOk
                                                        ampm={false}
                                                        fullWidth
                                                        required
                                                        error={newAction.submitted && !newAction.date}
                                                        disabled={newAction.submitting}
                                                    />
                                                </Grid>
                                                <Grid item xs={12}>
                                                    <TextField
                                                        id="outlined-behandelaar"
                                                        select
                                                        label="Select"
                                                        value={newAction.employeeId == undefined ? '' : newAction.employeeId}
                                                        onChange={this.handleModalChangeTextField('employeeId')}
                                                        SelectProps={{
                                                            MenuProps: {
                                                                className: classes.menu,
                                                            },
                                                        }}
                                                        margin="dense"
                                                        variant="outlined"
                                                        fullWidth
                                                        required
                                                        error={newAction.submitted && !newAction.employeeId}
                                                        disabled={newAction.submitting}
                                                    >
                                                        <MenuItem value=''><em>-</em></MenuItem>
                                                        {
                                                            this.state.employees && this.state.employees.length > 0 && this.state.employees.map((employee, index) => (
                                                                <MenuItem key={index} value={employee.id}>{employee.name}</MenuItem>
                                                            ))
                                                        }
                                                    </TextField>
                                                </Grid>
                                                <Grid item xs={12}>
                                                    <TextField
                                                        label={t('Onderwerp')}
                                                        value={newAction.description}
                                                        onChange={this.handleModalChangeTextField('description')}
                                                        margin="dense"
                                                        variant="outlined"
                                                        fullWidth
                                                        required
                                                        error={newAction.submitted && !newAction.description}
                                                        disabled={newAction.submitting}
                                                    />
                                                </Grid>
                                                <Grid item xs={12}>
                                                    <TextField
                                                        label={t('Uitgebreide omschrijving')}
                                                        value={newAction.descriptionExtended}
                                                        onChange={this.handleModalChangeTextField('descriptionExtended')}
                                                        margin="dense"
                                                        variant="outlined"
                                                        multiline
                                                        fullWidth
                                                        required
                                                        error={newAction.submitted && !newAction.descriptionExtended}
                                                        disabled={newAction.submitting}
                                                    />
                                                </Grid>
                                                <Grid container item xs={12} justify="flex-end">
                                                    <Button type="submit" color="primary" variant="contained" disabled={newAction.submitting}>{t('Toevoegen')}</Button>
                                                </Grid>
                                            </Grid>
                                        </MuiPickersUtilsProvider>
                                    </form>
                                }
                            </CardContent>
                        </Card>
                    </Fade>
                </Modal>
            </React.Fragment>
        );
    }
}

function mapStateToProps(state) {
    const { authentication, buildings } = state;
    const { user } = authentication;
    const { selected, all } = buildings;
    return {
        user,
        selected,
        buildings: all
    };
}

const connectedBuildingOverviewPage = connect(mapStateToProps)(withTranslation()(withStyles(styles)(BuildingOverviewPage)));
export { connectedBuildingOverviewPage as BuildingOverviewPage };