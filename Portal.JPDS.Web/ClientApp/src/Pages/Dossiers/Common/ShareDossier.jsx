import React, { useEffect, useState } from 'react';
import {
    Button,
    Checkbox,
    CircularProgress,
    Collapse,
    Dialog,
    DialogContent,
    FormControlLabel,
    Grid,
    IconButton,
    List,
    ListItem,
    makeStyles,
    Typography
} from '@material-ui/core';
import { Close, ExpandLess, ExpandMore, Telegram } from '@material-ui/icons';
import RichTextEditor from '../../Chat/RichTextEditor';
import { useTranslation } from 'react-i18next';
import { sendDossierNotification } from '../../../apis/dossiersApi';
import { groupBy } from "../../../_helpers";

const ShareDossier = ({ open, onClose, buildings, buildingId, buildingIds, selectedDossier, isReadOnly, ...props }) => {
    const classes = useStyles();
    const [currentTab, setCurrentTab] = useState(0);
    const [message, setMessage] = useState('');
    const { t } = useTranslation();
    const [usersList, setUsersList] = useState([]);
    const [selectedUsers, setSelectedUsers] = useState();
    const [selectedBuyer, setSelectedBuyer] = useState(false);
    const [availableRoles, setAvailableRoles] = useState();
    const selectedDossierBuilding = selectedDossier.buildingInfoList.find(x => x.buildingId == buildingId);
    const [isSending, setIsSending] = useState(false);
    useEffect(() => {
        if (selectedDossier.userList) {
            const usersByRole = groupBy(selectedDossier.userList, 'roleName');
            const users = Object.keys(usersByRole)
                .map(roleName => ({ roleName, usersList: usersByRole[roleName] }));
            setAvailableRoles(users);
            setSelectedUsers(users.map(p => ({ roleName: p.roleName, users: [] })))
        }
    }, [selectedDossier]);

    useEffect(() => {
        if (selectedUsers) {
            const data = [...new Set(selectedUsers.map(p => p.users.map(u => u.loginId)).flat())];
            setUsersList(data);
        }
    }, [selectedUsers]);

    useEffect(() => {
        setSelectedBuyer(false);
    }, [buildingId, buildingIds]);

    const handleChange = (ind, user) => {
        // const data = Object.assign({}, availableRoles);
        const allUsers = Object.assign([], selectedUsers);
        const index = allUsers[ind].users.findIndex(p => p.id === user.id);
        if (index >= 0) {
            allUsers[ind].users.splice(index, 1);
        } else allUsers[ind].users.push(user);
        setSelectedUsers(allUsers)
    }

    const handleAllChange = (e, index, users) => {
        e.stopPropagation();
        const allUsers = Object.assign([], selectedUsers);
        if (users.length === allUsers[index].users.length) {
            allUsers[index].users = [];
        } else allUsers[index].users = Object.assign([], users);
        setSelectedUsers(allUsers);
    }


    const handleShare = () => {
        const shareWithBuildings = selectedDossierBuilding ? [buildingId] : (buildingIds ? buildingIds : []);
        const shareData = {
            dossierId: selectedDossier.id,
            buildingIds: shareWithBuildings,
            message: message,
            buyerBuildingIds: (selectedDossier.isExternal === true && selectedBuyer) ? shareWithBuildings : [],
            toUserIdList: usersList
        };
        setIsSending(true)
        sendDossierNotification(shareData).then(res => {
            setIsSending(false);
            onClose();
        }).catch(er => {
            setIsSending(false)
        })

    }
    let buyerName = '';

    if (selectedDossierBuilding && selectedDossierBuilding.buyerContactInfo) {
        if (selectedDossierBuilding.buyerContactInfo.type === 0) {
            const { firstName, lastName } = selectedDossierBuilding.buyerContactInfo.p1;
            buyerName = `${firstName} ${lastName}`;
            if (selectedDossierBuilding.buyerContactInfo.p2) {
                const { p2 } = selectedDossierBuilding.buyerContactInfo;
                buyerName += `, ${p2.firstName} ${p2.lastName}`;
            }
        } else {
            const { relationName } = selectedDossierBuilding.buyerContactInfo.org;
            buyerName = relationName;
        }
    }
    else if (buildingIds && buildingIds.length > 0) {
        buyerName = `Delen met kopers (${buildingIds.length})`;
    }

    return (
        <Dialog fullWidth={true} maxWidth={'md'} open={open} onClose={onClose}>
            <DialogContent id="customized-dialog-title" className={classes.titleContainer} onClose={onClose}>
                <Typography variant='h6' className={classes.title}
                    noWrap>{t("dossier.share.title")}:</Typography>
                <IconButton className={classes.closeIcon} component="span" size="small" onClick={onClose}>
                    <Close className={classes.icon} />
                </IconButton>
            </DialogContent>
            <DialogContent className={classes.container}>
                <List component="nav" aria-labelledby="nested-list-subheader" className={classes.root}>
                    <>
                        {
                            selectedDossier.isExternal === true &&
                            (
                                (selectedDossierBuilding && selectedDossierBuilding.buyerContactInfo)
                                ||
                                (buildingIds && buildingIds.length > 0)
                            )
                            &&
                            <ListItem button className={classes.listContainer}>
                                <Grid container spacing={1}>
                                    <Grid item xs={12}>
                                        <FormControlLabel
                                            style={{ margin: 0 }}
                                            disabled={isReadOnly}
                                            control={
                                                <Checkbox
                                                    onChange={e => setSelectedBuyer(e.target.checked)}
                                                    checked={selectedBuyer}
                                                    disabled={isReadOnly}
                                                    color="primary" />
                                            }
                                            label={`${buyerName} (via chat)`}
                                            labelPlacement="end"
                                        />
                                    </Grid>
                                </Grid>
                            </ListItem>
                        }
                        {
                            selectedUsers && availableRoles && availableRoles.map((p, index) => {
                                const role = selectedUsers.find(u => u.roleName === p.roleName);
                                const isInterminate = role.users.map((u) => p.usersList.find((f) => f.id === u.id)).length;
                                const isAllSelected = role.users.map((u) => p.usersList.find((f) => f.id === u.id)).length === p.usersList.length;

                                return (
                                    <React.Fragment key={p.label}>
                                        <ListItem button onClick={(e) => {
                                            e.stopPropagation();
                                            setCurrentTab(index === currentTab ? false : index)
                                        }} className={classes.listContainer}>
                                            <div className={classes.listItemTitle}>
                                                <Checkbox
                                                    disabled={isReadOnly}
                                                    onChange={(e) => handleAllChange(e, index, p.usersList)}
                                                    indeterminate={!isAllSelected && isInterminate}
                                                    checked={isAllSelected} color="primary" />
                                                <div style={{ display: 'flex', alignItems: 'center', flexGrow: 1 }}>
                                                    <Typography variant={'p'}>
                                                        {`(${role.users.length}/${p.usersList.length})`}
                                                    </Typography>
                                                    <Typography variant={'h6'} style={{ marginLeft: 5 }}>{t(p.roleName)}</Typography>
                                                </div>
                                                {currentTab === index ? <ExpandLess /> : <ExpandMore />}
                                            </div>
                                        </ListItem>
                                        <Collapse className={classes.usersContainer} in={currentTab === index} timeout="auto" unmountOnExit>
                                            <Grid container spacing={1}>
                                                {p.usersList.map((u, i) => (
                                                    <Grid item md={3} lg={6}>
                                                        <FormControlLabel
                                                            disabled={isReadOnly}
                                                            control={
                                                                <Checkbox
                                                                    disabled={isReadOnly}
                                                                    onChange={() => handleChange(index, u)}
                                                                    checked={role.users.find(f => f.id === u.id)} color="primary" />
                                                            }
                                                            label={`${u.name} (${u.email})`}
                                                            labelPlacement="end"
                                                        />
                                                    </Grid>
                                                ))}
                                            </Grid>
                                        </Collapse>
                                    </React.Fragment>
                                )
                            })
                        }
                    </>
                </List>
                <Grid container>
                    <Grid item xs={12} className={classes.messageBoxGrid}>
                        <RichTextEditor
                            readOnly={isReadOnly}
                            label={t('Algemeen informatie')}
                            showToolbar={true} value={message}
                            onChange={value => setMessage(value)} />
                    </Grid>
                    <Grid item xs={12} style={{ display: 'flex' }}>
                        <Button disabled={isReadOnly || (usersList.length === 0 && !selectedBuyer) || isSending} onClick={handleShare}
                            variant="contained" color="primary"
                            className={classes.sendButton} endIcon={<Telegram />}>
                            {isSending ? <CircularProgress size={25} /> : t("dossier.share.button.title")}
                        </Button>
                    </Grid>
                </Grid>
            </DialogContent>
        </Dialog>
    );
}

export default ShareDossier;

const useStyles = makeStyles((theme) => ({
    title: {
        flexGrow: 1,
        color: theme.palette.primary.main,
    },
    titleContainer: {
        display: 'flex',
        alignItems: 'center',
    },
    icon: {
        fill: theme.palette.primary.main
    },
    container: {
        padding: theme.spacing(3.15),
        position: 'relative'
    },
    messageBoxGrid: {
        padding: theme.spacing(1.25),
        height: 100,
        border: '1px solid',
        borderColor: theme.palette.grey[300],
        overflow: 'auto'
    },
    grid: {
        marginTop: 15,
        marginBottom: 15,
    },
    closeIcon: {
        marginLeft: 7
    },
    sendButton: {
        marginLeft: 'auto',
        marginTop: 20,
    },
    listContainer: {
        flexWrap: 'wrap',
    },
    listItemTitle: {
        flexGrow: 1,
        width: '100%',
        display: 'flex',
        justifyContent: 'space-between'
    },
    usersContainer: {
        paddingLeft: theme.spacing(5.6),
        flexGrow: 1,
    }
}));
