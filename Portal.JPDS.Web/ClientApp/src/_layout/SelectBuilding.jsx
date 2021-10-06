import React, { Component } from 'react';
import { connect } from "react-redux";
import { MenuItem, Select } from '@material-ui/core';
import { commonActions } from "../_actions";
import { history } from '../_helpers';
import { userAccountTypeConstants, apps } from '../_constants';

class SelectBuilding extends Component {
    constructor() {
        super();

        this.renderOptions = this.renderOptions.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.stopPropagation = this.stopPropagation.bind(this);
    }

    handleChange(event) {
        var selectedItem = this.props.buildings.all.filter(x => x.buildingId === event.target.value)[0];
        this.props.dispatch(commonActions.selectBuilding(selectedItem));

        //if (this.props.app === apps.huisinfo && this.props.user.type !== userAccountTypeConstants.buyer) {
        //    history.push('/gebouw-overzicht')
        //}
    }
    stopPropagation(event) {
        event.stopPropagation();
    }

    //we are creating the options to be displayed
    renderOptions() {

    }

    render() {
        const { buildings, app } = this.props;
        const { selected } = buildings;
        return (
            <Select
                className={this.props.className} style={{ maxWidth: '100%' }}
                value={selected ? selected.buildingId : ``} onChange={this.handleChange} onClick={this.stopPropagation}>
                {buildings.all && buildings.all.filter(x => x.projectNo === buildings.selected.projectNo).map((dt, i) => (
                    <MenuItem key={i} value={dt.buildingId}>{app === apps.aftercare ? (!!dt.address ? dt.address : 'Bouwnummer ' + dt.buildingNoExtern) : dt.buildingNoExtern}</MenuItem>
                ))}
            </Select>
        );
    }
}


function mapStateToProps(state) {
    const { authentication, buildings, app } = state;
    const { user } = authentication;
    return {
        user,
        buildings,
        app
    };
}

export default connect(mapStateToProps)(SelectBuilding);