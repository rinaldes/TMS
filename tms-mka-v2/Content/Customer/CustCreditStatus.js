var formCS;
var validatorCS;

var formCSHead;
var validatorCSHead;

$(document).ready(function () {
    formCS = $("#frmStat");
    validatorCS = formCS.kendoValidator({
        rules: {
            comboreq: function (input) {
                if ($(input).data("kendoComboBox")) {
                    if ($(input).data("kendoComboBox").selectedIndex == -1) {
                        return false;
                    }
                }

                return true;
            },
            
        }
    }).data("kendoValidator");
     
    cbooverride = $("#cboStatusOveride").kendoComboBox({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: [
            { text: "Lancar", value: "GREEN" },
            { text: "Perlu diperhatikan", value: "YELLOW" },
            { text: "Tidak lancar", value: "RED" }
        ],
        filter: "contains",
        suggest: true,
    }).data("kendoComboBox");

    formCSHead = $("#formCSHead");
    validatorCSHead = formCSHead.kendoValidator({
        rules: {
            conreq: function (input) {
                if (input.filter("[type=radio]") && input.attr("required")) {
                    return $("#formCSHead").find("[type=radio]").is(":checked");
                }
                return true;
            },
            overduereq: function (input) {
                if ($("input[name=ConditionCS]:checked", "#formCSHead").val() == "OVERDUE")
                {
                    if (input.is("[name=ValueOverdue2]") || input.is("[name=TOPOverdue2]")) {
                        return input.val() != "";    
                    }
                    if (input.is("[name=MinTOPOverdue1]")) {
                        if ($("#MinTOPOverdue1").val() == "" || $("#MaxTOPOverdue1").val() == "") {
                            return false;
                        }
                    }
                }

                return true;
            },
            shipmentreq: function (input) {
                if ($("input[name=ConditionCS]:checked", "#formCSHead").val() == "SHIPMENT") {
                    if (input.is("[name=ShipmentDay1]") || input.is("[name=ShipmentDay2]")) {
                        return input.val() != "";
                    }
                }

                return true;
            },
        }
    }).data("kendoValidator");
    
    if ($("input[name=ConditionCS]:checked", "#formCSHead").val() == "OVERDUE") {
        $('#MinTOPOverdue1').data("kendoNumericTextBox").enable(true);
        $('#MaxTOPOverdue1').data("kendoNumericTextBox").enable(true);
        $('#ValueOverdue2').data("kendoNumericTextBox").enable(true);
        $('#TOPOverdue2').data("kendoNumericTextBox").enable(true);
        $('#ShipmentDay1').data("kendoNumericTextBox").enable(false);
        $('#ShipmentDay2').data("kendoNumericTextBox").enable(false);
    }
    else {
        $('#MinTOPOverdue1').data("kendoNumericTextBox").enable(false);
        $('#MaxTOPOverdue1').data("kendoNumericTextBox").enable(false);
        $('#ValueOverdue2').data("kendoNumericTextBox").enable(false);
        $('#TOPOverdue2').data("kendoNumericTextBox").enable(false);
        $('#ShipmentDay1').data("kendoNumericTextBox").enable(true);
        $('#ShipmentDay2').data("kendoNumericTextBox").enable(true);
    }

    $(".rdCs").on('click', function () {
        var myElement;
        $('.k-invalid-msg').hide();
        if (this.value == "OVERDUE")
        {
            var myElement1 = document.querySelector("#MinTOPOverdue1");
            myElement1.style.backgroundColor = "white";
            $('#MinTOPOverdue1').data("kendoNumericTextBox").enable(true);
            
            var myElement2 = document.querySelector("#MaxTOPOverdue1");
            myElement2.style.backgroundColor = "white";
            $('#MaxTOPOverdue1').data("kendoNumericTextBox").enable(true);

            myElement = document.querySelector("#ValueOverdue2");
            myElement.style.backgroundColor = "white";
            $('#ValueOverdue2').data("kendoNumericTextBox").enable(true);

            myElement = document.querySelector("#TOPOverdue2");
            myElement.style.backgroundColor = "white";
            $('#TOPOverdue2').data("kendoNumericTextBox").enable(true);


            $('#ShipmentDay1').data("kendoNumericTextBox").enable(false);
            $('#ShipmentDay1').data("kendoNumericTextBox").value('');

            $('#ShipmentDay2').data("kendoNumericTextBox").enable(false);
            $('#ShipmentDay2').data("kendoNumericTextBox").value('');
        }
        else if (this.value == "SHIPMENT")
        {
            $('#MinTOPOverdue1').data("kendoNumericTextBox").enable(false);
            $('#MinTOPOverdue1').data("kendoNumericTextBox").value('');
            myElement = document.querySelector("#MinTOPOverdue1");
            myElement.style.backgroundColor = "EEF1F5";

            $('#MaxTOPOverdue1').data("kendoNumericTextBox").enable(false);
            $('#MaxTOPOverdue1').data("kendoNumericTextBox").value('');
            myElement = document.querySelector("#MaxTOPOverdue1");
            myElement.style.backgroundColor = "EEF1F5";

            $('#ValueOverdue2').data("kendoNumericTextBox").enable(false);
            $('#ValueOverdue2').data("kendoNumericTextBox").value('');
            myElement = document.querySelector("#ValueOverdue2");
            myElement.style.backgroundColor = "EEF1F5";

            $('#TOPOverdue2').data("kendoNumericTextBox").enable(false);
            $('#TOPOverdue2').data("kendoNumericTextBox").value('');
            myElement = document.querySelector("#TOPOverdue2");
            myElement.style.backgroundColor = "EEF1F5";

            $('#ShipmentDay1').data("kendoNumericTextBox").enable(true);
            $('#ShipmentDay2').data("kendoNumericTextBox").enable(true);
        }
    });

    //grid pic
    var dsGridHisCs = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/BindingHisCs?idCust=' + $("#Id").val(),
                dataType: "json"
            },
        },
        schema: {
            data: "data",
            model: {
                fields: {
                    "tanggal": { type: "string" },
                    "awal": { type: "string" },
                    "akhir": { type: "string" },
                    "keterangan": { type: "string" },
                    "user": { type: "string" }
                }
            }
        },
        pageSize: 10,
    });

    $("#gridhistrycs").kendoGrid({
        dataSource: dsGridHisCs,
        sortable: true,
        columns: [
            {
                field: "tanggal",
                title: "Tanggal",
                template: "#= tanggal != null ? kendo.toString(kendo.parseDate(tanggal, 'yyyy-MM-dd HH:ss'), 'MM/dd/yyyy HH:ss') : ''#"
            },
            {
                field: "awal",
                title: "Status Awal"
            },
            {
                field: "akhir",
                title: "Status Akhir"
            },
            {
                field: "keterangan",
                title: "Keterangan"
            },
            {
                field: "user",
                title: "User"
            }
        ],
    }).data("kendoGrid");

    formCSHead.submit(function (e) {
        if (!validatorCSHead.validate()) {
            e.preventDefault();
            return false;
        }
    })
})

function refeshStat()
{
    //cbooverride.text('');
    //cbooverride.value();
    //$('#ketCS').val('');
}

function overideStatus()
{
    if (validatorCS.validate()) {
        var myElement1 = document.querySelector("#resOverride");
        myElement1.style.backgroundColor = cbooverride.value().toLowerCase();
        $("#StatusOveride").val(cbooverride.value());
        $('#KeteranganCS').val($('#ketCS').val());
        $("#modalStatusCS").modal('hide');
    }
}