var dsStat;

$(document).ready(function () {
    dsKlaim = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Driver/BindingKlaim?id=' + $("#Id").val(),
                dataType: "json"
            },
        },
        schema: {
            model: {
                fields: {
                    JnsOrder: { type: "string" },
                    NoSo: { type: "string" },
                    NoPol: { type: "string" },
                    JnsTruck: { type: "string" },
                    NamaCustomer: { type: "string" },
                    StrStatusClaim: { type: "string" },
                    BebanClaimDriver: { type: "number" },
                }
            }
        },
    });

    $("#gridKlaim").kendoGrid({
        dataSource: dsKlaim,
        columns: [
            {
                field: "JnsOrder",
                title: "Jenis Order"
            },
            {
                field: "NoSo",
                title: "No. SO"
            },
            {
                field: "NoPol",
                title: "Vehichle No"
            },
            {
                field: "JnsTruck",
                title: "Jenis Truck"
            },
            {
                field: "NamaCustomer",
                title: "Customer"
            },
            {
                //field: "keterangan",
                title: "Rute"
            },
            {
                //field: "keterangan",
                title: "Jenis Barang"
            },
            {
                //field: "keterangan",
                title: "Target Suhu"
            },
            {
                field: "StrStatusClaim",
                title: "Status Klaim"
            },
            {
                field: "BebanClaimDriver",
                title: "Nilai Klaim",
                template: "Rp #:BebanClaimDriver#"
            },
        ],
    }).data("kendoGrid");
});