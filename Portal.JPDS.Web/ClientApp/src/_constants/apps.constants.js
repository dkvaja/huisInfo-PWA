export const apps = {
    huisinfo: 0,
    aftercare: 1,
    constructionQuality: 3,
    resolverModule: 4
};

let info = [
    {
        appId: apps.huisinfo,
        nameKey: 'app.name.huisinfo',
        isDifferentNameForBuyer: true,
        icon: process.env.PUBLIC_URL + '/Content/Images/Logos/Apps/country-house.png',
        path: '/home'
    },
    {
        appId: apps.aftercare,
        nameKey: 'app.name.aftercare',
        isDifferentNameForBuyer: false,
        icon: process.env.PUBLIC_URL + '/Content/Images/Logos/Apps/reparation-tools.png',
        path: '/nazorg'
    },
    {
        appId: apps.constructionQuality,
        nameKey: 'app.name.constructionquality',
        isDifferentNameForBuyer: false,
        icon: process.env.PUBLIC_URL + '/Content/Images/Logos/Apps/reparation-tools.png',
        path: '/kwaliteitsborging'
    },
    {
        appId: apps.resolverModule,
        nameKey: 'app.name.resolverModule',
        isDifferentNameForBuyer: false,
        icon: process.env.PUBLIC_URL + '/Content/Images/Logos/Apps/reparation-tools.png',
        path: '/werkbonnen'
    }
];

export function getLinkToHome(app) {
    var appinfo = info.find(x => x.appId === app);
    if (appinfo)
        return appinfo.path;

    return '/';
}

export const appsInfo = info;