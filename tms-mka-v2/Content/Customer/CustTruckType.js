function chkClick(el,idx)
{
    if ($(el).is(":checked")) {
        $("input[name = 'ListCustTruckType[" + idx + "].IsKode']").prop("checked", false);
        $("input[name = 'ListCustTruckType[" + idx + "].Alias']").prop("readonly", false);
    }
    else
    {
        $("input[name = 'ListCustTruckType[" + idx + "].Alias']").prop("readonly", true);
    }
}

function kodeClick(el, idx) {
    if ($(el).is(":checked")) {
        $("input[name = 'ListCustTruckType[" + idx + "].IsAlias']").prop("checked", false);
        $("input[name = 'ListCustTruckType[" + idx + "].Alias']").prop("readonly", true);
    }
}