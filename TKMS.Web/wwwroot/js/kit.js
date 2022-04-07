var kitBaseUrl = "/Kit";
var kitLoaded = [];

var kitStatusIds = null;

var indentNo = null;
var accountNo = null;
var cifNo = null;
var branchId = null;
var cardTypeId = null;
var bfilBranchId = null;
var allocatedToId = null;
var allocatedDate = null;
var kitDamageReasonId = null;
var assignedDate = null;
var kitDate = null;
var isDestructionApproved = null;

var kitViewPage = null;
var currentSelectedTab = null;

$(document).ready(function () {

    $('#kitTab a').click(function (e) {
        e.preventDefault();
        showTab(this);
    });

    $("#staffAssignedDatatable, #staffAllocatedDatatable").DataTable({
        "processing": false,
        "serverSide": false,
        "filter": false,
        "bLengthChange": false,
        "bPaginate": false,
        "bInfo": false,
    });

    $("#kitDetailDatatable").DataTable({
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
                "data": "kitStatus", "name": "kitStatus",
                render: function (row, type, data) {
                    return getKitStatusColor(data.kitStatusId, data.kitStatus);
                }
            },
            { "data": "indentNo", "name": "indentNo", "autoWidth": true },
            {
                "data": "accountNo", "name": "accountNo",
                render: function (row, type, data) {
                    return `<a href='${kitBaseUrl}/KitDetail/${data.kitId}'>${data.accountNo}</a>`;
                }
            },
            { "data": "cifNo", "name": "cifNo", "autoWidth": true },
            { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
            { "data": "iblBranchName", "name": "iblBranchName", "autoWidth": true },
            { "data": "bfilBranchCode", "name": "bfilBranchCode", "autoWidth": true },
            { "data": "cardType", "name": "cardType", "autoWidth": true },
        ]
    });

    $("#kitForAllocationDatatable").DataTable({
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
        "aaSorting": [[1, 'asc']],
        "columns": [
            {
                render: function (row, type, data) {
                    return `<input type="checkbox" name="kitForAllocation"  value="${data.kitId}" />`;
                }
            },
            { "data": "indentNo", "name": "indentNo", "autoWidth": true },
            {
                "data": "accountNo", "name": "accountNo",
                render: function (row, type, data) {
                    return `<a href='${kitBaseUrl}/KitAllocation/${data.kitId}'>${data.accountNo}</a>`;
                }
            },
            { "data": "cifNo", "name": "cifNo", "autoWidth": true },
            { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
            { "data": "iblBranchName", "name": "iblBranchName", "autoWidth": true },
            { "data": "bfilBranchCode", "name": "bfilBranchCode", "autoWidth": true },
            { "data": "cardType", "name": "cardType", "autoWidth": true },
        ]
    });

    $("#kitAllocatedDatatable").DataTable({
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
        "aaSorting": [[1, 'asc']],
        "columns": [
            {
                render: function (row, type, data) {
                    if (data.kitStatusId == 4) { // Assigned Kit Status
                        return '';
                    }
                    return `<input type="checkbox" name="kitForAllocation"  value="${data.kitId}" />`;
                }
            },
            {
                "data": "kitStatus", "name": "kitStatus",
                render: function (row, type, data) {
                    return getKitStatusColor(data.kitStatusId, data.kitStatus);
                }
            },
            { "data": "indentNo", "name": "indentNo", "autoWidth": true },
            {
                "data": "accountNo", "name": "accountNo",
                render: function (row, type, data) {
                    return `<a href='${kitBaseUrl}/KitAllocated/${data.kitId}'>${data.accountNo}</a>`;
                }
            },
            { "data": "cifNo", "name": "cifNo", "autoWidth": true },
            { "data": "staffId", "name": "staffId", "autoWidth": true },
            { "data": "staffName", "name": "staffName", "autoWidth": true },
            {
                "data": "allocatedDate", "name": "allocatedDate",
                render: function (row, type, data) {
                    return getFormatDate(data.allocatedDate);
                }
            },
            {
                "data": "age", "name": "age",
                render: function (row, type, data) {
                    return data.ageDetail;
                }
            },
        ]
    });

    $("#kitAllocatedStaffWiseDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": false,
        "deferLoading": 0,
        "scrollX": true,
        "scrollY": '50vh',
        "scrollCollapse": true,
        "ajax": {
            "url": `${kitBaseUrl}/GetKitStaff`,
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
                "data": "staffId", "name": "staffId",
                render: function (row, type, data) {
                    return `<a href='${kitBaseUrl}/KitDetailStaffWise/${data.userId}'>${data.staffId}</a>`;
                }
            },
            { "data": "staffName", "name": "staffName", "autoWidth": true },
            { "data": "kitAllocated", "name": "kitAllocated", "autoWidth": true },
            { "data": "kitAssigned", "name": "kitAssigned", "autoWidth": true },
            { "data": "kitRemaining", "name": "kitRemaining", "autoWidth": true },
            {
                render: function (row, type, data) {
                    return `<a href='${kitBaseUrl}/KitDetailStaffWise/${data.userId}'>View Detail</a>`;
                }
            }
        ]
    });

    $("#kitDestructionDatatable").DataTable({
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
        "aaSorting": [[3, 'desc']],
        "columns": [
            { "data": "indentNo", "name": "indentNo", "autoWidth": true },
            {
                "data": "accountNo", "name": "accountNo",
                render: function (row, type, data) {
                    return `<a href='${kitBaseUrl}/Destruction/${data.kitId}'>${data.accountNo}</a>`;
                }
            },
            { "data": "cifNo", "name": "cifNo", "autoWidth": true },
            {
                "data": "kitDate", "name": "kitDate",
                render: function (row, type, data) {
                    return getFormatDate(data.kitDate);
                }
            },
            { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
            { "data": "iblBranchName", "name": "iblBranchName", "autoWidth": true },
            { "data": "bfilBranchCode", "name": "bfilBranchCode", "autoWidth": true },
            { "data": "cardType", "name": "cardType", "autoWidth": true },
            { "data": "kitDamageReason", "name": "kitDamageReason", "autoWidth": true },
            {
                "data": "isDestructionApproved", "name": "isDestructionApproved",
                render: function (row, type, data) {
                    return data.isDestructionApproved ? "Yes" : "No";
                }
            },
        ]
    });

    $("#kitDestructionHoDatatable, #kitDestructionApprovedDatatable, #kitDestructionCpuDatatable").DataTable({
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
        "aaSorting": [[0, 'desc']],
        "columns": [
            {
                "data": "kitDate", "name": "kitDate",
                render: function (row, type, data) {
                    return getFormatDate(data.kitDate);
                }
            },
            {
                "data": "accountNo", "name": "accountNo",
                render: function (row, type, data) {
                    return `<a href='${kitBaseUrl}/${kitViewPage}/${data.kitId}'>${data.accountNo}</a>`;
                }
            },
            { "data": "cifNo", "name": "cifNo", "autoWidth": true },
            { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
            { "data": "kitDamageReason", "name": "kitDamageReason", "autoWidth": true },
            {
                render: function (row, type, data) {
                    return `<a href='${kitBaseUrl}/${kitViewPage}/${data.kitId}'>View Detail</a>`;
                }
            },
        ]
    });

    $("#kitDestructionRejectedDatatable").DataTable({
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
        "aaSorting": [[0, 'desc']],
        "columns": [
            {
                "data": "kitDate", "name": "kitDate",
                render: function (row, type, data) {
                    return getFormatDate(data.kitDate);
                }
            },
            {
                "data": "accountNo", "name": "accountNo",
                render: function (row, type, data) {
                    return `<a href='${kitBaseUrl}/Rejected/${data.kitId}'>${data.accountNo}</a>`;
                }
            },
            { "data": "cifNo", "name": "cifNo", "autoWidth": true },
            { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
            { "data": "kitDamageReason", "name": "kitDamageReason", "autoWidth": true },
            { "data": "damageRemarks", "name": "damageRemarks", "autoWidth": true },
        ]
    });

    $("#kitDetailStaffDatatable").DataTable({
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
                "data": "kitStatus", "name": "kitStatus",
                render: function (row, type, data) {
                    return getKitStatusColor(data.kitStatusId, data.kitStatus);
                }
            },
            {
                "data": "accountNo", "name": "accountNo",
                render: function (row, type, data) {
                    return `<a href='${kitBaseUrl}/KitDetailStaff/${data.kitId}'>${data.accountNo}</a>`;
                }
            },
            { "data": "cifNo", "name": "cifNo", "autoWidth": true },
            {
                "data": "kitDate", "name": "kitDate",
                render: function (row, type, data) {
                    return getFormatDate(data.kitDate);
                }
            },
            {
                "data": "age", "name": "age",
                render: function (row, type, data) {
                    return data.ageDetail;
                }
            },
            { "data": "staffName", "name": "staffName", "autoWidth": true },
            {
                render: function (row, type, data) {
                    return `<a href='${kitBaseUrl}/KitDetailStaff/${data.kitId}'>View Detail</a>`;
                }
            },
        ]
    });

    $("#kitAssignedDatatable").DataTable({
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
                "data": "accountNo", "name": "accountNo",
                render: function (row, type, data) {
                    return `<a href='${kitBaseUrl}/KitDetailAssigned/${data.kitId}'>${data.accountNo}</a>`;
                }
            },
            { "data": "cifNo", "name": "cifNo", "autoWidth": true },
            { "data": "customerName", "name": "customerName", "autoWidth": true },
            {
                "data": "kitDate", "name": "kitDate",
                render: function (row, type, data) {
                    return getFormatDate(data.kitDate);
                }
            },
            { "data": "cardType", "name": "cardType", "autoWidth": true },
            {
                render: function (row, type, data) {
                    return `<a href='${kitBaseUrl}/KitDetailAssigned/${data.kitId}'>View Detail</a>`;
                }
            },
        ]
    });

    $('#btnAllocateKits').click(function (e) {
        var ids = [];
        $("input:checkbox[name=kitForAllocation]:checked").each(function () {
            ids.push($(this).val());
        });
        if (ids.length === 0) {
            displayToastr('Please select Kit(s)', "Kit Re Allocate", "Error")
            return;
        }
        setAllocateIds(ids.join(","));
    });

    $('#btnReAllocate').click(function (e) {
        var ids = [];
        $("input:checkbox[name=kitForAllocation]:checked").each(function () {
            ids.push($(this).val());
        });
        if (ids.length === 0) {
            displayToastr('Please select Kit(s)', "Kit Allocate", "Error")
            return;
        }
        setAllocateIds(ids.join(","));
    });

    $('#btnReturn').click(function (e) {
        var ids = [];
        $("input:checkbox[name=kitForAllocation]:checked").each(function () {
            ids.push($(this).val());
        });
        if (ids.length === 0) {
            displayToastr('Please select Kit(s)', "Kit Return", "Error")
            return;
        }
        setReturnIds(ids.join(","));
    });

    $('#btnDestruct').click(function (e) {
        var ids = [];
        $("input:checkbox[name=kitForAllocation]:checked").each(function () {
            ids.push($(this).val());
        });
        if (ids.length === 0) {
            displayToastr('Please select Kit(s)', "Kit Destruct", "Error")
            return;
        }
        setDestructIds(ids.join(","));
    });

    $('#btnAllocateKit').click(function (e) {
        var kitId = $('#KitId').val();
        setAllocateIds(kitId);
    });

    $('#btnKitAllocation').click(function (e) {
        if ($("#formKitAllocate").valid()) {
            $('#staff-password').val('');
            $('#staff-otp').val('');
            $('#sentSmsId').val('');
            enableOtp("Generate OTP");
            displayModel('kitAuthModal');
        }
    });

    $('#btnKitReturn').click(function (e) {
        $('#staff-password').val('');
        $('#staff-otp').val('');
        $('#sentSmsId').val('');
        $('#staff-mobile').val('');
        displayModel('kitAuthModal');
    });

    $('#btnSendOtp').click(function (e) {
        var isAllocate = $('#isAllocate').val();
        var userId = $('#StaffId').val();
        var mobile = $('#staff-mobile').val();
        if (mobile == '') {
            displayToastr('Please enter mobile no!', "OTP Generate", "Error");
            return;
        }
        generateOtp(userId, isAllocate);
    });

    $('#btnValidateAuth').click(function (e) {
        var staffId = $('#SelectedStaffId').val();
        var authType = $('input[name="auth-type"]:checked').val();
        var formType = $('#isAllocate').val() === 'true' ? 'Allocate' : 'Return';
        if (authType === 'credential') {
            var password = $('#staff-password').val();
            if (password === '') {
                displayToastr('Please enter Password!', `Kit ${formType}`, "Error")
                return;
            }
            showLoading();
            $.ajax({
                url: `${kitBaseUrl}/ValidateUser`,
                type: 'POST',
                data: { staffId, password },
                success: function (result) {
                    if (result.success) {
                        $(`#formKit${formType}`).get(0).submit();
                    } else {
                        hideLoading();
                        displayToastr(result.message, `Kit ${formType}`, "Error")
                    }
                },
                error: function (error) {
                    hideLoading();
                    displayToastr(error.data, `Kit ${formType}`, "Error")
                },
            });

        }
        if (authType === 'otp') {
            var staffOtp = $('#staff-otp').val();
            if (staffOtp === '') {
                displayToastr('Please enter OTP!', `Kit ${formType}`, "Error")
                return;
            }
            showLoading();
            $.ajax({
                url: `${kitBaseUrl}/ValidateOTP`,
                type: 'POST',
                data: { sentSmsId: $('#sentSmsId').val(), otp: staffOtp },
                success: function (result) {
                    if (result.success) {
                        $(`#formKit${formType}`).get(0).submit();
                    } else {
                        hideLoading();
                        displayToastr(result.message, `Kit ${formType}`, "Error")
                    }
                },
                error: function (error) {
                    hideLoading();
                    displayToastr(error.data, `Kit ${formType}`, "Error")
                },
            });
        }
    });

    $('#btnApproveDestruction').click(function (e) {
        kitDestruction($('#KitId').val(), $('#ApproveRemarks').val(), true);
    });

    $('#btnRejectDestruction').click(function (e) {
        kitDestruction($('#KitId').val(), $('#RejectRemarks').val(), false);
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

function getFilterParams(d) {

    indentNo = $('#indentNo').val();
    accountNo = $('#accountNo').val();
    cifNo = $('#cifNo').val();
    branchId = $('#branchId').val();
    cardTypeId = $('#cardTypeId').val();
    bfilBranchId = $('#bfilBranchId').val();
    allocatedToId = $('#allocatedToId').val();
    allocatedDate = $('#allocatedDate').val();
    kitDamageReasonId = $('#kitDamageReasonId').val();
    assignedDate = $('#assignedDate').val();
    kitDate = $('#kitDate').val();

    d.kitStatusIds = kitStatusIds;

    d.indentNo = indentNo;
    d.accountNo = accountNo;
    d.cifNo = cifNo;
    d.branchId = branchId;
    d.cardTypeId = cardTypeId;
    d.bfilBranchId = bfilBranchId;
    d.allocatedToId = allocatedToId;
    d.allocatedDate = allocatedDate;
    d.kitDamageReasonId = kitDamageReasonId;
    d.assignedDate = assignedDate;
    d.kitDate = kitDate;

    d.isDestructionApproved = isDestructionApproved;
}

function showTab(tab) {
    currentSelectedTab = tab;
    clearFilters();

    $(tab).tab('show');
    var kitTab = $(tab).attr("id");
    kitStatusIds = $(tab).attr("statusId");
    isDestructionApproved = $(tab).attr("destruction-approved");
    if (isDestructionApproved === undefined || isDestructionApproved === '') {
        isDestructionApproved = null;
    }
    if (!kitLoaded.includes(kitTab)) {
        kitLoaded.push(kitTab);
        kitViewPage = $(tab).attr("viewPage");
        $(`${$(tab).attr("href")}Datatable`).DataTable().ajax.reload();
    }

    resizeDataTable();
}

function setAllocateIds(ids) {
    $.ajax({
        url: `${kitBaseUrl}/SetAllocateIds`,
        type: 'POST',
        data: { kitIds: ids },
        success: function (result) {
            window.location.href = `${kitBaseUrl}/KitAllocate`;
        },
        error: function (error) {
        },
    });
}

function setDestructIds(ids) {
    $.ajax({
        url: `${kitBaseUrl}/SetDestructIds`,
        type: 'POST',
        data: { kitIds: ids },
        success: function (result) {
            window.location.href = `${kitBaseUrl}/KitDestruct`;
        },
        error: function (error) {
            displayToastr(error.data, "Kit Destruct", "Error");
        },
    });
}

function setReturnIds(ids) {
    $.ajax({
        url: `${kitBaseUrl}/SetReturnIds`,
        type: 'POST',
        data: { kitIds: ids },
        success: function (result) {
            if (result) {
                window.location.href = `${kitBaseUrl}/KitReturn`;
            } else {
                displayToastr("Please select one staff Kits", "Kit Return", "Error");
            }
        },
        error: function (error) {
            displayToastr(error.data, "Kit Return", "Error");
        },
    });
}

function GetStaffDetail(obj) {
    var userId = $(obj).find("option:selected").val();
    if (userId == '') {
        displayToastr('Please select Staff!', "Kit Allocate", "Error")
        return;
    }
    $.ajax({
        url: `${kitBaseUrl}/GetUserDetails`,
        type: 'POST',
        data: { userId },
        success: function (result) {
            if (result.success) {
                $('#SelectedStaffId').val(result.data.staffId);
                $('#StaffName').val(result.data.fullName);
                $('#staff-mobile').attr('value', result.data.mobileNo);
            }
        },
        error: function (error) {
            displayToastr(error.data, "User Detail", "Error")
        },
    });
}

function showAuthMode(showEle, hideEle) {
    $(`.${hideEle}`).hide();
    $(`.${showEle}`).show();
}

function generateOtp(userId, isAllocate) {
    $.ajax({
        url: `${kitBaseUrl}/GenerateOTP`,
        type: 'POST',
        data: { userId, mobile: $('#staff-mobile').val(), isAllocate },
        success: function (result) {
            if (result.success) {
                $('#sentSmsId').val(result.data.sentSmsId);
                disableOtp();
            } else {
                displayToastr(result.message, "OTP Generate", "Error");
            }
        },
        error: function (error) {
            displayToastr(error.data, "OTP Generate", "Error");
        },
    });
}

function disableOtp() {
    $("#btnSendOtp").prop('disabled', true);
    $("#progressBar").show();
    var totalTime = 30;
    var timeleft = 30;
    var downloadTimer = setInterval(function () {
        if (timeleft <= 0) {
            clearInterval(downloadTimer);
            enableOtp("Didn't Receive the OTP? Resend OTP.");
        }
        $("#progressBar").attr('aria-valuenow', (totalTime - timeleft));
        $("#progressBar").css("width", `${(totalTime - timeleft) * (100 / totalTime)}%`);
        timeleft -= 1;
    }, 1000);
}

function enableOtp(btnText) {
    $("#progressBar").hide();
    $("#progressBar").css("width", `${0}%`);
    $("#progressBar").val(0);
    $("#btnSendOtp").html(btnText);
    $("#btnSendOtp").prop('disabled', false);
}

function kitDestruction(kitId, destructionRemarks, destructionApproved) {
    $.ajax({
        url: `${kitBaseUrl}/KitDestuctionApproval`,
        type: 'POST',
        data: { kitId, destructionRemarks, destructionApproved },
        success: function (result) {
            if (result.success) {
                window.location.href = `${kitBaseUrl}#kitDestructionHo`;
            } else {
                displayToastr(result.message, "Kit Destruction", "Error")
            }
        },
        error: function (error) {
            displayToastr(error.data, "Kit Destruction", "Error")
        },
    });
}

function manageFilters() {
    switch ($(currentSelectedTab).attr("id")) {
        case 'kitDestructionHoTab':
        case 'kitDestructionApprovedTab':
        case 'kitDestructionRejectedTab':
        case 'kitDestructionCpuTab':
            $('#kitDateFilter').show();
            $('#accountNoFilter').show();
            $('#cifNoFilter').show();
            $('#branchIdFilter').show();
            $('#kitDamageReasonIdFilter').show();
            break;
        case 'kitDetailTab':
        case 'kitForAllocationTab':
            $('#indentNoFilter').show();
            $('#accountNoFilter').show();
            $('#cifNoFilter').show();
            $('#branchIdFilter').show();
            $('#cardTypeIdFilter').show();
            $('#bfilBranchIdFilter').show();
            break;
        case 'kitAllocatedTab':
            $('#indentNoFilter').show();
            $('#accountNoFilter').show();
            $('#cifNoFilter').show();
            $('#allocatedToIdFilter').show();
            $('#allocatedDateFilter').show();
            break;
        case 'kitAllocatedStaffWiseTab':
            $('#allocatedToIdFilter').show();
            break;
        case 'kitDestructionTab':
            $('#indentNoFilter').show();
            $('#accountNoFilter').show();
            $('#cifNoFilter').show();
            $('#branchIdFilter').show();
            $('#cardTypeIdFilter').show();
            $('#bfilBranchIdFilter').show();
            $('#kitDamageReasonIdFilter').show();
            $('#kitDateFilter').show();
            break;
        case 'kitDetailStaffTab':
            $('#accountNoFilter').show();
            $('#cifNoFilter').show();
            $('#assignedDateFilter').show();
            $('#allocatedToIdFilter').show();
            break;
        case 'kitAssignedTab':
            $('#accountNoFilter').show();
            $('#cifNoFilter').show();
            $('#assignedDateFilter').show();
            $('#cardTypeIdFilter').show();
            break;
        default:
            break;
    }


}

function uploadAssignedKit() {
    $('#assignedKitUploadError').html('');
    $("#assignedKitFileUpload").val(null);
    displayModel('uploadAssignedKit');
}