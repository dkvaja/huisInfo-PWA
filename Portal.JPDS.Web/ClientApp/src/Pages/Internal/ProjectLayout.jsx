import React from "react";
import {connect} from "react-redux";
import {withTranslation} from 'react-i18next';
import {commonActions} from "../../_actions";
import {history as History} from "../../_helpers";
import {withRouter} from 'react-router-dom';

const {webApiUrl} = window.appConfig;

class Layout extends React.Component {
	state = {
		actions: [],
		plannings: [],
		messages: []
	};

	componentDidMount() {
		const {user, selected} = this.props;
		this.UpdateSelectedBuilding()

		var intervalId = setInterval(this.timer, 10000);
		// store intervalId in the state so it can be accessed later:
		this.setState({intervalId: intervalId});
	}

	componentWillUnmount() {
		// use intervalId from the state to clear the interval
		clearInterval(this.state.intervalId);
	}

	componentDidUpdate(prevProps, prevState) {
		const {selected, match, history} = this.props;
		const {projectNo} = match.params;
		if (!prevProps.selected && projectNo !== selected.projectId) {
			this.UpdateSelectedBuilding();
		}
		if (!!prevProps.selected && prevProps.selected.buildingId !== selected.buildingId) {
			var url = match.url + '/';
			url = url.replace('/werk/' + prevProps.selected.projectNo + '/', '/werk/' + selected.projectNo + '/');
			url = url.substr(0, url.length - 1);

			if (!!history && !!history.location && history.location.pathname === url) {
				History.push(url, history.location.state);
			} else {
				History.push(url);
			}
		}
	}

	UpdateSelectedBuilding() {
		const {dispatch, selected, buildings, match} = this.props;
		const {projectNo} = match.params;
		if (selected && selected.projectNo.toUpperCase() != projectNo.toUpperCase()) {
			var selectedItems = buildings.filter(x => x.projectNo.toUpperCase() == projectNo.toUpperCase())
			if (selectedItems && selectedItems.length > 0) {
				dispatch(commonActions.selectBuilding(selectedItems[0]));
			} else {
				History.push('/home')
			}
		}
	}


	render() {
		const {classes} = this.props;
		const {} = this.state;

		return this.props.children;
	}
}

function mapStateToProps(state) {
	const {authentication, buildings, app, dashboardCount} = state;
	const {user} = authentication;
	const {selected, all} = buildings;
	const {messageCountPerBuilding} = dashboardCount;
	return {
		user,
		selected,
		buildings: all, app,
		messageCountPerBuilding
	};
}

const connectedLayout = connect(mapStateToProps)(withTranslation()(withRouter(Layout)));
export {connectedLayout as ProjectLayout};
