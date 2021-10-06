import React, { Component } from 'react';
import { connect } from "react-redux";
import { MenuItem, Select } from '@material-ui/core';
import { commonActions } from "../_actions";

class SelectProject extends Component {
    constructor() {
        super();

        this.renderOptions = this.renderOptions.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.stopPropagation = this.stopPropagation.bind(this);
    }

    handleChange(event) {
        var selectedItem = this.props.buildings.all.filter(x => x.buildingId === event.target.value)[0];
        this.props.dispatch(commonActions.selectBuilding(selectedItem));
    }
    stopPropagation(event) {
        event.stopPropagation();
    }

    //we are creating the options to be displayed
    renderOptions() {

    }

    render() {
        let selectedValue = '';
        let projectsSoFar = [];
        if (this.props.buildings.all) {
            this.props.buildings.all.map((building, index) => {
                if (projectsSoFar.filter(x => x.projectNo === building.projectNo).length === 0) {
                    projectsSoFar.push(building);
                    if (this.props.buildings.selected.projectNo === building.projectNo) {
                        selectedValue = building.buildingId;
                    }
                }
            });
        }

        return (
            <Select value={selectedValue} onChange={this.handleChange} onClick={this.stopPropagation} className={this.props.className} style={{ maxWidth:'100%' }}>
                {projectsSoFar.map((dt, i) => (
                    <MenuItem key={i} value={dt.buildingId}>{dt.projectName}</MenuItem>
                ))}
            </Select>
        );
    }
}


function mapStateToProps(state) {
    const { authentication, buildings } = state;
    const { user } = authentication;
    return {
        user,
        buildings
    };
}

export default connect(mapStateToProps)(SelectProject);