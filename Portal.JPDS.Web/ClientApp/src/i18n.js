import i18n from 'i18next'
import Backend from 'i18next-xhr-backend'
import { initReactI18next } from 'react-i18next'
import packageJson from '../package.json';

i18n
    .use(Backend)
    .use(initReactI18next)
    .init({
        lng: 'nl-NL',
        backend: {
            /* translation file path */
            loadPath: '/Content/{{ns}}/{{lng}}.json?v=' + packageJson.version
        },
        fallbackLng: 'nl-NL',
        debug: false,
        /* can have multiple namespace, in case you want to divide a huge translation into smaller pieces and load them on demand */
        ns: ['Translations'],
        defaultNS: 'Translations',
        keySeparator: false,
        interpolation: {
            escapeValue: false,
            formatSeparator: ','
        },
        react: {
            wait: true
        }
    })

export default i18n