var dsMasterTruk;
var gridMasterTruk;
$(document).ready(function () {
    dsMasterTruk = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/SalesOrderKontrak/GetItemTruckKonfirmasi?idSo=' + $("#SalesOrderKontrakId").val(),
                dataType: "json"
            },
        },
        schema: {
            total: "total",
            data: "data",
            model: {
                fields: {
                    "Id": { type: "number", defaultValue: 0 },
                    "IdTruk": { type: "number" },
                    "Status": { type: "string" },
                    "VehicleNumber": { type: "string" },
                    "JenisTruck": { type: "string" },
                    "IdDriver1": { type: "number" },
                    "KodeDriver1": { type: "string" },
                    "NamaDriver1": { type: "string" },
                    "IdDriver2": { type: "number" },
                    "KodeDriver2": { type: "string" },
                    "NamaDriver2": { type: "string" },
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
            {field: "Status",title: "Status"},
            {field: "VehicleNumber",title: "Vehicle Number"},
            {field: "JenisTruck",title: "Jenis Truk"},
            {field: "KodeDriver1",title: "ID Driver 1"},
            {field: "NamaDriver1",title: "Nama Driver 1"},
            { field: "KodeDriver2", title: "ID Driver 2" },
            { field: "NamaDriver2", title: "Nama Driver 2" },
        ],
    }).data("kendoGrid");
})