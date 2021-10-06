import { AppBar, Container, Grid, IconButton, isWidthUp, makeStyles, Toolbar, Tooltip, Typography, withWidth } from '@material-ui/core';
import { Archive, ArrowBack, Share } from '@material-ui/icons';
import React from 'react';
import { useTranslation } from 'react-i18next';
import AddDossier from './AddDossier';
import clsx from "clsx";

const DossierObjectDetails = ({ width,history, ...props }) => {
    const classes = useStyles();
    const { t } = useTranslation();
    const matchesWidthUpLg = isWidthUp('lg', width);
    const matchesWidthUpSm = isWidthUp('sm', width);

    return (
        <Container className={classes.mainContainer} maxWidth={false}>
            <Grid container className={classes.container}>
                <AppBar position="sticky">
                    <Toolbar variant="dense">
                        {
                            <React.Fragment>
                                <IconButton edge="start" aria-label="GoBack" color="inherit"
                                    onClick={() => history.goBack(matchesWidthUpLg, matchesWidthUpSm)}>
                                    <ArrowBack />
                                </IconButton>
                                {/* {
                                    <AddDossier 
                                        selectedBuilding={selected}
                                        isUpdateDossier={this.state.isUpdateDossier}
                                        onCancelUpdating={() => this.setState({ isUpdateDossier: false, selectedUpdatingDossier: null })}
                                        selectedDossier={selectedUpdatingDossier}
                                        buildings={buildings.filter(x => x.projectId === selected.projectId)} />
                                } */}
                                <Typography className={clsx(classes.grow, classes.bold)}
                                    noWrap></Typography>
                                <Tooltip title={t('Archiveren')}>
                                    <IconButton aria-label="Archive" color="inherit"
                                    //  onClick={() => this.setState({ archive: true })}
                                     >
                                        <Archive />
                                    </IconButton>
                                </Tooltip>
                                <Tooltip title={t('Delen')}>
                                    <IconButton aria-label="Share" edge="end" color="inherit"
                                        // onClick={() => this.setState({ openShareDossierMenu: true })}
                                        >
                                        <Share />
                                    </IconButton>
                                </Tooltip>

                            </React.Fragment>
                        }
                    </Toolbar>
                </AppBar>
                
            </Grid>
        </Container>
    )
};


const useStyles = makeStyles((theme) => ({
    mainContainer: {
        height: '100%',
        width: '100%',
        overflow: 'auto',
        padding: 0
    },
    grow: {
        flexGrow: 1
    },
}));

const Page = withWidth()(DossierObjectDetails);
export { Page as DossierObjectDetails }
