import React, { useState } from "react";
import {
  AppBar,
  Collapse,
  Divider,
  IconButton,
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
import { ExpandLess, ExpandMore, People } from "@material-ui/icons";
import clsx from "clsx";


export default function ArchivedDossiers({ dossiers, selectedDossier, handleSelectDossier }) {
  const { t } = useTranslation();
  const classes = useStyles();
  const [expandedArchived, setExpandedArchived] = useState(false);

  return (
    <>
      <AppBar position="sticky">
        <Toolbar variant="dense">
          <Typography className={clsx(classes.grow, classes.bold)} noWrap>
            {t('Archief')}
          </Typography>
          <Tooltip title={expandedArchived ? t('Inklappen') : t('Uitklappen')}>
            <IconButton aria-label="settings" color="inherit" edge="end"
              onClick={() => setExpandedArchived(!expandedArchived)}>
              {expandedArchived ? <ExpandLess /> : <ExpandMore />}
            </IconButton>
          </Tooltip>
        </Toolbar>
      </AppBar>
      <Collapse in={expandedArchived} timeout="auto" unmountOnExit>
        <List className={clsx(classes.dossierList, 'others')}>
          {dossiers && dossiers.length > 0 ?
            dossiers.map((dossier, index) => (
              <React.Fragment key={index}>
                <ListItem button className={classes.dossierListItem}
                  selected={selectedDossier && selectedDossier.id === dossier.id}
                  onClick={() => handleSelectDossier(dossier, 'archivedDossier')}>
                  <ListItemText
                    // primary={dossier.deadline ? formatDate(new Date(dossier.deadline)) : 'No Deadline'}
                    primary={dossier.name}
                    primaryTypographyProps={{ color: dossier.isOverdue === true ? 'error' : 'textPrimary' }}
                    secondary={dossier.deadline && formatDate(new Date(dossier.deadline))}
                    secondaryTypographyProps={{ component: "div" }}
                  />
                  &nbsp;
                  {dossier.type === 0 &&
                    <Tooltip title={t('Zichtbaar voor kopers')}><People fontSize="default"
                      color="primary" /></Tooltip>}
                  {/* {dossier.type === 1 &&
                    <Tooltip title={t('Onzichtbaar voor kopers')}><People fontSize="default"
                      color="action" /></Tooltip>}
                        &nbsp;
                        {dossier.status === 2 && <Tooltip title={t('Afgerond')}><Lock fontSize="default"
                    className={classes.colorSuccess} /></Tooltip>}
                  {dossier.status === 1 &&
                    <Tooltip title={t('Open')}><LockOpen fontSize="default" color="primary" /></Tooltip>} */}
                </ListItem>
                <Divider component="li" />
              </React.Fragment>
            )) : <ListItem><ListItemText secondary={t('Geen dossiers')} /></ListItem>}
        </List>
      </Collapse>
    </>
  );
}


const useStyles = makeStyles((theme) => ({
  grow: {
    flexGrow: 1
  },
  bold: {
    fontWeight: "bold"
  },
  dossierList: {
    flexGrow: 1,
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
  dossierListItem: {
    paddingTop: 0,
    paddingBottom: 0,
  },
}));
