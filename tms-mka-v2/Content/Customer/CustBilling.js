var currentTr;
var modalBilling;
var formBilling;
var validatorBilling;

var gridJadwal;
var dsGrid = [];
var warnaArr = [];
var BillAtt;
var lembar;
var path = '';
var fname = '';
var kendoUploadButton;

var dsupload = [];
function onSelectFile(e) {
    var flagExtension = 0;
    var allowedExtension = [".jpg", ".jpeg", ".pdf"];
    $.each(e.files, function (index, value) {
        if ($.inArray((value.extension).toLowerCase(), allowedExtension) === -1) {
            flagExtension = 1;
        }
    });
    if (flagExtension == 1) {
        alert("File yang diperbolehkan hanya berupa jpg/jpeg, pdf");
        e.preventDefault();
    }
    $("#tempfile").hide();
    setTimeout(function () {
        kendoUploadButton = $(".k-upload-selected");
        kendoUploadButton.hide();
    }, 1);
}

function hariEditor(container, options) {
    $('<input data-text-field="text" data-value-field="value" data-bind="value:' + options.field + '"/>')
           .appendTo(container)
           .kendoDropDownList({
               dataSource: [
                            { text: "Senin", value: "Senin" },
                            { text: "Selasa", value: "Selasa" },
                            { text: "Rabu", value: "Rabu" },
                            { text: "Kamis", value: "Kamis" },
                            { text: "Jumat", value: "Jumat" },
                            { text: "Sabtu", value: "Sabtu" },
                            { text: "Minggu", value: "Minggu" }
               ],
               dataTextField: "text",
               dataValueField: "value"
           });
}

function jamEditor(container, options) {
    $('<input data-bind="value:' + options.field + '"/>')
           .appendTo(container)
           .kendoMaskedTextBox({
               mask: "00:00 - 00:00"
           });
}

function createUploader() {
    BillAtt = $('<input type="file" name="files" id="filesBilling" />').appendTo("#wrapper").kendoUpload({
        multiple: false,
        async: {
            saveUrl: "/FileManagement/Upload?Dir=~/Uploads",
            removeUrl: "/FileManagement/Delete?Dir=~/Uploads",
            autoUpload: false
        },
        files: dsupload,
        select: onSelectFile,
        success: function (e) {
            if (e.operation == "upload") {
                $(".k-filename:contains('" + e.files[0].name + "')").text('').append('<a href="' + e.response.imagelink + '" target="_blank">' + e.response.fileName + '</a>');
                e.files[0].name = e.response.fileName;
                fname = e.response.fileName;
                path = e.response.imagelink;
                alert("a")
            } else if (e.operation == "remove") {
                fname = e.response.fileName;
                path = e.response.imagelink;
            }
            //fname = e.response.fullname;
            //path = e.response.filepath;
        },
    }).data("kendoUpload");
}

$(document).ready(function () {
    modalBilling = $("#modalFormBilling");
    formBilling = $("#formBilling");

    validatorBilling = formBilling.kendoValidator({
        rules: {
            warnareq: function (input) {
                if (input.is("[name=Hidwarna]") && warnaArr.length == 0) {
                    return false;
                }

                return true;
            },
            faxreq: function (input) {
                if (input.is("[name=FaxBilling]") && $("#IsFax").is(':checked'))
                {
                    return input.val() != "";
                }

                return true;
            },
            emailreq: function (input) {
                if (input.is("[name=EmailBilling]") && $("#IsEmail").is(':checked')) {
                    return input.val() != "";
                }

                return true;
            }
        }
    }).data("kendoValidator");

    lembar = $("#Lembar").kendoNumericTextBox({
        format: "n0",
        spinner: false
    }).data("kendoNumericTextBox");

    createUploader();

    gridJadwal = $("#gridJadwal").kendoGrid({
        dataSource: {
            data: dsGrid,
            schema: {
                model: {
                    fields: {
                        Hari: { type: "string"},
                        Jam: { type: "string"},
                        Catatan: { type: "string" },
                        Email: { type: "string"}
                    }
                }
            },
        },
        editable: { mode: "inline", confirmation: "Anda yakin menghapus data?" },
        toolbar: [{ name: "create", text: "Tambah" }],
        columns: [
            {
                command: [
                    {
                        name: "edit",
                        text: "Edit",
                        imageClass: "glyphicon glyphicon-edit",
                        template: '<a class="k-button-icon #=className#" #=attr# title="edit" href="\\#"><span class="#=imageClass#"></span></a>'
                    },
                    {
                        name: "destroy",
                        text: "Hapus",
                        imageClass: "glyphicon glyphicon-remove",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#" title="#=text#"><span class="#=imageClass#"></span></a>'
                    }
                ],
                width: 90
            },
            {
                field: "Hari",
                title: "Hari",
                editor: hariEditor,
                width: 150
            },
            {
                field: "Jam",
                title: "Jam",
                editor: jamEditor,
                width: 150
            },
            {
                field: "Catatan",
                title: "Catatan"
            },
            {
                field: "Email",
                title: "Email",
                width: 250
            }
        ],
        edit: function (e) {
            var detailCell = e.container.find("td:first");
            detailCell.html('');
            var commandCell = e.container.find("td:nth-child(1)");
            commandCell.html('<a class="k-button-icon k-grid-update form-inline" href="\\#" title="Simpan"><span class="glyphicon glyphicon-floppy-saved"></span></a> <a class="k-button-icon k-grid-cancel form-inline" href="\\#" title="Batal"><span class="glyphicon glyphicon-remove"></span></a>');
            var commandGroupCell = e.container.find("td:nth-child(0)");
            commandGroupCell.html('');
        },
        save: function (e) {
            toastr.options = {
                "closeButton": false,
                "debug": false,
                "positionClass": "toast-top-center",
                "onclick": null,
                "showDuration": "1000",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
            if (e.model.Hari == "") {
                toastr["error"]("Hari harus diisi !", "ERROR")
                e.preventDefault();
            }else if (!TimeValidate(e.model.Jam.split("-")[0].trim()))
            {
                toastr["error"]("Format Jam Salah !", "ERROR")
                e.preventDefault();
            }else if (!TimeValidate(e.model.Jam.split("-")[1].trim())) {
                toastr["error"]("Format Jam Salah !", "ERROR")
                e.preventDefault();
            }else if (e.model.Email == "") {
                toastr["error"]("Email harus diisi !", "ERROR")
                e.preventDefault();
            }
        }
    }).data("kendoGrid");
    //listener checkbox warna
    $(".warna").on('click', function () {
        if ($(this).is(':checked'))
            warnaArr.push(this.value);
        else
            warnaArr.pop(this.value);
    });
    //listener checkbox fax
    $("#IsFax").on('click', function () {
        if ($(this).is(':checked')) {
            $("#FaxBilling").prop('readonly', false);
            $("#FaxBilling").val('');
        }
        else {
            $("#FaxBilling").prop('readonly', true);
            $("#FaxBilling").val('');
        }
    });
    //listener checkbox fax
    $("#IsEmail").on('click', function () {
        if ($(this).is(':checked')) {
            $("#EmailBilling").prop('readonly', false);
            $("#EmailBilling").val('');
        }
        else {
            $("#EmailBilling").prop('readonly', true);
            $("#EmailBilling").val('');
        }
    });

    $("#IsFaktur").on('click', function () {
        if ($(this).is(':checked')) {
            $("#gridJadwal").show();
        }
        else {
            $("#gridJadwal").hide();
        }
    });
});

function updateRowNumberBilling() {
    $('td.no_billing').each(function (i) {
        $(this).text(i + 1);
    });
}

function ShowBillingPopup() {
    $('.k-invalid-msg').hide();
    $('#Condition').val("new");
    dsupload = [];
    warnaArr = [];
    $('#Dok').val("");
    lembar.value('');
    $(".warna").prop('checked', false);
    //uploader
    dsupload = [];
    //BillAtt.destroy();
    $("#wrapper").empty();
    createUploader();
    //end uploader
    $("#IsFax").prop('checked', false);
    $("#IsEmail").prop('checked', false);
    $("#IsFaktur").prop('checked', false);
    $("#IsJasa").prop('checked', false);
    $("#gridJadwal").hide();
    gridJadwal.dataSource.data([]);
    $('#FaxBilling').val("");
    $('#EmailBilling').val("");
}

function SaveBilling(conditon) {
    var markup = "";
    $('.k-invalid-msg').hide();
    if (validatorBilling.validate()) {
        if (!$('#IsFaktur').is(':checked')) {
            dsGrid.length = 0;
            gridJadwal.dataSource.read();
        }

        gridJadwal.saveRow();
        var dataGrid = gridJadwal.dataSource.data();

        var strStampe;
        if ($('#rdStampe:checked').val() == "true")
            strStampe = "Ya";
        else
            strStampe = "Tidak";
        console.log(path)
        markup = "<input id='BillingId' name='BillingId' type='hidden' value='0'><td style='display:none;'><input value='" + $('#Dok').val() + ";" + lembar.value() + ";" + warnaArr + ";" + $('#rdStampe:checked').val() + ";" + path + ";" + fname + ";" + $('#IsFax').is(':checked') + ";" + $('#FaxBilling').val() + ";" + $('#IsEmail').is(':checked') + ";" + $('#EmailBilling').val() + ";" + $('#IsFaktur').is(':checked') + ";" + JSON.stringify(dataGrid) + ";" + $('#IsJasa').is(':checked') + "' name='listBilling' /></td>" +
            "<td class='no_billing'></td><td>" + $('#Dok').val() + "</td><td>" + lembar.value() + "</td><td>" + warnaArr + "</td><td>" + strStampe + "</td><td>" + "<a href='" + path.replace('~/', '/') + "' target='_blank'>" + fname + "</a></td><td><a href='#' data-toggle='modal' data-target='#modalFormBilling' onclick='EditBilling($(this))'>Edit</a> | <a href='#' onclick='RemoveBilling($(this));'>Delete</a></td>";
        if (conditon == "new")
            $("#table-custbilling tbody").append("<tr>" + markup + "</tr>");
        else
            currentTr.closest("tr").html(markup);

        updateRowNumberBilling();
        modalBilling.modal('hide');
    }
}

function EditBilling(data)
{
    $('.k-invalid-msg').hide();
    $('#Condition').val("edit");
    $('#CustomerBillingId').val(data.closest("tr").find('input').val());
    $('#Dok').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[0]);
    lembar.value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[1]);
    warnaArr = [];
    $(".warna").prop('checked', false);
    var warnaList = data.closest("tr").find('td:eq(0)').find('input').val().split(';')[2];
    var substr = warnaList.split(',');
    for (var i = 0; i < substr.length; i++) {
        warnaArr.push(substr[i]);
        $(":checkbox[value=" + substr[i] + "]").prop("checked", true);
    }
    $(":radio[id=rdStampe][value=" + data.closest("tr").find('td:eq(0)').find('input').val().split(';')[3].toLowerCase() + "]").prop("checked", true);


    
    fname = data.closest("tr").find('td:eq(0)').find('input').val().split(';')[5];
    $("#wrapper").empty();
    dsupload.push({ name: fname });
    createUploader();
    path = data.closest("tr").find('td:eq(0)').find('input').val().split(';')[4];

    if (data.closest("tr").find('td:eq(0)').find('input').val().split(';')[6].toLowerCase() == "true")
        $("#IsFax").prop("checked", true);
    else
        $("#IsFax").prop("checked", false);
    $("#FaxBilling").val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[7]);
    if (data.closest("tr").find('td:eq(0)').find('input').val().split(';')[8].toLowerCase() == "true")
        $("#IsEmail").prop("checked", true);
    else
        $("#IsEmail").prop("checked", false);
    $("#EmailBilling").val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[9]);
    if (data.closest("tr").find('td:eq(0)').find('input').val().split(';')[10].toLowerCase() == "true") {
        $("#IsFaktur").prop("checked", true);
        var dummyJadwal = $.parseJSON(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[11]);
        dsGrid.length = 0;
        for (var i = 0; i < dummyJadwal.length; i++) {
            dsGrid.push(dummyJadwal[i]);
        }
        gridJadwal.dataSource.read();
        $("#gridJadwal").show();
    }
    else {
        $("#IsFaktur").prop("checked", false);
        dsGrid.length = 0;
        gridJadwal.dataSource.read();
        $("#gridJadwal").hide();
    }
    if (data.closest("tr").find('td:eq(0)').find('input').val().split(';')[12].toLowerCase() == "true")
        $("#IsJasa").prop("checked", true);
    else
        $("#IsJasa").prop("checked", false);
    currentTr = data;
}

function RemoveBilling(data) {
    data.closest("tr").remove();
    updateRowNumberBilling();
}