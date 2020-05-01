var currentTr;
var modalUnloadingAddress;
var formUnloadingAddress;
var validatorUnloadingAdd;

var cboProvinsiUnloading;
var cboKotaUnloading;
var cboKecUnloading;
var cboKelUnloading;

var dsProvinsiUnloading;
var dsKotaUnloading;
var dsKecUnloading;
var dsKelUnloading;

var GridCustUnLoad;
$(document).ready(function () {
    var dsGridUnLoad = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/BindingUnLoadingAddress?idCust=' + $("#Id").val(),
                dataType: "json"
            },
        },
        schema: {
            data: "data",
            model: {
                fields: {
                    Id: { type: "number" },
                    CustomerId: { type: "number" },
                    Code: { type: "string" },
                    Alamat: { type: "string" },
                    IdProvinsi: { type: "number" },
                    Provinsi: { type: "string" },
                    IdKabKota: { type: "number" },
                    Kota: { type: "string" },
                    IdKec: { type: "number" },
                    Kecamatan: { type: "string" },
                    IdKel: { type: "number" },
                    Kelurahan: { type: "string" },
                    Longitude: { type: "string" },
                    Latitude: { type: "string" },
                    Radius: { type: "number" },
                    Zona: { type: "string" },
                    Telp: { type: "string" },
                    Fax: { type: "string" },
                }
            }
        },
    });

    GridCustUnLoad = $("#GridCustUnLoad").kendoGrid({
        filterable: kendoGridFilterable,
        sortable: true,
        dataSource: dsGridUnLoad,
        columns: [
            {
                command: [
                    {
                        name: "edit",
                        text: "edit",
                        click: editUnLoad,
                        imageClass: "glyphicon glyphicon-edit",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=imageClass#"></span></a>'
                    },
                    {
                        name: "delete",
                        text: "delete",
                        click: deleteUnLoad,
                        imageClass: "glyphicon glyphicon-remove",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=iconClass# #=imageClass#"></span></a>'
                    }
                ],
                width: "60px"
            },
            {
                field: "Code",
                title: "Kode"
            },
            {
                field: "Zona",
                title: "Zona"
            },
            {
                field: "Alamat",
                title: "Alamat"
            },
            {
                field: "Kelurahan",
                title: "Kelurahan/Desa"
            },
            {
                field: "Kecamatan",
                title: "Kecamatan"
            },
            {
                field: "Kota",
                title: "Kabupaten/Kota"
            },
            {
                field: "Provinsi",
                title: "Provinsi"
            },
            {
                field: "Longitude",
                title: "Longitude"
            },
            {
                field: "Latitude",
                title: "Latitude"
            },
            {
                field: "Radius",
                title: "Radius"
            },
            
            {
                field: "Telp",
                title: "Telp"
            },
            {
                field: "Fax",
                title: "Fax"
            }
        ],
    }).data("kendoGrid");

    modalUnloadingAddress = $("#modalFormUnloadAddress");
    formUnloadingAddress = $("#formUnloadingAddress");

    validatorUnloadingAdd = formUnloadingAddress.kendoValidator({
        //rules: {
        //    comboreq: function (input) {
        //        if ($(input).data("kendoComboBox")) {
        //            if ($(input).data("kendoComboBox").selectedIndex == -1) {
        //                return false;
        //            }
        //        }

        //        return true;
        //    },
        //}
    }).data("kendoValidator");

    //location
    dsProvinsiUnloading = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetProvinsi',
                dataType: "json"
            },
        },
    });
    cboProvinsiUnloading = $("#ProvinsiUnloadingAdd").kendoComboBox({
        dataTextField: "Nama",
        dataValueField: "Id",
        dataSource: dsProvinsiUnloading,
        filter: "contains",
        suggest: true,
        autoBind: false,
        //change: OnProvChangeUnloading,
    }).data("kendoComboBox");
    cboKotaUnloading = $("#KotaUnloadingAdd").kendoComboBox({
        dataSource: {
            serverFiltering: true,
            transport: {
                read: {
                    url: '/Location/BindingComboType?type=Kab/Kota',
                    dataType: "json"
                },
            },
        },
        dataTextField: "Nama",
        dataValueField: "Id",
        filter: "contains",
        suggest: true,
        minLength: 3,
        autoBind: false,
        //change: OnKotaChangeUnloading,
    }).data("kendoComboBox");
    cboKecUnloading = $("#KecamatanUnloadingAdd").kendoComboBox({
        dataSource: {
            serverFiltering: true,
            transport: {
                read: {
                    url: '/Location/BindingComboType?type=Kecamatan',
                    dataType: "json"
                },
            },
        },
        dataTextField: "Nama",
        dataValueField: "Id",
        filter: "contains",
        suggest: true,
        minLength: 3,
        autoBind: false,
        //change: OnKecamatanChangeUnloading,
    }).data("kendoComboBox");
    cboKelUnloading = $("#KelurahanUnloadingAdd").kendoComboBox({
        dataSource: {
            serverFiltering: true,
            transport: {
                read: {
                    url: '/Location/BindingComboType?type=Kelurahan',
                    dataType: "json"
                },
            },
        },
        dataTextField: "Nama",
        dataValueField: "Id",
        filter: "contains",
        suggest: true,
        minLength: 3,
        autoBind: false,
        //change: OnKelurahanChangeUnloading
    }).data("kendoComboBox");

    //$("#TlpUnloadingAdd").kendoMaskedTextBox({
    //    mask: "000-000-000-000"
    //});
    //$("#FaxUnloadingAdd").kendoMaskedTextBox({
    //    mask: "000-000-000-000"
    //});
});

function ShowAddressUnloadingPopup() {
    $('.k-invalid-msg').hide();
    $('#CustomerUnloadingAddressId').val(0);
    $('#KodeUnloadingAdd').val("");
    $('#AlamatUnloadingAdd').val("");
    $('#KodeTlpUnloadingAdd').val("");
    $('#KodeFaxUnloadingAdd').val("");
    $('#TlpUnloadingAdd').val("");
    $('#FaxUnloadingAdd').val("");
    $('#ZonaUnloadingAdd').val("");
    cboProvinsiUnloading.value();
    cboProvinsiUnloading.text('');
    cboKotaUnloading.value();
    cboKotaUnloading.text('');
    cboKecUnloading.value();
    cboKecUnloading.text('');
    cboKelUnloading.value();
    cboKelUnloading.text('');
    $('#LatUnloadingAdd').data("kendoNumericTextBox").value('');
    $('#LongUnloadingAdd').data("kendoNumericTextBox").value('');
    $('#RadiusUnloadingAdd').data("kendoNumericTextBox").value('');
}

function SaveUnloadingAdd(conditon) {
    if (validatorUnloadingAdd.validate())
    {
        var data = {
            Id: $('#CustomerUnloadingAddressId').val(),
            CustomerId: $("#Id").val(),
            Code: $('#KodeUnloadingAdd').val(),
            Alamat: $('#AlamatUnloadingAdd').val(),
            IdProvinsi: cboProvinsiUnloading.value(),
            IdKabKota: !isNaN(cboKotaUnloading.value()) ? cboKotaUnloading.value() : cboKotaUnloading.value() == '' ? cboKotaUnloading.value() : -1,
            IdKec: !isNaN(cboKecUnloading.value()) ? cboKecUnloading.value() : cboKecUnloading.value() == '' ? cboKecUnloading.value() : -1,
            IdKel: !isNaN(cboKelUnloading.value()) ? cboKelUnloading.value() : cboKelUnloading.value() == '' ? cboKelUnloading.value() : -1,
            Latitude: $('#LatUnloadingAdd').val(),
            Longitude: $('#LongUnloadingAdd').val(),
            Radius: $('#RadiusUnloadingAdd').data("kendoNumericTextBox").value(),
            Zona: $('#ZonaUnloadingAdd').val(),
            Telp: $('#KodeTlpUnloadingAdd').val() + ' - ' + $('#TlpUnloadingAdd').val(),
            Fax: $('#KodeFaxUnloadingAdd').val() + ' - ' + $('#FaxUnloadingAdd').val()
        };

        goToSavePage("/Customer/CustomerSaveUnLoadingAddress/", data, GridCustUnLoad.dataSource);
        modalUnloadingAddress.modal('hide');
    }
}

function deleteUnLoad(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    goToDeletePage("/Customer/CustomerDeleteUnLoadingAdd?CustId=" + $("#Id").val() + "&id=" + data.Id, this.dataSource);
}

function editUnLoad(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    //init combobox
    //setKotaUnloading(data.IdProvinsi);
    //setKecamatanUnloading(data.IdKabKota);
    //setKelurahanUnloading(data.IdKec);

    $('.k-invalid-msg').hide();
    $('#CustomerUnloadingAddressId').val(data.Id);
    $('#KodeUnloadingAdd').val(data.Code);
    $('#AlamatUnloadingAdd').val(data.Alamat);
    var tel = ("" + data.Telp + "").split(' - ');
    var fax = ("" + data.Fax + "").split(' - ');
    $('#KodeTlpUnloadingAdd').data('kendoMaskedTextBox').value(tel[0]);
    $('#KodeFaxUnloadingAdd').data('kendoMaskedTextBox').value(fax[0]);
    $('#TlpUnloadingAdd').data('kendoMaskedTextBox').value(tel[1]);
    $('#FaxUnloadingAdd').data('kendoMaskedTextBox').value(fax[1]);
    $('#ZonaUnloadingAdd').val(data.Zona);
    cboProvinsiUnloading.value(data.IdProvinsi);
    //cboProvinsiUnloading.text(data.provinsi);
    cboKotaUnloading.value(data.IdKabKota);
    cboKotaUnloading.text(data.Kota);
    cboKecUnloading.value(data.IdKec);
    cboKecUnloading.text(data.Kecamatan);
    cboKelUnloading.value(data.IdKel);
    cboKelUnloading.text(data.Kelurahan);
    $('#LongUnloadingAdd').data("kendoNumericTextBox").value(data.Longitude);
    $('#LatUnloadingAdd').data("kendoNumericTextBox").value(data.Latitude);
    $('#RadiusUnloadingAdd').data("kendoNumericTextBox").value(data.Radius);

    modalUnloadingAddress.modal('show');
}

function OnProvChangeUnloading(e) {
    if (this.value() != "") {
        setKotaUnloading(this.value());
    }
    else {
        cboKotaUnloading.text('');
        cboKotaUnloading.value();
        cboKotaUnloading.setDataSource();
        cboKecUnloading.text('');
        cboKecUnloading.value();
        cboKecUnloading.setDataSource();
        cboKelUnloading.text('');
        cboKelUnloading.value();
        cboKelUnloading.setDataSource();
    }
    $('#LatUnloadingAdd').data("kendoNumericTextBox").value('');
    $('#LongUnloadingAdd').data("kendoNumericTextBox").value('');
}

function setKotaUnloading(idParent) {
    dsKotaUnloading = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetChildLoc?idParent=' + idParent,
                dataType: "json"
            },
        },
    });
    cboKotaUnloading.text('');
    cboKotaUnloading.value();
    cboKotaUnloading.setDataSource(dsKotaUnloading);
    cboKecUnloading.text('');
    cboKecUnloading.value();
    cboKecUnloading.setDataSource();
    cboKelUnloading.text('');
    cboKelUnloading.value();
    cboKelUnloading.setDataSource();
}

function OnKotaChangeUnloading(e) {
    if (this.value() != "") {
        setKecamatanUnloading(this.value());
    }
    else {
        cboKecUnloading.text('');
        cboKecUnloading.value();
        cboKecUnloading.setDataSource();
        cboKelUnloading.text('');
        cboKelUnloading.value();
        cboKelUnloading.setDataSource();
    }
    $('#LatUnloadingAdd').data("kendoNumericTextBox").value('');
    $('#LongUnloadingAdd').data("kendoNumericTextBox").value('');
}

function setKecamatanUnloading(idParent) {
    dsKecUnloading = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetChildLoc?idParent=' + idParent,
                dataType: "json"
            },
        },
    });
    cboKecUnloading.text('');
    cboKecUnloading.value();
    cboKecUnloading.setDataSource(dsKecUnloading);
    cboKelUnloading.text('');
    cboKelUnloading.value();
    cboKelUnloading.setDataSource();
}

function OnKecamatanChangeUnloading(e) {
    if (this.value() != "") {
        setKelurahanUnloading(this.value());
    }
    else {
        //kosongkan semua
        cboKelUnloading.text('');
        cboKelUnloading.value();
        cboKelUnloading.setDataSource();
    }
    $('#LatUnloadingAdd').data("kendoNumericTextBox").value('');
    $('#LongUnloadingAdd').data("kendoNumericTextBox").value('');
}

function setKelurahanUnloading(idParent) {
    dsKelUnloading = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetChildLoc?idParent=' + idParent,
                dataType: "json"
            },
        },
    });
    cboKelUnloading.text('');
    cboKelUnloading.value();
    cboKelUnloading.setDataSource(dsKelUnloading);
}

function OnKelurahanChangeUnloading(e) {
    $('#LatUnloadingAdd').data("kendoNumericTextBox").value('');
    $('#LongUnloadingAdd').data("kendoNumericTextBox").value('');
}

function addMapLatLongUnloadingAdd() {
    if (newLong != '')
        $("#valUnloadingLong").hide();
    if (newLat != '')
        $("#valUnloadingLat").hide();

    $('#LatUnloadingAdd').data("kendoNumericTextBox").value(newLat);
    $('#LongUnloadingAdd').data("kendoNumericTextBox").value(newLong);
    $('#modalviewmapunloadingaddress').modal('hide');
}