import React from "react";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import { Avatar, Container, Grid, Typography, Divider, Card, CardContent, CardHeader, List, ListItem, ListItemText, ListItemAvatar, Badge, Modal, Backdrop, Fade, IconButton, CardMedia, Dialog, DialogContent, DialogTitle, Box, AppBar, Toolbar, Button, withWidth, isWidthDown } from "@material-ui/core";
import { withStyles, withTheme } from '@material-ui/core/styles';
import { Schedule, Chat, LibraryBooks, Assignment, Close, ArrowBackIos, ArrowForwardIos, KeyboardArrowLeft, KeyboardArrowRight, LocalOffer, AttachFile } from "@material-ui/icons"
import { getDateText, md2plaintext, getDataTableTextLabels } from "../../_helpers"
import { userAccountTypeConstants } from "../../_constants"
import clsx from "clsx";
import { withTranslation } from 'react-i18next';
import MUIDataTable from "mui-datatables";

const { webApiUrl } = window.appConfig;

const styles = theme => ({
    grow: {
        flexGrow: 1
    },
    bold: {
        fontWeight: "bold"
    },
    mainContainer: {
        height: '100%',
        width: '100%',
        overflow: 'auto',
        padding: 0
    },
    dataTable: {
        height: '100%',
        display: 'flex',
        flexDirection: 'column',
        overflow: 'hidden',
        '& > .MuiPaper-root': {
            flex: '0 1 auto',
        },
        '& > .MuiToolbar-root.MuiToolbar-regular': {
            display: 'flex',
            '& > div': {
                padding: 0,
                textAlign: 'right',
                flex: '1 0 auto',
                '& .MuiTypography-root': {
                    textAlign: 'left'
                }
            }
        },
        '& .MuiTableCell-root': {
            padding: theme.spacing(0),
            '&.MuiTableCell-paddingCheckbox': {
                paddingLeft: theme.spacing(0.5),
                '& > div': {
                    justifyContent: 'center'
                },
                '& .MuiCheckbox-root': {
                    padding: theme.spacing(0.25)
                }
            },
            '&.MuiTableCell-head': {
                fontWeight: 'bold'
            }
        },
    },
    fullWidth: {
        width: '100%'
    },
});


class Page extends React.Component {
    state = {
        buildings: [],
        columnsVisibility: [],
        rowsPerPage: 50
    };

    componentDidMount() {
        const { user, selected } = this.props;

        var rowsPerPage = parseInt(localStorage.getItem('Objects_RowsPerPage'));
        if (!!rowsPerPage && rowsPerPage > 0 && rowsPerPage !== this.state.rowsPerPage) {
            this.setRowsPerPage(rowsPerPage);
        }

        if (selected) {
            this.UpdateBuildings();
        }
        var intervalId = setInterval(this.timer, 10000);
        // store intervalId in the state so it can be accessed later:
        this.setState({ intervalId: intervalId });
    }

    componentWillUnmount() {
        // use intervalId from the state to clear the interval
        clearInterval(this.state.intervalId);
    }

    componentDidUpdate(prevProps, prevState) {
        const { selected, messageCountPerBuilding } = this.props;
        if (!prevProps.selected || prevProps.selected.projectId !== selected.projectId) {
            this.UpdateBuildings();
        }
        if (this.state.buildings && prevProps.messageCountPerBuilding !== messageCountPerBuilding) {
            const buildings = this.state.buildings;

            buildings.forEach((building, index) => {
                building.messageCount = 0;
                if (messageCountPerBuilding) {
                    var obj = messageCountPerBuilding.find(x => x.buildingId.toUpperCase() === building.buildingId.toUpperCase());
                    if (obj)
                        building.messageCount = obj.count;
                }
            })

            this.setState({ buildings })
        }
    }

    timer = () => {
        //this.UpdateBuildings();
    }

    setRowsPerPage = rowsPerPage => {
        this.setState({ rowsPerPage });

        localStorage.setItem("Objects_RowsPerPage", rowsPerPage);

    }

    isColumnVisible = columnName => {
        const { width } = this.props;
        const { columnsVisibility } = this.state;
        const isSmallScreen = isWidthDown('sm', width);
        var column = columnsVisibility.find(x => x.name === columnName);
        if (column) {
            return column.visible;
        }
        else if (isSmallScreen) {
            return false;
        }
        else {
            return true;
        }
    }

    UpdateBuildings() {
        const { selected, buildings, messageCountPerBuilding } = this.props;

        if (selected) {
            this.setState({ buildings: buildings ? buildings.filter(x => x.projectId === selected.projectId) : [] });
        }
    }

    render() {
        const { t, classes, width } = this.props;
        const { buildings, columnsVisibility, rowsPerPage } = this.state;
        const isExtraSmallScreen = isWidthDown('xs', width);

        const columns = [
            {
                name: 'buildingId',
                options: {
                    display: 'excluded',
                    filter: false,
                    print: false,
                    download: false
                }
            },
            {
                name: 'buildingNoIntern',
                label: t('Bouwnummer'),
                options: {
                    filter: false,
                    sort: true,
                    customBodyRender: v =>
                        <Button style={{ padding: 0 }} component={Link} to={"/object/" + v} color="primary">{v}</Button>
                }
            },
            {
                name: 'propertyType',
                label: t('Woning type'),
                options: {
                    display: this.isColumnVisible('propertyType'),
                    filter: true,
                    filterType: 'multiselect',
                    sort: true,
                }
            },
            {
                name: 'buildingType',
                label: t('Gebouw soort'),
                options: {
                    display: this.isColumnVisible('buildingType'),
                    filter: true,
                    sort: true,
                }
            },
            {
                name: 'constructionFlow',
                label: t('Bouwstroom'),
                options: {
                    display: this.isColumnVisible('constructionFlow'),
                    filter: true,
                    sort: true,
                }
            },
            {
                name: 'buyerRenterName',
                label: t('Koper / Huurder / Objectgebruiker'),
                options: {
                    display: this.isColumnVisible('buyerRenterName'),
                    filter: true,
                    sort: true,
                }
            },
            {
                name: 'address',
                label: t('Adres'),
                options: {
                    display: this.isColumnVisible('address'),
                    filter: true,
                    sort: true,
                }
            },
            {
                name: 'status',
                label: t('Status'),
                options: {
                    display: this.isColumnVisible('status'),
                    filter: true,
                    sort: true,
                }
            },
            {
                name: 'messageCount',
                label: t('Berichten'),
                options: {
                    display: this.isColumnVisible('messageCount'),
                    filter: false,
                    sort: true,
                }
            },
        ];
        const options = {
            filterType: 'dropdown',
            responsive: 'vertical',
            rowsPerPageOptions: [25, 50, 100],
            rowsPerPage: rowsPerPage,
            download: !isExtraSmallScreen,
            print: !isExtraSmallScreen,
            customToolbarSelect: selectedRows => (
                //<CustomToolbarSelect selectedRows={selectedRows} />
                <></>
            ),
            //selectableRows: 'none',
            //onRowClick: rowData => {
            //    var requestId = rowData[0];
            //    if (requestId) {
            //        history.push('/objecten/dashboard/' + rowData[0]);
            //    }
            //},
            onViewColumnsChange: (changedColumn, action) => {
                if (action == 'add') {
                    var listToUpdate = columnsVisibility.slice();
                    var column = listToUpdate.find(x => x.name === changedColumn);
                    if (column) {
                        column.visible = true;
                    }
                    else {
                        listToUpdate.push({ name: changedColumn, visible: true });
                    }
                    this.setState({ columnsVisibility: listToUpdate });
                }
                else if (action == 'remove') {
                    var listToUpdate = columnsVisibility.slice();
                    var column = listToUpdate.find(x => x.name === changedColumn);
                    if (column) {
                        column.visible = false;
                    }
                    else {
                        listToUpdate.push({ name: changedColumn, visible: false });
                    }
                    this.setState({ columnsVisibility: listToUpdate });
                }
            },
            textLabels: getDataTableTextLabels(t),
            onChangeRowsPerPage: rowsPerPage => {
                this.setRowsPerPage(rowsPerPage)
            },
        };

        return (
            <Container maxWidth={false} className={classes.mainContainer}>
                <MUIDataTable
                    className={classes.dataTable}
                    title={t('Objecten')}
                    data={buildings}
                    columns={columns}
                    options={options}
                />
            </Container >
        );
    }
}

function mapStateToProps(state) {
    const { authentication, buildings, dashboardCount } = state;
    const { user } = authentication;
    const { selected, all } = buildings;
    const { messageCountPerBuilding } = dashboardCount;
    return {
        user,
        selected,
        buildings: all,
        messageCountPerBuilding
    };
}

const connectedPage = connect(mapStateToProps)(withWidth()(withTranslation()(withStyles(styles)(Page))));
export { connectedPage as ObjectsPage };