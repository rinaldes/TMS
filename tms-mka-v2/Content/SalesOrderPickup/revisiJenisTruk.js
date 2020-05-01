var dsTruck;

$(document).ready(function () {
    dsTruck = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/JenisTruck/GetJnsTruck',
                dataType: "json"
            },
        },
    });

    $("#JenisTruckBaruId").kendoComboBox({
        dataTextField: "StrJenisTruck",
        dataValueField: "Id",
        dataSource: dsTruck,
        filter: "contains",
        suggest: true,
        change: truckChange
    });

    function truckChange(e) {
        if (this.value() != "") {
            $.ajax({
                url: '/JenisTruck/GetJnsTruckById?id=' + this.value(),
                type: 'GET',
                dataType: 'Json',
                cache: false,
                success: function (obj) {
                    //$("#JenisTruckBaruId").val(obj.Id);
                    console.log($("#JenisTruckBaruId").data("kendoComboBox").value());
                }
            })
        }
    };
})