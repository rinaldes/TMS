var GridComment;

$(document).ready(function () {
    var dsComment = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/SalesOrderOnCall/BindingComment?id=' + $('#SalesOrderOnCallId').val(),
                dataType: "json"
            },
        },
        schema: {
            data: "data",
            model: {
                fields: {
                    "Id": { type: "number" },
                    "SalesOrderOnCallId": { type: "number" },
                    "Tanggal": { type: "date" },
                    "CommentUser": { type: "string" },
                    "Action": { type: "string" },
                    "Username": { type: "string" }
                }
            }
        },
        sortable: true,
        sort: { field: "Tanggal", dir: "desc" },
    });

    GridComment = $("#GridComment").kendoGrid({
        dataSource: dsComment,
        columns: [
            {
                field: "Tanggal",
                title: "Tanggal",
                template: "#= Tanggal != null ? kendo.toString(kendo.parseDate(Tanggal, 'yyyy-MM-dd'), 'dd/MM/yyyy HH:mm') : ''#"
            },
            {
                field: "CommentUser",
                title: "Comment"
            },
            {
                field: "Username",
                title: "User"
            },
            {
                field: "Action",
                title: "Action"
            },
        ],
    }).data("kendoGrid");
})