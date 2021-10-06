import React from "react";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import { Avatar, Container, Grid, Typography, Divider, Card, CardContent, CardHeader, List, ListItem, ListItemText, ListItemAvatar, Badge, Modal, Backdrop, Fade, IconButton, CardMedia, Dialog, DialogContent, DialogTitle, Box, AppBar, Toolbar, ListItemIcon } from "@material-ui/core";
import { withStyles, withTheme } from '@material-ui/core/styles';
import { Schedule, Chat, LibraryBooks, Assignment, Close, ArrowBackIos, ArrowForwardIos, KeyboardArrowLeft, KeyboardArrowRight, LocalOffer, AttachFile, InfoOutlined, TimerOff } from "@material-ui/icons"
import { getDateText, md2plaintext, formatDate, formatTime, authHeader } from "../_helpers"
import { userAccountTypeConstants } from "../_constants"
import clsx from "clsx";
import { withTranslation } from 'react-i18next';

const { webApiUrl } = window.appConfig;

const styles = theme => ({
    grow: {
        flexGrow: 1
    },
    bold: {
        fontWeight: "bold"
    },
    welcomePanel: {
        color: theme.palette.common.white,
        height: '40vh',
        position: 'relative',
        padding: theme.spacing(5, 0),
        '& h1': {
            textShadow: '0 0 10px rgb(0,0,0)'
        }
    },
    card: {
        maxWidth: 400,
        margin: 'auto'
    },
    cardHeader: {
        backgroundColor: theme.palette.primary.main,
        color: theme.palette.primary.contrastText
    },
    quotationPop: {
        backgroundColor: theme.palette.background.default,
        position: 'absolute',
        textDecoration: 'none !important',
        top: 0,
        right: 0,
        padding: theme.spacing(0.5, 1),
        borderRadius: '0 0 5px 5px'
    },
    fullWidth: {
        width: '100%'
    },
    list: {
        maxHeight: 308,
        overflow: 'auto',
        [theme.breakpoints.up("md")]: {
            maxHeight: 380,
            height: 380
        }
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
    }
});


class HomePage extends React.Component {
    state = {
        actions: [],
        plannings: [],
        news: [],
        messages: []
    };

    componentDidMount() {
        const { user, selected } = this.props;
        if (selected) {
            this.UpdatePlannings();
            this.UpdateMessages();
            if (user.type === userAccountTypeConstants.buyer) {
                this.UpdateNews();
            }
            else {
                this.UpdateActions();
            }
        }
        var intervalId = setInterval(this.timer, 10000);
        // store intervalId in the state so it can be accessed later:
        this.setState({ intervalId: intervalId });
    }

    componentWillUnmount() {
        // use intervalId from the state to clear the interval
        clearInterval(this.state.intervalId);
    }

    componentDidUpdate(prevProps, prevState) {
        const { selected } = this.props;
        if (!prevProps.selected || prevProps.selected.buildingId !== selected.buildingId) {
            if (this.props.user.type === userAccountTypeConstants.buyer) {
                this.UpdatePlannings();
                this.UpdateMessages();
            }
        }
        if (!prevProps.selected || prevProps.selected.projectId !== selected.projectId) {
            if (this.props.user.type === userAccountTypeConstants.buyer) {
                this.UpdateNews();
            }
            else {
                this.UpdatePlannings();
                this.UpdateActions();
                this.UpdateMessages();
            }
        }
    }

    timer = () => {
        this.UpdateMessages(false);
    }

    UpdateActions() {
        const { selected, user } = this.props;
        if (selected) {
            if (this.updateActionsController && this.updateActionsController.signal.aborted !== true) {
                this.updateActionsController.abort();
            }
            this.updateActionsController = new window.AbortController();

            const url = webApiUrl + 'api/home/GetActionsByProjectId/' + encodeURI(selected.projectId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader(),
                signal: this.updateActionsController.signal
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({
                        actions: findResponse
                    });
                });
        }
    }

    UpdatePlannings() {
        const { selected, user } = this.props;
        if (selected) {
            if (this.updatePlanningsController && this.updatePlanningsController.signal.aborted !== true) {
                this.updatePlanningsController.abort();
            }
            this.updatePlanningsController = new window.AbortController();
            const url = webApiUrl + 'api/home/'
                + (
                    user.type === userAccountTypeConstants.buyer
                        ?
                        ('GetPlanningsByBuildingId/' + selected.buildingId)
                        :
                        ('GetPlanningsByProjectId/' + selected.projectId)
                );
            const requestOptions = {
                method: 'GET',
                headers: authHeader(),
                signal: this.updatePlanningsController.signal
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({
                        plannings: findResponse
                    });
                });
        }
    }

    UpdateNews() {
        const { selected } = this.props;
        if (selected) {
            const url = webApiUrl + 'api/home/GetNewsByProjectId/' + encodeURI(selected.projectId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({
                        news: findResponse
                    });
                });
        }
    }

    UpdateMessages(refresh = true) {
        const { selected, user } = this.props;
        const { messages } = this.state;
        if (selected) {
            if (this.chatUpdateAbortController && this.chatUpdateAbortController.signal.aborted !== true) {
                this.chatUpdateAbortController.abort();
            }

            if (refresh) clearInterval(this.state.intervalId)

            this.chatUpdateAbortController = new window.AbortController();

            const url = webApiUrl + 'api/chat/' + (user.type === userAccountTypeConstants.buyer ? 'GetChatsByBuilding/' + selected.buildingId : 'GetChatsByProject/' + selected.projectId)
                + (messages.length > 0 ? '?dateTime=' + encodeURIComponent(messages[0].dateTime) : '');
            const requestOptions = {
                method: 'GET',
                headers: authHeader(),
                signal: this.chatUpdateAbortController.signal
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    var allChats = refresh === true ? [] : messages.slice();
                    if (refresh) {
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
                        messages: allChats
                    });
                });
        }
    }

    getChatSubTitle = chat => {
        const { user } = this.props;
        if (user.type !== userAccountTypeConstants.buyer) {
            return chat.buildingNoExtern
        }
        return chat.organisationName
    }

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

    renderActions() {
        const { user, t, classes } = this.props;


        return (
            <Card className={classes.card}>
                <CardHeader title={
                    <Typography variant="h6">{t('dashboard.actions.title')}</Typography>
                } avatar={<Assignment />} className={classes.cardHeader} />
                <List className={classes.list}>
                    {
                        this.state.actions.length > 0 ?
                            this.state.actions.map((data, index) => (
                                <React.Fragment key={index}>
                                    {index !== 0 && <Divider component="li" />}
                                    <ListItem title={data.description} button onClick={() => this.handleActionDialogOpen(index)}>
                                        <ListItemText primary={
                                            <Typography noWrap>{data.description}</Typography>
                                        }
                                            secondary={
                                                <React.Fragment>
                                                    <Typography variant="body2" color="textPrimary" noWrap>{t('general.date') + ': ' + formatDate(new Date(data.actionDate)) + ' ' + t('general.time') + ': ' + formatTime(data.startTime) + ' uur'}</Typography>
                                                    <Typography variant="body2" noWrap>
                                                        {data.buildingNoIntern + (data.buyerRenterP1Name ? ' - ' + data.buyerRenterP1Name + (data.buyerRenterP2Name ? ', ' + data.buyerRenterP2Name : '') : '')}
                                                    </Typography>
                                                </React.Fragment>
                                            }
                                            secondaryTypographyProps={{ component: "div" }}

                                        />
                                    </ListItem>
                                </React.Fragment>
                            ))
                            : <ListItem><ListItemText secondary={t('dashboard.actions.nodata')} /></ListItem>
                    }
                </List>
            </Card>
        )
    }

    renderPlannings() {
        const { user, t, classes, selected } = this.props;
        const currentDateTime = Date.now();
        return (
            <Card className={classes.card}>
                <CardHeader title={
                    <Typography variant="h6">{t('dashboard.planning.title')}</Typography>
                } avatar={<Schedule />} className={classes.cardHeader} />
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
                                                pathname: data.dossierId ? `/dossier/${data.dossierId}` : "",
                                                search: data.dossierId ? `buildingId=${selected.buildingId}` : ""
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
                </List>
            </Card>
        )
    }

    renderMessages() {
        const { user, t, classes } = this.props;
        const isUserBuyer = user.type === userAccountTypeConstants.buyer;
        return (
            <Card className={classes.card}>
                <Link to='/berichten'>
                    <CardHeader title={
                        <Typography variant="h6">{t('dashboard.messages.title')}</Typography>
                    } avatar={<Chat />} className={classes.cardHeader} />
                </Link>
                <List className={classes.list}>
                    {
                        this.state.messages.length > 0 ?
                            this.state.messages.map((chat, index) => {
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
                                            to={{
                                                pathname: '/berichten',
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
            </Card>
        )
    }

    renderNews() {
        const { user, t, classes } = this.props;
        return (
            <Card className={classes.card}>
                <CardHeader title={
                    <Typography variant="h6">{t('dashboard.news.title')}</Typography>
                } avatar={<LibraryBooks />} className={classes.cardHeader} />
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
                </List>
            </Card>
        )
    }

    render() {
        const { user, t, classes, selected, buildings, quotationsCount } = this.props;
        const { actions, actionIndex, news, newsIndex } = this.state;
        const openActionPopup = actionIndex >= 0 && actionIndex < actions.length;
        const selectedAction = openActionPopup ? actions[actionIndex] : null;
        if (selectedAction) {
            const building = buildings.find(x => x.buildingId === selectedAction.buildingId);
        }
        const isBuyer = user.type === userAccountTypeConstants.buyer;

        const openNewsPopup = newsIndex >= 0 && newsIndex < news.length;
        const selectedNews = openNewsPopup ? news[newsIndex] : null;



        return (
            <Container>
                <Grid container direction="row" justify="center" alignItems="center" className={classes.welcomePanel}>
                    {
                        isBuyer && quotationsCount > 0 &&
                        <Box boxShadow={3} component={Link} to="/aangevraagd" className={classes.quotationPop}>
                            <Grid container alignItems="center" spacing={1}>
                                <Grid item>
                                    <LocalOffer color="secondary" />
                                </Grid>
                                <Grid item>
                                    {
                                        quotationsCount === 1 ?
                                            <Typography color="textPrimary">
                                                Er staat (1)
                                                <br />
                                                offerte voor u klaar
                                            </Typography>
                                            :
                                            <Typography color="textPrimary">
                                                Er staan ({quotationsCount})
                                                <br />
                                                offertes voor u klaar
                                            </Typography>
                                    }
                                </Grid>
                            </Grid>
                        </Box>
                    }
                    <Grid item xs>
                        <Typography component="h1" variant="h4" gutterBottom align="center">{user.name}, {isBuyer ? t('dashboard.welcome.text.buyer') : t('dashboard.welcome.text')}</Typography>
                    </Grid>
                </Grid>
                <Grid container spacing={5}>
                    <Grid item xs={12} md={4}>
                        {isBuyer ? this.renderPlannings() : this.renderActions()}
                    </Grid>
                    <Grid item xs={12} md={4}>
                        {this.renderMessages()}
                    </Grid>
                    <Grid item xs={12} md={4}>
                        {isBuyer ? this.renderNews() : this.renderPlannings()}
                    </Grid>
                </Grid>
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
            </Container>
        );
    }
}

function mapStateToProps(state) {
    const { authentication, buildings, dashboardCount } = state;
    const { user } = authentication;
    const { selected, all } = buildings;
    const { quotationsCount } = dashboardCount;
    return {
        user,
        selected,
        buildings: all,
        quotationsCount
    };
}

const connectedHomePage = connect(mapStateToProps)(withTranslation()(withStyles(styles)(HomePage)));
export { connectedHomePage as HomePage };