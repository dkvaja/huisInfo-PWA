import React from 'react'
import { useTranslation } from 'react-i18next'
import { Select, Menu, MenuItem, SvgIcon } from '@material-ui/core'

const LanguageSelector = (props) => {
    const [anchorEl, setAnchorEl] = React.useState(null);
    const { t, i18n } = useTranslation();
    const { language } = i18n; const
        languages = [
        { culture: 'nl-NL', name: 'Dutch' },
        { culture: 'en', name: 'English' }];

    
    const changeLanguage = lang => {
        if (lang !== language)
            i18n.changeLanguage(lang);
        handleClose();
        //handle event to be triggered from calling component
        props.handleClose();
    }

    function handleClick(event) {
        setAnchorEl(event.currentTarget);
    }

    function handleClose() {
        setAnchorEl(null);
    }

    function getFlagIconSrc(lang) {
        return '/Content/Images/Flags/' + lang + '.svg';
    }

    return (
        <React.Fragment>
            <MenuItem aria-controls="simple-menu" aria-haspopup="true" onClick={handleClick}>
                <img src={getFlagIconSrc(language)} alt={language} width="24" />
                &nbsp;
                <span>{t('layout.navbar.language')}</span></MenuItem>
            <Menu
                id="simple-menu"
                anchorEl={anchorEl}
                keepMounted
                open={Boolean(anchorEl)}
                onClose={handleClose}
            >
                {
                    languages.map((lang, index) => (
                        <MenuItem key={`lang` + index} onClick={()=>changeLanguage(lang.culture)}><img src={getFlagIconSrc(lang.culture)} alt={lang.name} width="24" /></MenuItem>
                    ))
                }
            </Menu>
        </React.Fragment>
    )
}

export default LanguageSelector