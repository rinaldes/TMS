var currentTr;
var modalPic;
var formPic;
var validatorPic;

var cboDept;
var cboJabatan;

var dsDept;
var dsJabatan;

$(document).ready(function () {
    modalPic = $("#modalFormPic");
    formPic = $("#formPic");

    validatorPic = formPic.kendoValidator().data("kendoValidator");

    //combobox
    dsDept = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetDepartment',
                dataType: "json"
            },
        },
    });

    var dsJabatan = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetJabatan',
                dataType: "json"
            },
        },
    });

    cboDept = $("#DepartemenId").kendoComboBox({
        dataTextField: "Nama",
        dataValueField: "Id",
        dataSource: dsDept,
        filter: "contains",
        suggest: true,
        index: 3
    }).data("kendoComboBox");

    cboJabatan = $("#JabatanId").kendoComboBox({
        dataTextField: "Nama",
        dataValueField: "Id",
        dataSource: dsJabatan,
        filter: "contains",
        suggest: true,
        index: 3
    }).data("kendoComboBox");

    //$("#MobilePic").kendoMaskedTextBox({
    //    mask: "000-000-000-000"
    //});
});

function updateRowNumberPic() {
    $('td.no_pic').each(function (i) {
        $(this).text(i + 1);
    });
}

function ShowPicPopup() {
    $('.k-invalid-msg').hide();
    $('#Condition').val("new");
    $('#CustomerPicId').val(0);
    $('#Name').val("");
    cboDept.value();
    cboDept.text('');
    cboJabatan.value('');
    cboJabatan.text();
    $('#EmailAdd').val("");
    $('#MobilePic').val("");
}

function SavePic(conditon) {
    var markup = "";
    if (validatorPic.validate()) {

        markup = "<input id='PicId' name='PicId' type='hidden' value='0'><td style='display:none;'><input value='" + $('#CustomerPicId').val() + ";" + $('#Name').val() + ";" + cboDept.text() + ";" + cboJabatan.text() + ";" + $('#EmailAdd').val() + ";" + $('#MobilePic').val() + ";" + cboDept.value() + ";" + cboJabatan.value() + "' name='listPic'/></td>" +
        "<td class='no_pic'></td><td>" + $('#Name').val() + "</td><td>" + cboDept.text() + "</td><td>" + cboJabatan.text() + "</td><td>" + $('#EmailAdd').val() + "</td><td>" + $('#MobilePic').val() + "</td>" +
        "<td><a href='#' data-toggle='modal' data-target='#modalFormPic' onclick='EditPicRow($(this))'>Edit</a> | <a href='#' onclick='RemovePicRow($(this));'>Delete</a></td>";

        if (conditon == "new")
            $("#table-custpic tbody").append("<tr>" + markup + "</tr>");
        else
            currentTr.closest("tr").html(markup);

        updateRowNumberPic();
        modalPic.modal('hide');
    }
}

function EditPicRow(data) {
    $('#Condition').val("edit");
    $('#CustomerPicId').val(data.closest("tr").find('input').val());
    $('#Name').val(data.closest("tr").find('td:eq(2)').text().trim());
    var cboDept = $("#DepartemenId").data("kendoComboBox");
    cboDept.text(data.closest("tr").find('td:eq(3)').text().trim());
    var cboJabatan = $("#JabatanId").data("kendoComboBox");
    cboJabatan.text(data.closest("tr").find('td:eq(4)').text().trim());
    $('#EmailAdd').val(data.closest("tr").find('td:eq(5)').text().trim());
    $('#MobilePic').val(data.closest("tr").find('td:eq(6)').text().trim());
    currentTr = data;
}

function RemovePicRow(data) {
    data.closest("tr").remove();
    updateRowNumberPic();
}