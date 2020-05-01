var dsStat;

$(document).ready(function () {
    dsStat = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Driver/BindingAllHst?id=' + $("#Id").val(),
                dataType: "json"
            },
        },
        schema: {
            model: {
                fields: {
                    Muat: {type: "string"}, Perjalanan: {type: "string"}, Bongkar: {type: "string"}, Precooling: {type: "string"}, ACMati: {type: "string"}, SuhuSesuai: {type: "string"},
                    Klaim: {type: "string"}, Transfer: {type: "string"}, JenisOrder: {type: "string"}, NoSo: {type: "string"}, VehicleNo: {type: "string"}, JenisTruk: {type: "string"},
                    CustomerNama: {type: "string"}, Dari: {type: "string"}, Tujuan: {type: "string"}, JenisBarang: {type: "string"}, TargetSuhu: {type: "string"}, Status: {type: "string"},
                }
            }
        },
    });

    $("#gridAll").kendoGrid({
        dataSource: dsStat,
        columns: [
            {field: "Muat"}, {field: "Perjalanan"}, {field: "Bongkar"}, {field: "Precooling"}, {field: "ACMati", title: "AC Mati"}, {field: "SuhuSesuai", title: "Suhu Sesuai"}, {field: "Klaim"},
            {field: "Transfer"}, {field: "JenisOrder", title: "Jenis Order"}, {field: "NoSo", title: "No SO"}, {field: "VehicleNo", title: "Vehicle No"}, {field: "JenisTruck", title: "Jenis Truck"},
            {field: "CustomerNama", title: "Customer"}, {field: "Rute"}, {field: "JenisBarang", title: "Jenis Barang"}, {field: "TargetSuhu", title: "Target Suhu"},
            {field: "LevelDriver", title: "Level Driver"},
        ],
    }).data("kendoGrid");
});


function callAllHst(){
    $.ajax({
        url: "/Driver/BindingAllHst?id=" + $("#Id").val(),
        type: "POST",
        success: function (res) {
            var data = JSON.parse(res);
            var total = data.listModel1.length + data.listModel2.length + data.listModel3.length;
            data1 = data.listModel1;
            data2 = data.listModel2;
            data3 = data.listModel3;
            $('#allTotal').html("Total:  " + total + " Shipment")
            $('#allSuccess').html("Success: " + data.listModel1.length + "(" + data.listModel1.length * 100 / total + " %)")
            $('#allKlaim').html("Klaim: " + data.listModel2.length + "(" + data.listModel2.length * 100 / total + " %)")
            $('#allTransfer').html("Transfer: " + data.listModel3.length + "(" + data.listModel3.length * 100 / total + " %)")
        }
    });
    $("#gridAll").kendoGrid({
        dataSource: dsStat,
        columns: [
            {field: "Muat"}, {field: "Perjalanan"}, {field: "Bongkar"}, {field: "Precooling"}, {field: "ACMati", title: "AC Mati"}, {field: "SuhuSesuai", title: "Suhu Sesuai"}, {field: "Klaim"},
            {field: "Transfer"}, {field: "JenisOrder", title: "Jenis Order"}, {field: "NoSo", title: "No SO"}, {field: "VehicleNo", title: "Vehicle No"}, {field: "JenisTruck", title: "Jenis Truck"},
            {field: "CustomerNama", title: "Customer"}, {field: "Rute"}, {field: "JenisBarang", title: "Jenis Barang"}, {field: "TargetSuhu", title: "Target Suhu"},
            {field: "LevelDriver", title: "Level Driver"},
        ],
    }).data("kendoGrid");
    $("#grid2").kendoGrid({
        dataSource: data2,
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
    $("#grid3").kendoGrid({
        dataSource: data3,
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
}