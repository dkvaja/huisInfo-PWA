import React from "react";
import { Link } from "react-router-dom";
import { fade, makeStyles, useTheme } from "@material-ui/core/styles";
import {
	AppBar,
	Avatar,
	Badge,
	Button,
	Collapse,
	Container,
	Divider,
	Drawer,
	Grid,
	Hidden,
	IconButton,
	List,
	ListItem,
	ListItemText,
	Menu,
	MenuItem,
	Toolbar,
	Typography,
	useMediaQuery
} from "@material-ui/core";
import MenuIcon from "@material-ui/icons/Menu";
import {
	AccountCircle,
	Apps,
	Bookmarks,
	Chat,
	ChevronLeft as ChevronLeftIcon,
	ExitToApp as LogOutIcon,
	ExpandLess,
	ExpandMore,
	LockRounded as LockRoundedIcon,
	ShoppingBasket
} from '@material-ui/icons';
import { getCommonArray, getNameInitials, history } from '../_helpers';
import SelectBuilding from './SelectBuilding';
import SelectProject from "./SelectProject";
import { useTranslation } from 'react-i18next'
import { apps, appsInfo, userAccountTypeConstants } from '../_constants';
import clsx from "clsx";
import { ChangePasswordModal } from './ChangePasswordModal'
import { useSelector } from "react-redux";

const { webApiUrl } = window.appConfig;

const useStyles = makeStyles(theme => ({
	grow: {
		flexGrow: 1
	},
	appBarWrapper: {
		height: 96,
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
			padding: '0 !important'
		}
	},
	appBarContainer: {
		[theme.breakpoints.down("sm")]: {
			padding: 0
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
	const { dashboardCount, selectedBuilding, user, app } = props;
	const { t } = useTranslation()
	const classes = useStyles();
	const [anchorEl, setAnchorEl] = React.useState(null);
	const [menuOpen, setMenuOpen] = React.useState(false);
	const [openSubMenu, setOpenSubMenu] = React.useState(true);
	const [openSubMenu2, setOpenSubMenu2] = React.useState(true);
	const [openSubMenu3, setOpenSubMenu3] = React.useState(true);
	const [changePwdOpen, setChangePwdOpen] = React.useState(false);
	const theme = useTheme();
	const matches = useMediaQuery(theme.breakpoints.up('md'));
	const { dossiers } = useSelector(state => state.dossier);
	const { buyerGuidesBuildings: buildings } = useSelector(state => state.buildings);
	const showDossiers = (buildings && buildings.length > 0) || (dossiers.openOrClosedDossiers && dossiers.openOrClosedDossiers.length > 0);

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

	function getLinkToHome() {
		switch (app) {
			case apps.huisinfo:
				return '/home';
			case apps.aftercare:
				return '/nazorg';
			default:
				return '/';
		}
	}

	const userApps = getCommonArray(user.availableApps, appsInfo.map(x => x.appId));
	const isBuyer = user.type === userAccountTypeConstants.buyer;
	const linkToHome = getLinkToHome();

	//Hardcoded fix for clients to hide menu item for nazorg module
	const hideDrawerMenu = app == apps.aftercare && ['nazorg.batenburgbv.nl'].includes(window.location.hostname);

	const menuId = "primary-account-menu";

	//const logo = process.env.PUBLIC_URL + '/Content/Images/Logos/1Logo-JPDS.png';
	//const logo = process.env.PUBLIC_URL + '/Content/Images/Logos/Logo_Vink_Bouw.png';
	//const logo = process.env.PUBLIC_URL + '/Content/Images/Logos/logo_home4u.jpg';
	const logo = webApiUrl + (selectedBuilding ? ('api/home/ProjectLogo/' + selectedBuilding.projectId) : 'api/config/WebLogo');
	const userInitials = getNameInitials(user.name);

	const renderTopMenuList = (
		<List
			component="nav"
			aria-labelledby="nested-list-subheader"
			className={classes.root}
		>
			<ListItem button component={Link} to={linkToHome} className={classes.button}>
				<ListItemText primary={t('Home')} />
			</ListItem>
			<ListItem button onClick={handleClick}>
				<ListItemText primary={t('layout.menuitem.projectinformation')} />
				{openSubMenu ? <ExpandLess /> : <ExpandMore />}
			</ListItem>
			<Collapse in={openSubMenu} timeout="auto" unmountOnExit>
				<List component="div" disablePadding>
					<ListItem button component={Link} to="/algemene-projectinformatie"
						className={clsx(classes.nested, classes.button)}>
						<ListItemText primary={t('layout.menuitem.projectinformation.generalprojectinfo')} />
					</ListItem>
					{isBuyer === true &&
						<ListItem button component={Link} to="/mijn-woning" className={clsx(classes.nested, classes.button)}>
							<ListItemText primary={t('layout.menuitem.projectinformation.myhouse')} />
						</ListItem>
					}
					<ListItem button component={Link} to="/betrokken-partijen" className={clsx(classes.nested, classes.button)}>
						<ListItemText primary={t('layout.menuitem.projectinformation.involvedparties')} />
					</ListItem>
					<ListItem button component={Link} to="/veel-gestelde-vragen" className={clsx(classes.nested, classes.button)}>
						<ListItemText primary={t('layout.menuitem.projectinformation.faq')} />
					</ListItem>
				</List>
			</Collapse>
			{
				app === apps.huisinfo &&
				<React.Fragment>
					{
						isBuyer === true &&
						<React.Fragment>
							<ListItem button component={Link} to="/documenten" className={classes.button}>
								<ListItemText primary={t('layout.menuitem.documents')} />
							</ListItem>
							{showDossiers && <ListItem button component={Link} to="/dossier" className={classes.button}>
								<ListItemText primary={t('layout.menuitem.dossiers')} />
							</ListItem>}
							<ListItem button onClick={handleClick2}>
								<ListItemText primary={t('layout.menuitem.housingwishes')} />
								{openSubMenu2 ? <ExpandLess /> : <ExpandMore />}
							</ListItem>
							<Collapse in={openSubMenu2} timeout="auto" unmountOnExit>
								<List component="div" disablePadding>
									<ListItem button component={Link} to="/keuzelijst" className={clsx(classes.nested, classes.button)}>
										<ListItemText primary={t('layout.menuitem.housingwishes.standardoptionslist')} />
									</ListItem>
									<ListItem button component={Link} to="/aangevraagd" className={clsx(classes.nested, classes.button)}>
										<ListItemText primary={
											<Badge badgeContent={dashboardCount && dashboardCount.quotationsCount} color="secondary">
												{t('layout.menuitem.housingwishes.requested')}
												&nbsp;
											</Badge>
										} />
									</ListItem>
									<ListItem button component={Link} to="/mijndefinitieveopties"
										className={clsx(classes.nested, classes.button)}>
										<ListItemText primary={t('layout.menuitem.housingwishes.finaloptions')} />
									</ListItem>
								</List>
							</Collapse>
							{/* <ListItem button component={Link} to="/dossiers">
                                <ListItemText primary={t('Dossiers')} />
                            </ListItem> */}
						</React.Fragment>
					}
					{
						isBuyer === false &&
						<React.Fragment>
							<ListItem button onClick={handleClick3}>
								<ListItemText primary={t('layout.menuitem.configuration')} />
								{openSubMenu2 ? <ExpandLess /> : <ExpandMore />}
							</ListItem>
							<Collapse in={openSubMenu3} timeout="auto" unmountOnExit>
								<List component="div" disablePadding>
									<ListItem button component={Link} to="/standaard-opties"
										className={clsx(classes.nested, classes.button)}>
										<ListItemText primary={t('layout.menuitem.configuration.standardoptions')} />
									</ListItem>
									{
										//<ListItem button component={Link} to="/individuele-opties" className={classes.nested}>
										//    <ListItemText primary={t('layout.menuitem.configuration.individualoptions')} />
										//</ListItem>
									}
								</List>
							</Collapse>
						</React.Fragment>
					}
				</React.Fragment>
			}
		</List>
	);

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
						<Avatar alt={userInitials}
							src={webApiUrl + "api/home/GetPersonPhoto/" + user.personId}>{userInitials}</Avatar>
						{
							//<Avatar>
							//<AccountCircle />
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
				<div>
					<ListItem>
						<Grid container wrap="nowrap" alignItems="center" justify="center">
							<SelectProject className={classes.grow} />

						</Grid>
					</ListItem>
					{
						<ListItem>
							<Grid container wrap="nowrap" alignItems="center" justify="center">
								<Grid item>
									<Typography variant="body1"
										color="inherit">{app === apps.after ? t('layout.navbar.address') : t('layout.navbar.buildnumber')}: &nbsp;</Typography>
								</Grid>
								<Grid item xs zeroMinWidth>
									<SelectBuilding className={classes.selectBuilding} />
								</Grid>
							</Grid>
						</ListItem>
					}
					<Divider />

					{
						//    <MenuItem onClick={handleMenuClose} className={classes.button} component={Link} to="/berichten">
						//    <Badge badgeContent={messageCount} color="secondary">
						//        <Chat />
						//        &nbsp;
						//    <span>{t('layout.navbar.messages')}</span>
						//    </Badge>
						//</MenuItem>
						//<MenuItem onClick={handleMenuClose}>
						//    <Badge badgeContent={newsCount} color="secondary">
						//        <LibraryBooks />
						//        &nbsp;
						//<span>{t('layout.navbar.news')}</span>
						//    </Badge>
						//</MenuItem>
						//<MenuItem onClick={handleMenuClose}>
						//    <Badge invisible={true} badgeContent={messageCount} color="secondary">
						//        <NotificationsIcon />
						//        &nbsp;
						//    <span>{t('layout.navbar.systemmessages')}</span>
						//    </Badge>
						//</MenuItem>
						//<Divider />
					}
				</div>
			}
			{
				//<MenuItem onClick={handleMenuClose}>
				//<AccountCircle />
				//&nbsp;
				//<span>{t('layout.navbar.usersettings')}</span></MenuItem>
			}
			{
				//<MenuItem onClick={handleMenuClose}>
				//    <SettingsIcon />
				//    &nbsp;
				//<span>{t('layout.navbar.settings')}</span></MenuItem>
			}
			{
				//<LanguageSelector handleClose={handleMenuClose} />
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
			<MenuItem onClick={() => {
				if (user.viewOnly !== true) history.push('/login')
			}}>
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
					<Container className={classes.appBarContainer}>
						<Grid container alignItems="center">
							{
								hideDrawerMenu !== true &&
								<>
									<IconButton
										edge="start"
										className={classes.sectionMobile}
										color="inherit"
										aria-label="Open drawer"
										onClick={handleDrawerToggle}
									>
										<Badge badgeContent={dashboardCount && dashboardCount.quotationsCount} color="secondary">
											<MenuIcon />
										</Badge>
									</IconButton>
									<Button
										className={clsx(classes.sectionDesktop, classes.menuButton)}
										color="inherit"
										aria-label="Open drawer"
										onClick={handleDrawerToggle}
									>
										<Badge badgeContent={dashboardCount && dashboardCount.quotationsCount} color="secondary">
											<MenuIcon />
											&nbsp;
											{t('Menu')}
											&nbsp;
										</Badge>
									</Button>
								</>
							}
							<Link to={linkToHome} className={classes.logoAnchor}>
								<img src={logo} alt='JPDS' className={classes.logo} />
							</Link>
							<div className={classes.sectionDesktop}>

							</div>
							<div className={classes.grow} />
							<div style={{ display: 'flex' }} /*className={classes.sectionDesktop}*/>
								{
									app === apps.huisinfo &&
									<React.Fragment>
										<IconButton aria-label="Berichten" color="inherit" className={classes.button} component={Link}
											to="/berichten">
											<Badge badgeContent={dashboardCount && dashboardCount.messageCount} color="secondary">
												<Chat />
											</Badge>
										</IconButton>
										{
											//<IconButton aria-label="Nieuws" color="inherit">
											//<Badge badgeContent={newsCount} color="secondary">
											//    <LibraryBooks />
											//</Badge>
											//</IconButton>
										}
										<Hidden only="xs">
											<IconButton aria-label="Systeemberichten" color="inherit" className={classes.button}
												component={Link}
												to={{
													pathname: '/berichten',
													state: {
														showImportantMessages: true
													}
												}}>
												<Badge badgeContent={dashboardCount && dashboardCount.systemMessageCount} color="primary">
													<Bookmarks />
												</Badge>
											</IconButton>
										</Hidden>
										{
											isBuyer === true &&
											<IconButton aria-label="ShoppingCart" color="inherit" className={classes.button} component={Link}
												to={{
													pathname: '/keuzelijst',
													state: {
														bottomCartExpanded: dashboardCount && dashboardCount.selectedOptionsCount > 0
													}
												}}>
												<Badge badgeContent={dashboardCount && dashboardCount.selectedOptionsCount} color="secondary">
													<ShoppingBasket />
												</Badge>
											</IconButton>
										}
									</React.Fragment>
								}
								{
									userApps.length > 1 &&
									<IconButton aria-label="Apps" color="inherit" className={classes.button} component={Link} to="/">
										<Apps />
									</IconButton>
								}
								<IconButton
									edge="end"
									aria-label={user.name}
									aria-controls={menuId}
									aria-haspopup="true"
									onClick={handleProfileMenuOpen}
									color="inherit"
								>
									<Avatar className={classes.buttonAvatar} alt={userInitials}
										src={webApiUrl + "api/home/GetPersonPhoto/" + user.personId}>
										<AccountCircle />
									</Avatar>
								</IconButton>
							</div>
							{
								//<div className={classes.sectionMobile}>
								//    <IconButton
								//        edge="end"
								//        aria-label="Account of current user"
								//        aria-controls={menuId}
								//        aria-haspopup="true"
								//        onClick={handleProfileMenuOpen}
								//        color="inherit"
								//    >
								//        <Badge badgeContent={messageCount + newsCount + systemMessageCount} color="secondary">
								//            <AccountCircle />
								//        </Badge>
								//    </IconButton>
								//</div>
							}
						</Grid>
					</Container>
				</Toolbar>
			</AppBar>
			{matches &&
				<AppBar className={classes.projectAppbar} position="fixed">
					<Toolbar className={classes.projectToolbar} variant="dense">
						<Container>
							<Grid container alignItems="center">
								<Typography variant="body1" color="inherit">{t('layout.navbar.project')}: &nbsp;</Typography>
								<SelectProject className={classes.selectBox} />
								<div className={classes.grow} />
								{
									//isBuyer===true &&
									<React.Fragment>
										<Typography variant="body1"
											color="inherit">{app === apps.aftercare ? t('layout.navbar.address') : t('layout.navbar.buildnumber')}: &nbsp;</Typography>
										<SelectBuilding className={classes.selectBox} />
									</React.Fragment>
								}
							</Grid>
						</Container>
					</Toolbar>
				</AppBar>
			}
			{renderMenu}
			{
				//!matches &&
				<Drawer open={menuOpen} onClose={handleDrawerToggle}>
					<div
						className={classes.list}
						role="presentation"
						onClick={handleDrawerToggle}
						onKeyDown={handleDrawerToggle}
					>
						<div className={classes.drawerHeader}>
							<IconButton onClick={handleDrawerToggle}>
								<ChevronLeftIcon />
							</IconButton>
						</div>
						<Divider />
						{renderTopMenuList}
					</div>
				</Drawer>
			}
			{changePwdOpen && <ChangePasswordModal onChangePasswordClose={changePwdClose} changePwdOpen={changePwdOpen} />}
		</div>
	);
}
