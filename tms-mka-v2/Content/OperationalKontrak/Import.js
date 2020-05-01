var dsSoImport;
var gridSoImport;
var gridDeatilTruck;

function selectSOImport(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    gridItemTruk.dataSource.data([]);
    $.ajax({
        url: '/SalesOrderKontrak/GetTruckImport?idSo=' + data.SalesOrderKontrakId,
        type: 'GET',
        dataType: 'Json',
        cache: false,
        success: function (obj) {
            console.log(obj)
            for (var i = 0; i < obj.length; i++) {
                if (obj[i].StatusTruk == "Available") {
                    gridItemTruk.dataSource.add({
                        IdTruk: obj[i].Id,
                        Status: obj[i].StatusTruk,
                        VehicleNumber: obj[i].VehicleNo,
                        JenisTruck: obj[i].JenisTruk,
                        IdDriver1: '',
                        KodeDriver1: '',
                        NamaDriver1: '',
                        IdDriver2: '',
                        KodeDriver2: '',
                        NamaDriver2: '',
                    });
                }
            }
        }
    });
    $("#modalSo").modal('hide');
}

function viewSOImport(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));

    var dsDetailTrukImport = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/SalesOrderKontrak/GetTruckImport?idSo=' + data.SalesOrderKontrakId,
                dataType: "json"
            },
        },
        schema: {
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
                    "IdDriver2": { type: "number" },
                    "KodeDriver2": { type: "string" },
                    "NamaDriver2": { type: "string" },
                }
            }
        },
        pageSize: 10,
        pageable: true,
        sortable: true,
    });

    gridDeatilTruck.setDataSource(dsDetailTrukImport);
    
    $("#modalDetailTruck").modal('show');
}

$(document).ready(function () {
    dsSoImport = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/SalesOrderKontrak/Binding',
                dataType: "json"
            },
        },
        schema: {
            total: "total",
            data: "data",
            id: "SalesOrderId",
            model: {
                fields: {
                    "SalesOrderId": { type: "number" },
                    "SONumber": { type: "string" },
                    "KodeCustomer": { type: "string" },
                    "NamaCustomer": { type: "string" },
                    "TanggalMuat": { type: "date" },
                    "StrJenisTruck": { type: "string" },
                    "Rit": { type: "number" },
                    "Keterangan": { type: "string" },
                    "Status": { type: "string" },
                    "IsReturn": { type: "bool" },
                    "PeriodStr" : { type: "date" },
                    "PeriodEnd": { type: "date" },
                }
            }
        },
        pageSize: 10,
        pageable: true,
        sortable: true,
        sort: { field: "DateStatus", dir: "desc" },
    });

    gridSoImport = $("#GridSO").kendoGrid({
        dataSource: dsSoImport,
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
                        name: "Select",
                        text: "Select",
                        click: selectSOImport,
                        imageClass: "glyphicon glyphicon-ok",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=imageClass#"></span></a>'
                    },
                    {
                        name: "View",
                        text: "View",
                        click: viewSOImport,
                        imageClass: "glyphicon glyphicon-eye-open",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=iconClass# #=imageClass#"></span></a>'
                    }
                ],
                width: "70px"
            },
            {
                field: "SONumber",
                title: "No Kontrak",
                width: 150,
                //template: kendo.template($("#so-template").html()),
            },
            {
                field: "KodeCustomer",
                title: "Kode Customer",
                width: 180
            },
            {
                field: "NamaCustomer",
                title: "Customer",
                width: 200
            },
            {
                field: "PeriodStr",
                title: "Periode Awal",
                template: "#= PeriodStr != null ? kendo.toString(kendo.parseDate(PeriodStr, 'yyyy-MM-dd'), 'dd/MM/yyyy') : ''#",
                width: 160,
                groupHeaderTemplate: "Tanggal Muat : #= value != null ? kendo.toString(kendo.parseDate(value, 'yyyy-MM-dd'), 'dd/MM/yyyy') : '' #"
            },
            {
                field: "PeriodEnd",
                title: "Periode Akhir",
                template: "#= PeriodEnd != null ? kendo.toString(kendo.parseDate(PeriodEnd, 'yyyy-MM-dd'), 'dd/MM/yyyy') : ''#",
                width: 160,
                groupHeaderTemplate: "Tanggal Muat : #= value != null ? kendo.toString(kendo.parseDate(value, 'yyyy-MM-dd'), 'dd/MM/yyyy') : '' #"
            },
            {
                field: "StrJenisTruck",
                title: "Jenis Truk",
                width: 170
            },
            {
                field: "Rit",
                title: "Rit",
                width: 100
            },
            {
                field: "Keterangan",
                title: "Keterangan",
                width: 250
            },
        ],
    }).data("kendoGrid");

    gridDeatilTruck = $("#GridDetailSo").kendoGrid({
        filterable: kendoGridFilterable,
        sortable: true,
        reorderable: true,
        resizable: true,
        pageable: true,
        groupable: true,
        columns: [
            { field: "StatusTruk", title: "Status" },
            { field: "VehicleNo", title: "Vehicle Number" },
            { field: "JenisTruk", title: "Jenis Truk" },
            { field: "Merk", title: "Jenis Truk" },
        ],
    }).data("kendoGrid");
})