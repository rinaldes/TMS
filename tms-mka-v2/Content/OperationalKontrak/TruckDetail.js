var dsItemTruk;
var gridItemTruk;
var selectedData;

function SelectTruk(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    var dataItem = gridItemTruk.dataSource.data();

    if (ValidateTruck(data.Id)) {
//        if (data.StatusTruk == 'Available') {
            console.log(data)
            gridItemTruk.dataSource.add({
                IdTruk: data.Id,
                Status: data.StatusTruk,
                VehicleNumber: data.VehicleNo,
                JenisTruck: data.JenisTruk,
                IdDriver1: data.IdDriver1,
                KodeDriver1: data.KodeDriver1,
                NamaDriver1: data.NamaDriver1,
                StatusDriver1: data.StatusDriver1,
                IdDriver2: data.IdDriver2,
                KodeDriver2: data.KodeDriver2,
                StatusDriver2: data.StatusDriver2,
            });
            $('#DataTruckId').val(data.Id)
            $('#IdDriver1').val(data.IdDriver1)
            $('#IdDriver2').val(data.IdDriver2)
/*        }
        else {
            swal("", "Truk tidak tersedia, harap pilih truk lain", "warning");
        }*/
    }

    $('#ModalMasterTruk').modal('hide');
}

function DeleteItem(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    gridItemTruk.dataSource.remove(data);
}

function EditItem(e) {
    e.preventDefault();
    selectedData = this.dataItem(getDataRowGrid(e));
    $('#IdDriver1Item').val(selectedData.IdDriver1);
    $('#KodeDriver1Item').val(selectedData.KodeDriver1);
    $('#NamaDriver1Item').val(selectedData.NamaDriver1);
    $('#StatusDriver1Item').val(selectedData.StatusDriver1);
    $('#IdDriver2Item').val(selectedData.IdDriver2);
    $('#KodeDriver2Item').val(selectedData.KodeDriver2);
    $('#NamaDriver2Item').val(selectedData.NamaDriver2);
    $('#StatusDriver2Item').val(selectedData.StatusDriver2);
    $('#ModalDriver').modal('show');
}

function ValidateTruck(IdTruk) {
    var dataItem = gridItemTruk.dataSource.data();

    for (var i = 0 ; i < dataItem.length ; i++) {
        if (dataItem[i].IdTruk == IdTruk) {
            swal("", "Truk sudah dipilih.", "warning");
            return false;
        }
    }
    return true;
}

function ValidateDriver(idDriver) {
    var dataItem = gridItemTruk.dataSource.data();

    for (var i = 0 ; i < dataItem.length ; i++) {
        if (dataItem[i].IdDriver1 == idDriver || dataItem[i].IdDriver2 == idDriver) {
            ErrorNotif('Driver sudah dipilih pada truk ' + dataItem[i].VehicleNumber);
            return false;
        }
    }
    return true;
}

function SelectDriver(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    console.log(data)
//    if (data.StatusSo == 'Available') {
        if (caller == 1) {
            if (data.Id == $('#IdDriver2Item').val()) {
                ErrorNotif('Driver ' + data.NamaDriver + ' sudah dipilih.');
            }
            else {
                if (ValidateDriver(data.Id)) {
                    $('#IdDriver1Item').val(data.Id);
                    $('#KodeDriver1Item').val(data.KodeDriver);
                    $('#NamaDriver1Item').val(data.NamaDriver);
                    $('#StatusDriver1Item').val(data.Status);
                }
            }
        }
        else if (caller == 2) {
            if (data.Id == $('#IdDriver1Item').val()) {
                ErrorNotif('Driver ' + data.NamaDriver + ' sudah dipilih.');
            }
            else {
                if (ValidateDriver(data.Id)) {
                    $('#IdDriver2Item').val(data.Id);
                    $('#KodeDriver2Item').val(data.KodeDriver);
                    $('#NamaDriver2Item').val(data.NamaDriver);
                    $('#StatusDriver2Item').val(data.Status);
                }
            }
        }
/*    }
    else {
        ErrorNotif('Driver ' + data.NamaDriver + ' tidak tersedia, harap pilih driver lain.');
    }
*/
    $('#ModalMasterDriver').modal('hide');
}

function clearDriver(caller) {
    if (caller == 1) {
        $('#IdDriver1Item').val('');
        $('#KodeDriver1Item').val('');
        $('#NamaDriver1Item').val('');
        $('#StatusDriver1Item').val('');
    }
    else {
        $('#IdDriver2Item').val('');
        $('#KodeDriver2Item').val('');
        $('#NamaDriver2Item').val('');
        $('#StatusDriver2Item').val('');
    }
}

function SaveDriver() {
    selectedData.set("IdDriver1", $('#IdDriver1Item').val());
    selectedData.set("KodeDriver1", $('#KodeDriver1Item').val());
    selectedData.set("NamaDriver1", $('#NamaDriver1Item').val());
    selectedData.set("StatusDriver1", $('#StatusDriver1Item').val());
    selectedData.set("IdDriver2", $('#IdDriver2Item').val());
    selectedData.set("KodeDriver2", $('#KodeDriver2Item').val());
    selectedData.set("NamaDriver2", $('#NamaDriver2Item').val());
    selectedData.set("StatusDriver2", $('#StatusDriver2Item').val());
    $('#IdDriver1').val($('#IdDriver1Item').val())
    $('#IdDriver2').val($('#IdDriver2Item').val())
    $('#ModalDriver').modal('hide');
}

$(document).ready(function () {
    IdTruck = $('#JenisTruckId').val();
    IdSo = $('#SalesOrderId').val();

    dsItemTruk = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/SalesOrderKontrak/GetItemTruck?idSo=' + $("#SalesOrderKontrakId").val(),
                dataType: "json"
            },
        },
        schema: {
            model: {
                id: "Id",
                fields: {
                    "Id": { type: "number", defaultValue: 0 },
                    "IdTruk": { type: "number" },
                    "Status": { type: "string" },
                    "StatusDriver1": { type: "string" },
                    "StatusDriver2": { type: "string" },
                    "VehicleNumber": { type: "string" },
                    "JenisTruck": { type: "string" },
                    "IdDriver1": { type: "number" },
                    "KodeDriver1": { type: "string" },
                    "NamaDriver1": { type: "string" },
                    "IdDriver2": { type: "number" },
                    "KodeDriver2": { type: "string" },
                    "NamaDriver2": { type: "string" },
                }
            }
        },
        pageSize: 5,
        pageable: true,
        sortable: true,
    });

    gridItemTruk = $("#GirdItemTruk").kendoGrid({
        dataSource: dsItemTruk,
        filterable: kendoGridFilterable,
        sortable: true,
        pageable: true,
        columns: [
            {
                command: [
                    {
                        name: "edit",
                        text: "edit",
                        click: EditItem,
                        imageClass: "glyphicon glyphicon-edit",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=imageClass#"></span></a>'
                    },
                    {
                        name: "delete",
                        text: "delete",
                        click: DeleteItem,
                        imageClass: "glyphicon glyphicon-remove",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=iconClass# #=imageClass#"></span></a>'
                    }
                ],
                width: "60px"
            },
            { field: "Status", title: "Status" },
            { field: "VehicleNumber", title: "Vehicle Number" },
            { field: "JenisTruck", title: "Jenis Truk" },
            { field: "KodeDriver1", title: "ID Driver 1" },
            { field: "NamaDriver1", title: "Nama Driver 1" },
            { field: "KodeDriver2", title: "ID Driver 2" },
            { field: "NamaDriver2", title: "Nama Driver 2" },
        ],
    }).data("kendoGrid");
})

$('#formsubmit').submit(function (e) {
    if ($('#Status').val() != 'draft') {

    if (gridItemTruk.dataSource.total() == 0) {
        swal("", "Data truck tidak boleh kosong.", "warning");
        e.preventDefault();
    }
    else {
        for (var i = 0; i < gridItemTruk.dataSource.total() ; i++) {
            if (gridItemTruk.dataSource.data()[i].IdDriver1 == 0 || gridItemTruk.dataSource.data()[i].IdDriver1 == null) {
                swal("", "Driver pada truck " + gridItemTruk.dataSource.data()[i].VehicleNumber + " tidak boleh kosong.", "warning");
                e.preventDefault();
            }
            //cek driver anu kagok dipilih tina penetapa driver na teu aktif
            //driver 1
            if (gridItemTruk.dataSource.data()[i].StatusDriver1 != "ACTIVE") {
                swal("", "Driver 1 pada truck " + gridItemTruk.dataSource.data()[i].VehicleNumber + " tidak aktif.", "warning");
                e.preventDefault();
            }
            //driver 2
            if (gridItemTruk.dataSource.data()[i].StatusDriver2 != "" && gridItemTruk.dataSource.data()[i].StatusDriver2 != null) {
                if (gridItemTruk.dataSource.data()[i].StatusDriver2 != "ACTIVE") {
                    swal("", "Driver 2 pada truck " + gridItemTruk.dataSource.data()[i].VehicleNumber + " tidak aktif.", "warning");
                    e.preventDefault();
                }
            }
        }
        $('#strListTruck').val(JSON.stringify(gridItemTruk.dataSource.data()));
    }

    //jumlah truk harus sama dengan yang dipesan
    if (parseInt($('#JumlahTruck').val()) != gridItemTruk.dataSource.total() && $('#Status').val() == 'save planning') {
        swal("", "Jumlah truk yang dipilih tidak sama dengan jumlah truk yang dipesan.", "warning");
        e.preventDefault();
    }
    }
})

function notifSaveSubmitKontrak(frm, val) {
    var previousWindowKeyDown = window.onkeydown;
    return swal(
        {
            title: "Submit Data",
            text: "Apakah anda yakin untuk submit data ini?",
            type: "warning",
            cancelButtonText: "Batal",
            showCancelButton: true,
            confirmButtonClass: "btn btn-primary",
            confirmButtonText: "Ya",
            closeOnConfirm: false
        },
        function (isConfirm) {
            swal({
                title: "Loading",
                text: "Harap Menunggu...",
                imageUrl: "/Content/sweet-alert/ajax-loader.gif",
                closeOnConfirm: false,
                confirmButtonClass: "hidden",
                //imageSize: "80x80"
            });

            if (previousWindowKeyDown !== undefined && window.onkeydown !== previousWindowKeyDown)
                window.onkeydown = previousWindowKeyDown;

            $('#Status').val(val);
            var status = "Available"
            if (val == 'draft')
                frm.submit();
            else {
                for (var i = 0; i < gridItemTruk.dataSource.total() ; i++) {
                    $.ajax({
                        url: '/Driver/CheckDriverAvailability?id=' + gridItemTruk.dataSource.data()[i].IdDriver1,
                        type: "GET",
                        success: function (res) {
                            {
                                var data = jQuery.parseJSON(res);
                                status = data.data == null ? "NON AKTIF" : data.data.StatusSo;
                                if (i == gridItemTruk.dataSource.total() && status == "Available") {
                                    frm.submit();
                                }
                                else if (status != "Available") {
                                    frm.submit();
                                }
                            }
                        },
                    });
                }
            }
            //            frm.submit();
        });
}