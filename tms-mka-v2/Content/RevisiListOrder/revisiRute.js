var dsTruck;
var dsRute;
var gridRute;
var listIdRuteLoad = [];
var listIdRuteUnload = [];
var multi = [];

//var dsGridLoad = [], dsGridUnLoad = [];
//var GridCustLoad, GridCustUnLoad;
//var checkedIdLoad = {}, checkedIdUnoad = {};

$(document).ready(function () {

    if (piwbeg != null && piwbeg != '') {
        swal({
            title: "Error",
            type: 'error',
            text: piwbeg,
            showCloseButton: true,
        });
    }
    function tandaanAnuDipilih() {
        var view = gridRute.dataSource.view();
        for (var i = 0; i < view.length; i++) {
            if (view[i].Id == $('#RuteId').val()) {
                gridRute.tbody.find("tr[data-uid='" + view[i].uid + "']").addClass("k-state-selected")
            }
            else {
                gridRute.tbody.find("tr[data-uid='" + view[i].uid + "']").removeClass("k-state-selected")
            }
        }
    }

    function onDataBoundLoad(e) {
        var view = this.dataSource.view();
        for (var i = 0; i < view.length; i++) {
            if (checkedIdLoad[view[i].Id]) {
                this.tbody.find("tr[data-uid='" + view[i].uid + "']")
                .addClass("k-state-selected")
                .find(".checkbox")
                .attr("checked", "checked");
                var urut = $.map(dsGridLoad, function (itemLoad) {
                    if (itemLoad.Id == view[i].Id)
                        return itemLoad.urutan;
                });
                view[i].set("urutan", urut[0])
            }
        }
    }

    function onDataBoundUnload(e) {
        var view = this.dataSource.view();
        for (var i = 0; i < view.length; i++) {
            if (checkedIdUnoad[view[i].Id]) {
                this.tbody.find("tr[data-uid='" + view[i].uid + "']")
                .addClass("k-state-selected")
                .find(".checkbox")
                .attr("checked", "checked");
                var urut = $.map(dsGridUnLoad, function (itemLoad) {
                    if (itemLoad.Id == view[i].Id)
                        return itemLoad.urutan;
                });
                view[i].set("urutan", urut[0])
            }
        }
    }

    function selectLoad() {
        var checked = this.checked,
        row = $(this).closest("tr"),
        dataItem = GridCustLoad.dataItem(row);

        GridCustLoad.closeCell();

        checkedIdLoad[dataItem.Id] = checked;
        if (checked) {
            //-select the row
            row.addClass("k-state-selected");
            dataItem.IsSelect = true;
        } else {
            //-remove selection
            row.removeClass("k-state-selected");
            dataItem.IsSelect = false;
            dataItem.set("urutan", 0);
        }
    }

    function selectUnload() {
        var checked = this.checked,
        row = $(this).closest("tr"),
        dataItem = GridCustUnLoad.dataItem(row);

        GridCustUnLoad.closeCell();

        checkedIdUnoad[dataItem.Id] = checked;
        if (checked) {
            //-select the row
            row.addClass("k-state-selected");
            dataItem.IsSelect = true;
        } else {
            //-remove selection
            row.removeClass("k-state-selected");
            dataItem.IsSelect = false;
            dataItem.set("urutan", 0);
        }
    }

    function callAjaxForRute(idForAjax) {
        $.ajax({
            url: "/Rute/GetDataForSo",
            type: 'POST',
            dataType: 'Json',
            data: {
                //id: (eksyen == "RevisiRuteOnCall" ? datarute[i] : data.Id),
                id: idForAjax,
            },
            cache: false,
            success: function (res) {
                //console.log("Response from GetDataForSO");
                //console.log(res.data);
                listIdRuteLoad.push(res.data.IdAsal)
                listIdRuteUnload.push(res.data.IdTujuan)

                //console.log("res.data.IdAsal");
                //console.log(res.data.IdAsal);
                //console.log("res.data.IdAsal");
                //console.log(res.data.IdAsal);

                if (res.data.MultiDrop != null)
                    multi.push(res.data.MultiDrop);

                $('#StrMultidrop').val(multi);
            },
        }).done(function () {            
            $.ajax({
                url: "/Customer/GetSpecLocation",
                type: 'POST',
                dataType: 'Json',
                data: {
                    id: $('#CustomerId').val(),
                    idLoad: listIdRuteLoad,
                    idUnload: listIdRuteUnload
                },
                cache: false,
                success: function (res) {
                    dsGridLoad = [];
                    res.dataLoad.forEach(function (item) {
                        dsGridLoad.push({
                            Id: item.Id,
                            Alamat: item.Alamat,
                            Provinsi: item.Provinsi,
                            Kota: item.Kota,
                            Zona: item.Zona,
                            Telp: item.Telp,
                            Fax: item.Fax,
                            urutan: 0
                        })
                    });
                    GridCustLoad.dataSource.data(dsGridLoad);

                    dsGridUnLoad = [];
                    res.dataUnload.forEach(function (item) {
                        dsGridUnLoad.push({
                            Id: item.Id,
                            Alamat: item.Alamat,
                            Provinsi: item.Provinsi,
                            Kota: item.Kota,
                            Zona: item.Zona,
                            Telp: item.Telp,
                            Fax: item.Fax,
                            urutan: 0
                        })
                    });
                    GridCustUnLoad.dataSource.data(dsGridUnLoad);
                },
            });
        })
    }

    function SelectRute(e) {
        e.preventDefault();
        var promises = [];
        var request;
        var data = this.dataItem(getDataRowGrid(e));
        var datarute = (eksyen == "RevisiRuteOnCall" ? data.ListIdRute.split(',') : datarute = data.Id)

        $('#StrLoad').val(JSON.stringify(GridCustLoad.dataSource.data()));
        $('#StrUnLoad').val(JSON.stringify(GridCustUnLoad.dataSource.data()));        

        if (eksyen == "RevisiRuteOnCall") {            
            for (var i = 0 ; i < datarute.length ; i++) {
                request = callAjaxForRute(datarute[i]);
                //promises.push(request);
            }
        } else {
            request = callAjaxForRute(datarute);
            //promises.push(request);
        }
        
        //console.log("request");
        //console.log(request);

        //console.log("promises");
        //console.log(promises);        

        //console.log("listIdRuteLoad");
        //console.log(listIdRuteLoad);
        //console.log("listIdRuteUnload");
        //console.log(listIdRuteUnload);
        //console.log("Customer ID:");
        //console.log($('#CustomerId').val());

        //$.when.apply(null, promises).done(function () {            
        //$.ajax({
        //    url: "/Customer/GetSpecLocation",
        //    type: 'POST',
        //    dataType: 'Json',
        //    data: {
        //        id: $('#CustomerId').val(),
        //        idLoad: listIdRuteLoad,
        //        idUnload: listIdRuteUnload
        //    },
        //    cache: false,
        //    success: function (res) {
        //        dsGridLoad = [];
        //        res.dataLoad.forEach(function (item) {
        //            dsGridLoad.push({
        //                Id: item.Id,
        //                Alamat: item.Alamat,
        //                Provinsi: item.Provinsi,
        //                Kota: item.Kota,
        //                Zona: item.Zona,
        //                Telp: item.Telp,
        //                Fax: item.Fax,
        //                urutan: 0
        //            })
        //        });
        //        GridCustLoad.dataSource.data(dsGridLoad);

        //        dsGridUnLoad = [];
        //        res.dataUnload.forEach(function (item) {
        //            dsGridUnLoad.push({
        //                Id: item.Id,
        //                Alamat: item.Alamat,
        //                Provinsi: item.Provinsi,
        //                Kota: item.Kota,
        //                Zona: item.Zona,
        //                Telp: item.Telp,
        //                Fax: item.Fax,
        //                urutan: 0
        //            })
        //        });
        //        GridCustUnLoad.dataSource.data(dsGridUnLoad);
        //    },
        //});
        //})

        $('#RuteIdBaru').val(data.Id);
        $('#RuteBaru').val(data.NamaRuteDaftarHarga);

        $('#modalGridRute').modal('hide');
    }
    
    function getnerateRute(overide, isOnCall) {
        //var promises = [];
        //var multi = [];
        //var listIdRuteLoad = [];
        //var listIdRuteUnload = [];

        if (overide == false) {
            $("#Rute").val('');
            $("#RuteId").val('');
            checkedIdLoad = {};
            checkedIdUnoad = {};
            dsGridLoad = [];
            dsGridUnLoad = [];

        }

        if (isOnCall == true) {
            $.ajax({
                url: "/DaftarHargaOnCall/GetRuteByCustomer",
                type: 'POST',
                dataType: 'Json',
                data: {
                    idCust: $('#CustomerId').val(),
                    tanggalMuat: $('#TanggalMuat').val().toString(),
                },
                cache: false,
                success: function (res) {
                    dsRute = [];
                    for (var i = 0 ; i < res.data.length ; i++) {
                        if ($('#StrJenisTruck').val() == res.data[i].NamaJenisTruck)
                            dsRute.push({
                                Id: res.data[i].Id,
                                NamaRuteDaftarHarga: res.data[i].NamaRuteDaftarHarga,
                                ListNamaRute: res.data[i].ListNamaRute,
                                ListIdRute: res.data[i].ListIdRute,
                                NamaJenisTruck: res.data[i].NamaJenisTruck,
                                SatuanHarga: res.data[i].SatuanHarga,
                                Keterangan: res.data[i].Keterangan,
                            });
                        }
                    }
                    gridRute.dataSource.data(dsRute);
                },
                error: function () {
                    dsRute = [];
                    gridRute.dataSource.data(dsRute);
                }
            })
        } else {
            dsRute = new kendo.data.DataSource({
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
                                    if (filter[i].field == "MultiDrop") {
                                        filter[i].field = "MultiDrop.tujuan";
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
                                    if (sort[i].field == "MultiDrop") {
                                        sort[i].field = "MultiDrop.tujuan";
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
            });
        }

        //    GridCustLoad.dataSource.data(dsGridLoad);
        //    GridCustUnLoad.dataSource.data(dsGridUnLoad);
    }

    if (eksyen == "RevisiRuteOnCall") {
        getnerateRute(true, true);
    } else {
        getnerateRute(true, false);
    }

    console.log("dsRute: ");
    console.log(dsRute);
    if (eksyen == "RevisiRuteOnCall") {
        gridRute = $("#GridRute").kendoGrid({
            dataSource: {
                data: dsRute,
                batch: true,
                schema: {
                    model: {
                        fields: {
                            Id: { type: "number" },
                            ListIdRute: { type: "number" },
                            NamaRuteDaftarHarga: { type: "string" },
                            ListNamaRute: { type: "string" },
                            NamaJenisTruck: { type: "string" },
                            SatuanHarga: { type: "string" },
                            Keterangan: { type: "string" },
                        }
                    },
                },
                pageSize: 10,
                pageable: true,
                sortable: true,
            },
            filterable: kendoGridFilterable,
            sortable: true,
            reorderable: true,
            resizable: true,
            pageable: true,
            columns: [
                {
                    command: [
                        {
                            name: "select",
                            text: "Select",
                            click: SelectRute,
                            imageClass: "glyphicon glyphicon-ok",
                            template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=iconClass# #=imageClass#"></span></a>'
                        }
                    ],
                    width: "50px"
                },
                { field: "NamaRuteDaftarHarga", title: "Nama Rute Daftar Harga" },
                { field: "ListNamaRute", title: "List Nama Rute" },
                { field: "NamaJenisTruck", title: "Jenis Truck" },
                { field: "SatuanHarga", title: "Satuan" },
                { field: "Keterangan", title: "Keterangan" },
            ],
        }).data("kendoGrid");
    } else {
        $("#GridRute").kendoGrid({
            dataSource: dsRute,
            filterable: kendoGridFilterable,
            sortable: true,
            reorderable: true,
            resizable: true,
            pageable: true,
            columns: [
                {
                    command: [
                        {
                            name: "select",
                            text: "Select",
                            click: SelectRute,
                            imageClass: "glyphicon glyphicon-ok",
                            template: '<a class="k-button-icon #=className#" #=attr# href="\\#"><span class="#=iconClass# #=imageClass#"></span></a>'
                        }
                    ],
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
    }


    GridCustLoad = $("#gridmuat").kendoGrid({
        editable: true,
        dataSource: {
            data: dsGridLoad,
            batch: true,
            schema: {
                model: {
                    fields: {
                        Id: { type: "number", editable: false },
                        Alamat: { type: "string", editable: false },
                        Provinsi: { type: "string", editable: false },
                        Kota: { type: "string", editable: false },
                        Zona: { type: "string", editable: false },
                        Telp: { type: "string", editable: false },
                        Fax: { type: "string", editable: false },
                        urutan: { type: "number" },
                        IsSelect: { type: "boolean" }
                    }
                },
            },
            change: function (e) {
                if (e.action == "itemchange" && e.field == "urutan") {
                    var model = e.items[0];
                    if (model.urutan > 0) {
                        $("#gridmuat").find("tr[data-uid='" + model.uid + "']").addClass("k-state-selected").find(".checkbox").attr("checked", "checked");
                        //$("#gridmuat").find("tr[data-uid='" + model.uid + "'] td:eq(0)").find(".checkbox").attr("checked", "checked");
                        model.IsSelect = true;
                        checkedIdLoad[model.Id] = true;
                    }
                    else {
                        $("#gridmuat").find("tr[data-uid='" + model.uid + "']").removeClass("k-state-selected").find(".checkbox").attr("checked", false);
                        //$("#gridmuat").find("tr[data-uid='" + model.uid + "'] td:eq(0)").find(".checkbox").attr("checked", false);
                        model.IsSelect = false;
                        model.urutan = 0;
                        checkedIdLoad[model.Id] = false;
                    }
                    dsGridLoad = $("#gridmuat").data("kendoGrid").dataSource.data();
                }
            },
        },
        dataBound: onDataBoundLoad,
        columns: [
            {
                template: "<input type='checkbox' class='checkbox' />",
                width: "50px"
            },
            {
                field: "urutan",
                title: "Urutan"
            },
            {
                field: "Zona",
                title: "Zona"
            },
            {
                field: "Alamat",
                title: "Alamat"
            },
            {
                field: "Kota",
                title: "Kabupaten/Kota"
            },
            {
                field: "Provinsi",
                title: "Provinsi"
            },
            {
                field: "Telp",
                title: "Telp"
            },
            {
                field: "Fax",
                title: "Fax"
            }
        ],
    }).data("kendoGrid");
    GridCustLoad.table.on("click", ".checkbox", selectLoad);

    GridCustUnLoad = $("#gridbongkar").kendoGrid({
        editable: true,
        dataSource: {
            data: dsGridUnLoad,
            batch: true,
            schema: {
                model: {
                    fields: {
                        Id: { type: "number", editable: false },
                        Alamat: { type: "string", editable: false },
                        Provinsi: { type: "string", editable: false },
                        Kota: { type: "string", editable: false },
                        Zona: { type: "string", editable: false },
                        Telp: { type: "string", editable: false },
                        Fax: { type: "string", editable: false },
                        urutan: { type: "number" },
                        IsSelect: { type: "boolean" }
                    }
                }
            },
            change: function (e) {
                if (e.action == "itemchange" && e.field == "urutan") {
                    var model = e.items[0];
                    if (model.urutan > 0) {
                        $("#gridbongkar").find("tr[data-uid='" + model.uid + "']").addClass("k-state-selected").find(".checkbox").attr("checked", true);
                        //$("#gridbongkar").find("tr[data-uid='" + model.uid + "'] td:eq(0)").find(".checkbox").attr("checked", true);
                        model.IsSelect = true;
                        checkedIdUnoad[model.Id] = true;
                    }
                    else {
                        $("#gridbongkar").find("tr[data-uid='" + model.uid + "']").removeClass("k-state-selected").find(".checkbox").attr("checked", false);
                        //$("#gridbongkar").find("tr[data-uid='" + model.uid + "'] td:eq(0)").find(".checkbox").attr("checked", false);
                        model.IsSelect = false;
                        model.urutan = 0;
                        checkedIdUnoad[model.Id] = false;
                    }
                    dsGridUnLoad = $("#gridbongkar").data("kendoGrid").dataSource.data();
                }
            },
        },
        dataBound: onDataBoundUnload,
        columns: [
            {
                template: "<input type='checkbox' class='checkbox' />",
                width: "50px"
            },
            {
                field: "urutan",
                title: "Urutan"
            },
            {
                field: "Zona",
                title: "Zona"
            },
            {
                field: "Alamat",
                title: "Alamat"
            },
            {
                field: "Kota",
                title: "Kabupaten/Kota"
            },
            {
                field: "Provinsi",
                title: "Provinsi"
            },
            {
                field: "Telp",
                title: "Telp"
            },
            {
                field: "Fax",
                title: "Fax"
            }
        ],
    }).data("kendoGrid");
    GridCustUnLoad.table.on("click", ".checkbox", selectUnload);


})

$('#formsubmit').submit(function (e) {
    //e.preventDefault();
    $('#StrLoadBaru').val(JSON.stringify(GridCustLoad.dataSource.data()));
    $('#StrUnloadBaru').val(JSON.stringify(GridCustUnLoad.dataSource.data()));


    //console.log("StrLoadLama from submit");
    //console.log($('#StrLoad').val());
    //console.log("StrUnloadLama from submit");
    //console.log($('#StrUnload').val());

    //console.log("StrLoadBaru from submit");
    //console.log($('#StrLoadBaru').val());
    //console.log("StrUnloadBaru from submit");
    //console.log($('#StrUnloadBaru').val());

})