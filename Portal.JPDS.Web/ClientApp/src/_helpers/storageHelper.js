export function getStorageImagesForStandardOption(standardOptionId) {
    var images = JSON.parse(sessionStorage.getItem('SO_' + standardOptionId));
    return !!images ? images : [];
}

export function setStorageImagesForStandardOption(standardOptionId, listImages) {
    sessionStorage.setItem('SO_' + standardOptionId, JSON.stringify(listImages));
}

export function getStorageImagesForIndividualOption(individualOptionId) {
    var images = JSON.parse(sessionStorage.getItem('IO_' + individualOptionId));
    return !!images ? images : [];
}

export function setStorageImagesForIndividualOption(individualOptionId, listImages) {
    sessionStorage.setItem('IO_' + individualOptionId, JSON.stringify(listImages));
}