import React, { useState, useEffect } from "react";
import { IconButton, Popover, makeStyles, Tooltip, Dialog, DialogTitle, DialogContent, DialogContentText, TextField, DialogActions, Button, Typography, Grid, ButtonBase, Checkbox, Container, FormControlLabel, CircularProgress, Icon } from "@material-ui/core";
import { Add, PostAdd, Edit, Save, Cancel, DeleteOutline } from "@material-ui/icons";
import MUIDataTable from "mui-datatables";
import { useTranslation } from "react-i18next";
import { getDataTableTextLabels, authHeader } from "../../_helpers";
import RichTextEditor from "./RichTextEditor";
import Markdown from "../../components/Markdown";

const { webApiUrl } = window.appConfig;

const useStyles = makeStyles((theme) => ({
    grow: {
        flexGrow: 1
    },
    grid: {
        maxWidth: 980,
        padding: theme.spacing(0, 1, 1),
        [theme.breakpoints.down("md")]: {
            maxWidth: '100%'
        },
        '& .MuiToolbar-root': {
            margin: theme.spacing(0, -2, -0.875)
        },
        '& .MuiTableRow-root': {
            verticalAlign: 'top',
        },
        '& .MuiTableCell-root': {
            padding: theme.spacing(1),
            '&.MuiTableCell-head': {
                fontWeight: 'bold'
            }
        },
    },
    iconButton: {
        //margin: theme.spacing(-1, 0)
    },
    markdownBlock: {
        display: '-webkit-box',
        maxWidth: 400,
        maxHeight: 40,
        '-webkit-line-clamp': 2,
        '-webkit-box-orient': 'vertical',
        overflow: 'hidden',
        textOverflow: 'ellipsis',
    }
}));

export default function StandardTextManager(props) {
    const { disabled, onSelect, onChange, selectedBuilding, ...rest } = props;
    const { t } = useTranslation();
    const classes = useStyles();
    const [anchorEl, setAnchorEl] = React.useState(null);
    const [openDialog, setOpenDialog] = React.useState(false);
    const [color, setColor] = React.useState("default");
    const [standardTexts, setStandardTexts] = React.useState(null);
    const [popupBlock, setPopupBlock] = React.useState(null);

    useEffect(() => {
        UpdateStandardTexts()
    }, [selectedBuilding]);

    useEffect(() => {
        onChange(standardTexts);
    }, [standardTexts]);

    function UpdateStandardTexts() {
        if (selectedBuilding && selectedBuilding.projectId) {
            const url = webApiUrl + 'api/chat/GetStandardTextsByProject/' + encodeURI(selectedBuilding.projectId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    setStandardTexts(findResponse)
                })
                .catch(e => {
                    setTimeout(UpdateStandardTexts, 1000);
                });
        }
    }

    const handleClickAdd = isSignature => {
        setOpenDialog(true);
        let tempObj = {
            hashtag: '#',
            textBlock: '',
            isSignature
        };

        if (isSignature === true) {
            const textToEdit = standardTexts.find(x => x.isSignature === true);
            if (textToEdit) {
                tempObj = {
                    ...textToEdit,
                    hashtag: '#' + textToEdit.hashtag,
                };
            }
            else {
                tempObj.hashtag = '#handtekening'
            }
        }

        setPopupBlock(tempObj);
    };

    const handleCloseAdd = () => {
        setOpenDialog(false);
    };

    const handleClick = (event) => {
        setAnchorEl(event.currentTarget);
        setColor("primary");
    };

    const handleClose = () => {
        setAnchorEl(null);
        setColor("default");
    };

    const handleClickSave = () => {
        const texts = standardTexts.slice();
        if (!!popupBlock.textId) {
            const textToUpdate = texts.find(x => x.textId === popupBlock.textId);
            if (textToUpdate) {
                const tag = popupBlock.hashtag.substr(1);
                if (textToUpdate.hashtag !== tag || textToUpdate.textBlock !== popupBlock.textBlock) {
                    const url = webApiUrl + 'api/chat/UpdateStandardText';
                    const requestOptions = {
                        method: 'PATCH',
                        headers: authHeader('application/json'),
                        body: JSON.stringify({
                            textId: popupBlock.textId,
                            hashtag: tag,
                            textBlock: popupBlock.textBlock,
                            isSignature: popupBlock.isSignature
                        })
                    };

                    fetch(url, requestOptions)
                        .then(Response => Response.json())
                        .then(success => {
                            UpdateStandardTexts();
                            setPopupBlock(null);
                            setOpenDialog(false);
                        })
                        .catch(e => {
                            //this.setState({ uploading: false });
                            alert(t('general.api.error'));
                        });
                }
                else {
                    setPopupBlock(null);
                    setOpenDialog(false);
                }
            }
        }
        else if (selectedBuilding && selectedBuilding.projectId) {
            if (popupBlock.hashtag !== '#' && !standardTexts.find(x => '#' + x.hashtag == popupBlock.hashtag)) {
                const url = webApiUrl + 'api/chat/AddStandardTextForProject';
                const requestOptions = {
                    method: 'POST',
                    headers: authHeader('application/json'),
                    body: JSON.stringify({
                        projectId: selectedBuilding.projectId,
                        hashtag: popupBlock.hashtag.substr(1),
                        textBlock: popupBlock.textBlock,
                        isSignature: popupBlock.isSignature
                    })
                };

                fetch(url, requestOptions)
                    .then(Response => Response.json())
                    .then(success => {
                        UpdateStandardTexts();
                        setPopupBlock(null);
                        setOpenDialog(false);
                    })
                    .catch(e => {
                        //this.setState({ uploading: false });
                        alert(t('general.api.error'));
                    });
            }
        }
    }

    const handleEdit = textId => {
        setOpenDialog(true);
        const textToEdit = standardTexts.find(x => x.textId === textId);
        setPopupBlock({
            ...textToEdit,
            hashtag: '#' + textToEdit.hashtag,
        });
    }

    const handleChangeHashtag = value => {
        if (value.match(/^#[A-Za-z0-9\-\.\_]*$/g) && value.length <= 41) {
            let error = false;
            const texts = standardTexts.slice();
            if (value === '#' || texts.find(x => x.textId !== popupBlock.textId && '#' + x.hashtag === value)) {
                error = true;
            }
            setPopupBlock({
                ...popupBlock,
                hashtag: value,
                hashtagError: error
            });
        }
    }
    const handleChangeTextBlock = value => {
        setPopupBlock({
            ...popupBlock,
            textBlock: value,
            textBlockError: !value || value.trim() === ''
        });
    }

    const handleDelete = textId => {
        const url = webApiUrl + 'api/chat/DeleteStandardText/' + encodeURI(textId);
        const requestOptions = {
            method: 'DELETE',
            headers: authHeader()
        };

        fetch(url, requestOptions)
            .then(Response => Response.json())
            .then(success => {
                UpdateStandardTexts();
            })
            .catch(e => {
                //this.setState({ uploading: false });
                alert(t('general.api.error'));
            });
    }

    const open = Boolean(anchorEl);
    const id = open ? 'emoji-popover' : undefined;
    const columns = [
        {
            name: 'textId',
            options: {
                display: 'excluded',
                filter: false,
                print: false,
                download: false
            }
        },
        {
            name: 'hashtag',
            label: t('#hashtag'),
            options: {
                filter: false,
                sort: false,
                print: false,
                download: false,
                customBodyRender: (value) => '#' + value
            }
        },
        {
            name: 'textBlock',
            label: t('Tekst'),
            options: {
                filter: false,
                sort: false,
                print: false,
                download: false,
                customBodyRender: (value) => {
                    return (
                        <ButtonBase
                            className={classes.markdownBlock}
                            component='div'
                            focusRipple
                            onClick={
                                () => {
                                    onSelect(value);
                                    handleClose();
                                }
                            }
                        >
                            <Markdown value={value} />
                        </ButtonBase>
                    )
                }
            }
        },
        {
            name: 'action',
            label: t('Actie'),
            options: {
                filter: false,
                sort: false,
                customBodyRender: (value, tableMeta, updateValue) => {
                    const textId = tableMeta.rowData[0];
                    return (
                        <React.Fragment>
                            <Tooltip title={t("Wijzig")}>
                                <IconButton className={classes.iconButton} size="small" color="primary" aria-label="edit" onClick={() => handleEdit(textId)}>
                                    <Edit fontSize="small" />
                                </IconButton>
                            </Tooltip>
                            <Tooltip title={t("Verwijder")}>
                                <IconButton className={classes.iconButton} size="small" color="primary" aria-label="delete" onClick={() => handleDelete(textId)}>
                                    <DeleteOutline fontSize="small" />
                                </IconButton>
                            </Tooltip>
                        </React.Fragment>
                    )
                }
            }
        },
    ];

    const options = {
        filter: false,
        download: false,
        print: false,
        viewColumns: false,
        pagination: false,
        responsive: 'scrollFullHeight',
        selectableRows: 'none',
        textLabels: getDataTableTextLabels(t),
        customToolbar: () => {
            return (
                standardTexts &&
                <React.Fragment>
                    <Tooltip title={t("Handtekening")}>
                        <IconButton onClick={() => handleClickAdd(true)}>
                            <Icon className="fas fa-signature" />
                        </IconButton>
                    </Tooltip>
                    <Tooltip title={t("Toevoegen")}>
                        <IconButton edge="end" onClick={() => handleClickAdd(false)}>
                            <Add />
                        </IconButton>
                    </Tooltip>
                    <Dialog open={openDialog} onClose={handleCloseAdd} aria-labelledby="form-dialog-title" maxWidth='md' fullWidth>
                        {
                            popupBlock &&
                            <React.Fragment>
                                <DialogTitle id="form-dialog-title">{
                                    !!popupBlock.textId
                                        ?
                                        (
                                            popupBlock.isSignature ? t('Wijzig handtekening') : t('Wijzig standaard tekst')
                                        )
                                        :
                                        (
                                            popupBlock.isSignature ? t('Handtekening') : t('Voeg standaard tekst toe')
                                        )
                                }</DialogTitle>
                                <DialogContent>
                                    {
                                        //<DialogContentText>
                                        //    To subscribe to this website, please enter your email address here. We will send updates
                                        //    occasionally.
                                        //</DialogContentText>
                                    }
                                    {
                                        <TextField
                                            autoFocus
                                            margin="dense"
                                            id="name"
                                            label="Hashtag *"
                                            type="text"
                                            value={popupBlock.hashtag}
                                            maxLength={10}
                                            onChange={e => handleChangeHashtag(e.target.value)}
                                            fullWidth
                                            error={popupBlock.hashtagError === true}
                                            helperText={popupBlock.hashtagError === true && popupBlock.hashtag !== '#' && (popupBlock.hashtag + t(' is al in gebruik, kies een andere #hashtag'))}
                                        />
                                    }
                                    <br />
                                    <br />
                                    <Typography variant="caption">{t('Tekst')}&nbsp;*</Typography>
                                    <RichTextEditor
                                        label="Tekst..."
                                        showToolbar={true}
                                        onChange={data => handleChangeTextBlock(data)}
                                        value={popupBlock.textBlock}
                                    />
                                </DialogContent>
                                <DialogActions>
                                    <Button onClick={handleCloseAdd} color="secondary">{t('Annuleren')}</Button>
                                    <Button onClick={handleClickSave} color="primary" disabled={popupBlock.hashtagError === true || popupBlock.textBlockError === true}>{t('Opslaan')}</Button>
                                </DialogActions>
                            </React.Fragment>
                        }
                    </Dialog>
                </React.Fragment>
            );
        }
    };

    return (
        <React.Fragment>
            <Tooltip title={t("Standaard teksten")}>
                <IconButton aria-describedby={id} disabled={disabled} onClick={handleClick} color={color} {...rest}>
                    <PostAdd />
                </IconButton>
            </Tooltip>
            <Popover
                id={id}
                open={open && disabled !== true}
                className={classes.popover}
                anchorEl={anchorEl}
                onClose={handleClose}
                anchorOrigin={{
                    vertical: 'top',
                    horizontal: 'center',
                }}
                transformOrigin={{
                    vertical: 'bottom',
                    horizontal: 'center',
                }}
            >
                {
                    standardTexts ?
                        <MUIDataTable
                            className={classes.grid}
                            title={t('Standaard teksten')}
                            data={standardTexts.filter(x => !x.isSignature)}
                            columns={columns}
                            options={options}
                        />
                        :
                        <CircularProgress color="primary" size={24} />
                }
            </Popover>
        </React.Fragment>
    );
}