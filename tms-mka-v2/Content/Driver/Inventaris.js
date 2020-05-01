var dsInvStat;
var cboNamaBarang;
var validatorInventaris;

function ShowInventarisPopup() {
    $('.k-invalid-msg').hide();
    $('#InventarisId').val(0);
    $('#TanggalPemberian').data("kendoDatePicker").value(null);
    $('#TanggalPengembalian').data("kendoDatePicker").value(null);
    cboNamaBarang.text('');
    cboNamaBarang.value('');
    $('#InventarisKeterangan').val("");
    $('#modalFormInventaris').modal('show');
}

function saveInventaris() {
//    if (validatorInventaris.validate()) { dimatiin krn eror null validatorInventaris
    if ($('#TanggalPemberian').val() != "" && $('#IdNamaBarang').val() != ""){
        var data = {
            TanggalPemberian: $('#TanggalPemberian').val(),
            TanggalPengembalian: $('#TanggalPengembalian').val(),
            IdNamaBarang: $('#IdNamaBarang').val(),
            Keterangan: $('#InventarisKeterangan').val(),
            DriverId: $('#Id').val(),
            Id: $('#InventarisId').val(),
        };
        goToSavePage('/Driver/SaveInventaris', data, dsInvStat);
        $('#modalFormInventaris').modal('hide');
    }
}

function editInventaris(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    console.log(data)
    $('.k-invalid-msg').hide();
    $('#InventarisId').val(data.Id);
    $('#TanggalPemberian').val(data.StrTanggalPemberian.split(' ')[0]);
    $('#TanggalPengembalian').val(data.StrTanggalPengembalian.split(' ')[0]);
    cboNamaBarang.value(data.IdNamaBarang);
    cboNamaBarang.text(data.NamaBarang);
    $('#InventarisKeterangan').val(data.Keterangan);

    $('#modalFormInventaris').modal('show');
}

function deleteInventaris(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    goToDeletePage('/Driver/DeleteInventaris' + "?id=" + data.Id, this.dataSource);
}

$(document).ready(function () {
    validatorInventaris = $("#formModalInventaris").kendoValidator().data("kendoValidator");

    dsInvStat = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Driver/BindingInventaris?id=' + $("#Id").val(),
                dataType: "json"
            },
        },
        schema: {
            model: {
                fields: {
                    Tanggal: { type: "string" },
                    Status: { type: "string" },
                    keterangan: { type: "string" },
                }
            }
        },
    });

    $("#gridInventaris").kendoGrid({
        dataSource: dsInvStat,
        columns: [
            {
                field: "TanggalPemberian",
                title: "Tanggal Pemberian",
                template: "#= kendo.toString(kendo.parseDate(TanggalPemberian, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
            },
            {
                field: "TanggalPengembalian",
                title: "Tanggal Pengembalian",
                template: "#= kendo.toString(kendo.parseDate(TanggalPengembalian, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
            },
            {
                field: "NamaBarang",
                title: "Nama Barang"
            },
            //{
            //    field: "IdNamaBarang"
            //},
            {
                field: "Keterangan",
                title: "Keterangan"
            },
            {
                command: [
                    {
                        name: "edit",
                        text: "edit",
                        click: editInventaris,
                        imageClass: "glyphicon glyphicon-edit",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=imageClass#"></span></a>'
                    },
                    {
                        name: "delete",
                        text: "delete",
                        click: deleteInventaris,
                        imageClass: "glyphicon glyphicon-remove",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=iconClass# #=imageClass#"></span></a>'
                    }
                ],
                width: "60px"
            },
        ],
    }).data("kendoGrid");

    dsNamaBarang = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Base/GetNamaBarang',
                dataType: "json"
            },
        },
    });
    cboNamaBarang = $("#IdNamaBarang").kendoComboBox({
        dataSource: dsNamaBarang,
        dataTextField: "Nama",
        dataValueField: "Id",
        filter: "contains",
        suggest: true,
    }).data("kendoComboBox");
});