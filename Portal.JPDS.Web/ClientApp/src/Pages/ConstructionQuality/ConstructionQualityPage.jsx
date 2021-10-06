import React from "react";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import { Container, Typography, Box, Button, withWidth, isWidthDown, Tooltip } from "@material-ui/core";
import { withStyles } from '@material-ui/core/styles';
import { Schedule, PriorityHigh, ArrowRightAlt } from "@material-ui/icons"
import { authHeader, formatDate, getDataTableTextLabels, nl2br } from "../../_helpers"
import { withTranslation } from 'react-i18next';
import MUIDataTable from "mui-datatables";
import { commonActions } from "../../_actions/common.actions";

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
    warning: {
        color: theme.palette.warning.main
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
        requests: [],
        columnsVisibility: [
            { name: "isRequiredHandling", visible: true },
            { name: "number", visible: true },
            { name: "date", visible: true },
            { name: "buildingNoIntern", visible: true },
            { name: "location", visible: true },
            { name: "desc", visible: true },
            { name: "textResolvers", visible: true },
            { name: "targetCompletionDate", visible: true },
            { name: "carryOutAsType", visible: true },
            { name: "status", visible: true },
        ],
        rowsPerPage: 50,
        isLoading: true,
    };

    componentDidMount() {
        const { user, selected } = this.props;
        let { columnsVisibility } = this.state;

        if (
            JSON.parse(
                localStorage.getItem("CQRepairRequests_ColumnsVisibility")
            ) == undefined
        ) {
            localStorage.setItem(
                "CQRepairRequests_ColumnsVisibility",
                JSON.stringify(columnsVisibility)
            );
        } else {
            columnsVisibility = JSON.parse(
                localStorage.getItem("CQRepairRequests_ColumnsVisibility")
            );
        }

        var rowsPerPage = parseInt(
            localStorage.getItem("CQRepairRequests_RowsPerPage")
        );
        if (
            !!rowsPerPage &&
            rowsPerPage > 0 &&
            rowsPerPage !== this.state.rowsPerPage
        ) {
            this.setRowsPerPage(rowsPerPage);
        }

        if (selected) {
            this.UpdateRepairRequests();
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
        if (!prevProps.selected || prevProps.selected.projectId !== selected.projectId) {
            this.UpdateRepairRequests();
        }
    }

    timer = () => {
        //this.UpdateBuildings();
    };

    setRowsPerPage = (rowsPerPage) => {
        this.setState({ rowsPerPage });

        localStorage.setItem("CQRepairRequests_RowsPerPage", rowsPerPage);
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

    UpdateRepairRequests() {
        const { t, selected, buildings } = this.props;
        if (selected) {
            const url =
                webApiUrl +
                "api/RepairRequest/GetRepairRequestsByProject/" +
                encodeURI(selected.projectId);
            const requestOptions = {
                method: "GET",
                headers: authHeader(),
            };

            fetch(url, requestOptions)
                .then((Response) => Response.json())
                .then((findResponse) => {
                    findResponse.forEach((request, index) => {
                        request.textResolvers = [];
                        request.isRequiredHandling = false;
                        if (
                            request.completed !== true &&
                            request.resolvers &&
                            request.resolvers.length > 0
                        ) {
                            request.textResolvers = request.resolvers.map((x) => x.name);
                            request.isRequiredHandling =
                                request.resolvers.filter((x) => x.isRequiredHandling).length >
                                0;
                            if (
                                request.isRequiredHandling === false &&
                                request.resolvers.filter(
                                    (x) => x.status === 3 || x.status === 4
                                ).length === request.resolvers.length
                            ) {
                                request.isRequiredHandling = true;
                            }
                        }
                        var building = buildings.find(
                            (x) => x.buildingId === request.buildingId
                        );
                        request.buildingNoIntern = building && building.buildingNoIntern;

                        if (request.address) {
                            request.addressText =
                                (!!request.address.street ? request.address.street : "") +
                                " " +
                                (!!request.address.houseNo ? request.address.houseNo : "") +
                                (!!request.address.houseNoAddition
                                    ? " " + request.address.houseNoAddition
                                    : "");

                            request.address_postcode = request.address.postcode;
                            request.address_place = request.address.place;
                        }
                        request.priorityText =
                            request.priority >= 0 && request.priority <= 2
                                ? t("repairrequest.priority." + request.priority)
                                : "";
                    });
                    this.setState({ requests: findResponse, isLoading: false });
                });
        }
    }

    render() {
        const { t, classes, width, selected, defaultFilter } = this.props;
        const { requests, columnsVisibility, rowsPerPage } = this.state;
        const isExtraSmallScreen = isWidthDown("xs", width);

        const columns = [
            {
                name: "requestId",
                options: {
                    display: "excluded",
                    filter: false,
                    print: false,
                    download: false,
                },
            },
            {
                name: "isRequiredHandling",
                label: " ",
                options: {
                    display: this.isColumnVisible("isRequiredHandling"),
                    filter: true,
                    print: false,
                    download: false,
                    customBodyRender: (value, tableMeta, updateValue) => {
                        var requestId = tableMeta.rowData[0];
                        var request = requests.find((x) => x.requestId === requestId);
                        return (
                            <Typography noWrap>
                                {value === true && (
                                    <Tooltip title={<Typography>{t("Actie vereist")}</Typography>}>
                                        <PriorityHigh color="secondary" size="small" />
                                    </Tooltip>
                                )}
                                {request.overdue ? (
                                    <Tooltip title={<Typography>{t("Te laat")}</Typography>}>
                                        <Schedule fontSize="small" color="error" />
                                    </Tooltip>
                                ) : request.is48HoursReminder ? (
                                    <Tooltip title={<Typography>{t("Binnen 48 uur")}</Typography>}>
                                        <Schedule fontSize="small" className={classes.warning} />
                                    </Tooltip>
                                ) : ("")}
                            </Typography>
                        );
                    },
                    filterType:'checkbox',
                    filterOptions: {
                        names: ['Aandacht'],
                        logic: (value, filters, row) => {
                            if (filters.length && filters.includes("Aandacht") && value) return false;
                            return true;
                        },
                    },
                },
            },

            {
                name: "attachments",
                label: t("Afbeelding"),
                options: {
                    display: this.isColumnVisible("attachments"),
                    filter: false,
                    sort: false,
                    print: false,
                    download: false,
                    customBodyRender: (v) => {
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
                name: "number",
                label: t("Nummer"),
                options: {
                    display: this.isColumnVisible("number"),
                    filter: false,
                    sort: true,
                    customBodyRender: (value, tableMeta, updateValue) => {
                        var requestId = tableMeta.rowData[0];
                        return (
                            <Button
                                style={{ padding: 0 }}
                                component={Link}
                                to={
                                    "/werk/" +
                                    selected.projectNo +
                                    "/kwaliteitsborging/" +
                                    requestId
                                }
                                color="primary"
                            >
                                {value}
                            </Button>
                        );
                    },
                },
            },
            {
                name: "date",
                display: this.isColumnVisible("date"),
                label: t("Datum"),
                options: {
                    filter: false,
                    sort: true,
                    customBodyRender: (v) => formatDate(new Date(v)),
                },
            },
            {
                name: "buildingNoIntern",
                label: t("Bouwnummer"),
                options: {
                    display: this.isColumnVisible("buildingNoIntern"),
                    filter: true,
                    filterType: "multiselect",
                    filterList: defaultFilter ? defaultFilter[5] : [],
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
                name: "location",
                label: t("Locatie"),
                options: {
                    display: this.isColumnVisible("location"),
                    filter: true,
                    filterList: defaultFilter ? defaultFilter[9] : [],
                    filterType: "multiselect",
                    sort: true,
                },
            },
            {
                name: "desc",
                label: t("Korte omschrijving"),
                options: {
                    display: this.isColumnVisible("desc"),
                    filter: false,
                    sort: true,
                    customBodyRender: (value, tableMeta, updateValue) => {
                        var requestId = tableMeta.rowData[0];
                        var request = requests.find((x) => x.requestId === requestId);
                        return (
                            <Tooltip title={nl2br(request.detailDesc)}>
                                <span>{value}</span>
                            </Tooltip>
                        );
                    },
                },
            },
            {
                name: "textResolvers",
                label: t("Oplossers"),
                options: {
                    display: this.isColumnVisible("textResolvers"),
                    filter: true,
                    filterList: defaultFilter ? defaultFilter[11] : [],
                    filterType: "multiselect",
                    sort: true,
                    customBodyRender: (v) =>
                        v &&
                        v.map((value, index) => (
                            <React.Fragment key={index}>
                                {index !== 0 && <br />}
                                {value}
                            </React.Fragment>
                        )),
                },
            },
            {
                name: "targetCompletionDate",
                label: t("Streefdatum"),
                options: {
                    display: this.isColumnVisible("targetCompletionDate"),
                    filter: false,
                    sort: true,
                    customBodyRender: (v) => v && formatDate(new Date(v)),
                },
            },
            {
                name: "carryOutAsType",
                label: t("Soort"),
                options: {
                    display: this.isColumnVisible("carryOutAsType"),
                    filter: true,
                    filterList: defaultFilter ? defaultFilter[13] : [],
                    filterType: "multiselect",
                    sort: true,
                },
            },
            {
                name: "status",
                label: t("Status"),
                options: {
                    display: this.isColumnVisible("status"),
                    filter: true,
                    filterList: defaultFilter ? defaultFilter[14] : [],
                    filterType: "multiselect",
                    sort: true,
                    customBodyRender: (value, tableMeta, updateValue) => {
                        var requestId = tableMeta.rowData[0];
                        var request = requests.find((x) => x.requestId === requestId);
                        return request.completed === true ? (
                            <span style={{ color: "#0f0" }}>{value}</span>
                        ) : request.overdue === true ? (
                            <span style={{ color: "#f00" }}>{value}</span>
                        ) : (value);
                    },
                },
            },
            {
                name: "priorityText",
                label: t("Prioriteit"),
                options: {
                    display: "excluded",
                    filter: true,
                    filterList: defaultFilter ? defaultFilter[15] : [],
                },
            },
            {
                name: "priority",
                label: t("Prio"),
                options: {
                    display: this.isColumnVisible("priority"),
                    filter: false,
                    sort: true,
                    customBodyRender: (v) =>
                        v == 2 ? (
                            <Tooltip title={t("repairrequest.priority." + v)}>
                                <ArrowRightAlt
                                    className={classes.rotate45AntiClockwise}
                                    color="error"
                                />
                            </Tooltip>
                        ) : v == 1 ? (
                            <Tooltip title={t("repairrequest.priority." + v)}>
                                <ArrowRightAlt color="primary" />
                            </Tooltip>
                        ) : v == 0 ? (
                            <Tooltip title={t("repairrequest.priority." + v)}>
                                <ArrowRightAlt className={classes.rotate45Clockwise} />
                            </Tooltip>
                        ) : (""),
                },
            },
            {
                name: "buyerRenterName",
                label: t("Koper/Huurder/Objectgebruiker"),
                options: {
                    display: this.isColumnVisible("buyerRenterName"),
                    filter: false,
                    sort: true,
                },
            },
        ];
        let columnOrder = [];
        columnOrder =
            JSON.parse(localStorage.getItem("CQRepairRequests_ColumnOrder")) ==
                undefined
                ? columns.forEach((column, index) => {
                    columnOrder.push(index);
                })
                : JSON.parse(localStorage.getItem("CQRepairRequests_ColumnOrder"));

        const options = {
            draggableColumns: { enabled: true },
            filterType: "dropdown",
            responsive: "vertical",
            rowsPerPageOptions: [25, 50, 100],
            rowsPerPage: rowsPerPage,
            download: !isExtraSmallScreen,
            print: !isExtraSmallScreen,
            columnOrder,
            onFilterChange: (_, list) => {
                this.props.dispatch(commonActions.meldingenTableFilter(list))
            },
            customToolbarSelect: (selectedRows) => (
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
            onColumnOrderChange: (newColumnOrder, columnIndex, newPosition) => {
                localStorage.setItem(
                    "CQRepairRequests_ColumnOrder",
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
                        "CQRepairRequests_ColumnsVisibility",
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
                        "CQRepairRequests_ColumnsVisibility",
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
                    title={t("Meldingen")}
                    data={requests}
                    columns={columns}
                    options={options}
                />
            </Container>
        );
    }
}

function mapStateToProps(state) {
    const { authentication, buildings, dashboardCount, meldingenFilter } = state;
    const { user } = authentication;
    const { selected, all } = buildings;
    const { messageCountPerBuilding } = dashboardCount;
    return {
        user,
        selected,
        buildings: all,
        messageCountPerBuilding,
        defaultFilter:meldingenFilter
    };
}

const connectedPage = connect(mapStateToProps)(withWidth()(withTranslation()(withStyles(styles)(Page))));
export { connectedPage as ConstructionQualityPage };
