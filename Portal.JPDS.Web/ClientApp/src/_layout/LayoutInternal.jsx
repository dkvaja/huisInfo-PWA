import React from "react";
import { connect } from "react-redux";
import HeaderInternal from "./HeaderInternal";
import { CssBaseline } from "@material-ui/core";
import { withStyles } from '@material-ui/core/styles';
import { commonActions } from "../_actions";
import { withTranslation } from "react-i18next";
import { authHeader, getUser, history } from "../_helpers";
import { apps, internalLayoutViewConstants, userAccountTypeConstants } from "../_constants";
import MenuBar from "./MenuBar";
import { dossiersActions } from "../_actions/dossiers.actions";

const { webApiUrl } = window.appConfig;

const styles = theme => ({
	//common for most pages with fixed height
	layoutRoot: {
		minHeight: '100vh'
	},
	mainElement: {
		height: "calc(100% - 64px)",
		position: "fixed",
		overflowX: "hidden",
		overflowY: "auto",
		width: "100%",
		paddingLeft: 72,
		['@media (orientation: portrait)']: {
			paddingBottom: 56,
			paddingLeft: 0
		},
		[theme.breakpoints.down("sm")]: {
			height: "calc(100% - 64px)",
		},
		[theme.breakpoints.down("xs")]: {
			paddingBottom: 56,
			paddingLeft: 0,
			height: "calc(100% - 56px)",
		},
		[`${theme.breakpoints.down("xs")} and (orientation: landscape)`]: {
			height: "calc(100% - 48px)"
		}
	},
})

class LayoutInternal extends React.Component {
	state = {
		recentUnreadMessages: null,
		recentSavedMessages: null,
		updateSavedMessages: true
	};

	componentDidMount() {
		const { dispatch, user, app } = this.props;

		if (user && user.id) {
			this.UpdateRecentUnreadMessages();
			this.UpdateRecentSavedMessages();
			if (app !== null) {
				dispatch(commonActions.getBuildings(app));
			}
		}

		//Change document title here to customize based on user
		//document.title = "Custom title here";
		var intervalId = setInterval(this.timer, 10000);
		// store intervalId in the state so it can be accessed later:
		this.setState({ intervalId: intervalId });
	}

	componentWillUnmount() {
		// use intervalId from the state to clear the interval
		clearInterval(this.state.intervalId);
	}

	componentDidUpdate(prevProps, prevState) {
		const { dispatch, user, selected, app } = this.props;
		if (user && user.id && app !== null && (!prevProps.user || prevProps.user.id !== user.id || prevProps.app !== app)) {
			dispatch(commonActions.getBuildings(app));
		}
		if (!prevProps.selected || prevProps.selected.buildingId !== this.props.selected.buildingId) {
			if (user.type === userAccountTypeConstants.buyer && selected) {
				this.props.dispatch(dossiersActions.getDossiersByBuildingId(selected.buildingId));
			} else if (selected)
				this.props.dispatch(dossiersActions.getAllDossiersByProject(selected.projectId));
			this.UpdateDashboardCount()
		}
	}

	timer = () => {
		if (!!this.state.intervalId) {
			this.UpdateDashboardCount();
			this.CheckLoginStatus();
			this.UpdateRecentUnreadMessages();
			this.UpdateRecentSavedMessages();
		}
	}

	UpdateRecentUnreadMessages() {
		const { app } = this.props;
		if (app === apps.huisinfo) {
			const url = webApiUrl + 'api/chat/GetTopUnreadMessages';
			const requestOptions = {
				method: 'GET',
				headers: authHeader()
			};

			fetch(url, requestOptions)
				.then(Response => Response.json())
				.then(findResponse => {
					this.setState({
						recentUnreadMessages: findResponse
					});
				});
		}
	}

	UpdateRecentSavedMessages() {
		const { app } = this.props;
		if (app === apps.huisinfo) {
			if (this.state.updateSavedMessages === true) {
				const url = webApiUrl + 'api/chat/GetTopSavedMessages';
				const requestOptions = {
					method: 'GET',
					headers: authHeader()
				};

				fetch(url, requestOptions)
					.then(Response => Response.json())
					.then(findResponse => {
						this.setState({
							recentSavedMessages: findResponse,
							updateSavedMessages: false
						});
					});
			} else {
				this.setState({ updateSavedMessages: true });
			}
		}
	}

	CheckLoginStatus() {
		var localUser = getUser();
		if (!localUser) {
			history.push('/login');
		}
	}

	UpdateDashboardCount() {
		const { selected, user, app, dispatch } = this.props;
		if (!!selected && !!user && app != null) {
			dispatch(commonActions.getDashboardCount(app, selected));
		}
	}

	onScroll = () => {
		window.scrollTo(0, 1)
	}

	render() {
		const { alert, selected, user, classes, dashboardCount, app, history } = this.props;
		const { recentUnreadMessages, recentSavedMessages } = this.state;

		let layoutView = internalLayoutViewConstants.Project;
		if (history.location.pathname.startsWith('/object/')) {
			layoutView = internalLayoutViewConstants.Building;
		}

		let totalUnreadChats = 0, totalUnreadChatMessages = 0, totalCountSavedMessages = 0;

		if (selected && dashboardCount) {
			if (dashboardCount.messageCountPerBuilding) {
				var objects = dashboardCount.messageCountPerBuilding
					.filter(x => x.projectId.toUpperCase() === selected.projectId.toUpperCase());

				totalUnreadChats = objects.length > 0 ? objects.reduce((prev, next) => prev + next.noOfChats, 0) : 0;

				totalUnreadChatMessages = objects.length > 0 ? objects.reduce((prev, next) => prev + next.count, 0) : 0;
			}

			if (dashboardCount.savedMessagesCountPerBuilding) {
				var objects = dashboardCount.savedMessagesCountPerBuilding
					.filter(x => x.projectId.toUpperCase() === selected.projectId.toUpperCase());

				totalCountSavedMessages = objects.length > 0 ? objects.reduce((prev, next) => prev + next.count, 0) : 0;
			}
		}

		const dashboardCountObj = { totalUnreadChats, totalUnreadChatMessages, totalCountSavedMessages };
		return (
			user ?
				//Id layout-root is used for scroll position
				<div id="layout-root" className={classes.layoutRoot}>
					<CssBaseline />
					<HeaderInternal user={user} layoutView={layoutView} selectedBuilding={selected}
						app={app}
						dashboardCount={dashboardCountObj} recentUnreadMessages={recentUnreadMessages}
						recentSavedMessages={recentSavedMessages} />
					<main className={classes.mainElement} onScroll={this.onScroll}>
						<MenuBar user={user} layoutView={layoutView} selectedBuilding={selected} app={app}
							history={history} dashboardCount={dashboardCountObj} />

						{this.props.children}
					</main>
				</div>
				:
				this.props.children
		);
	}
}

function mapStateToProps(state) {
	const { alert, buildings, dashboardCount, app } = state;
	const { selected, all } = buildings;
	return {
		alert,
		selected,
		dashboardCount,
		app
	};
}

const connectedLayout = connect(mapStateToProps)(withTranslation()(withStyles(styles)(LayoutInternal)));
export { connectedLayout as LayoutInternal }
