var dsStat;

$(document).ready(function () {
    dsStat = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Driver/BindingBAPHistory?id=' + $("#Id").val(),
                dataType: "json"
            },
        },
        schema: {
            model: {
                fields: {
                    TanggalKejadian: { type: "string" },
                    NoBAP: { type: "string" },
                    NamaDriver1: { type: "string" },
                    NamaDriver2: { type: "string" },
                    VehicleNo: { type: "string" },
                    StrKategori: { type: "string" },
                    StatusBap: { type: "string" },
                }
            }
        },
    });

    $("#gridBAP").kendoGrid({
        dataSource: dsStat,
        columns: [
            {
                field: "TanggalKejadian",
                title: "Tanggal",
                template: "#= kendo.toString(kendo.parseDate(TanggalKejadian, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
            },
            {
                field: "NoBAP",
                title: "No. BAP"
            },
            {
                field: "NamaDriver1",
                title: "Driver 1"
            },
            {
                field: "NamaDriver2",
                title: "Driver 2"
            },
            {
                field: "VehicleNo",
                title: "Vehicle No"
            },
            {
                field: "StrKategori",
                title: "Kategori"
            },
            {
                field: "StatusBap",
                title: "Status"
            },
        ],
    }).data("kendoGrid");
});