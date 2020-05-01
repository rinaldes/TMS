var currentTr;
var modalNotif;
var formNotif;
var validatorNotif;

var gridRute;
var checkedIds = {};
var checkedNama = {};
var truckId = [];
var truckName = [];
$(document).ready(function () {
    modalNotif = $("#modalFormNotif");
    formNotif = $("#formNotif");

    validatorNotif = formNotif.kendoValidator().data("kendoValidator");

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

function updateRowNumberNotif() {
    $('td.no_notif').each(function (i) {
        $(this).text(i + 1);
    });
}

function ShowNotifPopup() {
    $('.k-invalid-msg').hide();
    $('#Condition').val("new");
    $('#IdPicNotif').val("");
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
    var markup = "";
    $('.k-invalid-msg').hide();
    if (validatorNotif.validate()) {

        var strstat;
        var stat;
        if ($('input:checkbox[name=StatusNotif]').is(':checked'))
        {
            strstat = "Aktif";
            stat = true;
        }
        else
        {
            strstat = "Tidak Aktif";
            stat = false;
        }

        console.log(truckId)
        console.log(truckName)
        
        markup = "<input id='NotifId' name='NotifId' type='hidden' value='0'><td style='display:none;'><input value='" + stat + ";" + $('#IdPicNotif').val() + ";" + $('#rdTypeNotif:checked').val() + ";" + $('#IdRuteNotif').val() + ";" + truckId + "' name='listNotif' /></td>" +
                "<td class='no_notif'></td><td>" + strstat + "</td><td>" + $('#NamaPicNotif').val() + "</td><td>" + $('#JabatanPicNotif').val() + "</td><td>" + $('#rdTypeNotif:checked').val() + "</td><td>" + $('#EmailPicNotif').val() + "</td><td>" + $('#MobilePicNotif').val() + "</td><td>" + $('#RuteNotif').val() + "</td><td>" + truckName + "</td><td><a href='#' data-toggle='modal' data-target='#modalFormNotif' onclick='EditNotif($(this))'>Edit</a> | <a href='#' onclick='RemoveNotif($(this));'>Delete</a></td>";
        if (conditon == "new")
            $("#table-custnotif tbody").append("<tr>" + markup + "</tr>");
        else
            currentTr.closest("tr").html(markup);

        updateRowNumberNotif();
        modalNotif.modal('hide');
    }
}

function EditNotif(data)
{
    $('.k-invalid-msg').hide();
    $('#Condition').val("edit");
    $("input:checkbox").prop('checked', false);
    $('#CustomerNotifId').val(data.closest("tr").find('input').val());
    if (data.closest("tr").find('td:eq(0)').find('input').val().split(';')[0].toLowerCase() == "true")
        $(":checkbox[name=StatusNotif]").prop("checked", true);
    $('#IdPicNotif').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[1]);
    $('#NamaPicNotif').val(data.closest("tr").find('td:eq(3)').text().trim());
    $('#JabatanPicNotif').val(data.closest("tr").find('td:eq(4)').text().trim());
    $('#EmailPicNotif').val(data.closest("tr").find('td:eq(6)').text().trim());
    $('#MobilePicNotif').val(data.closest("tr").find('td:eq(7)').text().trim());
    $(":radio[id=rdTypeNotif][value=" + data.closest("tr").find('td:eq(0)').find('input').val().split(';')[2] + "]").prop("checked", true);
    $('#IdRuteNotif').val(data.closest("tr").find('td:eq(0)').find('input').val().split(';')[3]);
    $('#RuteNotif').val(data.closest("tr").find('td:eq(8)').text().trim());
    checkedIds = {};
    checkedNama = {};
    truckId = [];
    truckName = [];
    var truckList = data.closest("tr").find('td:eq(0)').find('input').val().split(';')[4];
    var substr = truckList.split(',');
    for (var i = 0; i < substr.length; i++) {
        truckId.push(substr[i]);
        $(":checkbox[name=chkTruck][value=" + substr[i] + "]").prop("checked", true);
    }
    if($(":checkbox[name=chkTruck]").length == substr.length)
    {
        $(":checkbox[name=AllTruck]").prop("checked", true);
    }
    currentTr = data;
}

function RemoveNotif(data) {
    data.closest("tr").remove();
    updateRowNumberNotif();
}