import React from "react";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import {
    Container,
    Grid,
    Typography, ExpansionPanel, ExpansionPanelSummary, AppBar, TextField, Button, Toolbar, Table, TableBody, TableRow, TableCell, Avatar, ExpansionPanelDetails, IconButton, CircularProgress
} from "@material-ui/core";
import { withStyles, withTheme } from '@material-ui/core/styles';
import { Search, Person, ArrowDropDown, CloudDownload, AttachFile } from "@material-ui/icons"
import { validateFile, formatDate, authHeader, downloadFile } from "../../_helpers"
import { withTranslation } from 'react-i18next';
import clsx from "clsx";

const { webApiUrl } = window.appConfig;

const styles = theme => ({
    grow: {
        flexGrow: 1
    },
    bold: {
        fontWeight: "bold"
    },
    mainContainer: {
        [theme.breakpoints.down("xs")]: {
            padding: theme.spacing(0)
        }
    },
    container: {
        backgroundColor: theme.palette.background.paper,
        margin: theme.spacing(5, 0, 6),
        [theme.breakpoints.down("xs")]: {
            marginTop: theme.spacing(0)
        }
    },
    expansionPanel: {
        width: '100%',
        '& .MuiExpansionPanelSummary-root.Mui-expanded': {
            minHeight: 48,
        },
        '& .MuiExpansionPanelSummary-content.Mui-expanded': {
            margin: '12px 0'
        }
    },
    documentHeading: {
        color: theme.palette.primary.contrastText,
        backgroundColor: theme.palette.primary.light,
        '& .MuiIconButton-root': {
            color: "inherit"
        }
    },
    expansionPanelDetails: {
        padding: theme.spacing(1, 3)
    },
    button: {
        '&:hover': {
            color: theme.palette.primary.contrastText
        }
    }
});


class Page extends React.Component {
    state = {
        headers: [],
        uploading: false
    };

    componentDidMount() {
        this.UpdateDocuments();
    }

    componentDidUpdate(prevProps, prevState) {
        if (!prevProps.selected || prevProps.selected.buildingId !== this.props.selected.buildingId) {
            this.UpdateDocuments();
        }
    }

    UpdateDocuments() {
        const { selected } = this.props;
        if (selected) {
            const url = webApiUrl + 'api/home/GetBuildingDocuments/' + encodeURI(selected.buildingId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({ headers: findResponse });
                });
        }
    }

    uploadDocument = e => {
        const { selected, t } = this.props;
        const files = Array.from(e.target.files);

        if (validateFile(files[0]) === true) {
            this.setState({ uploading: true })

            const formData = new FormData()

            formData.append('file', files[0])

            const url = webApiUrl + `api/home/UploadDocument/` + encodeURI(selected.buildingId);
            fetch(url, {
                method: 'POST',
                headers: authHeader(),
                body: formData
            })
                .then(res => res.json())
                .then(res => {
                    this.setState({
                        uploading: false
                    })
                    this.UpdateDocuments();
                })
                .catch(e => {
                    this.setState({ uploading: false });
                    alert(t('general.api.error'));
                })
        }
    }

    render() {
        const { t, classes, selected, user } = this.props;
        const { headers, uploading } = this.state;
        return (
            <Container className={classes.mainContainer}>
                <Grid container>
                    <Grid item container xs={12} className={classes.container}>
                        <AppBar position="sticky">
                            <Toolbar variant="dense">
                                <Typography className={clsx(classes.grow, classes.bold)}>{t('Documenten')}</Typography>
                                <input accept="*" style={{ display: 'none' }} disabled={!selected || user.viewOnly === true} id="icon-button-file" type="file" onChange={this.uploadDocument} />
                                <label htmlFor="icon-button-file" style={{ margin: 0 }}>

                                    {
                                        uploading ?
                                            <CircularProgress color="inherit" size={24} />
                                            :
                                            <IconButton
                                                color="inherit"
                                                aria-label="upload"
                                                component="span"
                                                disabled={!selected}
                                            >
                                                <AttachFile />
                                            </IconButton>
                                    }
                                </label>
                            </Toolbar>
                        </AppBar>
                        <div style={{ width: '100%' }}>
                            {
                                headers.map((header, indexHeader) => (
                                    <ExpansionPanel key={indexHeader} className={classes.expansionPanel} defaultExpanded={true}>
                                        <ExpansionPanelSummary
                                            expandIcon={<ArrowDropDown />}
                                            aria-controls={'panel-cat-' + indexHeader + '-content'}
                                            id={'panel-cat-' + indexHeader + '-header'} className={classes.documentHeading}
                                        >
                                            <Typography className={classes.bold}>{header.header}</Typography>
                                        </ExpansionPanelSummary>
                                        <ExpansionPanelDetails className={classes.documentHeaderDetails}>
                                            <Grid container>
                                                {
                                                    header.attachments.map((document, index) => (
                                                        <Grid key={index} container item xs={12} alignItems="center" justify="flex-end">
                                                            <Typography className={classes.grow} noWrap>{document.description}</Typography>
                                                            <Grid item>
                                                                <Typography>{document.dateTime && formatDate(new Date(document.dateTime))}</Typography>
                                                            </Grid>
                                                            <Grid item>
                                                                <IconButton href={webApiUrl + 'api/home/GetAttachment/' + encodeURI(document.id)} download>
                                                                    <CloudDownload />
                                                                </IconButton>
                                                            </Grid>
                                                        </Grid>
                                                    ))
                                                }
                                            </Grid>
                                        </ExpansionPanelDetails>
                                    </ExpansionPanel>
                                ))
                            }
                        </div>
                    </Grid>
                </Grid>
            </Container>
        );
    }
}

function mapStateToProps(state) {
    const { authentication, buildings } = state;
    const { user } = authentication;
    const { selected } = buildings;
    return {
        user,
        selected
    };
}

const connectedPage = connect(mapStateToProps)(withTranslation()(withStyles(styles)(Page)));
export { connectedPage as DocumentsPage };