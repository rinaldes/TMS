var dsProvinsi;
var dsKota;
var dsKec;
var dsKel;
var dsJenisSim;

$(document).ready(function () {
    dsProvinsi = new kendo.data.DataSource({
        //serverFiltering: true,
        transport: {
            read: {
                url: '/Customer/GetProvinsi',
                dataType: "json"
            },
        },
        minLength: 3,
        autoBind: false,
    });
    cboProvinsi = $("#IdProvinsi").kendoComboBox({
        dataTextField: "Nama",
        dataValueField: "Id",
        dataSource: dsProvinsi,
        filter: "contains",
        suggest: true,
        //change: OnProvChange,
    }).data("kendoComboBox");

    cboKota = $("#IdKabKota").kendoComboBox({
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
        //change: OnKotaChange,
    }).data("kendoComboBox");
    cboKota.text($("#StrKabKota").val());

    cboKec = $("#IdKec").kendoComboBox({
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
        //change: OnKecamatanChange,
    }).data("kendoComboBox");
    cboKec.text($("#StrKec").val());

    cboKel = $("#IdKel").kendoComboBox({
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
    }).data("kendoComboBox");
    cboKel.text($("#StrKel").val());

    var dsProvinsi2 = new kendo.data.DataSource({
        //serverFiltering: true,
        transport: {
            read: {
                url: '/Customer/GetProvinsi',
                dataType: "json"
            },
        },
    });
    cboProvinsiDomisili = $("#IdProvinsiDomisili").kendoComboBox({
        dataTextField: "Nama",
        dataValueField: "Id",
        dataSource: dsProvinsi2,
        filter: "contains",
        suggest: true,
        minLength: 3,
        autoBind: false,
        //change: OnProvChangeDomisili,
    }).data("kendoComboBox");

    cboKotaDomisili = $("#IdKabKotaDomisili").kendoComboBox({
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
        //change: OnKotaChangeDomisili,
    }).data("kendoComboBox");
    cboKotaDomisili.text($("#StrKabKotaDomisili").val());

    cboKecDomisili = $("#IdKecDomisili").kendoComboBox({
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
        //change: OnKecamatanChangeDomisili,
    }).data("kendoComboBox");
    cboKecDomisili.text($("#StrKecDomisili").val());

    cboKelDomisili = $("#IdKelDomisili").kendoComboBox({
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
    }).data("kendoComboBox");
    cboKelDomisili.text($("#StrKelDomisili").val());

    if ($("#Id").val() != 0)
    {
        var idkota = $("#IdKabKota").val();
        var idkec = $("#IdKec").val();
        var idkel = $("#IdKel").val();
        //setKota($("#IdProvinsi").data("kendoComboBox").value());
        //setKecamatan(idkota);
        //setKelurahan(idkec);

        //$("#IdKabKota").data("kendoComboBox").value(idkota);
        //$("#IdKec").data("kendoComboBox").value(idkec);
        //$("#IdKel").data("kendoComboBox").value(idkel);

        if ($("#IsSameKtp").is(':checked') == false) {
            var idKotaDom = $("#IdKabKotaDomisili").val();
            var idKecDom = $("#IdKecDomisili").val();
            var idKelDom = $("#IdKelDomisili").val();
            //setKotaDomisili($("#IdProvinsiDomisili").data("kendoComboBox").value());
            //setKecamatanDomisili(idKotaDom);
            //setKelurahanDomisili(idKecDom);
            $("#IdKabKotaDomisili").data("kendoComboBox").value(idKotaDom);
            $("#IdKecDomisili").data("kendoComboBox").value(idKecDom);
            $("#IdKelDomisili").data("kendoComboBox").value(idKelDom);
        }
        else {
            $("#AlamatDomisili").val('');
            $("#AlamatDomisili").prop("readonly", "readonly");
            $("#RtDomisili").val('');
            $("#RtDomisili").data('kendoMaskedTextBox').enable(false);
            $("#RwDomisili").val('');
            $("#RwDomisili").data('kendoMaskedTextBox').enable(false);
            $("#IdProvinsiDomisili").data("kendoComboBox").value();
            $("#IdProvinsiDomisili").data("kendoComboBox").text('');
            $("#IdProvinsiDomisili").data("kendoComboBox").enable(false);
            $("#IdKabKotaDomisili").data("kendoComboBox").value();
            $("#IdKabKotaDomisili").data("kendoComboBox").text('');
            $("#IdKabKotaDomisili").data("kendoComboBox").enable(false);
            $("#IdKecDomisili").data("kendoComboBox").value();
            $("#IdKecDomisili").data("kendoComboBox").text('');
            $("#IdKecDomisili").data("kendoComboBox").enable(false);
            $("#IdKelDomisili").data("kendoComboBox").value();
            $("#IdKelDomisili").data("kendoComboBox").text('');
            $("#IdKelDomisili").data("kendoComboBox").enable(false);
        }
    }

    //Jenis SIM
    dsJenisSim = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Driver/GetJenisSim',
                dataType: "json"
            },
        },
    });
    InitLookUp($("#IdJenisSim"), dsJenisSim);

    dsStatus = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Driver/GetStatusDriver',
                dataType: "json"
            },
        },
    });
    InitLookUp($("#IdStatus"), dsStatus);
    
    dsRefCode = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Driver/GetRefCode',
                dataType: "json"
            },
        },
    });

    $("#IdReferensiDriver").kendoComboBox({
        dataSource: dsRefCode,
        dataTextField: "Nama",
        dataValueField: "Id",
        filter: "contains",
        suggest: true,
        change:onrefchange
    }).data("kendoComboBox");

    var dsFoto = [];
    dsFoto.push({ name: $("#Pathfoto").val() });

    //uploader foto
    $("#filesFoto").kendoUpload({
        files: dsFoto,
        multiple: false,
        showFileList: false,
        async: {
            saveUrl: "/FileManagement/Upload?Dir=~/Uploads",
            removeUrl: "/FileManagement/Delete?Dir=~/Uploads",
            autoUpload: true
        },
        select: onSelectImage,
        success: function (e) {
            if (e.operation == "upload") {
                $("#profile-foto").attr("src", e.response.imagelink);
                $('#Pathfoto').val(e.response.imagelink);
            }
            else {
                $("#profile-foto").attr("src", "");
                $('#Pathfoto').val("");
            }
        }
    });

    $("#IsSameKtp").on('click', function () {
        if ($(this).is(':checked')) {
            $("#AlamatDomisili").val('');
            $("#AlamatDomisili").prop("readonly", "readonly");
            $("#RtDomisili").val('');
            $("#RtDomisili").data('kendoMaskedTextBox').enable(false);
            $("#RwDomisili").val('');
            $("#RwDomisili").data('kendoMaskedTextBox').enable(false);
            $("#IdProvinsiDomisili").data("kendoComboBox").value();
            $("#IdProvinsiDomisili").data("kendoComboBox").text('');
            $("#IdProvinsiDomisili").data("kendoComboBox").enable(false);
            $("#IdKabKotaDomisili").data("kendoComboBox").value();
            $("#IdKabKotaDomisili").data("kendoComboBox").text('');
            $("#IdKabKotaDomisili").data("kendoComboBox").enable(false);
            $("#IdKecDomisili").data("kendoComboBox").value();
            $("#IdKecDomisili").data("kendoComboBox").text('');
            $("#IdKecDomisili").data("kendoComboBox").enable(false);
            $("#IdKelDomisili").data("kendoComboBox").value();
            $("#IdKelDomisili").data("kendoComboBox").text('');
            $("#IdKelDomisili").data("kendoComboBox").enable(false);
        }
        else {
            $("#AlamatDomisili").prop("readonly", "");
            $("#RtDomisili").data('kendoMaskedTextBox').enable(true);
            $("#RwDomisili").data('kendoMaskedTextBox').enable(true);
            $("#IdProvinsiDomisili").data("kendoComboBox").enable(true);
            $("#IdKabKotaDomisili").data("kendoComboBox").enable(true);
            $("#IdKecDomisili").data("kendoComboBox").enable(true);
            $("#IdKelDomisili").data("kendoComboBox").enable(true);
        }
    });

    var dsRef = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Driver/Binding',
                dataType: "json"
            },
        },
        schema: {
            total: "total",
            data: "data",
            model: {
                fields: {
                    "Id": { type: "number" },
                    "KodeDriver": { type: "string" },
                    "NamaDriver": { type: "string" },
                    "NamaPangilan": { type: "string" },
                    "TglLahir": { type: "string" },
                    "Alamat": { type: "string" },
                }
            }
        },
        pageSize: 10,
        pageable: true,
        serverFiltering: true,
        serverPaging: true,
        serverSorting: true,
        sortable: true,
        //sort: { field: "SubmittedDate", dir: "desc" }
    });

    $("#GridRef ").kendoGrid({
        dataSource: dsRef,
        filterable: kendoGridFilterable,
        sortable: true,
        reorderable: true,
        resizable: true,
        pageable: true,
        groupable: true,
        //height: "615",
        columns: [
            {
                command: [
                    {
                        name: "select",
                        text: "Select",
                        click: SelectRef,
                        imageClass: "glyphicon glyphicon-ok",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=iconClass# #=imageClass#"></span></a>'
                    }
                ],
                width: "50px"
            },
            {
                field: "KodeDriver",
                title: "Kode Driver"
            },
            {
                field: "NamaDriver",
                title: "Nama Sesuai KTP"
            },
            {
                field: "NamaPangilan",
                title: "Nama Panggilan"
            },
            {
                field: "TglLahir",
                title: "Tanggal Lahir",
                template: "#= kendo.toString(kendo.parseDate(TglLahir, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
            },
            {
                field: "Alamat",
                title: "Alamat"
            }
        ],
    }).data("kendoGrid");
});

function onrefchange(e)
{
    if (this.text() != "Driver MKA") {
        $("#IdRef").val('');
        $("#NamaRef").val('');
        $("#BtnRef").prop("disabled", "disabled");
        $("#CodeRef").val('');
        $("#KTPRef").val('');
        $("#tlpRef").val('');
        $("#hpRef").val('');
        $("#HubunganRef").val('');
        $("#HubunganRef").prop("disabled", "disabled");
        $("#KeteranganRef").val('');
        $("#KeteranganRef").prop("disabled", "disabled");
    }
    else {
        $("#BtnRef").prop("disabled", "");
        $("#HubunganRef").prop("disabled", "");
        $("#KeteranganRef").prop("disabled", "");
    }
}

function SelectRef(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    $('#IdRef').val(data.Id);
    $('#NamaRef').val(data.NamaDriver);
    $('#CodeRef').val(data.KodeDriver);
    $('#KTPRef').val(data.NoKtp);
    $('#tlpRef').val(data.NoTlp);
    $('#hpRef').val(data.NoTlp);
    $('#modalRef').modal('hide');
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

function OnProvChangeDomisili(e) {
    if (this.value() != "") {
        setKotaDomisili(this.value());
    }
    else {
        cboKotaDomisili.text('');
        cboKotaDomisili.value();
        cboKotaDomisili.setDataSource();
        cboKecDomisili.text('');
        cboKecDomisili.value();
        cboKecDomisili.setDataSource();
        cboKelDomisili.text('');
        cboKelDomisili.value();
        cboKelDomisili.setDataSource();
    }
}

function setKotaDomisili(idParent) {
    dsKotaDomisili = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetChildLoc?idParent=' + idParent,
                dataType: "json"
            },
        },
    });
    cboKotaDomisili.text('');
    cboKotaDomisili.value();
    cboKotaDomisili.setDataSource(dsKota);
    cboKecDomisili.text('');
    cboKecDomisili.value();
    cboKecDomisili.setDataSource();
    cboKelDomisili.text('');
    cboKelDomisili.value();
    cboKelDomisili.setDataSource();
}

function OnKotaChangeDomisili(e) {
    if (this.value() != "") {
        setKecamatanDomisili(this.value());
    }
    else {
        cboKecDomisili.text('');
        cboKecDomisili.value();
        cboKecDomisili.setDataSource();
        cboKelDomisili.text('');
        cboKelDomisili.value();
        cboKelDomisili.setDataSource();
    }
}

function setKecamatanDomisili(idParent) {
    dsKecDomisili = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetChildLoc?idParent=' + idParent,
                dataType: "json"
            },
        },
    });
    cboKecDomisili.text('');
    cboKecDomisili.value();
    cboKecDomisili.setDataSource(dsKecDomisili);
    cboKelDomisili.text('');
    cboKelDomisili.value();
    cboKelDomisili.setDataSource();
}

function OnKecamatanChangeDomisili(e) {
    if (this.value() != "") {
        setKelurahanDomisili(this.value());
    }
    else {
        //kosongkan semua
        cboKelDomisili.text('');
        cboKelDomisili.value();
        cboKelDomisili.setDataSource();
    }
}

function setKelurahanDomisili(idParent) {
    dsKelDomisili = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetChildLoc?idParent=' + idParent,
                dataType: "json"
            },
        },
    });
    cboKelDomisili.text('');
    cboKelDomisili.value();
    cboKelDomisili.setDataSource(dsKelDomisili);
}

