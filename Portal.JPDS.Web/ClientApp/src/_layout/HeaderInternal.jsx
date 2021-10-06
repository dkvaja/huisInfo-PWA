import React, { useImperativeHandle } from "react";
import { Link } from "react-router-dom";
import { fade, makeStyles, useTheme } from "@material-ui/core/styles";
import { AppBar, Toolbar, IconButton, Typography, Collapse, Grid, Badge, MenuItem, Menu, Drawer, Button, List, Divider, ListItem, ListItemIcon, ListItemText, Avatar, useMediaQuery, Container, Hidden, Icon, Tooltip } from "@material-ui/core";
import MenuIcon from "@material-ui/icons/Menu";
import {
    ChevronLeft as ChevronLeftIcon,
    AccountCircle,
    Mail as MailIcon,
    Notifications as NotificationsIcon,
    LibraryBooks,
    Settings as SettingsIcon,
    MoreVert as MoreIcon,
    ExitToApp as LogOutIcon,
    Chat,
    Apps,
    HomeWorkOutlined,
    ArrowRight,
    HouseOutlined,
    Bookmarks,
    LockRounded as LockRoundedIcon
} from '@material-ui/icons';
import { history, getUser, getCommonArray, getNameInitials } from '../_helpers';
import SelectBuilding from './SelectBuilding';
import SelectProject from "./SelectProject";
import { maxWidth } from "@material-ui/system";
import { useTranslation } from 'react-i18next'
import LanguageSelector from "./LanguageSelector";
import { userAccountTypeConstants, apps, appsInfo, getLinkToHome, internalLayoutViewConstants } from '../_constants';
import clsx from "clsx";
import SelectProjectInternal from "./SelectProjectInternal";
import SelectBuildingInternal from "./SelectBuildingInternal";
import MessagesDetailsPopover from "./MessagesDetailsPopover";
import { ChangePasswordModal } from "./ChangePasswordModal";
import { useSelector } from "react-redux";

const { webApiUrl } = window.appConfig;

const useStyles = makeStyles(theme => ({
    grow: {
        flexGrow: 1
    },
    appBarWrapper: {
        height: 64,
        [theme.breakpoints.down("sm")]: {
            height: 64
        },
        [theme.breakpoints.down("xs")]: {
            height: 56
        },
        [`${theme.breakpoints.down("xs")} and (orientation: landscape)`]: {
            height: 48
        },
        '& .MuiAppBar-positionFixed': {
            padding: '0 !important',
            '& .MuiToolbar-root': {
                borderBottomColor: theme.palette.primary.main,
                borderBottomStyle: 'solid',
                borderBottomWidth: theme.spacing(.5),
            }
        }
    },
    logoAnchor: {
        [theme.breakpoints.down("sm")]: {
            maxWidth: "calc(100% - 216px)"
        }
    },
    logo: {
        maxHeight: 36,
        marginRight: theme.spacing(1),
        [theme.breakpoints.down("sm")]: {
            maxWidth: "100%"
        }
    },
    search: {
        position: "relative",
        borderRadius: theme.shape.borderRadius,
        backgroundColor: fade(theme.palette.common.white, 0.15),
        "&:hover": {
            backgroundColor: fade(theme.palette.common.white, 0.25)
        },
        marginRight: theme.spacing(2),
        marginLeft: 0,
        width: "100%",
        [theme.breakpoints.up("sm")]: {
            marginLeft: theme.spacing(3),
            width: "auto"
        }
    },
    buttonAvatar: {
        height: theme.spacing(4),
        width: theme.spacing(4),
        margin: theme.spacing(-0.5),
        color: 'inherit',
        background: 'transparent'
    },
    nested: {
        paddingLeft: theme.spacing(4),
    },
    button: {
        '&:hover': {
            color: 'inherit'
        }
    },
    projectButton: {
        maxWidth: '45%',
        '&:hover': {
            color: theme.palette.primary.main
        }
    },
    searchIcon: {
        width: theme.spacing(7),
        height: "100%",
        position: "absolute",
        pointerEvents: "none",
        display: "flex",
        alignItems: "center",
        justifyContent: "center"
    },
    inputRoot: {
        color: "inherit"
    },
    inputInput: {
        padding: theme.spacing(1, 1, 1, 7),
        transition: theme.transitions.create("width"),
        width: "100%",
        [theme.breakpoints.up("md")]: {
            width: 200
        }
    },
    accountMenu: {
        maxWidth: 300,
        width: 300
    },
    userInfo: {
        padding: theme.spacing(0, 1, 1, 2),
        minWidth: 270
    },
    rightSection: {
        display: "flex"
    },
    sectionDesktop: {
        display: "none",
        [theme.breakpoints.up("md")]: {
            display: "flex"
        }
    },
    sectionMobile: {
        display: "flex",
        [theme.breakpoints.up("md")]: {
            display: "none"
        }
    },
    menuButton: {
        margin: theme.spacing(0, 1, 0, -1)
    },
    drawerHeader: {
        display: 'flex',
        alignItems: 'center',
        padding: '0 8px',
        ...theme.mixins.toolbar,
        justifyContent: 'flex-end',
    },
    list: {
        width: 250,
    },
    projectAppbar: {
        top: 64
    },
    projectToolbar: {
        minHeight: 30
    },
    selectBox: {
        color: 'inherit',
        '& svg': {
            color: 'inherit'
        }
    },
    selectBuilding: {
        width: '100%',
        textAlign: 'right'
    }
}));

export default function PrimaryAppBar(props) {
    const { dashboardCount, selectedBuilding, user, app, layoutView, recentUnreadMessages, recentSavedMessages } = props;
    const { messageCountPerBuilding } = dashboardCount;
    const { t } = useTranslation()
    const classes = useStyles();
    const [anchorEl, setAnchorEl] = React.useState(null);
    const [menuOpen, setMenuOpen] = React.useState(false);
    const [openSubMenu, setOpenSubMenu] = React.useState(true);
    const [openSubMenu2, setOpenSubMenu2] = React.useState(true);
    const [openSubMenu3, setOpenSubMenu3] = React.useState(true);
    const [changePwdOpen, setChangePwdOpen] = React.useState(false);
    const { rights } = useSelector(state => state.buildings);
    const theme = useTheme();
    const matches = useMediaQuery(theme.breakpoints.up('md'));

    const onChangePassword = () => {
        setChangePwdOpen(true);
    };

    const changePwdClose = (params) => {
        setChangePwdOpen(false);
    }

    function handleClick(event) {
        setOpenSubMenu(!openSubMenu);
        event.stopPropagation();
    }

    function handleClick2(event) {
        setOpenSubMenu2(!openSubMenu2);
        event.stopPropagation();
    }

    function handleClick3(event) {
        setOpenSubMenu3(!openSubMenu3);
        event.stopPropagation();
    }

    const isMenuOpen = Boolean(anchorEl);

    function handleProfileMenuOpen(event) {
        setAnchorEl(event.currentTarget);
    }

    function handleMenuClose() {
        setAnchorEl(null);
    }

    function handleDrawerToggle() {
        setMenuOpen(!menuOpen);
    }

    const userApps = getCommonArray(user.availableApps, appsInfo.map(x => x.appId));
    const selectedAppInfo = appsInfo.find(x => x.appId === app);
    const isBuyer = user.type === userAccountTypeConstants.buyer;
    const linkToHome = getLinkToHome(app);

    const menuId = "primary-account-menu";

    const logo = webApiUrl + (selectedBuilding ? ('api/home/ProjectLogo/' + selectedBuilding.projectId) : 'api/config/WebLogo');
    const userInitials = getNameInitials(user.name);

    const renderMenu = (
        <Menu
            anchorEl={anchorEl}
            anchorOrigin={{ vertical: "top", horizontal: "right" }}
            id={menuId}
            keepMounted
            transformOrigin={{ vertical: "top", horizontal: "right" }}
            open={isMenuOpen}
            onClose={handleMenuClose}
            className={classes.accountMenu}>
            <div className={classes.userInfo}>
                <Grid container wrap="nowrap" spacing={1} alignItems="center" justify="center">
                    <Grid item>
                        <Avatar alt={userInitials} src={webApiUrl + "api/home/GetPersonPhoto/" + user.personId}>{userInitials}</Avatar>
                        {
                            //<Avatar>
                            //    <AccountCircle />
                            //</Avatar>
                        }
                    </Grid>
                    <Grid item xs>
                        <Typography variant="body1" className={classes.grow}>{user.name}</Typography>
                    </Grid>
                </Grid>
            </div>
            <Divider />
            {
                !matches &&
                <>
                    {
                        selectedBuilding &&
                        <React.Fragment>
                            <MenuItem onClick={() => { history.push(linkToHome) }}>
                                <HomeWorkOutlined />
                                &nbsp;
                                <Typography noWrap>{selectedBuilding.projectName}</Typography>
                            </MenuItem>
                            {
                                layoutView === internalLayoutViewConstants.Building &&
                                <MenuItem onClick={() => { history.push('/object/' + selectedBuilding.buildingNoIntern) }}>
                                    <HouseOutlined />
                                    &nbsp;
                                    <Typography noWrap>{selectedBuilding.buildingNoIntern}</Typography>
                                </MenuItem>
                            }
                        </React.Fragment>
                    }
                    <Divider />
                </>
            }
            <MenuItem
                onClick={() => {
                    onChangePassword();
                }}
            >
                <LockRoundedIcon />
                &nbsp;
                <span>{t("layout.navbar.changepassword")}</span>
            </MenuItem>
            <MenuItem onClick={() => { history.push('/login') }}>
                <LogOutIcon />
                &nbsp;
                <span>{t('layout.navbar.logout')}</span>
            </MenuItem>
        </Menu>
    );

    return (
        <div className={clsx(classes.appBarWrapper, classes.grow)}>
            <AppBar position="fixed" color="inherit">
                <Toolbar>
                    <Grid container alignItems="center">
                        {
                            <IconButton edge="start" aria-label="Apps" color="inherit" disabled={userApps.filter(x => x !== app && appsInfo.find(y => y.appId == app)).length === 0} className={classes.button} component={Link} to="/">
                                <Apps />
                            </IconButton>
                        }
                        <Link to={linkToHome} className={classes.logoAnchor}>
                            <img src={logo} alt='JPDS' className={classes.logo} />
                        </Link>
                        <div className={classes.grow}>
                            {
                                matches &&
                                <React.Fragment>
                                    {
                                        selectedBuilding &&
                                        <React.Fragment>
                                            <Button color="primary" className={classes.projectButton} component={Link} to={'/werk/' + selectedBuilding.projectNo + (app !== apps.huisinfo && !!selectedAppInfo ? selectedAppInfo.path : '')}>
                                                <Typography variant="h6" noWrap>{selectedBuilding.projectName}</Typography>
                                            </Button>
                                            {
                                                layoutView === internalLayoutViewConstants.Building &&
                                                <React.Fragment>
                                                    <ArrowRight />
                                                    <Button color="primary" className={classes.projectButton} component={Link} to={'/object/' + selectedBuilding.buildingNoIntern}>
                                                        <Typography variant="h6" noWrap>{selectedBuilding.buildingNoIntern}</Typography>
                                                    </Button>
                                                </React.Fragment>
                                            }
                                        </React.Fragment>
                                    }
                                </React.Fragment>
                            }
                        </div>
                        <div style={{ display: 'flex' }}>
                            {
                                selectedBuilding && (layoutView === internalLayoutViewConstants.Project || layoutView === internalLayoutViewConstants.Building) &&
                                <React.Fragment>
                                    <SelectProjectInternal layoutView={layoutView} recentUnreadMessages={recentUnreadMessages} recentSavedMessages={recentSavedMessages} />
                                    {
                                        app === apps.huisinfo &&
                                        <SelectBuildingInternal layoutView={layoutView} />
                                    }
                                </React.Fragment>
                            }
                            {
                                app === apps.huisinfo && rights['menu.canShowMenu'] &&
                                <React.Fragment>
                                    {
                                        selectedBuilding &&
                                        <React.Fragment>
                                            {
                                                //<IconButton aria-label="Berichten" color="inherit" className={classes.button} component={Link}
                                                //    to={layoutView !== internalLayoutViewConstants.Building ? '/werk/' + selectedBuilding.projectNo + '/berichten' : '/object/' + selectedBuilding.buildingNoIntern + '/berichten'}>
                                                //    <Badge badgeContent={dashboardCount.totalUnreadChats} color="secondary">
                                                //        <Chat />
                                                //    </Badge>
                                                //</IconButton>
                                            }
                                            <MessagesDetailsPopover
                                                messages={recentUnreadMessages ? recentUnreadMessages.filter(x => x.projectId === selectedBuilding.projectId) : []}
                                                projectNo={selectedBuilding.projectNo}
                                                projectName={selectedBuilding.projectName}
                                                badgeContent={dashboardCount.totalUnreadChatMessages}
                                                badgeColor="secondary">
                                                <Chat />
                                            </MessagesDetailsPopover >
                                        </React.Fragment>
                                    }
                                    {
                                        selectedBuilding &&
                                        <Hidden only="xs">
                                            <MessagesDetailsPopover
                                                isSavedMessages={true}
                                                messages={recentSavedMessages ? recentSavedMessages.filter(x => x.projectId === selectedBuilding.projectId) : []}
                                                projectNo={selectedBuilding.projectNo}
                                                projectName={selectedBuilding.projectName}
                                                badgeContent={dashboardCount.totalCountSavedMessages}
                                                badgeColor="primary"
                                                invisible={!dashboardCount.totalCountSavedMessages}>
                                                <Bookmarks />
                                            </MessagesDetailsPopover >
                                            {
                                                //<IconButton aria-label="Systeemberichten" color="inherit" className={classes.button} component={Link}
                                                //    to={{
                                                //        pathname: '/werk/' + selectedBuilding.projectNo + '/berichten',
                                                //        state: {
                                                //            showImportantMessages: true
                                                //        }
                                                //    }}>
                                                //    <Badge badgeContent={dashboardCount.totalCountSavedMessages} color="primary">
                                                //        <Bookmarks />
                                                //    </Badge>
                                                //</IconButton>
                                            }
                                        </Hidden>
                                    }
                                    {
                                        //<Hidden only="xs">
                                        //    <IconButton aria-label="Systeemberichten" color="inherit" className={classes.button}>
                                        //        <Badge badgeContent={Math.floor(Math.random() * 20)} color="primary">
                                        //            <NotificationsIcon />
                                        //        </Badge>
                                        //    </IconButton>
                                        //</Hidden>
                                    }
                                </React.Fragment>
                            }
                            <Tooltip title={t('Account')}>
                                <IconButton
                                    edge="end"
                                    aria-label={user.name}
                                    aria-controls={menuId}
                                    aria-haspopup="true"
                                    onClick={handleProfileMenuOpen}
                                    color="inherit"
                                >
                                    <Avatar className={classes.buttonAvatar} alt={userInitials} src={webApiUrl + "api/home/GetPersonPhoto/" + user.personId}>
                                        <AccountCircle />
                                    </Avatar>
                                </IconButton>
                            </Tooltip>
                        </div>
                    </Grid>
                </Toolbar>
            </AppBar>
            {renderMenu}
            {changePwdOpen && <ChangePasswordModal onChangePasswordClose={changePwdClose} changePwdOpen={changePwdOpen} />}
        </div>
    );
}
