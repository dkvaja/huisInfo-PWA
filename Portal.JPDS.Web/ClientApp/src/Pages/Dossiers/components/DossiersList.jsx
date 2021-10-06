import React, { useEffect, useState } from "react";
import {
  AppBar,
  CircularProgress,
  Divider,
  List,
  ListItem,
  ListItemText,
  makeStyles,
  Toolbar,
  Tooltip,
  Typography
} from "@material-ui/core";
import { useTranslation } from "react-i18next";
import { formatDate } from "../../../_helpers";
import { CheckCircle, PriorityHigh, VisibilityOff } from "@material-ui/icons";
import clsx from "clsx";
import { useSelector } from "react-redux";
import { SortableContainer, SortableElement } from "react-sortable-hoc";
import { orderDossiers } from "../../../apis/dossiersApi";


export default function DossiersList({ dossiers, viewAs, selectedDossier, handleSelectDossier, handleSwitcher, isFullHeight, isBuyer }) {
  const { t } = useTranslation();
  const classes = useStyles();
  const [allDossiers, setAllDossiers] = useState([]);
  const { loading } = useSelector(state => state.dossier);
  const { rights } = useSelector(state => state.buildings);
  const canDragDossierList = rights['dossier.canDrag'];

  useEffect(() => {
    setAllDossiers(dossiers || []);
  }, [dossiers]);

  const arrayMove = (arr, old_index, new_index) => {
    const element = arr[old_index];
    arr.splice(old_index, 1);
    arr.splice(new_index, 0, element);
    return arr;
  };

  const onSortEnd = ({ oldIndex, newIndex }) => {
    const updatedOrderDossiers = arrayMove(Object.assign([], allDossiers), oldIndex, newIndex);
    const previousDossierId = updatedOrderDossiers[newIndex - 1] ? updatedOrderDossiers[newIndex - 1].id : "";
    const dossierId = allDossiers[oldIndex].id;
    setAllDossiers(updatedOrderDossiers);
    if (oldIndex !== newIndex) {
      orderDossiers({ previousDossierId, dossierId }).then(res => {
        console.log(res)
      }).catch(er => {
        console.log(er)
      })
    }
  };


  const SortableItem = SortableElement(({ dossier, ...rest }) => (
    <div className={classes.sortableItem}>
      <ListItem button className={classes.dossierListItem}
        selected={selectedDossier && selectedDossier.id === dossier.id}
        onClick={() => handleSelectDossier(dossier, 'dossier main')}>
        <ListItemText
          primary={
            <>
              {dossier.name} &nbsp;
              {
                dossier.status === 2 &&
                <Tooltip title={t('Afgerond')}>
                  <CheckCircle fontSize="default" className={classes.colorSuccess} />
                </Tooltip>
              }
              {
                dossier.hasUpdates &&
                <Tooltip title={t('Er is een aanpassing gedaan in het dossier.')}>
                  <PriorityHigh color="error" />
                </Tooltip>
              }
            </>
          }
          primaryTypographyProps={{ component: "div", style: { wordWrap: 'break-word' } }}
          secondary={dossier.deadline && formatDate(new Date(dossier.deadline))}
          secondaryTypographyProps={{
            color: dossier.isOverdue ? 'error' : 'textSecondary'
          }}
        />
        &nbsp;
        {
          isBuyer !== true &&
          (
            dossier.isExternal !== true &&
            <Tooltip title={t('Onzichtbaar voor kopers')}><VisibilityOff fontSize="default" color="action" /></Tooltip>
          )
        }
      </ListItem>
      <Divider component="li" />
    </div>
  ));

  const SortableList = SortableContainer(({ dossiersData }) => {
    return (
      <div>
        {
          dossiersData.map((dossier, index) => (
            <SortableItem disabled={viewAs === 'building' || !canDragDossierList} key={`${dossier.id}`} index={index} dossier={dossier} />
          ))
        }
      </div>
    );
  });

  return (
    <>
      <AppBar position="sticky">
        <Toolbar className={classes.toolBar} variant="dense">
          <Typography className={clsx(classes.grow, classes.bold)} noWrap>
            {t('Dossiers')}
          </Typography>
          {/*<IconButton aria-label="Sort" color="inherit" onClick={() => {*/}
          {/*}}>*/}
          {/*  <Icon style={{fontSize:17}} className="fas fa-sort-amount-down-alt" />*/}
          {/*</IconButton>*/}
          {/*<IconButton aria-label="Filter" color="inherit" edge="end" onClick={() => {*/}
          {/*}}>*/}
          {/*  <Icon style={{fontSize:17}} className="fas fa-filter" />*/}
          {/*</IconButton>*/}

        </Toolbar>

      </AppBar>
      <List className={clsx(classes.dossierList, isFullHeight && classes.isFullHeight)}>
        {loading ?
          <CircularProgress className={classes.mAuto} />
          :
          allDossiers && allDossiers.length > 0 ?
            <SortableList transitionDuration={400} dossiersData={allDossiers} axis="y" distance={10} onSortEnd={onSortEnd} />
            :
            <ListItem>
              <ListItemText secondary={t('Geen dossiers')} />
            </ListItem>
        }
      </List>
    </>
  );
}


const useStyles = makeStyles((theme) => ({
  mAuto: {
    margin: 'auto'
  },
  toolBar: {
    paddingRight: theme.spacing(3.125)
  },
  sortableItem: {
    background: theme.palette.common.white
  },
  dossierList: {
    flexGrow: 1,
    display: 'flex',
    flexDirection: 'column',
    maxHeight: 'calc(100% - 144px)',
    overflowX: 'hidden',
    overflowY: 'auto',
    width: '100%',
    '&.full': {
      maxHeight: 'calc(100% - 48px)',
    },
    '&.others': {
      height: 'auto',
      maxHeight: 'unset'
    }
  },
  isFullHeight: {
    maxHeight: 'calc(100% - 48px)',
  },
  colorSuccess: {
    color: theme.palette.success.main
  },
  dossierListItem: {
    paddingTop: 0,
    paddingBottom: 0,
  }, grow: {
    flexGrow: 1
  },
  bold: {
    fontWeight: "bold"
  },
  switch: {
    position: 'absolute',
    right: -10
  }
}));
