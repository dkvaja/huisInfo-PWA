import React, { Component } from 'react';
import { connect } from "react-redux";
import { MenuItem, Select, GridList, Button, IconButton, Popover, GridListTile, GridListTileBar, ListSubheader, Grid, Badge, withStyles, Typography } from '@material-ui/core';
import { commonActions } from "../_actions";
import { House } from '@material-ui/icons';
import { internalLayoutViewConstants } from '../_constants';
import { history } from '../_helpers';

const { webApiUrl } = window.appConfig;

const StyledBadge = withStyles(theme => ({
    badge: {
        right: theme.spacing(-0.25),
        padding: theme.spacing(0, 0.5),
        minWidth: theme.spacing(2),
        height: theme.spacing(2),
    },
}))(Badge);


class SelectBuildingInternal extends Component {
    state = {
        anchorEl: null
    }

    constructor() {
        super();

        this.handleChange = this.handleChange.bind(this);
        this.stopPropagation = this.stopPropagation.bind(this);
    }

    handleChange(buildingId) {
        var selectedItem = this.props.buildings.buyerGuidesBuildings.filter(x => x.buildingId === buildingId)[0];
        this.props.dispatch(commonActions.selectBuilding(selectedItem));

        if (this.props.layoutView !== internalLayoutViewConstants.Building) {
            history.push('/object/' + selectedItem.buildingNoIntern)
        }

        this.handleClose();
    }

    stopPropagation(event) {
        event.stopPropagation();
    }


    handleClick = (event) => {
        this.setState({
            anchorEl: event.currentTarget,
        });
    };

    handleClose = () => {
        this.setState({
            anchorEl: null,
        });
    };

    getMessagesCount = buildingId => {
        const { messageCountPerBuilding } = this.props;

        if (messageCountPerBuilding) {
            var obj = messageCountPerBuilding.find(x => x.buildingId.toUpperCase() === buildingId.toUpperCase());
            if (obj)
                return obj.count;
        }

        return 0;
    }

    render() {
        let buildings = [];
        if (this.props.buildings.buyerGuidesBuildings && this.props.buildings.selected) {
            buildings = this.props.buildings.buyerGuidesBuildings.filter(x => x.projectId === this.props.buildings.selected.projectId);
        }

        const { selected } = this.props.buildings;
        const totalCount = selected && this.props.messageCountPerBuilding
            ?
            this.props.messageCountPerBuilding
                .filter(x => x.projectId.toUpperCase() === selected.projectId.toUpperCase())
                .reduce((prev, next) => prev + next.count, 0)
            :
            0;

        const open = Boolean(this.state.anchorEl);
        const id = open ? 'building-popover' : undefined;

        return (
            buildings.length ? <React.Fragment>
                <IconButton color="inherit" aria-describedby={id} onClick={this.handleClick}>
                    <Badge badgeContent={totalCount} color="secondary">
                        <House />
                    </Badge>
                </IconButton>
                <Popover
                    id={id}
                    open={open}
                    anchorEl={this.state.anchorEl}
                    onClose={this.handleClose}
                    anchorOrigin={{
                        vertical: 'bottom',
                        horizontal: 'center',
                    }}
                    transformOrigin={{
                        vertical: 'top',
                        horizontal: 'center',
                    }}
                >
                    <Grid container justify="center">
                        <Grid item container justify="center" style={{ maxWidth: 300, margin: "8px", maxHeight: 'calc(100vh - 80px)' }}>
                            <Typography component="div" variant="subtitle2" color="textSecondary" style={{ fontWeight: "bold", width: '100%', padding: '0 16px', lineHeight: '24px' }}>Objecten</Typography>
                            {
                                buildings.map((building, index) => (
                                    <Grid item key={index}>
                                        <Button key={index} onClick={() => this.handleChange(building.buildingId)} style={{ minWidth: 'auto' }}>
                                            <StyledBadge color="secondary" badgeContent={this.getMessagesCount(building.buildingId)}>
                                                {building.buildingNoExtern}
                                            </StyledBadge>
                                        </Button>
                                    </Grid>
                                ))
                            }

                        </Grid>
                    </Grid>
                </Popover>
            </React.Fragment> : null
        );
    }
}


function mapStateToProps(state) {
    const { authentication, buildings, dashboardCount } = state;
    const { user } = authentication;
    const { messageCountPerBuilding } = dashboardCount;
    return {
        user,
        buildings,
        messageCountPerBuilding
    };
}

export default connect(mapStateToProps)(SelectBuildingInternal);