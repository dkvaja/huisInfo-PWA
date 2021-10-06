import React from "react";
import ReactDOM from "react-dom";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import { Avatar, Container, Grid, Typography, Divider, Card, CardContent, CardHeader, List, ListItem, ListItemText, AppBar, IconButton, Toolbar, Slide, Box, ListItemAvatar, TextField, Chip, Hidden, withWidth, Collapse, Badge, Modal, Backdrop, Fade, FormControl, InputLabel, Select, MenuItem, Button, ListItemSecondaryAction, Input, InputAdornment, CircularProgress, CardMedia, ExpansionPanel, ExpansionPanelSummary, ExpansionPanelDetails, FormControlLabel, Checkbox, Tooltip } from "@material-ui/core";
import { withStyles, withTheme } from '@material-ui/core/styles';
import { ToggleButton } from "@material-ui/lab";
import { Schedule, Chat, LibraryBooks, Add, Search, Notifications, ArrowBack, Send, ExpandLess, ExpandMore, NotificationsNone, AssignmentOutlined, AttachFile, DeleteOutline, ChevronRight, Close, InsertDriveFile, CloudDownload, Menu, ArrowDropDown, FormatQuote, Edit, Error, ErrorOutline } from "@material-ui/icons";
import DateFnsUtils from '@date-io/date-fns';
import nlLocale from "date-fns/locale/nl";
import enLocale from "date-fns/locale/en-US";
import {
    MuiPickersUtilsProvider,
    KeyboardTimePicker,
    KeyboardDatePicker,
    KeyboardDateTimePicker,
    DateTimePicker,
} from '@material-ui/pickers';
import { commonActions } from "../../_actions";
import { history, validateFile, nl2br, formatDate, authHeader } from '../../_helpers';
//import './home.css';
import { withTranslation } from 'react-i18next';
import clsx from "clsx";
import { isWidthUp } from "@material-ui/core/withWidth";
import { userAccountTypeConstants } from "../../_constants"
import OptionCard from "./OptionCard";
import NumberFormat from "react-number-format";
import RequestIndividualOption from "./RequestIndividualOption";

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
    standardOptionsContainer: {
        backgroundColor: theme.palette.background.paper,
        height: '100%',
        position: 'relative',
        overflow: 'hidden'
    },
    slideRight: {
        backgroundColor: theme.palette.background.paper,
        position: 'absolute',
        zIndex: 2,
        height: '100%',
        '&.shoppingCartVisible': {
            paddingBottom: 48,
        }
    },
    slideLeft: {
        backgroundColor: theme.palette.background.paper,
        position: 'absolute',
        zIndex: 2,
        right: 0,
        height: '100%',
        '&.shoppingCartVisible': {
            paddingBottom: 48,
        }
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
    categoryHeader: {
        backgroundColor: theme.palette.background.default,
        paddingTop: 0,
        paddingBottom: 0,
        '& .MuiListItemText-root': {
            margin: theme.spacing(0.25, 0)
        }
    },
    categoryHeaderIndividual: {
        backgroundColor: theme.palette.background.default,
    },
    nested: {
        paddingLeft: theme.spacing(4),
    },
    optionsContainer: {
        position: 'absolute',
        zIndex: 1,
        height: '100%',
        right: 0,
        '&.shoppingCartVisible': {
            paddingBottom: 48,
        }
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
        padding: theme.spacing(0, 3),
        height: 'calc(100% - 48px)'
    },
    bottomShoppingCart: {
        position: 'absolute',
        bottom: 0,
        zIndex: 3,
        '&.expanded': {
            height: '100%'
        }
    },
    expansionPanel: {
        width: '100%',
        '&.Mui-expanded': {
            height: '100%',
        },
        '& .MuiExpansionPanelSummary-root.Mui-expanded': {
            minHeight: 48,
        },
        '& .MuiExpansionPanelSummary-content.Mui-expanded': {
            margin: '12px 0'
        },
        '& .MuiCollapse-container.MuiCollapse-entered': {
            maxHeight: 'calc(100% - 48px)',
            overflow: 'auto'
        }
    },
    expansionPanelHeading: {
        color: theme.palette.primary.contrastText,
        backgroundColor: theme.palette.primary.main,
        '& .MuiIconButton-root': {
            color: "inherit"
        }
    },
    expansionPanelContainer: {
        padding: theme.spacing(3),
        '& .MuiExpansionPanelDetails-root': {
            paddingTop: theme.spacing(2)
        },
    },
    totalHeading: {
        backgroundColor: theme.palette.primary.main,
        color: theme.palette.primary.contrastText
    },
    headerColumn: {
        flexBasis: '50%',
        flexGrow: 1
    },
    inputBoxSearch: {
        color: 'inherit',
        maxWidth: '100%',
        margin: '-3px 5px -5px 0',
        '&:before': {
            display: 'none'
        },
        '&:after': {
            display: 'none'
        },
        '& svg': {
            color: 'inherit'
        },
        flexGrow: 1
    },
    searchList: {
        maxHeight: 'calc(100% - 48px)',
        overflowX: 'hidden',
        overflowY: 'auto',
        width: '100%'
    },
    searchListItem: {
        paddingTop: 0,
        paddingBottom: 0
    }
});


class Page extends React.Component {
    state = {
        categories: [],
        options: [],
        selectedOptions: [],
        individualOptions: [],
        showIndividualOptions: false,
        openLeftMenu: true,
        bottomCartExpanded: false,
        openSearchOptions: false,
        searchTerm: '',
        openSearchOptionsCategorized: false,
        searchTermCategorized: ''
    };

    componentDidMount() {
        const { location } = this.props;

        this.UpdateCategories();
        this.UpdateSelectedOptions();

        const bottomCartExpanded = (location && location.state && location.state.bottomCartExpanded === true);

        var intervalId = setInterval(this.timer, 10000);
        // store intervalId in the state so it can be accessed later:
        this.setState({ intervalId: intervalId, bottomCartExpanded });
    }

    componentWillUnmount() {
        // use intervalId from the state to clear the interval
        clearInterval(this.state.intervalId);
    }

    componentDidUpdate(prevProps, prevState) {
        if (!prevProps.selected || !this.props.selected || prevProps.selected.buildingId.toUpperCase() !== this.props.selected.buildingId.toUpperCase()) {
            this.UpdateCategories();
            this.UpdateSelectedOptions();
        }
    }


    timer = () => {
        this.UpdateSelectedOptions();
    }

    UpdateCategories() {
        const { selected } = this.props;
        if (selected) {
            const url = webApiUrl + 'api/shopping/GetStandardOptions/' + encodeURI(selected.buildingId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            this.setState({
                categories: [],
                options: [],
                selectedOptions: [],
                individualOptions: []
            });

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({
                        categories: findResponse.categories,
                        options: findResponse.options.map((option, index) => { return { ...option, index, ref: React.createRef() } })
                    });
                });
        }
    }

    UpdateSelectedOptions() {
        const { selected } = this.props;
        if (selected) {
            const url = webApiUrl + 'api/shopping/GetSelectedOptionsWithIndividualOptions/' + encodeURI(selected.buildingId);

            if (this.abortController && this.abortController.signal.aborted !== true) {
                this.abortController.abort();
            }

            this.abortController = new window.AbortController();

            const requestOptions = {
                method: 'GET',
                headers: authHeader(),
                signal: this.abortController.signal
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.updateSelectedOptionsToState(findResponse);
                })
                .catch(err => {
                    if (err.name === 'AbortError') {
                        //handle aborterror here
                    }
                });
        }
    }

    updateSelectedOptionsToState(response) {
        this.setState({
            selectedOptions: response.filter(x => x.type === 0),
            individualOptions: response.filter(x => x.type === 1).map((option, index) => { return { ...option, index, ref: React.createRef() } })
        });
    }

    toggleOpenLeftMenu = () => {
        this.setState({ openLeftMenu: !this.state.openLeftMenu });
    }

    showIndividualOptions = () => {
        this.setState({
            showIndividualOptions: !this.state.showIndividualOptions,
            selected: null
        });
    }

    handleToggleCategory = categoryId => {
        var categoriesToUpdate = this.state.categories.slice();
        var categoryToUpdate = categoriesToUpdate.find(x => x.optionCategoryProjectId.toUpperCase() === categoryId.toUpperCase());
        if (categoryToUpdate) {
            if (categoryToUpdate.expanded === undefined)
                categoryToUpdate.expanded = !categoryToUpdate.closed;
            categoryToUpdate.expanded = !categoryToUpdate.expanded;
            this.setState({
                categories: categoriesToUpdate
            });
        }
    }

    selectHeader(category, header) {
        const selected = {
            category: category.category,
            closed: category.closed,
            optionCategoryProjectId: category.optionCategoryProjectId,
            closingDate: category.closingDate,
            optionHeaderProjectId: header.optionHeaderProjectId,
            header: header.header,
        };

        this.setState({ selected, openLeftMenu: false, showIndividualOptions: false, bottomCartExpanded: false });
    }

    getSelectedOption(optionStandardId) {
        return this.state.selectedOptions.find(x => x.optionStandardId.toUpperCase() === optionStandardId.toUpperCase());
    }

    handleAddOption(option, quantity, comment) {
        const { user } = this.props;
        if (option.type !== 1) {
            const url = webApiUrl + 'api/shopping/AddToCartStandardOption/';
            const requestOptions = {
                method: 'POST',
                headers: authHeader('application/json'),
                body: JSON.stringify({
                    buildingId: option.buildingId,
                    optionId: option.optionStandardId,
                    quantity: quantity,
                    comment: comment
                })
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.updateSelectedOptionsToState(findResponse);
                });
        }
        else {
            const url = webApiUrl + 'api/shopping/AddToCartIndividualOption';
            const requestOptions = {
                method: 'POST',
                headers: authHeader('application/json'),
                body: JSON.stringify({
                    buildingId: option.buildingId,
                    optionId: option.optionId,
                    comment: comment
                })
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.updateSelectedOptionsToState(findResponse);
                });
        }
    }

    handleGoToOption = option => {
        var optionToScrollRef = option.ref;
        if (option.type !== 1) {
            var categoriesToUpdate = this.state.categories.slice();
            var categoryToUpdate = categoriesToUpdate.find(x => x.optionCategoryProjectId.toUpperCase() === option.optionCategoryProjectId.toUpperCase());
            if (categoryToUpdate) {
                var headerToUpdate = categoryToUpdate.headers.find(x => x.optionHeaderProjectId.toUpperCase() === option.optionHeaderProjectId.toUpperCase());
                if (headerToUpdate) {
                    if (!categoryToUpdate.expanded) {
                        categoryToUpdate.expanded = true;
                        this.setState({
                            categories: categoriesToUpdate
                        });
                    }
                    this.selectHeader(categoryToUpdate, headerToUpdate);

                    var optionToScroll = this.state.options.find(x => x.optionStandardId.toUpperCase() === option.optionStandardId.toUpperCase());
                    if (optionToScroll) {
                        optionToScrollRef = optionToScroll.ref;
                    }
                }
            }
        } else {
            this.showIndividualOptions();
        }

        //timeout to allow open the expansion panel before calculating the actual element top location
        setTimeout(
            () => {
                const { optionsList } = this.refs;
                if (optionsList) {
                    const scrollHeight = optionsList.scrollHeight;
                    const height = optionsList.clientHeight;
                    const maxScrollTop = scrollHeight - height;
                    const scrollTopValue = maxScrollTop > 0 && maxScrollTop > optionToScrollRef.current.offsetTop - 56 ? optionToScrollRef.current.offsetTop - 56 : maxScrollTop
                    ReactDOM.findDOMNode(optionsList).scrollTop = scrollTopValue;
                }
            },
            1000
        );
        if (this.state.bottomCartExpanded === true) {
            this.handleBottomCartExpandToggle();
        }
        if (this.state.openSearchOptions === true) {
            this.toggleOpenSearchOptions();
        }
        if (this.state.openSearchOptionsCategorized === true) {
            this.toggleOpenSearchOptionsCategorized();
        }
    }

    handleDeleteOption = option => {
        const url = webApiUrl + 'api/shopping/DeleteSelectedOption/' + option.optionId;
        const requestOptions = {
            method: 'DELETE',
            headers: authHeader()
        };

        fetch(url, requestOptions)
            .then(Response => Response.json())
            .then(findResponse => {
                var selectedOptions = this.state.selectedOptions.slice();
                var selectedIndividualOptions = this.getSelectedIndividualOptions().filter(x => x.optionId.toUpperCase() !== option.optionId.toUpperCase());
                var updatedOptions = selectedOptions.filter(x => x.optionId.toUpperCase() !== option.optionId.toUpperCase());
                var bottomCartExpanded = this.state.bottomCartExpanded;
                if (bottomCartExpanded === true && updatedOptions.length === 0 && selectedIndividualOptions.length === 0) {
                    bottomCartExpanded = false;
                }
                this.setState({
                    selectedOptions: updatedOptions,
                    bottomCartExpanded: bottomCartExpanded
                })

                this.UpdateSelectedOptions();
            });
    }

    getSelectedIndividualOptions() {
        return this.state.individualOptions.filter(x => x.subStatus === 20);
    }

    getSelectedOptionsByHeading(headerId, categoryId) {
        return this.state.selectedOptions.filter(x => x.optionCategoryProjectId.toUpperCase() === categoryId.toUpperCase() && x.optionHeaderProjectId.toUpperCase() === headerId.toUpperCase());
    }

    getTotalText() {
        const standardOptionsTotal = this.state.selectedOptions.reduce((prev, next) => prev + (next.salesPriceInclVAT * next.quantity), 0);
        const individualOptionsTotal = this.getSelectedIndividualOptions().reduce((prev, next) => prev + (next.salesPriceInclVAT * next.quantity), 0);
        const total = standardOptionsTotal + individualOptionsTotal;
        return (
            <React.Fragment>
                &euro; {total.toLocaleString('nl-NL', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</React.Fragment>
        );
    }

    isShoppingCartVisible() {
        return this.state.selectedOptions.length > 0 || this.getSelectedIndividualOptions().length > 0;
    }

    handleBottomCartExpandToggle = () => {
        this.setState({
            bottomCartExpanded: !this.state.bottomCartExpanded
        })
    }

    handleClickRequest = event => {
        const { selected, t } = this.props;
        if (selected) {
            const url = webApiUrl + 'api/shopping/RequestSelectedOptions/' + encodeURI(selected.buildingId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    history.push('/request-complete')
                })
                .catch(error => {
                    alert(t('general.api.error'));
                });
        }
        else {
            event.preventDefault();
        }
    }

    toggleOpenSearchOptions = () => {
        document.activeElement.blur();
        this.setState({ openSearchOptions: !this.state.openSearchOptions });
    }

    handleChangeSearch = event => {
        this.setState({
            searchTerm: event.target.value
        })
    }

    clearAndCloseSearch = () => {
        this.setState({
            openSearchOptions: false,
            searchTerm: ''
        });
    }

    toggleOpenSearchOptionsCategorized = () => {
        document.activeElement.blur();
        this.setState({ openSearchOptionsCategorized: !this.state.openSearchOptionsCategorized });
    }

    handleChangeCategorizedSearch = event => {
        this.setState({
            searchTermCategorized: event.target.value
        })
    }

    clearAndCloseCategorizedSearch = () => {
        this.setState({
            openSearchOptionsCategorized: false,
            searchTermCategorized: ''
        });
    }

    getFilteredItems(options, searchTerm) {
        var text = searchTerm.toUpperCase().split(' ');
        return options.filter(function (item) {
            return text.every(function (el) {
                var content = item.optionNo + ' ' + item.description + ' ' + item.commercialDescription;
                return content.toUpperCase().indexOf(el) > -1;
            });
        });
    }

    renderOptionInBasket(option, headerText, isClosed) {
        const { t, classes, user } = this.props;
        return (<React.Fragment>
            {
                headerText && headerText !== '' &&
                <Grid item xs={12}>
                    <Typography variant="body2">{headerText}</Typography>
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
                        {option.unit}
                    </Typography>
                </Grid>
                <Grid item xs={7} md={9}>
                    <Typography variant="body2" component="div" >{option.optionNo + ' - ' + option.description}</Typography>
                    <Typography variant="body2" component="div" color="textSecondary">{nl2br(option.commercialDescription)}</Typography>
                    <Typography variant="body2" component="div" color="textSecondary">
                        <FormatQuote fontSize="small" />
                        {option.additionalDescription}
                    </Typography>
                    {
                        isClosed === true &&
                        <Typography variant="body2" component="div" color="error">
                            <ErrorOutline fontSize="small" style={{ float: 'left' }} />
                            {
                                t('De sluitingsdatum voor deze optie is gepasseerd en kan daarom worden geweigerd.')
                            }
                        </Typography>
                    }
                </Grid>
                <Grid item container xs={3} md={2} alignItems="center" justify="flex-end">
                    <Hidden only={"xs"}>
                        <Grid item>
                            <IconButton edge="start" aria-label="edit" size="small" onClick={() => this.handleGoToOption(option)}>
                                <Edit color="primary" fontSize="small" />
                            </IconButton>
                        </Grid>
                    </Hidden>
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
                    <Hidden only={["sm", "md", "lg", "xl"]}>
                        <Grid item>
                            <IconButton edge="start" aria-label="edit" size="small" onClick={() => this.handleGoToOption(option)}>
                                <Edit color="primary" fontSize="small" />
                            </IconButton>
                            &nbsp;&nbsp;
                                                                                </Grid>
                    </Hidden>
                    <Grid item>
                        <IconButton edge="end" aria-label="delete" size="small" disabled={user.viewOnly === true} onClick={() => this.handleDeleteOption(option)}>
                            <DeleteOutline color="primary" fontSize="small" />
                        </IconButton>
                    </Grid>
                </Grid>
            </Grid>
        </React.Fragment>
        )
    }

    render() {
        const { user, t, classes, width } = this.props;
        const {
            selected,
            openLeftMenu,
            categories,
            options,
            individualOptions,
            showIndividualOptions,
            openSearchOptions,
            searchTerm,
            openSearchOptionsCategorized,
            searchTermCategorized
        } = this.state;

        const matches = isWidthUp('md', width);

        const optionsBySelectedCategory = selected ?
            options.filter(x => x.optionCategoryProjectId.toUpperCase() === selected.optionCategoryProjectId.toUpperCase() && x.optionHeaderProjectId.toUpperCase() === selected.optionHeaderProjectId.toUpperCase())
            :
            [];

        let filteredOptions = [];
        if (searchTerm && searchTerm.trim() !== '') {
            filteredOptions = this.getFilteredItems(options, searchTerm);
        }

        let filteredCategorizedOptions = [];
        if (searchTermCategorized && searchTermCategorized.trim() !== '') {
            filteredCategorizedOptions = this.getFilteredItems(optionsBySelectedCategory, searchTermCategorized);
        }

        return (
            <Container className={classes.mainContainer}>
                <Grid container className={classes.standardOptionsContainer}>
                    <Slide direction="right" appear={!matches} in={openLeftMenu || matches} mountOnEnter unmountOnExit>
                        <Grid item xs={12} md={4} container direction="column" className={this.isShoppingCartVisible() ? clsx(classes.slideRight, 'shoppingCartVisible') : classes.slideRight}>
                            <AppBar position="static">
                                <Toolbar variant="dense">
                                    {
                                        !matches && selected &&
                                        <IconButton aria-label="GoBack" edge="start" color="inherit" onClick={this.toggleOpenLeftMenu}>
                                            <ArrowBack />
                                        </IconButton>
                                    }
                                    <Typography className={clsx(classes.grow, classes.bold)} noWrap>
                                        {t('Opties')}
                                    </Typography>
                                    <RequestIndividualOption selectedBuilding={this.props.selected} user={user} />
                                    <Tooltip title={t("Zoek")}>
                                        <IconButton aria-label="Zoek" edge="end" color="inherit" onClick={this.toggleOpenSearchOptions}>
                                            <Search />
                                        </IconButton>
                                    </Tooltip>
                                </Toolbar>
                            </AppBar>
                            <List className={classes.categoryList}>
                                {
                                    (categories && categories.length > 0) || (individualOptions && individualOptions.length > 0) ?
                                        <React.Fragment>
                                            {
                                                categories.map((category, index) => (
                                                    <React.Fragment key={index}>
                                                        {index !== 0 && <Divider component="li" />}
                                                        <ListItem className={classes.categoryHeader} button onClick={() => this.handleToggleCategory(category.optionCategoryProjectId)}>
                                                            <ListItemText
                                                                primary={category.category}
                                                                primaryTypographyProps={{ noWrap: true }}
                                                                secondary={
                                                                    category.closed === true
                                                                        ?
                                                                        t('Sluitingsdatum gepasseerd')
                                                                        :
                                                                        (
                                                                            category.closingDate
                                                                                ?
                                                                                t('Sluitingsdatum') + ': ' + formatDate(new Date(category.closingDate))
                                                                                :
                                                                                t('Sluitingsdatum') + ': ' + t('N.t.b.')
                                                                        )
                                                                }
                                                                secondaryTypographyProps={{ noWrap: true }}
                                                            />
                                                            {(category.expanded === undefined ? !category.closed : category.expanded) ? <ExpandLess color="action" /> : <ExpandMore color="action" />}
                                                        </ListItem>

                                                        <Collapse in={category.expanded === undefined ? !category.closed : category.expanded} timeout="auto" unmountOnExit>
                                                            <List component="div" disablePadding>
                                                                {
                                                                    category.headers.map((header, indexHeader) => (
                                                                        <React.Fragment key={indexHeader}>
                                                                            <Divider component="li" />
                                                                            <ListItem
                                                                                title={header.header}
                                                                                button
                                                                                className={classes.nested}
                                                                                selected={selected && selected.optionCategoryProjectId === category.optionCategoryProjectId && selected.optionHeaderProjectId === header.optionHeaderProjectId}
                                                                                onClick={() => this.selectHeader(category, header)}>
                                                                                <ListItemText
                                                                                    primary={header.header}
                                                                                    primaryTypographyProps={{ noWrap: true }}
                                                                                />
                                                                                <ChevronRight color="action" />
                                                                            </ListItem>
                                                                        </React.Fragment>
                                                                    ))
                                                                }
                                                            </List>
                                                        </Collapse>
                                                    </React.Fragment>
                                                ))
                                            }
                                            {
                                                (individualOptions && individualOptions.length > 0) &&
                                                <React.Fragment>
                                                    <Divider component="li" />
                                                    <ListItem className={classes.categoryHeaderIndividual} button selected={showIndividualOptions} onClick={() => this.showIndividualOptions()}>
                                                        <ListItemText
                                                            primary={t('Individuele opties')}
                                                            primaryTypographyProps={{ noWrap: true }}
                                                        />
                                                        <ChevronRight color="action" />
                                                    </ListItem>
                                                </React.Fragment>
                                            }
                                        </React.Fragment>
                                        : <ListItem><ListItemText secondary={t('Geen opties')} /></ListItem>
                                }
                            </List>
                        </Grid>
                    </Slide>

                    <Slide direction="right" in={openSearchOptions} mountOnEnter unmountOnExit>
                        <Grid item xs={12} md={4} container direction="column" className={this.isShoppingCartVisible() ? clsx(classes.slideRight, 'shoppingCartVisible') : classes.slideRight}>
                            <AppBar position="static">
                                <Toolbar variant="dense">
                                    <Input
                                        color="inherit"
                                        className={classes.inputBoxSearch}
                                        type="search"
                                        value={searchTerm}
                                        onChange={this.handleChangeSearch}
                                        endAdornment={
                                            <InputAdornment position="end">
                                                <Search />
                                            </InputAdornment>
                                        }
                                    />
                                    <IconButton aria-label="Close" edge="end" color="inherit" onClick={this.clearAndCloseSearch}>
                                        <Close />
                                    </IconButton>
                                </Toolbar>
                            </AppBar>

                            <List className={classes.searchList} ref="searchList">
                                {
                                    filteredOptions && filteredOptions.length > 0 ?
                                        filteredOptions.map((option, indexOption) => (
                                            <React.Fragment key={indexOption}>
                                                <ListItem button className={classes.searchListItem} onClick={() => this.handleGoToOption(option)}>
                                                    <ListItemText
                                                        primary={option.optionNo + ' - ' + option.description}
                                                        primaryTypographyProps={{ noWrap: true, component: "p" }}
                                                        secondary={nl2br(option.commercialDescription)}
                                                        secondaryTypographyProps={{ noWrap: true }}
                                                    />
                                                    <ChevronRight color="action" />
                                                </ListItem>
                                                <Divider component="li" />
                                            </React.Fragment>
                                        ))
                                        :
                                        <ListItem><ListItemText secondary={t('Geen opties')} /></ListItem>
                                }
                            </List>
                        </Grid>
                    </Slide>

                    <Grid item xs={12} md={8} container direction="column" justify="center" className={this.isShoppingCartVisible() ? clsx(classes.optionsContainer, 'shoppingCartVisible') : classes.optionsContainer}>
                        <AppBar position="static">
                            <Toolbar variant="dense">
                                {
                                    !matches &&
                                    <IconButton aria-label="OpenMenu" edge="start" color="inherit" onClick={this.toggleOpenLeftMenu}>
                                        <Menu />
                                    </IconButton>
                                }
                                {
                                    (selected || showIndividualOptions) &&
                                    <React.Fragment>
                                        <div className={clsx(classes.grow, classes.optionsTitle)}>
                                            {
                                                selected &&
                                                <React.Fragment>
                                                    <Typography className={classes.bold} noWrap>
                                                        {selected.category}
                                                    </Typography>
                                                    <Typography component="p" variant="caption" className={classes.grow} noWrap>
                                                        {selected.header}
                                                    </Typography>
                                                </React.Fragment>
                                            }
                                            {
                                                showIndividualOptions &&
                                                <Typography className={classes.bold} noWrap>{t('Individuele opties')}</Typography>
                                            }
                                        </div>
                                        {
                                            <Tooltip title={t("Zoek")}>
                                                <IconButton aria-label="Zoek" edge="end" color="inherit" onClick={this.toggleOpenSearchOptionsCategorized}>
                                                    <Search />
                                                </IconButton>
                                            </Tooltip>
                                        }
                                    </React.Fragment>
                                }
                            </Toolbar>
                        </AppBar>
                        <Grid container direction="column" className={classes.options} ref="optionsList">
                            {
                                selected ?
                                    <Grid item xs={12}>
                                        {
                                            optionsBySelectedCategory.map((option, indexOption) => (
                                                <OptionCard key={option.index}
                                                    option={option}
                                                    user={user}
                                                    selected={this.getSelectedOption(option.optionStandardId)}
                                                    closed={selected.closed === true || !selected.closingDate}
                                                    unit={option.unit}
                                                    decimalPlaces={option.unitDecimalPlaces}
                                                    onAddOption={(quantity, comment) => this.handleAddOption(option, quantity, comment)}
                                                />
                                            ))
                                        }
                                    </Grid>
                                    :
                                    (
                                        showIndividualOptions ?
                                            <Grid item xs={12}>
                                                {
                                                    individualOptions.map((option, indexOption) => (
                                                        <OptionCard
                                                            key={indexOption}
                                                            option={option}
                                                            user={user}
                                                            selected={option.subStatus === 20 ? option : null}
                                                            unit={option.unit}
                                                            decimalPlaces={option.unitDecimalPlaces}
                                                            onAddOption={(quantity, comment) => this.handleAddOption(option, option.quantity, comment)} />
                                                    ))
                                                }
                                            </Grid>
                                            :
                                            <Grid item container xs={12} spacing={2} alignContent="center" alignItems="center" justify="center">
                                                <Grid item>
                                                    <img src={webApiUrl + "api/Config/WebLogo"} width="300" alt="JPDS" />
                                                    <Typography variant="h4" color="textSecondary" align="center">{t('Kies hier uw opties')}</Typography>
                                                </Grid>
                                            </Grid>
                                    )
                            }
                        </Grid>
                        {
                            //<AppBar position="static">
                            //    <Toolbar variant="dense">
                            //        <div className={classes.grow}></div>
                            //        <Typography className={classes.bold} align="right">{t('Totaalbedrag')}: {this.getTotalText()}</Typography>
                            //    </Toolbar>
                            //</AppBar>
                        }
                    </Grid>


                    <Slide direction="left" in={openSearchOptionsCategorized} mountOnEnter unmountOnExit>
                        <Grid item xs={12} md={4} container direction="column" className={this.isShoppingCartVisible() ? clsx(classes.slideLeft, 'shoppingCartVisible') : classes.slideLeft}>
                            <AppBar position="static">
                                <Toolbar variant="dense">
                                    <Input
                                        color="inherit"
                                        className={classes.inputBoxSearch}
                                        type="search"
                                        value={searchTermCategorized}
                                        onChange={this.handleChangeCategorizedSearch}
                                        endAdornment={
                                            <InputAdornment position="end">
                                                <Search />
                                            </InputAdornment>
                                        }
                                    />
                                    <IconButton aria-label="Close" edge="end" color="inherit" onClick={this.clearAndCloseCategorizedSearch}>
                                        <Close />
                                    </IconButton>
                                </Toolbar>
                            </AppBar>

                            <List className={classes.searchList} ref="searchList">
                                {
                                    filteredCategorizedOptions && filteredCategorizedOptions.length > 0 ?
                                        filteredCategorizedOptions.map((option, indexOption) => (
                                            <React.Fragment key={indexOption}>
                                                <ListItem button className={classes.searchListItem} onClick={() => this.handleGoToOption(option)}>
                                                    <ListItemText
                                                        primary={option.optionNo + ' - ' + option.description}
                                                        primaryTypographyProps={{ noWrap: true, component: "p" }}
                                                        secondary={nl2br(option.commercialDescription)}
                                                        secondaryTypographyProps={{ noWrap: true }}
                                                    />
                                                </ListItem>
                                                <Divider component="li" />
                                            </React.Fragment>
                                        ))
                                        :
                                        <ListItem><ListItemText secondary={t('Geen opties')} /></ListItem>
                                }
                            </List>
                        </Grid>
                    </Slide>


                    {this.isShoppingCartVisible() &&
                        <Grid item xs={12} className={this.state.bottomCartExpanded === true ? clsx(classes.bottomShoppingCart, 'expanded') : classes.bottomShoppingCart}>

                            <ExpansionPanel className={classes.expansionPanel} onChange={this.handleBottomCartExpandToggle} expanded={this.state.bottomCartExpanded === true}>
                                <ExpansionPanelSummary
                                    expandIcon={<ArrowDropDown />}
                                    aria-controls="panel3a-content"
                                    id="panel3a-header" className={classes.expansionPanelHeading}
                                >
                                    <Box display={{ xs: 'none', sm: 'initial' }}>
                                        <Typography className={classes.bold}>{t('Nog aan te vragen opties')}</Typography>
                                    </Box>
                                    <Typography className={clsx(classes.headerColumn, classes.bold)} align="right">{t('Totaalbedrag')}: {this.getTotalText()}</Typography>
                                </ExpansionPanelSummary>
                                <ExpansionPanelDetails className={classes.expansionPanelContainer}>
                                    <br />
                                    <div className={classes.fullWidth}>
                                        <Grid container direction="row" justify="flex-start">
                                            <Grid item xs={12} container spacing={2}>
                                                {
                                                    this.state.categories.map((category, indexCat) => (
                                                        category.headers.map((header, indexHeader) => (
                                                            this.getSelectedOptionsByHeading(header.optionHeaderProjectId, category.optionCategoryProjectId).map((option, indexOption) => (
                                                                <React.Fragment key={indexOption}>
                                                                    {
                                                                        this.renderOptionInBasket(option, indexOption === 0 ? category.category + ' - ' + header.header : '', category.closed)
                                                                    }
                                                                </React.Fragment>
                                                            ))
                                                        ))
                                                    ))
                                                }
                                                {
                                                    this.getSelectedIndividualOptions().map(
                                                        (option, indexOption) =>
                                                            (
                                                                <React.Fragment key={indexOption}>
                                                                    {
                                                                        this.renderOptionInBasket(option, indexOption === 0 ? t('Individuele opties') : '', option.closed)
                                                                    }
                                                                </React.Fragment>
                                                            )
                                                    )
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
                                                    <Typography variant="body2" component="div" color="textSecondary">{t('Door op de knop "Aanvragen" te klikken, bevestigt u dat u de bovenstaande opties wilt aanvragen.')}</Typography>
                                                    <Typography variant="body2" color="textSecondary">{t('Hierna wordt door uw koperbegeleider beoordeeld of alle gewenste opties mogelijk zijn. Als de opties mogelijk zijn ontvangt u van ons een offerte. U ontvangt een bericht als de offerte voor u klaar staat.')}</Typography>
                                                    <Typography variant="body2" color="textSecondary" paragraph>{t('Als u hier een individuele optie aanvraagt is deze al beoordeeld en ontvangt u van ons na uw aanvraag de definitieve offerte.')}</Typography>
                                                </Grid>
                                                <Grid item xs={12}>
                                                    <Button onClick={this.handleClickRequest} disabled={user.viewOnly === true} variant="contained" color="primary" size="large" fullWidth style={{ fontWeight: "bold" }}>
                                                        {t('AANVRAGEN')}
                                                    </Button>
                                                </Grid>
                                                <Grid item xs={12}>&nbsp;</Grid>
                                            </Grid>
                                        </Grid>
                                    </div>
                                </ExpansionPanelDetails>
                            </ExpansionPanel>
                        </Grid>
                    }
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
        selected,
        buildings
    };
}

const connectedPage = connect(mapStateToProps)(withWidth()(withTranslation()(withStyles(styles)(Page))));
export { connectedPage as StandardOptionsPage };