import React from "react";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import { Avatar, Container, Grid, Typography, Divider, Card, CardContent, CardHeader, List, ListItem, ListItemText, ListItemAvatar, Badge, Modal, Backdrop, Fade, IconButton, CardMedia, Dialog, DialogContent, DialogTitle, Box, AppBar, Toolbar, Button, withWidth, isWidthDown, Tooltip } from "@material-ui/core";
import { withStyles, withTheme } from '@material-ui/core/styles';
import { Schedule, Chat, LibraryBooks, Assignment, Close, ArrowBackIos, ArrowForwardIos, KeyboardArrowLeft, KeyboardArrowRight, LocalOffer, AttachFile, PriorityHigh, LowPriority, ArrowRightAlt } from "@material-ui/icons"
import { getDateText, md2plaintext, getDataTableTextLabels, authHeader, formatDate, nl2br } from "../../_helpers"
import { userAccountTypeConstants } from "../../_constants"
import clsx from "clsx";
import { withTranslation } from 'react-i18next';
import MUIDataTable from "mui-datatables";
import { Alert } from "bootstrap";

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
    tooltipImageContainer: {
        //maxWidth: '50vw',
        "& > img": {
            width: '100%'
        }
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
            padding: theme.spacing(0, 0.5, 0, 0),
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
    thumbnail: {
        width: 50,
        height: 50,
        backgroundRepeat: 'no-repeat',
        backgroundPosition: 'center',
        backgroundSize: 'contain',
        backgroundColor: 'rgba(0,0,0,0.1)',
    },
    rotate45Clockwise: {
        transform: 'rotate(45deg)'
    },
    rotate45AntiClockwise: {
        transform: 'rotate(-45deg)'
    }
});


class Page extends React.Component {
    state = {
        workOrders: [],
        columnsVisibility: [
            { name: "repairRequestImages", visible: true },
            { name: "workOrderNumber", visible: true },
            { name: "description", visible: true },
            { name: "buildingNoIntern", visible: true },
            { name: "addressText", visible: true },
            { name: "address_postcode", visible: true },
            { name: "address_place", visible: true },
            { name: "targetCompletionDate", visible: true },
            { name: "status", visible: true },
        ],
        isLoading: true,
        rowsPerPage: 50,
    };

    componentDidMount() {
        const { user, selected } = this.props;
        let { columnsVisibility } = this.state;
        if (
            JSON.parse(
                localStorage.getItem("ResolverWorkOrders_ColumnsVisibility")
            ) == undefined
        ) {
            localStorage.setItem("ResolverWorkOrders_ColumnsVisibility", JSON.stringify(columnsVisibility))

        } else {
            columnsVisibility = JSON.parse(
                localStorage.getItem("ResolverWorkOrders_ColumnsVisibility")
            );
        }

        var rowsPerPage = parseInt(
            localStorage.getItem("ResolverWorkOrders_RowsPerPage")
        );
        if (
            !!rowsPerPage &&
            rowsPerPage > 0 &&
            rowsPerPage !== this.state.rowsPerPage
        ) {
            this.setRowsPerPage(rowsPerPage);
        }

        if (selected) {
            this.UpdateWorkOrders();
        }
        var intervalId = setInterval(this.timer, 10000);
        // store intervalId in the state so it can be accessed later:
        this.setState({ intervalId: intervalId, columnsVisibility });
    }

    componentWillUnmount() {
        // use intervalId from the state to clear the interval
        clearInterval(this.state.intervalId);
    }

    componentDidUpdate(prevProps, prevState) {
        const { selected, messageCountPerBuilding } = this.props;
        if (
            !prevProps.selected ||
            prevProps.selected.projectId !== selected.projectId
        ) {
            this.UpdateWorkOrders();
        }
    }

    timer = () => {
        //this.UpdateBuildings();
    };

    setRowsPerPage = (rowsPerPage) => {
        this.setState({ rowsPerPage });

        localStorage.setItem("ResolverWorkOrders_RowsPerPage", rowsPerPage);
    };

    isColumnVisible = (columnName) => {
        const { width } = this.props;
        const { columnsVisibility } = this.state;
        const isSmallScreen = true; //isWidthDown('sm', width);
        var column = columnsVisibility.find((x) => x.name === columnName);
        if (column) {
            return column.visible;
        } else if (isSmallScreen) {
            return false;
        } else {
            return true;
        }
    };

    UpdateWorkOrders() {
        const { t, selected, buildings } = this.props;
        if (selected) {
            const url =
                webApiUrl +
                "api/RepairRequest/GetWorkOrdersByProjectId/" +
                encodeURI(selected.projectId);
            const requestOptions = {
                method: "GET",
                headers: authHeader(),
            };

            fetch(url, requestOptions)
                .then((Response) => Response.json())
                .then((findResponse) => {
                    findResponse.forEach((workOrder, index) => {
                        var building = buildings.find(
                            (x) => x.buildingId === workOrder.buildingId
                        );
                        workOrder.buildingNoIntern = building && building.buildingNoIntern;

                        if (workOrder.address) {
                            workOrder.addressText =
                                (!!workOrder.address.street ? workOrder.address.street : "") +
                                " " +
                                (!!workOrder.address.houseNo ? workOrder.address.houseNo : "") +
                                (!!workOrder.address.houseNoAddition
                                    ? " " + workOrder.address.houseNoAddition
                                    : "");

                            workOrder.address_postcode = workOrder.address.postcode;
                            workOrder.address_place = workOrder.address.place;
                        }
                        //if(workOrder.)
                    });
                    this.setState({ workOrders: findResponse, isLoading: false });
                });
        }
    }

    render() {
        const { t, classes, width, selected } = this.props;
        const { workOrders, columnsVisibility, rowsPerPage } = this.state;
        const isExtraSmallScreen = isWidthDown("xs", width);

        const columns = [
            {
                name: "resolverId",
                options: {
                    display: "excluded",
                    filter: false,
                    print: false,
                    download: false,
                },
            },
            {
                name: "repairRequestImages",
                label: t("Afbeelding"),
                options: {
                    display: this.isColumnVisible("repairRequestImages"),
                    filter: false,
                    sort: false,
                    print: false,
                    download: false,
                    customBodyRender: (v) =>
                    {
                        var imageId = v && v.length > 0 && v[0].attachmentId;
                        return (
                            <>
                                {
                                    !!imageId &&
                                    (
                                        <Tooltip title={
                                            <div className={classes.tooltipImageContainer}>
                                                <img src={`${webApiUrl}api/home/getattachment/${imageId}`} alt="" />
                                            </div>
                                        }
                                            arrow
                                            placement="right"
                                        >
                                            <Box
                                                boxShadow={1}
                                                borderRadius="borderRadius"
                                                className={classes.thumbnail}
                                                style={{
                                                    backgroundImage:
                                                        "url(" +
                                                        webApiUrl +
                                                        "api/home/getattachment/" +
                                                        imageId +
                                                        ")",
                                                }}
                                            >
                                            </Box>
                                        </Tooltip>
                                    )
                                }
                            </>
                        )
                    }
                },
            },
            {
                name: "workOrderNumber",
                label: t("Nummer"),
                options: {
                    display: this.isColumnVisible("workOrderNumber"),
                    filter: false,
                    sort: true,
                    customBodyRender: (value, tableMeta, updateValue) => {
                        var resolverId = tableMeta.rowData[0];
                        return (
                            <Button
                                style={{ padding: 0 }}
                                component={Link}
                                to={"/werk/" + selected.projectNo + "/werkbon/" + resolverId}
                                color="primary"
                            >
                                {value}
                            </Button>
                        );
                    },
                },
            },
            {
                name: "description",
                label: t("Omschrijving"),
                options: {
                    filter: false,
                    display: this.isColumnVisible("description"),
                    sort: true,
                    customBodyRender: (value, tableMeta, updateValue) => {
                        var resolverId = tableMeta.rowData[0];
                        var workOrder = workOrders.find((x) => x.resolverId === resolverId);
                        return (
                            <Tooltip
                                title={
                                    <>
                                        {nl2br(workOrder.workOrderText)}
                                        {workOrder.explanation && (
                                            <>
                                                <br />
                                                {nl2br(workOrder.explanation)}
                                            </>
                                        )}
                                    </>
                                }
                            >
                                <span>{value}&nbsp;</span>
                            </Tooltip>
                        );
                    },
                },
            },
            {
                name: "buildingNoIntern",
                label: t("Bouwnummer"),
                options: {
                    display: this.isColumnVisible("buildingNoIntern"),
                    filter: true,
                    filterType: "multiselect",
                    filterOptions: {
                        fullWidth: true,
                    },
                    sort: true,
                },
            },
            {
                name: "addressText",
                label: t("Objectadres"),
                options: {
                    display: this.isColumnVisible("addressText"),
                    filter: false,
                    sort: true,
                },
            },
            {
                name: "address_postcode",
                label: t("Postcode"),
                options: {
                    display: this.isColumnVisible("address_postcode"),
                    filter: false,
                    sort: true,
                },
            },
            {
                name: "address_place",
                label: t("Plaats"),
                options: {
                    display: this.isColumnVisible("address_place"),
                    filter: false,
                    sort: true,
                },
            },
            {
                name: "targetCompletionDate",
                label: t("Streefdatum afhandeling"),
                options: {
                    display: this.isColumnVisible("targetCompletionDate"),
                    filter: false,
                    sort: true,
                    customBodyRender: (v) => v && formatDate(new Date(v)),
                },
            },
            {
                name: "status",
                label: t("Status"),
                options: {
                    display: this.isColumnVisible("status"),
                    filter: true,
                    filterType: "multiselect",
                    filterOptions: {
                        fullWidth: true,
                    },
                    sort: true,
                    customBodyRender: (v) => t("resolver.status." + v),
                },
            },
            //{
            //    name: 'pointOfContact',
            //    label: t('Aanspreekpunt'),
            //    options: {
            //        display: this.isColumnVisible('pointOfContact'),
            //        filter: false,
            //        sort: true,
            //    }
            //},
            {
                name: "relationName",
                label: t("Relatie"),
                options: {
                    display: this.isColumnVisible("relationName"),
                    filter: false,
                    sort: true,
                },
            },
            {
                name: "name",
                label: t("Naam"),
                options: {
                    display: this.isColumnVisible("name"),
                    filter: false,
                    sort: true,
                },
            },
        ];

        let columnOrder = [];
        columnOrder =
            JSON.parse(localStorage.getItem("ResolverWorkOrders_ColumnOrder")) ==
                undefined
                ? columns.forEach((column, index) => {
                    columnOrder.push(index);
                })
                : JSON.parse(localStorage.getItem("ResolverWorkOrders_ColumnOrder"));

        const options = {
            draggableColumns: { enabled: true },
            filterType: "dropdown",
            responsive: "vertical",
            rowsPerPageOptions: [25, 50, 100],
            columnOrder,
            rowsPerPage: rowsPerPage,
            download: !isExtraSmallScreen,
            print: !isExtraSmallScreen,
            customToolbarSelect: (selectedRows) => (
                //<CustomToolbarSelect selectedRows={selectedRows} />
                <></>
            ),
            //selectableRows: 'none',
            //onRowClick: rowData => {
            //    var resolverId = rowData[0];
            //    if (resolverId) {
            //        history.push('/objecten/dashboard/' + rowData[0]);
            //    }
            //},
            onColumnOrderChange: (newColumnOrder, columnIndex, newPosition) => {
                localStorage.setItem(
                    "ResolverWorkOrders_ColumnOrder",
                    JSON.stringify(newColumnOrder)
                );
            },
            onViewColumnsChange: (changedColumn, action) => {
                if (action == "add") {
                    var listToUpdate = columnsVisibility;
                    var column = listToUpdate.find((x) => x.name === changedColumn);
                    if (column) {
                        column.visible = true;
                    } else {
                        listToUpdate.push({ name: changedColumn, visible: true });
                    }
                    this.setState({ columnsVisibility: listToUpdate });
                    localStorage.setItem(
                        "ResolverWorkOrders_ColumnsVisibility",
                        JSON.stringify(listToUpdate)
                    );
                } else if (action == "remove") {
                    var listToUpdate = columnsVisibility;
                    var column = listToUpdate.find((x) => x.name === changedColumn);
                    if (column) {
                        column.visible = false;
                    } else {
                        listToUpdate.push({ name: changedColumn, visible: false });
                    }
                    this.setState({ columnsVisibility: listToUpdate });
                    localStorage.setItem(
                        "ResolverWorkOrders_ColumnsVisibility",
                        JSON.stringify(listToUpdate)
                    );
                }
            },
            textLabels: getDataTableTextLabels(t, this.state.isLoading),
            onChangeRowsPerPage: (rowsPerPage) => {
                this.setRowsPerPage(rowsPerPage);
            },
        };

        return (
            <Container maxWidth={false} className={classes.mainContainer}>
                <MUIDataTable
                    className={classes.dataTable}
                    title={t("Werkbonnen")}
                    data={workOrders}
                    columns={columns}
                    options={options}
                />
            </Container>
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
export { connectedPage as ResolverGridPage };