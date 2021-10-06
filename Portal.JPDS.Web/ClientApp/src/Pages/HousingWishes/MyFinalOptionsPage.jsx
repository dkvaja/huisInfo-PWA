import React from "react";
import { connect } from "react-redux";
import {
    Container,
    Grid,
    Typography, ExpansionPanel, ExpansionPanelSummary, ExpansionPanelDetails, AppBar, Input, InputAdornment, IconButton
} from "@material-ui/core";
import { withStyles } from '@material-ui/core/styles';
import { Search, Close } from "@material-ui/icons"
import { withTranslation } from 'react-i18next';
import clsx from "clsx";
import SelectedOptionCard from './SelectedOptionCard';
import { authHeader } from "../../_helpers";

const { webApiUrl } = window.appConfig;

const styles = theme => ({
    heading: {
        color: theme.palette.primary.contrastText,
        backgroundColor: theme.palette.primary.main,
        cursor: 'default !important'
    },
    bold: {
        fontWeight: "bold"
    },
    card: {
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
    fullWidth: {
        width: '100%'
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
        categories: [],
        options: [],
        searchTerm: ''
    };

    componentDidMount() {
        this.UpdateCategories();
    }

    componentDidUpdate(prevProps, prevState) {
        if (!prevProps.selected || prevProps.selected.buildingId !== this.props.selected.buildingId) {
            this.UpdateCategories();
        }
        if (prevState.searchTerm !== this.state.searchTerm) {
            this.setState({
                filteredOptions: this.getFilteredItems(this.state.options)
            })
        }
    }

    UpdateCategories() {
        const { selected } = this.props;
        if (selected) {
            const url = webApiUrl + 'api/shopping/GetMyDefiniteOptions/' + encodeURI(selected.buildingId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            this.setState({
                categories: [],
                options: [],
                searchTerm: ''
            });

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

    getTotalText() {
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

    render() {
        const { t, classes } = this.props;
        return (
                <Container className={classes.mainContainer}>
                    <Grid container>
                        <Grid item xs={12} className={classes.container}>
                            <AppBar position="sticky" className={classes.stickyAppBar}>
                                <ExpansionPanel className={classes.expansionPanel}>
                                    <ExpansionPanelSummary className={classes.heading}>
                                        <Typography className={classes.bold}>{t('Definitieve opties')}</Typography>
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
                                    </ExpansionPanelSummary>
                                </ExpansionPanel>
                            </AppBar>
                        <div>
                            <Typography variant="body2" color="textSecondary" className={classes.descriptionText}>
                                {t('Deze opties zullen in uw woning worden verwerkt.')}
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
                            <AppBar position="static">
                                <ExpansionPanel className={classes.expansionPanel}>
                                    <ExpansionPanelSummary className={classes.heading}>
                                        <Typography className={clsx(classes.headerColumn, classes.bold)}>{t('Totaalbedrag')}</Typography>
                                        <Typography className={classes.bold}>{this.getTotalText()}</Typography>
                                    </ExpansionPanelSummary>
                                </ExpansionPanel>
                            </AppBar>
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
export { connectedPage as MyFinalOptionsPage };