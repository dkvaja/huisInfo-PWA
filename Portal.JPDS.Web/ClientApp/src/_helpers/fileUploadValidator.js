import { fileUploadConfig } from './fileUploadConfig';

export function validateFile(file, imagesOnly = false) {
    var allowedFormats = imagesOnly === true ? fileUploadConfig.allowedImageFormats : fileUploadConfig.allowedMimeTypes;
    if (!allowedFormats.find(x => x.mime === file.type.toLowerCase())) {
        alert('Dit bestands-type is niet ondersteund');
        return false;
    }
    else if (file.size > fileUploadConfig.maxFileSizeInMB * 1024 * 1024) {
        alert('Bestanden groter dan ' + fileUploadConfig.maxFileSizeInMB + 'MB zijn niet toegestaan.');
        return false;
    }
    else {
        return true;
    }
}