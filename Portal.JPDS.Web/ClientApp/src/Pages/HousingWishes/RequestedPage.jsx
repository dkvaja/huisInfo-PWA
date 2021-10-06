import React from "react";
import { connect } from "react-redux";
import {
    Avatar, Container, Grid, Typography, Divider, List, ListItem,
    ListItemText, AppBar, IconButton, Toolbar,
    Slide, ListItemAvatar, TextField,
    withWidth, Collapse, Badge, Modal, Backdrop, Fade,
    FormControl, InputLabel, Select, MenuItem, Button,
    ListItemSecondaryAction, Input, InputAdornment,
    CircularProgress, CardMedia, ExpansionPanel,
    ExpansionPanelSummary, ExpansionPanelDetails, FormControlLabel,
    Checkbox, Dialog, DialogTitle, DialogContent, DialogContentText, DialogActions, ListItemIcon, Tooltip, Icon
} from "@material-ui/core";
import { withStyles, withTheme } from '@material-ui/core/styles';
import { Schedule, Chat, LibraryBooks, Add, Search, Notifications, ArrowBack, Send, ExpandLess, ExpandMore, NotificationsNone, AssignmentOutlined, AttachFile, DeleteOutline, ChevronRight, Close, InsertDriveFile, CloudDownload, Menu, ArrowDropDown, FormatQuote, Edit, Error, ErrorOutline, Clear } from "@material-ui/icons";
import { history, validateFile, formatFileSize, nl2br, formatDate, authHeader, downloadFile } from '../../_helpers';
import { withTranslation } from 'react-i18next';
import clsx from "clsx";
import { isWidthUp } from "@material-ui/core/withWidth";
import NumberFormat from "react-number-format";
import SelectedOptionCard from "./SelectedOptionCard";

const { webApiUrl } = window.appConfig;

const styles = theme => ({
    grow: {
        flexGrow: 1
    },
    mainContainer: {
        height: '100%',
        paddingTop: theme.spacing(5),
        [theme.breakpoints.down("sm")]: {
            padding: theme.spacing(0)
        }
    },
    container: {
        backgroundColor: theme.palette.background.paper,
        height: '100%',
        position: 'relative',
        overflow: 'hidden'
    },
    slideRight: {
        backgroundColor: theme.palette.background.paper,
        position: 'absolute',
        zIndex: 2,
        height: '100%'
    },
    bold: {
        fontWeight: 'bold'
    },
    fullWidth: {
        width: '100%'
    },
    categoryList: {
        height: 'calc(100% - 48px)',
        overflow: 'auto',
        width: '100%'
    },
    menuIcon: {
        justifyContent: 'center'
    },
    optionsContainer: {
        position: 'absolute',
        zIndex: 1,
        height: '100%',
        right: 0
    },
    optionsTitle: {
        overflow: 'hidden',
        '& p': {
            lineHeight: 1.2
        }
    },
    options: {
        overflow: 'auto',
        flexGrow: 1,
        padding: theme.spacing(3, 3, 0),
        height: 'calc(100% - 48px)',
        '&.requested': {
            height: 'calc(100% - 96px)',
            padding: theme.spacing(0),
            marginBottom: theme.spacing(6)
        }
    },
    fixedBottomAppBar: {
        top: 'auto',
        bottom: 0
    },
    descriptionText: {
        margin: theme.spacing(2, 3)
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
    optionsCategory: {
        color: theme.palette.primary.contrastText,
        backgroundColor: theme.palette.primary.light,
        cursor: 'inherit !important',
        '& .MuiIconButton-root': {
            color: "inherit"
        }
    },
    categoryColumn: {
        flexBasis: '33.33%',
    },
    headerColumn: {
        flexBasis: '50%',
        flexGrow: 1
    },
    optionsDetails: {
        display: 'block',
        padding: 0
    },
    optionsHeading: {
        backgroundColor: theme.palette.grey[100],
        cursor: 'inherit !important',
    },
    expansionPanelDetails: {
        padding: theme.spacing(1, 3)
    },
    stickyAppBar: {
        background: 'none',
        boxShadow: 'none'
    },
    inputBox: {
        color: 'inherit',
        maxWidth: '100%',
        margin: '-3px 0 -5px',
        '&:before': {
            display: 'none'
        },
        '&:after': {
            display: 'none'
        },
        '& svg': {
            color: 'inherit'
        }
    },
});


class Page extends React.Component {
    state = {
        quotations: [],
        selectedQuote: null,
        openLeftMenu: true,
        quoteMenuExpanded: true,
        checkedConfirmation: false,
        documents: [],
        drawings: [],
        filesToUpload: [],
        uploading: false,
        cancelQuoteDialogOpen: false,
        cancelQuoteReason: '',
        cancelQuoteReasonError: false,
        cancelQuoteInPrgoress: false,
        selectRequestedOptions: false,
        categories: [],
        options: [],
        searchTerm: '',
        filteredOptions: []
    };

    componentDidMount() {
        const { location } = this.props;

        this.UpdateQuotations();
        this.UpdateCategories();

        var intervalId = setInterval(this.timer, 10000);
        // store intervalId in the state so it can be accessed later:
        this.setState({ intervalId });
    }

    componentWillUnmount() {
        // use intervalId from the state to clear the interval
        clearInterval(this.state.intervalId);
    }

    componentDidUpdate(prevProps, prevState) {
        if (!prevProps.selected || !this.props.selected || prevProps.selected.buildingId.toUpperCase() !== this.props.selected.buildingId.toUpperCase()) {
            this.UpdateQuotations();
            this.UpdateCategories();
        }
        if (this.state.selectedQuote) {
            if (!prevState.selectedQuote || prevState.selectedQuote.quoteId !== this.state.selectedQuote.quoteId) {
                this.UpdateDocuments();
            }
            if (!this.state.quotations.find(x => x.quoteId === this.state.selectedQuote.quoteId)) {
                this.setState({
                    selectedQuote: null,
                    checkedConfirmation: false,
                    openLeftMenu: false,
                    filesToUpload: [],
                    uploading: false
                });
            }
        }
        if (prevState.searchTerm !== this.state.searchTerm) {
            this.setState({
                filteredOptions: this.getFilteredItems(this.state.options)
            })
        }
    }

    timer = () => {
        this.UpdateQuotations(true);
        this.UpdateCategories(true);
    }

    UpdateQuotations(refresh = false) {
        const { selected } = this.props;
        if (selected) {
            if (refresh === false) {
                this.setState({
                    quotations: [],
                    selectedQuote: null,
                    checkedConfirmation: false,
                    documents: [],
                    drawings: [],
                    filesToUpload: [],
                    uploading: false,
                    cancelQuoteDialogOpen: false,
                    cancelQuoteReason: '',
                    cancelQuoteReasonError: false,
                    cancelQuoteInPrgoress: false
                })
            }

            const url = webApiUrl + 'api/shopping/GetQuotations/' + encodeURI(selected.buildingId);
            if (this.quotationsAbortController && this.quotationsAbortController.signal.aborted !== true) {
                this.quotationsAbortController.abort();
            }

            this.quotationsAbortController = new window.AbortController();

            const requestOptions = {
                method: 'GET',
                headers: authHeader(),
                signal: this.quotationsAbortController.signal
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({ quotations: findResponse });
                }).catch(console.log);
        }
    }

    UpdateDocuments() {
        const { selectedQuote } = this.state;
        if (selectedQuote) {
            this.setState({
                documents: [],
                drawings: []
            })

            const url = webApiUrl + 'api/shopping/GetQuotationDocuments/' + encodeURI(selectedQuote.quoteId);

            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({
                        documents: findResponse.documents,
                        drawings: findResponse.drawings,
                    });
                });
        }
    }

    UpdateCategories(refresh = false) {
        const { selected } = this.props;
        if (selected) {
            const url = webApiUrl + 'api/shopping/GetMyRequestedOptions/' + encodeURI(selected.buildingId);
            if (this.categoriesAbortController && this.categoriesAbortController.signal.aborted !== true) {
                this.categoriesAbortController.abort();
            }

            this.categoriesAbortController = new window.AbortController();

            const requestOptions = {
                method: 'GET',
                headers: authHeader(),
                signal: this.categoriesAbortController.signal
            };

            if (refresh === false) {
                this.setState({
                    categories: [],
                    options: [],
                    searchTerm: ''
                });
            }
            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({
                        categories: findResponse.categories,
                        options: findResponse.options,
                        filteredOptions: this.getFilteredItems(findResponse.options)
                    });
                });
        }
    }

    handleClickOrder = event => {
        const { selected, t, user } = this.props;
        const { selectedQuote, checkedConfirmation, filesToUpload } = this.state;
        if (checkedConfirmation === true && selectedQuote) {
            if (selectedQuote.isDigitalSigning === true) {
                const url = webApiUrl + 'api/shopping/OrderAndSendQuotationForDigitalSigning/' + encodeURI(selectedQuote.quoteId);

                const requestOptions = {
                    method: 'GET',
                    headers: authHeader()
                };

                fetch(url, requestOptions)
                    .then(Response => Response.json())
                    .then(findResponse => {
                        history.push('/order-unsigned')
                    })
                    .catch(error => {
                        alert(t('general.api.error'));
                    });
            }
            else {
                if (filesToUpload.length > 0) {
                    this.setState({ uploading: true })

                    const formData = new FormData()

                    for (var i = 0; i < filesToUpload.length; i++) {
                        formData.append('files', filesToUpload[i])
                    }

                    fetch(webApiUrl + 'api/shopping/OrderQuotationAndUploadSignedDocuments/' + encodeURI(selectedQuote.quoteId), {
                        method: 'POST',
                        headers: authHeader(),
                        body: formData
                    })
                        .then(Response => Response.json())
                        .then(res => {
                            history.push('/order-complete')
                        })
                        .catch(e => {
                            this.setState({ uploading: false });
                            alert(t('general.api.error'));
                        })
                }
            }
        }
        else {
            event.preventDefault();
        }
    }

    handleSubmitCancelQuote = event => {
        const { cancelQuoteReason, selectedQuote } = this.state;
        const { selected, t } = this.props;

        if (selected && selectedQuote && cancelQuoteReason && cancelQuoteReason.trim() !== '') {
            this.setState({ cancelQuoteInPrgoress: true });
            const message = cancelQuoteReason
            const url = webApiUrl + 'api/shopping/CancelQuotation/' + encodeURI(selectedQuote.quoteId);
            const requestOptions = {
                method: 'POST',
                headers: authHeader('application/json'),
                body: JSON.stringify(message)
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(res => {
                    this.UpdateQuotations();
                })
                .catch(e => {
                    alert(t('general.api.error'));
                    this.setState({ cancelQuoteInPrgoress: false });
                });
        }
        else {
            this.setState({ cancelQuoteReasonError: true });
        }
    }

    handleCancelQuoteDialogToggle = event => {
        this.setState({
            cancelQuoteDialogOpen: !this.state.cancelQuoteDialogOpen,
            cancelQuoteReason: '',
            cancelQuoteReasonError: false,
            cancelQuoteInPrgoress: false
        });
    }

    changeCancelReason = event => {
        this.setState({ cancelQuoteReason: event.target.value });
    }

    toggleOpenLeftMenu = () => {
        this.setState({ openLeftMenu: !this.state.openLeftMenu });
    }

    handleSelectQuotation = selectedQuote => {
        if (!this.state.selectedQuote || selectedQuote.quoteId !== this.state.selectedQuote.quoteId) {
            this.setState({
                selectedQuote,
                //Search #FUTURE_FIX in this page to fix related issue
                //#FUTURE_FIX temporarily making checkedConfirmation to true... later make it false when unhide the block.
                checkedConfirmation: true,
                openLeftMenu: false,
                filesToUpload: [],
                uploading: false,
                selectRequestedOptions: false
            });
        }
        else {
            this.setState({ openLeftMenu: false });
        }
    }

    handleSelectRequestedOptions = () => {
        this.setState({
            selectedQuote: null,
            checkedConfirmation: false,
            openLeftMenu: false,
            filesToUpload: [],
            uploading: false,
            selectRequestedOptions: true
        });
    }

    handleChangeConfirmation = () => {
        this.setState({
            checkedConfirmation: !this.state.checkedConfirmation
        })
    }

    handleSelectFiles = e => {
        const files = Array.from(e.target.files);
        let filesToUpload = this.state.filesToUpload.slice();
        for (var i = 0; i < files.length; i++) {
            if (validateFile(files[i]) === true) {
                filesToUpload.push(files[i]);
            }
        }
        this.setState({ filesToUpload });
    }

    handleRemoveFile = index => {
        let filesToUpload = this.state.filesToUpload.slice();
        filesToUpload.splice(index, 1);
        this.setState({ filesToUpload });
    }

    getTotalText() {
        var total = 0;
        if (this.state.selectedQuote) {
            total = this.state.selectedQuote.options.reduce((prev, next) => prev + (next.salesPriceInclVAT * next.quantity), 0);
        }

        return (
            <React.Fragment>
                &euro; {total.toLocaleString('nl-NL', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</React.Fragment>
        );
    }

    getTotalTextRequestedOptions() {
        return (
            <React.Fragment>
                &euro; {this.state.options.reduce((prev, next) => prev + (next.salesPriceInclVAT * next.quantity), 0).toLocaleString('nl-NL', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</React.Fragment>
        );
    }

    getFilteredItems(options) {
        var text = this.state.searchTerm.toUpperCase().split(' ');
        return options.filter(function (item) {
            return text.every(function (el) {
                var content = item.optionNo + ' ' + item.description + ' ' + item.commercialDescription;
                return content.toUpperCase().indexOf(el) > -1;
            });
        });
    }

    containsCategory(catId) {
        return this.state.filteredOptions.filter(x => x.optionCategoryProjectId === catId).length > 0;
    }

    containsHeader(catId, headerId) {
        return this.state.filteredOptions.filter(x => x.optionCategoryProjectId === catId && x.optionHeaderProjectId === headerId).length > 0;
    }

    handleChangeSearch = event => {
        this.setState({
            searchTerm: event.target.value
        })
    }

    clearSearchText = () => {
        this.setState({
            searchTerm: ''
        })
    }

    renderOptionsInBasket(selectedQuote) {
        const { t, classes } = this.props;
        let headerText = '';
        return (
            <React.Fragment>
                {
                    selectedQuote.options.map((option, indexOption) => {
                        let currentHeader = option.category + ' - ' + option.header;
                        if (headerText !== currentHeader) {
                            headerText = currentHeader;
                        }
                        else {
                            currentHeader = '';
                        }
                        return <React.Fragment key={indexOption}>
                            {
                                currentHeader !== '' &&
                                <Grid item xs={12}>
                                    <Typography variant="body2">{currentHeader}</Typography>
                                </Grid>
                            }
                            <Grid container item xs={12} alignItems="flex-start">
                                <Grid item xs={2} md={1}>
                                    <Typography variant="body2">
                                        <NumberFormat
                                            displayType="text"
                                            decimalScale={option.unitDecimalPlaces}
                                            fixedDecimalScale={true}
                                            thousandSeparator="."
                                            decimalSeparator=","
                                            value={option.quantity}
                                            suffix="&nbsp; "
                                        />
                                        {option.eenheid}
                                    </Typography>
                                </Grid>
                                <Grid item xs={7} md={9}>
                                    <Typography variant="body2" component="div" >{option.optionNo + ' - ' + option.description}</Typography>
                                    <Typography variant="body2" component="div" color="textSecondary">{nl2br(option.commercialDescription)}</Typography>
                                    <Typography variant="body2" component="div" color="textSecondary">
                                        <FormatQuote fontSize="small" />
                                        {option.additionalDescription}
                                    </Typography>
                                </Grid>
                                <Grid item container xs={3} md={2} alignItems="center" justify="flex-end">
                                    <Grid item xs={12} sm md={6}>
                                        <Typography variant="body2" component="div" align="right">
                                            {
                                                (!option.salesPriceEstimated && !option.salesPriceToBeDetermined) ?
                                                    <NumberFormat
                                                        prefix="&euro;&nbsp;"
                                                        displayType="text"
                                                        decimalScale={2}
                                                        fixedDecimalScale={true}
                                                        thousandSeparator="."
                                                        decimalSeparator=","
                                                        value={option.quantity * option.salesPriceInclVAT}
                                                    />
                                                    :
                                                    (option.salesPriceInclVAT_Text)
                                            }
                                        </Typography>
                                    </Grid>
                                </Grid>
                            </Grid>
                        </React.Fragment>
                    })
                }
            </React.Fragment>
        )
    }

    renderDocuments(documents) {
        const { classes } = this.props;
        return documents && documents.map((document, index) => (
            <Grid key={index} container item xs={12} alignItems="center" justify="flex-end">
                <Typography className={classes.grow} noWrap>{document.description}</Typography>
                <Grid item>
                    <Typography>{document.datum && formatDate(new Date(document.dateTime))}</Typography>
                </Grid>
                <Grid item>
                    <IconButton href={webApiUrl + 'api/home/GetAttachment/' + encodeURI(document.id)} download>
                        <CloudDownload />
                    </IconButton>
                </Grid>
            </Grid>
        ))
    }

    render() {
        const { user, t, classes, width, selected } = this.props;
        const {
            selectedQuote,
            openLeftMenu,
            quoteMenuExpanded,
            quotations,
            documents,
            drawings,
            filesToUpload,
            uploading,
            cancelQuoteDialogOpen,
            cancelQuoteReason,
            cancelQuoteReasonError,
            cancelQuoteInPrgoress,
            selectRequestedOptions
        } = this.state;
        const matches = isWidthUp('md', width);

        return (
            <Container className={classes.mainContainer}>
                <Grid container className={classes.container}>
                    <Slide direction="right" appear={!matches} in={openLeftMenu || matches} mountOnEnter unmountOnExit>
                        <Grid item xs={12} md={4} container direction="column" className={classes.slideRight}>
                            <AppBar position="static">
                                <Toolbar variant="dense">
                                    {
                                        !matches &&
                                        <IconButton aria-label="GoBack" edge="start" color="inherit" onClick={this.toggleOpenLeftMenu}>
                                            <ArrowBack />
                                        </IconButton>
                                    }
                                    <Typography className={clsx(classes.grow, classes.bold)} noWrap>
                                        {t('Aangevraagd')}
                                    </Typography>
                                    <IconButton aria-label="Search" edge="end" color="inherit">
                                        <Search />
                                    </IconButton>
                                </Toolbar>
                            </AppBar>
                            <List className={classes.categoryList}>
                                <ListItem
                                    title={t('In behandeling')}
                                    button
                                    selected={selectRequestedOptions}
                                    onClick={this.handleSelectRequestedOptions}>
                                    <ListItemText
                                        primary={t('In behandeling')}
                                        primaryTypographyProps={{ noWrap: true }}
                                    />
                                    <ChevronRight color="action" />
                                </ListItem>
                                <Divider component="li" />
                                <ListItem
                                    title={t('Offertes')}
                                    button
                                    //selected={selectedQuote}
                                    onClick={() => { this.setState({ quoteMenuExpanded: !quoteMenuExpanded }) }}>
                                    <ListItemText
                                        primary={t('Offertes')}
                                        primaryTypographyProps={{ noWrap: true }}
                                    />
                                    {quoteMenuExpanded ? <ExpandLess className={classes.impChatListIcon} color="action" /> : <ExpandMore className={classes.impChatListIcon} color="action" />}
                                </ListItem>
                                <Collapse in={quoteMenuExpanded} timeout="auto" unmountOnExit>
                                    {
                                        (quotations.length > 0) ?
                                            <React.Fragment>
                                                {
                                                    quotations.map((quote, index) => (
                                                        <React.Fragment key={index}>
                                                            <Divider component="li" />
                                                            <ListItem
                                                                title={t('Offerte') + ' - ' + quote.quoteNo}
                                                                button
                                                                //className={classes.nested}
                                                                selected={selectedQuote && selectedQuote.quoteId.toUpperCase() === quote.quoteId.toUpperCase()}
                                                                onClick={() => this.handleSelectQuotation(quote)}>
                                                                <ListItemIcon className={classes.menuIcon}>
                                                                    <Tooltip title={t("Ondertekenen")}>
                                                                        <Icon className="fas fa-file-signature" />
                                                                    </Tooltip>
                                                                </ListItemIcon>
                                                                <ListItemText
                                                                    primary={t('Offerte') + ' - ' + quote.quoteNo}
                                                                    primaryTypographyProps={{ noWrap: true }}
                                                                />
                                                                <ChevronRight color="action" />
                                                            </ListItem>
                                                        </React.Fragment>
                                                    ))
                                                }
                                            </React.Fragment>
                                            : <ListItem><ListItemText secondary={t('Geen offertes')} /></ListItem>
                                    }
                                </Collapse>
                            </List>
                        </Grid>
                    </Slide>


                    <Grid item xs={12} md={8} container direction="column" justify="center" className={classes.optionsContainer}>
                        <AppBar position="static">
                            <Toolbar variant="dense">
                                {
                                    !matches &&
                                    <IconButton aria-label="OpenMenu" edge="start" color="inherit" onClick={this.toggleOpenLeftMenu}>
                                        <Menu />
                                    </IconButton>
                                }
                                {
                                    selectedQuote ?
                                        <React.Fragment>
                                            <div className={clsx(classes.grow, classes.optionsTitle)}>
                                                <Typography className={classes.bold} noWrap>{t('Offerte') + ' - ' + selectedQuote.quoteNo}</Typography>
                                            </div>
                                            {
                                                <IconButton aria-label="Zoek" edge="end" color="inherit">
                                                    <Search />
                                                </IconButton>
                                            }
                                        </React.Fragment>
                                        :
                                        selectRequestedOptions === true ?
                                            <React.Fragment>
                                                <Typography className={classes.bold}>{t('Aangevraagde opties')}</Typography>
                                                <div className={classes.headerColumn} align="right">
                                                    <Input
                                                        onFocus={event => event.stopPropagation()}
                                                        className={classes.inputBox}
                                                        type="search"
                                                        value={this.state.searchTerm}
                                                        onChange={this.handleChangeSearch}
                                                        endAdornment={
                                                            <InputAdornment position="end">
                                                                {
                                                                    this.state.searchTerm !== '' ?
                                                                        <IconButton color="inherit" size="small" onClick={this.clearSearchText}>
                                                                            <Close />
                                                                        </IconButton>
                                                                        :
                                                                        <Search />
                                                                }
                                                            </InputAdornment>
                                                        }
                                                    />
                                                </div>
                                            </React.Fragment>
                                            :
                                            <React.Fragment></React.Fragment>
                                }
                            </Toolbar>
                        </AppBar>
                        <Grid container direction="column" className={selectRequestedOptions === true ? clsx(classes.options, 'requested') : classes.options}>
                            {
                                selectedQuote ?
                                    <Grid item container xs={12} direction="row" alignContent="flex-start">
                                        <Grid item xs={12} container spacing={2}>
                                            {
                                                this.renderOptionsInBasket(selectedQuote)
                                            }
                                            <Grid item xs={12}>
                                                <Divider />
                                            </Grid>
                                            <Grid container item xs={12}>
                                                <Grid item xs={12}>
                                                    <Typography variant="body2" className={classes.bold}>{t('Documenten')}</Typography>
                                                </Grid>
                                                <Grid container item xs={12}>
                                                    {
                                                        selectedQuote.isDigitalSigning !== true &&
                                                        this.renderDocuments(documents)
                                                    }
                                                    {
                                                        this.renderDocuments(drawings)
                                                    }
                                                </Grid>
                                            </Grid>
                                            {
                                                selectedQuote.isDigitalSigning !== true &&
                                                <React.Fragment>
                                                    <Grid item xs={12}>
                                                        <Typography variant="body2" color="textSecondary">
                                                            Let op! U kunt de offerte en bijlage(s) downloaden en ondertekenen. Nadat u de offerte en bijlagen hebt ondertekend kunt u ze hier weer uploaden. U kunt pas definitief bestellen als u de bestanden hebt ondertekend en weer hebt geupload.
                                                        </Typography>
                                                    </Grid>
                                                    <Grid container item xs={12}>
                                                        <Grid container item xs={12} alignItems="center">
                                                            <Typography variant="body2" className={clsx(classes.bold, classes.grow)}>{t('Getekende offerte uploaden')}</Typography>
                                                            <input accept="*" style={{ display: 'none' }} id="icon-button-file" type="file" multiple disabled={!selected || user.viewOnly === true} onChange={this.handleSelectFiles} />
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
                                                                            <Add />
                                                                        </IconButton>
                                                                }
                                                            </label>
                                                        </Grid>
                                                        <Grid container item xs={12}>
                                                            {
                                                                <List dense className={classes.grow}>
                                                                    {
                                                                        filesToUpload.map((file, index) => (
                                                                            <ListItem key={index}>
                                                                                <ListItemAvatar>
                                                                                    <Avatar>
                                                                                        <AttachFile />
                                                                                    </Avatar>
                                                                                </ListItemAvatar>
                                                                                <ListItemText
                                                                                    primary={file.name}
                                                                                    secondary={formatFileSize(file.size)}
                                                                                />
                                                                                <ListItemSecondaryAction>
                                                                                    <IconButton edge="end" aria-label="delete" onClick={() => this.handleRemoveFile(index)}>
                                                                                        <Clear />
                                                                                    </IconButton>
                                                                                </ListItemSecondaryAction>
                                                                            </ListItem>
                                                                        ))
                                                                    }
                                                                </List>
                                                            }
                                                        </Grid>
                                                    </Grid>
                                                </React.Fragment>
                                            }
                                            <Grid item xs={12}>
                                                <Divider />
                                            </Grid>
                                            <Grid container item xs={12}>
                                                <Grid item xs>
                                                    <Typography variant="body2" className={classes.bold}>{t('Totaal')}</Typography>
                                                    <br />
                                                </Grid>
                                                <Grid item>
                                                    <Typography variant="body2" className={classes.bold}>{this.getTotalText()}</Typography>
                                                </Grid>
                                            </Grid>

                                        </Grid>
                                        <Grid item xs={12} container>
                                            <Grid item xs={12}>
                                                <Typography variant="body2" component="div" color="textSecondary" paragraph>{
                                                    selectedQuote.isDigitalSigning !== true ?
                                                        t('Door op de knop "Bestellen" te klikken, bevestigt u dat u de bovenstaande opties wilt bestellen.')
                                                        :
                                                        t('Door op de knop "Digitaal ondertekenen" te klikken, bevestigt u dat u de bovenstaande opties wilt bestellen.')
                                                }</Typography>
                                                {
                                                    selectedQuote.isDigitalSigning === true &&
                                                    <Typography variant="body2" color="textSecondary">
                                                        Let op! U ontvangt van ons een email waarna er wordt gevraagd om de offerte <strong>digitaal te ondertekenen.</strong>  Na het digitaal ondertekenen is uw offerte definitief.
                                                    </Typography>
                                                }
                                            </Grid>
                                            {
                                                //Search #FUTURE_FIX in this page to fix related issue
                                                //#FUTURE_FIX temporarily commented.
                                                //<Grid item xs={12}>
                                                //    <FormControlLabel

                                                //        control={
                                                //            <Checkbox
                                                //                checked={this.state.checkedConfirmation}
                                                //                onChange={this.handleChangeConfirmation}
                                                //                value="checkedB"
                                                //                color="primary"
                                                //                size="small"
                                                //                disabled={user.viewOnly === true}
                                                //            />
                                                //        }
                                                //        label={
                                                //            <Typography variant="body2">{t('Ik ga akkoord met de `Algemene voorwaarden`')}</Typography>
                                                //        }
                                                //    />
                                                //</Grid>
                                            }
                                            <Grid item xs={12}>&nbsp;</Grid>
                                            <Grid item xs={12}>
                                                <Button onClick={this.handleClickOrder}
                                                    disabled={user.viewOnly === true}
                                                    variant="contained" color="primary" size="large" fullWidth style={{ fontWeight: "bold" }}>
                                                    {selectedQuote.isDigitalSigning !== true ? t('BESTELLEN') : t('DIGITAAL ONDERTEKENEN')}
                                                </Button>
                                            </Grid>
                                            <Grid item xs={12}>&nbsp;</Grid>
                                            <Grid item xs={12}>
                                                <Button onClick={this.handleCancelQuoteDialogToggle}
                                                    disabled={user.viewOnly === true}
                                                    variant="contained" color="secondary" size="large" fullWidth style={{ fontWeight: "bold" }}>
                                                    {t('OFFERTE ANNULEREN')}
                                                </Button>
                                            </Grid>
                                            <Grid item xs={12}>&nbsp;</Grid>
                                        </Grid>
                                    </Grid>
                                    :
                                    (
                                        selectRequestedOptions === true ?
                                            <React.Fragment>
                                                <div>
                                                    <Typography variant="body2" color="textSecondary" className={classes.descriptionText}>
                                                        {t('Uw aangevraagde opties worden door uw adviseur beoordeeld. Als deze in uw woning passen ontvangt u een offerte.')}
                                                    </Typography>
                                                    {
                                                        this.state.categories.filter(x => this.containsCategory(x.optionCategoryProjectId)).map((category, indexCat) => (
                                                            <ExpansionPanel key={indexCat} className={classes.expansionPanel} expanded={true}>
                                                                <ExpansionPanelSummary className={classes.optionsCategory}>
                                                                    <Typography className={classes.bold}>{category.category}</Typography>
                                                                </ExpansionPanelSummary>
                                                                <ExpansionPanelDetails className={classes.optionsDetails}>
                                                                    {
                                                                        category.headers.filter(x => this.containsHeader(category.optionCategoryProjectId, x.optionHeaderProjectId)).map((header, indexHeader) => (
                                                                            <ExpansionPanel key={indexHeader} ref={header.ref} className={classes.expansionPanel} expanded={true}>
                                                                                <ExpansionPanelSummary className={classes.optionsHeading}>
                                                                                    <Typography className={classes.bold}>{header.header}</Typography>
                                                                                </ExpansionPanelSummary>
                                                                                <ExpansionPanelDetails className={classes.expansionPanelDetails}>
                                                                                    <div className={classes.fullWidth}>
                                                                                        {
                                                                                            this.state.filteredOptions.filter(x => x.optionCategoryProjectId === category.optionCategoryProjectId && x.optionHeaderProjectId === header.optionHeaderProjectId).map((option, indexOption) => (
                                                                                                <SelectedOptionCard key={indexOption} option={option} />
                                                                                            ))
                                                                                        }
                                                                                    </div>
                                                                                </ExpansionPanelDetails>
                                                                            </ExpansionPanel>
                                                                        ))
                                                                    }
                                                                </ExpansionPanelDetails>
                                                            </ExpansionPanel>

                                                        ))
                                                    }
                                                </div>
                                                {
                                                    <AppBar position="absolute" className={classes.fixedBottomAppBar}>
                                                        <Toolbar variant="dense">
                                                            <Typography className={clsx(classes.headerColumn, classes.bold)}>{t('Totaalbedrag')}</Typography>
                                                            <Typography className={classes.bold}>{this.getTotalTextRequestedOptions()}</Typography>
                                                        </Toolbar>
                                                    </AppBar>
                                                }
                                            </React.Fragment>
                                            :
                                            <Grid item container xs={12} spacing={2} alignContent="center" alignItems="center" justify="center">
                                                <Grid item>
                                                    <img src={webApiUrl + "api/Config/WebLogo"} width="300" alt="JPDS" />
                                                    <Typography variant="h4" color="textSecondary" align="center">{t('Kies hier uw offerte')}</Typography>
                                                </Grid>
                                            </Grid>
                                    )
                            }
                        </Grid>
                    </Grid>
                </Grid>
                {
                    selectedQuote &&
                    <Dialog
                        fullWidth
                        maxWidth="sm"
                        open={cancelQuoteDialogOpen}
                        onClose={this.handleCancelQuoteDialogToggle}
                        aria-labelledby="form-dialog-title">
                        <DialogTitle id="form-dialog-title">{t('Offerte annuleren - ') + selectedQuote.quoteNo}</DialogTitle>
                        <DialogContent>
                            <DialogContentText>{t('Vul annuleringsreden in')}</DialogContentText>
                            <TextField
                                error={cancelQuoteReasonError}
                                autoFocus
                                margin="dense"
                                id="message"
                                label="Bericht"
                                required
                                multiline
                                rowsMax="5"
                                fullWidth
                                value={cancelQuoteReason}
                                onChange={this.changeCancelReason}
                                disabled={cancelQuoteInPrgoress === true || user.viewOnly === true}
                            />
                        </DialogContent>
                        <DialogActions>
                            <Button onClick={this.handleCancelQuoteDialogToggle} color="default" disabled={cancelQuoteInPrgoress === true || user.viewOnly === true}>
                                {t('Annuleren')}
                            </Button>
                            <Button onClick={this.handleSubmitCancelQuote} color="primary" disabled={cancelQuoteInPrgoress === true || user.viewOnly === true}>
                                {t('Versturen')}
                            </Button>
                        </DialogActions>
                    </Dialog>
                }
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
        selected,
        buildings
    };
}

const connectedPage = connect(mapStateToProps)(withWidth()(withTranslation()(withStyles(styles)(Page))));
export { connectedPage as RequestedPage };