import React from "react";
import { Link } from "react-router-dom";
import { makeStyles, useTheme } from "@material-ui/core/styles";
import { IconButton, Typography, Badge, List, Divider, ListItem, ListItemText, Avatar, useMediaQuery, Popover, ListItemAvatar, CardMedia, ListItemIcon } from "@material-ui/core";
import {
    AttachFile,
    Info,
    Error,
    ArrowDropDown
} from '@material-ui/icons';
import { getDateText, getNameInitials } from '../_helpers';
import { useTranslation } from 'react-i18next'
import Markdown from "../components/Markdown";

const { webApiUrl } = window.appConfig;

const useStyles = makeStyles(theme => ({
    systemMessage: {
        backgroundColor: '#fff5c4'
    },
    importantMessageIcon: {
        position: 'absolute',
        right: 0
    }
}));

export default function MessagesDetailsPopover(props) {
    const { children, invisible, badgeContent, badgeColor, messages, isSavedMessages, projectNo, projectName, onClose, handleChange } = props;
    const { t } = useTranslation()
    const classes = useStyles();
    const theme = useTheme();
    const matches = useMediaQuery(theme.breakpoints.up('md'));
    const [anchorEl, setAnchorEl] = React.useState(null);

    const handleClick = (event) => {
        event.preventDefault()
        event.stopPropagation();
        event.nativeEvent.stopImmediatePropagation();
        setAnchorEl(event.currentTarget);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    const handleCloseAll = () => {
        handleChange && handleChange();
        handleClose();
        if (onClose) {
            onClose();
        }
    }

    const open = Boolean(anchorEl);
    const id = open ? 'project-recent-messages-popover' : undefined;

    return (
        <React.Fragment>
            <IconButton color="inherit" aria-describedby={id} onClick={handleClick}>
                <Badge invisible={invisible} badgeContent={badgeContent} color={badgeColor}>
                    {children}
                </Badge>
            </IconButton>
            <Popover
                id={id}
                open={open}
                anchorEl={anchorEl}
                onClose={handleClose}
                anchorOrigin={{
                    vertical: 'bottom',
                    horizontal: 'center',
                }}
                transformOrigin={{
                    vertical: 'top',
                    horizontal: 'center',
                }}
            >
                <div style={{ width: 300, maxWidth: '100%', padding: '0 4px' }}>
                    <Typography component="div" variant="caption" color="textSecondary"
                        style={{ fontWeight: "bold", width: '100%', padding: '8px 12px 0' }}>
                        {(!isSavedMessages ? t('Recente berichten') : t('Opgeslagen berichten')) + ' - ' + projectNo}
                    </Typography>
                    <Typography noWrap component="div" variant="subtitle2" color="textSecondary"
                        style={{ fontWeight: "bold", width: '100%', padding: '0 12px 0' }}>
                        {projectName}
                    </Typography>
                    <List className={classes.list} style={{ width: '100%' }}>
                        {
                            messages && messages.length > 0 ?
                                <React.Fragment>{
                                    messages.map((message, index) => {
                                        const userInitials = getNameInitials(message.senderName);
                                        return (
                                            <React.Fragment key={index}>
                                                {
                                                    //index !== 0 &&
                                                    <Divider component="li" />
                                                }
                                                <ListItem className={message.senderName === null && classes.systemMessage}
                                                    title={message.buildingNoExtern}
                                                    button component={Link}
                                                    onClick={handleCloseAll}
                                                    to={{
                                                        pathname: '/werk/' + projectNo + '/berichten',
                                                        state: {
                                                            selectedChatId: message.chatId,
                                                            selectedChatMessageId: message.chatMessageId
                                                        }
                                                    }}
                                                >
                                                    {
                                                        message.senderName === null ?
                                                            <ListItemAvatar><Avatar style={{ background: 'none' }}><Typography color="primary"><Info /></Typography></Avatar></ListItemAvatar>
                                                            :
                                                            <ListItemAvatar>
                                                                <Avatar alt={userInitials} src={webApiUrl + "api/chat/GetParticipantPhoto/" + message.senderChatParticipantId}>{userInitials}</Avatar>
                                                            </ListItemAvatar>
                                                    }
                                                    <ListItemText
                                                        style={{ margin: 0 }}
                                                        primary={
                                                            <Typography variant="" component="div" noWrap color="textPrimary" style={{ paddingRight: 64 }}>{message.buildingNoExtern}</Typography>
                                                        }
                                                        secondary={
                                                            <React.Fragment>
                                                                {
                                                                    !!message.senderName && <Typography variant="caption" noWrap>{message.senderName}:&nbsp;</Typography>
                                                                }
                                                                <Typography variant="body2" color="textSecondary" noWrap>
                                                                    {
                                                                        !!message.isFile && !message.isImage &&
                                                                        <React.Fragment>
                                                                            <AttachFile fontSize="small" style={{ marginLeft: -5 }} />
                                                                            {message.message}
                                                                        </React.Fragment>
                                                                    }
                                                                    {
                                                                        !!message.isImage && <CardMedia component="img" className={classes.chatMedia} title={message.message} image={webApiUrl + 'api/chat/GetMessageAttachment/' + encodeURI(message.chatMessageId)} />
                                                                    }
                                                                    {
                                                                        !message.isFile &&
                                                                        <Typography variant="body2" component="div">
                                                                            <Markdown value={message.message} />
                                                                        </Typography>
                                                                    }

                                                                </Typography>
                                                            </React.Fragment>
                                                        }
                                                        secondaryTypographyProps={{ component: "div" }}
                                                    />
                                                    <Typography variant="caption" color="textPrimary" className={classes.chatListDate}
                                                        noWrap
                                                        style={{ alignSelf: 'start', minWidth: 72, marginLeft: -72, textAlign: 'right' }}>
                                                        {getDateText(new Date(message.dateTime))}
                                                    </Typography>
                                                    {
                                                        !!message.isImportant &&
                                                        <Error className={classes.importantMessageIcon} color="secondary" />
                                                    }
                                                </ListItem>
                                            </React.Fragment>
                                        )
                                    })
                                }
                                    <ListItem title={t('Meer')}
                                        button component={Link}
                                        onClick={handleCloseAll}
                                        to={
                                            !isSavedMessages
                                                ?
                                                {
                                                    pathname: '/werk/' + projectNo + '/berichten'
                                                }
                                                :
                                                {
                                                    pathname: '/werk/' + projectNo + '/berichten',
                                                    state: {
                                                        showImportantMessages: true
                                                    }
                                                }
                                        }>
                                        <ListItemIcon><ArrowDropDown color="textSecondary" /></ListItemIcon>
                                        <ListItemText
                                            style={{ margin: 0 }}
                                            primary={
                                                <Typography variant="caption" noWrap color="textSecondary">{t('Meer')}</Typography>
                                            }
                                        />
                                    </ListItem>
                                </React.Fragment>
                                : <ListItem><ListItemText secondary={t('dashboard.messages.nodata')} /></ListItem>
                        }
                    </List>
                </div>
            </Popover>
        </React.Fragment>
    );
}
