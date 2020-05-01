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

$(document).ready(function () {
    modalSupplier = $("#modalFormSupplier");
    formSupplier = $("#formSupplier");

    validatorSupplier = formSupplier.kendoValidator({
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
        change: OnProvChangeSupplier,
    }).data("kendoComboBox");
    cboKotaSupplier = $("#KotaSupplier").kendoComboBox({
        dataTextField: "Nama",
        dataValueField: "Id",
        filter: "contains",
        suggest: true,
        change: OnKotaChangeSupplier,
    }).data("kendoComboBox");
    cboKecSupplier = $("#KecamatanSupplier").kendoComboBox({
        dataTextField: "Nama",
        dataValueField: "Id",
        filter: "contains",
        suggest: true,
        change: OnKecamatanChangeSupplier,
    }).data("kendoComboBox");
    cboKelSupplier = $("#KelurahanSupplier").kendoComboBox({
        dataTextField: "Nama",
        dataValueField: "Id",
        filter: "contains",
        suggest: true,
        change: OnKelurahanChangeSupplier
    }).data("kendoComboBox");

    $("#TlpSupplier").kendoMaskedTextBox({
        mask: "000-000-000-000"
    });
    $("#FaxSupplier").kendoMaskedTextBox({
        mask: "000-000-000-000"
    });
});

function updateRowNumberSupplier() {
    $('td.no_Supplier').each(function (i) {
        $(this).text(i + 1);
    });
}

function ShowSupplierPopup() {
    $('.k-invalid-msg').hide();
    $('#Condition').val("new");
    $('#CustomerSupplierId').val(0);
    $('#KodeSupplier').val("");
    $('#AlamatSupplier').val("");
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
    var markup = "";
    if (validatorSupplier.validate()) {
        //validasi tambahan
        var existedData = [];
        var tdContent = "";
        $("#table-custSupplier tr").each(function (index, e) {
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

        markup = "<input id='SupplierId' type='hidden' value='0'><td style='display:none;'><input value='" + $('#KodeSupplier').val() + ";" + $('#AlamatSupplier').val() + ";"
            + $('#ZonaSupplier').val() + ";" + cboProvinsiSupplier.value() + ";" + cboKotaSupplier.value() + ";" + cboKecSupplier.value() + ";" + cboKelSupplier.value() + ";" + $('#LongSupplier').val() + ";"
            + $('#LatSupplier').val() + ";" + $('#RadiusSupplier').val() + ";" + $('#TlpSupplier').val() + ";" + $('#FaxSupplier').val() + ";" + $('#PicSupplier').val() + ";" + $('#NamaSupplier').val() + "' name='listSupplier' /></td>" +
        "<td class='no_Supplier'></td><td>" + $('#KodeSupplier').val() + "</td><td>" + $('#NamaSupplier').val() + "</td><td>" + $('#AlamatSupplier').val() + "</td><td>" + $('#ZonaSupplier').val() + "</td><td>" + cboKel.text() + "</td>" +
        "<td>" + cboKecSupplier.text() + "</td><td>" + cboKotaSupplier.text() + "</td><td>" + cboProvinsiSupplier.text() + "</td><td>" + $('#LongSupplier').val() + "</td><td>" + $('#LatSupplier').val() + "</td>" +
        "<td>" + $('#RadiusSupplier').val() + "</td><td>" + $('#PicSupplier').val() + "</td><td>" + $('#TlpSupplier').val() + "</td><td>" + $('#FaxSupplier').val() + "</td>" +
        "<td><a href='#' data-toggle='modal' data-target='#modalFormSupplier' onclick='EditSupplierRow($(this))'>Edit</a> | <a href='#' onclick='RemoveSupplierRow($(this));'>Delete</a></td>";

        if ($.inArray($('#KodeSupplier').val(), existedData) != -1)
            alert("Kode sudah digunakan.");
        else {
            if (conditon == "new")
                $("#table-custsupplier tbody").append("<tr>" + markup + "</tr>");
            else
                currentTr.closest("tr").html(markup);

            updateRowNumberSupplier();
            modalSupplier.modal('hide');
        }
    }
}

function EditSupplierRow(data) {
    //init combobox
    setKotaSupplier(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[3]);
    setKecamatanSupplier(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[4]);
    setKelurahanSupplier(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[5]);

    $('.k-invalid-msg').hide();
    $('#Condition').val("edit");
    $('#CustomerSupplierId').val(data.closest("tr").find('input').val());
    $('#KodeSupplier').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[0]);
    $('#AlamatSupplier').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[1]);
    $('#TlpSupplier').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[10]);
    $('#FaxSupplier').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[11]);
    $('#PicSupplier').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[12]);
    $('#ZonaSupplier').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[2]);
    cboProvinsiSupplier.value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[3]);
    cboKotaSupplier.value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[4]);
    cboKecSupplier.value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[5]);
    cboKelSupplier.value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[6]);
    $('#LongSupplier').data("kendoNumericTextBox").value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[7]);
    $('#LatSupplier').data("kendoNumericTextBox").value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[8]);
    $('#RadiusSupplier').data("kendoNumericTextBox").value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[9]);
    $('#NamaSupplier').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[13]);
    currentTr = data;
}

function RemoveSupplierRow(data) {
    data.closest("tr").remove();
    updateRowNumberSupplier();
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