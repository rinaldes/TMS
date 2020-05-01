$(document).ready(function () {
    call_piutang('All')
});
function call_piutang(piutang_type){
    dsPiutang = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Driver/BindingPiutangHistory?id=' + $("#Id").val() + '&piutang_type=' + piutang_type, dataType: "json"
            },
        },
        schema: {
            model: {
                fields: {
                    Tanggal: { type: "string" },
                    Keterangan: { type: "string" },
                    Jumlah: { type: "number" },
                    Saldo: { type: "number" },
                    Id: { type: "number" },
                }
            }
        },
    });
    $("#gridPiutang").kendoGrid({
        dataSource: dsPiutang,
        columns: [
            {field: "Tanggal", template: "#= kendo.toString(kendo.parseDate(Tanggal, 'yyyy-MM-dd'), 'dd/MM/yyyy HH:mm:ss') #", width: "86px"},
            { field: "Keterangan", template: kendo.template($("#jenisOrder-template").html()), width: "800px"},
            { field: "Jumlah", template: 'Rp #: kendo.format("{0:n}", Jumlah)#', attributes: { style: "text-align:right;" }, width: "77px" },
            { field: "Saldo", template: 'Rp #: kendo.format("{0:n}", Saldo)#', attributes: { style: "text-align:right;" }, width: "75px" }
        ],
    }).data("kendoGrid");
}