var reportBaseUrl = "/Report";
var indentLoaded = [];

var indentNo = null;
var referenceNo = null;
var wayBillNo = null;
var accountNo = null;
var cifNo = null;
var indentStartDate = null;
var indentEndDate = null;
var dispatchStartDate = null;
var dispatchEndDate = null;
var branchId = null;
var schemeCodeId = null;
var c5CodeId = null;
var cardTypeId = null;
var bfilBranchTypeId = null;
var regionId = null;
var bfilBranchId = null;

$(document).ready(function () {

    $(`#${$('#reportName').val()}`).DataTable({
        "processing": true,
        "serverSide": true,
        "filter": false,
        "bLengthChange": false,
        "bPaginate": true,
        "bInfo": false,
        "fnDrawCallback": function (oSettings) {
            $('.dataTables_paginate').hide();
        },
        "deferLoading": 0,
        "ordering": false,
        "dom": 'Bfrtip',
        "scrollX": true,
        "scrollY": '50vh',
        "scrollCollapse": true,
        "buttons": [
            { extend: 'copy' },
            { extend: 'csv' }
        ],
        "ajax": {
            "url": getReportURL(),
            "type": "POST",
            "datatype": "json",
            "data": function (d) { return getFilterParams(d); }
        },

        "columns": getColumns()
    });

    $("#btnReportFilter").click(function (e) {
        e.preventDefault();
        $(`#${$('#reportName').val()}`).DataTable().ajax.reload();
    });

    $("#btnReportFilterClear").click(function (e) {
        e.preventDefault();
        clearFilters();
        $(`#${$('#reportName').val()} tbody`).html('<tr class="odd"><td valign="top" colspan="20" class="dataTables_empty">No data available in table</td></tr>');
    });

    $('#btn-copy-export').on('click', function () {
        $('.buttons-copy').click()
    });

    $('#btn-csv-export').on('click', function () {
        $('.buttons-csv').click()
    });

    clearFilters();
});

function getIndentReportColumns() {
    return [
        { "data": "indentNumber", "name": "indentNumber", "autoWidth": true },
        {
            "data": "indentDate", "name": "indentDate",
            render: function (row, type, data) {
                return getFormatDate(data.indentDate);
            }
        },
        { "data": "iblBranchName", "name": "iblBranchName", "autoWidth": true },
        { "data": "schemeCode", "name": "schemeCode", "autoWidth": true },
        { "data": "c5Code", "name": "c5Code", "autoWidth": true },
        { "data": "cardType", "name": "cardType", "autoWidth": true },
        { "data": "noOfKit", "name": "noOfKit", "autoWidth": true },
        { "data": "branchType", "name": "branchType", "autoWidth": true },
        { "data": "dispatchAddress", "name": "dispatchAddress", "autoWidth": true },
        { "data": "bfilBranchCode", "name": "bfilBranchCode", "autoWidth": true },
        { "data": "pinCode", "name": "pinCode", "autoWidth": true },
        { "data": "contactPerson", "name": "contactPerson", "autoWidth": true },
        { "data": "contactNumber", "name": "contactNumber", "autoWidth": true },
        { "data": "indentedUserType", "name": "indentedUserType", "autoWidth": true },
        { "data": "indentedBy", "name": "indentedBy", "autoWidth": true },
        { "data": "bfilIndentHOStaus", "name": "bfilIndentHOStaus", "autoWidth": true },
        { "data": "bfilIndentHoApproveBy", "name": "bfilIndentHoApproveBy", "autoWidth": true },
        {
            "data": "hoApproveDate", "name": "hoApproveDate",
            render: function (row, type, data) {
                return getFormatDateTime(data.hoApproveDate);
            }
        },
        { "data": "cpuIndentStaus", "name": "cpuIndentStaus", "autoWidth": true },
        { "data": "cpuIndentHoApproveBy", "name": "cpuIndentHoApproveBy", "autoWidth": true },
        {
            "data": "cpuApproveDate", "name": "cpuApproveDate",
            render: function (row, type, data) {
                return getFormatDateTime(data.cpuApproveDate);
            }
        },
    ];
}
function getDispatchReportColumns() {
    return [
        { "data": "indentNumber", "name": "indentNumber", "autoWidth": true },
        {
            "data": "hoApproveDate", "name": "hoApproveDate",
            render: function (row, type, data) {
                return getFormatDate(data.hoApproveDate);
            }
        },
        {
            "data": "cpuApproveDate", "name": "cpuApproveDate",
            render: function (row, type, data) {
                return getFormatDate(data.cpuApproveDate);
            }
        },
        {
            "data": "dateOfDispatch", "name": "dateOfDispatch",
            render: function (row, type, data) {
                return getFormatDate(data.dateOfDispatch);
            }
        },
        {
            "data": "reportUploadDate", "name": "reportUploadDate",
            render: function (row, type, data) {
                return getFormatDate(data.reportUploadDate);
            }
        },
        { "data": "reportUploadBy", "name": "reportUploadBy", "autoWidth": true },
        { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
        { "data": "iblBranchName", "name": "iblBranchName", "autoWidth": true },
        { "data": "bfilBranchCode", "name": "bfilBranchCode", "autoWidth": true },
        { "data": "schemeCode", "name": "schemeCode", "autoWidth": true },
        { "data": "cardType", "name": "cardType", "autoWidth": true },
        { "data": "vendor", "name": "vendor", "autoWidth": true },
        { "data": "qty", "name": "qty", "autoWidth": true },
        { "data": "accountStart", "name": "accountStart", "autoWidth": true },
        { "data": "accountEnd", "name": "accountEnd", "autoWidth": true },
        { "data": "schemeType", "name": "schemeType", "autoWidth": true },
        { "data": "referenceNumber", "name": "referenceNumber", "autoWidth": true },
        {
            data: 'wayBills',
            render: function (data) {
                var wayBills = `<ul>`
                $.each(data, function (index, value) {
                    wayBills = `${wayBills}<li>${value}</li>`
                });
                return `${wayBills}</ul>`;
            },
        },
    ];
}
function getDeliveryReportColumns() {
    return [
        { "data": "indentNumber", "name": "indentNumber", "autoWidth": true },
        { "data": "wayBillNo", "name": "wayBillNo", "autoWidth": true },
        {
            "data": "dateOfDispatch", "name": "dateOfDispatch",
            render: function (row, type, data) {
                return getFormatDate(data.dateOfDispatch);
            }
        },
        { "data": "status", "name": "status", "autoWidth": true },
        {
            "data": "deliveryDate", "name": "deliveryDate",
            render: function (row, type, data) {
                return getFormatDate(data.deliveryDate);
            }
        },
        { "data": "receiverName", "name": "receiverName", "autoWidth": true },
        { "data": "courierName", "name": "courierName", "autoWidth": true },
        { "data": "referenceNumber", "name": "referenceNumber", "autoWidth": true },
    ];
}
function getAccountLevelDispatchReportColumns() {
    return [
        { "data": "indentNumber", "name": "indentNumber", "autoWidth": true },
        {
            "data": "hoApproveDate", "name": "hoApproveDate",
            render: function (row, type, data) {
                return getFormatDate(data.hoApproveDate);
            }
        },
        {
            "data": "cpuApproveDate", "name": "cpuApproveDate",
            render: function (row, type, data) {
                return getFormatDate(data.cpuApproveDate);
            }
        },
        {
            "data": "dateOfDispatch", "name": "dateOfDispatch",
            render: function (row, type, data) {
                return getFormatDate(data.dateOfDispatch);
            }
        },
        {
            "data": "reportUploadDate", "name": "reportUploadDate",
            render: function (row, type, data) {
                return getFormatDate(data.reportUploadDate);
            }
        },
        { "data": "reportUploadBy", "name": "reportUploadBy", "autoWidth": true },
        {
            "data": "dateOfUpload", "name": "dateOfUpload",
            render: function (row, type, data) {
                return getFormatDate(data.dateOfUpload);
            }
        },
        { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
        { "data": "iblBranchName", "name": "iblBranchName", "autoWidth": true },
        { "data": "bfilBranchCode", "name": "bfilBranchCode", "autoWidth": true },
        { "data": "cifNo", "name": "cifNo", "autoWidth": true },
        { "data": "accountNo", "name": "accountNo", "autoWidth": true },
        { "data": "status", "name": "status", "autoWidth": true },
    ];
}
function getReceivedAtROReportColumns() {
    return [
        { "data": "indentNumber", "name": "indentNumber", "autoWidth": true },
        {
            "data": "hoApproveDate", "name": "hoApproveDate",
            render: function (row, type, data) {
                return getFormatDate(data.hoApproveDate);
            }
        },
        {
            "data": "cpuApproveDate", "name": "cpuApproveDate",
            render: function (row, type, data) {
                return getFormatDate(data.cpuApproveDate);
            }
        },
        {
            "data": "dateOfDispatch", "name": "dateOfDispatch",
            render: function (row, type, data) {
                return getFormatDate(data.dateOfDispatch);
            }
        },
        {
            "data": "reportUploadDate", "name": "reportUploadDate",
            render: function (row, type, data) {
                return getFormatDate(data.reportUploadDate);
            }
        },
        { "data": "reportUploadBy", "name": "reportUploadBy", "autoWidth": true },
        { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
        { "data": "iblBranchName", "name": "iblBranchName", "autoWidth": true },
        { "data": "bfilBranchCode", "name": "bfilBranchCode", "autoWidth": true },
        { "data": "schemeCode", "name": "schemeCode", "autoWidth": true },
        { "data": "cardType", "name": "cardType", "autoWidth": true },
        { "data": "vendor", "name": "vendor", "autoWidth": true },
        { "data": "qty", "name": "qty", "autoWidth": true },
        { "data": "accountStart", "name": "accountStart", "autoWidth": true },
        { "data": "accountEnd", "name": "accountEnd", "autoWidth": true },
        { "data": "schemeType", "name": "schemeType", "autoWidth": true },
        {
            "data": "reportDate", "name": "reportDate",
            render: function (row, type, data) {
                return getFormatDate(data.reportDate);
            }
        },
        { "data": "referenceNumber", "name": "referenceNumber", "autoWidth": true },
        {
            data: 'wayBills',
            render: function (data) {
                var wayBills = `<ul>`
                $.each(data, function (index, value) {
                    wayBills = `${wayBills}<li>${value}</li>`
                });
                return `${wayBills}</ul>`;
            },
        },
        { "data": "status", "name": "status", "autoWidth": true },
        { "data": "user", "name": "user", "autoWidth": true },
        {
            "data": "userConfirmationDate", "name": "userConfirmationDate",
            render: function (row, type, data) {
                return getFormatDateTime(data.userConfirmationDate);
            }
        },

    ];
}
function getReceivedAtBranchReportColumns() {
    return [
        { "data": "indentNumber", "name": "indentNumber", "autoWidth": true },
        {
            "data": "hoApproveDate", "name": "hoApproveDate",
            render: function (row, type, data) {
                return getFormatDate(data.hoApproveDate);
            }
        },
        {
            "data": "cpuApproveDate", "name": "cpuApproveDate",
            render: function (row, type, data) {
                return getFormatDate(data.cpuApproveDate);
            }
        },
        {
            "data": "dateOfDispatch", "name": "dateOfDispatch",
            render: function (row, type, data) {
                return getFormatDate(data.dateOfDispatch);
            }
        },
        {
            "data": "reportUploadDate", "name": "reportUploadDate",
            render: function (row, type, data) {
                return getFormatDate(data.reportUploadDate);
            }
        },
        { "data": "reportUploadBy", "name": "reportUploadBy", "autoWidth": true },
        { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
        { "data": "iblBranchName", "name": "iblBranchName", "autoWidth": true },
        { "data": "bfilBranchCode", "name": "bfilBranchCode", "autoWidth": true },
        { "data": "schemeCode", "name": "schemeCode", "autoWidth": true },
        { "data": "cardType", "name": "cardType", "autoWidth": true },
        { "data": "vendor", "name": "vendor", "autoWidth": true },
        { "data": "qty", "name": "qty", "autoWidth": true },
        { "data": "accountStart", "name": "accountStart", "autoWidth": true },
        { "data": "accountEnd", "name": "accountEnd", "autoWidth": true },
        { "data": "schemeType", "name": "schemeType", "autoWidth": true },
        {
            "data": "reportDate", "name": "reportDate",
            render: function (row, type, data) {
                return getFormatDate(data.reportDate);
            }
        },
        { "data": "referenceNumber", "name": "referenceNumber", "autoWidth": true },
        {
            data: 'wayBills',
            render: function (data) {
                var wayBills = `<ul>`
                $.each(data, function (index, value) {
                    wayBills = `${wayBills}<li>${value}</li>`
                });
                return `${wayBills}</ul>`;
            },
        },
        { "data": "status", "name": "status", "autoWidth": true },
        { "data": "user", "name": "user", "autoWidth": true },
        {
            "data": "userConfirmationDate", "name": "userConfirmationDate",
            render: function (row, type, data) {
                return getFormatDateTime(data.userConfirmationDate);
            }
        },
    ];
}
function getScannedReportColumns() {
    return [
        { "data": "indentNumber", "name": "indentNumber", "autoWidth": true },
        {
            "data": "hoApproveDate", "name": "hoApproveDate",
            render: function (row, type, data) {
                return getFormatDate(data.hoApproveDate);
            }
        },
        {
            "data": "cpuApproveDate", "name": "cpuApproveDate",
            render: function (row, type, data) {
                return getFormatDate(data.cpuApproveDate);
            }
        },
        {
            "data": "dateOfDispatch", "name": "dateOfDispatch",
            render: function (row, type, data) {
                return getFormatDate(data.dateOfDispatch);
            }
        },
        {
            "data": "reportUploadDate", "name": "reportUploadDate",
            render: function (row, type, data) {
                return getFormatDate(data.reportUploadDate);
            }
        },
        { "data": "reportUploadBy", "name": "reportUploadBy", "autoWidth": true },
        {
            "data": "dateOfUpload", "name": "dateOfUpload",
            render: function (row, type, data) {
                return getFormatDate(data.dateOfUpload);
            }
        },
        { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
        { "data": "iblBranchName", "name": "iblBranchName", "autoWidth": true },
        { "data": "bfilBranchCode", "name": "bfilBranchCode", "autoWidth": true },
        { "data": "cifNo", "name": "cifNo", "autoWidth": true },
        { "data": "accountNo", "name": "accountNo", "autoWidth": true },
        { "data": "status", "name": "status", "autoWidth": true },
        { "data": "receivedBy", "name": "receivedBy", "autoWidth": true },
        {
            "data": "receivedDate", "name": "receivedDate",
            render: function (row, type, data) {
                return getFormatDateTime(data.receivedDate);
            }
        },
        { "data": "reportStatus", "name": "reportStatus", "autoWidth": true },
        { "data": "scannedBy", "name": "scannedBy", "autoWidth": true },
        {
            "data": "scannedDate", "name": "scannedDate",
            render: function (row, type, data) {
                return getFormatDateTime(data.scannedDate);
            }
        },
    ];
}
function getAllocationReportColumns() {
    return [
        { "data": "indentNumber", "name": "indentNumber", "autoWidth": true },
        {
            "data": "hoApproveDate", "name": "hoApproveDate",
            render: function (row, type, data) {
                return getFormatDate(data.hoApproveDate);
            }
        },
        {
            "data": "cpuApproveDate", "name": "cpuApproveDate",
            render: function (row, type, data) {
                return getFormatDate(data.cpuApproveDate);
            }
        },
        {
            "data": "dateOfDispatch", "name": "dateOfDispatch",
            render: function (row, type, data) {
                return getFormatDate(data.dateOfDispatch);
            }
        },
        {
            "data": "reportUploadDate", "name": "reportUploadDate",
            render: function (row, type, data) {
                return getFormatDate(data.reportUploadDate);
            }
        },
        { "data": "reportUploadBy", "name": "reportUploadBy", "autoWidth": true },
        {
            "data": "dateOfUpload", "name": "dateOfUpload",
            render: function (row, type, data) {
                return getFormatDate(data.dateOfUpload);
            }
        },
        { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
        { "data": "iblBranchName", "name": "iblBranchName", "autoWidth": true },
        { "data": "bfilBranchCode", "name": "bfilBranchCode", "autoWidth": true },
        { "data": "cifNo", "name": "cifNo", "autoWidth": true },
        { "data": "accountNo", "name": "accountNo", "autoWidth": true },
        { "data": "status", "name": "status", "autoWidth": true },
        { "data": "receivedBy", "name": "receivedBy", "autoWidth": true },
        {
            "data": "receivedDate", "name": "receivedDate",
            render: function (row, type, data) {
                return getFormatDateTime(data.receivedDate);
            }
        },
        { "data": "reportStatus", "name": "reportStatus", "autoWidth": true },
        { "data": "scannedBy", "name": "scannedBy", "autoWidth": true },
        {
            "data": "scannedDate", "name": "scannedDate",
            render: function (row, type, data) {
                return getFormatDateTime(data.scannedDate);
            }
        },
        { "data": "allocatedToStaffId", "name": "allocatedToStaffId", "autoWidth": true },
        { "data": "allocatedTo", "name": "allocatedTo", "autoWidth": true },
        { "data": "allocatedBy", "name": "allocatedBy", "autoWidth": true },
        {
            "data": "allocatedDate", "name": "allocatedDate",
            render: function (row, type, data) {
                return getFormatDateTime(data.allocatedDate);
            }
        },
    ];
}
function getAssignedReportColumns() {
    return [
        { "data": "indentNumber", "name": "indentNumber", "autoWidth": true },
        {
            "data": "hoApproveDate", "name": "hoApproveDate",
            render: function (row, type, data) {
                return getFormatDate(data.hoApproveDate);
            }
        },
        {
            "data": "cpuApproveDate", "name": "cpuApproveDate",
            render: function (row, type, data) {
                return getFormatDate(data.cpuApproveDate);
            }
        },
        {
            "data": "dateOfDispatch", "name": "dateOfDispatch",
            render: function (row, type, data) {
                return getFormatDate(data.dateOfDispatch);
            }
        },
        {
            "data": "reportUploadDate", "name": "reportUploadDate",
            render: function (row, type, data) {
                return getFormatDate(data.reportUploadDate);
            }
        },
        { "data": "reportUploadBy", "name": "reportUploadBy", "autoWidth": true },
        {
            "data": "dateOfUpload", "name": "dateOfUpload",
            render: function (row, type, data) {
                return getFormatDate(data.dateOfUpload);
            }
        },
        { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
        { "data": "iblBranchName", "name": "iblBranchName", "autoWidth": true },
        { "data": "bfilBranchCode", "name": "bfilBranchCode", "autoWidth": true },
        { "data": "cifNo", "name": "cifNo", "autoWidth": true },
        { "data": "accountNo", "name": "accountNo", "autoWidth": true },
        { "data": "status", "name": "status", "autoWidth": true },
        { "data": "receivedBy", "name": "receivedBy", "autoWidth": true },
        {
            "data": "receivedDate", "name": "receivedDate",
            render: function (row, type, data) {
                return getFormatDateTime(data.receivedDate);
            }
        },
        { "data": "reportStatus", "name": "reportStatus", "autoWidth": true },
        { "data": "scannedBy", "name": "scannedBy", "autoWidth": true },
        {
            "data": "scannedDate", "name": "scannedDate",
            render: function (row, type, data) {
                return getFormatDateTime(data.scannedDate);
            }
        },
        { "data": "allocatedToStaffId", "name": "allocatedToStaffId", "autoWidth": true },
        { "data": "allocatedTo", "name": "allocatedTo", "autoWidth": true },
        { "data": "allocatedBy", "name": "allocatedBy", "autoWidth": true },
        {
            "data": "allocatedDate", "name": "allocatedDate",
            render: function (row, type, data) {
                return getFormatDateTime(data.allocatedDate);
            }
        },
        {
            "data": "assignedDate", "name": "assignedDate",
            render: function (row, type, data) {
                return getFormatDateTime(data.assignedDate);
            }
        },
        { "data": "assignedTo", "name": "assignedTo", "autoWidth": true }
    ];
}
function getDestructionReportColumns() {
    return [
        { "data": "indentNumber", "name": "indentNumber", "autoWidth": true },
        {
            "data": "hoApproveDate", "name": "hoApproveDate",
            render: function (row, type, data) {
                return getFormatDate(data.hoApproveDate);
            }
        },
        {
            "data": "cpuApproveDate", "name": "cpuApproveDate",
            render: function (row, type, data) {
                return getFormatDate(data.cpuApproveDate);
            }
        },
        {
            "data": "dateOfDispatch", "name": "dateOfDispatch",
            render: function (row, type, data) {
                return getFormatDate(data.dateOfDispatch);
            }
        },
        {
            "data": "reportUploadDate", "name": "reportUploadDate",
            render: function (row, type, data) {
                return getFormatDate(data.reportUploadDate);
            }
        },
        { "data": "reportUploadBy", "name": "reportUploadBy", "autoWidth": true },
        {
            "data": "dateOfUpload", "name": "dateOfUpload",
            render: function (row, type, data) {
                return getFormatDate(data.dateOfUpload);
            }
        },
        { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
        { "data": "iblBranchName", "name": "iblBranchName", "autoWidth": true },
        { "data": "bfilBranchCode", "name": "bfilBranchCode", "autoWidth": true },
        { "data": "cifNo", "name": "cifNo", "autoWidth": true },
        { "data": "accountNo", "name": "accountNo", "autoWidth": true },
        { "data": "status", "name": "status", "autoWidth": true },
        { "data": "receivedBy", "name": "receivedBy", "autoWidth": true },
        {
            "data": "receivedDate", "name": "receivedDate",
            render: function (row, type, data) {
                return getFormatDateTime(data.receivedDate);
            }
        },
        { "data": "reportStatus", "name": "reportStatus", "autoWidth": true },
        { "data": "reason", "name": "reason", "autoWidth": true },
        { "data": "scannedBy", "name": "scannedBy", "autoWidth": true },
        {
            "data": "scannedDate", "name": "scannedDate",
            render: function (row, type, data) {
                return getFormatDateTime(data.scannedDate);
            }
        },
        { "data": "desturtionApproved", "name": "desturtionApproved", "autoWidth": true },
        { "data": "destructionBy", "name": "destructionBy", "autoWidth": true },
        {
            "data": "destructionByDate", "name": "destructionByDate",
            render: function (row, type, data) {
                return getFormatDateTime(data.destructionByDate);
            }
        },
    ];
}
function getTotalStockReportColumns() {
    return [
        { "data": "bfilBranchCode", "name": "bfilBranchCode", "autoWidth": true },
        { "data": "bfilBranchName", "name": "bfilBranchName", "autoWidth": true },
        { "data": "totalIndented", "name": "totalIndented", "autoWidth": true },
        { "data": "totalDispatched", "name": "totalDispatched", "autoWidth": true },
        { "data": "totalReceived", "name": "totalReceived", "autoWidth": true },
        { "data": "totalAllocated", "name": "totalAllocated", "autoWidth": true },
        { "data": "totalDestructed", "name": "totalDestructed", "autoWidth": true },
        { "data": "totalAssigned", "name": "totalAssigned", "autoWidth": true },
    ];
}
function getReturnReportColumns() {
    return [
        { "data": "indentNumber", "name": "indentNumber", "autoWidth": true },
        {
            "data": "hoApproveDate", "name": "hoApproveDate",
            render: function (row, type, data) {
                return getFormatDate(data.hoApproveDate);
            }
        },
        {
            "data": "cpuApproveDate", "name": "cpuApproveDate",
            render: function (row, type, data) {
                return getFormatDate(data.cpuApproveDate);
            }
        },
        {
            "data": "dateOfDispatch", "name": "dateOfDispatch",
            render: function (row, type, data) {
                return getFormatDate(data.dateOfDispatch);
            }
        },
        {
            "data": "reportUploadDate", "name": "reportUploadDate",
            render: function (row, type, data) {
                return getFormatDate(data.reportUploadDate);
            }
        },
        { "data": "reportUploadBy", "name": "reportUploadBy", "autoWidth": true },
        {
            "data": "dateOfUpload", "name": "dateOfUpload",
            render: function (row, type, data) {
                return getFormatDate(data.dateOfUpload);
            }
        },
        { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
        { "data": "iblBranchName", "name": "iblBranchName", "autoWidth": true },
        { "data": "bfilBranchCode", "name": "bfilBranchCode", "autoWidth": true },
        { "data": "cifNo", "name": "cifNo", "autoWidth": true },
        { "data": "accountNo", "name": "accountNo", "autoWidth": true },
        { "data": "status", "name": "status", "autoWidth": true },
        { "data": "receivedBy", "name": "receivedBy", "autoWidth": true },
        {
            "data": "receivedDate", "name": "receivedDate",
            render: function (row, type, data) {
                return getFormatDateTime(data.receivedDate);
            }
        },
        { "data": "reportStatus", "name": "reportStatus", "autoWidth": true },
        { "data": "scannedBy", "name": "scannedBy", "autoWidth": true },
        {
            "data": "scannedDate", "name": "scannedDate",
            render: function (row, type, data) {
                return getFormatDateTime(data.scannedDate);
            }
        },
        { "data": "allocatedToStaffId", "name": "allocatedToStaffId", "autoWidth": true },
        { "data": "allocatedTo", "name": "allocatedTo", "autoWidth": true },
        { "data": "allocatedBy", "name": "allocatedBy", "autoWidth": true },
        {
            "data": "allocatedDate", "name": "allocatedDate",
            render: function (row, type, data) {
                return getFormatDateTime(data.allocatedDate);
            }
        },
        { "data": "returnedBy", "name": "returnedBy", "autoWidth": true },
        {
            "data": "returnedDate", "name": "returnedDate",
            render: function (row, type, data) {
                return getFormatDateTime(data.returnedDate);
            }
        },
        { "data": "returnedAcceptBy", "name": "returnedAcceptBy", "autoWidth": true },
    ];
}

//Generic functions
function getColumns() {
    switch ($('#reportName').val()) {
        case 'indentReportDatatable':
            return getIndentReportColumns();

        case 'dispatchReportDatatable':
            return getDispatchReportColumns();

        case 'deliveryReportDatatable':
            return getDeliveryReportColumns();

        case 'accountLevelDispatchReportDatatable':
            return getAccountLevelDispatchReportColumns();

        case 'receivedAtROReportDatatable':
            return getReceivedAtROReportColumns();

        case 'receivedAtBranchReportDatatable':
            return getReceivedAtBranchReportColumns();

        case 'scannedReportDatatable':
            return getScannedReportColumns();

        case 'allocationReportDatatable':
            return getAllocationReportColumns();

        case 'assignedReportDatatable':
            return getAssignedReportColumns();

        case 'destructionReportDatatable':
            return getDestructionReportColumns();

        case 'totalStockReportDatatable':
            return getTotalStockReportColumns();

        case 'returnReportDatatable':
            return getReturnReportColumns();
    }
}

function getReportURL() {
    switch ($('#reportName').val()) {
        case 'indentReportDatatable':
            return `${reportBaseUrl}/GetIndentReport`;
        case 'dispatchReportDatatable':
            return `${reportBaseUrl}/GetDispatchReport`;
        case 'deliveryReportDatatable':
            return `${reportBaseUrl}/GetDeliveryReport`;
        case 'accountLevelDispatchReportDatatable':
            return `${reportBaseUrl}/GetAccountLevelDispatchReport`;
        case 'receivedAtROReportDatatable':
            return `${reportBaseUrl}/GetReceivedAtROReport`;
        case 'receivedAtBranchReportDatatable':
            return `${reportBaseUrl}/GetReceivedAtBranchReport`;
        case 'scannedReportDatatable':
            return `${reportBaseUrl}/GetScannedReport`;
        case 'allocationReportDatatable':
            return `${reportBaseUrl}/GetAllocationReport`;
        case 'assignedReportDatatable':
            return `${reportBaseUrl}/GetAssignedReport`;
        case 'destructionReportDatatable':
            return `${reportBaseUrl}/GetDestructionReport`;
        case 'totalStockReportDatatable':
            return `${reportBaseUrl}/GetTotalStockReport`;
        case 'returnReportDatatable':
            return `${reportBaseUrl}/GetReturnReport`;
    }
}

function clearFilters() {
    $('#indentNo').val('');
    $('#referenceNo').val('');
    $('#wayBillNo').val('');
    $('#accountNo').val('');
    $('#cifNo').val('');
    $('#indentStartDate').val('');
    $('#indentEndDate').val('');
    $('#dispatchStartDate').val('');
    $('#dispatchEndDate').val('');
    $('#deliveryStartDate').val('');
    $('#deliveryEndDate').val('');
    $('#branchId').val('').trigger('change');
    $('#schemeCodeId').val('').trigger('change');
    $('#c5CodeId').val('').trigger('change');
    $('#cardTypeId').val('').trigger('change');
    $('#bfilBranchTypeId').val('').trigger('change');
    $('#bfilRegionId').val('').trigger('change');
    $('#bfilBranchId').val('').trigger('change');

    indentNo = null;
    referenceNo = null;
    wayBillNo = null;
    accountNo = null;
    cifNo = null;
    indentStartDate = null;
    indentEndDate = null;
    dispatchStartDate = null;
    dispatchEndDate = null;
    branchId = null;
    schemeCodeId = null;
    c5CodeId = null;
    cardTypeId = null;
    bfilBranchTypeId = null;
    regionId = null;
    bfilBranchId = null;

    $('#indentNoFilter').hide();
    $('#referenceNoFilter').hide();
    $('#wayBillNoFilter').hide();
    $('#accountNoFilter').hide();
    $('#cifNoFilter').hide();
    $('#indentStartDateFilter').hide();
    $('#indentEndDateFilter').hide();
    $('#dispatchStartDateFilter').hide();
    $('#dispatchEndDateFilter').hide();
    $('#deliveryStartDateFilter').hide();
    $('#deliveryEndDateFilter').hide();
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
    referenceNo = $('#referenceNo').val();
    wayBillNo = $('#wayBillNo').val();
    accountNo = $('#accountNo').val();
    cifNo = $('#cifNo').val();
    indentStartDate = $('#indentStartDate').val();
    indentEndDate = $('#indentEndDate').val();
    dispatchStartDate = $('#dispatchStartDate').val();
    dispatchEndDate = $('#dispatchEndDate').val();
    deliveryStartDate = $('#deliveryStartDate').val();
    deliveryEndDate = $('#deliveryEndDate').val();
    branchId = $('#branchId').val();
    schemeCodeId = $('#schemeCodeId').val();
    c5CodeId = $('#c5CodeId').val();
    cardTypeId = $('#cardTypeId').val();
    bfilBranchTypeId = $('#bfilBranchTypeId').val();
    regionId = $('#bfilRegionId').val();
    bfilBranchId = $('#bfilBranchId').val();

    d.indentNo = indentNo;
    d.referenceNo = referenceNo;
    d.wayBillNo = wayBillNo;
    d.accountNo = accountNo;
    d.cifNo = cifNo;
    d.indentStartDate = indentStartDate;
    d.indentEndDate = indentEndDate;
    d.dispatchStartDate = dispatchStartDate;
    d.dispatchEndDate = dispatchEndDate;
    d.deliveryStartDate = deliveryStartDate;
    d.deliveryEndDate = deliveryEndDate;
    d.branchId = branchId;
    d.schemeCodeId = schemeCodeId;
    d.c5CodeId = c5CodeId;
    d.cardTypeId = cardTypeId;
    d.bfilBranchTypeId = bfilBranchTypeId;
    d.regionId = regionId;
    d.bfilBranchId = bfilBranchId;
}

function manageFilters() {
    switch ($('#reportName').val()) {
        case 'indentReportDatatable':
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
        case 'dispatchReportDatatable':
            $('#indentNoFilter').show();
            $('#indentStartDateFilter').show();
            $('#indentEndDateFilter').show();
            $('#dispatchStartDateFilter').show();
            $('#dispatchEndDateFilter').show();
            $('#branchIdFilter').show();
            $('#schemeCodeIdFilter').show();
            $('#c5CodeIdFilter').show();
            $('#cardTypeIdFilter').show();
            $('#bfilRegionIdFilter').show();
            $('#bfilBranchIdFilter').show();
            break;
        case 'deliveryReportDatatable':
            $('#indentNoFilter').show();
            $('#referenceNoFilter').show();
            $('#wayBillNoFilter').show();
            $('#dispatchStartDateFilter').show();
            $('#dispatchEndDateFilter').show();
            $('#deliveryStartDateFilter').show();
            $('#deliveryEndDateFilter').show();
            break;
        case 'accountLevelDispatchReportDatatable':
        case 'scannedReportDatatable':
        case 'allocationReportDatatable':
        case 'assignedReportDatatable':
        case 'destructionReportDatatable':
        case 'returnReportDatatable':
            $('#indentNoFilter').show();
            $('#accountNoFilter').show();
            $('#cifNoFilter').show();
            $('#dispatchStartDateFilter').show();
            $('#dispatchEndDateFilter').show();
            $('#branchIdFilter').show();
            $('#bfilRegionIdFilter').show();
            $('#bfilBranchIdFilter').show();
            break;
        case 'receivedAtROReportDatatable':
        case 'receivedAtBranchReportDatatable':
            $('#indentNoFilter').show();
            $('#referenceNoFilter').show();
            $('#wayBillNoFilter').show();
            $('#dispatchStartDateFilter').show();
            $('#dispatchEndDateFilter').show();
            $('#branchIdFilter').show();
            $('#schemeCodeIdFilter').show();
            $('#cardTypeIdFilter').show();
            $('#bfilRegionIdFilter').show();
            $('#bfilBranchIdFilter').show();
            break;
        case 'totalStockReportDatatable':
            $('#bfilRegionIdFilter').show();
            $('#bfilBranchIdFilter').show();
            break;
        default:
            break;
    }
}

