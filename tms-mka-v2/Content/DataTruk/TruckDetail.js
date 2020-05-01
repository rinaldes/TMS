var dsMasterTruk;
var gridMasterTruk;
var IdTruck = 0;
var IdSo = 0;

function GetHistoryJalanTruck(Id) {
    var dsDetailHis = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/DataTruck/GetHistoryJalan?id=' + Id,
                dataType: "json"
            },
        },
        pageSize: 10,
        pageable: true,
        sortable: true,
    });

    $("#GridHistoryJalanTruck").kendoGrid({
        dataSource: dsDetailHis,
        filterable: kendoGridFilterable,
        sortable: true,
        reorderable: true,
        resizable: true,
        pageable: true,
        columns: [
        { field: "driver1", title: "Driver 1", template: "<a href='\\#' data-toggle='modal' data-target='\\#ModalHistoryJalan' onclick='GetHistoryJalan(#:idDriver1#)'> #= driver1 # </a>" },
        { field: "driver2", title: "Driver 2", template: "<a href='\\#' data-toggle='modal' data-target='\\#ModalHistoryJalan' onclick='GetHistoryJalan(#:idDriver2#)'> #= driver2 # </a>" },
        { field: "customer", title: "Customer" },
        { field: "rute", title: "Rute" },
        { field: "tglMuat", title: "Tgl Muat", template: "#= tglMuat != null ? kendo.toString(kendo.parseDate(tglMuat, 'yyyy-MM-dd'), 'dd/MM/yyyy') : ''#" },
        ],
    });
}

$(document).ready(function () {
    dsMasterTruk = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/DataTruck/BindingDetail?IdTruck=' + IdTruck + '&idSo=' + IdSo,
                dataType: "json"
            },
        },
        schema: {
            total: "total",
            data: "data",
            model: {
                fields: {
                    "Id": { type: "number" },
                    "StatusOrder": { type: "string" },
                    "StatusTruk": { type: "string" },
                    "VehicleNo": { type: "string" },
                    "JenisTruk": { type: "string" },
                    "Pendingin": { type: "string" },
                    "Lantai": { type: "string" },
                    "Dinding": { type: "string" },
                    "AlokasiPool": { type: "string" },
                    "AlokasiUnit": { type: "string" },
                    "KondisiKhusus": { type: "string" },
                    "IdDriver1": { type: "number" },
                    "KodeDriver1": { type: "string" },
                    "NamaDriver1": { type: "string" },
                    "StatusDriver1": { type: "string" },
                    "IdDriver2": { type: "number" },
                    "KodeDriver2": { type: "string" },
                    "NamaDriver2": { type: "string" },
                    "StatusDriver2": { type: "string" },
                }
            }
        },
        pageSize: 5,
        pageable: true,
        sortable: true,
    });

    gridMasterTruk = $("#GridMasterTruk").kendoGrid({
        dataSource: dsMasterTruk,
        filterable: kendoGridFilterable,
        sortable: true,
        reorderable: true,
        resizable: true,
        pageable: true,
        groupable: true,
        columns: [
            {
                command: [
                    {
                        name: "select",
                        text: "Select",
                        click: SelectTruk,
                        imageClass: "glyphicon glyphicon-ok",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=iconClass# #=imageClass#"></span></a>'
                    }
                ],
                width: "50px"
            },
            { field: "StatusOrder", title: "Status Order" },
            { field: "StatusTruk", title: "Status Truk" },
            { field: "VehicleNo", title: "Vehicle Number", template: "<a href='\\#' data-toggle='modal' data-target='\\#ModalHistoryJalanTruck' onclick='GetHistoryJalanTruck(#:Id#)'> #= VehicleNo # </a>" },
            { field: "JenisTruk", title: "Jenis Truk" },
            { field: "Pendingin", title: "Pendingin" },
            { field: "Lantai", title: "Lantai" },
            { field: "Dinding", title: "Dinding" },
            { field: "AlokasiPool", title: "Alokasi Pool" },
            { field: "AlokasiUnit", title: "Alokasi Unit" },
            { field: "KondisiKhusus", title: "Kondisi Khusus" },
            { field: "DokumenExp", title: "Dokumen Expired" }
        ],
    }).data("kendoGrid");
})