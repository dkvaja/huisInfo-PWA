import React, { useState } from 'react';
import {
  Button,
  Checkbox,
  Dialog,
  DialogActions,
  DialogContent,
  FormControlLabel,
  Grid,
  IconButton,
  makeStyles
} from '@material-ui/core';
import { Close } from '@material-ui/icons';

const DossierSelectionDialog = ({open, onClose, onSelect}) => {
  const classes = useStyles();
  const [isOpenDossierSelected, setIsOpenDossierSelected] = useState(false);
  const [isClosedDossierSelected, setIsClosedDossierSelected] = useState(false);

  return (
      <Dialog open={open} onClose={onClose}>
        <DialogContent className={classes.container}>
          <IconButton className={classes.closeIcon} component="span" size="small" onClick={onClose}>
            <Close/>
          </IconButton>
          <Grid container spacing={1}>
            <Grid item xs={12} className={classes.mainContent}>
              <FormControlLabel
                  control={<Checkbox color="primary" checked={isOpenDossierSelected && isClosedDossierSelected}
                                     indeterminate={(isOpenDossierSelected || isClosedDossierSelected) && !(isOpenDossierSelected && isClosedDossierSelected)}
                                     onChange={() => setIsOpenDossierSelected(!isOpenDossierSelected || !isClosedDossierSelected) || setIsClosedDossierSelected(!isOpenDossierSelected || !isClosedDossierSelected)}/>}
                  label="Select All"
                  labelPlacement="end"
              />
            </Grid>
            <Grid item xs={12} className={classes.mainContent}>
              <FormControlLabel
                  control={<Checkbox color="primary" checked={isOpenDossierSelected}
                                     onChange={() => setIsOpenDossierSelected(!isOpenDossierSelected)}/>}
                  label="Open Dossiers"
                  labelPlacement="end"
              />
            </Grid>
            <Grid item xs={12} className={classes.mainContent}>
              <FormControlLabel
                  control={<Checkbox color="primary" checked={isClosedDossierSelected}
                                     onChange={() => setIsClosedDossierSelected(!isClosedDossierSelected)}/>}
                  label="Close Dossiers"
                  labelPlacement="end"
              />
            </Grid>
          </Grid>
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose} color="secondary">
            Cancel
          </Button>
          <Button onClick={onSelect} color="primary">
            Ok
          </Button>
        </DialogActions>
      </Dialog>
  );
}

export default DossierSelectionDialog;

const useStyles = makeStyles((theme) => ({
  title: {
    textAlign: 'center',
    color: theme.palette.primary.main,
  },
  root: {
    width: '240px'
  },
  mainContent: {
    '& > label': {
      marginBottom: 0
    }
  },
  icons: {
    width: '3em',
    height: '3em',
    fill: theme.palette.primary.main
  },
  container: {
    position: 'relative',
    maxWidth: 300
  },
  closeIcon: {
    position: 'absolute',
    right: 10,
    top: 10
  }
}));
