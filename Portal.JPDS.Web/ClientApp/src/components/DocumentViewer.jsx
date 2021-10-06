import React from "react";
import { IconButton, makeStyles, Dialog, DialogTitle, DialogContent, Typography, Grid, CircularProgress, Container, useMediaQuery } from "@material-ui/core";
import { Close, SkipPrevious, SkipNext } from "@material-ui/icons";
import { useTranslation } from "react-i18next";
import DocViewer, { DocViewerRenderers } from "react-doc-viewer";

export default function DocumentViewer(props) {
    const { open, onClose, documents, loader, ...rest } = props;
    const { t } = useTranslation();
    const classes = useStyles();
    const isSmallScreen = useMediaQuery(theme => theme.breakpoints.down('sm'));

    const handleCloseObjects = () => {
        onClose();
    }

    const myHeader = (state, previousDocument, nextDocument) => {
        if (!state.currentDocument || state.config.header.disableFileName) {
            return null;
        }
        return (
            <Container className={classes.headerContainer}>
                <Typography variant={'h6'} noWrap>{state.currentDocument.name || ""}</Typography>
                <div className={classes.iconContainer}>
                    <IconButton onClick={previousDocument} disabled={state.currentFileNo === 0}  >
                        <SkipPrevious />
                    </IconButton>
                    <IconButton onClick={nextDocument} disabled={state.currentFileNo >= state.documents.length - 1} >
                        <SkipNext />
                    </IconButton>
                </div>
            </Container>
        );
    };

    return (
        <Dialog fullScreen={isSmallScreen} fullWidth={true} maxWidth='lg' open={open} onClose={handleCloseObjects} aria-labelledby="form-dialog-title" scroll="paper">
            <DialogTitle className={classes.dialogTitle} id="dialog-objects-title" disableTypography>
                <Grid className={classes.dialogTitleContent} container spacing={1}>
                    <Grid item className={classes.grow}>
                        {/*<Typography variant="h5" className={classes.dialogTitleTypo}>{t('Selecteer betrokkene')}</Typography>*/}
                    </Grid>
                    <Grid>
                        <IconButton className={classes.closeButton} color="inherit" aria-label="close" onClick={handleCloseObjects}>
                            <Close className={classes.iconStyle} />
                        </IconButton>
                    </Grid>
                </Grid>
            </DialogTitle>
            <DialogContent className={classes.dialogContent}>
                {
                    loader && <CircularProgress className={classes.mAuto} />
                }
                {
                    !loader && open && <Grid container className={classes.dialogContainer} spacing={1} justify="center" align={"center"}>
                        {/* <Grid item xs={12}>
                            <Typography>
                                { }
                            </Typography>
                        </Grid> */}
                        <DocViewer
                            config={{
                                header: {
                                    overrideComponent: myHeader,
                                }
                            }}
                            documents={documents} pluginRenderers={DocViewerRenderers} style={{ width: '100%' }} />
                    </Grid>
                }
            </DialogContent>
        </Dialog>
    );
}


const useStyles = makeStyles((theme) => ({
    grow: {
        flexGrow: 1,
        display: 'flex',
        alignItems: 'center'
    },
    border: {
        borderTop: `1px solid ${theme.palette.grey[400]}`,
        marginTop: 20,
    },
    headerContainer: {
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center'
    },
    iconContainer: {
        display: 'flex',
        flexWrap: 'no-wrap'
    },
    dialogContent: {
        padding: 10,
        minHeight: '80vh',
        display: 'flex'
    },
    mAuto: {
        margin: 'auto'
    },
    dialogContainer: {
    },
    dialogTitle: {
        padding: 5,
        background: '#3f51b5',
    },
    dialogTitleTypo: {
        color: 'white',
        [theme.breakpoints.down('sm')]: {
            fontSize: '5vw',
        },
    },
    dialogTitleContent: {
        padding: 10
    },
    iconStyle: {
        fill: 'white',
        width: '1em',
        height: '1em',
    },
    closeButton: {
        margin: 0,
    },
    userNameTypo: {
        paddingLeft: 10,
        margin: 5,
        [theme.breakpoints.down('sm')]: {
            fontSize: '3vw',
        },
    },
    tableTitles: {
        display: 'flex',
        // alignItems: 'center',
    },
    gridTitle: {
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        flexDirection: 'column'
    },
    tableTitlesTypo: {
        // color: 'white',
        alignSelf: 'center',
        paddingLeft: 5,
        [theme.breakpoints.down('sm')]: {
            fontSize: '4vw',
        },
    },
    borderBottom: {
        borderBottom: `1px solid ${theme.palette.grey[400]}`,
        background: theme.palette.grey[200]
    },
    subtitles: {
        fontSize: '0.696rem',
        marginTop: 10,
        textAlign: 'center',
        marginBottom: -2,
    },
    subtile2: {
        fontSize: '0.686rem',
        textAlign: 'center'
        // color: '#ffffff'
    },
    userContainer: {
        marginTop: 13,
    }
}));
