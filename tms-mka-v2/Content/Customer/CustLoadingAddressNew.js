var currentTr;
var modalLoadingAddress;
var formLoadingAddress;
var validatorLoadingAdd;

var cboProvinsiLoad;
var cboKotaLoad;
var cboKecLoad;
var cboKelLoad;

var dsProvinsiLoad;
var dsKotaLoad;
var dsKecLoad;
var dsKelLoad;

var GridCustLoad;
$(document).ready(function () {
    var dsGridLoad = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/BindingLoadingAddress?idCust=' + $("#Id").val(),
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

    GridCustLoad = $("#GridCustLoad").kendoGrid({
        dataSource: dsGridLoad,
        columns: [
            {
                command: [
                    {
                        name: "edit",
                        text: "edit",
                        click: editLoad,
                        imageClass: "glyphicon glyphicon-edit",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=imageClass#"></span></a>'
                    },
                    {
                        name: "delete",
                        text: "delete",
                        click: deleteLoad,
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

    modalLoadingAddress = $("#modalFormLoadingAddress");
    formLoadingAddress = $("#formLoadingAddress");

    validatorLoadingAdd = formLoadingAddress.kendoValidator({
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
    dsProvinsiLoad = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetProvinsi',
                dataType: "json"
            },
        },
    });
    cboProvinsiLoad = $("#ProvinsiLoadingAdd").kendoComboBox({
        dataTextField: "Nama",
        dataValueField: "Id",
        dataSource: dsProvinsiLoad,
        filter: "contains",
        suggest: true,
        //change: OnProvChangeLoad,
    }).data("kendoComboBox");
    cboKotaLoad = $("#KotaLoadingAdd").kendoComboBox({
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
        //change: OnKotaChangeLoad,
    }).data("kendoComboBox");
    cboKecLoad = $("#KecamatanLoadingAdd").kendoComboBox({
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
        //change: OnKecamatanChangeLoad,
    }).data("kendoComboBox");
    cboKelLoad = $("#KelurahanLoadingAdd").kendoComboBox({
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
        //change: OnKelurahanChangeLoad
    }).data("kendoComboBox");

    //$("#TlpLoadingAdd").kendoMaskedTextBox({
    //    mask: "000-000-000-000"
    //});
    //$("#FaxLoadingAdd").kendoMaskedTextBox({
    //    mask: "000-000-000-000"
    //});
});

function ShowLoadingAddressPopup() {
    $('.k-invalid-msg').hide();
    $('#CustomerLoadingAddressId').val(0);
    $('#KodeLoadingAdd').val("");
    $('#AlamatLoadingAdd').val("");
    $('#KodeTlpLoadingAdd').val("");
    $('#KodeFaxLoadingAdd').val("");
    $('#TlpLoadingAdd').val("");
    $('#FaxLoadingAdd').val("");
    $('#ZonaLoadingAdd').val("");
    cboProvinsiLoad.value();
    cboProvinsiLoad.text('');
    cboKotaLoad.value();
    cboKotaLoad.text('');
    cboKecLoad.value();
    cboKecLoad.text('');
    cboKelLoad.value();
    cboKelLoad.text('');
    $('#LatLoadingAdd').data("kendoNumericTextBox").value('');
    $('#LongLoadingAdd').data("kendoNumericTextBox").value('');
    $('#RadiusLoadingAdd').data("kendoNumericTextBox").value('');
}

function SaveAddLoading() {
    if (validatorLoadingAdd.validate())
    {
        var data = {
            Id: $('#CustomerLoadingAddressId').val(),
            CustomerId: $("#Id").val(),
            Code: $('#KodeLoadingAdd').val(),
            Alamat: $('#AlamatLoadingAdd').val(),
            IdProvinsi: cboProvinsiLoad.value(),
            IdKabKota: !isNaN(cboKotaLoad.value()) ? cboKotaLoad.value() : cboKotaLoad.value() == '' ? cboKotaLoad.value() : -1,
            IdKec: !isNaN(cboKecLoad.value()) ? cboKecLoad.value() : cboKecLoad.value() == '' ? cboKecLoad.value() : -1,
            IdKel: !isNaN(cboKelLoad.value()) ? cboKelLoad.value() : cboKelLoad.value() == '' ? cboKelLoad.value() : -1,
            Latitude: $('#LatLoadingAdd').val(),
            Longitude: $('#LongLoadingAdd').val(),
            Radius: $('#RadiusLoadingAdd').data("kendoNumericTextBox").value(),
            Zona: $('#ZonaLoadingAdd').val(),
            Telp: $('#KodeTlpLoadingAdd').val() + ' - ' + $('#TlpLoadingAdd').val(),
            Fax: $('#KodeFaxLoadingAdd').val() + ' - ' + $('#FaxLoadingAdd').val()
        };

        goToSavePage("/Customer/CustomerSaveLoadingAddress/", data, GridCustLoad.dataSource);
        modalLoadingAddress.modal('hide');
    }
}

function deleteLoad(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    goToDeletePage("/Customer/CustomerDeleteLoadingAdd?CustId=" + $("#Id").val() + "&id=" + data.Id, this.dataSource);
}

function editLoad(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));

    //setKotaLoad(data.IdProvinsi);
    //setKecamatanLoad(data.IdKabKota);
    //setKelurahanLoad(data.IdKec);

    $('.k-invalid-msg').hide();
    $('#CustomerLoadingAddressId').val(data.Id);
    $('#KodeLoadingAdd').val(data.Code);
    $('#AlamatLoadingAdd').val(data.Alamat);
    var tel = data.Telp.split(' - ');
    var fax = data.Fax.split(' - ');
    $('#KodeTlpLoadingAdd').data('kendoMaskedTextBox').value(tel[0]);
    $('#KodeFaxLoadingAdd').data('kendoMaskedTextBox').value(fax[0]);
    $('#TlpLoadingAdd').data('kendoMaskedTextBox').value(tel[1]);
    $('#FaxLoadingAdd').data('kendoMaskedTextBox').value(fax[1]);
    $('#ZonaLoadingAdd').val(data.Zona);
    cboProvinsiLoad.value(data.IdProvinsi);
    cboKotaLoad.value(data.IdKabKota);
    cboKotaLoad.text(data.Kota);
    cboKecLoad.value(data.IdKec);
    cboKecLoad.text(data.Kecamatan);
    cboKelLoad.value(data.IdKel);
    cboKelLoad.text(data.Kelurahan);
    $('#LongLoadingAdd').data("kendoNumericTextBox").value(data.Longitude);
    $('#LatLoadingAdd').data("kendoNumericTextBox").value(data.Latitude);
    $('#RadiusLoadingAdd').data("kendoNumericTextBox").value(data.Radius);

    modalLoadingAddress.modal('show');
}

function OnProvChangeLoad(e) {
    if (this.value() != "") {
        setKotaLoad(this.value());
    }
    else {
        cboKotaLoad.text('');
        cboKotaLoad.value();
        cboKotaLoad.setDataSource();
        cboKecLoad.text('');
        cboKecLoad.value();
        cboKecLoad.setDataSource();
        cboKelLoad.text('');
        cboKelLoad.value();
        cboKelLoad.setDataSource();
    }
    $('#LatLoadingAdd').data("kendoNumericTextBox").value('');
    $('#LongLoadingAdd').data("kendoNumericTextBox").value('');
}

function setKotaLoad(idParent) {
    dsKotaLoad = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetChildLoc?idParent=' + idParent,
                dataType: "json"
            },
        },
    });
    cboKotaLoad.text('');
    cboKotaLoad.value();
    cboKotaLoad.setDataSource(dsKotaLoad);
    cboKecLoad.text('');
    cboKecLoad.value();
    cboKecLoad.setDataSource();
    cboKelLoad.text('');
    cboKelLoad.value();
    cboKelLoad.setDataSource();
}

function OnKotaChangeLoad(e) {
    if (this.value() != "") {
        setKecamatanLoad(this.value());
    }
    else {
        cboKecLoad.text('');
        cboKecLoad.value();
        cboKecLoad.setDataSource();
        cboKelLoad.text('');
        cboKelLoad.value();
        cboKelLoad.setDataSource();
    }
    $('#LatLoadingAdd').data("kendoNumericTextBox").value('');
    $('#LongLoadingAdd').data("kendoNumericTextBox").value('');
}

function setKecamatanLoad(idParent) {
    dsKecLoad = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetChildLoc?idParent=' + idParent,
                dataType: "json"
            },
        },
    });
    cboKecLoad.text('');
    cboKecLoad.value();
    cboKecLoad.setDataSource(dsKecLoad);
    cboKelLoad.text('');
    cboKelLoad.value();
    cboKelLoad.setDataSource();
}

function OnKecamatanChangeLoad(e) {
    if (this.value() != "") {
        setKelurahanLoad(this.value());
    }
    else {
        //kosongkan semua
        cboKelLoad.text('');
        cboKelLoad.value();
        cboKelLoad.setDataSource();
    }
    $('#LatLoadingAdd').data("kendoNumericTextBox").value('');
    $('#LongLoadingAdd').data("kendoNumericTextBox").value('');
}

function setKelurahanLoad(idParent) {
    dsKelLoad = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetChildLoc?idParent=' + idParent,
                dataType: "json"
            },
        },
    });
    cboKelLoad.text('');
    cboKelLoad.value();
    cboKelLoad.setDataSource(dsKelLoad);
}

function OnKelurahanChangeLoad(e) {
    $('#LatLoadingAdd').data("kendoNumericTextBox").value('');
    $('#LongLoadingAdd').data("kendoNumericTextBox").value('');
}

function addMapLatLongAddLoad() {
    if (newLong != '')
        $("#valLoadingLong").hide();
    if (newLat != '')
        $("#valLoadingLat").hide();

    $('#LatLoadingAdd').data("kendoNumericTextBox").value(newLat);
    $('#LongLoadingAdd').data("kendoNumericTextBox").value(newLong);
    $('#modalviewmapaddressload').modal('hide');
}