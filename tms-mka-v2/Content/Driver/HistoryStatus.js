var dsStat;

$(document).ready(function () {
    dsStat = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Driver/BindingStatHistory?id=' + $("#Id").val(),
                dataType: "json"
            },
        },
        schema: {
            model: {
                fields: {
                    Tanggal: { type: "string" },
                    Status: { type: "string" },
                    keterangan: { type: "string" },
                }
            }
        },
    });

    $("#gridStat ").kendoGrid({
        dataSource: dsStat,
        columns: [
            {
                field: "Tanggal",
                title: "Tanggal",
                template: "#= kendo.toString(kendo.parseDate(Tanggal, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
            },
            {
                field: "Status",
                title: "Status"
            },
            {
                field: "keterangan",
                title: "keterangan"
            },
        ],
    }).data("kendoGrid");
});