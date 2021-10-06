import React from "react";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import { Container, Grid, Typography, AppBar, Toolbar, IconButton, TableHead, Popper, Box } from "@material-ui/core";
import { withStyles } from '@material-ui/core/styles';
import { userAccountTypeConstants } from "../../_constants"
import clsx from "clsx";
import { withTranslation } from 'react-i18next';
import MUIDataTable from "mui-datatables";
import { history, getDataTableTextLabels, formatDate, authHeader } from "../../_helpers";
import { Add } from "@material-ui/icons";

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
        paddingTop: theme.spacing(5),
        [theme.breakpoints.down("sm")]: {
            padding: theme.spacing(0)
        }
    },
    container: {
        //backgroundColor: theme.palette.background.paper,
        height: '100%',
        position: 'relative',
        overflow: 'auto'
    },
    gridWrapper: {
        maxHeight: 'calc(100% - 48px)',
        overflow: 'auto',
        width: '100%'
    },
    grid: {
        boxShadow: 'none',
        borderRadius: 0,
        '& .MuiTableCell-root.MuiTableCell-head': {
            fontWeight: 'bold'
        }
    },
    welcomePanel: {
        color: theme.palette.common.white,
        height: '40vh',
        position: 'relative',
        padding: theme.spacing(5, 0),
        '& h1': {
            textShadow: '0 0 10px rgb(0,0,0)'
        }
    },
    thumbnail: {
        width: 50,
        height: 50,
        backgroundRepeat: 'no-repeat',
        backgroundPosition: 'center',
        backgroundSize: 'contain',
        backgroundColor: 'rgba(0,0,0,0.1)',
        margin: theme.spacing(-1.5, 0)
    }
});


class Page extends React.Component {
    state = {
        requests: [],
        searchTerm: '',
        popperImageId: null,
        popperAnchorId: null,
        selectedStatus: 'all'
    };

    componentDidMount() {
        this.UpdateRepairRequests();
    }

    componentDidUpdate(prevProps, prevState) {
        if (!prevProps.selected || !this.props.selected || prevProps.selected.buildingId.toUpperCase() !== this.props.selected.buildingId.toUpperCase()) {
            this.UpdateRepairRequests();
        }
    }

    UpdateRepairRequests() {
        const { selected } = this.props;
        if (selected) {
            const url = webApiUrl + 'api/RepairRequest/GetRepairRequests/' + encodeURI(selected.buildingId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            this.setState({
                requests: [],
                searchTerm: '',
                selectedStatus: 'all'
            });

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({ requests: findResponse });
                });
        }
    }

    openDetails = request => {
        // console.log(request);
    }

    render() {
        const { user, t, classes, selected } = this.props;
        const { requests } = this.state;

        const isBuyer = user.type === userAccountTypeConstants.buyer;
        const columns = [
            {
                name: 'requestId',
                options: {
                    display: 'excluded',
                    filter: false,
                    print: false,
                    download: false
                }
            },
            {
                name: 'attachments',
                label: t('Afbeelding'),
                options: {
                    filter: false,
                    sort: false,
                    print: false,
                    download: false,
                    customBodyRender: v => (
                        <Box boxShadow={1} borderRadius="borderRadius"
                            onMouseEnter={(e) => { this.setState({ popperAnchorId: e.currentTarget, popperImageId: (v.length > 0 ? v[0] : null) }); }}
                            onMouseLeave={() => { this.setState({ popperImageId: null }); }}
                            className={classes.thumbnail}
                            style={{
                                backgroundImage: (
                                    v.length > 0 ?
                                        'url(/api/home/getattachment/' + v[0].attachmentId + ')'
                                        :
                                        'url(/Content/Images/Logos/Apps/reparation-tools.png)'
                                )
                            }}></Box>
                    )
                }
            },
            {
                name: 'number',
                label: t('Nummer'),
                options: {
                    filter: false,
                    sort: true,
                }
            },
            {
                name: 'location',
                label: t('Locatie'),
                options: {
                    filter: true,
                    sort: true,
                }
            },
            {
                name: 'desc',
                label: t('Korte omschrijving'),
                options: {
                    filter: false,
                    sort: true,
                }
            },
            {
                name: 'date',
                label: t('Datum'),
                options: {
                    filter: true,
                    sort: true,
                    customBodyRender: v => formatDate(new Date(v))
                }
            },
            {
                name: 'status',
                label: t('Status'),
                options: {
                    filter: true,
                    sort: true,
                }
            },
        ];
        const options = {
            filterType: 'dropdown',
            responsive: 'scrollFullHeight',
            selectableRows: 'none',
            onRowClick: rowData => {
                var requestId = rowData[0];
                if (requestId) {
                    history.push('/nazorg/details/' + rowData[0]);
                }
            },
            textLabels: getDataTableTextLabels(t)
        };

        return (
            <Container className={classes.mainContainer}>
                {
                    //<Grid container direction="row" justify="center" alignItems="center" className={classes.welcomePanel}>
                    //    <Grid item xs>
                    //        <Typography component="h1" variant="h4" gutterBottom align="center">{user.name}, {isBuyer ? t('dashboard.welcome.text.buyer') : t('dashboard.welcome.text')}</Typography>
                    //    </Grid>
                    //</Grid>
                }
                <Grid container className={classes.container} direction="column">
                    <AppBar position="static">
                        <Toolbar variant="dense">
                            <Typography className={clsx(classes.grow, classes.bold)}>{selected && selected.address}</Typography>
                            <IconButton
                                color="inherit"
                                edge="end"
                                aria-label="new Action"
                                component="span"
                                onClick={() => history.push('/nazorg/nieuwe')}
                            >
                                <Add />
                            </IconButton>
                        </Toolbar>
                    </AppBar>
                    <Grid item xs={12} className={classes.gridWrapper}>
                        <MUIDataTable
                            className={classes.grid}
                            title={t('Meldingen')}
                            data={requests}
                            columns={columns}
                            options={options}
                        />
                    </Grid>
                </Grid>
                <Popper
                    id="mouse-over-popper"
                    open={this.state.popperImageId !== null}
                    anchorEl={this.state.popperAnchorId}
                    placement="right" disablePortal={false}
                    modifiers={{
                        flip: {
                            enabled: true,
                        },
                        preventOverflow: {
                            enabled: true,
                            boundariesElement: 'scrollParent',
                        },
                        arrow: {
                            enabled: false,
                            element: this.state.popperAnchorId,
                        },
                    }}
                >
                    <Box boxShadow={1} borderRadius="borderRadius" component="img" src={'/api/home/getattachment/' + this.state.popperImageId}
                        alt="" style={{
                            maxWidth: '80%',
                            maxHeight: '80vh',
                            backgroundColor: '#fff'
                        }}>
                    </Box>
                </Popper>
            </Container >
        );
    }
}

function mapStateToProps(state) {
    const { authentication, buildings, dashboardCount } = state;
    const { user } = authentication;
    const { selected, all } = buildings;
    const { quotationsCount } = dashboardCount;
    return {
        user,
        selected,
        buildings: all,
        quotationsCount
    };
}

const connectedPage = connect(mapStateToProps)(withTranslation()(withStyles(styles)(Page)));
export { connectedPage as RepairRequestsHomePage };
