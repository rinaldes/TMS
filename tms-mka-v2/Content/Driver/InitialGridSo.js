var dsMasterDriver;
var gridMasterDriver;
var Caller;

function GetHistoryJalan(Id) {
    var dsDetailHis = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Driver/GetHistoryJalan?id=' + Id,
                dataType: "json"
            },
        },
        pageSize: 10,
        pageable: true,
        sortable: true,
    });

    $("#GridHistoryJalanDriver").kendoGrid({
        dataSource: dsDetailHis,
        filterable: kendoGridFilterable,
        sortable: true,
        reorderable: true,
        resizable: true,
        pageable: true,
        columns: [
        { field: "shipmentId", title: "shipment ID" },
        { field: "noSo", title: "No SO" },
        { field: "tanggalMuat", title: "Tgl Muat", template: "#= tanggalMuat != null ? kendo.toString(kendo.parseDate(tanggalMuat, 'yyyy-MM-dd'), 'dd/MM/yyyy') : ''#" },
        { field: "jenisOrder", title: "Jenis Order" },
        { field: "customer", title: "Customer" },
        { field: "rute", title: "Rute" },
        ],
    });
}

    function call_piutang_kpd(piutang_type, driverId) {
        var dsPiutangKpd = new kendo.data.DataSource({
            transport: {
                read: {
                    url: '/Driver/BindingPiutangHistory?id=' + driverId + '&piutang_type=' + piutang_type, dataType: "json"
                },
            },
            schema: {
                model: {
                    fields: {
                        Id: { type: "number" },
                        Tanggal: { type: "string" },
                        Keterangan: { type: "string" },
                        Jumlah: { type: "number" },
                        Saldo: { type: "number" },
                    }
                }
            },
        });
        $("#GridAUJBatal").kendoGrid({
            dataSource: dsPiutangKpd,
            columns: [
                { field: "Tanggal", template: "#= kendo.toString(kendo.parseDate(Tanggal, 'yyyy-MM-dd'), 'dd/MM/yyyy HH:MM:ss') #", width: "86px" },
                { field: "Keterangan", width: "800px" },
                { field: "Jumlah", template: 'Rp #: kendo.format("{0:n}", Jumlah)#', attributes: { style: "text-align:right;" }, width: "77px" },
                { field: "Saldo", template: 'Rp #: kendo.format("{0:n}", Saldo)#', attributes: { style: "text-align:right;" }, width: "75px" }
            ],
        }).data("kendoGrid");
    }

$(document).ready(function () {
    IdSo = 0;
    dsMasterDriver = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Driver/BindingDetailSo?idSo=' + IdSo,
                dataType: "json"
            },
        },
        schema: {
            total: "total",
            data: "data",
            model: {
                fields: {
                    "Id": { type: "number" },
                    "Status": { type: "string" },
                    "StatusSo" : {type: "string"},
                    "KodeDriver": { type: "string" },
                    "KodeDriverOld": { type: "string" },
                    "NamaDriver": { type: "string" },
                    "NamaPangilan": { type: "string" },
                    "DokumenPending": { type: "string" },
                    "TglBerlakuSim": { type: "string" },
                    "Training": { type: "string" },
                }
            }
        },
        pageSize: 5,
        pageable: true,
        sortable: true,
    });

    gridMasterDriver = $("#GridMasterDriver").kendoGrid({
        dataSource: dsMasterDriver,
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
                        click: SelectDriver,
                        imageClass: "glyphicon glyphicon-ok",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=iconClass# #=imageClass#"></span></a>'
                    }
                ],
                width: "50px"
            },
            { field: "StatusSo", title: "Status Order" },
            { field: "KodeDriver", title: "Kode Driver" },
            { field: "KodeDriverOld", title: "Kode Driver Lama" },
            { field: "NamaDriver", title: "Nama Driver", template: "<a href='\\#' data-toggle='modal' data-target='\\#ModalHistoryJalan' onclick='GetHistoryJalan(#:Id#)'> #= NamaDriver # </a>" },
            { field: "NamaPangilan", title: "Nama Panggilan" },
            { field: "DokumenPending", title: "Dokumen Pending" },
            { field: "TglBerlakuSim", title: "Sim Expired", template: "#= TglBerlakuSim != null ? kendo.toString(kendo.parseDate(TglBerlakuSim, 'yyyy-MM-dd'), 'dd/MM/yyyy') : ''#" },
            { field: "Training", title: "Training" }
        ],
    }).data("kendoGrid");

    gridMasterDriverPiutang = $("#GridMasterDriverPiutang").kendoGrid({
        dataSource: dsMasterDriver,
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
                        click: SelectDriver,
                        imageClass: "glyphicon glyphicon-ok",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=iconClass# #=imageClass#"></span></a>'
                    }
                ],
                width: "50px"
            },
            { field: "StatusSo", title: "Status Order" },
            { field: "KodeDriver", title: "Kode Driver" },
            { field: "KodeDriverOld", title: "Kode Driver Lama" },
            { field: "NamaDriver", title: "Nama Driver", template: "<a href='\\#' data-toggle='modal' data-target='\\#ModalAUJBatal' onclick=\"call_piutang_kpd('B', #:Id#)\"> #= NamaDriver # </a>" },
            { field: "NamaPangilan", title: "Nama Panggilan" },
            { field: "DokumenPending", title: "Dokumen Pending" },
            { field: "TglBerlakuSim", title: "Sim Expired", template: "#= TglBerlakuSim != null ? kendo.toString(kendo.parseDate(TglBerlakuSim, 'yyyy-MM-dd'), 'dd/MM/yyyy') : ''#" },
            { field: "Training", title: "Training" }
        ],
    }).data("kendoGrid");

    var dsHB = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Driver/BindingSettlementBatal?id=' + $('#IdDriver1').val(),
                dataType: "json"
            },
        },
        schema: {
            total: "total",
            data: "data",
            model: {
                fields: {
                    "Id": {type: "number"}, "JenisOrder": {type: "string"}, "NoDn": {type: "string"}, "NoSo": {type: "string"},
                    "Customer": {type: "string"}, "VehicleNo": {type: "string"}, "Driver": {type: "string"}, "JenisBatal": {type: "string"},
                    "Tanggal": {type: "date"}, "IsProses": { type: "bool" }
                }
            }
        },
        pageSize: 10, pageable: true, sortable: true,
    });

    $("#GridHistoryBatal").kendoGrid({
        dataSource: dsHB, filterable: kendoGridFilterable, sortable: true, reorderable: true, resizable: true, pageable: true, groupable: true,
        columns: [
            {field: "JenisOrder", title: "Jenis Order"}, {field: "NoDn", title: "No.DN"}, {field: "NoSo", title: "No.SO"}, {field: "Customer"},
            {field: "VehicleNo", title: "No.Polisi"}, {field: "Driver"},
            {
                field: "Tanggal", title: "Tanggal Batal",
                template: "#= Tanggal != null ? kendo.toString(kendo.parseDate(Tanggal, 'yyyy-MM-dd'), 'dd/MM/yyyy') : ''#",
            },
            {field: "JenisBatal", title: "Jenis Batal"},
        ],
    }).data("kendoGrid");
    $("#GridAUJBatal").kendoGrid({
        dataSource: [],
        columns: [
            { field: "Tanggal", template: "#= kendo.toString(kendo.parseDate(Tanggal, 'yyyy-MM-dd'), 'dd/MM/yyyy HH:MM:ss') #", width: "86px" },
            { field: "Keterangan", width: "800px" },
            { field: "Jumlah", template: 'Rp #: kendo.format("{0:n}", Jumlah)#', attributes: { style: "text-align:right;" }, width: "77px" },
            { field: "Saldo", template: 'Rp #: kendo.format("{0:n}", Saldo)#', attributes: { style: "text-align:right;" }, width: "75px" }
        ],
    }).data("kendoGrid");
})