var currentTr;
var modalAddress;
var formAddress;
var validatorAdd;

var cboProvinsi;
var cboKota;
var cboKec;
var cboKel;
var cboOffice;

var dsProvinsi;
var dsKota;
var dsKec;
var dsKel;

var GridCustAdd;
$(document).ready(function () {
    //grid
    var dsGridAdd = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/BindingAddress?idCust=' + $("#Id").val(),
                dataType: "json"
            },
        },
        schema: {
            data: "data",
            model: {
                fields: {
                    "Id": { type: "number" },
                    "CustomerId" : { type: "number" },
                    "Code" : { type: "string" },
                    "Alamat" : { type: "string" },
                    "IdProvinsi" : { type: "number" },
                    "provinsi" : { type: "string" },
                    "IdKabKota" : { type: "number" },
                    "kota" : { type: "string" },
                    "IdKec" : { type: "number" },
                    "kecamatan" : { type: "string" },
                    "IdKel" : { type: "number" },
                    "kelurahan" : { type: "string" },
                    "Longitude" : { type: "string" },
                    "Latitude" : { type: "string" },
                    "Radius" : { type: "string" },
                    "Zona" : { type: "string" },    
                    "OfficeTypeId" : { type: "number" },
                    "office" : { type: "string" },
                    "Telp" : { type: "string" },
                    "Fax": { type: "string" }
                }
            }
        },
    });

    GridCustAdd = $("#GridCustAddress").kendoGrid({
        dataSource: dsGridAdd,
        columns: [
            {
                command: [
                    {
                        name: "edit",
                        text: "edit",
                        click: editAddress,
                        imageClass: "glyphicon glyphicon-edit",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=imageClass#"></span></a>'
                    },
                    {
                        name: "delete",
                        text: "delete",
                        click: deleteAddress,
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
                field: "kelurahan",
                title: "Kelurahan/Desa"
            },
            {
                field: "kecamatan",
                title: "Kecamatan"
            },
            {
                field: "kota",
                title: "Kabupaten/Kota"
            },
            {
                field: "provinsi",
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
            },
            {
                field: "office",
                title: "Office"
            },
        ],
    }).data("kendoGrid");

    modalAddress = $("#modalFormAddress");
    formAddress = $("#formAddress");

    validatorAdd = formAddress.kendoValidator({
        rules: {
            comboreq: function (input) {
                if (input.is("[data-comboreq-msg]") && $(input).data("kendoComboBox"))
                {
                    if ($(input).data("kendoComboBox").selectedIndex == -1)
                    {
                        return false;
                    }
                }
                
                return true;
            },
        }
    }).data("kendoValidator");

    //location
    dsProvinsi = new kendo.data.DataSource({
        //serverFiltering: true,
        transport: {
            read: {
                url: '/Customer/GetProvinsi',
                dataType: "json"
            },
        },
    });
    cboProvinsi = $("#ProvinsiAdd").kendoComboBox({
        dataTextField: "Nama",
        dataValueField: "Id",
        dataSource: dsProvinsi,
        filter: "contains",
        suggest: true,
        //change: OnProvChange,
    }).data("kendoComboBox");
    cboKota = $("#KotaAdd").kendoComboBox({
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
        //change: OnKotaChange,
    }).data("kendoComboBox");
    cboKec = $("#KecamatanAdd").kendoComboBox({
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
        //change: OnKecamatanChange,
    }).data("kendoComboBox");
    cboKel = $("#KelurahanAdd").kendoComboBox({
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
        //change: OnKelurahanChange
    }).data("kendoComboBox");
    //office type
    dsOffice = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetOfficeType',
                dataType: "json"
            },
        },
    });
    cboOffice = $("#OfficeAdd").kendoComboBox({
        dataSource: dsOffice,
        dataTextField: "Nama",
        dataValueField: "Id",
        filter: "contains",
        suggest: true
    }).data("kendoComboBox");

    //$("#TlpAdd").kendoMaskedTextBox({
    //    mask: "000-000-000-000"

    //});
    //$("#FaxAdd").kendoMaskedTextBox({
    //    mask: "000-000-000-000"
    //});
});

function ShowAddressPopup() {
    $('.k-invalid-msg').hide();
    $('#CustomerAddressId').val(0);
    $('#KodeAdd').val("");
    $('#AlamatAdd').val("");
    $('#KodeTlpAdd').val("");
    $('#KodeFaxAdd').val("");
    $('#TlpAdd').val("");
    $('#FaxAdd').val("");
    cboOffice.value();
    cboOffice.text('');
    $('#ZonaAdd').val("");
    cboProvinsi.value();
    cboProvinsi.text('');
    cboKota.value();
    cboKota.text('');
    cboKec.value();
    cboKec.text('');
    cboKel.value();
    cboKel.text('');
    $('#LatAdd').data("kendoNumericTextBox").value('');
    $('#LongAdd').data("kendoNumericTextBox").value('');
    $('#RadiusAdd').data("kendoNumericTextBox").value('');
}

function SaveAdd() {
    if (validatorAdd.validate())
    {
        var data = {
            Id: $('#CustomerAddressId').val(),
            CustomerId: $("#Id").val(),
            Code: $('#KodeAdd').val(),
            Alamat: $('#AlamatAdd').val(),
            IdProvinsi:  cboProvinsi.value(),
            IdKabKota: !isNaN(cboKota.value()) ? cboKota.value() : cboKota.value() == '' ? cboKota.value() : -1,
            IdKec: !isNaN(cboKec.value()) ? cboKec.value() : cboKec.value() == '' ? cboKec.value() : -1,
            IdKel: !isNaN(cboKel.value()) ? cboKel.value() : cboKel.value() == '' ? cboKel.value() : -1,
            Latitude: $('#LatAdd').val(),
            Longitude: $('#LongAdd').val(),
            Radius: $('#RadiusAdd').data("kendoNumericTextBox").value(),
            Zona: $('#ZonaAdd').val(),
            OfficeTypeId: cboOffice.value(),
            Telp: $('#KodeTlpAdd').val() + ' - ' + $('#TlpAdd').val(),
            Fax: $('#KodeFaxAdd').val() + ' - ' + $('#FaxAdd').val()
        };

        goToSavePage("/Customer/CustomerSaveAddress/", data, GridCustAdd.dataSource);
        modalAddress.modal('hide');
    }
}

function deleteAddress(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    goToDeletePage("/Customer/CustomerDeleteAddress?CustId=" + $("#Id").val() + "&id=" + data.Id, this.dataSource);
}

function editAddress(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    //init combobox
    //setKota(data.IdProvinsi);
    //setKecamatan(data.IdKabKota);
    //setKelurahan(data.IdKec);

    $('.k-invalid-msg').hide();
    $('#CustomerAddressId').val(data.Id);
    $('#KodeAdd').val(data.Code);
    $('#AlamatAdd').val(data.Alamat);
    var tel = data.Telp.split(' - ');
    var fax = data.Fax.split(' - ');
    $('#KodeTlpAdd').data('kendoMaskedTextBox').value(tel[0]);
    $('#KodeFaxAdd').data('kendoMaskedTextBox').value(fax[0]);
    $('#TlpAdd').data('kendoMaskedTextBox').value(tel[1]);
    $('#FaxAdd').data('kendoMaskedTextBox').value(fax[1]);
    cboOffice.value(data.OfficeTypeId);
    $('#ZonaAdd').val(data.Zona);
    cboProvinsi.value(data.IdProvinsi);
    //cboProvinsi.text(data.provinsi);
    cboKota.value(data.IdKabKota);
    cboKota.text(data.kota);
    cboKec.value(data.IdKec);
    cboKec.text(data.kecamatan);
    cboKel.value(data.IdKel);
    cboKel.text(data.kelurahan);
    $('#LongAdd').data("kendoNumericTextBox").value(data.Longitude);
    $('#LatAdd').data("kendoNumericTextBox").value(data.Latitude);
    $('#RadiusAdd').data("kendoNumericTextBox").value(data.Radius);
    modalAddress.modal('show');
}

function OnProvChange(e) {
    if (this.value() != "") {
        setKota(this.value());
    }
    else {
        cboKota.text('');
        cboKota.value();
        cboKota.setDataSource();
        cboKec.text('');
        cboKec.value();
        cboKec.setDataSource();
        cboKel.text('');
        cboKel.value();
        cboKel.setDataSource();
    }
    $('#LatAdd').data("kendoNumericTextBox").value('');
    $('#LongAdd').data("kendoNumericTextBox").value('');
}

function setKota(idParent) {
    dsKota = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetChildLoc?idParent=' + idParent,
                dataType: "json"
            },
        },
    });
    cboKota.text('');
    cboKota.value();
    cboKota.setDataSource(dsKota);
    cboKec.text('');
    cboKec.value();
    cboKec.setDataSource();
    cboKel.text('');
    cboKel.value();
    cboKel.setDataSource();
}

function OnKotaChange(e) {
    if (this.value() != "") {
        setKecamatan(this.value());
    }
    else {
        cboKec.text('');
        cboKec.value();
        cboKec.setDataSource();
        cboKel.text('');
        cboKel.value();
        cboKel.setDataSource();
    }
    $('#LatAdd').data("kendoNumericTextBox").value('');
    $('#LongAdd').data("kendoNumericTextBox").value('');
}

function setKecamatan(idParent) {
    dsKec = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetChildLoc?idParent=' + idParent,
                dataType: "json"
            },
        },
    });
    cboKec.text('');
    cboKec.value();
    cboKec.setDataSource(dsKec);
    cboKel.text('');
    cboKel.value();
    cboKel.setDataSource();
}

function OnKecamatanChange(e) {
    if (this.value() != "") {
        setKelurahan(this.value());
    }
    else {
        //kosongkan semua
        cboKel.text('');
        cboKel.value();
        cboKel.setDataSource();
    }
    $('#LatAdd').data("kendoNumericTextBox").value('');
    $('#LongAdd').data("kendoNumericTextBox").value('');
}

function setKelurahan(idParent) {
    dsKel = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetChildLoc?idParent=' + idParent,
                dataType: "json"
            },
        },
    });
    cboKel.text('');
    cboKel.value();
    cboKel.setDataSource(dsKel);
}

function OnKelurahanChange(e) {
    $('#LatAdd').data("kendoNumericTextBox").value('');
    $('#LongAdd').data("kendoNumericTextBox").value('');
}

function addMapLatLongAdd() {
    if (newLong != '')
        $("#valLong").hide();
    if (newLat != '')
        $("#valLat").hide();

    $('#LatAdd').data("kendoNumericTextBox").value(newLat);
    $('#LongAdd').data("kendoNumericTextBox").value(newLong);
    $('#modalviewmapaddress').modal('hide');
}