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
import { DeleteOutline, ExpandLess, ExpandMore } from "@material-ui/icons";
import clsx from "clsx";


export default function DraftDossiers({ dossiers, selectedDossier, handleDeleteDraft, handleUpdateDraft, canEdit, ...props }) {
  const { t } = useTranslation();
  const classes = useStyles();
  const [expandedDrafts, setExpandedDrafts] = useState(false);
  return (
    <>
      <AppBar position="sticky">
        <Toolbar variant="dense">
          <Typography className={clsx(classes.grow, classes.bold)} noWrap>
            {t('Concept')}
          </Typography>
          <Tooltip title={expandedDrafts ? t('Inklappen') : t('Uitklappen')}>
            <IconButton aria-label="settings" color="inherit" edge="end"
              onClick={() => setExpandedDrafts(!expandedDrafts)}>
              {expandedDrafts ? <ExpandLess /> : <ExpandMore />}
            </IconButton>
          </Tooltip>
        </Toolbar>
      </AppBar>
      <Collapse in={expandedDrafts} timeout="auto" unmountOnExit>
        <List className={clsx(classes.dossierList, 'others')}>
          {dossiers && dossiers.length > 0 ?
            dossiers.map((dossier, index) => (
              <React.Fragment key={index}>
                <ListItem button className={classes.dossierListItem}
                  selected={selectedDossier && selectedDossier.id === dossier.id}
                  onClick={(e) => {
                    e.stopPropagation();
                    handleUpdateDraft(dossier)
                  }}>
                  <ListItemText
                    primary={dossier.name}
                    secondary={dossier.deadline && formatDate(new Date(dossier.deadline))}
                    secondaryTypographyProps={{ component: "div" }}
                  />
                  {canEdit && <IconButton aria-label="Delete" onClick={(e) => {
                    e.stopPropagation();
                    handleDeleteDraft(dossier)
                  }}>
                    <DeleteOutline />
                  </IconButton>}
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
