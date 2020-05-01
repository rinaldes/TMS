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

$(document).ready(function () {
    modalLoadingAddress = $("#modalFormLoadingAddress");
    formLoadingAddress = $("#formLoadingAddress");

    validatorLoadingAdd = formLoadingAddress.kendoValidator({
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
        change: OnProvChangeLoad,
    }).data("kendoComboBox");
    cboKotaLoad = $("#KotaLoadingAdd").kendoComboBox({
        dataTextField: "Nama",
        dataValueField: "Id",
        filter: "contains",
        suggest: true,
        change: OnKotaChangeLoad,
    }).data("kendoComboBox");
    cboKecLoad = $("#KecamatanLoadingAdd").kendoComboBox({
        dataTextField: "Nama",
        dataValueField: "Id",
        filter: "contains",
        suggest: true,
        change: OnKecamatanChangeLoad,
    }).data("kendoComboBox");
    cboKelLoad = $("#KelurahanLoadingAdd").kendoComboBox({
        dataTextField: "Nama",
        dataValueField: "Id",
        filter: "contains",
        suggest: true,
        change: OnKelurahanChangeLoad
    }).data("kendoComboBox");

    $("#TlpLoadingAdd").kendoMaskedTextBox({
        mask: "000-000-000-000"
    });
    $("#FaxLoadingAdd").kendoMaskedTextBox({
        mask: "000-000-000-000"
    });
});

function updateRowNumberLoadingAdd() {
    $('td.no_Loadingadress').each(function (i) {
        $(this).text(i + 1);
    });
}

function ShowLoadingAddressPopup() {
    $('.k-invalid-msg').hide();
    $('#Condition').val("new");
    $('#CustomerAddressId').val(0);
    $('#KodeLoadingAdd').val("");
    $('#AlamatLoadingAdd').val("");
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

function SaveAddLoading(conditon) {
    var markup = "";
    if (validatorLoadingAdd.validate()) {
        //validasi tambahan
        var existedData = [];
        var tdContent = "";
        $("#table-custloadaddr tr").each(function (index, e) {
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

        markup = "<input id='LoadingAddressId' type='hidden' value='0'><td style='display:none;'><input value='" + $('#KodeLoadingAdd').val() + ";" + $('#AlamatLoadingAdd').val() + ";"
            + $('#ZonaLoadingAdd').val() + ";" + cboProvinsiLoad.value() + ";" + cboKotaLoad.value() + ";" + cboKecLoad.value() + ";" + cboKelLoad.value() + ";" + $('#LongLoadingAdd').val() + ";"
            + $('#LatLoadingAdd').val() + ";" + $('#RadiusLoadingAdd').val() + ";" + $('#TlpLoadingAdd').val() + ";" + $('#FaxLoadingAdd').val() + "' name='listLoadingAddress' /></td>" +
        "<td class='no_Loadingadress'></td><td>" + $('#KodeLoadingAdd').val() + "</td><td>" + $('#AlamatLoadingAdd').val() + "</td><td>" + $('#ZonaLoadingAdd').val() + "</td><td>" + cboKelLoad.text() + "</td>" +
        "<td>" + cboKecLoad.text() + "</td><td>" + cboKotaLoad.text() + "</td><td>" + cboProvinsiLoad.text() + "</td><td>" + $('#LongLoadingAdd').val() + "</td><td>" + $('#LatLoadingAdd').val() + "</td>" +
        "<td>" + $('#RadiusLoadingAdd').val() + "</td><td>" + $('#TlpLoadingAdd').val() + "</td><td>" + $('#FaxLoadingAdd').val() + "</td>" +
        "<td><a href='#' data-toggle='modal' data-target='#modalformLoadingAddress' onclick='EditLoadingAddressRow($(this))'>Edit</a> | <a href='#' onclick='RemoveLoadingAddressRow($(this));'>Delete</a></td>";

        if ($.inArray($('#KodeLoadingAdd').val(), existedData) != -1)
            alert("Kode sudah digunakan.");
        else {
            if (conditon == "new")
                $("#table-custloadaddr tbody").append("<tr>" + markup + "</tr>");
            else
                currentTr.closest("tr").html(markup);

            updateRowNumberLoadingAdd();
            modalLoadingAddress.modal('hide');
        }
    }
}

function EditLoadingAddressRow(data) {
    setKotaLoad(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[3]);
    setKecamatanLoad(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[4]);
    setKelurahanLoad(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[5]);

    $('.k-invalid-msg').hide();
    $('#Condition').val("edit");
    $('#CustomerLoadingAddressId').val(data.closest("tr").find('input').val());
    $('#KodeLoadingAdd').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[0]);
    $('#AlamatLoadingAdd').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[1]);
    $('#TlpLoadingAdd').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[10]);
    $('#FaxLoadingAdd').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[11]);
    $('#ZonaLoadingAdd').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[2]);
    cboProvinsiLoad.value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[3]);
    cboKotaLoad.value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[4]);
    cboKecLoad.value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[5]);
    cboKelLoad.value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[6]);
    $('#LongLoadingAdd').data("kendoNumericTextBox").value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[7]);
    $('#LatLoadingAdd').data("kendoNumericTextBox").value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[8]);
    $('#RadiusLoadingAdd').data("kendoNumericTextBox").value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[9]);

    currentTr = data;
}

function RemoveLoadingAddressRow(data) {
    data.closest("tr").remove();
    updateRowNumberLoadingAdd();
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