var currentTr;
var modalNotif;
var formNotif;
var validatorNotif;

var gridRute;
var checkedIds = {};
var checkedNama = {};
var truckId = [];
var truckName = [];

var GridCustNotif;
$(document).ready(function () {
    var dsGridNotif = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Customer/BindingNotif?idCust=' + $("#Id").val(),
                dataType: "json"
            },
        },
        schema: {
            data: "data",
            model: {
                fields: {
                    Id : { type:"number" },
                    CustomerId : { type:"number" },
                    IsActive : { type:"boolean" },
                    IdPic : { type:"number" },
                    Nama : { type:"string" },
                    Jabatan : { type:"string" },
                    Email : { type:"string" },
                    Sms : { type:"string" },
                    NotifType : { type:"string" },
                    strIdRute : { type:"string" },
                    strRute: { type: "string" },
                    strIdTruck: { type: "string" },
                    strTruck: { type: "string" },
                }
            }
        },
    });
    GridCustNotif = $("#GridCustNotif").kendoGrid({
        dataSource: dsGridNotif,
        columns: [
            {
                command: [
                    {
                        name: "edit",
                        text: "edit",
                        click: editNotif,
                        imageClass: "glyphicon glyphicon-edit",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=imageClass#"></span></a>'
                    },
                    {
                        name: "delete",
                        text: "delete",
                        click: deleteNotif,
                        imageClass: "glyphicon glyphicon-remove",
                        template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=iconClass# #=imageClass#"></span></a>'
                    }
                ],
                width: "60px"
            },

            {
                field: "IsActive",
                title: "Status",
                template: "#=IsActive ? 'Aktif' : 'Tidak Aktif'#"

            },
            {
                field: "Nama",
                title: "Nama"
            },
            {
                field: "Jabatan",
                title: "Jabatan"
            },
            {
                field: "Email",
                title: "Email"
            },
            {
                field: "Sms",
                title: "Sms"
            },
            {
                field: "NotifType",
                title: "Notifikasi"
            },
            {
                field: "strRute",
                title: "Rute"
            },
            {
                field: "strTruck",
                title: "Truck"
            },
        ],
    }).data("kendoGrid");

    modalNotif = $("#modalFormNotif");
    formNotif = $("#formNotif");

    validatorNotif = formNotif.kendoValidator({
        rules: {
            trukreq: function (input) {
                if (input.is("[name=HidTruk]") && truckId.length == 0) {
                    return false;
                }

                return true;
            },
            rutereq: function (input) {
                if (input.is("[name=HidRuteNotif]") && $("#RuteNotif").val() == '') {
                    return false;
                }

                return true;
            },
        }
    }).data("kendoValidator");

    var dsRute = new kendo.data.DataSource({
        transport: {
            read: {
                url: '/Rute/Binding',
                dataType: "json"
            },
            parameterMap: function (options, operation) {
                if (operation !== "read" && options != '') {
                    return kendo.stringify(options);
                }
                else if (operation == "read") {
                    if (options.filter) {
                        filter = options.filter.filters;
                        for (var i in filter) {
                            if (filter[i].field == "Asal") {
                                filter[i].field = "LocationAsal.Nama";
                            }
                            if (filter[i].field == "AreaAsal") {
                                filter[i].field = "AreaAsal.Nama";
                            }
                            if (filter[i].field == "Tujuan") {
                                filter[i].field = "LocationTujuan.Nama";
                            }
                            if (filter[i].field == "AreaTujuan") {
                                filter[i].field = "AreaTujuan.Nama";
                            }
                            if (filter[i].field == "Multidrop") {
                                filter[i].field = "Multidrop.tujuan";
                            }
                        }
                    }

                    if (options.sort) {
                        sort = options.sort;
                        for (var i in sort) {
                            if (sort[i].field == "Asal") {
                                sort[i].field = "LocationAsal.Nama";
                            }
                            if (sort[i].field == "AreaAsal") {
                                sort[i].field = "AreaAsal.Nama";
                            }
                            if (sort[i].field == "Tujuan") {
                                sort[i].field = "LocationTujuan.Nama";
                            }
                            if (sort[i].field == "AreaTujuan") {
                                sort[i].field = "AreaTujuan.Nama";
                            }
                            if (sort[i].field == "Multidrop") {
                                sort[i].field = "Multidrop.tujuan";
                            }
                        }
                    }
                    return options;
                }
            }
        },
        schema: {
            total: "total",
            data: "data",
            model: {
                fields: {
                    "Id": { type: "number" },
                    "Kode": { type: "string" },
                    "Nama": { type: "string" },
                    "Asal": { type: "string" },
                    "AreaAsal": { type: "string" },
                    "Tujuan": { type: "string" },
                    "AreaTujuan": { type: "string" },
                    "MultiDrop": { type: "string" },
                    "Jarak": { type: "number" },
                    "WaktuKerja": { type: "number" },
                    "WatkuTempuh": { type: "number" },
                    "Toleransi": { type: "number" },
                    "Keterangan": { type: "number" },
                }
            }
        },
        pageSize: 10,
        pageable: true,
        serverFiltering: true,
        serverPaging: true,
        serverSorting: true,
        sortable: true,
        //sort: { field: "SubmittedDate", dir: "desc" }
    });

    gridRute = $("#GridRute").kendoGrid({
        dataSource: dsRute,
        filterable: kendoGridFilterable,
        sortable: true,
        reorderable: true,
        resizable: true,
        pageable: true,
        dataBound: onDataBound,
        columns: [
            {
                template: "<input type='checkbox' class='checkbox' />",
                width: "50px"
            },
            {
                field: "Nama",
                title: "Nama Rute"
            },
            {
                field: "Asal",
                title: "Dari"
            },
            {
                field: "Tujuan",
                title: "Tujuan"
            },
            {
                field: "MultiDrop",
                title: "MultiDrop"
            },
                    
        ],
    }).data("kendoGrid");

    gridRute.table.on("click", ".checkbox", selectRow);

    $('#chkAll').change(function () {
        if ($(this).is(":checked")) {
            $("input:checkbox[name=chkTruck]").prop('checked', true);
            var checkedValues = $("input:checkbox[name=chkTruck]").map(function () {
                return this.value.split('|')[0];
            }).get();
            var checkedValuesName = $("input:checkbox[name=chkTruck]").map(function () {
                return this.value.split('|')[1];
            }).get();
            truckId = checkedValues;
            truckName = checkedValuesName;
        }
        else {
            $("input:checkbox[name=chkTruck]").prop('checked', false);
            truckId = [];
            truckName = [];
        }
    });

    $("input:checkbox[name=chkTruck]").on('click', function () {
        $("#chkAll").prop('checked', false);
        if ($(this).is(':checked'))
        {
            truckId.push(this.value.split('|')[0]);
            truckName.push(this.value.split('|')[1]);
        }
        else
        {
            var valremove = this.value.split('|')[0];
            truckId = jQuery.grep(truckId, function(value) {
              return value != valremove;
            });
            var valremoveName = this.value.split('|')[1];
            truckName = jQuery.grep(truckName, function (value) {
                return value != valremoveName;
            });
        }
    });
});

function refreshRute()
{
    if ($('#IdRuteNotif').val() == "")
    {
        gridRute.dataSource.read();
        gridRute.refresh();
    }
}

function selectRow() {
    var checked = this.checked,
    row = $(this).closest("tr"),
    dataItem = gridRute.dataItem(row);

    checkedIds[dataItem.Id] = checked;
    checkedNama[dataItem.Nama] = checked;
    if (checked) {
        //-select the row
        row.addClass("k-state-selected");
    } else {
        //-remove selection
        row.removeClass("k-state-selected");
    }
}

function onDataBound(e) {
    var view = this.dataSource.view();
    for (var i = 0; i < view.length; i++) {
        console.log(checkedIds)
        if (checkedIds[view[i].Id]) {
            this.tbody.find("tr[data-uid='" + view[i].uid + "']")
            .addClass("k-state-selected")
            .find(".checkbox")
            .attr("checked", "checked");
        }
    }
}

function SelectRute()
{
    var checked = [];
    var checked2 = [];
    for (var i in checkedIds) {
        if (checkedIds[i]) {
            checked.push(i);
        }
    }
    for (var i in checkedNama) {
        if (checkedNama[i]) {
            checked2.push(i);
        }
    }

    $('#IdRuteNotif').val(checked.join(','));
    $('#RuteNotif').val(checked2.join(', '));
    $('#modalGridRute').modal('hide');
}

function ShowNotifPopup() {
    $('.k-invalid-msg').hide();
    $('#CustomerNotifId').val(0);
    $('#NamaPicNotif').val("");
    $('#JabatanPicNotif').val("");
    $('#EmailPicNotif').val("");
    $('#MobilePicNotif').val("");
    $('#IdRuteNotif').val("");
    $('#RuteNotif').val("");
    gridRute.dataSource.read();
    gridRute.refresh();
    $("input:checkbox").prop('checked', false);
    truckId = [];
    truckName = [];
    checkedIds = {};
    checkedNama = {};
}

function SaveNotif(conditon) {
    if (validatorNotif.validate())
    {
        var stat;
        if ($('input:checkbox[name=StatusNotif]').is(':checked')) {
            stat = true;
        }
        else {
            stat = false;
        }

        var data = {
            Id: $('#CustomerNotifId').val(),
            CustomerId: $("#Id").val(),
            IsActive: stat,
            IdPic: $('#IdPicNotif').val(),
            NotifType: $('#rdTypeNotif:checked').val(),
            strIdRute: $('#IdRuteNotif').val(),
            strIdTruck: truckId.join(),
        };
        console.log(data)
        goToSavePage("/Customer/CustomerSaveNotif/", data, GridCustNotif.dataSource);
        modalNotif.modal('hide');
    }
}

function deleteNotif(e) {
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    goToDeletePage("/Customer/CustomerDeleteNotif?CustId=" + $("#Id").val() + "&id=" + data.Id, this.dataSource);
}

function editNotif(e)
{
    e.preventDefault();
    var data = this.dataItem(getDataRowGrid(e));
    $('.k-invalid-msg').hide();
    $("input:checkbox").prop('checked', false);
    $('#CustomerNotifId').val(data.Id);
    if (data.IsActive == true)
        $(":checkbox[name=StatusNotif]").prop("checked", true);
    $('#IdPicNotif').val(data.IdPic);
    $('#NamaPicNotif').val(data.Nama);
    $('#JabatanPicNotif').val(data.Jabatan);
    $('#EmailPicNotif').val(data.Email);
    $('#MobilePicNotif').val(data.Sms);
    $(":radio[id=rdTypeNotif][value=" + data.NotifType + "]").prop("checked", true);
    $('#IdRuteNotif').val(data.strIdRute);
    $('#RuteNotif').val(data.strRute);
    checkedIds = {};
    checkedNama = {};
    var substrRute = data.strIdRute.split(',');
    for (var i = 0; i < substrRute.length; i++) {
        checkedIds[substrRute[i]] = true;
    }
    var substrRuteNama = data.strRute.split(', ');
    for (var i = 0; i < substrRuteNama.length; i++) {
        checkedNama[substrRuteNama[i]] = true;
    }
    gridRute.dataSource.read();
    gridRute.refresh();

    truckId = [];
    truckName = [];
    var truckList = data.strIdTruck;
    var substr = truckList.split(',');
    for (var i = 0; i < substr.length; i++) {
        truckId.push(substr[i]);
        $(":checkbox[name=chkTruck][value=" + substr[i] + "]").prop("checked", true);
    }
    

    if($(":checkbox[name=chkTruck]").length == substr.length)
    {
        $(":checkbox[name=AllTruck]").prop("checked", true);
    }

    modalNotif.modal('show');
}