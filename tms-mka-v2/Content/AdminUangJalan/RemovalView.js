$(document).ready(function () {
    gridSpbuView = $("#gridSolarView").kendoGrid({
        dataSource: {
            data: dsSolarView,
            batch: true,
            schema: {
                model: {
                    fields: {
                        NamaSpbu: { type: "string"},
                        value: { type: "number"},
                    }
                }
            },
            aggregate: [{ field: "value", aggregate: "sum" }],
        },
        resizable: true,
        columns: [
        { field: "NamaSpbu", title: "SPBU" },
        { field: "value", title: "Rp", footerTemplate: "Total : Rp #= sum == null ? 0 : kendo.format('{0:n}', sum) #", template: 'Rp #: value == null ? 0 : kendo.format("{0:n}", value)#', },
        ],
    }).data("kendoGrid");

    gridKapalView = $("#gridKapalView").kendoGrid({
        dataSource: {
            data: dsKapalView,
            batch: true,
            schema: {
                model: {
                    fields: {
                        NamaPenyebrangan: { type: "string" },
                        value: { type: "number" },
                    }
                }
            },
            aggregate: [{ field: "value", aggregate: "sum" }],
        },
        resizable: true,
        columns: [
        { field: "NamaPenyebrangan", title: "Penyebrangan" },
        { field: "value", title: "Rp", footerTemplate: "Total : Rp #= sum == null ? 0 : kendo.format('{0:n}', sum) #", template: 'Rp #: value == null ? 0 : kendo.format("{0:n}", value)#', },
        ],
    }).data("kendoGrid");

    gridUangView = $("#gridUangView").kendoGrid({
        dataSource: {
            data: dsGridUangView,
            batch: true,
            schema: {
                model: {
                    fields: {
                        Nama: { type: "string" , editable:false},
                        value: { type: "number" , editable:false},
                        Tanggal: { type: "Date"}
                    }
                }
            },
            aggregate: [{ field: "value", aggregate: "sum" }],
        },
        resizable: true,
        editable: true,
        columns: [
        { field: "Nama", title: "Keterangan" },
        { field: "value", title: "Rp", footerTemplate: "Total : Rp #= sum == null ? 0 : kendo.format('{0:n}', sum) #", template: 'Rp #: value == null ? 0 : kendo.format("{0:n}", value)#', },
        { field: "Tanggal", title: "Tanggal", editor: TanggalEditor, format: "{0:dd/MM/yyyy}" },
        ],
    }).data("kendoGrid");
})