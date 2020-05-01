var dsGridDp;
var GridDp;
var validatorDp;

function SelectRekening(e)
{
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));

    $('#IdRekeningDp').val(data.Id);
    $('#RekeningDp').val(data.NoRekening);
    $('#modalRekening').modal('hide');
}

function SaveDp() {
    if ($('#TanggalDp').val() == '' || $('#PenerimaDp').val() == '' || $('#JenisDp').val() == '' || $('#IdRekeningDp').val() == '' || $('#JumlahDp').val() == '' || $('#SalesOrderProsesKonsolidasiId').val() == '')
    {
        swal({
            title: "Error",
            type: 'error',
            text: 'Tolong Lengkapi Data Form Isian.',
            timer: 2000,
            showConfirmButton: false
        })
        $('.showSweetAlert').css('z-index', 10000000)
    }
    else {
        var data = {
            Id: $('#IdDp').val(),
            Tanggal: $('#TanggalDp').val(),
            Penerima: $('#PenerimaDp').val(),
            Jenis: $('#JenisDp').val(),
            RekeningId: $('#IdRekeningDp').val(),
            Jumlah: $('#JumlahDp').data('kendoNumericTextBox').value(),
            SalesOrderOnCallId: $('#SalesOrderOnCallId').val(),
        }
        
        goToSavePage("/SalesOrderOnCall/SaveDp/", data, GridDp.dataSource);
        $('#ModalFormDp').modal('hide');
    }
}

function clearFormDp()
{
    $('.k-invalid-msg').hide();
    $('#IdDp').val('');
    $('#TanggalDp').val('');
    $('#PenerimaDp').val('');
    $('#JenisDp').val('');
    $('#IdRekeningDp').val('');
    $('#RekeningDp').val('');
    $('#JumlahDp').data('kendoNumericTextBox').value('');
}

function editDp(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));

    $('.k-invalid-msg').hide();
    $('#IdDp').val(data.Id);
    $('#TanggalDp').val(kendo.toString(kendo.parseDate(data.Tanggal, 'yyyy-MM-dd'), 'MM/dd/yyyy'));
    $('#PenerimaDp').val(data.Penerima);
    $('#JenisDp').val(data.Jenis);
    $('#IdRekeningDp').val(data.IdRekening);
    $('#RekeningDp').val(data.NoRekening);
    $('#JumlahDp').data('kendoNumericTextBox').value(data.Jumlah);
    $('#ModalFormDp').modal('show');
}

function deleteDp(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    goToDeletePage("/SalesOrderOnCall/DeleteDp?IdSo=" + $("#SalesOrderOnCallId").val() + "&id=" + data.Id, this.dataSource);
}

$(document).ready(function () {
    validatorDp = $("#FormDp").kendoValidator().data("kendoValidator");

    dsGridDp = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/SalesOrderOnCall/BindingDp?id=' + $('#SalesOrderOnCallId').val(),
                dataType: "json"
            },
        },
        schema: {
            data: "data",
            model: {
                fields: {
                    "Id": { type: "number" },
                    "Tanggal": { type: "date" },
                    "Penerima": { type: "string" },
                    "Jenis": { type: "string" },
                    "IdRekening": { type: "number" },
                    "NoRekening": { type: "number" },
                    "Jumlah": { type: "string" }
                }
            }
        },
    });

    GridDp = $("#GirdDp").kendoGrid({
        dataSource: dsGridDp,
        columns: [
            {
                command: [
                    {
                        name: "edit",
                        text: "edit",
                        click: editDp,
                        imageClass: "glyphicon glyphicon-edit",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=imageClass#"></span></a>'
                    },
                    {
                        name: "delete",
                        text: "delete",
                        click: deleteDp,
                        imageClass: "glyphicon glyphicon-remove",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=iconClass# #=imageClass#"></span></a>'
                    }
                ],
                width: "60px"
            },
            {
                field: "Tanggal",
                title: "Tanggal",
                template: "#= Tanggal != null ? kendo.toString(kendo.parseDate(Tanggal, 'yyyy-MM-dd'), 'MM/dd/yyyy') : ''#"
            },
            {
                field: "Penerima",
                title: "Penerima"
            },
            {
                field: "Jenis",
                title: "Jenis"
            },
            {
                field: "NoRekening",
                title: "NoRekening"
            },
            {
                field: "Jumlah",
                title: "Jumlah",
                template: 'Rp #: Jumlah == null ? 0 : kendo.format("{0:n}", Jumlah)#',
            }
        ],
    }).data("kendoGrid");

    var dsRekening = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Rekening/Binding',
                dataType: "json"
            },
            parameterMap: function (options, operation) {
                if (operation !== "read" && options != '') {
                    return kendo.stringify(options);
                }
                else if (operation == "read") {
                    if (options.filter) {
                        filter = options.filter.filters;
                        for (var i in filter) {
                            if (filter[i].field == "Type_") {
                                filter[i].field = "Type";
                            }
                            if (filter[i].field == "StrBank") {
                                filter[i].field = "LookupCodeBank.Nama";
                            }
                        }
                    }

                    if (options.sort) {
                        sort = options.sort;
                        for (var i in sort) {
                            if (sort[i].field == "Type_") {
                                sort[i].field = "Type";
                            }
                            if (sort[i].field == "StrBank") {
                                sort[i].field = "LookupCodeBank.Nama";
                            }
                        }
                    }
                    return options;
                }
            }
        },
        schema: {
            total: "total",
            data: "data",
            model: {
                fields: {
                    "Id": { type: "number" },
                    "NamaRekening": { type: "string" },
                    "NoRekening": { type: "string" },
                    "StrBank": { type: "string" },
                    "Type_": { type: "string" },
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

    $("#GridRekening").kendoGrid({
        dataSource: dsRekening,
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
                        click: SelectRekening,
                        imageClass: "glyphicon glyphicon-ok",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=iconClass# #=imageClass#"></span></a>'
                    }
                ],
                width: "50px"
            },
            {
                field: "StrBank",
                title: "Bank"
            },
            {
                field: "NoRekening",
                title: "No Rekening"
            },
            {
                field: "NamaRekening",
                title: "Nama Rekening"
            },
            {
                field: "Type_",
                title: "Type",
                filterable: { multi: true, dataSource: [{ Type_: "PPN" }, { Type_: "Non PPN" }] }
            }
        ],
    }).data("kendoGrid");
})