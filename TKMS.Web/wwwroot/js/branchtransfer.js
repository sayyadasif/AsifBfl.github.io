var branchTransferBaseUrl = "/BranchTransfer";
var kitBaseUrl = "/Kit";

var kitLoaded = [];

var kitStatusIds = null;

var indentNo = null;
var accountNo = null;
var cifNo = null;
var branchId = null;
var cardTypeId = null;
var bfilBranchId = null;
var kitDate = null;
var isSent = null;

var kitViewPage = null;
var currentSelectedTab = null;

$(document).ready(function () {

    $('#kitTab a').click(function (e) {
        e.preventDefault();
        showTab(this);
    });

    $("#kitTransferDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": false,
        "deferLoading": 0,
        "scrollX": true,
        "scrollY": '50vh',
        "scrollCollapse": true,
        "ajax": {
            "url": `${kitBaseUrl}/GetKits`,
            "type": "POST",
            "datatype": "json",
            "data": function (d) { return getFilterParams(d); }
        },
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        "columns": [
            {
                render: function (row, type, data) {
                    return `<input type="checkbox" name="kitForTransfer"  value="${data.kitId}" />`;
                }
            },
            { "data": "indentNo", "name": "indentNo", "autoWidth": true },
            {
                "data": "accountNo", "name": "accountNo",
                render: function (row, type, data) {
                    return `<a href='${branchTransferBaseUrl}/KitTransfer/${data.kitId}'>${data.accountNo}</a>`;
                }
            },
            { "data": "cifNo", "name": "cifNo", "autoWidth": true },
            { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
            { "data": "iblBranchName", "name": "iblBranchName", "autoWidth": true },
            { "data": "bfilBranchCode", "name": "bfilBranchCode", "autoWidth": true },
            { "data": "cardType", "name": "cardType", "autoWidth": true },
        ]
    });

    $("#kitSentDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": false,
        "deferLoading": 0,
        "scrollX": true,
        "scrollY": '50vh',
        "scrollCollapse": true,
        "ajax": {
            "url": `${branchTransferBaseUrl}/GetTransferKits`,
            "type": "POST",
            "datatype": "json",
            "data": function (d) { return getFilterParams(d, true); }
        },
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        "columns": [
            { "data": "indentNo", "name": "indentNo", "autoWidth": true },
            {
                "data": "accountNo", "name": "accountNo",
                render: function (row, type, data) {
                    return `<a href='${branchTransferBaseUrl}/KitSent/${data.branchTransferId}'>${data.accountNo}</a>`;
                }
            },
            {
                "data": "transferDate", "name": "transferDate",
                render: function (row, type, data) {
                    return getFormatDate(data.transferDate);
                }
            },
            { "data": "cifNo", "name": "cifNo", "autoWidth": true },
            { "data": "fromBranchCode", "name": "fromBranchCode", "autoWidth": true },
            { "data": "fromBranchName", "name": "fromBranchName", "autoWidth": true },
            { "data": "toBranchCode", "name": "toBranchCode", "autoWidth": true },
            { "data": "cardType", "name": "cardType", "autoWidth": true },
        ]
    });

    $("#kitReceivedDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": false,
        "deferLoading": 0,
        "scrollX": true,
        "scrollY": '50vh',
        "scrollCollapse": true,
        "ajax": {
            "url": `${branchTransferBaseUrl}/GetTransferKits`,
            "type": "POST",
            "datatype": "json",
            "data": function (d) { return getFilterParams(d, false); }
        },
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        "columns": [
            { "data": "indentNo", "name": "indentNo", "autoWidth": true },
            {
                "data": "accountNo", "name": "accountNo",
                render: function (row, type, data) {
                    return `<a href='${branchTransferBaseUrl}/KitReceived/${data.branchTransferId}'>${data.accountNo}</a>`;
                }
            },
            {
                "data": "receivedDate", "name": "receivedDate",
                render: function (row, type, data) {
                    return getFormatDate(data.receivedDate);
                }
            },
            { "data": "cifNo", "name": "cifNo", "autoWidth": true },
            { "data": "fromBranchCode", "name": "fromBranchCode", "autoWidth": true },
            { "data": "fromBranchName", "name": "fromBranchName", "autoWidth": true },
            { "data": "toBranchCode", "name": "toBranchCode", "autoWidth": true },
            { "data": "cardType", "name": "cardType", "autoWidth": true },
        ]
    });

    $("#transferKitDatatable").DataTable({
        "processing": false,
        "serverSide": false,
        "filter": false,
        "bLengthChange": false,
        "bPaginate": false,
        "bInfo": false,
    });

    $('#btnKitsTransfer').click(function (e) {
        var ids = [];
        $("input:checkbox[name=kitForTransfer]:checked").each(function () {
            ids.push($(this).val());
        });
        if (ids.length === 0) {
            displayToastr('Please select Kit(s)', "Kit Transfer", "Error")
            return;
        }
        $.ajax({
            url: `${branchTransferBaseUrl}/SetTransferIds`,
            type: 'POST',
            data: { kitIds: ids.join(",") },
            success: function (result) {
                window.location.href = `${branchTransferBaseUrl}/BranchTransfer`;
            },
            error: function (error) {
                displayToastr(error.data, "Kit Transfer", "Error");
            },
        });
    });

    var kitSelect = $(location).prop('hash');
    if (kitSelect != '') {
        showTab($('#kitTab a[href="' + kitSelect + '"]'));
    } else {
        showTab($("#kitTab a:first"));
    }

    $("#btnKitFilter").click(function (e) {
        $(`${$(currentSelectedTab).attr("href")}Datatable`).DataTable().ajax.reload();
    });

    $("#btnKitFilterClear").click(function (e) {
        clearFilters();
        $(`${$(currentSelectedTab).attr("href")}Datatable`).DataTable().ajax.reload();
    });
});

function clearFilters() {
    $('#indentNo').val('');
    $('#accountNo').val('');
    $('#cifNo').val('');
    $('#branchId').val('').trigger('change');
    $('#cardTypeId').val('').trigger('change');
    $('#bfilBranchId').val('').trigger('change');
    $('#allocatedToId').val('').trigger('change');
    $('#allocatedDate').val('');
    $('#kitDamageReasonId').val('').trigger('change');
    $('#assignedDate').val('');
    $('#kitDate').val('');

    indentNo = null;
    accountNo = null;
    cifNo = null;
    branchId = null;
    cardTypeId = null;
    bfilBranchId = null;
    allocatedToId = null;
    allocatedDate = null;
    kitDamageReasonId = null;
    assignedDate = null;
    kitDate = null;

    $('#indentNoFilter').hide();
    $('#accountNoFilter').hide();
    $('#cifNoFilter').hide();
    $('#branchIdFilter').hide();
    $('#cardTypeIdFilter').hide();
    $('#bfilBranchIdFilter').hide();
    $('#allocatedToIdFilter').hide();
    $('#allocatedDateFilter').hide();
    $('#kitDamageReasonIdFilter').hide();
    $('#assignedDateFilter').hide();
    $('#kitDateFilter').hide();

    manageFilters();
}

function getFilterParams(d, isTransfer) {

    indentNo = $('#indentNo').val();
    accountNo = $('#accountNo').val();
    cifNo = $('#cifNo').val();
    branchId = $('#branchId').val();
    cardTypeId = $('#cardTypeId').val();
    bfilBranchId = $('#bfilBranchId').val();
    kitDate = $('#kitDate').val();

    d.kitStatusIds = kitStatusIds;

    d.indentNo = indentNo;
    d.accountNo = accountNo;
    d.cifNo = cifNo;
    d.branchId = branchId;
    d.cardTypeId = cardTypeId;
    d.bfilBranchId = bfilBranchId;
    d.isSent = isSent;
    if (isTransfer) {
        d.transferDate = kitDate;
    } else {
        d.receivedDate = kitDate;
    }
}

function showTab(tab) {
    currentSelectedTab = tab;
    clearFilters();

    $(tab).tab('show');
    var kitTab = $(tab).attr("id");
    kitStatusIds = $(tab).attr("statusId");
    isSent = $(tab).attr("isSent");
    if (!kitLoaded.includes(kitTab)) {
        kitLoaded.push(kitTab);
        kitViewPage = $(tab).attr("viewPage");
        $(`${$(tab).attr("href")}Datatable`).DataTable().ajax.reload();
    }

    resizeDataTable();
}

function manageFilters() {
    switch ($(currentSelectedTab).attr("id")) {
        case 'kitTransferTab':
            $('#indentNoFilter').show();
            $('#accountNoFilter').show();
            $('#cifNoFilter').show();
            $('#branchIdFilter').show();
            $('#cardTypeIdFilter').show();
            $('#bfilBranchIdFilter').show();
            break;
        default:
            $('#indentNoFilter').show();
            $('#accountNoFilter').show();
            $('#cifNoFilter').show();
            $('#branchIdFilter').show();
            $('#cardTypeIdFilter').show();
            $('#bfilBranchIdFilter').show();
            $('#kitDateFilter').show();
            break;
    }
}

function GetBranchDetail(obj) {
    var branchId = $(obj).find("option:selected").val();
    if (branchId == '') {
        displayToastr('Please select Branch!', "Kit Transfer", "Error")
        return;
    }
    $.ajax({
        url: `${branchTransferBaseUrl}/GetBranchDetails`,
        type: 'POST',
        data: { branchId },
        success: function (result) {
            if (result.success) {
                $('#ToBranchName').val(result.data.branchName);
            }
        },
        error: function (error) {
            displayToastr(error.data, "Kit Transfer", "Error")
        },
    });
}

function validateBranchTransfer() {
    if ($("#formBranchTransfer").valid()) {
        showLoading();
        return true;
    }
    return false;
}
