var dsStat;

$(document).ready(function () {
    dsStat = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Driver/BindingAllHistoryDelivery?id=' + $("#Id").val(),
                dataType: "json"
            },
        },
        schema: {
            model: {
                fields: {
                    Muat: {type: "string"}, Perjalanan: {type: "string"}, Bongkar: {type: "string"}, Precooling: {type: "string"}, ACMati: {type: "string"}, SuhuSesuai: {type: "string"},
                    Klaim: {type: "string"}, Transfer: {type: "string"}, JenisOrder: {type: "string"}, NoSo: {type: "string"}, VehicleNo: {type: "string"}, JenisTruk: {type: "string"},
                    CustomerNama: {type: "string"}, Dari: {type: "string"}, Tujuan: {type: "string"}, JenisBarang: {type: "string"}, TargetSuhu: {type: "string"}, Status: {type: "string"},
                    TglTiba: {type: "string"}, TanggalBerangkat: {type: "string"}, TargetTiba: {type: "string"}, TargetMuat: {type: "string"}, TanggalTiba: {type: "string"}, TglBerangkat: {type: "string"},
                    TargetWaktu: {type: "string"}
                }
            }
        },
    });

    $("#gridOnTime").kendoGrid({
        dataSource: dsStat,
        columns: [
            {field: "Muat"}, {field: "Perjalanan"}, {field: "Bongkar"}, {field: "JenisOrder", title: "Jenis Order"}, {field: "NoSo", title: "No SO"}, {field: "VehicleNo", title: "Vehicle No"},
            {field: "JenisTruck", title: "Jenis Truck"}, {field: "CustomerNama", title: "Customer"}, {field: "Rute"}, {field: "TargetMuat", title: "Target Muat"},
            {field: "TanggalTiba", title: "Tanggal Tiba"}, {field: "TglBerangkat", title: "Tanggal Berangkat"}, {field: "TargetWaktu", title: "Target Waktu"}, {field: "TargetTiba", title: "Target Tiba"},
            {field: "TanggalTiba", title: "Aktual Tiba"}, {field: "Delay"}
        ],
    }).data("kendoGrid");
});