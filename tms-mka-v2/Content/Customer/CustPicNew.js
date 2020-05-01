var currentTr;
var modalPic;
var formPic;
var validatorPic;

var cboDept;
var cboJabatan;

var dsDept;
var dsJabatan;

var GridCustPic;

$(document).ready(function () {
    //grid
    var dsGridPic = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/BindingPic?idCust=' + $("#Id").val(),
                dataType: "json"
            },
        },
        schema: {
            data: "data",
            model: {
                fields: {
                    "Id" : { type: "number" },
                    "CustomerId": { type: "number" },
                    "Code": { type: "string" },
                    "Nama" : { type: "string" },
                    "DepartemenId" : { type: "number" },
                    "Dept" : { type: "string" },
                    "JabatanId" : { type: "number" },
                    "Jabatan" : { type: "string" },
                    "EmailAdd" : { type: "string" },
                    "Mobile": { type: "string" }
                }
            }
        },
    });

    GridCustPic = $("#GridCustPic").kendoGrid({
        dataSource: dsGridPic,
        columns: [
            {
                command: [
                    {
                        name: "edit",
                        text: "edit",
                        click: editPic,
                        imageClass: "glyphicon glyphicon-edit",
                        template: '<a class="k-button-icon #=className#" #=attr# ><span class="#=imageClass#"></span></a>'
                    },
                    {
                        name: "delete",
                        text: "delete",
                        click: deletePic,
                        imageClass: "glyphicon glyphicon-remove",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=iconClass# #=imageClass#"></span></a>'
                    }
                ],
                width: "60px"
            },
            {
                field: "Code",
                title: "Kode"
            },
            {
                field: "Nama",
                title: "Nama"
            },
            {
                field: "Dept",
                title: "Departemen"
            },
            {
                field: "Jabatan",
                title: "Jabatan"
            },
            {
                field: "EmailAdd",
                title: "Email"
            },
            {
                field: "Mobile",
                title: "Hp"
            },
        ],
    }).data("kendoGrid");

    modalPic = $("#modalFormPic");
    formPic = $("#formPic");

    validatorPic = formPic.kendoValidator().data("kendoValidator");

    //combobox
    dsDept = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetDepartment',
                dataType: "json"
            },
        },
    });

    var dsJabatan = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/GetJabatan',
                dataType: "json"
            },
        },
    });

    cboDept = $("#DepartemenId").kendoComboBox({
        dataTextField: "Nama",
        dataValueField: "Id",
        dataSource: dsDept,
        filter: "contains",
        suggest: true,
        index: 3
    }).data("kendoComboBox");

    cboJabatan = $("#JabatanId").kendoComboBox({
        dataTextField: "Nama",
        dataValueField: "Id",
        dataSource: dsJabatan,
        filter: "contains",
        suggest: true,
        index: 3
    }).data("kendoComboBox");

    //$("#MobilePic").kendoMaskedTextBox({
    //    mask: "000-000-000-000"
    //});
});

function ShowPicPopup() {
    $('.k-invalid-msg').hide();
    $('#CustomerPicId').val(0);
    $('#Code').val("");
    $('#Name').val("");
    cboDept.value();
    cboDept.text('');
    cboJabatan.value('');
    cboJabatan.text();
    $('#EmailAdd').val("");
    $('#MobilePic').val("");
}

function SavePic() {
    if (validatorPic.validate())
    {
        var data = {
            Id: $('#CustomerPicId').val(),
            Code : $('#Name').val(),
            Nama: $('#Name').val(),
            CustomerId: $('#Id').val(),
            DepartemenId: cboDept.value(),
            JabatanId: cboJabatan.value(),
            EmailAdd: $('#EmailAdd').val(),
            Mobile: $('#MobilePic').val()
        };

        //modalPic.modal('hide');

        goToSavePage("/Customer/CustomerSavePIC/", data, GridCustPic.dataSource);
        modalPic.modal('hide');
    }
}

function deletePic(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    goToDeletePage("/Customer/CustomerDeletePIC?CustId=" + $("#Id").val() + "&id=" + data.Id, this.dataSource);
}

function editPic(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    $('.k-invalid-msg').hide();
    $('#CustomerPicId').val(data.Id);
    $('#Code').val(data.Code);
    $('#Name').val(data.Nama);
    cboDept.value(data.DepartemenId);
    cboJabatan.value(data.JabatanId);
    $('#EmailAdd').val(data.EmailAdd);
    $('#MobilePic').data("kendoMaskedTextBox").value(data.Mobile);
    modalPic.modal('show');
}