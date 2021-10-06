export function getCommonArray(arr1, arr2) {
	var result = [];
	for (var i = 0; i < arr1.length; i++) {
		if (arr2.includes(arr1[i])) {
			result.push(arr1[i])
		}
	}
	return result
}

export const groupBy = function (xs, key) {
	return xs.reduce(function (rv, x) {
		(rv[x[key]] = rv[x[key]] || []).push(x);
		return rv;
	}, {});
};

export const compareTwoObjects = (obj1, obj2) => {
	if (obj1 && obj2) {
		let isSame = true;
		for (const key in obj1) {
			if (!isSame) return false;
			if (typeof (obj1[key]) === 'object')
				isSame = isSame && compareTwoObjects(obj1[key], obj2[key]);
			else
				isSame = isSame && obj1[key] === obj2[key];
		}
		return isSame;
	}
	return false
}
