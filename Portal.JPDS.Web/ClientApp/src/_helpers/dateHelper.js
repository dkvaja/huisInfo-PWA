export function formatDate(date, withTime) {
    var dd = String(date.getDate()).padStart(2, '0');
    var mm = String(date.getMonth() + 1).padStart(2, '0');
    var yyyy = date.getFullYear();
    var time = '';
    if (withTime)
        time = " " + String(date.getHours()).padStart(2, '0') + ":" + String(date.getMinutes()).padStart(2, '0');
    return dd + '-' + mm + '-' + yyyy + time;
}

export function formatTime(obj) {
    var isDate = obj instanceof Date;
    var hours = 0, minutes = 0;
    if (isDate == true) {
        hours = obj.getHours();
        minutes = obj.getMinutes();
    }
    else if (!!obj) {
        hours = obj.hours;
        minutes = obj.minutes;
    }
    var hh = String(hours).padStart(2, '0');
    var mm = String(minutes).padStart(2, '0');

    return hh + ':' + mm;
}

export function getWeekName(date) {
    return date.toLocaleDateString('nl-NL', { weekday: 'long' });
}

export function getDateText(date) {
    //const formatDate = new Intl.DateTimeFormat('nl-NL', { day: '2-digit', month: '2-digit', year: 'numeric' });
    //const formatTime = new Intl.DateTimeFormat('nl-NL', { hour: '2-digit', minute: '2-digit' });
    //const formatDay = new Intl.DateTimeFormat('nl-NL', { weekday: 'long' });
    var now = new Date().getTime();
    var dateTime = date.getTime();
    var daysDiff = Math.round((now - dateTime) / (1000 * 3600 * 24));
    switch (daysDiff) {
        case 0: { return formatTime(date); }
        case 1:
        case 2:
        case 3:
        case 4:
        case 5:
        case 6: { return getWeekName(date); }
        default: {
            return formatDate(date);
        }
    }
}