var indentBaseUrl = "/Indent";
var indentLoaded = [];

var indentStatusIds = null;

var indentNo = null;
var indentStartDate = null;
var indentEndDate = null;
var branchId = null;
var schemeCodeId = null;
var c5CodeId = null;
var cardTypeId = null;
var bfilBranchTypeId = null;
var regionId = null;
var bfilBranchId = null;
var dispatchQty = null;
var showStock = null;

var indentViewPage = null;
var currentSelectedTab = null;

$(document).ready(function () {

    $("#addressDetail").on("input", function () {
        checkAddressChanged()
    });

    $("#pinCode").on("input", function () {
        checkAddressChanged()
    });

    $('#indentTab a').click(function (e) {
        e.preventDefault();
        showTab(this);
    });

    $("#indentRaisedDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": false,
        "deferLoading": 0,
        "scrollX": true,
        "scrollY": '50vh',
        "scrollCollapse": true,
        "ajax": {
            "url": `${indentBaseUrl}/GetIndents`,
            "type": "POST",
            "datatype": "json",
            "data": function (d) { return getFilterParams(d); }
        },
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        "aaSorting": [[0, 'desc']],
        "columns": [
            {
                "data": "indentStatus", "name": "indentStatus",
                render: function (row, type, data) {
                    return getIndentStatusColor(data.indentStatusId, data.indentStatus);
                }
            },
            {
                "data": "indentNo", "name": "indentNo",
                render: function (row, type, data) {
                    return `<a href='${indentBaseUrl}/IndentDetail/${data.indentId}'>${data.indentNo}</a>`;
                }
            },
            {
                "data": "indentDate", "name": "indentDate",
                render: function (row, type, data) {
                    return getFormatDate(data.indentDate);
                }
            },
            { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
            { "data": "schemeCode", "name": "SchemeCode", "autoWidth": true },
            { "data": "c5Code", "name": "C5Code", "autoWidth": true },
            { "data": "cardType", "name": "CardType", "autoWidth": true },
            { "data": "noOfKit", "name": "NoOfKit", "autoWidth": true },
            { "data": "bfilBranchType", "name": "BfilBranchType", "autoWidth": true },
            { "data": "bfilBranchCode", "name": "BfilBranchCode", "autoWidth": true },
            { "data": "ifscCode", "name": "IfscCode", "autoWidth": true },
        ]
    });

    $("#indentRejectedDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": false,
        "deferLoading": 0,
        "scrollX": true,
        "scrollY": '50vh',
        "scrollCollapse": true,
        "ajax": {
            "url": `${indentBaseUrl}/GetIndents`,
            "type": "POST",
            "datatype": "json",
            "data": function (d) { return getFilterParams(d); }
        },
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        "aaSorting": [[1, 'desc']],
        "columns": [
            {
                "data": "indentNo", "name": "indentNo",
                render: function (row, type, data) {
                    return `<a href='${indentBaseUrl}/${indentViewPage}/${data.indentId}'>${data.indentNo}</a>`;
                }
            },
            {
                "data": "indentDate", "name": "indentDate",
                render: function (row, type, data) {
                    return getFormatDate(data.indentDate);
                }
            },
            { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
            { "data": "schemeCode", "name": "SchemeCode", "autoWidth": true },
            { "data": "c5Code", "name": "C5Code", "autoWidth": true },
            { "data": "cardType", "name": "CardType", "autoWidth": true },
            { "data": "noOfKit", "name": "NoOfKit", "autoWidth": true },
            { "data": "bfilBranchType", "name": "BfilBranchType", "autoWidth": true },
            { "data": "bfilBranchCode", "name": "BfilBranchCode", "autoWidth": true },
            { "data": "ifscCode", "name": "IfscCode", "autoWidth": true },
            { "data": "rejectedReason", "name": "RejectedReason", "autoWidth": true },
        ]
    });

    $("#indentDispatchedDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": false,
        "deferLoading": 0,
        "scrollX": true,
        "scrollY": '50vh',
        "scrollCollapse": true,
        "ajax": {
            "url": `${indentBaseUrl}/GetIndents`,
            "type": "POST",
            "datatype": "json",
            "data": function (d) { return getFilterParams(d); }
        },
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        "aaSorting": [[0, 'desc']],
        "columns": [
            {
                "data": "indentStatus", "name": "indentStatus",
                render: function (row, type, data) {
                    return getIndentStatusColor(data.indentStatusId, data.indentStatus);
                }
            },
            {
                "data": "indentNo", "name": "indentNo",
                render: function (row, type, data) {
                    return `<a href='${indentBaseUrl}/DispatchedDetail/${data.indentId}'>${data.indentNo}</a>`;
                }
            },
            {
                "data": "indentDate", "name": "indentDate",
                render: function (row, type, data) {
                    return getFormatDate(data.indentDate);
                }
            },
            { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
            { "data": "schemeCode", "name": "SchemeCode", "autoWidth": true },
            { "data": "c5Code", "name": "C5Code", "autoWidth": true },
            { "data": "cardType", "name": "CardType", "autoWidth": true },
            { "data": "noOfKit", "name": "NoOfKit", "autoWidth": true },
            { "data": "noOfKitDispatched", "name": "noOfKitDispatched", "autoWidth": true },
            { "data": "noOfKitCancelled", "name": "noOfKitCancelled", "autoWidth": true },
            { "data": "bfilBranchType", "name": "BfilBranchType", "autoWidth": true },
            { "data": "bfilBranchCode", "name": "BfilBranchCode", "autoWidth": true },
            { "data": "ifscCode", "name": "IfscCode", "autoWidth": true },
        ]
    });

    $("#indentReceivedDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": false,
        "deferLoading": 0,
        "scrollX": true,
        "scrollY": '50vh',
        "scrollCollapse": true,
        "ajax": {
            "url": `${indentBaseUrl}/GetIndents`,
            "type": "POST",
            "datatype": "json",
            "data": function (d) { return getFilterParams(d); }
        },
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        "aaSorting": [[1, 'desc']],
        "columns": [
            {
                "data": "indentNo", "name": "indentNo",
                render: function (row, type, data) {
                    return `<a href='${indentBaseUrl}/ReceivedDetail/${data.indentId}'>${data.indentNo}</a>`;
                }
            },
            {
                "data": "indentDate", "name": "indentDate",
                render: function (row, type, data) {
                    return getFormatDate(data.indentDate);
                }
            },
            { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
            { "data": "noOfKit", "name": "NoOfKit", "autoWidth": true },
            { "data": "stockPresent", "name": "stockPresent", "autoWidth": true },
            { "data": "schemeCode", "name": "SchemeCode", "autoWidth": true },
            { "data": "c5Code", "name": "C5Code", "autoWidth": true },
            { "data": "bfilBranchCode", "name": "BfilBranchCode", "autoWidth": true },
            {
                render: function (row, type, data) {
                    return `<a href='${indentBaseUrl}/ReceivedDetail/${data.indentId}'>Take Action</a>`;
                }
            },
        ]
    });

    $("#indentReceivedCpuDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": false,
        "deferLoading": 0,
        "scrollX": true,
        "scrollY": '50vh',
        "scrollCollapse": true,
        "ajax": {
            "url": `${indentBaseUrl}/GetIndents`,
            "type": "POST",
            "datatype": "json",
            "data": function (d) { return getFilterParams(d); }
        },
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        "aaSorting": [[1, 'desc']],
        "columns": [
            {
                "data": "indentNo", "name": "indentNo",
                render: function (row, type, data) {
                    return `<a href='${indentBaseUrl}/ReceivedDetail/${data.indentId}'>${data.indentNo}</a>`;
                }
            },
            {
                "data": "indentDate", "name": "indentDate",
                render: function (row, type, data) {
                    return getFormatDate(data.indentDate);
                }
            },
            { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
            { "data": "noOfKit", "name": "NoOfKit", "autoWidth": true },
            { "data": "schemeCode", "name": "SchemeCode", "autoWidth": true },
            { "data": "c5Code", "name": "C5Code", "autoWidth": true },
            { "data": "bfilBranchCode", "name": "BfilBranchCode", "autoWidth": true },
            {
                render: function (row, type, data) {
                    return `<a href='${indentBaseUrl}/ReceivedDetail/${data.indentId}'>Take Action</a>`;
                }
            },
        ]
    });

    $("#indentApprovedDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": false,
        "deferLoading": 0,
        "scrollX": true,
        "scrollY": '50vh',
        "scrollCollapse": true,
        "ajax": {
            "url": `${indentBaseUrl}/GetIndents`,
            "type": "POST",
            "datatype": "json",
            "data": function (d) { return getFilterParams(d); }
        },
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        "aaSorting": [[1, 'desc']],
        "columns": [
            {
                "data": "indentNo", "name": "indentNo",
                render: function (row, type, data) {
                    return `<a href='${indentBaseUrl}/ApprovedDetail/${data.indentId}'>${data.indentNo}</a>`;
                }
            },
            {
                "data": "indentDate", "name": "indentDate",
                render: function (row, type, data) {
                    return getFormatDate(data.indentDate);
                }
            },
            { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
            { "data": "schemeCode", "name": "SchemeCode", "autoWidth": true },
            { "data": "c5Code", "name": "C5Code", "autoWidth": true },
            { "data": "cardType", "name": "CardType", "autoWidth": true },
            { "data": "noOfKit", "name": "NoOfKit", "autoWidth": true },
            { "data": "bfilBranchType", "name": "BfilBranchType", "autoWidth": true },
            { "data": "bfilBranchCode", "name": "BfilBranchCode", "autoWidth": true },
            { "data": "ifscCode", "name": "IfscCode", "autoWidth": true },
        ]
    });

    $("#indentReceivedAtRoDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": false,
        "deferLoading": 0,
        "scrollX": true,
        "scrollY": '50vh',
        "scrollCollapse": true,
        "ajax": {
            "url": `${indentBaseUrl}/GetIndents`,
            "type": "POST",
            "datatype": "json",
            "data": function (d) { return getFilterParams(d); }
        },
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        "aaSorting": [[1, 'desc']],
        "columns": [
            {
                render: function (row, type, data) {
                    return `<input type="checkbox" name="indentAtRo"  value="${data.indentStatusId}" />`;
                }
            },
            {
                "data": "indentStatus", "name": "indentStatus",
                render: function (row, type, data) {
                    return getIndentStatusColor(data.indentStatusId, data.indentStatus);
                }
            },
            {
                "data": "indentNo", "name": "indentNo",
                render: function (row, type, data) {
                    return `<a href='${indentBaseUrl}/IndentDetail/${data.indentId}'>${data.indentNo}</a>`;
                }
            },
            {
                "data": "indentDate", "name": "indentDate",
                render: function (row, type, data) {
                    return getFormatDate(data.indentDate);
                }
            },
            { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
            { "data": "schemeCode", "name": "SchemeCode", "autoWidth": true },
            { "data": "c5Code", "name": "C5Code", "autoWidth": true },
            { "data": "cardType", "name": "CardType", "autoWidth": true },
            { "data": "noOfKit", "name": "NoOfKit", "autoWidth": true },
            { "data": "bfilBranchType", "name": "BfilBranchType", "autoWidth": true },
            { "data": "bfilBranchCode", "name": "BfilBranchCode", "autoWidth": true },
            { "data": "ifscCode", "name": "IfscCode", "autoWidth": true },
        ]
    });

    $("#indentForDispatchedDatatable, #indentCancelledDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": false,
        "deferLoading": 0,
        "scrollX": true,
        "scrollY": '50vh',
        "scrollCollapse": true,
        "ajax": {
            "url": `${indentBaseUrl}/GetIndents`,
            "type": "POST",
            "datatype": "json",
            "data": function (d) { return getFilterParams(d); }
        },
        "aaSorting": [[1, 'desc']],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        "columns": [
            {
                "data": "indentNo", "name": "indentNo",
                render: function (row, type, data) {
                    return `<a href='${indentBaseUrl}/${indentViewPage}/${data.indentId}'>${data.indentNo}</a>`;
                }
            },
            {
                "data": "indentDate", "name": "indentDate",
                render: function (row, type, data) {
                    return getFormatDate(data.indentDate);
                }
            },
            { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
            { "data": "schemeCode", "name": "SchemeCode", "autoWidth": true },
            { "data": "c5Code", "name": "C5Code", "autoWidth": true },
            { "data": "cardType", "name": "CardType", "autoWidth": true },
            { "data": "noOfKit", "name": "NoOfKit", "autoWidth": true },
            { "data": "noOfKitDispatched", "name": "noOfKitDispatched", "autoWidth": true },
            { "data": "bfilBranchType", "name": "BfilBranchType", "autoWidth": true },
            { "data": "bfilBranchCode", "name": "BfilBranchCode", "autoWidth": true },
            { "data": "ifscCode", "name": "IfscCode", "autoWidth": true },
        ]
    });

    var indentSelect = $(location).prop('hash');
    if (indentSelect != '') {
        showTab($('#indentTab a[href="' + indentSelect + '"]'));
    } else {
        showTab($("#indentTab a:first"));
    }

    $('#btnApprove').click(function (e) {
        var indentId = $('#IndentId').val();
        var indentStatusId = $('#IndentStatusId').val();
        var remarks = $('#ApproveRemarks').val();

        showLoading();

        $.ajax({
            url: `${indentBaseUrl}/UpdateIndentStatus`,
            type: 'POST',
            data: { indentId, indentStatusId, remarks, isApproved: true }, // approved
            success: function (result) {
                if (result.success) {
                    hideModel('approveModal');
                    window.location.href = `${indentBaseUrl}#indentReceived`;
                    displayToastr('Indent Approved!', 'Indent', "Success");
                } else {
                    displayToastr(result.message, 'Indent', "Error");
                }
                hideLoading();
            },
            error: function (error) {
                displayToastr(error.data, 'Indent', "Error");
                hideLoading();
            },
        });
    });

    $('#btnReject').click(function (e) {
        var indentId = $('#IndentId').val();
        var indentStatusId = $('#IndentStatusId').val();
        var remarks = $('#RejectRemarks').val();
        var rejectionReasonId = $('#RejectionReasonId').find("option:selected").val();

        if (rejectionReasonId === '') {
            displayToastr('Please select Reason for Rejection!', 'Reason for Rejection', "Error");
            return;
        }

        showLoading();

        $.ajax({
            url: `${indentBaseUrl}/UpdateIndentStatus`,
            type: 'POST',
            data: { indentId, indentStatusId, rejectionReasonId, remarks, isRejected: true }, // rejected
            success: function (result) {
                if (result.success) {
                    hideModel('rejectModal');
                    window.location.href = `${indentBaseUrl}#indentReceived`;
                    displayToastr('Indent Rejected!', 'Indent', "Success");
                } else {
                    displayToastr(result.message, 'Indent', "Error");
                }
                hideLoading();
            },
            error: function (error) {
                displayToastr(error.data, 'Indent', "Error");
                hideLoading();
            },
        });
    });

    $('#btnCancelIndent').click(function (e) {
        var indentId = $('#IndentId').val();
        var indentStatusId = $('#IndentStatusId').val();
        var remarks = $('#CancelRemarks').val();

        showLoading();

        $.ajax({
            url: `${indentBaseUrl}/UpdateIndentStatus`,
            type: 'POST',
            data: { indentId, remarks, isCancelled: true }, // Cancelled
            success: function (result) {
                if (result.success) {
                    hideModel('approveModal');
                    window.location.href = `${indentBaseUrl}#indentReceived`;
                    displayToastr('Indent Cancelled!', 'Indent', "Success");
                } else {
                    displayToastr(result.message, 'Indent', "Error");
                }
                hideLoading();
            },
            error: function (error) {
                displayToastr(error.data, 'Indent', "Error");
                hideLoading();
            },
        });
    });

    $('.receivedAtRo').click(function (e) {
        var indentId = $('#IndentId').val();
        var dispatchId = $(this).attr('dispatchId');

        showLoading();

        $.ajax({
            url: `${indentBaseUrl}/UpdateDispatchStatus`,
            type: 'POST',
            data: { indentId, dispatchId, isReceivedAtRo: true }, // ReceivedAtRo
            success: function (result) {
                if (result.success) {
                    displayToastr('Dispatch Received At Ro!', 'Indent Dispatch', "Success");
                    $(`#btnReceivedAtRo${dispatchId}`).hide();
                    $(`#btnDispatchToBranch${dispatchId}`).show();
                } else {
                    displayToastr(result.message, 'Indent Dispatch', "Error");
                }
                hideLoading();
            },
            error: function (error) {
                displayToastr(error.data, 'Indent Dispatch', "Error");
                hideLoading();
            },
        });
    });

    $('.dispatchToBranch').click(function (e) {
        var indentId = $('#IndentId').val();
        var dispatchId = $(this).attr('dispatchId');
        window.location.href = `${indentBaseUrl}/BranchDispatch/${dispatchId}`;
    });

    $('.receivedAtBranch').click(function (e) {
        var indentId = $('#IndentId').val();
        var dispatchId = $(this).attr('dispatchId');

        showLoading();

        $.ajax({
            url: `${indentBaseUrl}/UpdateDispatchStatus`,
            type: 'POST',
            data: { indentId, dispatchId, isReceivedAtBranch: true }, // Received At Branch
            success: function (result) {
                if (result.success) {
                    displayToastr('Dispatch Received At Branch!', 'Indent Dispatch', "Success");
                    if (result.data.isAllDispatch) {
                        window.location.href = `${indentBaseUrl}#indentDispatched`;
                    } else {
                        $(`#btnReceivedAtBranch${dispatchId}`).hide();
                    }
                } else {
                    displayToastr(result.message, 'Indent Dispatch', "Error");
                }
                hideLoading();
            },
            error: function (error) {
                displayToastr(error.data, 'Indent Dispatch', "Error");
                hideLoading();
            },
        });
    });

    $('#btnIndentDispatch').click(function (e) {
        var indentId = $('#IndentId').val();
        window.location.href = `${indentBaseUrl}/Dispatch/${indentId}`;
    });

    $('#btnIndentDispatchBack').click(function (e) {
        var indentId = $('#IndentId').val();
        window.location.href = `${indentBaseUrl}/DispatchDetail/${indentId}`;
    });

    $("#btnIndentUpload").click(function (e) {
        var files = $('#indentFileUpload').prop("files");
        if (files.length == 0) {
            displayToastr("Please select the file!", "Indnet", "Error")
            return;
        }
        showLoading();
        formData = new FormData();
        formData.append("file", files[0]);
        $('#indentUploadError').html('');

        jQuery.ajax({
            type: 'POST',
            url: `${indentBaseUrl}/UploadIndents`,
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success) {
                    hideModel('uploadExcelModal');
                    $('#indentFileUpload').val('');
                    window.location.href = `${indentBaseUrl}#indentRaised`;
                    displayToastr(response.message, "Indnet", "Success")
                } else {
                    $('#indentUploadError').html(response.message);
                    displayToastr("Indent Uplaod Error", "Indnet", "Error")
                }
                hideLoading();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                displayToastr(textStatus, "Indnet", "Error")
                hideLoading();
            }
        });
    });

    $("#btnDeliveryReportUpload").click(function (e) {
        var files = $('#deliveryReportFileUpload').prop("files");
        if (files.length == 0) {
            displayToastr("Please select the file!", "Dispatch Upload Report", "Error")
            return;
        }
        showLoading();

        var dispatchId = $('#dispatchId').val();
        formData = new FormData();
        formData.append("dispatchId", dispatchId);
        formData.append("file", files[0]);
        $('#reportUploadError').html('');

        jQuery.ajax({
            type: 'POST',
            url: `${indentBaseUrl}/UploadDeliveryReport`,
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success) {
                    $.each(response.data, function (index, w) {
                        $(`#deliveryDate${w.dispatchWayBillId}`).html(getFormatDate(w.deliveryDate));
                        $(`#courier${w.dispatchWayBillId}`).html(w.courierPartner);
                        $(`#receiver${w.dispatchWayBillId}`).html(w.receiveBy);
                    });
                    $(`#btnDeliveryReport${dispatchId}`).hide();
                    displayToastr(response.message, "Dispatch Upload Report", "Success")
                    hideModel('deliveryReportModal');
                } else {
                    displayToastr("Dispatch Upload Report Error", "Dispatch Upload Report", "Error")
                    $('#reportUploadError').html(response.message);
                }
                hideLoading();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                displayToastr(textStatus, "Dispatch Upload Report", "Error")
                hideLoading();
            }
        });
    });

    $("#DispatchNoOfKit").change(function (e) {
        validateDispatch();
    })

    $(".dispatch-input").change(function (e) {
        $(`#label${$(this).attr('id')}`).text($(this)[0].files[0].name)
        validateDispatch();
    })

    $("#btnIndentFilter").click(function (e) {
        $(`${$(currentSelectedTab).attr("href")}Datatable`).DataTable().ajax.reload();
    });

    $("#btnIndentFilterClear").click(function (e) {
        clearFilters();
        $(`${$(currentSelectedTab).attr("href")}Datatable`).DataTable().ajax.reload();
    });
});

function clearFilters() {
    $('#indentNo').val('');
    $('#indentStartDate').val('');
    $('#indentEndDate').val('');
    $('#branchId').val('').trigger('change');
    $('#schemeCodeId').val('').trigger('change');
    $('#c5CodeId').val('').trigger('change');
    $('#cardTypeId').val('').trigger('change');
    $('#bfilBranchTypeId').val('').trigger('change');
    $('#bfilRegionId').val('').trigger('change');
    $('#bfilBranchId').val('').trigger('change');

    branchId = null;
    schemeCodeId = null;
    c5CodeId = null;
    cardTypeId = null;
    bfilBranchTypeId = null;
    regionId = null;
    bfilBranchId = null;
    indentNo = null;
    indentStartDate = null;
    indentEndDate = null;

    $('#indentNoFilter').hide();
    $('#indentStartDateFilter').hide();
    $('#indentEndDateFilter').hide();
    $('#branchIdFilter').hide();
    $('#schemeCodeIdFilter').hide();
    $('#c5CodeIdFilter').hide();
    $('#cardTypeIdFilter').hide();
    $('#bfilBranchTypeIdFilter').hide();
    $('#bfilRegionIdFilter').hide();
    $('#bfilBranchIdFilter').hide();

    manageFilters();
}

function getFilterParams(d) {

    indentNo = $('#indentNo').val();
    indentStartDate = $('#indentStartDate').val();
    indentEndDate = $('#indentEndDate').val();
    branchId = $('#branchId').val();
    schemeCodeId = $('#schemeCodeId').val();
    c5CodeId = $('#c5CodeId').val();
    cardTypeId = $('#cardTypeId').val();
    bfilBranchTypeId = $('#bfilBranchTypeId').val();
    regionId = $('#bfilRegionId').val();
    bfilBranchId = $('#bfilBranchId').val();

    d.indentStatusIds = indentStatusIds;
    d.indentNo = indentNo;
    d.indentStartDate = indentStartDate;
    d.indentEndDate = indentEndDate;
    d.branchId = branchId;
    d.schemeCodeId = schemeCodeId;
    d.c5CodeId = c5CodeId;
    d.cardTypeId = cardTypeId;
    d.bfilBranchTypeId = bfilBranchTypeId;
    d.regionId = regionId;
    d.bfilBranchId = bfilBranchId;
    d.dispatchQty = dispatchQty;
    d.showStock = showStock;
}

function showTab(tab) {
    currentSelectedTab = tab;
    clearFilters();

    $(tab).tab('show');
    var indentTab = $(tab).attr("id");
    indentStatusIds = $(tab).attr("statusId");
    dispatchQty = $(tab).attr("dispatchQty");
    showStock = $(tab).attr("showStock");
    if (!indentLoaded.includes(indentTab)) {
        indentLoaded.push(indentTab);
        indentViewPage = $(tab).attr("viewPage");
        $(`${$(tab).attr("href")}Datatable`).DataTable().ajax.reload();
    }

    resizeDataTable();
}

function checkAddressChanged() {
    $('#IsAddressUpdated').val(false);

    if ($('#addressDetail').val() != $('#oldAddress').val() || $('#pinCode').val() != $('#oldPinCode').val()) {
        $('#IsAddressUpdated').val(true);
    }
}

function GetBranches(obj) {
    var regionId = $(obj).find("option:selected").val();
    if (regionId == '') {
        displayToastr('Please select Region!', "Indent", "Error")
        return;
    }
    $.ajax({
        url: `${indentBaseUrl}/GetBranches`,
        type: 'POST',
        data: { regionId },
        success: function (result) {
            if (result) {
                $('#BfilBranchId').html('');
                $('#IfscCode').val('');
                $('#BfilBranchId').append($("<option></option>").val
                    ('').html('Select BFIL Branch'));
                $.each(result, function (key, value) {
                    if (parseInt($('#BfilBranchIdVal').val()) == parseInt(value.Value)) {
                        $('#BfilBranchId').append($("<option selected=\"selected\"></option>").val
                            (value.id).html(`${value.branchCode} - ${value.text}`));
                    }
                    else {
                        $('#BfilBranchId').append($("<option></option>").val
                            (value.id).html(`${value.branchCode} - ${value.text}`));
                    }
                });
            }
        },
        error: function (error) {
            displayToastr(error.data, "Branch Detail", "Error")
        },
    });
}

function GetAddress(obj) {
    var regionId = $(obj).find("option:selected").val();
    $.ajax({
        url: `${indentBaseUrl}/GetAddressDetails`,
        type: 'POST',
        data: { 'regionId': regionId },
        success: function (result) {
            if (result) {
                $('#addressDetail').val(result.addressDetail);
                $('#pinCode').val(result.pinCode);
                $('#DispatchAddressId').val(result.addressId);
                $('#oldAddress').val(result.addressDetail);
                $('#oldPinCode').val(result.pinCode);
            }
        },
        error: function (error) {
            displayToastr(error.data, "Address Detail", "Error")
        },
    });
}

function GetBranch(obj) {
    var branchId = $(obj).find("option:selected").val();
    $.ajax({
        url: `${indentBaseUrl}/GetBranchDetails`,
        type: 'POST',
        data: { 'branchId': branchId },
        success: function (result) {
            if (result.success) {
                $('#IblBranchId').html('');
                $('#SchemeCodeId').html('');
                $('#C5CodeId').html('');
                $('#CardTypeId').html('');
                $('#IfscCode').val(result.data.branch.ifscCode);
                $.each(result.data.iblBranches, function (key, value) {
                    $('#IblBranchId').append($("<option></option>").val
                        (value.id).html(value.text));
                });
                $.each(result.data.schemeCodes, function (key, value) {
                    $('#SchemeCodeId').append($("<option></option>").val
                        (value.id).html(value.text));
                });
                $.each(result.data.c5Codes, function (key, value) {
                    $('#C5CodeId').append($("<option></option>").val
                        (value.id).html(value.text));
                });
                $.each(result.data.cardTypes, function (key, value) {
                    $('#CardTypeId').append($("<option></option>").val
                        (value.id).html(value.text));
                });
                $('#IblBranchId, #SchemeCodeId, #C5CodeId, #CardTypeId').trigger('blur');
            }
        },
        error: function (error) {
            displayToastr(error.data, "Branch Detail", "Error")
        },
    });
}

function uploadIndent() {
    $('#indentUploadError').html('');
    $("#indentFileUpload").val(null);
    displayModel('uploadExcelModal');
}

function uploadDeliveryReport(id) {
    $('#reportUploadError').html('');
    $('#dispatchId').val(id);
    $("#deliveryReportFileUpload").val(null);
    displayModel('deliveryReportModal');
}

function showBranchDispatch(showEle, hideEle) {
    $(`.${hideEle}`).hide();
    $(`.${showEle}`).show();
}

function manageFilters() {
    switch ($(currentSelectedTab).attr("id")) {
        case 'indentReceivedTab':
        case 'indentReceivedCpuTab':
            $('#indentNoFilter').show();
            $('#indentStartDateFilter').show();
            $('#indentEndDateFilter').show();
            $('#branchIdFilter').show();
            $('#schemeCodeIdFilter').show();
            $('#c5CodeIdFilter').show();
            $('#bfilRegionIdFilter').show();
            $('#bfilBranchIdFilter').show();
            break;
        default:
            $('#indentNoFilter').show();
            $('#indentStartDateFilter').show();
            $('#indentEndDateFilter').show();
            $('#branchIdFilter').show();
            $('#schemeCodeIdFilter').show();
            $('#c5CodeIdFilter').show();
            $('#cardTypeIdFilter').show();
            $('#bfilBranchTypeIdFilter').show();
            $('#bfilRegionIdFilter').show();
            $('#bfilBranchIdFilter').show();
            break;
    }
}

function validateIndent() {
    if ($("#formIndent").valid()) {
        showLoading();
        return true;
    }
    return false;
}

function validateDispatch() {
    $('#btnDispatch').prop('disabled', !(eval($('#DispatchNoOfKit').val()) > 0 &&
        $('#BranchDispatch').val() != '' &&
        $('#KitDispatch').val() != ''));
}