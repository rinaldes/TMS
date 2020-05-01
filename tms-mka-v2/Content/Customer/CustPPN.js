var formPPN;
var validatorPPN;

var dsRekening;

function getRekening(stat) {
    dsRekening = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetCustRekening?stat=' + stat,
                dataType: "json"
            },
        },
    });

    var combobox;
    combobox = $("#IdRekening").data("kendoComboBox");
    var temp = $("#IdRekening").val();
    combobox.text('');
    combobox.value();
    combobox.setDataSource(dsRekening);
    combobox.value(temp)
    //refresh page
    $(".k-invalid-msg").hide();
    var myElement = document.querySelector("#NoNpwp");
    if (stat == "PPN") {
        myElement.style.backgroundColor = "";
        $("#NoNpwp").data("kendoMaskedTextBox").enable(true);
        $('#NamaNpwp').prop('readonly', false);
        $('#AddressNpwp').prop('readonly', false);
    }
    else {
        myElement.style.backgroundColor = "#EEF1F5";
        $("#NoNpwp").data("kendoMaskedTextBox").enable(false);
        $('#NoNpwp').val('');
        $('#NamaNpwp').prop('readonly', true);
        $('#NamaNpwp').val('');
        $('#AddressNpwp').prop('readonly', true);
        $('#AddressNpwp').val('');
    }
}

$(document).ready(function () {
    formPPN = $("#formPPN");

    validatorPPN = formPPN.kendoValidator({
        rules: {
            isReq: function (input) {
                console.log()
                if (input.data("isreqField") == "IsPPn")
                {
                    console.log(input.val() != "")
                    return input.val() != "";
                }

                return true;
            }
        }
    }).data("kendoValidator");

    $("#NoNpwp").kendoMaskedTextBox({
        mask: "00.000.000.0 - 000.000"
    });


    $("#IdRekening").kendoComboBox({
        dataTextField: "NamaNoRekening",
        dataValueField: "Id",
        filter: "contains",
        suggest: true,
        index: 3
    });

    getRekening(statPPN);

    formPPN.submit(function (e) {
        if (!validatorPPN.validate()) {
            e.preventDefault();
            return false;
        }
    })
})