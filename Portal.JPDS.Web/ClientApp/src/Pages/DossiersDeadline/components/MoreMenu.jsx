import React from 'react';
import { ClickAwayListener, Grow, makeStyles, MenuItem, MenuList, Paper, Popper } from '@material-ui/core';
import { SystemUpdateAlt } from '@material-ui/icons';
import { useTranslation } from "react-i18next";

const MoreMenu = ({open, onClickAway, anchorEl, onClickListItem, disabled, ...props}) => {
  const classes = useStyles();
  const {t} = useTranslation();

  return (
      <React.Fragment>
        <Popper style={{zIndex: 1}} open={open} anchorEl={anchorEl} role={undefined} transition disablePortal>
          {({TransitionProps, placement}) => (
              <Grow
                  {...TransitionProps}
                  style={{transformOrigin: placement === 'bottom' ? 'center top' : 'center bottom'}}
              >
                <Paper>
                  <ClickAwayListener onClickAway={onClickAway}>
                    <MenuList autoFocusItem={open} id="menu-list-grow">
                      <MenuItem disabled={disabled} className={classes.menuListItem} onClick={(e)=>e.stopPropagation()}>
                        {t('download')}
                        <SystemUpdateAlt style={{fontSize: '1.2em', color: 'grey'}}/>
                      </MenuItem>
                    </MenuList>
                  </ClickAwayListener>
                </Paper>
              </Grow>
          )}
        </Popper>
      </React.Fragment>
  )
}

export default MoreMenu;

const useStyles = makeStyles((theme) => ({
  menuListItem: {
    minWidth: '200px',
    display: 'flex',
    justifyContent: 'space-between'
  },
  subMenuListItem: {
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
