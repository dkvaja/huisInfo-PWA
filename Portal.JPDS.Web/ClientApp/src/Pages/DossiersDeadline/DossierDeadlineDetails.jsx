import {
  AppBar,
  Checkbox,
  Container,
  Grid,
  IconButton,
  makeStyles,
  Toolbar,
  Tooltip,
  Typography
} from '@material-ui/core';
import { ArrowBack, FilterList, History, PriorityHigh, Search, Share } from '@material-ui/icons';
import React, { useRef, useState } from 'react';
import clsx from "clsx";
import { useHistory, useLocation } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { connect } from 'react-redux';
import Paper from '@material-ui/core/Paper';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableRow from '@material-ui/core/TableRow';
import DossierSelectionDialog from './components/DossierSelectionDialog';
import FilterMenu from "./components/FilterMenu";
import { formatDate } from '../../_helpers';

const DossierDeadlineDetails = (props) => {
  const { buildings } = props;
  const { state } = useLocation();
  const history = useHistory();
  if (!state) {
    history.push('/');
    return null;
  }
  const dossier = state.dossier;
  const classes = useStyles();
  const filterRef = useRef();

  const { t } = useTranslation();
  const [isFilterOpen, setIsFilterOpen] = useState(false);
  const [isOpenDossierSelection, setIsOpenDossierSelection] = useState(false);
  const cols = 18;
  const rows = Math.ceil(buildings.length / cols);
  return (
    <Container className={classes.mainContainer} maxWidth={false}>
      <DossierSelectionDialog open={isOpenDossierSelection} onClose={() => setIsOpenDossierSelection(false)} />
      <Grid container className={classes.container}>
        <AppBar position="sticky">
          <Toolbar variant="dense">
            {
              <React.Fragment>
                <IconButton edge="start" aria-label="GoBack" color="inherit" onClick={props.history.goBack}>
                  <ArrowBack />
                </IconButton>
                <Typography className={clsx(classes.grow, classes.bold)} noWrap>
                  {dossier.name}
                </Typography>
                <Tooltip title={t('Delen')}>
                  <IconButton aria-label="Share" edge="end" color="inherit">
                    <Share />
                  </IconButton>
                </Tooltip>
                <Tooltip title={t('search')}>
                  <IconButton aria-label="Search" color="inherit">
                    <Search />
                  </IconButton>
                </Tooltip>
                <Tooltip title={t('filter')}>
                  <IconButton aria-label="filter" color="inherit" onClick={() => setIsFilterOpen(!isFilterOpen)}
                    ref={filterRef}>
                    <FilterList />
                  </IconButton>
                </Tooltip>
              </React.Fragment>
            }
          </Toolbar>
          <FilterMenu
            open={isFilterOpen}
            anchorEl={filterRef.current}
            onClickAway={() => setIsFilterOpen(false)}
          // onClickListItem={() => setIsFilterOpen(false)}
          />
        </AppBar>
        <Paper className={classes.root}>
          <TableContainer className={classes.container}>
            <Table stickyHeader aria-label="sticky table">
              <TableBody>
                {[...Array.from(Array(rows))].map((row, i) => {
                  return (
                    <TableRow className={classes.row} hover role="checkbox" tabIndex={-1} key={i}>
                      {buildings && buildings.slice(i * cols, (i * cols) + cols).map((column, ind) => {
                        const value = column.status;
                        const selectedBuilding = dossier.buildingInfoList.find(b => b.buildingId === column.buildingId);
                        return (
                          <TableCell className={`${classes.borderRight} ${classes.objectDetails}`}
                            key={column.buildingId}
                            align={column.align}>
                            {selectedBuilding ? <>
                              <div style={{ display: 'flex', flexDirection: 'row', alignItems: 'center' }}>
                                <Checkbox color='default' className={classes.checkBox} />
                                <History
                                  className={clsx(selectedBuilding.is48hoursReminder && classes.warning, selectedBuilding.isOverdue && classes.pending, classes.icon)} />
                                {!(ind % 4) &&
                                  <PriorityHigh color='secondary' className={classes.icon} />}
                              </div>
                              <Typography style={{ padding: 5, fontSize: 12 }}>
                                {formatDate(new Date(selectedBuilding.deadline))}
                              </Typography>
                              <span style={{ padding: 5, fontSize: 12 }}>
                                {selectedBuilding.status === 0 ? t('dossier.general.status.value.' + 2) : t('dossier.general.status.value.' + 1)}
                              </span>
                            </> : null}
                            <div className={classes.buildingNo}>{t(column.buildingNoExtern)}</div>
                          </TableCell>
                        );
                      })}
                    </TableRow>
                  );
                })}
              </TableBody>
            </Table>
          </TableContainer>
          {/* <TablePagination
                        rowsPerPageOptions={[10, 25, 100]}
                        component="div"
                        count={rows.length}
                        rowsPerPage={rowsPerPage}
                        page={page}
                        onChangePage={handleChangePage}
                        onChangeRowsPerPage={handleChangeRowsPerPage}
                    /> */}
        </Paper>
      </Grid>
    </Container>
  )
};


const useStyles = makeStyles((theme) => ({
  root: {
    width: '100%',
    height: '92%',
    // padding: 20,
    paddingTop: 10
  },
  head: {
    backgroundColor: theme.palette.background.paper,
    boxShadow: theme.shadows[0],
    textAlign: 'center',
    padding: 0
    // left: 225
  },
  mainContainer: {
    height: '100%',
    width: '100%',
    overflow: 'auto',
    padding: 0
  },
  container: {
    backgroundColor: theme.palette.background.paper,
    [theme.breakpoints.down("xs")]: {
      marginTop: theme.spacing(0)
    },
    height: '100%'
  },
  grow: {
    flexGrow: 1
  },
  bold: {
    fontWeight: "bold"
  },
  innerContainer: {
    position: 'relative',
    height: 'calc(100% - 48px)',
    overflow: 'hidden',
    zIndex: 1
  },
  row: {
    height: 120,
  },
  dossierHeader: {
    position: 'sticky',
    left: 0,
    zIndex: 1,
    background: '#fff',
    boxShadow: '3px -3px 6px #ccc',
    padding: 5,
    borderRight: '1px solid #ccc',
    minWidth: 220,
  },
  dossierName: {
    whiteSpace: 'nowrap',
    overflow: 'hidden',
    textOverflow: 'ellipsis',
    maxWidth: '200px'
  },
  icon: {
    fontSize: '1.4em'
  },
  checkBox: {
    '& svg': {
      fontSize: '1.4rem',
    },
    padding: 5,
    paddingLeft: 0
    // position: 'absolute',
    // left: -7,
    // top: -8
  },
  colorSuccess: {
    color: theme.palette.success.main
  },
  dossierHeaderTitles: {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center'
  },
  borderRight: {
    borderRight: '1px solid #ccc'
  },
  objectDetails: {
    position: 'relative',
    minWidth: 100,
    padding: 0
    // textAlign: 'center'
  },
  buildingNo: {
    position: 'absolute',
    top: 0,
    right: 0,
    fontSize: 10,
    background: '#eeeeee80',
    padding: 3,
    borderRadius: 10,
    margin: 2,
  },
  warning: {
    fill: '#ffc107'
  },
  pending: {
    fill: '#dc3545'
  }
}));

function mapStateToProps(state) {
  const { authentication, buildings } = state;
  const { user } = authentication;
  const { selected, all } = buildings;
  return {
    user,
    selected,
    buildings: all
  };
}

const connectedPage = connect(mapStateToProps)(DossierDeadlineDetails);
export { connectedPage as DossierDeadlineDetails };
