var listuploadMitra = [];
var listuploadJaminan = [];
var listuploadIjazah = [];
var listuploadBukuNikah = [];
var listuploadSKCK = [];
var listuploadDomisili = [];
var listuploadKK = [];
var listuploadKTP = [];
var listuploadSim = [];

$(document).ready(function () {
    SetDatasoure($("#UrlKemitraan").val(), listuploadMitra);
    CreateUploader($("#filesKemitraan"), listuploadMitra);
    CreateLink($("#filesKemitraan"), listuploadMitra);
    SetDatasoure($("#UrlJaminanKel").val(), listuploadJaminan);
    CreateUploader($("#filesJaminanKel"), listuploadJaminan);
    CreateLink($("#filesJaminanKel"), listuploadJaminan);
    SetDatasoure($("#UrlIjazah").val(), listuploadIjazah);
    CreateUploader($("#filesIjazah"), listuploadIjazah);
    CreateLink($("#filesIjazah"), listuploadIjazah);
    SetDatasoure($("#UrlBukuNikah").val(), listuploadBukuNikah);
    CreateUploader($("#filesBukuNikah"), listuploadBukuNikah);
    CreateLink($("#filesBukuNikah"), listuploadBukuNikah);
    SetDatasoure($("#UrlSKCK").val(), listuploadSKCK);
    CreateUploader($("#filesSKCK"), listuploadSKCK);
    CreateLink($("#filesSKCK"), listuploadSKCK);
    SetDatasoure($("#UrlDomisili").val(), listuploadDomisili);
    CreateUploader($("#filesDomisili"), listuploadDomisili);
    CreateLink($("#filesDomisili"), listuploadDomisili);
    SetDatasoure($("#UrlKK").val(), listuploadKK);
    CreateUploader($("#filesKK"), listuploadKK);
    CreateLink($("#filesKK"), listuploadKK);
    SetDatasoure($("#UrlKTP").val(), listuploadKTP);
    CreateUploader($("#filesKTP"), listuploadKTP);
    CreateLink($("#filesKTP"), listuploadKTP);
    SetDatasoure($("#UrlSIM").val(), listuploadSim);
    CreateUploader($("#filesSIM"), listuploadSim);
    CreateLink($("#filesSIM"), listuploadSim);
});
function CreateUploader(el, dataSouce) {
    el.kendoUpload({
        multiple: true,
        files: dataSouce,
        async: {
            saveUrl: "/FileManagement/Upload?Dir=~/Uploads/Driver",
            removeUrl: "/FileManagement/Delete?Dir=~/Uploads/Driver&temp=true",
            autoUpload: true
        },
        success: function (e, options) {
            if (e.operation == "upload") {
                $(".k-filename:contains('" + e.files[0].name + "')").text('').append('<a href="' + e.response.imagelink + '" target="_blank">' + e.response.fileName + '</a>');
                e.files[0].name = e.response.fileName;
                dataSouce.push({ name: e.response.fileName });
            } else if (e.operation == "remove") {
                dataSouce = jQuery.grep(dataSouce, function (a) {
                    return a !== e.response.fileName;
                });
            }
        },
        select: onSelectImage,
    }).data("kendoUpload");
}
function SetDatasoure(source, dataSouce)
{
    var s = source.split(","),
    i;

    if (s[0] != "")
    {
        for (i = 0; i < s.length; i++) {
            dataSouce.push({ name: s[i] });
        }
    }
}
function ShowHide(el, IsCopy, BtnUpload) {
    if ($(el).is(":checked")) {
        $('*[id*=' + IsCopy + ']:visible').each(function () {
            $(this).prop("disabled", "");
        });
        BtnUpload.show();
    }
    else {
        $('*[id*=' + IsCopy + ']:visible').each(function () {
            $(this).prop("disabled", "disabled");
        });
        BtnUpload.hide();
    }
}
function CreateLink(el, dataSouce) {
    var filename = el.data("kendoUpload").wrapper.find(".k-filename")
    for (var i = 0; i < filename.length; i++) {
        $(filename[i]).html("<a href='/Uploads/Driver/" + dataSouce[i].name + "' target='_blank'>" + dataSouce[i].name + "</a>")
    }
}