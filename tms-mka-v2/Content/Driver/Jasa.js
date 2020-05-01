var validatorJasa;
var gridJasa;
function ShowJasaPopup() {
    $('.k-invalid-msg').hide();
    $('#JasaId').val(0);
    $('#TanggalJasa').data("kendoDatePicker").value(null);
    $('#KeteranganJasa').val("");
    $('#modalFormJasa').modal('show');
}
function saveJasa() {
    if (validatorJasa.validate()) {
        var data = {
            Id: $('#JasaId').val(),
            IdDriver: $('#Id').val(),
            TanggalJasa: kendo.toString($('#TanggalJasa').data("kendoDatePicker").value(), "d"), 
            Keterangan: $('#KeteranganJasa').val(),
        };
        goToSavePage("/Driver/SaveJasa/", data, gridJasa.dataSource);
        $('#modalFormJasa').modal('hide');
    }
}
function editJasa(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    console.log(data)
    $('.k-invalid-msg').hide();
    $('#JasaId').val(data.Id);
    $('#TanggalJasa').data("kendoDatePicker").value(data.Tanggal);
    $('#KeteranganJasa').val(data.Keterangan);
    $('#modalFormJasa').modal('show');
}
function deleteJasa(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    goToDeletePage("/Driver/DeleteJasa?IdDriver=" + $("#Id").val() + "&id=" + data.Id, this.dataSource);
}

$(document).ready(function () {
    validatorJasa = $('#formModalJasa').kendoValidator().data("kendoValidator");
    var dsGridJasa = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Driver/BindingJasa?idDriver=' + $("#Id").val(),
                dataType: "json"
            },
        },
        schema: {
            model: {
                fields: {
                    Id: { type: "number" },
                    Tanggal: { type: "date" },
                    keterangan: { type: "string" },
                }
            }
        },
    });

    gridJasa = $("#gridJasa").kendoGrid({
        dataSource: dsGridJasa,
        columns: [
            {
                field: "Tanggal",
                title: "Tanggal",
                template: "#= kendo.toString(kendo.parseDate(Tanggal, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
            },
            {
                field: "Keterangan",
                title: "keterangan"
            },
            {
                command: [
                    {
                        name: "edit",
                        text: "edit",
                        click: editJasa,
                        imageClass: "glyphicon glyphicon-edit",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=imageClass#"></span></a>'
                    },
                    {
                        name: "delete",
                        text: "delete",
                        click: deleteJasa,
                        imageClass: "glyphicon glyphicon-remove",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=iconClass# #=imageClass#"></span></a>'
                    }
                ],
                width: "60px"
            },
        ],
    }).data("kendoGrid");
});