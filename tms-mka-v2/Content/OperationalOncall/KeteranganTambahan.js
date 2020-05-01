var GridMasterRekening;

function SelectRek(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));

    $('#AtmId').val(data.Id);
    $('#StrRekening').val(data.NoRekening);
    $('#AtasNamaRek').val(data.AtasNama);
    $('#Bank').val(data.NamaBank);

    $('#ModalMasterRekening').modal('hide');
}

function GenerateCash(val) {
    var combobox = $("#IdDriverTitip").data("kendoComboBox");
    if (val == "True") {
        combobox.enable(true);
        $('#btnbrowserek').attr('disabled', 'disabled');
        //$('#RekeningId').val(' ');
        //$('#StrRekening').val(' ');
        //$('#AtasNamaRek').val(' ');
        //$('#Bank').val(' ');
        //$('#KeteranganRek').val(' ');
        $('#KeteranganRek').attr('readonly', 'readonly');
    }
    else {
        combobox.enable(false);
        $('#btnbrowserek').removeAttr('disabled');
        $('#KeteranganRek').removeAttr('readonly');
    }
}

$(document).ready(function () {
    $("#IdDriverTitip").kendoComboBox({
        dataTextField: "text",
        dataValueField: "value",
        filter: "contains",
        //autoBind: false,
        minLength: 3,
        dataSource: {
            transport: {
                read: {
                    url: "/Driver/BindingCombobox",
                    dataType: "json"
                }
            }
        }
    });

    var ds = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/atm/binding',
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
                            if (filter[i].field == "NamaBank") {
                                filter[i].field = "LookupCodeBank.Nama";
                            }
                            if (filter[i].field == "KodeDriver") {
                                filter[i].field = "Driver.KodeDriver";
                            }
                            if (filter[i].field == "NamaDriver") {
                                filter[i].field = "Driver.NamaDriver";
                            }
                            if (filter[i].field == "Panggilan") {
                                filter[i].field = "Driver.NamaPangilan";
                            }
                        }
                    }

                    if (options.sort) {
                        sort = options.sort;
                        for (var i in sort) {
                            if (sort[i].field == "NamaBank") {
                                sort[i].field = "LookupCodeBank.Nama";
                            }
                            if (sort[i].field == "KodeDriver") {
                                sort[i].field = "Driver.KodeDriver";
                            }
                            if (sort[i].field == "NamaDriver") {
                                sort[i].field = "Driver.NamaDriver";
                            }
                            if (sort[i].field == "Panggilan") {
                                sort[i].field = "Driver.NamaPangilan";
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
                    "NoKartu": { type: "string" },
                    "NamaBank": { type: "string" },
                    "NoRekening": { type: "string" },
                    "AtasNama": { type: "string" },
                    "KodeDriver": { type: "string" },
                    "NamaDriver": { type: "string" },
                    "Panggilan": { type: "string" }
                }
            }
        },
        pageSize: 10,
        pageable: true,
        serverFiltering: true,
        serverPaging: true,
        serverSorting: true,
        sortable: true,
    });

    GridMasterRekening = $("#GridMasterRekening").kendoGrid({
        dataSource: ds,
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
                        click: SelectRek,
                        imageClass: "glyphicon glyphicon-ok",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=iconClass# #=imageClass#"></span></a>'
                    }
                ],
                width: "50px"
            },
            {
                field: "NoKartu",
                title: "No Kartu",
                width: "150px",
            },
            {
                field: "NamaBank",
                title: "Nama Bank",
                width: "150px",
            },
            {
                field: "NoRekening",
                title: "No Rekening",
                width: "150px",
            },
            {
                field: "AtasNama",
                title: "Atas Nama",
                width: "150px",
            },
            {
                field: "KodeDriver",
                title: "Kode Driver",
                width: "150px",
            },
            {
                field: "NamaDriver",
                title: "Nama Driver",
                width: "250px",
            },
            {
                field: "Panggilan",
                title: "Nama Panggilan",
                width: "230px",
            }
        ],
    }).data("kendoGrid");

    GenerateCash($('input[name =IsCash]:checked').val());
})