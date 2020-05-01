var dsStat;

$(document).ready(function () {
    dsStat = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Driver/BindingTrainingHistory?id=' + $("#Id").val(),
                dataType: "json"
            },
        },
        schema: {
            model: {
                fields: {
                    Training: { type: "string" },
                    Materi: { type: "string" },
                    Nilai: { type: "string" },
                    Tanggal: { type: "string" },
                    Status: { type: "string" },
                    keterangan: { type: "string" },
                }
            }
        },
    });

    $("#gridTraining").kendoGrid({
        dataSource: dsStat,
        columns: [
            {
                field: "Training",
                title: "Training"
            },
            {
                field: "Materi",
                title: "Materi"
            },
            {
                field: "Nilai",
                title: "Nilai"
            },
            {
                field: "Tanggal",
                title: "Tanggal",
                template: "#= kendo.toString(kendo.parseDate(Tanggal, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
            },
            {
                field: "keterangan",
                title: "Pengulangan Ke"
            },
        ],
    }).data("kendoGrid");
});