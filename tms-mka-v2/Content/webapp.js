//global variable
var gridIndex;
var currController;
var currAction;
var listKolom = [];
var totalFile = 0;
var startRange;
var endRange;

/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

disable_enter()
var kendoGridFilterable = {
    messages: {
        info: "Tampilkan yang memiliki nilai:", // sets the text on top of the filter menu
        filter: "Filter", // sets the text for the "Filter" button
        clear: "Hapus", // sets the text for the "Clear" button

        // when filtering boolean numbers
        isTrue: "Ya", // sets the text for "isTrue" radio button
        isFalse: "Tidak", // sets the text for "isFalse" radio button

        //changes the text of the "And" and "Or" of the filter menu
        and: "Dan",
        or: "Atau"
    },
    operators: {
        //string: {
        //    eq: "Is equal to",
        //    neq: "Is not equal to",
        //    startswith: "Starts with",
        //    contains: "Contains",
        //    endswith: "Ends with"
        //},
        //filter menu for "string" type columns
        string: {
            contains: "Memiliki Kata",
            eq: "Sama Dengan",
            neq: "Tidak Sama Dengan",
            startswith: "Memiliki Awalan",
            endswith: "Memiliki Akhiran"
        },
        //filter menu for "number" type columns
        number: {
            eq: "Sama Dengan",
            neq: "Tidak Sama Dengan",
            gte: "Lebih Besar Atau Sama Dengan",
            gt: "Lebih Besar",
            lte: "Lebih Kecil Atau Sama Dengan",
            lt: "Lebih Kecil"
        },
        //filter menu for "date" type columns
        date: {
            eq: "Sama Dengan",
            neq: "Tidak Sama Dengan",
            gte: "Setelah Atau Sama Dengan",
            gt: "Setelah",
            lte: "Sebelum Atau Sama Dengan",
            lt: "Sebelum"
        },
        //filter menu for foreign key values
        enums: {
            eq: "Sama Dengan",
            neq: "Tidak Sama Dengan"
        },
    },
    extra: true,
};

function getDataRowGrid(e) {
    return $(e.target).closest("tr");
}
/**
 * konfirmasi delete sebelum di redirect
 * response: { Success: t/f, Message }
 */
function SuccessNotif() {
    var staticNotification = $("#notif").kendoNotification({
        appendTo: "#appendto"
    }).data("kendoNotification");

    staticNotification.show("Data berhasil diupdate.", "success");
}
function ErrorNotif(msg) {
    var staticNotification = $("#notif").kendoNotification({
        appendTo: "#appendto"
    }).data("kendoNotification");

    staticNotification.show(msg, "error");
}
function goToDeletePage(url, datasource) {
    var previousWindowKeyDown = window.onkeydown;
    swal(
        {
            title: "Hapus Data",
            text: "Apakah anda yakin untuk menghapus data ini?",
            type: "warning",
            cancelButtonText: "Batal",
            showCancelButton: true,
            confirmButtonClass: "btn btn-primary",
            confirmButtonText: "Ya",
            closeOnConfirm: false
        },
        function () {
            swal({
                title: "Loading",
                text: "Harap Menunggu...",
                imageUrl: "/Content/sweet-alert/ajax-loader.gif",
                closeOnConfirm: false,
                confirmButtonClass: "hidden",
                //imageSize: "80x80"
            });

            if (previousWindowKeyDown !== undefined && window.onkeydown !== previousWindowKeyDown)
                window.onkeydown = previousWindowKeyDown;

            $.ajax({
                url: url,
                type: "POST",
                success: function (data) {
                    if (data.Success == true) {
                        if (datasource != null) {
                            datasource.read();
                        }
                        //swal("Status", "Data berhasil dihapus", "success");
                        swal.close();
                    }
                    else {
                        swal("",data.Message,"warning");
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    swal("Data tidak bisa dihapus, sedang digunakan di menu lain.");
                }
            });
        });
}
function goToSubmitPage(url, datasource) {
    var previousWindowKeyDown = window.onkeydown;
    swal(
        {
            title: "Submit Data",
            text: "Apakah anda yakin untuk submit data ini?",
            type: "warning",
            cancelButtonText: "Batal",
            showCancelButton: true,
            confirmButtonClass: "btn btn-primary",
            confirmButtonText: "Ya",
            closeOnConfirm: false
        },
        function () {
            swal({
                title: "Loading",
                text: "Harap Menunggu...",
                imageUrl: "/Content/sweet-alert/ajax-loader.gif",
                closeOnConfirm: false,
                confirmButtonClass: "hidden",
                //imageSize: "80x80"
            });

            if (previousWindowKeyDown !== undefined && window.onkeydown !== previousWindowKeyDown)
                window.onkeydown = previousWindowKeyDown;

            $.ajax({
                url: url,
                type: "POST",
                success: function (data) {
                    if (data.Success == true) {
                        if (datasource != null) {
                            datasource.read();
                        }
                        //swal("Status", "Data berhasil disubmit", "success");
                        swal.close();
                    }
                    else {
                        swal('error',data.Message,'error');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    swal("Internal error server");
                }
            });
        });
}
function goToReturnPage(url, datasource) {
    var previousWindowKeyDown = window.onkeydown;
    swal(
        {
            title: "Return Data",
            text: "Apakah anda yakin untuk return data ini?",
            type: "warning",
            cancelButtonText: "Batal",
            showCancelButton: true,
            confirmButtonClass: "btn btn-primary",
            confirmButtonText: "Ya",
            closeOnConfirm: false
        },
        function () {
            swal({
                title: "Loading",
                text: "Harap Menunggu...",
                imageUrl: "/Content/sweet-alert/ajax-loader.gif",
                closeOnConfirm: false,
                confirmButtonClass: "hidden",
                //imageSize: "80x80"
            });

            if (previousWindowKeyDown !== undefined && window.onkeydown !== previousWindowKeyDown)
                window.onkeydown = previousWindowKeyDown;

            $.ajax({
                url: url,
                type: "POST",
                success: function (data) {
                    if (data.Success == true) {
                        if (datasource != null) {
                            datasource.read();
                        }
                        //swal("Status", "Data berhasil disubmit", "success");
                        swal.close();
                    }
                    else {
                        swal('error', data.Message, 'error');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    swal("Internal error server");
                }
            });
        });
}

function goToSavePage(url, datamodel, grid) {
    var previousWindowKeyDown = window.onkeydown;

    swal({
        title: "Loading",
        text: "Harap Menunggu...",
        imageUrl: "/Content/sweet-alert/ajax-loader.gif",
        closeOnConfirm: false,
        confirmButtonClass: "hidden",
    });

    //if (previousWindowKeyDown !== undefined && window.onkeydown !== previousWindowKeyDown)
    window.onkeydown = previousWindowKeyDown;

    $.ajax({
        url: url,
        //contentType: "application/json",
        type: "POST",
        data: { model: datamodel },
        success: function (data) {
            if (data.Success == true) {
                if (grid != null) {
                    grid.read();
                }
                swal("Status", "Data berhasil disimpan", "success");
            }
            else {
                swal(data.Message);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            swal("Server Error. Harap hubungi administrator");
        }
    });
}
function notifSave(frm,val) {
    var previousWindowKeyDown = window.onkeydown;
    return swal(
        {
            title: "Submit Data",
            text: "Apakah anda yakin untuk submit data ini?",
            type: "warning",
            cancelButtonText: "Batal",
            showCancelButton: true,
            confirmButtonClass: "btn btn-primary",
            confirmButtonText: "Ya",
            closeOnConfirm: false
        },
        function (isConfirm) {
            swal({
                title: "Loading",
                text: "Harap Menunggu...",
                imageUrl: "/Content/sweet-alert/ajax-loader.gif",
                closeOnConfirm: false,
                confirmButtonClass: "hidden",
                //imageSize: "80x80"
            });

            if (previousWindowKeyDown !== undefined && window.onkeydown !== previousWindowKeyDown)
                window.onkeydown = previousWindowKeyDown;

            $('#Status').val(val);
            frm.submit();
        });
}
/**
 * validasi select image kendo upload
 */

function onSelectImage(e) {
    var flagExtension = 0;
    var flagSize = 0;
    var allowedExtension = [".jpg", ".jpeg", ".png", ".pdf"];
    var maxImage = 1048576;
    $.each(e.files, function (index, value) {
        if ($.inArray((value.extension).toLowerCase(), allowedExtension) === -1) {
            flagExtension = 1;
        }
        if (value.size > maxImage) {
            if (value.size > maxImage) {
                flagSize = 1;
            }
        }
    });
    if (flagExtension == 1) {
        //alert('File yang diperbolehkan hanya berupa jpg/jpeg, png dan pdf');
        swal("", "File yang diperbolehkan hanya berupa jpg/jpeg, png dan pdf", "error");
        e.preventDefault();
    }
    if (flagSize != 0) {
        if (flagSize == 1) {
            //alert('Ukuran file gambar tidak boleh ada yang melebihi 1Mb');
            swal("", "Ukuran file gambar tidak boleh ada yang melebihi 1Mb", "error");
            e.preventDefault();
        }
    }
}
function onSelectUpload(e) {
    var flagExtension = 0;
    var flagSize = 0;
    var allowedExtension = [".jpg", ".jpeg", ".png", ".pdf", ".xlsx", ".xls"];
    var maxImage = 1048576;
    $.each(e.files, function (index, value) {
        if ($.inArray((value.extension).toLowerCase(), allowedExtension) === -1) {
            flagExtension = 1;
        }
        if (value.size > maxImage) {
            if (value.size > maxImage) {
                flagSize = 1;
            }
        }
    });
    if (flagExtension == 1) {
        //alert('File yang diperbolehkan hanya berupa jpg/jpeg, png dan pdf');
        swal("", "File yang diperbolehkan hanya berupa jpg/jpeg, png ,pdf, dan excell", "error");
        e.preventDefault();
    }
    if (flagSize != 0) {
        if (flagSize == 1) {
            //alert('Ukuran file gambar tidak boleh ada yang melebihi 1Mb');
            swal("", "Ukuran file gambar tidak boleh ada yang melebihi 1Mb", "error");
            e.preventDefault();
        }
    }
}
//ambil data kolom hide/show
function GetColomnData(grid) {
    var visibleColumns = [];
    jQuery.each(grid.columns, function (index) {
        if (this.command == null) {
            if (!this.hidden) {
                var el = '';
                //cek kolom database
                if (jQuery.inArray(this.field, listKolom) !== -1) {
                    el = '<div class="md-checkbox">' +
                            '<input id="cbcolumn-' + index + '" type="checkbox" class="md-check" value=' + this.field + ' checked >' +
                            '<label for="cbcolumn-' + index + '">' +
                                '<span></span>' +
                                '<span class="check"></span>' +
                                '<span class="box"></span>' + this.title +
                            '</label>' +
                        '</div>';
                    gridIndex.hideColumn(this.field);
                }
                else {
                    el = '<div class="md-checkbox">' +
                            '<input id="cbcolumn-' + index + '" type="checkbox" class="md-check" value=' + this.field + '>' +
                            '<label for="cbcolumn-' + index + '">' +
                                '<span></span>' +
                                '<span class="check"></span>' +
                                '<span class="box"></span>' + this.title +
                            '</label>' +
                        '</div>';
                }

                $("#content-col").append(el);
            }
        }
    });
}
function TanggalEditor(container, options) {
    $('<input required data-required-msg="Tanggal harus diisi." data-bind="value:' + options.field + '" name="' + options.field + '" data-format="' + options.format + '"/>')
           .appendTo(container)
           .kendoDatePicker({ format: "dd/MM/yyyy" }).attr("readonly", "readonly").data("kendoDatePicker");
    $("<span class='k-invalid-msg' data-for='" + options.field + "'></span>").appendTo(container);
}

function hpEditor(container, options) {
    $('<input class="k-input k-textbox" required data-required-msg="Hp harus diisi." data-bind="value:' + options.field + '" name="' + options.field + '"/>')
            .appendTo(container)
            .kendoMaskedTextBox({
                mask: "0000-0000-0000",
                clearPromptChar: true
            });
    $("<span class='k-invalid-msg' data-for='" + options.field + "'></span>").appendTo(container);
}

function emailEditor(container, options) {
    $('<input class="k-input k-textbox" type="email" required data-email-msg="Email tidak valid." data-required-msg="Email harus diisi." data-bind="value:' + options.field + '" name="' + options.field + '"/>')
           .appendTo(container);
    $("<span class='k-invalid-msg' data-for='" + options.field + "'></span>").appendTo(container);
}

function jamEditor(container, options) {
    $('<input required data-format-msg="Format jam salah." data-required-msg="Jam harus diisi." data-bind="value:' + options.field + '" name="' + options.field + '"/> ')
           .appendTo(container)
           .kendoMaskedTextBox({
               mask: "00:00 - 00:00"
           });
    $("<span class='k-invalid-msg' data-for='" + options.field + "'></span>").appendTo(container);
}

function hariEditor(container, options) {
    $('<input required data-comboreq-msg="Hari harus diisi." data-required-msg="Hari harus diisi." data-text-field="text" data-value-field="value" data-bind="value:' + options.field + '" name="' + options.field + '" />')
           .appendTo(container)
           .kendoDropDownList({
               dataSource: [
                            { text: "Senin", value: "Senin" },
                            { text: "Selasa", value: "Selasa" },
                            { text: "Rabu", value: "Rabu" },
                            { text: "Kamis", value: "Kamis" },
                            { text: "Jumat", value: "Jumat" },
                            { text: "Sabtu", value: "Sabtu" },
                            { text: "Minggu", value: "Minggu" }
               ],
               dataTextField: "text",
               dataValueField: "value"
           });
    $("<span class='k-invalid-msg' data-for='" + options.field + "'></span>").appendTo(container);
}

function InitLookUp(el, data) {
    el.kendoComboBox({
        dataTextField: "Nama",
        dataValueField: "Id",
        dataSource: data,
        filter: "contains",
        suggest: true,
    });
}

function TimeValidate(value) {
    if (!/^\d{2}:\d{2}$/.test(value)) return false;
    var parts = value.split(':');
    if (parts[0] > 23 || parts[1] > 59) return false;
    return true;
}

function CreateDateRange(elStart, elEnd) {
    startRange = elStart.kendoDatePicker({
        change: startDateChange
    }).attr("readonly", "readonly").data("kendoDatePicker");

    endRange = elEnd.kendoDatePicker({
        change: endDateChange
    }).attr("readonly", "readonly").data("kendoDatePicker");

    startRange.max(endRange.value());
    endRange.min(startRange.value());
}

function startDateChange() {
    var startDate = startRange.value(),
    endDate = endRange.value();

    if (startDate) {
        startDate = new Date(startDate);
        startDate.setDate(startDate.getDate());
        endRange.min(startDate);
    } else if (endDate) {
        startRange.max(new Date(endDate));
    } else {
        endDate = new Date();
        startRange.max(endDate);
        endRange.min(endDate);
    }
}

function disable_enter(){
    $('body').on('keydown', 'input, select', function(e) {
      var self = $(this)
        , form = self.parents('form:eq(0)')
        , focusable
        , next
        ;
      if (e.keyCode == 13 && $(this).attr('class').indexOf('search') == -1) {
        focusable = form.find('input,select,textarea').filter(':visible');
        next = focusable.eq(focusable.index(this)+1);
        next.focus();
        return false;
      }
    });
}

function endDateChange() {
    var endDate = endRange.value(),
    startDate = startRange.value();

    if (endDate) {
        endDate = new Date(endDate);
        endDate.setDate(endDate.getDate());
        startRange.max(endDate);
    } else if (startDate) {
        endRange.min(new Date(startDate));
    } else {
        endDate = new Date();
        startRange.max(endDate);
        endRange.min(endDate);
    }
}

function onSelectFileExcell(e) {
    var flagExtension = 0;
    var allowedExtension = [".xlsx", ".xls"];
    $.each(e.files, function (index, value) {
        if ($.inArray((value.extension).toLowerCase(), allowedExtension) === -1) {
            flagExtension = 1;
        }
    });
    if (flagExtension == 1) {
        alert("File yang diperbolehkan hanya berupa Excell");
        e.preventDefault();
    }

    setTimeout(function () {
        kendoUploadButtonExim = $(".k-upload-selected");
        kendoUploadButtonExim.hide();
    }, 1);
}

function CreateUploaderExim(el, url, grid) {
    el.kendoUpload({
        multiple: true,
        async: {
            saveUrl: url,
            autoUpload: false
        },
        success: function (e, options) {
            totalFile++;
            console.log(e)
            if (totalFile == e.files.length) {
                swal("Status", "Upload data berhasil.", "success");
                grid.data("kendoGrid").dataSource.read();
            }
        },
        select: onSelectFileExcell,
        upload: function () {
            swal({
                title: "Loading",
                text: "Harap Menunggu...",
                imageUrl: "/Content/sweet-alertCss/ajax-loader.gif",
                closeOnConfirm: false,
                confirmButtonClass: "hidden",
            });
        }
    }).data("kendoUpload");
}

$(document).ready(function () {
    kendo.culture("id-ID");

    $(".field-validation-error").addClass("text-danger");

    $("body").css("font-size", "12px");

    $("#content-col").on('click', ':checkbox', function () {
        if ($(this).is(':checked')) {
            gridIndex.hideColumn($(this).val());
            //save / update
            $.ajax({
                url: currController + '/saveReference',
                type: 'GET',
                dataType: 'Json',
                cache: false,
                data: {
                    act: currAction,
                    contr: currController,
                    kolom: $(this).val(),
                    HideShow: 'hide'
                },
                success: function (obj) {

                }
            })
        }
        else {
            gridIndex.showColumn($(this).val());
            //save / update
            $.ajax({
                url: currController + '/saveReference',
                type: 'GET',
                dataType: 'Json',
                cache: false,
                data: {
                    act: currAction,
                    contr: currController,
                    kolom: $(this).val(),
                    HideShow: 'show'
                },
                success: function (obj) {

                }
            })
        }
    });

    //required sign using asterisk
    $('input[type=text], input[type=password], input[type=hidden], textarea, select').each(function () {
        var req = $(this).attr('data-val-required');
        if (undefined != req) {
            var label = $(this).parentsUntil('form').find('label[for="' + $(this).attr('id') + '"]');
            var text = label.text();
            if (text.length > 0) {
                label.append('<span style="color:red"> *</span>');
            }
        }
    });
    $(".timepicker-24").timepicker({ autoclose: !0, minuteStep: 5, showSeconds: !1, showMeridian: !1 });

    $(".timepicker").parent(".input-group").on("click", ".input-group-btn", function (t) {
        t.preventDefault(), $(this).parent(".input-group").find(".timepicker").timepicker("showWidget")
    })

    $(".focus").focus();

    $(".form-control-datepicker").kendoDatePicker({
        //dateFormat: "yy-mm-dd",
        format: "dd/MM/yyyy"
    }).attr("readonly", "readonly");

    $(".form-control-timepicker").kendoTimePicker().attr("readonly", "readonly");

    $(".form-control-datepickerdis").kendoDatePicker({
        //dateFormat: "yy-mm-dd",
        format: "dd/MM/yyyy"
    });

    $(".form-control-dateyear").kendoDatePicker({
        start: "decade",
        depth: "decade",
        format: "yyyy",
        footer: false
    }).attr("readonly", "readonly");

    $(".form-control-datetimepicker").kendoDateTimePicker({
        value: new Date()
    }).attr("readonly", "readonly");

    $(".form-control-numeric-suhu").kendoNumericTextBox({
        format: "# \xB0C",
        min: -30,
        max: 30,
        decimals: 0,
        step: 1,
        spinners: false,
        placeholder: "0 \xB0C"
    });

    $(".form-control-numeric-minute").kendoNumericTextBox({
        format: "# Menit",
        min: 0,
        decimals: 0,
        step: 1,
        spinners: false,
        placeholder: "0 Menit"
    });

    $(".form-control-numeric-persen").kendoNumericTextBox({
        format: "#.00 \\%",
        min: 0,
        max: 100,
        spinners: false,
        placeholder: "0 \%"
    });

    $(".form-control-numeric-persen3").kendoNumericTextBox({
        format: "#.000 \\%",
        min: 0,
        max: 100,
        spinners: false,
        decimals: 3,
        placeholder: "0 \%"
    });

    $('.form-control-numeric').kendoNumericTextBox({
        min: 0,
        max: 1000000000000000000,
        decimals: 0,
        format: 'n0',
        spinners: false,
        placeholder: "0"
    });

    $('.form-control-numeric-rit').kendoNumericTextBox({
        min: 1,
        max: 2,
        decimals: 0,
        format: 'n0',
        spinners: false
    });

    $('.form-control-numeric-decimal').kendoNumericTextBox({
        min: 0,
        max: 1000000000000000000,
        decimals: 2,
        format: 'n2',
        spinners: false
    });

    $('.form-control-numeric-idr').kendoNumericTextBox({
        min: 0,
        max: 1000000000000000000,
        decimals: 2,
        format: "Rp ###,###.00 ",
        spinners: false,
        placeholder: "Rp 0.00"
    });

    $('.form-control-numeric-km').kendoNumericTextBox({
        min: 0,
        max: 1000000000000000000,
        decimals: 2,
        format: "#.00 KM",
        spinners: false,
        placeholder: "0 KM"
    });

    $('.form-control-numeric-km-jam').kendoNumericTextBox({
        min: 0,
        max: 1000000000000000000,
        decimals: 2,
        format: "#.00 KM / Jam",
        spinners: false,
        placeholder: "0 KM / Jam"
    });

    $('.form-control-numeric-meter').kendoNumericTextBox({
        min: 0,
        max: 1000000000000000000,
        decimals: 0,
        format: "# Meter",
        spinners: false,
        placeholder: "0 Meter"
    });

    $('.form-control-numeric-kg').kendoNumericTextBox({
        min: 0,
        max: 1000000000000000000,
        format: "# KG",
        spinners: false,
        placeholder: "0 KG"
    });

    $('.form-control-numeric-jam').kendoNumericTextBox({
        min: 0,
        max: 1000000000000000000,
        decimals: 0,
        format: "# Jam",
        spinners: false,
        placeholder: "0 Jam"
    });

    $('.form-control-numeric-hari').kendoNumericTextBox({
        min: 0,
        max: 1000000000000000000,
        decimals: 0,
        format: "# Hari",
        spinners: false,
        placeholder: "0 Hari"
    });

    $('.form-control-numeric-menit').kendoNumericTextBox({
        min: 0,
        max: 1000000000000000000,
        decimals: 0,
        format: "# Menit",
        spinners: false,
        placeholder: "0 Menit"
    });

    $('.form-control-numeric-ltr').kendoNumericTextBox({
        min: 0,
        max: 1000000000000000000,
        format: "# ltr",
        spinners: false,
        placeholder: "0 ltr"
    });

    $('.form-control-numeric-ns').kendoNumericTextBox({
        min: 0,
        max: 1000000000000000000,
        decimals: 0,
        format: '#',
        spinners: false
    });
    $('.form-control-numeric-latlong').kendoNumericTextBox({
        decimals: 14,
        format: '#.##############',
        spinners: false
    });
    $('.form-control-kodearea').kendoMaskedTextBox({
        mask: "0000",
        clearPromptChar: true
    });
    $('.form-control-tel').kendoMaskedTextBox({
        mask: "00000000",
        clearPromptChar: true
    });
    $('.form-control-hp').kendoMaskedTextBox({
        mask: "0000-0000-0000",
        clearPromptChar: true
    });
    $('.form-control-rtrw').kendoMaskedTextBox({
        mask: "000",
        clearPromptChar: true
    });
})