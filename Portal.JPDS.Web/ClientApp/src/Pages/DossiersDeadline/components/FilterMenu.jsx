import React, { useState, useEffect } from 'react';
import {
  Button,
  FormControl,
  Grid,
  Grow,
  InputLabel,
  makeStyles,
  MenuItem,
  Paper,
  Popper,
  Select,
  Checkbox,
  Box,
  FormControlLabel
} from '@material-ui/core';
import DateFnsUtils from "@date-io/date-fns";
import nlLocale from "date-fns/locale/nl";
import { DatePicker, MuiPickersUtilsProvider } from "@material-ui/pickers";
import { useTranslation } from "react-i18next";

const FilterMenu = ({ open, onSelect, onClickAway, anchorEl, onClickListItem, mainFilterData, handleFilterData, onKeyDown, ...props }) => {
  const [filterParams, setFilterParams] = useState({
    startDate: null,
    endDate: null,
    status: 0,
    is48hoursReminder: false,
    isOverdue: false,
    hasUpdates: false
  });
  const classes = useStyles();
  const { t } = useTranslation();

  useEffect(() => {
    setFilterParams(mainFilterData)
  }, [mainFilterData])

  const handleApplyFilter = () => {
    handleFilterData(filterParams);
    const falseFilters = {}
    Object.keys(filterParams).filter(filter => !filterParams[filter])
      .map(filter => falseFilters[filter] = filterParams[filter])
    setFilterParams({ ...filterParams, ...falseFilters });
    onClickAway();
  }

  const handleClearFilter = () => {
    setFilterParams({
      startDate: null,
      endDate: null,
      status: 0,
      is48hoursReminder: false,
      isOverdue: false,
      hasUpdates: false
    })
    handleFilterData({
      startDate: null,
      endDate: null,
      status: 0,
      is48hoursReminder: false,
      isOverdue: false,
      hasUpdates: false
    });
    onClickAway();
  }

  return (
    <Popper style={{ zIndex: 1 }} open={open} anchorEl={anchorEl} role={undefined} transition disablePortal modifiers={{
      arrow: {
        enabled: true,
        element: anchorEl,
      },
    }}>
      {({ TransitionProps, placement }) => (
        <Grow
          {...TransitionProps}
          style={{ transformOrigin: placement === 'bottom' ? 'center top' : 'center bottom' }}
        >
          <Paper className={classes.paper}>
            <Grid container spacing={1} className={classes.mainContent} >
              <Grid item xs={12} container wrap="nowrap">
                <MuiPickersUtilsProvider utils={DateFnsUtils} locale={nlLocale}>
                  <DatePicker
                    variant="inline"
                    margin="dense"
                    id="startDate-time-picker"
                    label={t('dossierdeadline.filterMenu.input.startDate.label')}
                    format="dd-MM-yyyy"
                    helperText={""}
                    maxDate={filterParams.endDate ? filterParams.endDate : null}
                    value={filterParams.startDate}
                    onChange={(date) => setFilterParams({ ...filterParams, startDate: date.toJSON() })}
                    inputVariant="outlined"
                    autoOk
                    ampm={false}
                  />
                  <Box className={classes.datePickerDivider} mx={1}>
                    -
                  </Box>
                  <DatePicker
                    variant="inline"
                    margin="dense"
                    id="endDate-time-picker"
                    label={t('dossierdeadline.filterMenu.input.endDate.label')}
                    format="dd-MM-yyyy"
                    minDate={filterParams.startDate ? filterParams.startDate : null}
                    helperText={""}
                    value={filterParams.endDate}
                    onChange={(date) => setFilterParams({ ...filterParams, endDate: date.toJSON() })}
                    inputVariant="outlined"
                    autoOk
                    ampm={false}
                  />
                </MuiPickersUtilsProvider>
              </Grid>
              <Grid item xs={12}>
                <FormControl
                  variant="outlined"
                  margin="dense"
                  fullWidth
                >
                  <InputLabel id="status-label">{t('Status')}</InputLabel>
                  <Select
                    labelId="status-select"
                    id="status-select"
                    value={filterParams.status}
                    onChange={(e) => setFilterParams({ ...filterParams, status: e.target.value })}
                    label={t('Status')}
                  >
                    {
                      [0, 1, 2].map((status, index) => (
                        status === 0 ? <MenuItem value={status}>{t('dossier.status.all')}</MenuItem> :
                          <MenuItem key={index} value={status}>{t('dossier.status.' + status)}</MenuItem>
                      ))
                    }
                  </Select>
                </FormControl>
              </Grid>
              <Grid container alignItems='center' justify='space-between' item xs={12}>
                <FormControlLabel
                  value={filterParams.is48hoursReminder}
                  control={<Checkbox color="primary" />}
                  className={classes.noMarginBottom}
                  checked={filterParams.is48hoursReminder}
                  label={t("dossierdeadline.filterMenu.checkBox.is48hoursReminder.label")}
                  labelPlacement="end"
                  onChange={() => setFilterParams({ ...filterParams, is48hoursReminder: !filterParams.is48hoursReminder })}
                />
                <FormControlLabel
                  value={filterParams.isOverdue}
                  control={<Checkbox color="primary" />}
                  className={classes.noMarginBottom}
                  checked={filterParams.isOverdue}
                  label={t("dossierdeadline.filterMenu.checkBox.isOverdue.label")}
                  labelPlacement="end"
                  onChange={() => setFilterParams({ ...filterParams, isOverdue: !filterParams.isOverdue })}
                />
              </Grid>
              <Grid container item xs={12}>
                <FormControlLabel
                  value={filterParams.hasUpdates}
                  control={<Checkbox color="primary" />}
                  checked={filterParams.hasUpdates}
                  className={classes.noMarginBottom}
                  label={t("dossierdeadline.filterMenu.checkBox.hasUpdates.label")}
                  labelPlacement="end"
                  onChange={() => setFilterParams({ ...filterParams, hasUpdates: !filterParams.hasUpdates })}
                />
              </Grid>
              <Grid container item xs={12} className={classes.actions} justify="flex-end"
                alignItems="flex-end">
                <Button onClick={handleClearFilter} color="secondary">
                  {t('dossierdeadline.filterMenu.button.clear.title')}
                </Button>
                <Button onClick={handleApplyFilter} color="primary" disabled={(!!filterParams.startDate && !filterParams.endDate) || (!!filterParams.endDate && !filterParams.startDate)}>
                  {t('dossierdeadline.filterMenu.button.filter.title')}
                </Button>
              </Grid>
            </Grid>
          </Paper>
        </Grow>
      )}
    </Popper>
  )
}

export default FilterMenu;

const useStyles = makeStyles((theme) => ({
  paper: {
    padding: 10,
  },
  mainContent: {
    maxWidth: 300
  },
  datePickerDivider: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    fontWeight: 'bold'
  },
  noMarginBottom: {
    marginBottom: 0
  },
  actions: {
    // marginTop: 5,
  },
  menuListItem: {
    minWidth: '200px',
    display: 'flex',
    justifyContent: 'space-between'
  },

  icon: {
    fill: '#3f51b5'
  },
  collapseContainer: {
    padding: '10px',
  }
}));