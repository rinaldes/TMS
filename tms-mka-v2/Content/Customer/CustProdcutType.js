var currentTr;
var modalProductType;
var formProductType;
var validatorProductType;

var cboProduct;

var dsProduct;

$(document).ready(function () {
    modalProductType = $("#modalFormProductType");
    formProductType = $("#formprodtype");

    validatorProductType = formProductType.kendoValidator().data("kendoValidator");

    //combobox product
    dsProduct = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetProduct',
                dataType: "json"
            },
        },
    });
    cboProduct = $("#ProductId").kendoComboBox({
        dataTextField: "NamaProduk",
        dataValueField: "Id",
        dataSource: dsProduct,
        filter: "contains",
        suggest: true,
        change: OnProductChange,
        index: 3
    }).data("kendoComboBox");
});

function updateRowNumberPt() {
    $('td.no_pt').each(function (i) {
        $(this).text(i + 1);
    });
}

function ShowFromPt() {
    $('.k-invalid-msg').hide();
    cboProduct.value('');
    cboProduct.text('');
    $('#Condition').val("new");
    $('#CategoryPt').val("");
    $('#SuhuMaxPt').val("");
    $('#SuhuMinPt').val("");
    $('#TargetSuhuPt').val("");
    $('#TresholdPt').val("");
    $('#KeteranganPt').val("");
    $('#SpecialTreatment').val("");
    $('.checkbox-list').find('span').removeClass('checked');
    $('.checkbox-list').find('input:checkbox').prop('checked', false);
}

function SavePt(conditon) {
    var markup = "";

    if (validatorProductType.validate()) {
        //validasi tambahan
        var existedDataProduct = [];
        var tdContent = "";
        $("#table-custprodtype tr").each(function (index, e) {
            if (index > 0) {
                tdContent = $(this).children('td').eq(2).text().trim();
                if (conditon == "new") {
                    existedDataProduct.push(tdContent);
                }
                else {
                    if (tdContent != currentTr.closest("tr").find('td:eq(2)').text().trim())
                        existedDataProduct.push(tdContent);
                }
            }
        });

        var treatment = "";
        var selectedTreatment = [];
        $('.checkbox-list input:checked').each(function () {
            selectedTreatment.push(this.value);
        });
        treatment = selectedTreatment.join(", ");
        
        markup = "<input id='ProdTypeId' name='ProdTypeId' type='hidden' value='0'><td style='display:none;'><input value='" + 0 + ";" + cboProduct.value() + ";" + $('#KeteranganPt').val() + ";" + treatment +
               "' name='ProdTypeData'/> </td><td class='no_pt'></td><td>" + cboProduct.text() + "</td><td>" + $('#CategoryPt').val() + "</td><td>" + $('#TargetSuhuPt').val() +
               "</td><td>" + $('#SuhuMaxPt').val() + "</td><td>" + $('#SuhuMinPt').val() + "</td><td>" + $('#TresholdPt').val() +"</td><td>" + treatment + "</td><td>" + $('#KeteranganPt').val() + "</td><td><a href='#' data-toggle='modal' data-target='#modalFormProductType' onclick='EditRowPt($(this))'>Edit</a> | <a href='#' onclick='RemoveRowPt($(this));'>Delete</a></td>";

        if ($.inArray(cboProduct.text(), existedDataProduct) != -1)
            alert("Product sudah digunakan.");
        else {
            if (conditon == "new")
                $("#table-custprodtype tbody").append("<tr>" + markup + "</tr>");
            else
                currentTr.closest("tr").html(markup);

            updateRowNumberPt();
            modalProductType.modal('hide');
        }
    }
}

function EditRowPt(data) {   
    $('#Condition').val("edit");
    cboProduct.value(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[1]);
    $('#ProductTypeId').val(data.closest("tr").find('input').val());
    $('#CategoryPt').val(data.closest("tr").find('td:eq(3)').text().trim());
    $('#TargetSuhuPt').val(data.closest("tr").find('td:eq(4)').text().trim());
    $('#SuhuMaxPt').val(data.closest("tr").find('td:eq(5)').text().trim());
    $('#SuhuMinPt').val(data.closest("tr").find('td:eq(6)').text().trim());
    $('#TresholdPt').val(data.closest("tr").find('td:eq(7)').text().trim());
    $('#KeteranganPt').val(data.closest("tr").find('td:eq(9)').text().trim());
    $('.checkbox-list').find('span').removeClass('checked');
    $('.checkbox-list').find('input:checkbox').prop('checked', false);
    var substrST = data.closest("tr").find('td:eq(8)').text().trim().split(', ');
    for (var i = 0; i < substrST.length; i++) {
        $(".checkbox-list input:checkbox").each(function () {
            if ($(this).val() == substrST[i]) {
                $(this).prop('checked', true)
                $(this).parent().addClass("checked");
            }
        });
    }
    currentTr = data;
}

function RemoveRowPt(data) {
    data.closest("tr").remove();
    updateRowNumberPt();
}

function OnProductChange(e) {
    if (this.value() != "") {
        $.ajax({
            url: '/Customer/GetProductById',
            type: 'GET',
            dataType: 'Json',
            cache: false,
            data: {
                id: this.value()
            },
            success: function (obj) {
                $('#CategoryPt').val(obj.LookupCode.Nama);
                $('#SuhuMaxPt').val(obj.MaxTemps);
                $('#SuhuMinPt').val(obj.MinTemps);
                $('#TargetSuhuPt').val(obj.TargetSuhu);
                $('#KeteranganPt').val(obj.Remarks);
                $('#TresholdPt').val(obj.Treshold);
            }
        })
    }
    else {
        $('#CategoryPt').val("");
        $('#SuhuMaxPt').val("");
        $('#SuhuMinPt').val("");
        $('#TargetSuhuPt').val("");
        $('#KeteranganPt').val("");
    }
}