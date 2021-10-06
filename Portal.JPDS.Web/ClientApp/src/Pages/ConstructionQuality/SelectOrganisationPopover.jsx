import React, { useEffect, useState } from "react";
import { Collapse, FormControl, Grid, IconButton, Input, InputAdornment, InputLabel, makeStyles, MenuItem, MenuList, Select, TextField, Typography } from "@material-ui/core";
import { Close, FilterList, Search } from "@material-ui/icons";
import { Popover } from "mui-datatables";
import { useTranslation } from "react-i18next";
import { authHeader } from "../../_helpers";
import { Autocomplete } from "@material-ui/lab";

const { webApiUrl } = window.appConfig;

const useStyles = makeStyles(theme => ({
    grow: {
        flexGrow: 1
    }
}));

export default function SelectOrganisationPopover(props) {
    const { t } = useTranslation()
    const classes = useStyles();
    const { projectId, productServices, disabled, selectOrgAnchorEl, setSelectOrgAnchorEl, onSelect, setLoading } = props;
    const [searchTerm, setSearchTerm] = useState(null);
    const [searchResolversResult, setSearchResolversResult] = useState([]);
    const [selectOrgMethod, setSelectOrgMethod] = useState(0);
    const [selectOrgProductService, setSelectOrgProductService] = useState(null);
    const [showEditFilterAnchor, setShowEditFilterAnchor] = useState(false);
    const [isMounted,setIsMounted]=useState(false);

    const GetResolversList = () => {
        if(!isMounted){
            setLoading && setLoading(true)
            setIsMounted(true)
        }
        const url = webApiUrl + 'api/Organisation/GetOrganisations/' + encodeURI(projectId) + '?'
            + 'methodName=' + selectOrgMethod
            + (!!selectOrgProductService && selectOrgProductService.id.trim() !== '' ? ('&productServiceId=' + selectOrgProductService.id) : '')
            + (!!searchTerm ? ('&searchText=' + encodeURI(searchTerm)) : '')

        const requestOptions = {
            method: 'GET',
            headers: authHeader()
        };

        fetch(url, requestOptions)
            .then(Response => Response.json())
            .then(findResponse => {
                setSearchResolversResult(findResponse);
                setLoading && setLoading(false)
            }).catch((err) => {
                setLoading && setLoading(false)
            });
    }

    useEffect(() => {
        GetResolversList();
    }, [selectOrgMethod, selectOrgProductService, searchTerm]);

    return (
        <div style={{ padding: '16px' }}>
            <Grid container direction="column">
                <Grid item>
                    <Grid container direction="row">
                        <Grid item className={classes.grow}>
                            {
                                searchTerm == null
                                    ?
                                    <Typography variant="h6">{selectOrgMethod >= 0 && t('Oplosser')}</Typography>
                                    :
                                    <Input
                                        onFocus={event => event.stopPropagation()}
                                        autoFocus
                                        type="search"
                                        value={searchTerm}
                                        onChange={e => setSearchTerm(e.target.value)}
                                        endAdornment={
                                            searchTerm !== '' &&
                                            <InputAdornment position="end">
                                                <IconButton color="inherit" size="small" onClick={() => setSearchTerm(null)}>
                                                    <Close />
                                                </IconButton>
                                            </InputAdornment>
                                        }
                                    />
                            }
                        </Grid>
                        <Grid item>
                            <IconButton
                                color="inherit"
                                aria-label="search"
                                component="span"
                                style={{ marginTop: -8, marginBottom: -8 }}
                                onClick={e => setSearchTerm(searchTerm == null ? '' : null)}
                            >
                                <Search />
                            </IconButton>
                        </Grid>
                        <Grid item>
                            <IconButton
                                aria-describedby={'filter-list'}
                                color="inherit"
                                aria-label="filter-resolvers"
                                component="span"
                                style={{ marginTop: -8, marginBottom: -8 }}
                                onClick={e => setShowEditFilterAnchor(!showEditFilterAnchor)}
                            >
                                <FilterList />
                            </IconButton>
                        </Grid>
                        <Grid item>
                            <IconButton
                                color="inherit"
                                component="span"
                                edge="end"
                                style={{ marginTop: -8, marginBottom: -8 }}
                                onClick={() => setSelectOrgAnchorEl(null)}
                            >
                                <Close />
                            </IconButton>
                        </Grid>
                    </Grid>
                </Grid>
                <Grid item>
                    <Collapse in={showEditFilterAnchor}>
                        <Grid container spacing={1}>
                            <Grid item xs={12} sm={6}>
                                <FormControl
                                    variant="outlined"
                                    margin="dense"
                                    fullWidth
                                    disabled={disabled}
                                //style={{ marginTop: -10, marginBottom: -10 }}
                                >
                                    <InputLabel id="change-method-select-label">{t('Methode')}</InputLabel>
                                    <Select
                                        labelId="change-method-select-label"
                                        id="change-method-select"
                                        value={selectOrgMethod}
                                        onChange={e => setSelectOrgMethod(e.target.value)}
                                        label={t('Methode')}
                                    >
                                        {
                                            [0, 1, 2].map((item, index) =>
                                                <MenuItem key={index} value={item}>{t('workorder.organisation.search.method.' + item)}</MenuItem>
                                            )
                                        }
                                    </Select>
                                </FormControl>
                            </Grid>
                            <Grid item xs={12} sm={6}>
                                <Autocomplete
                                    id="product-service-method-select-addworkorder"
                                    fullWidth
                                    disabled={disabled}
                                    options={productServices && productServices}
                                    value={selectOrgProductService}
                                    onChange={(e, v) => setSelectOrgProductService(v)}
                                    getOptionLabel={(option) => option.code + ' - ' + option.description}
                                    renderInput={(params) => <TextField  {...params} label={t('Product/Dienst')} variant="outlined" margin="dense" />}
                                />
                            </Grid>
                        </Grid>
                    </Collapse>
                </Grid>
                <Grid item>
                    <MenuList style={{ margin: '0 -16px -12px' }}>
                        {
                            !!searchResolversResult &&
                            searchResolversResult.map((item, index) =>
                                <MenuItem key={index} onClick={() => onSelect(item)}>{item.name}</MenuItem>
                            )
                        }
                    </MenuList>
                </Grid>
            </Grid>
        </div>
    )
}