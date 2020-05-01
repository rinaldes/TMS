var currentTr;
var modalProductType;
var formProductType;
var validatorProductType;

var cboProduct;

var dsProduct;

var GridCustProduct;
$(document).ready(function () {
    //grid
    var dsGridProduct = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/BindingProduct?idCust=' + $("#Id").val(),
                dataType: "json"
            },
        },
        schema: {
            data: "data",
            model: {
                fields: {
                    "Id": { type: "number" },
                    "CustomerId": { type: "number" },
                    "idProduk": { type: "number" },
                    "NamaProduct": { type: "string" },
                    "Kategori": { type: "string" },
                    "TargetSuhu": { type: "number" },
                    "SuhuMax": { type: "number" },
                    "SuhuMin": { type: "number" },
                    "Interval": { type: "number" },
                    "PenangananKhusus": { type: "string" },
                    "Keterangan": { type: "string" },
                }
            }
        },
    });

    GridCustProduct = $("#GridCustProduct").kendoGrid({
        dataSource: dsGridProduct,
        columns: [
            {
                command: [
                    {
                        name: "edit",
                        text: "edit",
                        click: editProduct,
                        imageClass: "glyphicon glyphicon-edit",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=imageClass#"></span></a>'
                    },
                    {
                        name: "delete",
                        text: "delete",
                        click: deleteProduct,
                        imageClass: "glyphicon glyphicon-remove",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=iconClass# #=imageClass#"></span></a>'
                    }
                ],
                width: "60px"
            },
            {
                field: "NamaProduct",
                title: "Nama Product"
            },
            {
                field: "Kategori",
                title: "Kategori"
            },
            {
                field: "TargetSuhu",
                title: "Target Suhu"
            },
            {
                field: "SuhuMax",
                title: "Suhu Max"
            },
            {
                field: "SuhuMin",
                title: "Suhu Min"
            },
            {
                field: "Interval",
                title: "Interval Alert"
            },
            {
                field: "PenangananKhusus",
                title: "Penanganan Khusus"
            },
            {
                field: "Keterangan",
                title: "Keterangan"
            },
        ],
    }).data("kendoGrid");

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

function ShowFromPt() {
    $('.k-invalid-msg').hide();
    cboProduct.value('');
    cboProduct.text('');
    $("#ProductTypeId").val(0);
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

function SavePt() {
    if (validatorProductType.validate())
    {
        var treatment = "";
        var selectedTreatment = [];
        $('.checkbox-list input:checked').each(function () {
            selectedTreatment.push(this.value);
        });
        treatment = selectedTreatment.join(", ");

        var data = {
            Id: $("#ProductTypeId").val(),
            CustomerId: $("#Id").val(),
            idProduk: cboProduct.value(),
            PenangananKhusus: treatment,
            Keterangan: $('#KeteranganPt').val()
        };

        goToSavePage("/Customer/CustomerSaveProduct/", data, GridCustProduct.dataSource);
        modalProductType.modal('hide');
    }
}

function deleteProduct(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    goToDeletePage("/Customer/CustomerDeleteProduct?CustId=" + $("#Id").val() + "&id=" + data.Id, this.dataSource);
}

function editProduct(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));

    cboProduct.value(data.idProduk);
    $('#ProductTypeId').val(data.Id);
    $('#CategoryPt').val(data.Kategori);
    $('#TargetSuhuPt').val(data.TargetSuhu);
    $('#SuhuMaxPt').val(data.SuhuMax);
    $('#SuhuMinPt').val(data.SuhuMin);
    $('#TresholdPt').val(data.Interval);
    $('#KeteranganPt').val(data.Keterangan);
    $('.checkbox-list').find('span').removeClass('checked');
    $('.checkbox-list').find('input:checkbox').prop('checked', false);
    if (data.PenangananKhusus !== null)
    {
        var substrST = data.PenangananKhusus.split(', ');
        for (var i = 0; i < substrST.length; i++) {
            $(".checkbox-list input:checkbox").each(function () {
                if ($(this).val() == substrST[i]) {
                    $(this).prop('checked', true)
                    $(this).parent().addClass("checked");
                }
            });
        }
    }
    modalProductType.modal('show');
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