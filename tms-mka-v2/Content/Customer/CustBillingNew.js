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

var GridCustBIlling;

var txtfax;
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

function CreateLink() {
    var filename = $("#filesBilling").data("kendoUpload").wrapper.find(".k-filename")
    for (var i = 0; i < filename.length; i++) {
        $(filename[i]).html("<a href='/Uploads/" + dsupload[i].name + "' target='_blank'>" + dsupload[i].name.replace(dsupload[i].name.substr(0, 36), '') + "</a>")
    }
}

function createUploader() {
    BillAtt = $('<input type="file" name="files" id="filesBilling" />').appendTo("#wrapper").kendoUpload({
        multiple: false,
        async: {
            saveUrl: "/FileManagement/Upload?Dir=~/Uploads",
            removeUrl: "/FileManagement/Delete?Dir=~/Uploads&temp=true",
            autoUpload: false
        },
        files: dsupload,
        select: onSelectFile,
        remove: function (e) {
            fname = "";
            path = "";
        },
        success: function (e) {
            if (e.operation == "upload") {
                $(".k-filename:contains('" + e.files[0].name + "')").text('').append('<a href="' + e.response.imagelink + '" target="_blank">' + e.response.fileName + '</a>');
                e.files[0].name = e.response.fileName;
                fname = e.response.fileName;
                path = e.response.imagelink;
                
                doSave();

                App.unblockUI("#formBilling");
            } else if (e.operation == "remove") {
                fname = "";
                path = "";
            }
        },
    }).data("kendoUpload");
}

$(document).ready(function () {
    //grid
    var dsGridBilling = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/BindingBilling?idCust=' + $("#Id").val(),
                dataType: "json"
            },
        },
        schema: {
            data: "data",
            model: {
                fields: {
                    Id: { type: "number" },
                    CustomerId: { type: "number" },
                    DocumentName: { type: "string" },
                    Lembar: { type: "number" },
                    Warna: { type: "string" },
                    Stempel: { type: "boolan" },
                    IsFax: { type: "boolean" },
                    Fax: { type: "string" },
                    IsEmail: { type: "boolean" },
                    Email: { type: "string" },
                    IsTukarFaktur: { type: "boolean" },
                    IsJasaPengiriman: { type: "boolean" },
                    UrlAtt: { type: "string" },
                    FileName: { type: "string" },
                    srtDataJadwal: { type: "string" }
                }
            }
        },
    });

    GridCustBilling = $("#GridCustBilling").kendoGrid({
        dataSource: dsGridBilling,
        columns: [
            {
                command: [
                    {
                        name: "edit",
                        text: "edit",
                        click: editBilling,
                        imageClass: "glyphicon glyphicon-edit",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=imageClass#"></span></a>'
                    },
                    {
                        name: "delete",
                        text: "delete",
                        click: deleteBilling,
                        imageClass: "glyphicon glyphicon-remove",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=iconClass# #=imageClass#"></span></a>'
                    }
                ],
                width: "60px"
            },
            {
                field: "DocumentName",
                title: "Nama Dokumen"
            },
            {
                field: "Lembar",
                title: "Lembar",
                width: "60px"
            },
            {
                field: "Warna",
                title: "Warna"
            },
            {
                field: "Stempel",
                title: "Stempel",
                template: "#=Stempel ? 'Ya' : 'Tidak' #",
                width: "70px"
            },
            {
                field: "FileName",
                title: "Attachment",
                template: "<a href='#= UrlAtt #' target='_blank'> #= FileName != null ? FileName.replace(FileName.substr(0, 36), '') : '' # </a> "
            }
        ],
    }).data("kendoGrid");


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
            kodefaxreq: function (input) {
                if (input.is("[name=KodeFaxBilling]") && $("#IsFax").is(':checked')) {
                    return input.val() != "";
                }

                return true;
            },
            faxreq: function (input) {
                if (input.is("[name=FaxBilling]") && $("#IsFax").is(':checked')) {
                    return input.val() != "";
                }

                return true;
            },
            emailreq: function (input) {
                if (input.is("[name=EmailBilling]") && $("#IsEmail").is(':checked')) {
                    return input.val() != "";
                }

                return true;
            },
            inforeq: function (input)
            {
                if (input.is("[name=Info]") && $("#IsFax").is(':checked') == false && $("#IsEmail").is(':checked') == false
                    && $("#IsFaktur").is(':checked') == false && $("#IsJasa").is(':checked') == false) {
                    console.log("false")
                    return false;
                }
                console.log("true")
                return true;
            }
        }
    }).data("kendoValidator");

    lembar = $("#Lembar").kendoNumericTextBox({
        format: "n0",
        spinner: false
    }).data("kendoNumericTextBox");

    createUploader();
    CreateLink();

    gridJadwal = $("#gridJadwal").kendoGrid({
        dataSource: {
            data: dsGrid,
            schema: {
                model: {
                    id:"Id",
                    fields: {
                        Id:{type:"number", defaultValue:0},
                        Hari: {
                            type: "string",
                            validation: {
                                required: { message: "Hari harus diisi." },
                                comboreq: function (input) {
                                    if ($(input).data("kendoComboBox")) {
                                        if ($(input).data("kendoComboBox").selectedIndex == -1) {
                                            return false;
                                        }
                                    }

                                    return true;
                                },
                            }
                        },
                        Jam: {
                            type: "string",
                            validation: {
                                required: { message: "Jam harus diisi." },
                                format: function (input) {
                                    if (input.is("[name='Jam']") && input.val() != "") {
                                        if (!TimeValidate(input.val().split("-")[0].trim()))
                                            return false;
                                        if (!TimeValidate(input.val().split("-")[1].trim()))
                                            return false;
                                    }

                                    return true;
                                }
                            }
                        },
                        Catatan: { type: "string" },
                        Email: {
                            type: "string",
                            validation: {
                                required: { message: "Email harus diisi." }
                            }
                        }
                    }
                }
            },
        },
        scrollable:false,
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
                editor: emailEditor,
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
    }).data("kendoGrid");
    //listener checkbox warna
    $(".warna").on('click', function () {
        if ($(this).is(':checked'))
            warnaArr.push(this.value);
        else
            warnaArr.pop(this.value);
    });
    //$("#FaxBilling").kendoMaskedTextBox({
    //    mask: "0000-0000"
    //});
    //txtfax = $("#FaxBilling").data("kendoMaskedTextBox");
    //txtfax.readonly();

    $("#FaxBilling").data("kendoMaskedTextBox").enable(false);
    //listener checkbox fax
    var myElement = document.querySelector("#FaxBilling");
    myElement.style.backgroundColor = "#EEF1F5";
    var myElement2 = document.querySelector("#KodeFaxBilling");
    myElement2.style.backgroundColor = "#EEF1F5";
    $("#IsFax").on('click', function () {
        if ($(this).is(':checked')) {
            //txtfax.readonly(false);
            myElement.style.backgroundColor = "";
            myElement2.style.backgroundColor = "";
            $("#FaxBilling").data("kendoMaskedTextBox").enable(true);
            $("#FaxBilling").val('');
            $("#KodeFaxBilling").data("kendoMaskedTextBox").enable(true);
            $("#KodeFaxBilling").val('');
        }
        else {
            //txtfax.readonly();
            myElement.style.backgroundColor = "#EEF1F5";
            myElement2.style.backgroundColor = "#EEF1F5";
            $("#FaxBilling").data("kendoMaskedTextBox").enable(false);
            $("#FaxBilling").val('');
            $("#KodeFaxBilling").data("kendoMaskedTextBox").enable(false);
            $("#KodeFaxBilling").val('');
        }
    });
    //listener checkbox fax
    $("#IsEmail").on('click', function () {
        if ($(this).is(':checked')) {
            $("#EmailBilling").prop('disabled', '');
            $("#EmailBilling").val('');
        }
        else {
            $("#EmailBilling").prop('disabled', 'disabled');
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

function ShowBillingPopup() {
    $('.k-invalid-msg').hide();
    $('#CustomerBillingId').val(0);
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
    CreateLink();
    //end uploader

    $("#IsFax").prop('checked', false);
    $("#IsEmail").prop('checked', false);
    $("#IsFaktur").prop('checked', false);
    $("#IsJasa").prop('checked', false);
    $("#gridJadwal").hide();
    gridJadwal.dataSource.data([]);
    var myElement = document.querySelector("#FaxBilling");
    var myElement2 = document.querySelector("#KodeFaxBilling");
    myElement.style.backgroundColor = "#EEF1F5";
    myElement2.style.backgroundColor = "#EEF1F5";
    $("#FaxBilling").data("kendoMaskedTextBox").enable(false);
    $("#FaxBilling").val('');
    $("#KodeFaxBilling").data("kendoMaskedTextBox").enable(false);
    $("#KodeFaxBilling").val('');
    $('#EmailBilling').val("");
}

function doSave()
{
    if (!$('#IsFaktur').is(':checked')) {
        dsGrid.length = 0;
        gridJadwal.dataSource.read();
    }

    gridJadwal.saveRow();
    var dataGrid = gridJadwal.dataSource.data();

    var data = {
        Id : $('#CustomerBillingId').val(),
        CustomerId : $('#Id').val(),
        DocumentName : $('#Dok').val(),
        Lembar : lembar.value(),
        Warna : warnaArr.join(", "),
        Stempel: $('#rdStampe:checked').val(),
        IsFax: $('#IsFax').is(':checked'),
        Fax: $('#KodeFaxBilling').val() + ' - ' + $('#FaxBilling').val(),
        IsEmail: $('#IsEmail').is(':checked'),
        Email: $('#EmailBilling').val(),
        IsTukarFaktur: $('#IsFaktur').is(':checked'),
        IsJasaPengiriman: $('#IsJasa').is(':checked'),
        IsActive: $('#IsActive').is(':checked'),
        UrlAtt: path,
        FileName: fname,
        srtDataJadwal: JSON.stringify(dataGrid),
    };

    goToSavePage("/Customer/CustomerSaveBilling/", data, GridCustBilling.dataSource);

    kendoUploadButton = null;
    modalBilling.modal('hide');
}

function SaveBilling() {
    $('.k-invalid-msg').hide();
    if (validatorBilling.validate())
    {
        if (kendoUploadButton) {
            App.blockUI({
                target: "#formBilling",
                overlayColor: "none",
                cenrerY: !0,
                animate: !0
            })
            kendoUploadButton.click();
        }
        else {
            doSave();
        }
    }
}

function deleteBilling(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    goToDeletePage("/Customer/CustomerDeleteBilling?CustId=" + $("#Id").val() + "&id=" + data.Id, this.dataSource);
}

function editBilling(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    $('.k-invalid-msg').hide();
    $('#CustomerBillingId').val(data.Id);
    $('#Dok').val(data.DocumentName);
    lembar.value(data.Lembar);
    warnaArr = [];
    $(".warna").prop('checked', false);
    var warnaList = data.Warna;
    var substr = warnaList.split(', ');
    for (var i = 0; i < substr.length; i++) {
        warnaArr.push(substr[i]);
        $(":checkbox[value=" + substr[i] + "]").prop("checked", true);

    }
    $(":radio[id=rdStampe][value=" + data.Stempel + "]").prop("checked", true);

    dsupload = [];
    if (data.FileName != null && data.UrlAtt != null)
    {
        fname = data.FileName;
        path = data.UrlAtt;
        dsupload.push({ name: fname });
    }

    $("#wrapper").empty();        
    createUploader();
    CreateLink();

    var myElement = document.querySelector("#FaxBilling");
    var myElement2 = document.querySelector("#KodeFaxBilling");
    if (data.IsFax == true) {
        myElement.style.backgroundColor = "";
        myElement2.style.backgroundColor = "";
        $("#FaxBilling").data("kendoMaskedTextBox").enable(true);
        $("#IsFax").prop("checked", true);
    }
    else {
        myElement.style.backgroundColor = "#EEF1F5";
        myElement2.style.backgroundColor = "#EEF1F5";
        $("#FaxBilling").data("kendoMaskedTextBox").enable(false);
        $("#IsFax").prop("checked", false);
    }
    var faxBilling = data.Fax.split(' - ');
    $("#FaxBilling").data("kendoMaskedTextBox").value(faxBilling[0]);
    $("#KodeFaxBilling").data("kendoMaskedTextBox").value(faxBilling[1]);

    if (data.IsEmail == true)
        $("#IsEmail").prop("checked", true);
    else
        $("#IsEmail").prop("checked", false);
    $("#EmailBilling").val(data.Email);
    if (data.IsTukarFaktur == true) {
        $("#IsFaktur").prop("checked", true);
        var dummyJadwal = $.parseJSON(data.srtDataJadwal);
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
    if (data.IsJasaPengiriman == true)
        $("#IsJasa").prop("checked", true);
    else
        $("#IsJasa").prop("checked", false);
    if (data.IsActive == true)
        $("#IsActive").prop("checked", true);
    else
        $("#IsActive").prop("checked", false);
            
    modalBilling.modal('show');
}