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

$(document).ready(function () {
    modalAddress = $("#modalFormAddress");
    formAddress = $("#formAddress");

    validatorAdd = formAddress.kendoValidator({
        rules: {
            comboreq: function (input) {
                if ($(input).data("kendoComboBox"))
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
        change: OnProvChange,
    }).data("kendoComboBox");
    cboKota = $("#KotaAdd").kendoComboBox({
        dataTextField: "Nama",
        dataValueField: "Id",
        filter: "contains",
        suggest: true,
        change: OnKotaChange,
    }).data("kendoComboBox");
    cboKec = $("#KecamatanAdd").kendoComboBox({
        dataTextField: "Nama",
        dataValueField: "Id",
        filter: "contains",
        suggest: true,
        change: OnKecamatanChange,
    }).data("kendoComboBox");
    cboKel = $("#KelurahanAdd").kendoComboBox({
        dataTextField: "Nama",
        dataValueField: "Id",
        filter: "contains",
        suggest: true,
        change: OnKelurahanChange
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

    $("#TlpAdd").kendoMaskedTextBox({
        mask: "000-000-000-000"
    });
    $("#FaxAdd").kendoMaskedTextBox({
        mask: "000-000-000-000"
    });
});

function updateRowNumberAdd() {
    $('td.no_address').each(function (i) {
        $(this).text(i + 1);
    });
}

function ShowAddressPopup() {
    $('.k-invalid-msg').hide();
    $('#Condition').val("new");
    $('#CustomerAddressId').val(0);
    $('#KodeAdd').val("");
    $('#AlamatAdd').val("");
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

function SaveAdd(conditon) {
    var markup = "";
    var existedDataAdd = [];

    if (validatorAdd.validate()) {
        //validasi tambahan
        $(".table-custaddr tr").each(function (index, e) {
            if (index > 0) {
                tdContent = $(this).children('td').eq(0).text().trim();
                if (tdContent != currentTr.closest("tr").find('td:eq(0)').text().trim())
                    existedDataAdd.push(tdContent);
            }
        });

        markup = "<input id='AddressId' type='hidden' value='0'><td style='display:none;'><input value='" + $('#KodeAdd').val() + ";" + $('#AlamatAdd').val() + ";"
            + $('#ZonaAdd').val() + ";" + cboProvinsi.value() + ";" + cboKota.value() + ";" + cboKec.value() + ";" + cboKel.value() + ";" + $('#LongAdd').val() + ";"
            + $('#LatAdd').val() + ";" + $('#RadiusAdd').val() + ";" + $('#TlpAdd').val() + ";" + $('#FaxAdd').val() + ";" + cboOffice.value() + "' name='listAddress' /></td>" +
        "<td class='no_address'></td><td>" + $('#KodeAdd').val() + "</td><td>" + $('#AlamatAdd').val() + "</td><td>" + $('#ZonaAdd').val() + "</td><td>" + cboKel.text() + "</td>" +
        "<td>" + cboKec.text() + "</td><td>" + cboKota.text() + "</td><td>" + cboProvinsi.text() + "</td><td>" + $('#LongAdd').val() + "</td><td>" + $('#LatAdd').val() + "</td>" +
        "<td>" + $('#RadiusAdd').val() + "</td><td>" + $('#TlpAdd').val() + "</td><td>" + $('#FaxAdd').val() + "</td><td>" + cboOffice.text() + "</td>" +
        "<td><a href='#' data-toggle='modal' data-target='#modalFormAddress' onclick='EditAddRow($(this))'>Edit</a> | <a href='#' onclick='RemoveAddRow($(this));'>Delete</a></td>";

        if ($.inArray($('#KodeAdd').val(), existedDataAdd) != -1)
            alert("Kode sudah digunakan.");
        else {
            if (conditon == "new")
                $("#table-custaddr tbody").append("<tr>" + markup + "</tr>");
            else
                currentTr.closest("tr").html(markup);

            updateRowNumberAdd();
            modalAddress.modal('hide');
        }
    }
}

function EditAddRow(data) {
    //init combobox
    setKota(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[3]);
    setKecamatan(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[4]);
    setKelurahan(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[5]);

    $('.k-invalid-msg').hide();
    $('#Condition').val("edit");
    $('#CustomerAddressId').val(data.closest("tr").find('input').val());
    $('#KodeAdd').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[0]);
    $('#AlamatAdd').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[1]);
    $('#TlpAdd').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[10]);
    $('#FaxAdd').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[11]);
    cboOffice.value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[12]);
    $('#ZonaAdd').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[2]);
    cboProvinsi.value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[3]);
    cboKota.value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[4]);
    cboKec.value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[5]);
    cboKel.value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[6]);
    $('#LongAdd').data("kendoNumericTextBox").value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[7]);
    $('#LatAdd').data("kendoNumericTextBox").value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[8]);
    $('#RadiusAdd').data("kendoNumericTextBox").value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[9]);
    currentTr = data;
}

function RemoveAddRow(data) {
    data.closest("tr").remove();
    updateRowNumberAdd();
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