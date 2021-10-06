import React from 'react';
import NumberFormat from 'react-number-format';
import { makeStyles, useTheme } from '@material-ui/core/styles';
import Card from '@material-ui/core/Card';
import CardActionArea from '@material-ui/core/CardActionArea';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import CardMedia from '@material-ui/core/CardMedia';
import Button from '@material-ui/core/Button';
import Grid from '@material-ui/core/Grid';
import Typography from '@material-ui/core/Typography';
import { Chip, CardHeader, Avatar, IconButton, Collapse, TextField, InputBase, ButtonGroup, InputAdornment, useMediaQuery, Popover, Modal, Backdrop, Fade, Dialog, DialogTitle, DialogContent } from '@material-ui/core';
import { MoreVert as MoreVertIcon, Add, Remove, Close, Edit, FilterNone, Lock, AssignmentTurnedIn, KeyboardArrowLeft, KeyboardArrowRight } from '@material-ui/icons';
import { useTranslation } from 'react-i18next'
import { history, nl2br, authHeader, getStorageImagesForStandardOption, setStorageImagesForStandardOption, getStorageImagesForIndividualOption, setStorageImagesForIndividualOption } from "../../_helpers"
import { optionStatusConstants } from '../../_constants';
import { maxHeight } from '@material-ui/system';

const { webApiUrl } = window.appConfig;

const useStyles = makeStyles(theme => ({
    card: {
        boxShadow: theme.shadows[0],
        borderWidth: 1,
        borderStyle: 'solid',
        borderColor: theme.palette.grey[200],
        margin: theme.spacing(1, -2),
    },
    fullWidth: { width: '100%' },
    mainContent: {
        padding: theme.spacing(2)
    },
    bottomContent: {
        padding: theme.spacing(0, 2, 2)
    },
    actionButton: {
        borderTopWidth: 0,
        borderRightWidth: 0,
        borderBottomWidth: 1,
        borderLeftWidth: 1,
        borderStyle: 'solid',
        borderColor: theme.palette.grey[200],
        padding: 6,
        display: 'inline-block',
        float: "right"
    },
    cardActionArea: {
        '&:focus': {
            outline: 'none'
        }
    },
    bold: {
        fontWeight: "bold"
    },
    price: {
        color: '#ff8000',
        fontWeight: "bold"
    },
    mediaMobileWrapper: {
        position: "relative",
        top: 'calc(50% - 18px)'
    },
    media: {
        width: 100,
        padding: '35% 0',
        backgroundSize: 'contain',
        [theme.breakpoints.up("sm")]: {
            width: 200
        }
    },
    areaCollapse: {
        backgroundColor: theme.palette.grey[100]
    },
    numberFormat: {
        width: 160,
        margin: 0,
        '& input': {
            textAlign: 'center',
            fontWeight: 'bold',
            color: theme.palette.primary.main
        }
    },
    numberFormatDecimal: {
        width: 190,
        margin: 0,
        '& input': {
            textAlign: 'center',
            fontWeight: 'bold',
            color: theme.palette.primary.main
        }
    },
    popoverTypography: {
        padding: theme.spacing(2)
    },
    modal: {
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
    },
    modalCard: {
        width: 'auto'
    },

    dialogTitle: {
        padding: 0,
        backgroundColor: theme.palette.primary.main,
        color: theme.palette.primary.contrastText,
        '& .MuiCardHeader-content': {
            overflow: 'hidden'
        },
        '& .MuiCardHeader-action': {
            marginBottom: -8
        }
    },
    dialogContent: {
        padding: 0
    },
    cardMedia: {
        backgroundSize: 'contain',
        width: '100%',
        padding: '75% 0 0',
        height: 0
    }
}));

export default function OptionCard(props) {
    const { t } = useTranslation()
    const classes = useStyles();
    const { option, user, selected, closed, onAddOption, unit, decimalPlaces, ...others } = props;
    const [expanded, setExpanded] = React.useState(false);
    const [quantity, setQuantity] = React.useState(selected ? selected.quantity : (option.quantity <= 0 ? 1 : option.quantity));
    const [comment, setComment] = React.useState(selected ? selected.additionalDescription : '');
    const [prevSelected, setPrevSelected] = React.useState(null);
    const theme = useTheme();
    const matches = useMediaQuery(theme.breakpoints.up('sm'));
    const [anchorEl, setAnchorEl] = React.useState(null);
    const [images, setImages] = React.useState(
        !!option.optionStandardId
            ?
            getStorageImagesForStandardOption(option.optionStandardId)
            :
            getStorageImagesForIndividualOption(option.optionId)
    );
    const [isImageListLoaded, setIsImageListLoaded] = React.useState(false);
    const [openImage, setOpenImage] = React.useState(false);
    const [imageIndex, setImageIndex] = React.useState(0);

    React.useEffect(() => {
        if (selected) {
            if (prevSelected) {
                if (selected.quantity !== prevSelected.quantity) {
                    setQuantity(selected.quantity);
                    setPrevSelected(selected);
                }
                if (selected.additionalDescription !== prevSelected.additionalDescription) {
                    setComment(selected.additionalDescription);
                    setPrevSelected(selected);
                }
            }
            else {
                setQuantity(selected.quantity);
                setComment(selected.additionalDescription);
                setPrevSelected(selected);
            }
        }


        //if (selected && (
        //    prevSelected ?
        //        (selected.quantity !== prevSelected.quantity || selected.additionalDescription !== prevSelected.additionalDescription)
        //        :
        //        (selected.quantity !== quantity || selected.additionalDescription !== comment)
        //)) {
        //    setQuantity(selected.quantity);
        //    setComment(selected.additionalDescription);
        //    setPrevSelected(selected);
        //}
    });

    if (!isImageListLoaded) {
        if (option.optionStandardId) {
            const url = webApiUrl + 'api/shopping/GetStandardOptionImageList/' + encodeURI(option.optionStandardId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    setImages(findResponse);
                    setIsImageListLoaded(true);
                    setStorageImagesForStandardOption(option.optionStandardId, findResponse);
                });
        }
        else {
            const url = webApiUrl + 'api/shopping/GetSelectedOptionImageList/' + encodeURI(option.optionId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    setImages(findResponse);
                    setIsImageListLoaded(true);
                    setStorageImagesForIndividualOption(option.optionId, findResponse);
                });
        }
    }

    function handleChipClick(event) {
        event.stopPropagation();
        setAnchorEl(event.currentTarget);
    }

    function handlePopoverClose() {
        setAnchorEl(null);
    }

    const open = Boolean(anchorEl);
    const popoverId = open ? 'simple-popover' : undefined;

    function handleExpandClick() {
        if (!closed) {
            setExpanded(!expanded);
        }
    }

    const handleChangeComment = event => {
        setComment(event.target.value);
    };

    const handleChangeQuantity = event => {
        var value = event.floatValue;
        setQuantity(value);
    };

    const isAllowedQuantity = obj => {
        var value = obj.floatValue;
        return value && (value >= 1 && value < 10000);
    }

    const handleIncrementQuantity = () => {
        var value = quantity + 1;
        if (value < 10000)
            setQuantity(value);
    }
    const handleDecrementQuantity = () => {
        var value = quantity - 1;
        if (value >= 1)
            setQuantity(value);
    }

    const handleButtonSubmit = () => {
        onAddOption(quantity, comment);
        handleExpandClick();
    }

    const handleImageModalOpen = event => {
        event.preventDefault();
        event.stopPropagation();
        setOpenImage(true);
    }
    const handleImageModalClose = () => {
        setOpenImage(false);
    }

    return (
        <Card className={classes.card} {...others} ref={option.ref} >
            <CardActionArea className={classes.cardActionArea} disabled={closed} onClick={handleExpandClick}
                aria-expanded={expanded}>
                <Grid container>
                    <Grid item xs container spacing={2} className={classes.mainContent}>

                        <Grid item xs container direction="column">
                            <Grid item xs>
                                <Typography variant="body1" className={classes.bold} paragraph={option.type !== 1}>{option.optionNo + ' - ' + option.description}</Typography>
                                {
                                    option.type === 1 &&
                                    <Typography variant="body2">{option.category + ' - ' + option.header}</Typography>
                                }
                                <Typography variant="body2" color="textSecondary">{nl2br(option.commercialDescription)}</Typography>
                            </Grid>

                        </Grid>
                        {
                            matches && images && images.length > 0 &&
                            <Grid item>
                                <CardMedia
                                    className={classes.media}
                                    image={webApiUrl + 'api/home/GetAttachment/' + encodeURI(images[0].id)}
                                    title={option.description}
                                    onClick={(event) => handleImageModalOpen(event)}
                                />
                            </Grid>
                        }
                    </Grid>
                    <Grid item>
                        <div className={classes.actionButton}>
                            {
                                closed ?
                                    <Lock color="disabled" />
                                    :
                                    expanded ?
                                        <Close />
                                        :
                                        selected ?
                                            <Edit color="primary" />
                                            :
                                            <Add color="primary" />
                            }
                        </div>
                        {
                            !matches && images && images.length > 0 &&
                            <div className={classes.mediaMobileWrapper}>
                                <CardMedia
                                    className={classes.media}
                                    image={webApiUrl + 'api/home/GetAttachment/' + encodeURI(images[0].id)}
                                    title={option.description}
                                    onClick={(event) => handleImageModalOpen(event)}
                                />
                            </div>
                        }
                    </Grid>
                    <Grid item container className={classes.bottomContent}>
                        <Grid item xs>
                            <Typography variant="body1" noWrap className={classes.price}>
                                {
                                    !option.salesPriceEstimated && !option.salesPriceToBeDetermined &&
                                    <React.Fragment>&euro; </React.Fragment>
                                }
                                {option.salesPriceInclVAT_Text}
                                &nbsp;
                            {
                                    unit &&
                                    <React.Fragment>
                                        {t('per')}
                                        &nbsp;
                                    {unit}
                                    </React.Fragment>
                                }
                            </Typography>
                        </Grid>
                        {
                            selected &&
                            <Grid item>
                                <Chip aria-describedby={popoverId} onClick={handleChipClick} size="small" label={
                                    <React.Fragment>
                                        <NumberFormat
                                            displayType="text"
                                            decimalScale={decimalPlaces}
                                            fixedDecimalScale={true}
                                            thousandSeparator="."
                                            decimalSeparator=","
                                            value={selected.quantity}
                                            suffix="&nbsp;"
                                        />
                                        <span>{unit}&nbsp;&euro;&nbsp;</span>
                                        <NumberFormat
                                            displayType="text"
                                            decimalScale={2}
                                            fixedDecimalScale={true}
                                            thousandSeparator="."
                                            decimalSeparator=","
                                            value={selected.quantity * option.salesPriceInclVAT}
                                        />
                                    </React.Fragment>
                                } color="primary" className={classes.bold} />
                                {
                                    selected.additionalDescription && selected.additionalDescription.trim() !== '' &&
                                    <Popover
                                        id={popoverId}
                                        open={open}
                                        anchorEl={anchorEl}
                                        onClose={handlePopoverClose}
                                        anchorOrigin={{
                                            vertical: 'bottom',
                                            horizontal: 'center',
                                        }}
                                        transformOrigin={{
                                            vertical: 'top',
                                            horizontal: 'center',
                                        }}
                                        onClick={(event) => event.stopPropagation()}
                                    >
                                        <Typography className={classes.popoverTypography}>
                                            {
                                                nl2br(selected.additionalDescription)
                                            }
                                        </Typography>
                                    </Popover>}
                            </Grid>
                        }
                    </Grid>
                </Grid>
            </CardActionArea>
            {false &&
                <CardActions>
                    <Button size="small" color="primary">
                        Share
        </Button>
                    <Button size="small" color="primary">
                        Learn More
        </Button>
                </CardActions>
            }
            {
                !closed &&
                <Collapse className={classes.areaCollapse} in={expanded} timeout="auto" unmountOnExit>
                    <Grid container direction="column"
                        alignItems="center"
                        justify="center" className={classes.mainContent}>
                        <Grid item md={6} container spacing={1}>
                            {
                                option.type !== 1 &&
                                <Grid item xs={12}>
                                    <TextField
                                        id="standard-textarea"
                                        label={t('Opmerkingen')}
                                        multiline
                                        variant="outlined"
                                        value={comment}
                                        fullWidth
                                        margin="dense"
                                        onChange={handleChangeComment}
                                        disabled={user.viewOnly === true}
                                    />
                                </Grid>
                            }
                            <Grid item xs={12} >
                                <Grid item container spacing={1} alignItems="center">
                                    <Grid item>
                                        <NumberFormat
                                            disabled={option.type === 1 || user.viewOnly === true}
                                            className={decimalPlaces > 0 ? classes.numberFormatDecimal : classes.numberFormat}
                                            customInput={TextField}
                                            decimalScale={decimalPlaces}
                                            fixedDecimalScale={true}
                                            allowNegative={false}
                                            thousandSeparator="."
                                            decimalSeparator=","
                                            value={quantity}
                                            variant="outlined"
                                            onValueChange={handleChangeQuantity}
                                            isAllowed={isAllowedQuantity}
                                            align="center"
                                            margin="dense"
                                            InputProps={{
                                                startAdornment: (
                                                    <InputAdornment position="start">
                                                        <IconButton
                                                            disabled={option.type === 1 || user.viewOnly === true}
                                                            edge="start"
                                                            aria-label="decrement"
                                                            onClick={handleDecrementQuantity}
                                                        >
                                                            <Remove />
                                                        </IconButton>
                                                    </InputAdornment>
                                                ),
                                                endAdornment: (
                                                    <InputAdornment position="end">
                                                        <IconButton
                                                            disabled={option.type === 1 || user.viewOnly === true}
                                                            edge="end"
                                                            aria-label="increment"
                                                            onClick={handleIncrementQuantity}
                                                        >
                                                            <Add />
                                                        </IconButton>
                                                    </InputAdornment>
                                                ),
                                            }}
                                        />
                                    </Grid>
                                    <Grid item>
                                        <Typography>{unit}</Typography>
                                    </Grid>
                                    <Grid item xs>
                                        <Button
                                            variant="contained"
                                            color="primary"
                                            fullWidth
                                            style={{ fontWeight: "bold", fontSize: '1rem' }}
                                            disabled={user.viewOnly === true}
                                            onClick={handleButtonSubmit}>
                                            {
                                                (!option.salesPriceEstimated && !option.salesPriceToBeDetermined) ?
                                                    <NumberFormat
                                                        prefix="€ "
                                                        displayType="text"
                                                        decimalScale={2}
                                                        fixedDecimalScale={true}
                                                        thousandSeparator="."
                                                        decimalSeparator=","
                                                        value={quantity * option.salesPriceInclVAT}
                                                    />
                                                    :
                                                    option.salesPriceInclVAT_Text
                                            }
                                        </Button>
                                    </Grid>
                                </Grid>
                            </Grid>
                        </Grid>
                    </Grid>
                </Collapse>
            }

            {
                images && images.length > 0 &&
                <Dialog onClose={handleImageModalClose} aria-labelledby="simple-dialog-title" open={openImage} fullWidth={true} maxWidth="md">
                    <DialogTitle id="simple-dialog-title" className={classes.dialogTitle} disableTypography>
                        <CardHeader id="transition-dialog-title"
                            title={
                                <Typography variant="h6" noWrap>{option.optionNo + ' - ' + option.description}</Typography>
                            }
                            action={
                                <React.Fragment>
                                    <IconButton color="inherit" aria-label="previous" disabled={imageIndex <= 0} onClick={() => setImageIndex(imageIndex - 1)}>
                                        <KeyboardArrowLeft />
                                    </IconButton>
                                    <IconButton color="inherit" aria-label="next" disabled={imageIndex >= (images.length - 1)} onClick={() => setImageIndex(imageIndex + 1)}>
                                        <KeyboardArrowRight />
                                    </IconButton>
                                    <IconButton color="inherit" aria-label="close" onClick={handleImageModalClose}>
                                        <Close />
                                    </IconButton>
                                </React.Fragment>
                            } />
                    </DialogTitle>
                    <DialogContent className={classes.dialogContent}>
                        <CardMedia
                            component="div"
                            alt={option.description}
                            className={classes.cardMedia}
                            image={webApiUrl + 'api/home/GetAttachment/' + encodeURI(images[imageIndex].id)}
                            title={option.description}
                        />
                    </DialogContent>
                </Dialog>
                //<Modal
                //    aria-labelledby="transition-modal-title"
                //    aria-describedby="transition-modal-description"
                //    className={classes.modal}
                //    open={openImage}
                //    onClose={handleImageModalClose}
                //    closeAfterTransition
                //    BackdropComponent={Backdrop}
                //    BackdropProps={{
                //        timeout: 500,
                //    }}
                //>
                //    <Fade in={openImage}>
                //        <Card style={{ position: 'relative' }}>
                //            <IconButton style={{ position: 'absolute', right: '0' }} onClick={handleImageModalClose}><Close /></IconButton>
                //            <CardMedia
                //                component="img"
                //                alt={option.description}
                //                title={option.description}
                //                image={'/Files/Image/' + images[0].guid}
                //                style={{ maxHeight: '100vh', maxWidth: '100%' }}
                //            />
                //        </Card>
                //    </Fade>
                //</Modal>
            }
        </Card>
    );
}
