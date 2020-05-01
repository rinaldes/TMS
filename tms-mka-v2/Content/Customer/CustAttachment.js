var kendoUploadButton;
var filesAtt;
function FileTemplate(container, options) {
    if (container.url != null) {
        var extFile = container.url.split('.').pop();
        var ExtensionImage = ["jpg", "jpeg", "pdf"];
        if ($.inArray((extFile).toLowerCase(), ExtensionImage) !== -1) {
            return '<a href="' + container.url.replace("~/", "") + '" target="blank_"> ' + container.realfname.replace(container.realfname.substr(0, 36), '') + ' </a>';
        }
    }
}

function onSelectImage(e) {
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
    kendoUploadButton = null;
    setTimeout(function () {
        //kendoUploadButton = $(".k-upload-selected");
        //kendoUploadButton.hide();
    }, 1);
}

function getlistFile(options) {
    filesAtt = []
    if (options.model.realfname != "" && options.model.realfname != null) {
        filesAtt.push({ name: options.model.realfname })
    }
    return filesAtt;
}

function fileUploadEditor(container, options) {
    $('<input type="file" name="files" id="files"/>')
       .appendTo(container)
       .kendoUpload({
           multiple: false,
           async: {
               saveUrl: "/FileManagement/Upload?Dir=~/Uploads",
               removeUrl: "/FileManagement/Delete?Dir=~/Uploads&temp=true",
               autoUpload: true
           },
           files: getlistFile(options),
           select: onSelectImage,
           success: function (e) {
               if (e.operation == "upload") {
                   $(".k-filename:contains('" + e.files[0].name + "')").text('').append('<a href="' + e.response.imagelink + '" target="_blank">' + e.response.fileName.replace(e.response.fileName.substr(0, 36), '') + '</a>');
                   e.files[0].name = e.response.fileName;
                   options.model.set("realfname", e.response.fileName);
                   options.model.set("url", e.response.imagelink);
               } else if (e.operation == "remove") {
                   options.model.set("realfname", "");
                   options.model.set("url", "");
               }
           },
       });
    $("<span class='k-invalid-msg' data-for='Url'></span>").appendTo(container);
    var filename = $("#files").data("kendoUpload").wrapper.find(".k-filename")
    for (var i = 0; i < filename.length; i++) {
        $(filename[i]).html("<a href='/Uploads/" + filesAtt[i].name + "' target='_blank'>" + filesAtt[i].name.replace(filesAtt[i].name.substr(0, 36), '') + "</a>")
    }
}
$(document).ready(function () {
    $("#GridAttachment").kendoGrid({
        batch: true,
        dataSource: {
            sync: function (e) {
                this.read();
            },
            transport: {
                read: {
                    url: function (data) {
                        return "/Customer/bindingAttachment/" + $("#Id").val();
                    },
                    dataType: "json",
                    type: "POST",
                },
                update: {
                    url: "/Customer/UpdateAttachment",
                    dataType: "json",
                    type: "POST"
                },
                create: {
                    url: "/Customer/AddAttachment",
                    dataType: "json",
                    type: "POST"
                },
            },
            schema: {
                data: "data",
                total: "total",
                model: {
                    id: "id",
                    fields: {
                        "id": { type: "number" },
                        "CustId": { defaultValue: $("#Id").val(), type: "number" },
                        "url": {
                            type: "string",
                            validation: {
                                required: { message: "File harus diisi." },
                            }
                        },
                        "filename": {
                            type: "string",
                            validation: {
                                required: { message: "Nama harus diisi." },
                            }
                        },
                        "realfname": { type: "string" }
                    }
                },
            },
            pageSize: 10,
        },
        editable: { mode: "inline", confirmation: "Anda yakin menghapus data?" },
        //height: 400,
        scrollable: false,
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
                        //name: "delete",
                        //text: "delete",
                        click: deleteAtt,
                        imageClass: "glyphicon glyphicon-remove",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=iconClass# #=imageClass#"></span></a>'
                    }
                    //{
                    //    name: "destroy",
                    //    text: "Hapus",
                    //    imageClass: "glyphicon glyphicon-remove",
                    //    template: '<a class="k-button-icon #=className#" #=attr# href="\\#" title="#=text#"><span class="#=imageClass#"></span></a>'
                    //}
                ],
                width: 100
            },
            {
                field: "CustId",
                hidden: true,
            },
            {
                field: "filename",
                title: "Nama"
            },
            {
                field: "realfname",
                hidden: true
            },
            {
                field: "url",
                title: "File",
                template: FileTemplate,
                editor: fileUploadEditor
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
    })
    function deleteAtt(e) {
        e.preventDefault();
        var data = this.dataItem(getDataRowGrid(e));
        goToDeletePage("/Customer/DeleteAttachment?custid=" + $("#Id").val() + "&id=" + data.id, this.dataSource);
    }
})