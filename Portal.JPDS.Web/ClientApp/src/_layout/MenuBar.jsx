import React, { useImperativeHandle } from "react";
import { Link } from "react-router-dom";
import { fade, makeStyles, useTheme } from "@material-ui/core/styles";
import { AppBar, Toolbar, IconButton, Typography, Collapse, Grid, Badge, MenuItem, Menu, Drawer, Button, List, Divider, ListItem, ListItemIcon, ListItemText, Avatar, useMediaQuery, Container, Hidden, Icon, Box, Popover, Tooltip } from "@material-ui/core";
import MenuIcon from "@material-ui/icons/Menu";
import {
    Home, Chat, HomeWork, MoreHoriz, ArtTrack, Build, Description, EventNote, PriorityHigh
} from '@material-ui/icons';
import { history, getUser, getCommonArray } from '../_helpers';
import { maxWidth } from "@material-ui/system";
import { useTranslation } from 'react-i18next'
import LanguageSelector from "./LanguageSelector";
import { userAccountTypeConstants, apps, appsInfo, getLinkToHome, internalLayoutViewConstants } from '../_constants';
import clsx from "clsx";
import { useRef } from "react";
import { useSelector } from "react-redux";

const { webApiUrl } = window.appConfig;

const useStyles = makeStyles(theme => ({
    root: {
        width: 72,
        height: '100%',
        paddingTop: 0,
        backgroundColor: theme.palette.primary.main,
        color: theme.palette.primary.contrastText,
        overflowY: 'auto',
        overflowX: 'hidden',
        position: 'absolute',
        left: 0,
        zIndex: 1101,
        [theme.breakpoints.down("xs")]: {
            height: 56,
            width: '100%',
            overflowY: 'hidden',
            overflowX: 'auto',
            bottom: 0
        },
        ['@media (orientation: portrait)']: {
            height: 56,
            width: '100%',
            overflowY: 'hidden',
            overflowX: 'auto',
            bottom: 0
        }
    },
    button: {
        height: 56,
        width: 72,
        borderLeft: '4px solid transparent',
        borderRight: '4px solid transparent',
        [theme.breakpoints.down("xs")]: {
            borderLeft: 'none',
            borderRight: 'none',
            borderTop: '4px solid transparent',
            borderBottom: '4px solid transparent',
            float: "left"
        },
        ['@media (orientation: portrait)']: {
            borderLeft: 'none',
            borderRight: 'none',
            borderTop: '4px solid transparent',
            borderBottom: '4px solid transparent',
            float: "left"
        },
        '&.more': {
            float: "right"
        },
        '&.Mui-selected': {
            borderLeftColor: 'rgba(255,255,255,0.7)',
            borderBottomColor: 'rgba(255,255,255,0.7)'
        },
        '&:hover': {
            color: 'inherit'
        },
        '& .text': {
            fontSize: '.6rem',
            maxWidth: '100%'
        }
    }
}));

export default function MenuBar(props) {
    const { dashboardCount, user, app, history, selectedBuilding, layoutView } = props;
    const { messageCountPerBuilding } = dashboardCount;
    const menuList = useRef();
    const { t } = useTranslation();
    const classes = useStyles();
    const theme = useTheme();
    const matches = useMediaQuery(theme.breakpoints.up('md'));
    const { loading, rights } = useSelector(state => state.buildings);
    const { dossiers } = useSelector(state => state.dossier);
    const showDossiers = rights['menu.canShowMenu'] || (dossiers.openOrClosedDossiers && dossiers.openOrClosedDossiers.length > 0);
    const hasDossierUpdates = dossiers.openOrClosedDossiers && dossiers.openOrClosedDossiers.some(p => p.hasUpdates);
    const [anchorEl, setAnchorEl] = React.useState(null);
    const [dimensions, setDimensions] = React.useState({
        height: window.innerHeight,
        width: window.innerWidth
    });

    React.useEffect(() => {
        window.addEventListener("resize", handleResize);
        return () => window.removeEventListener("resize", handleResize);
    });

    function handleResize() {
        setDimensions({
            height: window.innerHeight,
            width: window.innerWidth
        })
    }

    function getMenuItems() {
        switch (app) {
            case apps.huisinfo:
                switch (layoutView) {
                    case internalLayoutViewConstants.Project:
                        const baseUrl = selectedBuilding ? '/werk/' + selectedBuilding.projectNo : '';
                        const renderMenu = rights['menu.canShowMenu'] ? [(
                            <ListItem button disableGutters component={Link} to={baseUrl} className={classes.button} selected={history.location.pathname === baseUrl}>
                                <Grid container direction="column" justify="center" alignItems="center" >
                                    <Home fontSize="default" />
                                    <Typography noWrap className="text"> {t('Dashboard')} </Typography>
                                </Grid>
                            </ListItem>
                        ),
                        (
                            <ListItem button disableGutters component={Link} to={baseUrl + '/objecten'} className={classes.button} selected={history.location.pathname.startsWith(baseUrl + '/objecten')}>
                                <Grid container direction="column" justify="center" alignItems="center" >
                                    <HomeWork fontSize="default" />
                                    <Typography noWrap className="text"> {t('Objecten')} </Typography>
                                </Grid>
                            </ListItem>
                        ),
                        (
                            <ListItem button disableGutters component={Link} to={baseUrl + '/berichten'} className={classes.button} selected={history.location.pathname.startsWith(baseUrl + '/berichten')}>
                                <Grid container direction="column" justify="center" alignItems="center" >
                                    <Badge badgeContent={dashboardCount.totalUnreadChats} color="secondary">
                                        <Chat fontSize="default" />
                                    </Badge>
                                    <Typography noWrap className="text"> {t('Berichten')} </Typography>
                                </Grid>
                            </ListItem>
                        ),
                        (
                            <ListItem button disableGutters component={Link} to={baseUrl + '/standaard-opties'} className={classes.button} selected={history.location.pathname.startsWith(baseUrl + '/standaard-opties')}>
                                <Grid container direction="column" justify="center" alignItems="center" >
                                    <Build fontSize="default" />
                                    <Typography noWrap className="text"> {t('Opties')} </Typography>
                                </Grid>
                            </ListItem>
                        )] : [];
                        return [
                            ...renderMenu,
                            (
                                <ListItem button disableGutters component={Link} to={baseUrl + '/dossier'}
                                    className={classes.button}
                                    selected={history.location.pathname.startsWith(baseUrl + '/dossier') || history.location.pathname.startsWith(baseUrl + '/building')}>
                                    <Grid container direction="column" justify="center" alignItems="center" >
                                        <Description fontSize="default" />
                                        <Typography noWrap className="text"> {t('Dossiers')} </Typography>
                                    </Grid>
                                    {
                                        hasDossierUpdates &&
                                        <Tooltip title={t('Er is een aanpassing gedaan in het dossier.')}>
                                            <PriorityHigh color="error" style={{ position: 'absolute', right: 0, top: 0 }} />
                                        </Tooltip>
                                    }
                                </ListItem>
                            ),
                            (
                                showDossiers &&
                                <ListItem button disableGutters component={Link} to={baseUrl + '/deadline-dossier'}
                                    className={classes.button}
                                    selected={history.location.pathname.startsWith(baseUrl + '/deadline-dossier')}>
                                    <Grid container direction="column" justify="center" alignItems="center" >
                                        <EventNote fontSize="default" />
                                        <Typography style={{ textAlign: 'center' }} className="text"> {t('Deadline')} </Typography>
                                    </Grid>
                                </ListItem>
                            )
                        ];
                    case internalLayoutViewConstants.Building:
                        return selectedBuilding ?
                            [(
                                <ListItem button disableGutters component={Link} to={'/object/' + selectedBuilding.buildingNoIntern} className={classes.button} selected={history.location.pathname === '/object/' + selectedBuilding.buildingNoIntern}>
                                    <Grid container direction="column" justify="center" alignItems="center" >
                                        <Home fontSize="default" />
                                        <Typography noWrap className="text"> {t('Dashboard')} </Typography>
                                    </Grid>
                                </ListItem>
                            ),
                            (
                                <ListItem button disableGutters component={Link} to={'/object/' + selectedBuilding.buildingNoIntern + '/berichten'} className={classes.button} selected={history.location.pathname.startsWith('/object/' + selectedBuilding.buildingNoIntern + '/berichten')}>
                                    <Grid container direction="column" justify="center" alignItems="center" >
                                        <Badge badgeContent={dashboardCount.totalUnreadChats} color="secondary">
                                            <Chat fontSize="default" />
                                        </Badge>
                                        <Typography noWrap className="text"> {t('Berichten')} </Typography>
                                    </Grid>
                                </ListItem>
                            )]
                            :
                            [];
                    default: return [];
                }
            case apps.constructionQuality:
                switch (layoutView) {
                    case internalLayoutViewConstants.Project:
                        const baseUrl = selectedBuilding ? '/werk/' + selectedBuilding.projectNo + '/kwaliteitsborging' : '';
                        return [(
                            <ListItem button disableGutters component={Link} to={baseUrl} className={classes.button} selected={history.location.pathname === baseUrl}>
                                <Grid container direction="column" justify="center" alignItems="center" >
                                    <Home fontSize="default" />
                                    <Typography noWrap className="text"> {t('Meldingen')} </Typography>
                                </Grid>
                            </ListItem >
                        )];
                    case internalLayoutViewConstants.Building:
                        return selectedBuilding ?
                            [(
                                <ListItem button disableGutters component={Link} to={'/object/' + selectedBuilding.buildingNoIntern + '/kwaliteitsborging'} className={classes.button} selected={history.location.pathname === '/object/' + selectedBuilding.buildingNoIntern + '/kwaliteitsborging'}>
                                    <Grid container direction="column" justify="center" alignItems="center" >
                                        <Home fontSize="default" />
                                        <Typography noWrap className="text"> {t('Meldingen')} </Typography>
                                    </Grid>
                                </ListItem>
                            )]
                            :
                            [];
                    default: return [];
                }
            case apps.resolverModule:
                switch (layoutView) {
                    case internalLayoutViewConstants.Project:
                        const baseUrl = selectedBuilding ? '/werk/' + selectedBuilding.projectNo + '/werkbonnen' : '';
                        return [(
                            <ListItem button disableGutters component={Link} to={baseUrl} className={classes.button} selected={history.location.pathname === baseUrl}>
                                <Grid container direction="column" justify="center" alignItems="center" >
                                    <Home fontSize="default" />
                                    <Typography noWrap className="text"> {t('Werkbonnen')} </Typography>
                                </Grid>
                            </ListItem>
                        )];
                    case internalLayoutViewConstants.Building:
                        return selectedBuilding ?
                            [(
                                <ListItem button disableGutters component={Link} to={'/object/' + selectedBuilding.buildingNoIntern + '/werkbonnen'} className={classes.button} selected={history.location.pathname === '/object/' + selectedBuilding.buildingNoIntern + '/kwaliteitsborging'}>
                                    <Grid container direction="column" justify="center" alignItems="center" >
                                        <Home fontSize="default" />
                                        <Typography noWrap className="text"> {t('Werkbonnen')} </Typography>
                                    </Grid>
                                </ListItem>
                            )]
                            :
                            [];
                    default: return [];
                }
            default: return [];
        }
    }

    const userApps = getCommonArray(user.availableApps, appsInfo.map(x => x.appId));
    const isBuyer = user.type === userAccountTypeConstants.buyer;
    const linkToHome = getLinkToHome(app);

    const menuItems = getMenuItems();

    const isPortrait = dimensions.height > dimensions.width;
    let noOfMenuItemsCanFit = menuItems.length;
    if (isPortrait === true) {
        noOfMenuItemsCanFit = Math.floor(dimensions.width / 72);
    }

    const handleMoreClick = (event) => {
        setAnchorEl(event.currentTarget);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    const open = Boolean(anchorEl);
    const id = open ? 'more-buttons-popover' : undefined;

    return (!loading &&
        <Box ref={menuList} component={List} boxShadow={5} className={classes.root}>
            {
                menuItems.map((item, index) =>
                    (noOfMenuItemsCanFit >= menuItems.length || index < noOfMenuItemsCanFit - 1) &&
                    (
                        <React.Fragment key={index}>{item}</React.Fragment>
                    )
                )
            }
            {
                noOfMenuItemsCanFit < menuItems.length &&
                <React.Fragment>
                    <ListItem aria-describedby={id} button disableGutters className={clsx(classes.button, 'more')} onClick={handleMoreClick}>
                        <Grid container direction="column" justify="center" alignItems="center" >
                            <MoreHoriz fontSize="default" />
                            <Typography noWrap className="text"> {t('Meer')} </Typography>
                        </Grid>
                    </ListItem>
                    <Popover
                        id={id}
                        open={open}
                        anchorEl={anchorEl}
                        onClose={handleClose}
                        anchorOrigin={{
                            vertical: 'top',
                            horizontal: 'right',
                        }}
                        transformOrigin={{
                            vertical: 'bottom',
                            horizontal: 'right',
                        }}
                    >
                        <div className={classes.emojiContainer}>
                            {
                                menuItems.map((item, index) =>
                                    (index >= noOfMenuItemsCanFit - 1) &&
                                    (
                                        <React.Fragment key={index}>{item}</React.Fragment>
                                    )
                                )
                            }
                        </div>
                    </Popover>
                </React.Fragment>
            }
        </Box>
    );
}
