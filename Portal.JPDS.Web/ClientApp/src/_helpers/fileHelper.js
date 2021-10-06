import axios from "axios";
import { fileUploadConfig } from "./fileUploadConfig";

export function formatFileSize(bytes, decimalPoint) {
	if (bytes == 0) return '0 Bytes';
	var k = 1000,
		dm = decimalPoint || 2,
		sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'],
		i = Math.floor(Math.log(bytes) / Math.log(k));
	return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
}

export function totalFilesSizeIsValid(files, validMB) {
	const totalFilesByteSize = files.reduce((p, c) => p + c.size, 0);
	const converted = +(totalFilesByteSize / (1024 * 1024)).toFixed(2);
	return converted < validMB;
}

export const isValidFiles = files => files.every(p => fileUploadConfig.allowedMimeTypes.find(f => f.mime === p.type));

export const toBase64 = file => new Promise((resolve, reject) => {
	const reader = new FileReader();
	reader.readAsDataURL(file);
	reader.onload = () => resolve(reader.result);
	reader.onerror = error => reject(error);
});

//Mime types. Some keys are duplicated.
export function getMimeTypefromString(extension) {
	let mimeType = fileUploadConfig.allowedImageFormats.find(p => p.extension === extension);
	if (!mimeType) mimeType = fileUploadConfig.allowedMimeTypes.find(p => p.extension === extension);
	if (!mimeType) return 'application/octet-stream';
	return mimeType.mime;
}


export const downloadFile = async (url, download) => {
	const file = await axios.get(url, { responseType: 'blob' });
	if (file) {
		const { data } = file;
		const url = window.URL.createObjectURL(data);
		let link = document.createElement("a");
		link.href = url;
		link.download = download;
		document.body.appendChild(link).click();
		setTimeout(() => window.URL.revokeObjectURL(url), 200);
	}
}