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

$(document).ready(function () {
    modalUnloadingAddress = $("#modalFormUnloadAddress");
    formUnloadingAddress = $("#formUnloadingAddress");

    validatorUnloadingAdd = formUnloadingAddress.kendoValidator({
        rules: {
            comboreq: function (input) {
                if ($(input).data("kendoComboBox")) {
                    if ($(input).data("kendoComboBox").selectedIndex == -1) {
                        return false;
                    }
                }

                return true;
            },
        }
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
        change: OnProvChangeUnloading,
    }).data("kendoComboBox");
    cboKotaUnloading = $("#KotaUnloadingAdd").kendoComboBox({
        dataTextField: "Nama",
        dataValueField: "Id",
        filter: "contains",
        suggest: true,
        change: OnKotaChangeUnloading,
    }).data("kendoComboBox");
    cboKecUnloading = $("#KecamatanUnloadingAdd").kendoComboBox({
        dataTextField: "Nama",
        dataValueField: "Id",
        filter: "contains",
        suggest: true,
        change: OnKecamatanChangeUnloading,
    }).data("kendoComboBox");
    cboKelUnloading = $("#KelurahanUnloadingAdd").kendoComboBox({
        dataTextField: "Nama",
        dataValueField: "Id",
        filter: "contains",
        suggest: true,
        change: OnKelurahanChangeUnloading
    }).data("kendoComboBox");

    $("#TlpUnloadingAdd").kendoMaskedTextBox({
        mask: "000-000-000-000"
    });
    $("#FaxUnloadingAdd").kendoMaskedTextBox({
        mask: "000-000-000-000"
    });
});

function updateRowNumberUnloadingAdd() {
    $('td.no_unloadingaddress').each(function (i) {
        $(this).text(i + 1);
    });
}

function ShowAddressUnloadingPopup() {
    $('.k-invalid-msg').hide();
    $('#Condition').val("new");
    $('#CustomerUnloadingAddressId').val(0);
    $('#KodeUnloadingAdd').val("");
    $('#AlamatUnloadingAdd').val("");
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
    var markup = "";

    if (validatorUnloadingAdd.validate()) {
        //validasi tambahan
        var existedData = [];
        var tdContent = "";
        $("#table-custaddrunloading tr").each(function (index, e) {
            if (index > 0) {
                tdContent = $(this).children('td').eq(2).text().trim();
                if (conditon == "new") {
                    existedData.push(tdContent);
                }
                else {
                    if (tdContent != currentTr.closest("tr").find('td:eq(2)').text().trim())
                        existedData.push(tdContent);
                }
            }
        });

        markup = "<input id='AddressId' type='hidden' value='0'><td style='display:none;'><input value='" + $('#KodeUnloadingAdd').val() + ";" + $('#AlamatUnloadingAdd').val() + ";"
            + $('#ZonaUnloadingAdd').val() + ";" + cboProvinsiUnloading.value() + ";" + cboKotaUnloading.value() + ";" + cboKecUnloading.value() + ";" + cboKelUnloading.value() + ";" + $('#LongUnloadingAdd').val() + ";"
            + $('#LatUnloadingAdd').val() + ";" + $('#RadiusUnloadingAdd').val() + ";" + $('#TlpUnloadingAdd').val() + ";" + $('#FaxUnloadingAdd').val() + "' name='listUnloadingAddress' /></td>" +
        "<td class='no_unloadingaddress'></td><td>" + $('#KodeUnloadingAdd').val() + "</td><td>" + $('#AlamatUnloadingAdd').val() + "</td><td>" + $('#ZonaUnloadingAdd').val() + "</td><td>" + cboKelUnloading.text() + "</td>" +
        "<td>" + cboKecUnloading.text() + "</td><td>" + cboKotaUnloading.text() + "</td><td>" + cboProvinsiUnloading.text() + "</td><td>" + $('#LongUnloadingAdd').val() + "</td><td>" + $('#LatUnloadingAdd').val() + "</td>" +
        "<td>" + $('#RadiusUnloadingAdd').val() + "</td><td>" + $('#TlpUnloadingAdd').val() + "</td><td>" + $('#FaxUnloadingAdd').val() + "</td>" +
        "<td><a href='#' data-toggle='modal' data-target='#modalFormUnloadAddress' onclick='EditUnloadAddRow($(this))'>Edit</a> | <a href='#' onclick='RemoveUnloadAddRow($(this));'>Delete</a></td>";

        if ($.inArray($('#KodeUnloadingAdd').val(), existedData) != -1)
            alert("Kode sudah digunakan.");
        else {
            if (conditon == "new")
                $("#table-custaddrunloading tbody").append("<tr>" + markup + "</tr>");
            else
                currentTr.closest("tr").html(markup);

            updateRowNumberUnloadingAdd();
            modalUnloadingAddress.modal('hide');
        }
    }
}

function EditUnloadAddRow(data) {
    //init combobox
    setKotaUnloading(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[3]);
    setKecamatanUnloading(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[4]);
    setKelurahanUnloading(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[5]);

    $('.k-invalid-msg').hide();
    $('#Condition').val("edit");
    $('#CustomerUnloadingAddressId').val(data.closest("tr").find('input').val());
    $('#KodeUnloadingAdd').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[0]);
    $('#AlamatUnloadingAdd').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[1]);
    $('#TlpUnloadingAdd').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[10]);
    $('#FaxUnloadingAdd').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[11]);
    $('#ZonaUnloadingAdd').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[2]);
    cboProvinsiUnloading.value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[3]);
    cboKotaUnloading.value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[4]);
    cboKecUnloading.value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[5]);
    cboKelUnloading.value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[6]);
    $('#LongUnloadingAdd').data("kendoNumericTextBox").value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[7]);
    $('#LatUnloadingAdd').data("kendoNumericTextBox").value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[8]);
    $('#RadiusUnloadingAdd').data("kendoNumericTextBox").value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[9]);
    currentTr = data;
}

function RemoveUnloadAddRow(data) {
    data.closest("tr").remove();
    updateRowNumberUnloadingAdd();
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