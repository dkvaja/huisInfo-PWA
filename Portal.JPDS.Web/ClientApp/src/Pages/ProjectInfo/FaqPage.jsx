import React from "react";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import {
    Container,
    Grid,
    Typography, ExpansionPanel, ExpansionPanelSummary, AppBar, TextField, Button, Toolbar, Table, TableBody, TableRow, TableCell, Avatar, ExpansionPanelDetails, Input, InputAdornment, IconButton
} from "@material-ui/core";
import { withStyles, withTheme } from '@material-ui/core/styles';
import { Search, Person, ArrowDropDown, Close } from "@material-ui/icons";
import { nl2br, authHeader } from "../../_helpers";
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
    qaHeading: {
        color: theme.palette.primary.contrastText,
        backgroundColor: theme.palette.primary.light,
        '& .MuiIconButton-root': {
            color: "inherit"
        }
    },
    qaHeaderDetails: {
        display: 'block',
        padding: 0
    },
    qaQuestion: {
        backgroundColor: theme.palette.grey[100],
    },
    expansionPanelDetails: {
        padding: theme.spacing(1, 3)
    },
    button: {
        '&:hover': {
            color: theme.palette.primary.contrastText
        }
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
        headers: [],
        filteredHeaders: [],
        searchTerm: ''
    };

    componentDidMount() {
        this.UpdateQuestions();
    }

    componentDidUpdate(prevProps, prevState) {
        if (!prevProps.selected || !this.props.selected || prevProps.selected.projectId.toUpperCase() !== this.props.selected.projectId.toUpperCase()) {
            this.UpdateQuestions();
        }
        if (prevState.searchTerm !== this.state.searchTerm) {
            this.setState({
                filteredHeaders: this.getFilteredItems(this.state.headers)
            })
        }
    }

    UpdateQuestions() {
        const { selected } = this.props;
        if (selected) {
            const url = webApiUrl + 'api/home/GetFAQ/' + encodeURI(selected.projectId);
            const requestOptions = {
                method: 'GET',
                headers: authHeader()
            };

            this.setState({
                headers: [],
                filteredHeaders: [],
                searchTerm: ''
            });

            fetch(url, requestOptions)
                .then(Response => Response.json())
                .then(findResponse => {
                    this.setState({
                        headers: findResponse,
                        filteredHeaders: this.getFilteredItems(findResponse)
                    });
                });
        }
    }

    getFilteredItems(headers) {
        var text = this.state.searchTerm.toUpperCase().split(' ');
        var resultHeaders = headers.slice();
        for (var i = 0; i < resultHeaders.length; i++) {
            resultHeaders[i].faqs = resultHeaders[i].faqs.filter(function (item) {
                return text.every(function (el) {
                    var content = item.ques + ' ' + item.ans;
                    return content.toUpperCase().indexOf(el) > -1;
                })
            }).slice();
        }

        return resultHeaders;
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
        const { filteredHeaders } = this.state;
        return (
            <Container className={classes.mainContainer}>
                <Grid container>
                    <Grid item container xs={12} className={classes.container}>
                        <AppBar position="sticky">
                            <Toolbar variant="dense">
                                <Typography className={clsx(classes.grow, classes.bold)} noWrap>{t('Veel gestelde vragen')}</Typography>
                                <div align="right">
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
                            </Toolbar>
                        </AppBar>
                        <div style={{ width: '100%' }}>
                            {
                                filteredHeaders.map((header, indexHeader) => (
                                    header.faqs.length > 0 &&
                                    <ExpansionPanel key={indexHeader} className={classes.expansionPanel} defaultExpanded={true}>
                                        <ExpansionPanelSummary
                                            expandIcon={<ArrowDropDown />}
                                            aria-controls={'panel-cat-' + indexHeader + '-content'}
                                            id={'panel-cat-' + indexHeader + '-header'} className={classes.qaHeading}
                                        >
                                            <Typography className={classes.bold}>{header.header}</Typography>
                                        </ExpansionPanelSummary>
                                        <ExpansionPanelDetails className={classes.qaHeaderDetails}>
                                            {
                                                header.faqs.map((faq, index) => (
                                                    <ExpansionPanel key={index} className={classes.expansionPanel}>
                                                        <ExpansionPanelSummary
                                                            expandIcon={<ArrowDropDown />}
                                                            aria-controls={'panel-cat-' + indexHeader + '-head-' + index + '-content'}
                                                            id={'panel-cat-' + indexHeader + '-head-' + index + '-header'} className={classes.qaQuestion}
                                                        >
                                                            <Typography className={classes.bold}>{faq.ques}</Typography>
                                                        </ExpansionPanelSummary>
                                                        <ExpansionPanelDetails className={classes.expansionPanelDetails}>
                                                            <Typography>{nl2br(faq.ans)}</Typography>
                                                        </ExpansionPanelDetails>
                                                    </ExpansionPanel>
                                                ))
                                            }
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
export { connectedPage as FaqPage };