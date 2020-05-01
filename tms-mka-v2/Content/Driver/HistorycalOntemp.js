var dsOnTemp;

$(document).ready(function () {
    dsOnTemp = new kendo.data.DataSource({
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
                    TargetWaktu: {type: "string"}, RangeSuhu: {type: "string"}, SuhuAvg: {type: "string"}, AcOff: {type: "string"}
                }
            }
        },
    });

    $("#gridOnTemp").kendoGrid({
        dataSource: dsOnTemp,
        columns: [
            {field: "Precooling"}, {field: "ACMati", title: "Ac. MAti"}, {field: "SuhuSesuai", title: "Suhu Sesuai"}, {field: "JenisOrder", title: "Jenis Order"}, {field: "NoSo", title: "No. SO"},
            {field: "VehicleNo", title: "Vehicle No"}, {field: "JenisTruck", title: "Jenis Truck"}, {field: "CustomerNama", title: "Customer"}, {field: "Rute", title: "Rute"},
            {field: "JenisBarang", title: "Jenis Barang"}, {field: "RangeSuhu", title: "Range Suhu"}, {field: "SuhuAvg", title: "Suhu AVG", template: "#= parseFloat(SuhuAvg).toFixed(2) #"},
            {title: "Deviasi"}, {field: "AcOff", title: "Ac Off"}, {title: "Max Off"},
        ],
    }).data("kendoGrid");
});