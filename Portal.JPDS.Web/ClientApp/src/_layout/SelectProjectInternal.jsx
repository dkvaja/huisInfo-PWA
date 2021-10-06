import React, { Component } from 'react';
import { connect } from "react-redux";
import { MenuItem, Select, GridList, ButtonBase, IconButton, Popover, GridListTile, GridListTileBar, ListSubheader, Badge, Tooltip } from '@material-ui/core';
import { commonActions } from "../_actions";
import { HomeWork, ChatOutlined, ForumOutlined, BookmarksOutlined } from '@material-ui/icons';
import { apps, internalLayoutViewConstants } from '../_constants';
import { groupBy, history } from '../_helpers';
import MessagesDetailsPopover from './MessagesDetailsPopover';
import { Link } from 'react-router-dom';
import { withTranslation } from 'react-i18next';

const { webApiUrl } = window.appConfig;

class SelectProjectInternal extends Component {
    state = {
        anchorEl: null
    }

    constructor() {
        super();

        this.handleChange = this.handleChange.bind(this);
        this.stopPropagation = this.stopPropagation.bind(this);
    }

    handleChange(buildingId) {
        var selectedItem = this.props.buildings.all.filter(x => x.buildingId === buildingId)[0];
        this.props.dispatch(commonActions.selectBuilding(selectedItem));

        if (this.props.layoutView !== internalLayoutViewConstants.Project) {
            history.push('/home')
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

    getChatsCount = projectId => {
        const { messageCountPerBuilding } = this.props;

        if (messageCountPerBuilding) {
            return messageCountPerBuilding
                .filter(x => x.projectId.toUpperCase() === projectId.toUpperCase())
                .reduce((prev, next) => prev + next.noOfChats, 0);
        }
        return 0;
    }

    getMessagesCount = projectId => {
        const { messageCountPerBuilding } = this.props;

        if (messageCountPerBuilding) {
            return messageCountPerBuilding
                .filter(x => x.projectId.toUpperCase() === projectId.toUpperCase())
                .reduce((prev, next) => prev + next.count, 0);
        }
        return 0;
    }

    getSavedMessagesCount = projectId => {
        const { savedMessagesCountPerBuilding } = this.props;

        if (savedMessagesCountPerBuilding) {
            return savedMessagesCountPerBuilding
                .filter(x => x.projectId.toUpperCase() === projectId.toUpperCase())
                .reduce((prev, next) => prev + next.count, 0);
        }
        return 0;
    }

    render() {
        const { t, recentUnreadMessages, recentSavedMessages, app, user } = this.props;
        let selectedValue = '';
        let projectsSoFar = [];
        if (this.props.buildings.all) {
            projectsSoFar = Object.values(groupBy(this.props.buildings.all, 'projectId'))
                .map(p => {
                    return p.find(b => b.roles && b.roles.some(r => r === 'BuyersGuide' || r === 'Spectator')) || p[0]
                    //    {...p[0], isBuyerGuide: p.find(b => b.roles && b.roles.some(r=>r==='BuyersGuide'||'Spectator'))};
                }).flat();
            //projectsSoFar = this.props.buildings.all;
        }

        const totalCount = this.props.messageCountPerBuilding
            ?
            this.props.messageCountPerBuilding
                .filter(x => projectsSoFar.filter(p => p.roles && p.roles.some(r => r === 'BuyersGuide' || r === 'Spectator')).map(y => y.projectId).includes(x.projectId))
                .reduce((prev, next) => prev + next.count, 0)
            :
            0;

        const open = Boolean(this.state.anchorEl);
        const id = open ? 'project-popover' : undefined;
        const visibleBadge = !projectsSoFar.some(p => p.roles && p.roles.some(r => r === 'BuyersGuide' || r === 'Spectator'));
        return (
            //<Select value={selectedValue} onChange={this.handleChange} onClick={this.stopPropagation} className={this.props.className} style={{ maxWidth:'100%' }}>
            //    {projectsSoFar.map((dt, i) => (
            //        <MenuItem key={i} value={dt.buildingId}>{dt.projectName}</MenuItem>
            //    ))}
            //</Select>
            <React.Fragment>
                <Tooltip title={t('Projectwissel')}>
                    <IconButton color="inherit" aria-describedby={id} onClick={this.handleClick}>
                        <Badge badgeContent={totalCount} invisible={!totalCount || visibleBadge} inlist color="primary">
                            <HomeWork />
                        </Badge>
                    </IconButton>
                </Tooltip>
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
                    <GridList cellHeight={100} style={{ maxWidth: 300, margin: "4px 0", maxHeight: 'calc(100vh - 80px)' }}>
                        <GridListTile key="Subheader" cols={2} style={{ height: 'auto', padding: 0 }}>
                            <ListSubheader component="div" style={{ fontWeight: "bold", lineHeight: '24px' }}>Werk</ListSubheader>
                        </GridListTile>
                        {
                            projectsSoFar.map((project, index) => (
                                <GridListTile key={index} cols={2}
                                    style={
                                        {
                                            backgroundImage: 'url(' + webApiUrl + 'api/home/ProjectBackground/' + project.projectId + '),url(' + webApiUrl + 'api/Config/WebBackground)',
                                            backgroundSize: 'cover',
                                            backgroundPosition: 'center',
                                            padding: 0,
                                            margin: '1px 4px',
                                            width: 'calc(100% - 8px)'
                                        }
                                    }
                                >
                                    {
                                        app === apps.huisinfo && project.roles && project.roles.some(r => r === 'BuyersGuide' || r === 'Spectator') &&
                                        <div style={{ position: 'absolute', right: 0, padding: 8, zIndex: 1 }}>
                                            <IconButton aria-label="Berichten" color="inherit" style={{ color: "inherit" }} component={Link} to={'/werk/' + project.projectNo + '/berichten'}>
                                                <Badge badgeContent={this.getChatsCount(project.projectId)} color="primary">
                                                    <ForumOutlined />
                                                </Badge>
                                            </IconButton>
                                            <MessagesDetailsPopover
                                                onClose={this.handleClose}
                                                messages={recentUnreadMessages ? recentUnreadMessages.filter(x => x.projectId === project.projectId) : []}
                                                projectNo={project.projectNo}
                                                projectName={project.projectName}
                                                badgeContent={this.getMessagesCount(project.projectId)} badgeColor="secondary">
                                                <ChatOutlined />
                                            </MessagesDetailsPopover >

                                            <MessagesDetailsPopover
                                                handleChange={() => this.handleChange(project.buildingId)}
                                                isSavedMessages={true}
                                                onClose={this.handleClose}
                                                messages={recentSavedMessages ? recentSavedMessages.filter(x => x.projectId === project.projectId) : []}
                                                projectNo={project.projectNo}
                                                projectName={project.projectName}
                                                badgeContent={this.getSavedMessagesCount(project.projectId)} badgeColor="primary">
                                                <BookmarksOutlined />
                                            </MessagesDetailsPopover >
                                        </div>
                                    }
                                    <ButtonBase style={{ height: '100%' }} onClick={() => this.handleChange(project.buildingId)}>
                                        <div style={{ width: 300, maxWidth: '100%', height: 1 }} />
                                        <GridListTileBar
                                            title={project.projectName}
                                        />
                                    </ButtonBase>
                                </GridListTile>
                            ))
                        }
                    </GridList>
                </Popover>
            </React.Fragment>
        );
    }
}


function mapStateToProps(state) {
    const { authentication, buildings, dashboardCount, app } = state;
    const { user } = authentication;
    const { messageCountPerBuilding, savedMessagesCountPerBuilding } = dashboardCount;
    return {
        user,
        buildings,
        messageCountPerBuilding,
        savedMessagesCountPerBuilding,
        app
    };
}

export default connect(mapStateToProps)(withTranslation()(SelectProjectInternal));
