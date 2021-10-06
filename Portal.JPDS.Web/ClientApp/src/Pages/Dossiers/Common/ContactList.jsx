import { makeStyles } from '@material-ui/core';
import { Grid, Typography } from '@material-ui/core'
import React, { useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next'
import ContactCard from "../DossierContactCard";

export default function ContactList({ loggedInUserFromRights, selectedDossierContacts, buyerContactInfo, ...props }) {
    const { t } = useTranslation();
    const classes = useStyles();
    const [contacts, setContacts] = useState([]);
    useEffect(() => {
        if (loggedInUserFromRights) {
            setContacts(selectedDossierContacts.filter(p =>
                (loggedInUserFromRights.isInternal && p.isInternal && p.isInternalVisible)
                || (loggedInUserFromRights.isExternal && p.isExternal && p.isExternalVisible)
            ));
        }
    }, [loggedInUserFromRights, selectedDossierContacts]);
    return (
        <Grid container spacing={1} className={classes.container}>
            <Grid item xs={12}>
                <Typography className={classes.header}>{t('Contactgegevens') + ':'}</Typography>
            </Grid>
            {contacts && contacts.map((contact, index) =>
                contact.userContactInfo &&
                <Grid key={index} item xs={12}>
                    <ContactCard
                        isVisible={contact.isExternalVisible}
                        object={contact.userContactInfo}
                        isOrg
                        subTitle={`${contact.userContactInfo.relationName} ${contact.userContactInfo.relationFunctionName ? `(${contact.userContactInfo.relationFunctionName})` : ''}`} />
                </Grid>
            )}
            {buyerContactInfo &&
                <>
                    <Grid item xs={12}>
                        <ContactCard
                            isVisible={buyerContactInfo.isExternalVisible}
                            isOrg={buyerContactInfo.type === 1}
                            object={buyerContactInfo.type === 0 ? buyerContactInfo.p1 : buyerContactInfo.org}
                            subTitle={buyerContactInfo.type === 0 ? "Koper 1" : 'Koper organisatie'} />
                    </Grid>
                    {
                        buyerContactInfo.type === 0 && buyerContactInfo.p2 && <Grid item xs={12}>
                            <ContactCard
                                isVisible={buyerContactInfo.isExternalVisible}
                                object={buyerContactInfo.p2}
                                subTitle={"Koper 2"} />
                        </Grid>
                    }
                </>}
        </Grid>
    )
}

const useStyles = makeStyles((theme) => ({
    container: {
        [theme.breakpoints.down('sm')]: {
            padding: theme.spacing(2),
        }
    },
    header: {
        padding: theme.spacing(1.5, 0, 1.5, 0),
        [theme.breakpoints.down('sm')]: {
            fontSize: '1rem',
            fontWeight: 'bold'
        }
    }
}));