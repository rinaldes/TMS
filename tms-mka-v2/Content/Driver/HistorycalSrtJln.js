var dsStat;
var data1 = [], data2 = [], data3 = [];
$(document).ready(function () {
    setTimeout(function () {
        callSrtJln()
    }, 2000)
});

function callSrtJln(){
    $.ajax({
        url: "/driver/BindingDokumen?id=" + $("#Id").val(),
        type: "POST",
        success: function (res) {
            var data = JSON.parse(res);
            var total = data.listModel1.length + data.listModel2.length + data.listModel3.length;
            data1 = data.listModel1;
            data2 = data.listModel2;
            data3 = data.listModel3;
            $('#SrtJlnLengkap').html("Lengkap: " + data.listModel1.length + "(" + data.listModel1.length * 100 / total + " %)")
            $('#SrtJlnTdkLengkap').html("Tidak Lengkap: " + data.listModel2.length + "(" + data.listModel2.length * 100 / total + " %)")
            $('#SrtJlnTdkAda').html("Tidak Ada: " + data.listModel3.length + "(" + data.listModel3.length * 100 / total + " %)")
        }
    });
    $("#grid1").kendoGrid({
        dataSource: data1,
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