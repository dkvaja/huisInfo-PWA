import React from "react";
import ReactDOM from "react-dom";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import ReactResizeDetector from 'react-resize-detector';
import { Avatar, Container, Grid, Typography, Divider, Card, CardContent, CardHeader, List, ListItem, ListItemText, AppBar, IconButton, Toolbar, Slide, Box, ListItemAvatar, TextField, Chip, Hidden, withWidth, Collapse, Badge, Modal, Backdrop, Fade, FormControl, InputLabel, Select, MenuItem, Button, ListItemSecondaryAction, Input, InputAdornment, CircularProgress, CardMedia, CardActionArea, Menu, Tooltip, Icon } from "@material-ui/core";
import { withStyles, withTheme } from '@material-ui/core/styles';
import { ToggleButton } from "@material-ui/lab";
import { Schedule, Chat, LibraryBooks, Add, Search, Notifications, ArrowBack, Send, ExpandLess, ExpandMore, NotificationsNone, AssignmentOutlined, AttachFile, DeleteOutline, ChevronRight, Close, InsertDriveFile, CloudDownload, Reply, PhotoCamera, Edit, TextFormat, PriorityHigh, PostAdd, Create, Error, Bookmarks, Bookmark, BookmarkBorder, Info, InfoOutlined } from "@material-ui/icons";
import DateFnsUtils from '@date-io/date-fns';
import nlLocale from "date-fns/locale/nl";
import enLocale from "date-fns/locale/en-US";
import {
    MuiPickersUtilsProvider,
    DateTimePicker,
} from '@material-ui/pickers';
import { commonActions } from "../../_actions";
import { history, validateFile, getDateText, md2plaintext, formatTime, formatDate, authHeader, getNameInitials, downloadFile } from '../../_helpers';
//import './home.css';
import { withTranslation } from 'react-i18next';
import clsx from "clsx";
import { isWidthUp } from "@material-ui/core/withWidth";
import { userAccountTypeConstants } from "../../_constants";
import RichTextEditor from "./RichTextEditor";
import Markdown from "../../components/Markdown";
import StandardTextManager from "./StandardTextManager";
import EmojiSelector from "./EmojiSelector";

const colors = [
    '#35cd96', '#6bcbef', '#e542a3', '#91ab01', '#ffa97a',
    '#1f7aec', '#dfb610', '#029d00', '#8b7add', '#fe7c7f',
    '#ba33dc', '#59d368', '#b04632', '#fd85d4', '#8393ca',
    '#ff8f2c', '#3bdec3', '#b4876e', '#c90379', '#ef4b4f'
];

const { webApiUrl } = window.appConfig;

const styles = theme => ({
    grow: {
        flexGrow: 1
    },
    mainContainer: {
        paddingTop: theme.spacing(5),
        height: "100%",
        '&.fullwidth': {
            maxWidth: '100%',
            padding: theme.spacing(0)
        },
        [theme.breakpoints.down("sm")]: {
            padding: theme.spacing(0)
        }
    },
    chatContainer: {
        backgroundColor: theme.palette.background.paper,
        height: '100%',
        position: 'relative',
        overflow: 'hidden'
    },
    allChats: {
        position: 'relative',
        zIndex: 1,
        height: '100%'
    },
    slideRight: {
        backgroundColor: theme.palette.background.paper,
        position: 'absolute',
        zIndex: 2,
        height: '100%'
    },
    slideLeft: {
        backgroundColor: theme.palette.background.paper,
        position: 'absolute',
        zIndex: 2100,
        right: 0,
        height: '100%'
    },
    bold: {
        fontWeight: 'bold'
    },
    fullWidth: {
        width: '100%'
    },
    chatList: {
        maxHeight: 'calc(100% - 48px)',
        overflowX: 'hidden',
        overflowY: 'auto',
        width: '100%',
        padding: 0
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
            marginRight: '60px',
            marginTop: theme.spacing(.5),
            marginBottom: theme.spacing(.5),
            '& < *': {
                lineHeight: '1.4!important'
            }
        }
    },
    searchList: {
        maxHeight: 'calc(100% - 48px)',
        overflowX: 'hidden',
        overflowY: 'auto',
        width: '100%'
    },
    searchListItem: {
        paddingTop: 0,
        paddingBottom: 0,
        '& .MuiListItemText-root': {
            marginRight: '60px'
        }
    },
    searchMessagesList: {
        maxHeight: 'calc(100% - 48px)',
        overflowX: 'hidden',
        overflowY: 'auto',
        width: '100%'
    },
    searchMessagesListItem: {
        paddingTop: 0,
        paddingBottom: 0,
        '& .MuiListItemText-root': {
            marginRight: '60px'
        }
    },
    subjectList: {
        maxHeight: 'calc(100% - 48px)',
        overflow: 'auto',
        width: '100%'
    },
    subjectHeader: {
        backgroundColor: theme.palette.background.default,
    },
    importantChatsList: {
        maxHeight: 'calc(100% - 48px)',
        overflow: 'auto',
        width: '100%'
    },
    importantChatCategory: {
        fontWeight: 'bold',
        backgroundColor: theme.palette.background.default,
        paddingTop: 0,
        paddingBottom: 0
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
    importantChatBadge: {
        position: 'absolute',
        right: 48,
        top: 20
    },
    nested: {
        paddingLeft: theme.spacing(4),
    },
    chatBoxContainer: {
        backgroundColor: theme.palette.background.default,
        //backgroundImage: 'url(/content/images/background/email-pattern.png)',
        backgroundImage: 'url(/content/images/background/brickwall-bg.png)',
        height: '100%',
        [theme.breakpoints.down("sm")]: {
            position: 'absolute',
            zIndex: 2
        }
    },
    chatBoxTitle: {
        overflow: 'hidden',
        '& p': {
            lineHeight: 1.2
        }
    },
    chatBox: {
        overflowY: 'scroll',
        overflowX: 'hidden',
        flex: 1,
        padding: theme.spacing(2, 8),
        scrollbarWidth: 'thin',
        [theme.breakpoints.down("sm")]: {
            padding: theme.spacing(1, 1, 1, 5),
        }
    },
    chatItemsWrapper: {
        maxWidth: 790,
    },
    chatCardContainer: {
        marginTop: theme.spacing(-2)
    },
    chatItemAvatar: {
        position: 'relative',
        left: theme.spacing(-6),
        top: theme.spacing(1),
        height: 0,
        width: 0,
        [theme.breakpoints.down("sm")]: {
            left: theme.spacing(-4),
            top: theme.spacing(0),
        },
        '& > div': {
            fontSize: '1.1rem',
            [theme.breakpoints.down("sm")]: {
                fontSize: '.9rem',
                height: theme.spacing(3.5),
                width: theme.spacing(3.5),
            }
        }
    },
    chatCard: {
        borderLeftWidth: theme.spacing(0.5),
        borderLeftStyle: 'solid',
        borderLeftColor: 'transparent',
        boxShadow: '0 1px .5px rgba(0,0,0,.13)',
        minWidth: 120,
        transition: theme.transitions.create(
            ['background-color', 'color', 'opacity'],
            { duration: 1500 }
        ),
        '& .MuiCardContent-root': {
            padding: theme.spacing(1, 1.5, 0, 1),
            position: 'relative',
            wordWrap: 'break-word',
            '& .MuiCardHeader-root': {
                padding: 0
            }
        },
        '&.sentbyme': {
            backgroundColor: '#e3f2fd',
            '& .chatButtonsContainer': {
                backgroundColor: '#e3f2fd'
            }
        },
        '&.system': {
            backgroundColor: '#fff5c4',
            marginTop: theme.spacing(-1.5),
            '& .chatButtonsContainer': {
                backgroundColor: '#fff5c4'
            }
        },
        '&.focused': {
            backgroundColor: '#cfcfcf'
        },
        '& .chatButtonsContainer': {
            backgroundColor: '#fff',
            position: 'absolute',
            right: 0,
            top: 0,
            display: 'none'
        },
        '&:focus-within': {
            '& .chatButtonsContainer': {
                display: 'block'
            }
        },
        '&:focus': {
            '& .chatButtonsContainer': {
                display: 'block'
            }
        },
        '&:hover': {
            '& .chatButtonsContainer': {
                display: 'block'
            }
        }
    },
    chatCardImportantIcon: {
        margin: theme.spacing(1, -1.5),
        float: 'right',
        [theme.breakpoints.down("sm")]: {
            margin: theme.spacing(1, -1.25, 0, -1.75),
        }
    },
    chatCardInfoIcon: {
        zIndex: 1,
        margin: theme.spacing(0, 1, 0),
        color: theme.palette.primary.main
    },
    chatMedia: {
        width: 'auto',
        maxWidth: '100%',
        height: 150,
        cursor: 'pointer',
        [theme.breakpoints.up("md")]: {
            height: 200,
        }
    },
    quoteMessageCard: {
        width: '100%',
        backgroundColor: 'rgba(0,0,0,.05)',
        margin: theme.spacing(1, 0),
        boxShadow: 'none',
        borderLeftWidth: 4,
        borderLeftStyle: 'solid',
        '& .MuiCardActionArea-root': {
            display: 'flex',
            justifyContent: 'flex-start'
        },
        '& .MuiCardContent-root': {
            padding: theme.spacing(1, 2),
            flexGrow: 1
        },
        '& .MuiCardMedia-root': {
            width: 62,
            height: 62
        }
    },
    chatActions: {
        //backgroundColor: theme.palette.background.default,
        maxHeight: 'calc(100% - 64px)',
        padding: theme.spacing(1, 8, 0),
        overflowY: 'scroll',
        [theme.breakpoints.down("sm")]: {
            padding: theme.spacing(1, 1, 0),
        }
    },
    chatCommentBoxContainer: {
        maxWidth: 774,
        width: '100%',
        margin: 'auto',
        borderLeftWidth: theme.spacing(0.5),
        borderLeftStyle: 'solid',
        borderLeftColor: 'transparent',
        padding: theme.spacing(0.5, 1, 0.5, 0.5),
        backgroundColor: theme.palette.common.white,
        borderRadius: theme.shape.borderRadius,
        [theme.breakpoints.down("md")]: {
            maxWidth: '100%',
        },
        '&:focus-within': {
            borderBottomWidth: theme.spacing(0.25),
            borderBottomStyle: 'solid',
            borderBottomColor: theme.palette.primary.main,
            paddingBottom: theme.spacing(0.25)
        }
    },
    important: {
        borderLeftWidth: theme.spacing(0.5),
        borderLeftStyle: 'solid',
        borderLeftColor: theme.palette.secondary.main,
    },
    commentBoxImportantHeader: {
        fontWeight: 'bold',
        paddingLeft: theme.spacing(0.5)
    },
    chatActionBoxContainer: {
        padding: theme.spacing(0.5, 1),
        maxWidth: 774,
        margin: 'auto',
        '& > div': {
            marginRight: theme.spacing(1),
            '&:last-child': {
                marginRight: theme.spacing(0)
            }
        },
    },
    replyMessageCard: {
        width: 'calc(100% - 48px)',
        backgroundColor: 'rgba(0,0,0,.05)',
        display: 'flex',
        justifyContent: 'flex-start',
        alignItems: 'center',
        boxShadow: 'none',
        borderLeftWidth: 4,
        borderLeftStyle: 'solid',
        marginBottom: theme.spacing(0.5),
        '& .MuiCardContent-root': {
            padding: theme.spacing(1, 2),
            flexGrow: 1
        },
        '& .MuiCardMedia-root': {
            width: 62,
            height: 62
        }
    },
    chatField: {
        margin: 'auto'
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
    inputBoxSearch: {
        color: 'inherit',
        maxWidth: '100%',
        margin: '-3px 5px -5px 0',
        '&:before': {
            display: 'none'
        },
        '&:after': {
            display: 'none'
        },
        '& svg': {
            color: 'inherit'
        },
        flexGrow: 1
    },
    selectBoxSearch: {
        minWidth: 'initial',
        '& .MuiInputBase-root': {
            color: 'inherit',
            minWidth: 52,
            maxWidth: 60,
            margin: '-3px 0 -5px',
            textAlign: 'right',
            '&:before': {
                display: 'none'
            },
            '&:after': {
                display: 'none'
            },
            '& .MuiSelect-root': {
                paddingRight: 22
            },
            '& svg': {
                color: 'inherit'
            }
        }
    },
    textButton: {
        maxWidth: '100%',
        minWidth: 0,
        overflow: 'hidden',
        paddingY: 0,
        marginLeft: theme.spacing(-1)
    },
    appBarButton: {
        maxWidth: '100%',
        minWidth: 0,
        overflow: 'hidden',
        '&:hover': {
            color: theme.palette.grey[300]
        }
    },
    toolbarButton: {
        overflow: "hidden",
        textOverflow: "ellipsis",
        display: "block",
        width: "200px;"
    }
});

class MessagesPage extends React.Component {
    state = {
        chats: [],
        loading: false,
        chatItems: [],
        focusedChatMessageId: null,
        chatItemToReplyOn: null,
        chatParticipants: {},
        chatStartList: [],
        chatBoxOpen: false,
        openAddNew: false,
        comment: '',
        defaultComment: '',
        markCommentImportant: false,
        textToInsertInComment: '',
        richTextToInsertInComment: '',
        standardTexts: null,
        scrollToBottom: false,
        messagesContainerHeight: 0,
        employees: [],
        expandedList: [],
        openImportantChats: false,
        importantExpandedList: [],
        searchTerm: '',
        searchCategoryId: 'all',
        searchWithAttachment: false,
        searchResults: [],
        hasMoreSearchResults: false,
        searchTermMessages: '',
        searchMessagesWithAttachment: false,
        searchMessagesResults: [],
        hasMoreSearchMessagesResults: false,
        uploading: false,
        showRichTextEditorToolbar: false,
        retryUpdateChatCount: 0
    };

    componentDidMount() {
        const { location } = this.props;
        this.UpdateActionEmployess();
        this.UpdateChatStartList();
        this.UpdateChats(null, true);
        this.UpdateImportantChats();

        const openImportantChats = (location && location.state && location.state.showImportantMessages === true);

        var intervalId = setInterval(this.timer, 10000);
        // store intervalId in the state so it can be accessed later:
        this.setState({ intervalId: intervalId, openImportantChats });
    }

    componentWillReceiveProps(nextProps) {
        this.openChatMessage(nextProps);
    }

    componentWillUnmount() {
        // use intervalId from the state to clear the interval
        clearInterval(this.state.intervalId);
    }

    componentDidUpdate(prevProps, prevState) {
        if (!prevProps.selected || prevProps.selected.buildingId !== this.props.selected.buildingId) {
            this.UpdateActionEmployess();
            this.UpdateChatStartList();
            this.UpdateChats(null, true);
            this.UpdateImportantChats();
        }
        if (prevProps.selected && this.props.selected && prevProps.selected.projectId.toUpperCase() !== this.props.selected.projectId.toUpperCase()) {
            this.closeChatBox();
        }
        if (this.state.searchTerm.trim() !== prevState.searchTerm.trim() || this.state.searchCategoryId !== prevState.searchCategoryId || this.state.searchWithAttachment !== prevState.searchWithAttachment) {
            this.SearchChats();
        }
        if (this.state.searchTermMessages.trim() !== prevState.searchTermMessages.trim() || this.state.searchMessagesWithAttachment !== prevState.searchMessagesWithAttachment) {
            this.SearchChatMessages();
        }
        if (this.state.goToChatMessage === true) {
            this.setState({ goToChatMessage: false });
        }
        else {
            if (this.state.selected && (!prevState.selected || prevState.selected.chatId !== this.state.selected.chatId)) {
                this.UpdateChatItems();
            }
            if (this.state.selectedHasUpdate === true || (this.state.hasNewer === true && this.state.isScrollPositionBottom)) {
                this.UpdateChatItems(true);
            }
        }
        if (this.state.selected && (!prevState.selected || prevState.selected.chatId !== this.state.selected.chatId)) {
            this.UpdateChatParticipants();
        }
        if (this.state.scrollToBottom) {
            this.scrollToBottom();
            this.setState({ scrollToBottom: false });
        }
        if (this.state.selected && this.state.selected.unreadMessagesCount > 0 && this.state.isScrollPositionBottom) {
            this.UpdateChatItemAsRead();
        }
        if (this.state.lastScrollHeight && this.state.lastScrollHeight >= 0) {
            ReactDOM.findDOMNode(this.refs.messageList).scrollTop = this.refs.messageList.scrollHeight - this.state.lastScrollHeight;
            this.setState({ lastScrollHeight: -1 });
        }
        if (this.state.focusedChatMessageId !== null) {
            setTimeout(() => {
                this.setState({ focusedChatMessageId: null })
            }, 1500);
        }
    }

    timer = () => {
        this.UpdateChats();
        this.UpdateImportantChats();
    }

    UpdateChatParticipants() {
        const { selected } = this.state;
        if (selected && !this.state.chatParticipants[selected.chatId]) {
            const url = webApiUrl + 'api/chat/GetChatParticipants/' + encodeURI(selected.chatId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    var self = findResponse.filter(x => x.loginId === this.props.user.id);
                    var buyers = findResponse.filter(x => x.isBuyer && x.loginId !== this.props.user.id);
                    var others = findResponse.filter(x => !x.isBuyer && x.loginId !== this.props.user.id);
                    var allParticipants = [];
                    if (this.props.user.type === userAccountTypeConstants.buyer)
                        allParticipants = others.concat(buyers).concat(self);
                    else
                        allParticipants = buyers.concat(others).concat(self);

                    var chatParticipantsNew = Object.assign({}, this.state.chatParticipants);
                    chatParticipantsNew[selected.chatId] = allParticipants;

                    this.setState({ chatParticipants: chatParticipantsNew });
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

    isFilterByBuilding() {
        const { user, selectedBuildingOnly } = this.props;
        return selectedBuildingOnly === true || user.type === userAccountTypeConstants.buyer
    }

    UpdateChatStartList() {
        const { selected, user, selectedBuildingOnly } = this.props;
        if (selected) {
            const url = webApiUrl + 'api/chat/'
                + (
                    !this.isFilterByBuilding()
                        ?
                        ('GetChatStartListByProject/' + selected.projectId)
                        :
                        ('GetChatStartListByBuilding/' + selected.buildingId)
                );

            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };
            this.setState({ loading: true })
            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({ chatStartList: findResponse });
                });
        }
    }

    UpdateImportantChats() {
        const { selected, user } = this.props;
        if (selected) {
            const url = webApiUrl + 'api/chat/'
                + (
                    this.isFilterByBuilding()
                        ?
                        'GetImportantMessagesByBuilding/' + selected.buildingId
                        :
                        'GetImportantMessagesByProject/' + selected.projectId
                );

            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({
                        importantChats: findResponse
                    });
                });
        }
    }

    UpdateChats(startItem = null, refresh = false) {
        const { selected, user } = this.props;
        const { chats } = this.state;
        if (selected) {
            if (this.chatUpdateAbortController && this.chatUpdateAbortController.signal.aborted !== true) {
                this.chatUpdateAbortController.abort();
            }

            if (refresh) clearInterval(this.state.intervalId)


            this.chatUpdateAbortController = new window.AbortController();
            const url = webApiUrl + 'api/chat/' + (this.isFilterByBuilding() ? 'GetChatsByBuilding/' + selected.buildingId : 'GetChatsByProject/' + selected.projectId)
                + (chats.length > 0 && !refresh ? '?dateTime=' + encodeURIComponent(chats[0].dateTime) : '');

            const requestOptions = {
                method: 'GET',
                headers: authHeader(),
                signal: this.chatUpdateAbortController.signal
            };
            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    var allChats = refresh === true ? [] : chats.slice();
                    var expandedList = refresh === true ? [] : this.state.expandedList.slice();
                    let selectedHasUpdate = false;
                    let selectedNew = Object.assign({}, this.state.selected);

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
                            existingChat.lastMessageIsAttachment = findResponse[i].lastMessageIsAttachment;
                            if (this.state.selected && this.state.selected.chatId === existingChat.chatId) {
                                selectedHasUpdate = true;
                                selectedNew = findResponse[i];
                            }
                        }
                        else {
                            allChats.push(findResponse[i]);
                        }
                    }

                    if (!selectedHasUpdate) {
                        selectedNew = this.state.selected;
                    }

                    allChats.sort(function (a, b) { return new Date(b.dateTime) - new Date(a.dateTime); });

                    if (refresh === true) {
                        this.setState({
                            chats: allChats,
                            chatItems: [],
                            chatItemToReplyOn: null,
                            selected: selectedNew,
                            selectedHasUpdate: false,
                            expandedList
                        });
                    }
                    else {
                        this.setState({
                            chats: allChats,
                            selected: selectedNew,
                            selectedHasUpdate
                        });
                    }

                    if (startItem) {
                        const chat = this.getChatByStartItem(startItem);
                        if (chat) {
                            this.selectChat(chat);
                        }
                    }

                    if (refresh === true) {
                        this.openChatMessage(this.props, allChats)
                    };
                    this.setState({ loading: false, retryUpdateChatCount: 0 })
                }).catch((err) => {
                    if (refresh && err.code !== 20) {
                        setTimeout(() => {
                            this.UpdateChats(null, true)
                        }, this.state.retryUpdateChatCount < 5 ? 5000 : 30000);
                        this.setState({ retryUpdateChatCount: this.state.retryUpdateChatCount + 1 })
                    }
                })
        }
    }

    openChatMessage(props, allChats = this.state.chats) {
        const { location } = props;
        if (location && location.state) {
            const { selectedChatId, selectedChatMessageId } = location.state;
            if (!!selectedChatId) {
                if (!!selectedChatMessageId) {
                    this.goToChatMessage(selectedChatId, selectedChatMessageId)
                }
                else {
                    const chat = allChats.find(x => x.chatId.toUpperCase() === selectedChatId.toUpperCase());
                    this.selectChat(chat);
                }
            }
        }
    }


    UpdateChatItems(getNewer = null) {
        const { user } = this.props;
        const { selected, chats, chatItems, isScrollPositionBottom } = this.state;
        if (selected) {
            if (getNewer == null) {
                var defaultComment = localStorage.getItem(selected.chatId + '_' + user.id);
                if (!defaultComment) {
                    defaultComment = '';
                }
                // this.handleChangeComment(defaultComment);
                console.debug("ghgh:", defaultComment);
                this.setState({ defaultComment: defaultComment })

            }

            const url = webApiUrl + 'api/chat/GetChatItems/' + selected.chatId
                + (chatItems && chatItems.length > 0 && getNewer !== null
                    ?
                    (
                        getNewer === true
                            ?
                            '?newer=true&dateTime=' + encodeURIComponent(chatItems[chatItems.length - 1].dateTime)
                            :
                            '?newer=false&dateTime=' + encodeURIComponent(chatItems[0].dateTime)
                    )
                    : ''
                );

            if (this.chatItemsAbortController && this.chatItemsAbortController.signal.aborted !== true) {
                this.chatItemsAbortController.abort();
            }

            this.chatItemsAbortController = new window.AbortController();

            const requestOptions = {
                method: 'GET',
                headers: authHeader(),
                signal: this.chatItemsAbortController.signal
            };

            fetch(url, requestOptions)
                .then(Response => Response.json(), e => { })
                .then(findResponse => {
                    if (!findResponse) {
                        return;
                    }
                    var allChatItems = [];
                    const hasHistory = getNewer !== true ? findResponse.length === 10 : this.state.hasHistory === true;
                    const hasNewer = getNewer === true ? findResponse.length === 10 : this.state.hasNewer === true;
                    var chatItemToReplyOn = null;
                    if (getNewer === null) {
                        allChatItems = findResponse;
                    }
                    else {
                        allChatItems = chatItems.slice();
                        allChatItems = allChatItems.concat(findResponse);
                        chatItemToReplyOn = this.state.chatItemToReplyOn;
                    }

                    let lastScrollHeight = -1;
                    if (getNewer === false) {

                        lastScrollHeight = this.refs.messageList.scrollHeight;
                    }

                    allChatItems.sort(function (a, b) { return new Date(a.dateTime) - new Date(b.dateTime); });

                    const scrollToBottom = getNewer === true ? isScrollPositionBottom : (getNewer === false ? this.state.scrollToBottom : true);

                    this.setState({
                        chatItems: allChatItems,
                        chatItemToReplyOn,
                        hasHistory,
                        hasNewer,
                        scrollToBottom,
                        lastScrollHeight,
                        selectedHasUpdate: false
                    });

                    this.updateOnScroll();
                });
        }
    }

    SearchChats(refresh = true) {
        const { user, selected } = this.props;
        const { searchTerm, searchCategoryId, searchWithAttachment } = this.state;

        if (selected && searchTerm && searchTerm.trim() !== '') {
            const url = webApiUrl + 'api/chat/SearchChats';

            if (this.searchAbortController && this.searchAbortController.signal.aborted !== true) {
                this.searchAbortController.abort();
            }

            this.searchAbortController = new window.AbortController();

            const requestOptions = {
                method: 'POST',
                headers: authHeader('application/json'),
                body: JSON.stringify({
                    chatId: null,
                    buildingId: this.isFilterByBuilding() ? selected.buildingId : (searchCategoryId !== 'all' ? searchCategoryId : null),
                    projectId: !this.isFilterByBuilding() ? selected.projectId : null,
                    searchTerm: searchTerm,
                    organisationId: searchCategoryId !== 'all' && user.type === userAccountTypeConstants.buyer ? searchCategoryId : null,
                    dateTime: ((refresh !== true && this.state.searchResults.length > 0) ? this.state.searchResults[this.state.searchResults.length - 1].dateTime : null),
                    attachment: searchWithAttachment,
                    count: 10
                }),
                signal: this.searchAbortController.signal
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    var searchResults = [];
                    const hasMoreSearchResults = findResponse.length === 10;

                    if (refresh === true) {
                        searchResults = findResponse;
                    }
                    else {
                        searchResults = this.state.searchResults.slice();
                        searchResults = searchResults.concat(findResponse);
                    }

                    searchResults.sort(function (a, b) { return new Date(b.dateTime) - new Date(a.dateTime); });


                    this.setState({
                        searchResults,
                        hasMoreSearchResults,
                    });

                    this.updateSearchResultsOnScroll();
                })
                .catch(err => {
                    if (err.name === 'AbortError') {
                        //handle aborterror here
                    }
                });
        }
        else {
            if (this.searchAbortController && this.searchAbortController.signal.aborted !== true) {
                this.searchAbortController.abort();
            }
            this.setState({
                searchResults: [],
                hasMoreSearchResults: false
            });
        }
    }

    updateSearchResultsOnScroll = () => {
        const { hasMoreSearchResults } = this.state;
        const { searchList } = this.refs;
        if (searchList) {
            const scrollTop = searchList.scrollTop;
            const scrollHeight = searchList.scrollHeight;
            const height = searchList.clientHeight;
            const maxScrollTop = scrollHeight - height;
            const isScrollPositionBottom = (scrollTop === maxScrollTop);
            if (isScrollPositionBottom === true && hasMoreSearchResults === true) {
                this.SearchChats(false);
            }
        }
    };

    SearchChatMessages(refresh = true) {
        const { user } = this.props;
        const { selected, searchTermMessages, searchMessagesWithAttachment } = this.state;

        if (selected && ((searchTermMessages && searchTermMessages.trim() !== '') || searchMessagesWithAttachment === true)) {
            const url = webApiUrl + 'api/chat/SearchChats';

            if (this.searchMessageAbortController && this.searchMessageAbortController.signal.aborted !== true) {
                this.searchMessageAbortController.abort();
            }

            this.searchMessageAbortController = new window.AbortController();

            const requestOptions = {
                method: 'POST',
                headers: authHeader('application/json'),
                body: JSON.stringify({
                    chatId: selected.chatId,
                    buildingId: null,
                    projectId: null,
                    searchTerm: searchTermMessages,
                    organisationId: null,
                    dateTime: ((refresh !== true && this.state.searchMessagesResults.length > 0) ? this.state.searchMessagesResults[this.state.searchMessagesResults.length - 1].dateTime : null),
                    attachment: searchMessagesWithAttachment,
                    count: 10
                }),
                signal: this.searchMessageAbortController.signal
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    var searchMessagesResults = [];
                    const hasMoreSearchMessagesResults = findResponse.length === 10;

                    if (refresh === true) {
                        searchMessagesResults = findResponse;
                    }
                    else {
                        searchMessagesResults = this.state.searchMessagesResults.slice();
                        searchMessagesResults = searchMessagesResults.concat(findResponse);
                    }

                    searchMessagesResults.sort(function (a, b) { return new Date(b.dateTime) - new Date(a.dateTime); });


                    this.setState({
                        searchMessagesResults,
                        hasMoreSearchMessagesResults,
                    });

                    this.updateSearchMessagesResultsOnScroll();
                })
                .catch(err => {
                    if (err.name === 'AbortError') {
                        //handle aborterror here
                    }
                });
        }
        else {
            if (this.searchMessageAbortController && this.searchMessageAbortController.signal.aborted !== true) {
                this.searchMessageAbortController.abort();
            }
            this.setState({
                searchMessagesResults: [],
                hasMoreSearchMessagesResults: false
            });
        }
    }

    updateSearchMessagesResultsOnScroll = () => {
        const { hasMoreSearchMessagesResults } = this.state;
        const { searchMessagesList } = this.refs;
        if (searchMessagesList) {
            const scrollTop = searchMessagesList.scrollTop;
            const scrollHeight = searchMessagesList.scrollHeight;
            const height = searchMessagesList.clientHeight;
            const maxScrollTop = scrollHeight - height;
            const isScrollPositionBottom = (scrollTop === maxScrollTop);
            if (isScrollPositionBottom === true && hasMoreSearchMessagesResults === true) {
                this.SearchChatMessages(false);
            }
        }
    };

    UpdateChatItemAsRead() {
        const { selected, chatItems } = this.state;
        if (selected && chatItems.length > 0 && this.state.isScrollPositionBottom) {
            const lastMessage = chatItems[chatItems.length - 1];
            if (selected.chatId === lastMessage.chatId) {
                const url = webApiUrl + 'api/chat/MarkLastReadChatItem/' + lastMessage.chatMessageId;
                const requestOptions = {
                    method: 'GET',
                    headers: authHeader()
                };

                fetch(url, requestOptions)
                    .then(Response => Response.json())
                    .then(success => {
                        if (success) {
                            var chatsToUpdate = this.state.chats.slice();
                            var chat = chatsToUpdate.find(x => x.chatId === selected.chatId);
                            if (chat) {
                                chat.unreadMessagesCount = 0;
                            }
                            this.setState({ chats: chatsToUpdate });
                        }
                    });
            }
        }
    }

    createNewChat(obj) {
        var existingChat = this.getChatByStartItem(obj);
        if (existingChat) {
            this.setState({
                selected: existingChat,
                chatBoxOpen: true,
                openAddNew: false
            });
        }
        else {
            const url = webApiUrl + 'api/chat/AddNewChat';
            const requestOptions = {
                method: 'POST',
                headers: authHeader('application/json'),
                body: JSON.stringify({
                    buildingId: obj.buildingId,
                    organisationId: obj.organisationId,
                    subject: ''
                })
            };

            fetch(url, requestOptions)
                .then(Response => {
                    if (Response.status === 200)
                        this.UpdateChats(obj);
                });
            this.setState({ openAddNew: false });
        }
    }

    uploadAttachment = e => {
        const { user, t } = this.props;
        const { selected } = this.state;
        if (selected) {
            const files = Array.from(e.target.files)

            if (validateFile(files[0]) === true) {
                this.setState({ uploading: true })

                const formData = new FormData()

                formData.append('file', files[0])

                fetch(webApiUrl + `api/chat/UploadAttachment/` + selected.chatId, {
                    method: 'POST',
                    headers: authHeader(),
                    body: formData
                })
                    .then(res => res.json())
                    .then(res => {
                        if (res === false) {
                            alert(t('general.api.error'));
                        }
                        this.setState({ uploading: false });
                    })
                    .catch(e => {
                        this.setState({ uploading: false });
                        alert(t('general.api.error'));
                    })
            }
        }
    }

    sendNewChatMessage = () => {
        const { user } = this.props;
        const { comment, selected, chatItemToReplyOn, markCommentImportant } = this.state;
        //var comments = stateToMarkdown(comment.getCurrentContent());
        if (selected && comment.trim() !== '') {
            const url = webApiUrl + 'api/chat/AddNewChatMessage';
            const requestOptions = {
                method: 'POST',
                headers: authHeader('application/json'),
                body: JSON.stringify({
                    chatId: selected.chatId,
                    message: comment,
                    important: markCommentImportant,
                    replyToChatMessageId: chatItemToReplyOn ? chatItemToReplyOn.chatMessageId : null
                })
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(success => {
                    if (success) {
                        localStorage.removeItem(selected.chatId + '_' + user.id);
                        this.setState({
                            selectedHasUpdate: true, scrollToBottom: true,
                            comment: '',
                            defaultComment: '',
                            markCommentImportant: false,
                            chatItemToReplyOn: null,
                            showRichTextEditorToolbar: false
                        });
                    }
                });
        }

    }

    handleMarkUnmarkImportantChatMessage(chatItem, isMark) {
        const { selected } = this.state;
        const url = webApiUrl + 'api/chat/MarkUnmarkChatMessageImportant/' + chatItem.chatMessageId + '?isMark=' + isMark;
        const requestOptions = {
            method: 'GET',
            headers: authHeader()
        };

        // BEGIN COMMENT--- this piece of code is written before fetch to make it look smooth on UI, need to fix in future

        // END COMMENT

        fetch(url, requestOptions)
            .then(Response => {
                var importantChats = this.state.importantChats.slice();

                var chatToUpdate = importantChats.find(x => x.chatId.toUpperCase() === chatItem.chatId.toUpperCase());
                if (chatToUpdate) {
                    if (isMark) {
                        var importantChatMessage = {
                            chatMessageId: chatItem.chatMessageId,
                            message: chatItem.message,
                            dateTime: chatItem.dateTime,
                            chatParticipantId: selected.chatParticipantId,
                            isFile: chatItem.isFile
                        }

                        var messages = chatToUpdate.messages;
                        messages.push(importantChatMessage);
                        messages.sort(function (a, b) { return new Date(a.dateTime) - new Date(b.dateTime); });
                        chatToUpdate.messages = messages;

                        this.setState({
                            importantChats
                        });


                    }
                    else {
                        chatToUpdate.messages = chatToUpdate.messages.filter(x => x.chatMessageId.toUpperCase() !== chatItem.chatMessageId.toUpperCase());
                        this.setState({
                            importantChats
                        });
                    }
                }
            })
            .catch(error => {
                console.log('Error while deleting');
            });
    }

    handleUnmarkImportantChatMessage(message, chatId) {
        const { selected } = this.state;
        const url = webApiUrl + 'api/chat/MarkUnmarkChatMessageImportant/' + message.chatMessageId + '?isMark=' + false;
        const requestOptions = {
            method: 'GET',
            headers: authHeader()
        };

        // BEGIN COMMENT--- this piece of code is written before fetch to make it look smooth on UI, need to fix in future

        // END COMMENT

        fetch(url, requestOptions)
            .then(Response => {
                var importantChats = this.state.importantChats.slice();

                var chatToUpdate = importantChats.find(x => x.chatId.toUpperCase() === chatId.toUpperCase());
                if (chatToUpdate) {
                    chatToUpdate.messages = chatToUpdate.messages.filter(x => x.chatMessageId.toUpperCase() !== message.chatMessageId.toUpperCase());
                    this.setState({
                        importantChats
                    });
                }
            })
            .catch(error => {
                console.log('Error while removing');
            });
    }

    isChatMessageImportant = chatItem => {
        if (this.state.importantChats) {
            var importantChats = this.state.importantChats.slice();
            var importantChat = importantChats.find(x => x.chatId.toUpperCase() === chatItem.chatId.toUpperCase());
            if (importantChat) {
                var importantChatMessage = importantChat.messages.find(x => x.chatMessageId.toUpperCase() === chatItem.chatMessageId.toUpperCase());
                if (importantChatMessage) {
                    return true;
                }
            }
        }
        return false;
    }

    handleReplyChatMessage = chatItem => {
        this.setState({ chatItemToReplyOn: chatItem });
    }

    handleCloseReply = () => {
        this.setState({ chatItemToReplyOn: null });
    }

    handleDeleteChatMessage = chatItem => {
        const { t, user } = this.props;
        const { selected } = this.state;
        const url = webApiUrl + 'api/chat/DeleteChatMessage/' + chatItem.chatMessageId;
        const requestOptions = {
            method: 'DELETE',
            headers: authHeader()
        };

        fetch(url, requestOptions)
            .then(Response => Response.json())
            .then(findResponse => {
                var chatItems = this.state.chatItems.slice();
                chatItem = chatItems.find(x => x.chatMessageId.toUpperCase() === chatItem.chatMessageId.toUpperCase());
                chatItem.message = '';
                chatItem.deleted = true;
                chatItem.deletedByParticipant = user.name;
                this.setState({
                    chatItems
                })
            })
            .catch(error => {
                console.log('Error while deleting');
            });
    }

    getUserColor(chatId, loginId) {
        const { chatParticipants } = this.state;
        if (loginId === null) {
            return '#3f51b5';
        }
        if (chatParticipants[chatId]) {
            for (var i = 0; i < chatParticipants[chatId].length; i++) {
                if (chatParticipants[chatId][i].loginId === loginId) {
                    return colors[i % 20];
                }
            }
        }
        return '';
    }

    getUserAvatar(chatId, loginId, userInitials) {
        const { chatParticipants } = this.state;

        var backgroundColor = '';

        if (chatParticipants[chatId]) {
            for (var i = 0; i < chatParticipants[chatId].length; i++) {
                if (chatParticipants[chatId][i].loginId === loginId) {
                    backgroundColor = colors[i % 20];
                    if (chatParticipants[chatId][i].hasPhoto === true) {
                        var srcUrl = webApiUrl + "api/home/GetPersonPhoto/" + chatParticipants[chatId][i].personId;
                        return <Avatar style={{ backgroundColor }} alt={userInitials} src={srcUrl} />
                    }
                }
            }
        }
        return <Avatar style={{ backgroundColor }} >{userInitials}</Avatar>;
    }

    getUserColorByParticipantId(chatId, chatParticipantId) {
        const { chatParticipants } = this.state;
        if (chatParticipantId === null) {
            return '#3f51b5';
        }
        if (chatParticipants[chatId]) {
            for (var i = 0; i < chatParticipants[chatId].length; i++) {
                if (chatParticipants[chatId][i].chatParticipantId === chatParticipantId) {
                    return colors[i % 20];
                }
            }
        }
        return '';
    }

    getNameByParticipantId(chatId, chatParticipantId) {
        const { chatParticipants } = this.state;
        const { user, t } = this.props;
        if (chatParticipants[chatId]) {
            var chatParticipant = chatParticipants[chatId].find(x => x.chatParticipantId === chatParticipantId);
            if (chatParticipant) {
                if (chatParticipant.loginId === user.id)
                    return t('U');
                return chatParticipant.name;
            }
        }
        return '';
    }

    selectChat = chat => {
        this.setState({ selected: chat, chatBoxOpen: true });
    }

    toggleOpenAddNew = () => {
        this.setState({ openAddNew: !this.state.openAddNew });
    }

    handleChatCategoryToggle = categoryId => {
        var expandedList = this.state.expandedList.slice();
        if (expandedList.find(x => x === categoryId)) {
            expandedList = expandedList.filter(x => x !== categoryId);
        }
        else {
            expandedList.push(categoryId);
        }
        this.setState({ expandedList });
    }

    toggleOpenSearchChats = () => {
        document.activeElement.blur();
        this.setState({ openSearchChats: !this.state.openSearchChats });
    }

    handleChangeSearch = event => {
        this.setState({
            searchTerm: event.target.value
        })
    }

    handleChangeSearchCategory = event => {
        this.setState({
            searchCategoryId: event.target.value
        })
    }

    toggleSearchWithAttachment = () => {
        document.activeElement.blur();
        this.setState({ searchWithAttachment: !this.state.searchWithAttachment });
    }

    clearAndCloseSearch = () => {
        this.setState({
            openSearchChats: false,
            searchTerm: '',
            searchResults: [],
            searchCategoryId: 'all',
            searchWithAttachment: false,
            hasMoreSearchResults: false
        });
    }

    toggleOpenSearchChatMessages = () => {
        document.activeElement.blur();
        this.setState({ openSearchChatMessages: !this.state.openSearchChatMessages });
    }

    handleChangeSearchMessages = event => {
        this.setState({
            searchTermMessages: event.target.value
        })
    }

    toggleSearchMessagesWithAttachment = () => {
        document.activeElement.blur();
        this.setState({ searchMessagesWithAttachment: !this.state.searchMessagesWithAttachment });
    }

    clearAndCloseSearchMessages = () => {
        this.setState({
            openSearchChatMessages: false,
            searchTermMessages: '',
            searchMessagesResults: [],
            searchMessagesWithAttachment: false,
            hasMoreSearchMessagesResults: false
        });
    }

    toggleOpenImportantChats = () => {
        this.setState({ openImportantChats: !this.state.openImportantChats });
    }

    handleImportantChatCategoryToggle = chatId => {
        var importantExpandedList = this.state.importantExpandedList.slice();
        if (importantExpandedList.find(x => x === chatId)) {
            importantExpandedList = importantExpandedList.filter(x => x !== chatId);
        }
        else {
            importantExpandedList.push(chatId);
        }
        this.setState({ importantExpandedList });
    }

    isImportantChatExpanded = chatId => {
        if (this.state.importantExpandedList.find(x => x === chatId)) {
            return true;
        }
        return false;
    }

    closeChatBox = () => {
        this.setState({ selected: null, chatBoxOpen: false, chatItems: [] });
    }

    handleChangeComment = comment => {
        console.debug('comment:', comment)
        const { user } = this.props;
        const { selected } = this.state;
        if (selected && user) {
            localStorage.setItem(selected.chatId + '_' + user.id, comment);
        }
        this.setState({ comment: comment });
    };

    getChatSubTitle = chat => {
        const { user } = this.props;
        if (user.type !== userAccountTypeConstants.buyer) {
            return chat.buildingNoExtern
        }
        return chat.organisationName
    }

    getChatByStartItem = item => {
        return this.state.chats.find(x => x.buildingId.toUpperCase() === item.buildingId.toUpperCase()
            && x.organisationId.toUpperCase() === item.organisationId.toUpperCase());
    }

    updateOnScroll = () => {
        const { refs, props } = this;
        const { messageList } = refs;
        if (messageList) {
            const scrollTop = messageList.scrollTop;
            if (scrollTop === 0 && this.state.hasHistory) {
                this.UpdateChatItems(false);
            }

            const scrollHeight = messageList.scrollHeight;
            const height = messageList.clientHeight;
            const maxScrollTop = scrollHeight - height;
            const isScrollPositionBottom = Math.abs(scrollTop - maxScrollTop) <= 2;
            this.setState({ isScrollPositionBottom });
        }
    };

    onResizeMessageListBox = (width, height) => {
        const { messageList } = this.refs;
        if (messageList) {
            const { messagesContainerHeight } = this.state;
            if (messagesContainerHeight !== messageList.clientHeight) {
                const maxScrollTop = messageList.scrollHeight - messageList.clientHeight;
                if (messageList.scrollTop !== maxScrollTop) {
                    const scrollTop = messageList.scrollTop + Math.ceil(messagesContainerHeight - messageList.clientHeight);
                    ReactDOM.findDOMNode(messageList).scrollTop = scrollTop;
                }
            }
        }
        this.setState({ messagesContainerHeight: messageList.clientHeight });
    }

    scrollToBottom = () => {
        const { messageList } = this.refs;
        const scrollHeight = messageList.scrollHeight;
        const height = messageList.clientHeight;
        const maxScrollTop = scrollHeight - height;
        ReactDOM.findDOMNode(messageList).scrollTop = maxScrollTop > 0 ? maxScrollTop : 0;
    }

    handleChatActionModalOpen = chatMessage => {
        const chatAction = Object.assign({}, chatMessage);
        chatAction.date = Date.now();
        this.setState({ chatAction });
    }

    handleChatActionModalClose = () => {
        this.setState({ chatAction: null });
    }

    handleModalDateChange = date => {
        const chatAction = Object.assign({}, this.state.chatAction);
        chatAction.date = date;
        this.setState({ chatAction });
    }

    handleModalChangeTextField = name => event => {
        const chatAction = Object.assign({}, this.state.chatAction);
        switch (name) {
            case 'employeeId': chatAction.employeeId = event.target.value; break;
            case 'description': chatAction.description = event.target.value; break;
            case 'bericht': chatAction.message = event.target.value; break;
            default: return;
        }
        this.setState({ chatAction });
    }

    handleModalActionSubmit = event => {
        event.preventDefault();
        const chatAction = Object.assign({}, this.state.chatAction);
        const { selected } = this.state;
        const { t } = this.props;

        if (selected && event.target.checkValidity()) {
            chatAction.submitting = true;
            this.setState({ chatAction });

            const url = webApiUrl + 'api/home/AddNewAction';
            const requestOptions = {
                method: 'POST',
                headers: authHeader('application/json'),
                body: JSON.stringify({
                    buildingId: selected.buildingId,
                    employeeId: chatAction.employeeId,
                    description: chatAction.description,
                    descriptionExtended: chatAction.message,
                    dateTime: new Date(chatAction.date).toJSON()
                })
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(success => {
                    if (success) {
                        alert(t('Uw actie is toevoegd.'));
                        this.handleChatActionModalClose();
                    }
                })
                .catch(error => {
                    alert(t('general.api.error'));
                    chatAction.submitting = false;
                    this.setState({ chatAction });
                });
        }
        else {
            chatAction.submitted = true;
            this.setState({ chatAction });
        }
    }

    handleImageModalOpen = chatItem => {
        this.setState({ chatItemWithAttachment: chatItem });
    }

    handleImageModalClose = () => {
        this.setState({ chatItemWithAttachment: null });
    }

    goToChatMessage(chatId, chatMessageId) {
        const { selected, chats, chatItems } = this.state;
        if (selected && selected.chatId === chatId) {
            var findChatItem = chatItems.find(x => x.chatMessageId === chatMessageId);
            if (findChatItem) {
                this.setState({ chatBoxOpen: true });
                setTimeout(() => {
                    const { messageList } = this.refs;
                    var item = document.getElementById('message-' + chatMessageId);
                    if (messageList && item) {
                        const scrollHeight = messageList.scrollHeight;
                        const height = messageList.clientHeight;
                        const maxScrollTop = scrollHeight - height;
                        const scrollTopValue = maxScrollTop > 0 && maxScrollTop > item.offsetTop - 50 ? item.offsetTop - 50 : maxScrollTop
                        ReactDOM.findDOMNode(messageList).scrollTop = scrollTopValue;
                        this.setState({ scrollToBottom: false, isScrollPositionBottom: false, openSearchChatMessages: false, focusedChatMessageId: chatMessageId });
                    }
                }, 500);
                return;
            }
        }
        var selectedNew = chats.find(x => x.chatId === chatId);
        if (selectedNew) {
            const url = webApiUrl + 'api/chat/GetChatItemsByMessageId/' + chatMessageId

            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    var chatItems = findResponse;

                    var hasHistory = true;
                    var hasNewer = true;
                    var chatItemsCopy = this.state.chatItems.slice();
                    if (chatItems.filter(x => chatItemsCopy.find(y => y.chatMessageId === x.chatMessageId && y.chatId === x.chatId)).length > 0) {
                        hasHistory = this.state.hasHistory;
                        hasNewer = this.state.hasNewer;
                        for (var i = 0; i < chatItemsCopy; i++) {
                            if (!chatItems.find(x => x.chatMessageId === chatItemsCopy[i].chatMessageId)) {
                                chatItems.push(chatItemsCopy[i]);
                            }
                        }
                    }
                    chatItems.sort(function (a, b) { return new Date(a.dateTime) - new Date(b.dateTime); });

                    const categoryId = this.props.user.type !== userAccountTypeConstants.buyer ? selectedNew.buildingId : selectedNew.organisationId
                    var expandedList = this.state.expandedList.slice();
                    if (!expandedList.find(x => x === categoryId)) {
                        expandedList.push(categoryId);
                    }
                    this.setState({ expandedList });

                    this.setState({
                        selected: selectedNew,
                        chatBoxOpen: true,
                        chatItems,
                        hasHistory,
                        hasNewer,
                        lastScrollHeight: -1,
                        scrollToBottom: false,
                        selectedHasUpdate: false,
                        goToChatMessage: true,
                        isScrollPositionBottom: false,
                        expandedList
                    });

                    this.goToChatMessage(chatId, chatMessageId);
                });
        }
    }

    goToBuildingOverviewPage = buildingId => {
        var selectedItem = this.props.buildings.all.find(x => x.buildingId === buildingId);
        if (selectedItem) {
            this.props.dispatch(commonActions.selectBuilding(selectedItem));

            history.push('/object/' + selectedItem.buildingNoIntern)
        }
    }

    handleShowHideRichTextEditorToolbar = () => {
        this.setState({ showRichTextEditorToolbar: !this.state.showRichTextEditorToolbar });
    }

    handleMarkCommentImportant = () => {
        this.setState({ markCommentImportant: !this.state.markCommentImportant });
    }

    render() {
        const { user, t, classes, width, isFullWidth, selectedBuildingOnly, rights } = this.props;
        const {
            selected,
            chats,
            chatItems,
            focusedChatMessageId,
            chatItemToReplyOn,
            openAddNew,
            openImportantChats,
            chatBoxOpen,
            chatStartList,
            comment,
            defaultComment,
            markCommentImportant,
            textToInsertInComment,
            richTextToInsertInComment,
            standardTexts,
            chatAction,
            importantChats,
            expandedList,
            importantExpandedList,
            openSearchChats,
            searchTerm,
            searchCategoryId,
            searchResults,
            searchWithAttachment,
            openSearchChatMessages,
            searchTermMessages,
            searchMessagesResults,
            searchMessagesWithAttachment,
            uploading,
            chatItemWithAttachment,
            chatParticipants,
            showRichTextEditorToolbar
        } = this.state;

        const matches = isWidthUp('md', width);
        var date, prevDate;
        const localeMap = {
            en: enLocale,
            nl: nlLocale,
        };

        const isUserBuyer = user.type === userAccountTypeConstants.buyer;

        var signature = null;
        if (standardTexts && standardTexts.filter(x => x.isSignature === true).length > 0) {
            const text = standardTexts.find(x => x.isSignature === true).textBlock;
            if (text && text.trim() !== '') {
                signature = text;
            }
        }

        var chatCategories = null;
        if (chats && chats.length > 0) {
            chatCategories = chats.reduce((catsSoFar, chat) => {
                var category = isUserBuyer !== true ? chat.buildingId : chat.organisationId
                if (!catsSoFar[category]) {
                    catsSoFar[category] = {
                        chats: [],
                        sum: 0,
                        expanded: expandedList.findIndex(x => x === category) >= 0
                    };
                }
                catsSoFar[category].chats.push(chat);
                catsSoFar[category].sum += chat.unreadMessagesCount;
                return catsSoFar;
            }, {});
        }

        return (
            <Container className={isFullWidth === true ? clsx(classes.mainContainer, 'fullwidth') : classes.mainContainer}>
                <Grid container className={classes.chatContainer}>
                    <Grid item xs={12} md={4} lg={isFullWidth ? 3 : 4} container direction="column" className={classes.allChats} component={Box} boxShadow={1}>
                        <AppBar position="static">
                            <Toolbar variant="dense">
                                <IconButton disabled={!rights["chat.details.write"]} aria-label="Berichten" color="inherit" onClick={this.toggleOpenAddNew}>
                                    <Chat />
                                </IconButton>
                                <IconButton aria-label="Zoek" color="inherit" onClick={this.toggleOpenSearchChats}>
                                    <Search />
                                </IconButton>
                                <IconButton aria-label="Important" color="inherit" onClick={this.toggleOpenImportantChats}>
                                    <Bookmarks />
                                </IconButton>
                            </Toolbar>
                        </AppBar>

                        <List className={classes.chatList}>
                            {this.state.loading ?
                                <div style={{ display: 'flex', justifyContent: 'center' }}>
                                    <CircularProgress color="primary" size={24} />
                                </div>
                                :
                                chats && chats.length > 0 ?
                                    chats.map((chat, index) => {
                                        let chatTitle = this.getChatSubTitle(chat);
                                        const chatTitleInitial = chatTitle && chatTitle.length > 0
                                            ?
                                            (
                                                isUserBuyer ? chatTitle[0] :
                                                    (
                                                        chatTitle.length <= 3 ?
                                                            chatTitle :
                                                            chatTitle.substr(0, 3)
                                                    )
                                            ) : '-';
                                        return (
                                            <React.Fragment key={index}>
                                                {index !== 0 && <Divider component="li" />}
                                                <ListItem className={classes.chatListItem} title={chatTitle} button selected={selected && selected.chatId === chat.chatId} onClick={() => this.selectChat(chat)}>
                                                    <ListItemAvatar>
                                                        <Avatar>
                                                            {chatTitleInitial}
                                                        </Avatar>
                                                    </ListItemAvatar>
                                                    <ListItemText
                                                        primary={
                                                            <Typography noWrap>{chatTitle}</Typography>
                                                        }
                                                        secondary={
                                                            <Typography variant="body2" color="textSecondary" noWrap>
                                                                {chat.lastMessageIsAttachment && <AttachFile fontSize="small" style={{ marginLeft: -5 }} />}
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
                                                    <Typography variant="caption" className={classes.chatListDate}>{getDateText(new Date(chat.dateTime))}</Typography>
                                                    {
                                                        chat.unreadMessagesCount > 0 &&
                                                        <Badge className={classes.chatListCount} badgeContent={chat.unreadMessagesCount} color="primary" ><span></span></Badge>
                                                    }
                                                    <ChevronRight className={classes.chatListIcon} color="action" />
                                                </ListItem>
                                            </React.Fragment>
                                        )
                                    })
                                    : <ListItem><ListItemText secondary={t('Geen berichten')} /></ListItem>
                            }
                        </List>
                    </Grid>
                    <Slide direction="right" in={openSearchChats} mountOnEnter unmountOnExit>
                        <Grid item xs={12} md={4} lg={isFullWidth ? 3 : 4} container direction="column" className={classes.slideRight}>
                            <AppBar position="static">
                                <Toolbar variant="dense">
                                    <Input
                                        color="inherit"
                                        className={classes.inputBoxSearch}
                                        type="search"
                                        value={searchTerm}
                                        onChange={this.handleChangeSearch}
                                        endAdornment={
                                            <InputAdornment position="end">
                                                <Search />
                                            </InputAdornment>
                                        }
                                    />
                                    <TextField
                                        id="standard-select-category"
                                        select
                                        className={classes.selectBoxSearch}
                                        value={searchCategoryId}
                                        onChange={this.handleChangeSearchCategory}
                                    >
                                        <MenuItem value='all'>{t('Alle')}</MenuItem>
                                        {
                                            chatCategories &&
                                            Object.keys(chatCategories).map(category => (
                                                <MenuItem key={category} value={category}>
                                                    {this.getChatSubTitle(chatCategories[category].chats[0])}
                                                </MenuItem>
                                            ))
                                        }
                                    </TextField>
                                    <ToggleButton style={{ color: 'inherit', borderColor: 'transparent' }} size="small" value='attachment' selected={searchWithAttachment} onChange={this.toggleSearchWithAttachment} aria-label="attachments">
                                        <AttachFile />
                                    </ToggleButton>
                                    <IconButton aria-label="Close" color="inherit" onClick={this.clearAndCloseSearch}>
                                        <Close />
                                    </IconButton>
                                </Toolbar>
                            </AppBar>

                            <List className={classes.searchList} ref="searchList" onScroll={this.updateSearchResultsOnScroll}>
                                {
                                    searchResults && searchResults.length > 0 ?
                                        searchResults.map((message, indexMessage) => (
                                            <React.Fragment key={indexMessage}>
                                                <ListItem button className={classes.searchListItem} onClick={() => this.goToChatMessage(message.chatId, message.chatMessageId)}>
                                                    <ListItemText
                                                        primary={
                                                            <Grid container alignItems="center">
                                                                <Grid item xs={12}>
                                                                    {
                                                                        isUserBuyer ?
                                                                            <Typography noWrap>{this.getChatSubTitle(message)}</Typography>
                                                                            :
                                                                            <Button className={classes.textButton} variant="text" color="primary" size="small" onClick={() => this.goToBuildingOverviewPage(message.buildingId)}>
                                                                                <Typography noWrap>{this.getChatSubTitle(message)}</Typography>
                                                                            </Button>
                                                                    }
                                                                </Grid>
                                                            </Grid>
                                                        }
                                                        secondary={
                                                            <Typography variant="body2" color="textSecondary" noWrap>
                                                                {message.isFile && <AttachFile fontSize="small" style={{ marginLeft: -5 }} />}
                                                                {
                                                                    message.isSender !== true &&
                                                                    <React.Fragment>
                                                                        {
                                                                            message.senderName === null ?
                                                                                <React.Fragment><InfoOutlined style={{ marginTop: '-3px' }} fontSize="small" />&nbsp;</React.Fragment>
                                                                                :
                                                                                <React.Fragment>{message.senderName}:&nbsp;</React.Fragment>
                                                                        }
                                                                    </React.Fragment>
                                                                }
                                                                {md2plaintext(message.message)}
                                                            </Typography>
                                                        }
                                                        secondaryTypographyProps={{ component: "div" }}
                                                    />
                                                    <Typography variant="caption" className={classes.chatListDate}>{getDateText(new Date(message.dateTime))}</Typography>

                                                    <ChevronRight className={classes.chatListIcon} color="action" />
                                                </ListItem>
                                                <Divider component="li" />
                                            </React.Fragment>
                                        ))
                                        :
                                        <ListItem><ListItemText secondary={t('Geen berichten')} /></ListItem>
                                }
                            </List>
                        </Grid>
                    </Slide>

                    <Slide direction="right" in={openAddNew} mountOnEnter unmountOnExit>
                        <Grid item xs={12} md={4} lg={isFullWidth ? 3 : 4} container direction="column" className={classes.slideRight}>
                            <AppBar position="static">
                                <Toolbar variant="dense">
                                    <IconButton edge="start" aria-label="GoBack" color="inherit" onClick={this.toggleOpenAddNew}>
                                        <ArrowBack />
                                    </IconButton>
                                    <Typography className={clsx(classes.grow, classes.bold)} noWrap>
                                        {t('Nieuw bericht')}
                                    </Typography>
                                </Toolbar>
                            </AppBar>
                            <List className={classes.subjectList}>
                                {
                                    chatStartList.map((item, index) => (
                                        <React.Fragment key={index}>
                                            {index !== 0 && <Divider component="li" />}
                                            <ListItem key={index} button onClick={() => this.createNewChat(item)}>
                                                <ListItemText primary={item.title} />
                                            </ListItem>
                                        </React.Fragment>
                                    ))}



                            </List>
                        </Grid>
                    </Slide>

                    <Slide direction="right" in={openImportantChats} mountOnEnter unmountOnExit>
                        <Grid item xs={12} md={4} lg={isFullWidth ? 3 : 4} container direction="column" className={classes.slideRight}>
                            <AppBar position="static">
                                <Toolbar variant="dense">
                                    <IconButton edge="start" aria-label="GoBack" color="inherit" onClick={this.toggleOpenImportantChats}>
                                        <ArrowBack />
                                    </IconButton>
                                    <Typography className={clsx(classes.grow, classes.bold)} noWrap>
                                        {t('Belangrijk berichten')}
                                    </Typography>
                                </Toolbar>
                            </AppBar>
                            <List className={classes.importantChatsList}>
                                {
                                    importantChats && importantChats.length > 0 &&
                                    importantChats.map((chat, index) => (
                                        <React.Fragment key={index}>
                                            {index !== 0 && <Divider component="li" />}
                                            <ListItem className={classes.importantChatCategory} button onClick={() => this.handleImportantChatCategoryToggle(chat.chatId)}>
                                                <ListItemText
                                                    primary={
                                                        <div style={{ maxWidth: '100%' }}>
                                                            {isUserBuyer ?
                                                                <Typography noWrap>{this.getChatSubTitle(chat)}</Typography>
                                                                :
                                                                <Button className={classes.textButton} variant="text" color="primary" size="small" onClick={() => this.goToBuildingOverviewPage(chat.buildingId)}>
                                                                    <Typography noWrap>
                                                                        {this.getChatSubTitle(chat)}
                                                                    </Typography>
                                                                </Button>
                                                            }
                                                            <Badge className={classes.importantChatBadge} badgeContent={chat.messages.length} color="primary"><span></span></Badge>
                                                        </div>
                                                    }
                                                />
                                                {this.isImportantChatExpanded(chat.chatId) ? <ExpandLess className={classes.impChatListIcon} color="action" /> : <ExpandMore className={classes.impChatListIcon} color="action" />}
                                            </ListItem>

                                            <Collapse in={this.isImportantChatExpanded(chat.chatId)} timeout="auto" unmountOnExit>
                                                <List component="div" disablePadding>
                                                    {
                                                        chat.messages.length > 0 &&
                                                        chat.messages.map((message, indexMessage) => (
                                                            <React.Fragment key={indexMessage}>
                                                                <Divider component="li" />
                                                                <ListItem button className={classes.importantChatItem} selected={selected && selected.chatId === chat.chatId} onClick={() => this.goToChatMessage(chat.chatId, message.chatMessageId)}>
                                                                    <ListItemText primary={
                                                                        <Typography variant="body2">
                                                                            {message.isFile && <AttachFile fontSize="small" style={{ marginLeft: -5 }} />}
                                                                            {md2plaintext(message.message)}</Typography>}
                                                                    />
                                                                    <ListItemSecondaryAction>
                                                                        <Grid container direction="column" alignItems="flex-end">
                                                                            <Typography variant="caption" className={classes.impChatListDate}>{getDateText(new Date(message.dateTime))}</Typography>
                                                                            <IconButton edge="end" aria-label="important" size="small" disabled={user.viewOnly === true || !rights["chat.details.write"]} onClick={() => this.handleUnmarkImportantChatMessage(message, chat.chatId)}>
                                                                                <Bookmark color="primary" />
                                                                            </IconButton>
                                                                        </Grid>
                                                                    </ListItemSecondaryAction>
                                                                </ListItem>
                                                            </React.Fragment>
                                                        ))
                                                    }
                                                </List>
                                            </Collapse>
                                        </React.Fragment>
                                    ))
                                }
                            </List>
                        </Grid>
                    </Slide>
                    <Slide direction="left" appear={!matches} in={chatBoxOpen || matches} mountOnEnter unmountOnExit>
                        <Grid item xs={12} md={8} lg={isFullWidth ? 9 : 8} container direction="column" justify="center" className={classes.chatBoxContainer}>
                            <AppBar position="static">
                                <Toolbar variant="dense">
                                    {
                                        selected &&
                                        <React.Fragment>
                                            <Hidden mdUp>
                                                <IconButton edge="start" aria-label="GoBack" color="inherit" onClick={this.closeChatBox}>
                                                    <ArrowBack />
                                                </IconButton>
                                            </Hidden>
                                            <div className={clsx(classes.grow, classes.chatBoxTitle)}>
                                                {
                                                    isUserBuyer ?
                                                        <Typography className={classes.bold} noWrap>
                                                            {this.getChatSubTitle(selected)}
                                                        </Typography>
                                                        :
                                                        <Button className={classes.appBarButton} variant="text" size="small" color="inherit" onClick={() => this.goToBuildingOverviewPage(selected.buildingId)}>
                                                            <Typography className={classes.bold} noWrap>
                                                                {this.getChatSubTitle(selected)}
                                                            </Typography>
                                                        </Button>
                                                }
                                                <Typography component="p" variant="caption" className={classes.grow} noWrap>
                                                    {
                                                        selected.buildingNoExtern + ', ' + selected.organisationName +
                                                        (
                                                            chatParticipants[selected.chatId]
                                                                ?
                                                                ' (' + chatParticipants[selected.chatId].map(x => ' ' + x.name).join().trim() + ')'
                                                                :
                                                                ''
                                                        )
                                                    }
                                                </Typography>
                                            </div>
                                            {
                                                <IconButton aria-label="Zoek" color="inherit" onClick={this.toggleOpenSearchChatMessages}>
                                                    <Search />
                                                </IconButton>
                                            }

                                        </React.Fragment>
                                    }
                                </Toolbar>
                            </AppBar>
                            <Grid container direction="column" alignItems="center" className={classes.chatBox} ref="messageList" onScroll={this.updateOnScroll}>
                                <ReactResizeDetector handleWidth handleHeight onResize={this.onResizeMessageListBox}>
                                    {
                                        selected ?
                                            <Grid className={classes.chatItemsWrapper} container spacing={2} alignContent="flex-start">
                                                {
                                                    chatItems.length > 0 ?
                                                        chatItems.map((chatItem, index) => {
                                                            date = new Date(chatItem.dateTime);
                                                            const dateText = formatDate(date);
                                                            const timeText = formatTime(date);
                                                            date.setHours(0, 0, 0, 0);
                                                            const showDate = index === 0 || date - prevDate !== 0;
                                                            prevDate = date;
                                                            const isSender = chatItem.senderLoginId === user.id;
                                                            const isImportant = this.isChatMessageImportant(chatItem);
                                                            const isFocused = focusedChatMessageId === chatItem.chatMessageId;
                                                            const isSystemMessage = chatItem.senderLoginId == null;
                                                            const sameAsPrevSender = index > 0 && !showDate && chatItems[index - 1].senderLoginId === chatItem.senderLoginId;

                                                            const userInitials = getNameInitials(chatItem.senderName);

                                                            let classesForCard = [classes.chatCard];
                                                            if (chatItem.important === true) {
                                                                classesForCard.push(classes.important);
                                                            }
                                                            if (isSystemMessage === true) {
                                                                classesForCard.push('system')
                                                            }
                                                            if (isFocused === true) {
                                                                classesForCard.push('focused')
                                                            }
                                                            else if (isSender === true) {
                                                                classesForCard.push('sentbyme')
                                                            }

                                                            const justifyAlignment = isSystemMessage ? "center" : isSender ? "flex-end" : "flex-start";

                                                            return (
                                                                <React.Fragment key={chatItem.chatMessageId}>
                                                                    {
                                                                        showDate &&
                                                                        <Grid item container xs={12} spacing={1} justify="center">
                                                                            <Chip label={dateText} className={classes.chip} size="small" />
                                                                        </Grid>
                                                                    }
                                                                    <Grid id={'message-' + chatItem.chatMessageId}
                                                                        item
                                                                        container
                                                                        //xs={12}
                                                                        spacing={1}
                                                                        justify={justifyAlignment}
                                                                        className={sameAsPrevSender ? classes.chatCardContainer : ''}
                                                                    >
                                                                        {
                                                                            /**
                                                                            !isSender &&
                                                                            <Grid item>
                                                                                <Avatar>U{chatItem.senderId}</Avatar>
                                                                            </Grid>
                                                                            **/
                                                                        }
                                                                        <Grid item container xs={10} md={9} justify={justifyAlignment}>
                                                                            <Grid item style={{ maxWidth: '100%' }}>
                                                                                {
                                                                                    !isSender && !sameAsPrevSender && !isSystemMessage &&
                                                                                    <div className={classes.chatItemAvatar}>
                                                                                        {this.getUserAvatar(chatItem.chatId, chatItem.senderLoginId, userInitials)}
                                                                                    </div>
                                                                                }
                                                                                {
                                                                                    isSystemMessage === true &&
                                                                                    <Info className={classes.chatCardInfoIcon} />
                                                                                }
                                                                                {
                                                                                    chatItem.important === true &&
                                                                                    <Error className={classes.chatCardImportantIcon} color="secondary" />
                                                                                }
                                                                                <Card tabIndex={0} className={clsx(classesForCard)}>
                                                                                    <CardContent>
                                                                                        {
                                                                                            chatItem.deleted !== true &&
                                                                                            <div className="chatButtonsContainer">
                                                                                                <IconButton aria-label="Reply" disabled={user.viewOnly === true || !rights["chat.details.write"]} size="small" onClick={() => this.handleReplyChatMessage(chatItem)}>
                                                                                                    <Reply fontSize="inherit" />
                                                                                                </IconButton>
                                                                                                {
                                                                                                    isUserBuyer !== true &&
                                                                                                    <IconButton aria-label="Alert" disabled={user.viewOnly === true || !rights["chat.details.write"]} size="small" onClick={() => this.handleChatActionModalOpen(chatItem)}>
                                                                                                        <AssignmentOutlined fontSize="inherit" />
                                                                                                    </IconButton>
                                                                                                }
                                                                                                <IconButton aria-label="Alert" disabled={user.viewOnly === true || !rights["chat.details.write"]} size="small" onClick={() => this.handleMarkUnmarkImportantChatMessage(chatItem, !isImportant)}>
                                                                                                    {
                                                                                                        isImportant ?
                                                                                                            <Bookmark color="primary" fontSize="inherit" />
                                                                                                            :
                                                                                                            <BookmarkBorder fontSize="inherit" />
                                                                                                    }
                                                                                                </IconButton>
                                                                                                {
                                                                                                    isUserBuyer !== true && isSystemMessage !== true &&
                                                                                                    <IconButton aria-label="Alert" disabled={user.viewOnly === true || !rights["chat.details.write"]} size="small" onClick={() => this.handleDeleteChatMessage(chatItem)}>
                                                                                                        <DeleteOutline fontSize="inherit" />
                                                                                                    </IconButton>
                                                                                                }
                                                                                            </div>
                                                                                        }
                                                                                        {
                                                                                            !isSender && !sameAsPrevSender &&
                                                                                            <Typography
                                                                                                variant="caption"
                                                                                                style={{
                                                                                                    color: this.getUserColor(chatItem.chatId, chatItem.senderLoginId),
                                                                                                    //marginRight: isUserBuyer === true ? 40 : 80
                                                                                                }}>{chatItem.senderName}</Typography>
                                                                                        }
                                                                                        {
                                                                                            chatItem.important === true &&
                                                                                            <Typography variant="subtitle2" color="secondary" className={classes.bold}>{t('BELANGRIJK!')}</Typography>
                                                                                        }
                                                                                        {
                                                                                            chatItem.replyToChatMessageId &&
                                                                                            <Card className={classes.quoteMessageCard} style={{ borderLeftColor: this.getUserColorByParticipantId(chatItem.chatId, chatItem.replyToChatMessageSenderChatParticipantId) }}>
                                                                                                <CardActionArea onClick={() => this.goToChatMessage(chatItem.chatId, chatItem.replyToChatMessageId)}>
                                                                                                    <CardContent>
                                                                                                        <Typography variant="caption" style={{ color: this.getUserColorByParticipantId(chatItem.chatId, chatItem.replyToChatMessageSenderChatParticipantId) }}>
                                                                                                            {
                                                                                                                chatItem.replyToChatMessageSenderChatParticipantId === null ?
                                                                                                                    <React.Fragment>
                                                                                                                        <Info style={{ marginLeft: -4 }} /> {t('Info bericht')}
                                                                                                                    </React.Fragment>
                                                                                                                    :
                                                                                                                    this.getNameByParticipantId(chatItem.chatId, chatItem.replyToChatMessageSenderChatParticipantId)
                                                                                                            }
                                                                                                        </Typography>
                                                                                                        {
                                                                                                            chatItem.replyToChatMessageDeleted === true ?
                                                                                                                <Typography variant="caption" component="p" color="error">{t('Dit bericht is verwijderd')}</Typography>
                                                                                                                :
                                                                                                                (
                                                                                                                    <Typography variant="body2" component="div" style={{ maxHeight: 60, overflow: 'hidden' }}>
                                                                                                                        {
                                                                                                                            chatItem.replyToChatMessageIsFile ?
                                                                                                                                (
                                                                                                                                    chatItem.replyToChatMessageIsImage === true ?
                                                                                                                                        <React.Fragment>
                                                                                                                                            <PhotoCamera />
                                                                                                                                            {t('Photo')}
                                                                                                                                        </React.Fragment>
                                                                                                                                        :
                                                                                                                                        <React.Fragment>
                                                                                                                                            <InsertDriveFile />
                                                                                                                                            {chatItem.replyToChatMessageMessage}
                                                                                                                                        </React.Fragment>
                                                                                                                                )
                                                                                                                                :
                                                                                                                                <Markdown value={chatItem.replyToChatMessageMessage} />
                                                                                                                        }
                                                                                                                    </Typography>
                                                                                                                )
                                                                                                        }
                                                                                                    </CardContent>
                                                                                                    {
                                                                                                        chatItem.replyToChatMessageIsImage === true &&
                                                                                                        <CardMedia title={chatItem.replyToChatMessageMessage} image={webApiUrl + 'api/chat/GetMessageAttachment/' + encodeURI(chatItem.replyToChatMessageId)} />
                                                                                                    }
                                                                                                </CardActionArea>
                                                                                            </Card>
                                                                                        }
                                                                                        {
                                                                                            chatItem.deleted === true ?
                                                                                                <Typography variant="caption" component="p" color="error">{t('Dit bericht is verwijderd door') + ' ' + chatItem.deletedByParticipant}</Typography>
                                                                                                :
                                                                                                (
                                                                                                    chatItem.isFile ?
                                                                                                        (
                                                                                                            chatItem.isImage ?
                                                                                                                <CardMedia component="img" onClick={() => this.handleImageModalOpen(chatItem)} className={classes.chatMedia} title={chatItem.message} image={webApiUrl + 'api/chat/GetMessageAttachment/' + encodeURI(chatItem.chatMessageId)} />
                                                                                                                :
                                                                                                                <CardHeader avatar={<InsertDriveFile />} title={chatItem.message}
                                                                                                                    action={
                                                                                                                        <IconButton href={webApiUrl + 'api/chat/GetMessageAttachment/' + encodeURI(chatItem.chatMessageId)} download>
                                                                                                                            <CloudDownload />
                                                                                                                        </IconButton>
                                                                                                                    } />
                                                                                                        )
                                                                                                        :
                                                                                                        <Typography variant="body2" component="div">
                                                                                                            <Markdown value={chatItem.message} />
                                                                                                        </Typography>
                                                                                                )
                                                                                        }
                                                                                        <Typography variant="caption" component="p" color="textSecondary" align="right">
                                                                                            {timeText}
                                                                                        </Typography>
                                                                                    </CardContent>
                                                                                </Card>

                                                                            </Grid>
                                                                        </Grid>
                                                                    </Grid>
                                                                </React.Fragment>
                                                            )
                                                        })
                                                        :
                                                        <Grid item container xs={12} spacing={1}>
                                                            <Typography component="p" align="center">{selected ? t('Nog geen berichten') : ''}</Typography>
                                                        </Grid>
                                                }
                                            </Grid>
                                            :
                                            <Grid item container xs={12} alignContent="center" alignItems="center" justify="center">
                                                <Grid item>
                                                    <img src={webApiUrl + "api/Config/WebLogo"} width="300" alt="JPDS" />
                                                    <Typography variant="h4" color="textSecondary" align="center">Berichten</Typography>
                                                </Grid>
                                            </Grid>
                                    }
                                </ReactResizeDetector>
                            </Grid>
                            <Grid container className={classes.chatActions} ref="commentBox">
                                {
                                    selected &&
                                    <React.Fragment>
                                        <Grid container item xs={12} className={markCommentImportant ? clsx(classes.chatCommentBoxContainer, classes.important) : classes.chatCommentBoxContainer}>
                                            {
                                                markCommentImportant === true &&
                                                <Typography variant="subtitle2" color="secondary" className={classes.commentBoxImportantHeader}>{t('BELANGRIJK!')}</Typography>
                                            }
                                            {
                                                chatItemToReplyOn &&
                                                <Grid item xs={12} container direction="row" alignItems="center" justify="center">
                                                    <Card className={classes.replyMessageCard} style={{ borderLeftColor: this.getUserColor(chatItemToReplyOn.chatId, chatItemToReplyOn.senderLoginId) }}>
                                                        <CardContent>
                                                            <Typography variant="caption" style={{ color: this.getUserColor(chatItemToReplyOn.chatId, chatItemToReplyOn.senderLoginId) }}>
                                                                {
                                                                    chatItemToReplyOn.senderChatParticipantId === null ?
                                                                        <React.Fragment>
                                                                            <Info style={{ marginLeft: -4 }} /> {t('Info bericht')}
                                                                        </React.Fragment>
                                                                        :
                                                                        this.getNameByParticipantId(chatItemToReplyOn.chatId, chatItemToReplyOn.senderChatParticipantId)
                                                                }
                                                            </Typography>
                                                            {
                                                                chatItemToReplyOn.deleted === true ?
                                                                    <Typography variant="caption" component="p" color="error">{t('Dit bericht is verwijderd door') + ' ' + chatItemToReplyOn.deletedByParticipant}</Typography>
                                                                    :
                                                                    (
                                                                        <Typography variant="body2" component="div" style={{ maxHeight: 40, overflow: 'hidden' }}>
                                                                            {
                                                                                chatItemToReplyOn.isFile ?
                                                                                    (
                                                                                        chatItemToReplyOn.isImage === true ?
                                                                                            <React.Fragment>
                                                                                                <PhotoCamera />
                                                                                                {t('Photo')}
                                                                                            </React.Fragment>
                                                                                            :
                                                                                            <React.Fragment>
                                                                                                <InsertDriveFile />
                                                                                                {chatItemToReplyOn.message}
                                                                                            </React.Fragment>
                                                                                    )
                                                                                    :
                                                                                    <Markdown value={chatItemToReplyOn.message} />
                                                                            }
                                                                        </Typography>
                                                                    )
                                                            }
                                                        </CardContent>
                                                        {
                                                            chatItemToReplyOn.isImage === true &&
                                                            <CardMedia title={chatItemToReplyOn.message} image={webApiUrl + 'api/chat/GetMessageAttachment/' + encodeURI(chatItemToReplyOn.chatMessageId)} />
                                                        }
                                                    </Card>
                                                    <Grid item>
                                                        <IconButton aria-label="Reply" onClick={this.handleCloseReply}>
                                                            <Close />
                                                        </IconButton>
                                                    </Grid>
                                                </Grid>
                                            }
                                            <Grid item xs={12}>
                                                <RichTextEditor
                                                    label="Bericht..."
                                                    showToolbar={showRichTextEditorToolbar}
                                                    onChange={this.handleChangeComment}
                                                    textToInsertAtCursor={textToInsertInComment}
                                                    richTextToInsertAtCursor={richTextToInsertInComment}
                                                    onCompleteTextInsert={() => this.setState({ textToInsertInComment: '', richTextToInsertInComment: '' })}
                                                    value={comment}
                                                    defaultValue={defaultComment}
                                                    readOnly={user.viewOnly === true || !rights["chat.details.write"]}
                                                    autocomplete={{
                                                        strategies:
                                                            standardTexts
                                                                ?
                                                                [
                                                                    {
                                                                        items: standardTexts && standardTexts.map((text) => ({
                                                                            keys: [text.hashtag],
                                                                            value: text.textBlock,
                                                                            content: (<Markdown value={text.textBlock} />)
                                                                        })),
                                                                        triggerChar: "#",
                                                                        //insertSpaceAfter: false,
                                                                        atomicBlockName: "markdown"
                                                                    }
                                                                ]
                                                                :
                                                                []
                                                    }}
                                                />
                                            </Grid>
                                        </Grid>
                                        <Grid item xs={12} container className={classes.chatActionBoxContainer} direction="row" alignItems="flex-start" justify="flex-start">
                                            <Grid item>
                                                <IconButton size="small" aria-label="Chat" disabled={user.viewOnly === true || !rights["chat.details.write"]} color={showRichTextEditorToolbar === true ? "primary" : "default"} onClick={this.handleShowHideRichTextEditorToolbar}>
                                                    <TextFormat />
                                                </IconButton>
                                            </Grid>
                                            {
                                                !isUserBuyer &&
                                                <React.Fragment>
                                                    <Grid item>
                                                        <IconButton size="small" aria-label="Important" disabled={user.viewOnly === true || !rights["chat.details.write"]} color={markCommentImportant === true ? "secondary" : "default"} onClick={this.handleMarkCommentImportant}>
                                                            <PriorityHigh />
                                                        </IconButton>
                                                    </Grid>
                                                    <Grid item>
                                                        <StandardTextManager
                                                            size="small"
                                                            disabled={user.viewOnly === true || !rights["chat.details.write"]}
                                                            selectedBuilding={this.props.selected}
                                                            onSelect={value => this.setState({ richTextToInsertInComment: value })}
                                                            onChange={texts => this.setState({ standardTexts: texts })}
                                                        />
                                                    </Grid>
                                                    {
                                                        signature &&
                                                        <Grid item>
                                                            <Tooltip title={t("Handtekening")}>
                                                                <IconButton size="small" aria-label="Signature" disabled={user.viewOnly === true || !rights["chat.details.write"]} onClick={() => this.setState({ richTextToInsertInComment: signature })}>
                                                                    <Icon className="fas fa-signature" />
                                                                </IconButton>
                                                            </Tooltip>
                                                        </Grid>
                                                    }
                                                </React.Fragment>
                                            }
                                            <Grid item>
                                                {
                                                    <React.Fragment>
                                                        <input accept="*" style={{ display: 'none' }} disabled={user.viewOnly === true || !rights["chat.details.write"]} id="icon-button-file" type="file" onChange={this.uploadAttachment} disabled={uploading} />
                                                        <label htmlFor={`${!this.props.selected || user.viewOnly === true || !rights["chat.details.write"] ? '' : 'icon-button-file'}`} style={{ margin: 0 }}>
                                                            {
                                                                uploading ?
                                                                    <CircularProgress color="primary" size={24} />
                                                                    :
                                                                    <IconButton
                                                                        size="small"
                                                                        color="inherit"
                                                                        aria-label="uploads"
                                                                        component="span"
                                                                        disabled={!this.props.selected || user.viewOnly === true || !rights["chat.details.write"]}
                                                                    >
                                                                        <AttachFile />
                                                                    </IconButton>
                                                            }
                                                        </label>
                                                    </React.Fragment>
                                                }
                                            </Grid>
                                            <Grid item>
                                                <EmojiSelector size="small" disabled={user.viewOnly === true || !rights["chat.details.write"]} onSelect={(value) => this.setState({ textToInsertInComment: value })} />
                                            </Grid>
                                            <Grid item className={classes.grow}>
                                            </Grid>
                                            <Grid item>
                                                <IconButton disabled={user.viewOnly === true || !rights["chat.details.write"]} edge="end" size="small" aria-label="Bericht" onClick={this.sendNewChatMessage}>
                                                    <Send />
                                                </IconButton>
                                            </Grid>
                                        </Grid>
                                    </React.Fragment>
                                }
                            </Grid>
                        </Grid>
                    </Slide>

                    <Slide direction="left" in={openSearchChatMessages} mountOnEnter unmountOnExit>
                        <Grid item xs={12} md={4} lg={isFullWidth ? 3 : 4} container direction="column" className={classes.slideLeft}>
                            <AppBar position="static">
                                <Toolbar variant="dense">
                                    <Input
                                        color="inherit"
                                        className={classes.inputBoxSearch}
                                        type="search"
                                        value={searchTermMessages}
                                        onChange={this.handleChangeSearchMessages}
                                        endAdornment={
                                            <InputAdornment position="end">
                                                <Search />
                                            </InputAdornment>
                                        }
                                    />
                                    <ToggleButton style={{ color: 'inherit', borderColor: 'transparent' }} size="small" value='attachment' selected={searchMessagesWithAttachment} onChange={this.toggleSearchMessagesWithAttachment} aria-label="attachments">
                                        <AttachFile />
                                    </ToggleButton>
                                    <IconButton aria-label="Close" color="inherit" onClick={this.clearAndCloseSearchMessages}>
                                        <Close />
                                    </IconButton>
                                </Toolbar>
                            </AppBar>

                            <List className={classes.searchMessagesList} ref="searchMessagesList" onScroll={this.updateSearchMessagesResultsOnScroll}>
                                {
                                    searchMessagesResults && searchMessagesResults.length > 0 ?
                                        searchMessagesResults.map((message, indexMessage) => (
                                            <React.Fragment key={indexMessage}>
                                                <ListItem button className={classes.searchMessagesListItem} onClick={() => this.goToChatMessage(message.chatId, message.chatMessageId)}>
                                                    <ListItemText
                                                        primary={
                                                            <Typography variant="caption" color="textSecondary">{getDateText(new Date(message.dateTime))}</Typography>
                                                        }
                                                        secondary={
                                                            <Typography variant="body2" noWrap>
                                                                {message.isFile && <AttachFile fontSize="small" style={{ marginLeft: -5 }} />}
                                                                {
                                                                    message.isSender !== true &&
                                                                    <React.Fragment>
                                                                        {
                                                                            message.senderName === null ?
                                                                                <React.Fragment><InfoOutlined style={{ marginTop: '-3px' }} fontSize="small" />&nbsp;</React.Fragment>
                                                                                :
                                                                                <React.Fragment>{message.senderName}:&nbsp;</React.Fragment>
                                                                        }
                                                                    </React.Fragment>
                                                                }
                                                                {md2plaintext(message.message)}
                                                            </Typography>
                                                        }
                                                    />
                                                </ListItem>
                                                <Divider component="li" />
                                            </React.Fragment>
                                        ))
                                        :
                                        <ListItem><ListItemText secondary={t('Geen berichten')} /></ListItem>
                                }
                            </List>
                        </Grid>
                    </Slide>
                </Grid>
                <Modal
                    aria-labelledby="transition-modal-title"
                    aria-describedby="transition-modal-description"
                    className={classes.modal}
                    open={chatAction != undefined}
                    onClose={this.handleChatActionModalClose}
                    closeAfterTransition
                    BackdropComponent={Backdrop}
                    BackdropProps={{
                        timeout: 500,
                    }}
                >
                    <Fade in={chatAction != undefined}>
                        <Card className={classes.modalCard}>
                            <CardHeader id="transition-modal-title" title={
                                <Typography variant="h6">{t('Actie toevoegen')}</Typography>
                            } className={classes.modalCardHeader} />
                            <CardContent id="transition-modal-description">
                                {
                                    chatAction &&
                                    <form noValidate onSubmit={this.handleModalActionSubmit} disabled={chatAction.submitting}>
                                        <MuiPickersUtilsProvider utils={DateFnsUtils} locale={nlLocale}>
                                            <Grid container spacing={1} justify="space-around">
                                                <Grid item xs={12}>
                                                    <DateTimePicker
                                                        variant="inline"
                                                        margin="dense"
                                                        id="date-time-picker"
                                                        label={t('Startdatumtijd')}
                                                        format="dd/MM/yyyy HH:mm"
                                                        value={chatAction.date}
                                                        onChange={(date) => this.handleModalDateChange(date)}
                                                        inputVariant="outlined"
                                                        autoOk
                                                        ampm={false}
                                                        fullWidth
                                                        required
                                                        error={chatAction.submitted && !chatAction.date}
                                                        disabled={chatAction.submitting}
                                                    />
                                                </Grid>
                                                <Grid item xs={12}>
                                                    <TextField
                                                        id="outlined-behandelaar"
                                                        select
                                                        label="Select"
                                                        value={chatAction.employeeId == undefined ? '' : chatAction.employeeId}
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
                                                        error={chatAction.submitted && !chatAction.employeeId}
                                                        disabled={chatAction.submitting}
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
                                                        value={chatAction.description}
                                                        onChange={this.handleModalChangeTextField('description')}
                                                        margin="dense"
                                                        variant="outlined"
                                                        fullWidth
                                                        required
                                                        error={chatAction.submitted && !chatAction.description}
                                                        disabled={chatAction.submitting}
                                                    />
                                                </Grid>
                                                <Grid item xs={12}>
                                                    <TextField
                                                        label={t('Uitgebreide omschrijving')}
                                                        value={chatAction.message}
                                                        onChange={this.handleModalChangeTextField('bericht')}
                                                        margin="dense"
                                                        variant="outlined"
                                                        multiline
                                                        fullWidth
                                                        required
                                                        error={chatAction.submitted && !chatAction.message}
                                                        disabled={chatAction.submitting}
                                                    />
                                                </Grid>
                                                <Grid container item xs={12} justify="flex-end">
                                                    <Button type="submit" color="primary" variant="contained" disabled={chatAction.submitting}>{t('Toevoegen')}</Button>
                                                </Grid>
                                            </Grid>
                                        </MuiPickersUtilsProvider>
                                    </form>
                                }
                            </CardContent>
                        </Card>
                    </Fade>
                </Modal>

                {
                    chatItemWithAttachment && chatItemWithAttachment.isImage &&
                    <Modal
                        aria-labelledby="transition-modal-title"
                        aria-describedby="transition-modal-description"
                        className={classes.modal}
                        open={true}
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
                                    alt={chatItemWithAttachment.message}
                                    title={chatItemWithAttachment.message}
                                    image={webApiUrl + 'api/chat/GetMessageAttachment/' + encodeURI(chatItemWithAttachment.chatMessageId)}
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
    const { selected, rights } = buildings;
    return {
        user,
        selected,
        buildings,
        rights
    };
}

const connectedMessagesPage = connect(mapStateToProps)(withWidth()(withTranslation()(withStyles(styles)(MessagesPage))));
export { connectedMessagesPage as MessagesPage };