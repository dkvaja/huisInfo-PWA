import React, { Component } from 'react'
import { withStyles } from '@material-ui/core/styles';
import clsx from "clsx";
import { CloudUpload } from '@material-ui/icons';
import { CircularProgress } from '@material-ui/core';
import UploadFiles from '../Pages/Dossiers/Common/UploadFiles';

const styles = theme => ({
    dropzone: {
        height: 0,
        width: '100%',
        padding: '50% 0',
        backgroundColor: '#fff',
        border: '2px dashed rgb(187, 186, 186)',
        borderRadius: 5,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        flexDirection: 'column',
        fontSize: 16,
        '&.highlight': {
            backgroundColor: 'rgb(188, 185, 236)'
        }
    },
    icon: {
        opacity: 0.3,
        height: 64,
        width: 64
    },
    fileInput: {
        display: 'none'
    }
});


class Dropzone extends Component {
    constructor(props) {
        super(props)
        this.state = { hightlight: false, isUploadFilesDialogOpen: false }
        this.fileInputRef = React.createRef()

        this.openFileDialog = this.openFileDialog.bind(this)
        this.onFilesAdded = this.onFilesAdded.bind(this)
        this.onDragOver = this.onDragOver.bind(this)
        this.onDragLeave = this.onDragLeave.bind(this)
        this.onDrop = this.onDrop.bind(this)
    }

    openFileDialog(e) {
        if (this.props.disabled) return
        if (this.props.withUploadFileDialog) {
            this.setState({ isUploadFilesDialogOpen: true })
            return false;
        }
        this.fileInputRef.current.click()
    }

    onFilesAdded(evt) {
        if (this.props.disabled) return
        const files = evt.target.files
        if (this.props.onFilesAdded) {
            const array = this.fileListToArray(files)
            this.props.onFilesAdded(array)
        }
    }

    onDragOver(evt) {
        evt.preventDefault()

        if (this.props.disabled) return

        this.setState({ hightlight: true })
    }

    onDragLeave() {
        this.setState({ hightlight: false })
    }

    onDrop(event) {
        event.preventDefault()

        if (this.props.disabled) return

        const files = event.dataTransfer.files
        if (this.props.onFilesAdded) {
            const array = this.fileListToArray(files)
            this.props.onFilesAdded(array)
        }
        this.setState({ hightlight: false })
    }

    fileListToArray(list) {
        const array = []
        for (var i = 0; i < list.length; i++) {
            array.push(list.item(i))
        }
        return array
    }

    handleUploadFilesDialogClose = (e) => {
        this.setState({ isUploadFilesDialogOpen: false })
        return false;
    }

    render() {
        const { classes, iconClasses = {}, className = {}, uploading, handleSelectExistingImages, handlePreviewOfFiles } = this.props;
        return (
            <React.Fragment>
                {this.props.withUploadFileDialog && <UploadFiles
                    selectedDossier={this.props.selectedObjects}
                    handlePreviewOfFiles={handlePreviewOfFiles}
                    handleSelectExistingImages={(data) => {
                        handleSelectExistingImages(data);
                        this.setState({ isUploadFilesDialogOpen: false })
                    }}
                    buildings={this.props.buildings}
                    handleSubmit={(f) => this.props.onFilesAdded(f)}
                    accept={this.props.accept || '*'}
                    open={this.state.isUploadFilesDialogOpen}
                    onClose={this.handleUploadFilesDialogClose}
                    accept={this.props.accept || '*'} />}
                <div
                    className={this.state.hightlight ? clsx(classes.dropzone, 'highlight', className) : clsx(classes.dropzone, className)}
                    onDragOver={this.onDragOver}
                    onDragLeave={this.onDragLeave}
                    onDrop={this.onDrop}
                    onClick={this.openFileDialog}
                    style={{ cursor: this.props.disabled ? 'default' : 'pointer' }}
                >
                    <input
                        ref={this.fileInputRef}
                        className={classes.fileInput}
                        type="file"
                        accept={this.props.accept ? this.props.accept : "*"}
                        multiple
                        onChange={this.onFilesAdded}
                    />
                    {
                        uploading === true ?
                            <CircularProgress className={classes.icon} />
                            :
                            <CloudUpload className={clsx(classes.icon, iconClasses)} />
                    }
                </div>
            </React.Fragment>
        )
    }
}

export default withStyles(styles)(Dropzone)
