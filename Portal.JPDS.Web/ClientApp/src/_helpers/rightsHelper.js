
const rights = {
    "dossier.archiveList.read": false,
    "dossier.archiveList.write": false,
    "dossier.draftList.read": false,
    "dossier.draftList.write": false,
    "dossier.generalInformation.read": false,
    "dossier.generalInformation.write": false,
    "dossier.files.read": false,
    "dossier.files.write": false,
    "dossier.canCreateDossier": false,
    "dossier.canSwitchView": false,
    "dossier.canDrag": false,
    "dossier.canShare": false,
    "menu.canShowMenu": false,
    "chat.details.read": false,
    "chat.details.write": false,
    "page.standardOptions.read": false,
    "page.standardOptions.write": false,
    "selected.object.read": false,
    "selected.object.write": false,
    "page.canShowHomePage": false
};

export function getRights(roles, key) {
    if (roles.length) {
        const assignedRights = roles.reduce((p, c) => {
            if (key) {
                switch (c) {
                    case "BuyersGuide": {
                        const updateDossierRight = editRights(key, true, p);
                        return { ...updateDossierRight }
                    }
                    case "Spectator": {
                        const updateDossierRight = editRights(key, false, p);
                        return { ...updateDossierRight }
                    }
                    case "Buyer": {
                        const updateDossierRight = editRights(key, false, p);
                        return { ...updateDossierRight }
                    }
                    case "SiteManager": {
                        const updateDossierRight = editRights(key, false, p);
                        return { ...updateDossierRight }
                    }
                    default: {
                        return {
                            ...p
                        }
                    }
                }
            } else {
                switch (c) {
                    case "BuyersGuide": {
                        const dossierRights = editRights("dossier", true, p);
                        const selectedObjectRights = editRights("object", true, p);
                        const chatRights = editRights("chat", true, p);
                        const menuRights = editRights("menu", true, p);
                        const pageRights = { ...editRights("canShowHomePage", true, p), ...editRights("standardOptions.write", true, p) };
                        return {
                            ...p,
                            ...dossierRights,
                            ...menuRights,
                            ...chatRights,
                            ...selectedObjectRights,
                            ...pageRights
                        };
                    }
                    case "Spectator": {
                        const allReadsRights = editRights("read", true, p);
                        const menuRights = editRights("menu", true, p);
                        const pageRights = {
                            ...editRights("canShowHomePage", true, p),
                            ...editRights("standardOptions.read", true, p)
                        };
                        return {
                            ...p,
                            ...allReadsRights,
                            ...menuRights,
                            ...pageRights
                        };
                    }
                    case "Buyer": {
                        const chatRights = editRights("chat", true, p);
                        const filesRights = editRights("files", true, p);
                        const pageRights = editRights("canShowHomePage", true, p);
                        return { ...p, ...chatRights, ...filesRights, ...pageRights };
                    }
                    case "SiteManager": {
                        const filesRights = editRights("files", true, p);
                        const dossierRights = { ...editRights("read", false, p), ...editRights("canShare", true, p) };
                        const chatRights = editRights("chat", true, p);
                        const pageRights = editRights("canShowHomePage", false, p);
                        return {
                            ...p,
                            ...dossierRights,
                            ...chatRights,
                            ...filesRights,
                            ...pageRights
                        };
                    }
                    default:
                        return {
                            ...p
                        }
                }
            }
        }, key ? { [key]: false } : rights);
        return assignedRights;
    }

}

function editRights(key, enable, availableRights) {
    return Object.keys(availableRights)
        .filter(p => {
            if (key.includes('write') && enable)
                return p.includes('read') || p.includes('write')
            return p.includes(key)
        })
        .reduce((p, c) => ({ ...p, [c]: availableRights[c] || enable }), {});
}