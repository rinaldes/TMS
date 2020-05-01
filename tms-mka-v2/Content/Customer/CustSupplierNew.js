var currentTr;
var modalSupplier;
var formSupplier;
var validatorSupplier;

var cboProvinsiSupplier;
var cboKotaSupplier;
var cboKecSupplier;
var cboKelSupplier;

var dsProvinsiSupplier;
var dsKotaSupplier;
var dsKecSupplier;
var dsKelSupplier;

var GridCustSupp;
$(document).ready(function () {
    var dsGridSupp = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/BindingSupplier?idCust=' + $("#Id").val(),
                dataType: "json"
            },
        },
        schema: {
            data: "data",
            model: {
                fields: {
                    Id: { type: "number" },
                    CustomerId: { type: "number" },
                    Nama: { type: "string" },
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
                    Pic: { type:"string" }
                }
            }
        },
    });

    GridCustSupp = $("#GridCustSupp").kendoGrid({
        dataSource: dsGridSupp,
        columns: [
            {
                command: [
                    {
                        name: "edit",
                        text: "edit",
                        click: editSupp,
                        imageClass: "glyphicon glyphicon-edit",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=imageClass#"></span></a>'
                    },
                    {
                        name: "delete",
                        text: "delete",
                        click: deleteSupp,
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
                field: "Nama",
                title: "Nama"
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
            },
            {
                field: "Pic",
                title: "PIC"
            }
        ],
    }).data("kendoGrid");

    modalSupplier = $("#modalFormSupplier");
    formSupplier = $("#formSupplier");

    validatorSupplier = formSupplier.kendoValidator({
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
    dsProvinsiSupplier = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetProvinsi',
                dataType: "json"
            },
        },
    });
    cboProvinsiSupplier = $("#ProvinsiSupplier").kendoComboBox({
        dataTextField: "Nama",
        dataValueField: "Id",
        dataSource: dsProvinsiSupplier,
        filter: "contains",
        suggest: true,
        //change: OnProvChangeSupplier,
    }).data("kendoComboBox");
    cboKotaSupplier = $("#KotaSupplier").kendoComboBox({
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
        //change: OnKotaChangeSupplier,
    }).data("kendoComboBox");
    cboKecSupplier = $("#KecamatanSupplier").kendoComboBox({
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
        //change: OnKecamatanChangeSupplier,
    }).data("kendoComboBox");
    cboKelSupplier = $("#KelurahanSupplier").kendoComboBox({
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
        //change: OnKelurahanChangeSupplier
    }).data("kendoComboBox");

    //$("#TlpSupplier").kendoMaskedTextBox({
    //    mask: "000-000-000-000"
    //});
    //$("#FaxSupplier").kendoMaskedTextBox({
    //    mask: "000-000-000-000"
    //});
});

function ShowSupplierPopup() {
    $('.k-invalid-msg').hide();
    $('#CustomerSupplierId').val(0);
    $('#KodeSupplier').val("");
    $('#NamaSupplier').val("");
    $('#AlamatSupplier').val("");
    $('#KodeTlpSupplier').val("");
    $('#KodeFaxSupplier').val("");
    $('#TlpSupplier').val("");
    $('#FaxSupplier').val("");
    $('#ZonaSupplier').val("");
    cboProvinsiSupplier.value();
    cboProvinsiSupplier.text('');
    cboKotaSupplier.value();
    cboKotaSupplier.text('');
    cboKecSupplier.value();
    cboKecSupplier.text('');
    cboKelSupplier.value();
    cboKelSupplier.text('');
    $('#LatSupplier').data("kendoNumericTextBox").value('');
    $('#LongSupplier').data("kendoNumericTextBox").value('');
    $('#RadiusSupplier').data("kendoNumericTextBox").value('');
    $('#PicSupplier').val("");
    $('#NamaSupplier').val("");
}

function SaveSupplier(conditon) {
    if (validatorSupplier.validate())
    {
        var data = {
            Id: $('#CustomerSupplierId').val(),
            CustomerId: $("#Id").val(),
            Code: $('#KodeSupplier').val(),
            Nama: $('#NamaSupplier').val(),
            Alamat: $('#AlamatSupplier').val(),
            IdProvinsi: cboProvinsiSupplier.value(),
            IdKabKota: !isNaN(cboKotaSupplier.value()) ? cboKotaSupplier.value() : cboKotaSupplier.value() == '' ? cboKotaSupplier.value() : -1,
            IdKec: !isNaN(cboKecSupplier.value()) ? cboKecSupplier.value() : cboKecSupplier.value() == '' ? cboKecSupplier.value() : -1,
            IdKel: !isNaN(cboKelSupplier.value()) ? cboKelSupplier.value() : cboKelSupplier.value() == '' ? cboKelSupplier.value() : -1,
            Latitude: $('#LatSupplier').val(),
            Longitude: $('#LongSupplier').val(),
            Radius: $('#RadiusSupplier').data("kendoNumericTextBox").value(),
            Zona: $('#ZonaSupplier').val(),
            Telp: $('#KodeTlpSupplier').val() + ' - ' + $('#TlpSupplier').val(),
            Fax: $('#KodeFaxSupplier').val() + ' - ' + $('#FaxSupplier').val(),
            Pic: $('#PicSupplier').val(),
            Nama: $('#NamaSupplier').val()
        };

        goToSavePage("/Customer/CustomerSaveSupplier/", data, GridCustSupp.dataSource);
        modalSupplier.modal('hide');
    }
}

function deleteSupp(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    goToDeletePage("/Customer/CustomerDeleteSupp?CustId=" + $("#Id").val() + "&id=" + data.Id, this.dataSource);
}

function editSupp(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    //init combobox
    //setKotaSupplier(data.IdProvinsi);
    //setKecamatanSupplier(data.IdKabKota);
    //setKelurahanSupplier(data.IdKec);

    $('.k-invalid-msg').hide();
    $('#CustomerSupplierId').val(data.Id);
    $('#KodeSupplier').val(data.Code);
    $('#NamaSupplier').val(data.Code);
    $('#AlamatSupplier').val(data.Alamat);
    var tel = data.Telp.split(' - ');
    var fax = data.Fax.split(' - ');
    $('#KodeTlpSupplier').data('kendoMaskedTextBox').value(tel[0]);
    $('#KodeFaxSupplier').data('kendoMaskedTextBox').value(fax[0]);
    $('#TlpSupplier').data('kendoMaskedTextBox').value(tel[1]);
    $('#FaxSupplier').data('kendoMaskedTextBox').value(fax[1]);
    $('#PicSupplier').val(data.Pic);
    $('#ZonaSupplier').val(data.Zona);
    cboProvinsiSupplier.value(data.IdProvinsi);
    //cboProvinsiSupplier.text(data.provinsi);
    cboKotaSupplier.value(data.IdKabKota);
    cboKotaSupplier.text(data.Kota);
    cboKecSupplier.value(data.IdKec);
    cboKecSupplier.text(data.Kecamatan);
    cboKelSupplier.value(data.IdKel);
    cboKelSupplier.text(data.Kelurahan);
    $('#LongSupplier').data("kendoNumericTextBox").value(data.Longitude);
    $('#LatSupplier').data("kendoNumericTextBox").value(data.Latitude);
    $('#RadiusSupplier').data("kendoNumericTextBox").value(data.Radius);
    $('#NamaSupplier').val(data.Nama);
    modalSupplier.modal('show');
}

function OnProvChangeSupplier(e) {
    if (this.value() != "") {
        setKotaSupplier(this.value());
    }
    else {
        cboKotaSupplier.text('');
        cboKotaSupplier.value();
        cboKotaSupplier.setDataSource();
        cboKecSupplier.text('');
        cboKecSupplier.value();
        cboKecSupplier.setDataSource();
        cboKelSupplier.text('');
        cboKelSupplier.value();
        cboKelSupplier.setDataSource();
    }
    $('#LatSupplier').data("kendoNumericTextBox").value('');
    $('#LongSupplier').data("kendoNumericTextBox").value('');
}

function setKotaSupplier(idParent) {
    dsKotaSupplier = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetChildLoc?idParent=' + idParent,
                dataType: "json"
            },
        },
    });
    cboKotaSupplier.text('');
    cboKotaSupplier.value();
    cboKotaSupplier.setDataSource(dsKotaSupplier);
    cboKecSupplier.text('');
    cboKecSupplier.value();
    cboKecSupplier.setDataSource();
    cboKelSupplier.text('');
    cboKelSupplier.value();
    cboKelSupplier.setDataSource();
}

function OnKotaChangeSupplier(e) {
    if (this.value() != "") {
        setKecamatanSupplier(this.value());
    }
    else {
        cboKecSupplier.text('');
        cboKecSupplier.value();
        cboKecSupplier.setDataSource();
        cboKelSupplier.text('');
        cboKelSupplier.value();
        cboKelSupplier.setDataSource();
    }
    $('#LatSupplier').data("kendoNumericTextBox").value('');
    $('#LongSupplier').data("kendoNumericTextBox").value('');
}

function setKecamatanSupplier(idParent) {
    dsKecSupplier = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetChildLoc?idParent=' + idParent,
                dataType: "json"
            },
        },
    });
    cboKecSupplier.text('');
    cboKecSupplier.value();
    cboKecSupplier.setDataSource(dsKecSupplier);
    cboKelSupplier.text('');
    cboKelSupplier.value();
    cboKelSupplier.setDataSource();
}

function OnKecamatanChangeSupplier(e) {
    if (this.value() != "") {
        setKelurahanSupplier(this.value());
    }
    else {
        //kosongkan semua
        cboKelSupplier.text('');
        cboKelSupplier.value();
        cboKelSupplier.setDataSource();
    }
    $('#LatSupplier').data("kendoNumericTextBox").value('');
    $('#LongSupplier').data("kendoNumericTextBox").value('');
}

function setKelurahanSupplier(idParent) {
    dsKelSupplier = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetChildLoc?idParent=' + idParent,
                dataType: "json"
            },
        },
    });
    cboKelSupplier.text('');
    cboKelSupplier.value();
    cboKelSupplier.setDataSource(dsKelSupplier);
}

function OnKelurahanChangeSupplier(e) {
    $('#LatSupplier').data("kendoNumericTextBox").value('');
    $('#LongSupplier').data("kendoNumericTextBox").value('');
}

function addMapLatLongSupplier() {
    if (newLong != '')
        $("#valSupplierLong").hide();
    if (newLat != '')
        $("#valSupplierLat").hide();

    $('#LatSupplier').data("kendoNumericTextBox").value(newLat);
    $('#LongSupplier').data("kendoNumericTextBox").value(newLong);
    $('#modalviewmapSupplier').modal('hide');
}