var dsStat;

$(document).ready(function () {
    dsHisTf = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Driver/BindingTransfer?id=' + $("#Id").val(),
                dataType: "json"
            },
        },
        schema: {
            model: {
                fields: {
                    JnsSo : { type: "string" },
                    NoSo : { type: "string" },
                    NoPol : { type: "string" },
                    JnsTruck : { type: "string" },
                    Customer : { type: "string" },
                    Rute : { type: "string" },
                    JnsBarang : { type: "string" },
                    TargetSuhu : { type: "number" },
                    Jumtf : { type: "number" },
                    DateTf : { type: "datetime" },
                    KetTf: { type: "string" },
                }
            }
        },
    });

    $("#gridTrans").kendoGrid({
        dataSource: dsHisTf,
        columns: [
            { field: "JnsSo", title: "Jenis Order" },
            { field: "NoSo", title: "No. SO" },
            { field: "NoPol", title: "Vehichle No" },
            { field: "JnsTruck", title: "Jenis Truck" },
            { field: "Customer", title: "Customer" },
            { field: "Rute", title: "Rute" },
            { field: "JnsBarang", title: "Jenis Barang" },
            { field: "TargetSuhu", title: "Target Suhu" },
            { field: "KetTf", title: "Keterangan Transfer" },
        ],
    }).data("kendoGrid");
});