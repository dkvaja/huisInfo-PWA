import React from "react";
import { mdToDraftjs } from 'draftjs-md-converter';
import { convertFromRaw } from "draft-js";

export const nl2br = text =>
    text && text
        .split(/(?:\r\n|\r|\n)/g).map((item, key) => {
            return <React.Fragment key={key}>{item}<br /></React.Fragment>
        });


export const md2plaintext = text => text && convertFromRaw(mdToDraftjs(text)).getPlainText(' ');

export function getDataTableTextLabels(t, isLoading = false) {
    return {
        body: {
            noMatch: isLoading ? t("datatable.label.body.loading") : t("datatable.label.body.nomatch"),
            toolTip: t("datatable.label.body.tooltip"),
            columnHeaderTooltip: column => t("datatable.label.body.columnheadertooltip").replace("@[column.label]", column.label)
        },
        pagination: {
            next: t("datatable.label.pagination.next"),
            previous: t("datatable.label.pagination.previous"),
            rowsPerPage: t("datatable.label.pagination.rowsPerPage"),
            displayRows: t("datatable.label.pagination.displayRows"),
        },
        toolbar: {
            search: t("datatable.label.toolbar.search"),
            downloadCsv: t("datatable.label.toolbar.downloadCsv"),
            print: t("datatable.label.toolbar.print"),
            viewColumns: t("datatable.label.toolbar.viewcolumns"),
            filterTable: t("datatable.label.toolbar.filterTable"),
        },
        filter: {
            all: t("datatable.label.filter.all"),
            title: t("datatable.label.filter.title"),
            reset: t("datatable.label.filter.reset"),
        },
        viewColumns: {
            title: t("datatable.label.viewcolumns.title"),
            titleAria: t("datatable.label.viewcolumns.titlearia"),
        },
        selectedRows: {
            text: t("datatable.label.selectedrows.text"),
            delete: t("datatable.label.selectedrows.delete"),
            deleteAria: t("datatable.label.selectedrows.deletearia"),
        },
    }
};


export const getNameInitials = name => {
    const initials = (name && name.match(/\b\w/g)) || [];
    const nameInitials = ((initials.shift() || '') + (initials.pop() || '')).toUpperCase();
    return nameInitials;
}