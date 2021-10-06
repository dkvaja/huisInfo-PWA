import React from "react";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import {
    Container,
    Grid,
    Typography, ExpansionPanel, ExpansionPanelSummary, AppBar, TextField, Button, Toolbar, Table, TableBody, TableRow, TableCell, Avatar, CircularProgress, IconButton, Tooltip
} from "@material-ui/core";
import { withStyles, withTheme } from '@material-ui/core/styles';
import { Search, Person, AttachFile, ArrowBack, ChevronLeft, ChevronRight, Delete } from "@material-ui/icons"
import { withTranslation } from 'react-i18next';
import { SortableContainer, SortableElement } from 'react-sortable-hoc';
import clsx from "clsx";
import StandardOptionsGrid from "./StandardOptionsGrid";
import Dropzone from "../../components/Dropzone";
import { validateFile, history, authHeader } from "../../_helpers";

const { webApiUrl } = window.appConfig;

const styles = theme => ({
    grow: {
        flexGrow: 1
    },
    bold: {
        fontWeight: "bold"
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
        }
    },
    innerContainer: {
        padding: theme.spacing(2)
    },
    bigAvatar: {
        margin: 'auto',
        width: 120,
        height: 120
    },
    button: {
        '&:hover': {
            color: theme.palette.primary.contrastText
        }
    },
    dropzoneContainer: {
        width: '33.33%',
        float: 'left',
        padding: theme.spacing(1),
        marginBottom: -10,
        [theme.breakpoints.down("md")]: {
            width: '40%',
        },
        [theme.breakpoints.down("sm")]: {
            width: '50%',
        }
    },
    imageBlock: {
        width: '16.66%',
        float: 'left',
        padding: theme.spacing(1),
        position: 'relative',
        '& > button': {
            position: 'absolute',
            top: 0,
            right: 0
        },
        [theme.breakpoints.down("md")]: {
            width: '20%',
        },
        [theme.breakpoints.down("sm")]: {
            width: '25%',
        },
        [theme.breakpoints.down("xs")]: {
            width: '50%',
        }
    }
});


const SortableItem = SortableElement(({ image, classes, attachmentToDelete, handleDeleteFile, canEdit }) =>
    <div className={classes && classes.imageBlock}>
        <div style={
            {
                backgroundImage: 'url(' + webApiUrl + 'api/home/GetAttachment/' + encodeURI(image.id) + ')',
                backgroundSize: 'cover',
                backgroundPosition: 'center',
                width: '100%',
                padding: '50% 0',
                height: 0
            }}>
        </div>
        <IconButton color="secondary" aria-label="delete" disabled={attachmentToDelete === image.id || !canEdit} onClick={() => handleDeleteFile(image.id)}>
            {
                attachmentToDelete === image.id ?
                    <CircularProgress color="inherit" size={24} />
                    :
                    <Delete />
            }
        </IconButton>
    </div>);

const SortableList = SortableContainer(({ images, classes, selectedOption, uploading, attachmentToDelete, handleUploadFiles, handleDeleteFile, canEdit }) => {
    return (
        <div>
            <div className={classes.dropzoneContainer}>
                <Dropzone onFilesAdded={handleUploadFiles} disabled={!selectedOption || uploading === true || !canEdit} uploading={uploading} accept="image/*" />
            </div>

            {images.map((image, index) => (
                <SortableItem disabled={!canEdit} canEdit={canEdit} key={`item-${image.id}`} index={index} image={image} classes={classes} attachmentToDelete={attachmentToDelete} handleDeleteFile={handleDeleteFile} />
            ))}
        </div>
    );
});

class Page extends React.Component {
    state = {
        options: [],
        uploading: false,
        selectedOption: null,
        prevOption: null,
        nextOption: null,
        images: []
    };

    componentDidMount() {
        this.UpdateStandardOptions();
    }

    componentDidUpdate(prevProps, prevState) {
        if (!prevProps.selected || !this.props.selected || prevProps.selected.projectId.toUpperCase() !== this.props.selected.projectId.toUpperCase()) {
            this.UpdateStandardOptions();
        }
        if (this.state.selectedOption !== prevState.selectedOption) {
            this.UpdateImages();
        }
    }

    UpdateStandardOptions() {
        const { selected } = this.props;
        if (selected) {
            const url = webApiUrl + 'api/shopping/GetStandardOptionsPerProject/' + encodeURI(selected.projectId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            this.setState({
                options: [],
                uploading: false,
                selectedOption: null,
                images: []
            });

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({ options: findResponse });
                });
        }
    }

    UpdateImages() {
        const { selectedOption } = this.state;
        if (selectedOption) {
            const url = webApiUrl + 'api/shopping/GetStandardOptionImageList/' + encodeURI(selectedOption.optionStandardId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({ images: findResponse });
                });
        }
        else {
            this.setState({ images: [] });
        }
    }

    UpdateOrderOfImages(lstIds) {
        const { selectedOption } = this.state;
        const { t } = this.props;
        if (selectedOption) {
            const url = webApiUrl + 'api/shopping/SortStandardOptionAttachments';
            const requestOptions = {
                method: 'POST',
                headers: authHeader('application/json'),
                body: JSON.stringify(lstIds)
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                })
                .catch(e => {
                    alert(t('general.api.error'));
                });
        }
    }

    markOptionSelected = option => {
        const { options } = this.state;
        var index = options.findIndex(x => option && x.optionStandardId === option.optionStandardId);
        const prevOption = index > 0 ? options[index - 1] : null;
        const nextOption = index >= 0 && index != options.length - 1 ? options[index + 1] : null;
        console.debug(index)
        this.setState({ selectedOption: option, prevOption, nextOption });
    }

    arrayMove(arr, old_index, new_index) {
        if (new_index >= arr.length) {
            var k = new_index - arr.length + 1;
            while (k--) {
                arr.push(undefined);
            }
        }
        arr.splice(new_index, 0, arr.splice(old_index, 1)[0]);
        return arr;
    };

    onSortEnd = ({ oldIndex, newIndex }) => {
        const { images } = this.state;
        const oldOrderIds = images.map(x => x.id);
        const updatedOrder = this.arrayMove(images, oldIndex, newIndex);
        this.setState({
            images: updatedOrder,
        });

        var ids = updatedOrder.map(x => x.id)//.join();

        if (JSON.stringify(ids) !== JSON.stringify(oldOrderIds))
            this.UpdateOrderOfImages(ids);
    };

    handleUploadFiles = files => {
        let filesToUpload = [];
        for (let i = 0; i < files.length; i++) {
            if (validateFile(files[i], true) === true) {
                filesToUpload.push(files[i]);
            }
        }

        if (filesToUpload.length > 0) {
            this.setState({ uploading: true })
            const { selected, t, user } = this.props;
            const { selectedOption, images } = this.state;

            const formData = new FormData();

            for (let i = 0; i < filesToUpload.length; i++) {
                formData.append('files', filesToUpload[i])
            }

            fetch(webApiUrl + `api/shopping/UploadStandardOptionImages/` + selectedOption.optionStandardId, {
                method: 'POST',
                headers: authHeader(),
                body: formData
            })
                .then(Response => Response.json())
                .then(res => {
                    this.UpdateImages();
                    this.setState({ uploading: false });
                })
                .catch(e => {
                    this.setState({ uploading: false });
                    alert(t('general.api.error'));
                })
        }
    }

    handleDeleteFile = attachmentId => {
        const { t } = this.props;
        this.setState({ attachmentToDelete: attachmentId });

        fetch(webApiUrl + `api/shopping/DeleteStandardOptionAttachment/` + attachmentId, {
            method: 'DELETE',
            headers: authHeader()
        })
            .then(Response => Response.json())
            .then(res => {
                this.UpdateImages();
            })
            .catch(e => {
                this.setState({ attachmentToDelete: null });
                alert(t('general.api.error'));
            })
    }

    render() {
        const { t, classes, rights } = this.props;
        const { options, uploading, selectedOption, prevOption, nextOption, images, attachmentToDelete } = this.state;

        return (
            <Container className={classes.mainContainer} maxWidth={false}>
                <Grid container>
                    <Grid item container xs={12} className={classes.container}>
                        <AppBar position="sticky">
                            <Toolbar variant="dense">
                                {
                                    selectedOption ?
                                        <React.Fragment>
                                            <IconButton aria-label="GoBack" edge="start" color="inherit" onClick={() => this.markOptionSelected(null)}>
                                                <ArrowBack />
                                            </IconButton>
                                            <Typography className={clsx(classes.grow, classes.bold)} noWrap>{selectedOption.optionNo + ' ' + selectedOption.description}</Typography>
                                            <Tooltip title={t('Vorige optie')}>
                                                <IconButton aria-label="Prev" color="inherit" disabled={!prevOption} onClick={() => this.markOptionSelected(prevOption)}>
                                                    <ChevronLeft />
                                                </IconButton>
                                            </Tooltip>
                                            <Tooltip title={t('Volgende optie')}>
                                                <IconButton aria-label="Prev" edge="end" color="inherit" disabled={!nextOption} onClick={() => this.markOptionSelected(nextOption)}>
                                                    <ChevronRight />
                                                </IconButton>
                                            </Tooltip>
                                        </React.Fragment>
                                        :
                                        <Typography className={clsx(classes.grow, classes.bold)}>{t('Standaard opties configuratie')}</Typography>
                                }
                            </Toolbar>
                        </AppBar>
                        {
                            selectedOption ?
                                <Grid item xs={12} className={classes.innerContainer}>
                                    <SortableList canEdit={rights['page.standardOptions.write']} axis="xy" distance={10} images={images} classes={classes} selectedOption={selectedOption} uploading={uploading} attachmentToDelete={attachmentToDelete} handleUploadFiles={this.handleUploadFiles} handleDeleteFile={this.handleDeleteFile} onSortEnd={this.onSortEnd} />
                                </Grid>
                                :
                                <Grid item xs={12}>
                                    <StandardOptionsGrid options={options} onSelect={this.markOptionSelected} />
                                </Grid>
                        }
                    </Grid>
                </Grid>
            </Container>
        );
    }
}

function mapStateToProps(state) {
    const { authentication, buildings } = state;
    const { user } = authentication;
    const { selected, rights } = buildings;
    return {
        user,
        selected,
        rights
    };
}

const connectedPage = connect(mapStateToProps)(withTranslation()(withStyles(styles)(Page)));
export { connectedPage as StandardOptionsConfigPage };