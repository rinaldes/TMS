var dsItemTruk, dsAlertTruk, dsAlertDriver1, dsAlertDriver2;
var gridItemTruk, gridAlertTruk, gridAlertDriver1, gridAlertDriver2;
var selectedData;

function SelectTruk(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    $('#IdDataTruck').val(data.Id);
    $('#VehicleNo').val(data.VehicleNo);
    $('#JenisTruckItem').val(data.JenisTruk);
    $('#Driver1Id').val(data.IdDriver1);
    $('#KodeDriver1').val(data.KodeDriver1);
    $('#NamaDriver1').val(data.NamaDriver1);
    $('#Driver2Id').val(data.IdDriver2);
    GenerateAlert($('#IdDataTruck').val(), $('#Driver1Id').val(), $('#Driver2Id').val());
    if ($('#area').val() != 'YES') {
        //formatted_tglMuat = $('#TanggalMuat').val().split(' ')[0]
        //formatted_tglMuat = formatted_tglMuat.split('/')[2] + '-' + formatted_tglMuat.split('/')[1] + '-' + formatted_tglMuat.split('/')[0]
        $.ajax({
            url: '/PlanningOncall/CheckAvailability?id=' + data.Id,
            type: 'GET',
            dataType: 'Json',
            cache: false,
            success: function (res) {
/*                formatted_WaktuSelesai = res.waktupulang.split(' ')[0]
                formatted_WaktuSelesai = formatted_WaktuSelesai.split('/')[2] + '-' + formatted_WaktuSelesai.split('/')[1] + '-' + formatted_WaktuSelesai.split('/')[0]
                if (Date.parse(formatted_WaktuSelesai) <= Date.parse(formatted_tglMuat)) { // asalna JKTS-JKTS :p // ini klo current planning muat > pembanding muat
                    $('#IdDataTruck').val(data.Id);
                    $('#VehicleNo').val(data.VehicleNo);
                    $('#JenisTruckItem').val(data.JenisTruk);
                    $('#Driver1Id').val(data.IdDriver1);
                    $('#KodeDriver1').val(data.KodeDriver1);
                    $('#NamaDriver1').val(data.NamaDriver1);
                    $('#Driver2Id').val(data.IdDriver2);
                    $('#KodeDriver2').val(data.KodeDriver2);
                    $('#NamaDriver2').val(data.NamaDriver2);
                    FindAtm(data.IdDriver1);
                    GenerateAlert($('#IdDataTruck').val(), $('#Driver1Id').val(), $('#Driver2Id').val());
                }
                else if (Date.parse(formatted_WaktuMuat) > Date.parse(formatted_tglPulang)) { //current planning muat < pembanding muat
                    $('#IdDataTruck').val(data.Id);
                    $('#VehicleNo').val(data.VehicleNo);
                    $('#JenisTruckItem').val(data.JenisTruk);
                    $('#Driver1Id').val(data.IdDriver1);
                    $('#KodeDriver1').val(data.KodeDriver1);
                    $('#NamaDriver1').val(data.NamaDriver1);
                    $('#Driver2Id').val(data.IdDriver2);
                    $('#KodeDriver2').val(data.KodeDriver2);
                    $('#NamaDriver2').val(data.NamaDriver2);
                    FindAtm(data.IdDriver1);
                    GenerateAlert($('#IdDataTruck').val(), $('#Driver1Id').val(), $('#Driver2Id').val());
                }
                else {
                    swal("", "Truk tidak tersedia, harap pilih truk lain", "warning");
                }*/
                $('#IdDataTruck').val(data.Id);
                $('#VehicleNo').val(data.VehicleNo);
                $('#JenisTruckItem').val(data.JenisTruk);
                $('#Driver1Id').val(data.IdDriver1);
                $('#KodeDriver1').val(data.KodeDriver1);
                $('#NamaDriver1').val(data.NamaDriver1);
                $('#Driver2Id').val(data.IdDriver2);
                $('#KodeDriver2').val(data.KodeDriver2);
                $('#NamaDriver2').val(data.NamaDriver2);
                FindAtm(data.IdDriver1);
                GenerateAlert($('#IdDataTruck').val(), $('#Driver1Id').val(), $('#Driver2Id').val());
            }
        })
    }
    $('#ModalMasterTruk').modal('hide');
}

function DeleteTruk() {
    $('#IdDataTruck').val('');
    $('#VehicleNo').val('');
    $('#JenisTruckItem').val('');
    $('#Driver1Id').val('');
    $('#KodeDriver1').val('');
    $('#NamaDriver1').val('');
    $('#Driver2Id').val('');
    $('#KodeDriver2').val('');
    $('#NamaDriver2').val('');
    GenerateAlert($('#IdDataTruck').val(), $('#Driver1Id').val(), $('#Driver2Id').val());
}

function SelectDriver(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    if (data.StatusSo == 'Available' || $('#area').val() == 'YES') {
        if (caller == 1) {
            if (data.Id == $('#Driver2Id').val()) {
                swal(' ', 'Driver ' + data.NamaDriver + ' sudah dipilih.', 'warning');
            }
            else {
                $('#Driver1Id').val(data.Id);
                $('#KodeDriver1').val(data.KodeDriver);
                $('#NamaDriver1').val(data.NamaDriver);
                FindAtm(data.Id);
                GenerateDriver1(data.Id);
            }
        }
        else if (caller == 2) {
            if (data.Id == $('#Driver1Id').val()) {
                swal(' ', 'Driver ' + data.NamaDriver + ' sudah dipilih.', 'warning');
            }
            else {
                $('#Driver2Id').val(data.Id);
                $('#KodeDriver2').val(data.KodeDriver);
                $('#NamaDriver2').val(data.NamaDriver);
                GenerateDriver2(data.Id);
            }
        }
    }
    else { //hitung hari kerja
        /*        $.ajax({
            url: '/PlanningOncall/CheckAvailability?DriverId=' + data.Id + '&SONumber=' + $('#SONumber').val(),
            type: 'GET',
            dataType: 'Json',
            cache: false,
            success: function (res) {
                if (res.Message == "YES") {
                    if (caller == 1) {
                        if (data.Id == $('#Driver2Id').val()) {
                            swal(' ', 'Driver ' + data.NamaDriver + ' sudah dipilih.', 'warning');
                        }
                        else {
                            $('#Driver1Id').val(data.Id);
                            $('#KodeDriver1').val(data.KodeDriver);
                            $('#NamaDriver1').val(data.NamaDriver);
                            FindAtm(data.Id);
                            GenerateDriver1(data.Id);
                        }
                    }
                    else if (caller == 2) {
                        if (data.Id == $('#Driver1Id').val()) {
                            swal(' ', 'Driver ' + data.NamaDriver + ' sudah dipilih.', 'warning');
                        }
                        else {
                            $('#Driver2Id').val(data.Id);
                            $('#KodeDriver2').val(data.KodeDriver);
                            $('#NamaDriver2').val(data.NamaDriver);
                            GenerateDriver2(data.Id);
                        }
                    }
                }
                else {
                    swal(' ', 'Driver ' + data.NamaDriver + ' Tidak Tersedia.', 'warning');
                }
            }
        })*/
        if (caller == 1) {
            if (data.Id == $('#Driver2Id').val()) {
                swal(' ', 'Driver ' + data.NamaDriver + ' sudah dipilih.', 'warning');
            }
            else {
                $('#Driver1Id').val(data.Id);
                $('#KodeDriver1').val(data.KodeDriver);
                $('#NamaDriver1').val(data.NamaDriver);
                FindAtm(data.Id);
                GenerateDriver1(data.Id);
            }
        }
        else if (caller == 2) {
            if (data.Id == $('#Driver1Id').val()) {
                swal(' ', 'Driver ' + data.NamaDriver + ' sudah dipilih.', 'warning');
            }
            else {
                $('#Driver2Id').val(data.Id);
                $('#KodeDriver2').val(data.KodeDriver);
                $('#NamaDriver2').val(data.NamaDriver);
                GenerateDriver2(data.Id);
            }
        }
    }
    $('#ModalMasterDriver').modal('hide');
}

function clearDriver(caller) {
    if (caller == 1) {
        $('#Driver1Id').val('');
        $('#KodeDriver1').val('');
        $('#NamaDriver1').val('');
        GenerateDriver1($('#Driver1Id').val());
    }
    else {
        $('#Driver2Id').val('');
        $('#KodeDriver2').val('');
        $('#NamaDriver2').val('');
        GenerateDriver2($('#Driver2Id').val());
    }
}

function GenerateAlert(idTruck, idDriver1, idDriver2) {
    GenerateAlertTruck(idTruck);
    GenerateDriver1(idDriver1);
    GenerateDriver2(idDriver2);
}

function GenerateAlertTruck(id) {
    dsAlertTruk = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/DataTruck/GetAlert?id=' + id,
                dataType: "json"
            },
        },
    });
    gridAlertTruk.setDataSource(dsAlertTruk);
}

function GenerateDriver1(id) {
    if (id != null && id != 0) {
        dsAlertDriver1 = new kendo.data.DataSource({
            transport: {
                read: {
                    url: '/Driver/GetAlert?id=' + id,
                    dataType: "json"
                },
            },
        });
        gridAlertDriver1.setDataSource(dsAlertDriver1);
    }
}

function GenerateDriver2(id) {
    if (id != null && id != 0) {
        dsAlertDriver2 = new kendo.data.DataSource({
            transport: {
                read: {
                    url: '/Driver/GetAlert?id=' + id,
                    dataType: "json"
                },
            },
        });
        gridAlertDriver2.setDataSource(dsAlertDriver2);
    }
}

function FindAtm(id) {
    if (id != null && id != 0) {
        $.ajax({
            url: '/Atm/FindByDriver?id=' + id,
            type: 'GET',
            dataType: 'Json',
            cache: false,
            success: function (res) {
                $('#AtmId').val(res.Id);
                $('#StrRekening').val(res.NoRekening);
                $('#AtasNamaRek').val(res.AtasNama);
                $('#Bank').val(res.NamaBank);
            }
        })
    }
    else {
        $('#AtmId').val('');
        $('#StrRekening').val('');
        $('#AtasNamaRek').val('');
        $('#Bank').val('');
    }
}

$(document).ready(function () {
    IdTruck = $('#JenisTruckId').val() != null ? $('#JenisTruckId').val() : $('#IdJnsTruck').val();
    IdSo = $('#SalesOrderId').val();
    gridAlertTruk = $("#GridAlertTruk").kendoGrid({
        dataSource: dsAlertTruk,
        columns: [
            { field: "Keterangan", title: "Alert" },
            { field: "Value", title: "" },
        ],
    }).data("kendoGrid");

    gridAlertDriver1 = $("#GridAlertDr1").kendoGrid({
        dataSource: dsAlertDriver1,
        columns: [
            { field: "Keterangan", title: "Alert" },
            { field: "Value", title: "" },
        ],
    }).data("kendoGrid");

    gridAlertDriver2 = $("#GridAlertDr2").kendoGrid({
        dataSource: dsAlertDriver2,
        columns: [
            { field: "Keterangan", title: "Alert" },
            { field: "Value", title: "" },
        ],
    }).data("kendoGrid");

    GenerateAlert($('#IdDataTruck').val(), $('#Driver1Id').val(), $('#Driver2Id').val());
})