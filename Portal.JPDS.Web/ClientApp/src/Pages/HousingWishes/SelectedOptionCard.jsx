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
import { Chip, CardHeader, Avatar, IconButton, Collapse, TextField, InputBase, ButtonGroup, InputAdornment, useMediaQuery, Popover } from '@material-ui/core';
import { MoreVert as MoreVertIcon, Add, Remove, Close, Edit, FilterNone, Lock, AssignmentTurnedIn, AssignmentLate, Assignment, Today } from '@material-ui/icons';
import { useTranslation } from 'react-i18next'
import { history, nl2br, formatDate, authHeader, getStorageImagesForStandardOption, getStorageImagesForIndividualOption, setStorageImagesForStandardOption, setStorageImagesForIndividualOption } from "../../_helpers"
import { optionStatusConstants } from '../../_constants';

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
        top: 'calc(50% - 35px)'
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
    }
}));

export default function SelectedOptionCard(props) {
    const { t } = useTranslation()
    const classes = useStyles();
    const { option, ...others } = props;
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

    const statusText =
        option.optionStatus === 3 ?
            (
                <Chip
                    icon={
                        <AssignmentTurnedIn />
                    }
                    label={t('Definitief')}
                    className={classes.chip}
                    variant="outlined"
                    color="primary"
                    size="small"
                />
            )
            :
            option.optionStatus === 5 ?
                (
                    <Chip
                        icon={
                            <AssignmentLate />
                        }
                        label={t('Online besteld unsigned')}
                        className={classes.chip}
                        variant="outlined"
                        color="secondary"
                        size="small"
                    />
                )
                :
                option.optionStatus === 6 ?
                    <Chip
                        icon={
                            <Assignment />
                        }
                        label={t('Online besteld')}
                        className={classes.chip}
                        variant="outlined"
                        size="small"
                    />
                    :
                    '';

    return (
        <Card className={classes.card} {...others} ref={option.ref} >
            <CardActionArea className={classes.cardActionArea} disabled>
                <Grid container>
                    <Grid item xs container spacing={2} className={classes.mainContent}>

                        <Grid item xs container direction="column">
                            <Grid item xs>
                                <Typography variant="body1" className={classes.bold} >{option.optionNo + ' - ' + option.description}</Typography>
                                <br />
                                <Typography variant="body2" color="textSecondary">{nl2br(option.commercialDescription)}</Typography>
                                <br />
                            </Grid>
                        </Grid>
                        {
                            matches && images && images.length > 0 &&
                            <Grid item>
                                <CardMedia
                                    className={classes.media}
                                    image={webApiUrl + 'api/home/GetAttachment/' + encodeURI(images[0].id)}
                                    title={option.description}
                                />
                            </Grid>
                        }
                    </Grid>
                    {
                        !matches && images && images.length > 0 &&
                        <Grid item>
                            <div className={classes.mediaMobileWrapper}>
                                <CardMedia
                                    className={classes.media}
                                    image={webApiUrl + 'api/home/GetAttachment/' + encodeURI(images[0].id)}
                                    title={option.description}
                                />
                            </div>
                        </Grid>
                    }

                    <Grid item container justify="flex-end" spacing={1} className={classes.bottomContent}>
                        <Grid item xs>
                            <Typography variant="body1" className={classes.price} noWrap>
                                {
                                    !option.salesPriceEstimated && !option.salesPriceToBeDetermined &&
                                    <React.Fragment>&euro; </React.Fragment>
                                }
                                {option.salesPriceInclVAT_Text}
                                &nbsp;
                                   {
                                    option.unit &&
                                    <React.Fragment>
                                        {t('per')}
                                        &nbsp;
                                    {option.unit}
                                    </React.Fragment>
                                }
                            </Typography>
                        </Grid>
                        <Grid item>
                            <Chip
                                icon={
                                    <Today />
                                }
                                label={formatDate(new Date(option.modifiedOn))}
                                className={classes.chip}
                                variant="outlined"
                                size="small"
                            />
                        </Grid>
                        {
                            option.optionStatus !== 5 &&
                            <Grid item>{statusText}</Grid>
                        }
                        <Grid item>
                            <Chip aria-describedby={popoverId} onClick={handleChipClick} size="small"
                                label={
                                    <React.Fragment>
                                        <NumberFormat
                                            displayType="text"
                                            decimalScale={option.decimalPlaces}
                                            fixedDecimalScale={true}
                                            thousandSeparator="."
                                            decimalSeparator=","
                                            value={option.quantity}
                                            suffix="&nbsp;"
                                        />
                                        <span>{option.unit}&nbsp;&euro;&nbsp;</span>
                                        <NumberFormat
                                            displayType="text"
                                            decimalScale={2}
                                            fixedDecimalScale={true}
                                            thousandSeparator="."
                                            decimalSeparator=","
                                            value={option.quantity * option.salesPriceInclVAT}
                                        />
                                    </React.Fragment>
                                }
                                color="primary" className={classes.bold} />
                            {
                                option.additionalDescription && option.additionalDescription.trim() !== '' &&
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
                                            nl2br(option.additionalDescription)
                                        }
                                    </Typography>
                                </Popover>}
                        </Grid>
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
        </Card>
    );
}
