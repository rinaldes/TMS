var dsItemTruk = [];
var gridItemTruk;
var selectedData;

function GenerateTanggalSO() {
    if ($('#patokanStr').data("kendoDatePicker").value() == null || $('#patokanEnd').data("kendoDatePicker").value() == null) {
        swal("", "Harap pilih periode mulai dan akhir.", "warning")
    }
    else {
        dsItemTruk = [];
        gridItemTruk.dataSource.data(dsItemTruk);
        $.ajax({
            url: '/SalesOrderKontrak/getDetailByRange',
            type: 'GET',
            dataType: 'Json',
            cache: false,
            data: {
                Id: $('#SalesOrderId').val(),
                StrDate: kendo.toString($('#patokanStr').data("kendoDatePicker").value(), "dd-MM-yyyy"),
                EndDate: kendo.toString($('#patokanEnd').data("kendoDatePicker").value(), "dd-MM-yyyy"),
            },
            success: function (res) {
                if (res.length > 0) {
                    $.each(res, function (index, data) {
                        gridItemTruk.dataSource.add({
                            Id: data.Id,
                            noSO: data.NoSo,
                            TanggalMuat: data.TaggalMuat,
                            IdTruk: data.IdTruk,
                            VehicleNumber: data.VehicleNumber,
                            JenisTruck: data.JenisTruck,
                            IdDriver1: data.IdDriver1,
                            KodeDriver1: data.KodeDriver1,
                            NamaDriver1: data.NamaDriver1,
                            IdDriver2: data.IdDriver2,
                            KodeDriver2: data.KodeDriver2,
                            NamaDriver2: data.NamaDriver2,
                        });
                    });
                }
                else {
                    swal("", "Sales order sudah diproses.", "warning");
                }
            }
        })
    }
}

function EditItem(e) {
    e.preventDefault();
    selectedData = this.dataItem(getDataRowGrid(e));
    $('#IdTruckItem').val(selectedData.IdTruk);
    $('#VehicleNoItem').val(selectedData.VehicleNumber);
    $('#JenisTrukItem').val(selectedData.JenisTruck);
    $('#IdDriver1Item').val(selectedData.IdDriver1);
    $('#KodeDriver1Item').val(selectedData.KodeDriver1);
    $('#NamaDriver1Item').val(selectedData.NamaDriver1);
    $('#IdDriver2Item').val(selectedData.IdDriver2);
    $('#KodeDriver2Item').val(selectedData.KodeDriver2);
    $('#NamaDriver2Item').val(selectedData.NamaDriver2);
    $('#ModalDriver').modal('show');
}

function SelectTruk(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    $('#IdTruckItem').val(data.IdTruk);
    $('#VehicleNoItem').val(data.VehicleNumber);
    $('#JenisTrukItem').val(data.JenisTruck);
    /*$('#IdDriver1Item').val(data.IdDriver1);
    $('#KodeDriver1Item').val(data.KodeDriver1);
    $('#NamaDriver1Item').val(data.NamaDriver1);
    $('#IdDriver2Item').val(data.IdDriver2);
    $('#KodeDriver2Item').val(data.KodeDriver2);
    $('#NamaDriver2Item').val(data.NamaDriver2);*/
    $('#ModalMasterTruk').modal('hide');
}

function clearTruk() {
    $('#IdTrukItem').val('');
    $('#VehicleNoItem').val('');
    $('#JenisTruckItem').val('');
}

function DeleteItem(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    gridItemTruk.dataSource.remove(data);
}

function ProsesTanggalSO()
{
    //cek apakah semua sudah di assign truk dan driver nya
    var dataItem = gridItemTruk.dataSource.data();

    if (dataItem.length > 0) {
        var SelSO = [];
        var SelDriver = [];
        for (var i = 0; i < dataItem.length - 1 ; i++) {
            if (dataItem[i].IdTruk == null) {
                SelSO.push(dataItem[i].noSO);
            }
            if (dataItem[i].IdDriver1 == null) {
                SelDriver.push(dataItem[i].noSO);
            }
        }
        if (SelSO.length > 0) {
            swal("", "Truk belum dipilih untuk SO " + SelSO.join(), "warning");
        } else if (SelDriver.length > 0) {
            swal("", "Driver belum dipilih untuk SO " + SelSO.join(), "warning");
        } else {
            //valid
            $.ajax({
                url: '/KonfirmasiKontrak/Proses',
                type: 'POST',
                dataType: 'Json',
                cache: false,
                data: {
                    listSo: JSON.stringify(dataItem),
                    idSo: IdSo == 0 ? $('#SalesOrderId').val() : IdSo
                },
                success: function (res) {
                    swal("", "Success.", "success");
                    dsItemTruk = [];
                    gridItemTruk.dataSource.data(dsItemTruk);
                }
            })
        }

    } else {
        swal("", "Tidak ada sales order yang dipilih.", "warning");
    }
}

function ValidateDriver(idDriver) {
    var dataItem = gridItemTruk.dataSource.data();

    for (var i = 0 ; i < dataItem.length ; i++) {
        if (dataItem[i].uid != selectedData.uid) {
            if (dataItem[i].IdDriver1 == idDriver || dataItem[i].IdDriver2 == idDriver) {
                ErrorNotif('Driver sudah dipilih pada truk ' + dataItem[i].VehicleNumber);
                return false;
            }
        }
    }
    return true;
}

function SelectDriver(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));

    if (caller == 1) {
        if (data.Id == $('#IdDriver2Item').val()) {
            ErrorNotif('Driver ' + data.NamaDriver + ' sudah dipilih.');
        }
        else {
            if (ValidateDriver(data.Id)) {
                $('#IdDriver1Item').val(data.Id);
                $('#KodeDriver1Item').val(data.KodeDriver);
                $('#NamaDriver1Item').val(data.NamaDriver);
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
            }
        }
    }
    $('#ModalMasterDriver').modal('hide');
}

function clearDriver(caller) {
    if (caller == 1) {
        $('#IdDriver1Item').val('');
        $('#KodeDriver1Item').val('');
        $('#NamaDriver1Item').val('');
    }
    else {
        $('#IdDriver2Item').val('');
        $('#KodeDriver2Item').val('');
        $('#NamaDriver2Item').val('');
    }
}

function SaveDriver() {
    selectedData.set("IdTruk", $('#IdTruckItem').val());
    selectedData.set("VehicleNumber", $('#VehicleNoItem').val());
    selectedData.set("JenisTruck", $('#JenisTrukItem').val());
    selectedData.set("IdDriver1", $('#IdDriver1Item').val());
    selectedData.set("KodeDriver1", $('#KodeDriver1Item').val());
    selectedData.set("NamaDriver1", $('#NamaDriver1Item').val());
    selectedData.set("IdDriver2", $('#IdDriver2Item').val());
    selectedData.set("KodeDriver2", $('#KodeDriver2Item').val());
    selectedData.set("NamaDriver2", $('#NamaDriver2Item').val());
    $('#ModalDriver').modal('hide');
}

$(document).ready(function () {
    IdTruck = $('#JenisTruckId').val();
    IdSo = $('#SalesOrderId').val();

    $('#patokanStr').data("kendoDatePicker").min($('#PeriodStr').val());
    $('#patokanStr').data("kendoDatePicker").max($('#PeriodEnd').val());

    $('#patokanEnd').data("kendoDatePicker").min($('#PeriodStr').val());
    $('#patokanEnd').data("kendoDatePicker").max($('#PeriodEnd').val());

    gridItemTruk = $("#GirdItemTruk").kendoGrid({
        dataSource: {
            data: dsItemTruk,
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        "Id": { type: "number", defaultValue: 0 },
                        "noSO": { type: "string" },
                        "TanggalMuat": { type: "date" },
                        "IdTruk": { type: "number" },
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
        },
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
            { field: "noSO", title: "No SO" },
            { field: "TanggalMuat", title: "Tanggal Muat" },
            { field: "VehicleNumber", title: "Vehicle Number" },
            { field: "JenisTruck", title: "Jenis Truk" },
            { field: "KodeDriver1", title: "ID Driver 1" },
            { field: "NamaDriver1", title: "Nama Driver 1" },
            { field: "KodeDriver2", title: "ID Driver 2" },
            { field: "NamaDriver2", title: "Nama Driver 2" },
        ],
    }).data("kendoGrid");
})