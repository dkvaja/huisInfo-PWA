import React, { useImperativeHandle, useState } from "react";
import { Avatar, Badge, Button, Card, CardContent, CardHeader, Checkbox, Collapse, Divider, FormControlLabel, Grid, Icon, IconButton, makeStyles, Tooltip } from "@material-ui/core";
import { useTranslation } from "react-i18next";
import { getNameInitials } from "../../_helpers";
import { Business, Email, ExpandLess, ExpandMore, Home, Phone, Smartphone } from "@material-ui/icons";

const { webApiUrl } = window.appConfig;

const useStyles = makeStyles(theme => ({
    contactCard: {
        '& .MuiCardHeader-avatar': {
            '& .MuiBadge-badge': {
                '& .MuiAvatar-root': {
                    height: theme.spacing(2),
                    width: theme.spacing(2),
                    backgroundColor: theme.palette.success.main
                }
            }
        },
        '& .MuiCardHeader-content': {
            overflow: 'hidden'
        },
        '& .MuiCardHeader-action': {
            alignSelf: 'center',
            margin: theme.spacing(-.5, -1, -.5, 0)
        },
        '& .MuiCardContent-root': {
            paddingTop: 0
        }
    },
    infoGridRow: {
        '& > div': {
            padding: theme.spacing(0.5, 2),
        },
        '&:hover': {
            backgroundColor: theme.palette.action.hover
        },
        '& .MuiInputLabel-outlined': {
            whiteSpace: 'nowrap',
            maxWidth: '100%',
            overflow: 'hidden',
            textOverflow: 'ellipsis'
        }
    },
}));

export default function RepairRequestContactCard(props) {
    const { object, isOrg, subTitle, pointOfContactType, selectedPointOfContactType, updatePointOfContact, hidePointOfContactCheckbox } = props;
    const { t } = useTranslation()
    const classes = useStyles();
    const [expanded, setExpanded] = useState(false);
    const userInitials = getNameInitials(object.name);
    const photoUrl = !isOrg
        ?
        webApiUrl + "api/home/GetPersonPhoto/" + object.personId
        :
        webApiUrl + "api/organisation/GetOrganisationLogo/" + object.organisatonId;
    const relationNameInitials = isOrg === true ? getNameInitials(object.relationName) : '';
    const selectedPointOfContact = pointOfContactType === selectedPointOfContactType;

    const pointOfContactRender = (
        <FormControlLabel
            control={
                <Checkbox
                    //onChange={handleChange}
                    onChange={e => { if (e.target.checked === true) updatePointOfContact(pointOfContactType); }}
                    name={subTitle}
                    color="primary"
                />
            }
            label="Aanspreekpunt"
        />
    );

    return (
        <Card className={classes.contactCard}>
            <CardHeader
                avatar={
                    <Badge
                        overlap="circle"
                        anchorOrigin={{
                            vertical: 'bottom',
                            horizontal: 'right',
                        }}
                        badgeContent={
                            selectedPointOfContact && <Avatar />
                        }
                    >
                        <Avatar alt={userInitials} src={photoUrl}>{userInitials}</Avatar>
                    </Badge>}
                action={
                    <Tooltip title={expanded === true ? t('Inklappen') : t('Uitklappen')}>
                        <IconButton aria-label="settings" onClick={() => setExpanded(!expanded)}>
                            {
                                expanded === true ?
                                    <ExpandLess />
                                    :
                                    <ExpandMore />
                            }
                        </IconButton>
                    </Tooltip>
                }
                title={object.name}
                titleTypographyProps={{ title: object.name, noWrap: true }}
                subheader={subTitle}
                subheaderTypographyProps={{ title: subTitle, noWrap: true }}
            />
            <Collapse in={expanded} timeout="auto" unmountOnExit>
                {
                    isOrg !== true ?
                        <CardContent>
                            <Grid container>
                                {
                                    object.telephonePrivate &&
                                    <>
                                        <Grid item xs={12}>
                                            <Grid container className={classes.infoGridRow}>
                                                <Grid item xs={12}>
                                                    <Home />&nbsp;<Button color="primary" href={'tel:' + object.telephonePrivate}>{object.telephonePrivate}&nbsp;<Phone /></Button>
                                                </Grid>
                                            </Grid>
                                        </Grid>
                                        <Grid item xs={12} >
                                            <Divider />
                                        </Grid>
                                    </>
                                }
                                {
                                    object.mobile &&
                                    <>
                                        <Grid item xs={12}>
                                            <Grid container className={classes.infoGridRow}>
                                                <Grid item xs={12}>
                                                    <Smartphone />&nbsp;<Button color="primary" href={'tel:' + object.mobile}>{object.mobile}&nbsp;<Phone /></Button>
                                                </Grid>
                                            </Grid>
                                        </Grid>
                                        <Grid item xs={12} >
                                            <Divider />
                                        </Grid>
                                    </>
                                }
                                {
                                    object.telephoneWork &&
                                    <>
                                        <Grid item xs={12}>
                                            <Grid container className={classes.infoGridRow}>
                                                <Grid item xs={12}>
                                                    <Business />&nbsp;<Button color="primary" href={'tel:' + object.telephoneWork}>{object.telephoneWork}&nbsp;<Phone /></Button>
                                                </Grid>
                                            </Grid>
                                        </Grid>
                                        <Grid item xs={12} >
                                            <Divider />
                                        </Grid>
                                    </>
                                }
                                {
                                    object.emailPrivate &&
                                    <>
                                        <Grid item xs={12}>
                                            <Grid container className={classes.infoGridRow}>
                                                <Grid item xs={12}>
                                                    <Home />&nbsp;<Button color="primary" href={'mailto:' + object.emailPrivate}>{object.emailPrivate}&nbsp;<Email /></Button>
                                                </Grid>
                                            </Grid>
                                        </Grid>
                                        <Grid item xs={12} >
                                            <Divider />
                                        </Grid>
                                    </>
                                }
                                {
                                    object.emailWork &&
                                    <>
                                        <Grid item xs={12}>
                                            <Grid container className={classes.infoGridRow}>
                                                <Grid item xs={12}>
                                                    <Business />&nbsp;<Button color="primary" href={'mailto:' + object.emailWork}>{object.emailWork}&nbsp;<Email /></Button>
                                                </Grid>
                                            </Grid>
                                        </Grid>
                                        <Grid item xs={12} >
                                            <Divider />
                                        </Grid>
                                    </>
                                }
                                {
                                    selectedPointOfContact !== true && hidePointOfContactCheckbox !== true &&
                                    <>
                                        <Grid item xs={12}>
                                            <Grid container className={classes.infoGridRow}>
                                                <Grid item xs={12}>
                                                    {pointOfContactRender}
                                                </Grid>
                                            </Grid>
                                        </Grid>
                                        <Grid item xs={12} >
                                            <Divider />
                                        </Grid>
                                    </>
                                }
                            </Grid>
                        </CardContent>
                        :
                        <>
                            <CardContent>
                                <Grid container>
                                    {
                                        object.telephone &&

                                        <>
                                            <Grid item xs={12}>
                                                <Grid container className={classes.infoGridRow}>
                                                    <Grid item xs={12}>
                                                        <Business />&nbsp;<Button color="primary" href={'tel:' + object.telephone}>{object.telephone}&nbsp;<Phone /></Button>
                                                    </Grid>
                                                </Grid>
                                            </Grid>
                                            <Grid item xs={12} >
                                                <Divider />
                                            </Grid>
                                        </>
                                    }
                                    {
                                        object.email &&
                                        <>
                                            <Grid item xs={12}>
                                                <Grid container className={classes.infoGridRow}>
                                                    <Grid item xs={12}>
                                                        <Business />&nbsp;<Button color="primary" href={'mailto:' + object.email}>{object.email}&nbsp;<Email /></Button>
                                                    </Grid>
                                                </Grid>
                                            </Grid>
                                            <Grid item xs={12} >
                                                <Divider />
                                            </Grid>
                                        </>
                                    }
                                    {
                                        !object.relationName && selectedPointOfContact !== true && hidePointOfContactCheckbox !== true &&
                                        <>
                                            <Grid item xs={12}>
                                                <Grid container className={classes.infoGridRow}>
                                                    <Grid item xs={12}>
                                                        {pointOfContactRender}
                                                    </Grid>
                                                </Grid>
                                            </Grid>
                                            <Grid item xs={12} >
                                                <Divider />
                                            </Grid>
                                        </>
                                    }
                                </Grid>
                            </CardContent>
                            {
                                object.relationName &&
                                <>
                                    <CardHeader
                                        avatar={<Avatar alt={relationNameInitials} src={webApiUrl + "api/home/GetPersonPhoto/" + object.relationPersonId}>{relationNameInitials}</Avatar>}
                                        action={
                                            object.relationPersonSex === 0 ?
                                                <Tooltip title={t('Man')}>
                                                    <Icon color="action" className="fas fa-male" />
                                                </Tooltip>
                                                :
                                                <Tooltip title={t('Vrouw')}>
                                                    <Icon color="action" className="fas fa-female" />
                                                </Tooltip>
                                        }
                                        title={object.relationName}
                                        titleTypographyProps={{ title: object.name, noWrap: true }}
                                        subheader={object.relationFunctionName}
                                        subheaderTypographyProps={{ title: subTitle, noWrap: true }}
                                    />
                                    <CardContent>
                                        <Grid container>
                                            {
                                                object.relationTelephone &&
                                                <>
                                                    <Grid item xs={12}>
                                                        <Grid container className={classes.infoGridRow}>
                                                            <Grid item xs={12}>
                                                                <Business />&nbsp;<Button color="primary" href={'tel:' + object.relationTelephone}>{object.relationTelephone}&nbsp;<Phone /></Button>
                                                            </Grid>
                                                        </Grid>
                                                    </Grid>
                                                    <Grid item xs={12} >
                                                        <Divider />
                                                    </Grid>
                                                </>
                                            }
                                            {
                                                object.relationMobile &&
                                                <>
                                                    <Grid item xs={12}>
                                                        <Grid container className={classes.infoGridRow}>
                                                            <Grid item xs={12}>
                                                                <Smartphone />&nbsp;<Button color="primary" href={'tel:' + object.relationMobile}>{object.relationMobile}&nbsp;<Phone /></Button>
                                                            </Grid>
                                                        </Grid>
                                                    </Grid>
                                                    <Grid item xs={12} >
                                                        <Divider />
                                                    </Grid>
                                                </>
                                            }
                                            {
                                                object.relationEmail &&
                                                <>
                                                    <Grid item xs={12}>
                                                        <Grid container className={classes.infoGridRow}>
                                                            <Grid item xs={12}>
                                                                <Business />&nbsp;<Button color="primary" href={'mailto:' + object.relationEmail}>{object.relationEmail}&nbsp;<Email /></Button>
                                                            </Grid>
                                                        </Grid>
                                                    </Grid>
                                                    <Grid item xs={12} >
                                                        <Divider />
                                                    </Grid>
                                                </>
                                            }
                                            {
                                                selectedPointOfContact !== true && hidePointOfContactCheckbox !== true &&
                                                <>
                                                    <Grid item xs={12}>
                                                        <Grid container className={classes.infoGridRow}>
                                                            <Grid item xs={12}>
                                                                {pointOfContactRender}
                                                            </Grid>
                                                        </Grid>
                                                    </Grid>
                                                    <Grid item xs={12} >
                                                        <Divider />
                                                    </Grid>
                                                </>
                                            }
                                        </Grid>
                                    </CardContent>
                                </>
                            }
                        </>
                }
            </Collapse>
        </Card>
    );
}