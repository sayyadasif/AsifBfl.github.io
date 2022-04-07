var indentBaseUrl = "/Indent";
var homeBaseUrl = "/Home";
var notificationBaseUrl = "/Notification";
var notificationCookie = "notification-generate";

var indentStartDate = null;
var indentEndDate = null;

$(document).ready(function () {

    if ($('#notication-details').length) {
        if ($('#notication-details').length) {
            var cookieExists = readCookie(notificationCookie);
            if (cookieExists == null || cookieExists == undefined) {
                createCookie(notificationCookie, new Date(), 1);
                generateNotifactions();
            }
        }
    }

    updateMonthFilter($('#month-filter'));
});

function generateNotifactions() {
    $.ajax({
        url: `${notificationBaseUrl}/GenerateNotifactions`,
        type: 'POST',
        success: function (result) {
        },
        error: function (error) {
        },
    });
}

function LoadChart(url, canvas, monthId) {
    $.ajax({
        type: "POST",
        url: `${homeBaseUrl}/${url}`,
        data: { monthId },
        success: function (response) {
            var labels = [];
            var data = [];
            var colors = [];
            $.each(response, function (index, indent) {
                labels.push(indent.statusName);
                data.push(indent.count);
                colors.push(indent.color);
            });
            new Chart($(`#${canvas}`), {
                type: 'pie',
                responsive: true,
                options: {
                    plugins: {
                        legend: {
                            display: true,
                            position: $(`#${canvas}`).attr("legend-position"),
                        }
                    }
                },
                data: {
                    labels: labels,
                    datasets: [{
                        data: data,
                        backgroundColor: colors,
                        hoverOffset: 4,
                        borderWidth: 1
                    }]
                }
            });
        },
        failure: function (response) {
        }
    });
}

function updateMonthFilter(obj) {
    var monthId = $(obj).find("option:selected").val();
    getDashboard(monthId);

}

function getDashboard(monthId) {
    showLoading();
    $.ajax({
        url: `${homeBaseUrl}/GetDashboard`,
        type: 'POST',
        data: { monthId },
        success: function (result) {
            $("#dashboard-details").html(result);

            if ($('#rejectedDatatable').length || $('#dispatchedDatatable').length) {
                getDateRange(monthId);
            }

            if ($('#chartIndent').val()) {
                LoadChart('GetIndentStatusOverview', 'pieIndentChart', monthId);
            }

            if ($('#chartKit').val()) {
                LoadChart('GetKitStatusOverview', 'pieKitChart', monthId);
            }

            hideLoading();
        },
        error: function (error) {
            hideLoading();
        },
    });
}

function getDateRange(monthId) {
    $.ajax({
        url: `${homeBaseUrl}/GetDateRange`,
        type: 'POST',
        data: { monthId },
        success: function (result) {
            indentStartDate = result.item1;
            indentEndDate = result.item2;

            if ($('#rejectedDatatable').length) {
                bindRejectedTable();
            }

            if ($('#dispatchedDatatable').length) {
                bindDispatchedTable();
            }
        },
        error: function (error) {
            hideLoading();
        },
    });

}

function bindRejectedTable() {
    $("#rejectedDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": false,
        "bLengthChange": false,
        "bPaginate": true,
        "bInfo": false,
        "fnDrawCallback": function (oSettings) {
            $('.dataTables_paginate').hide();
        },
        "ajax": {
            "url": `${indentBaseUrl}/GetIndents`,
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.indentStatusIds = "3";  // Rejected
                d.indentStartDate = indentStartDate;
                d.indentEndDate = indentEndDate;
            }
        },
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        "columns": [
            { "data": "indentNo", "name": "indentNo", "autoWidth": true },
            {
                "data": "indentDate", "name": "indentDate",
                render: function (row, type, data) {
                    return moment(data.indentDate).format('DD-MM-YYYY');
                }
            },
            { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
            { "data": "bfilBranchCode", "name": "bfilBranchCode", "autoWidth": true },
            { "data": "rejectedReason", "name": "rejectedReason", "autoWidth": true }
        ]
    });
}

function bindDispatchedTable() {
    $("#dispatchedDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": false,
        "bLengthChange": false,
        "bPaginate": true,
        "bInfo": false,
        "fnDrawCallback": function (oSettings) {
            $('.dataTables_paginate').hide();
        },
        "ajax": {
            "url": `${indentBaseUrl}/GetIndents`,
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.indentStatusIds = "5,6";  // Dispatched Status
                d.indentStartDate = indentStartDate;
                d.indentEndDate = indentEndDate;
            }
        },
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        "columns": [
            { "data": "indentNo", "name": "indentNo", "autoWidth": true },
            {
                "data": "indentDate", "name": "indentDate",
                render: function (row, type, data) {
                    return moment(data.indentDate).format('DD-MM-YYYY');
                }
            },
            { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
            { "data": "bfilBranchCode", "name": "bfilBranchCode", "autoWidth": true },
            { "data": "indentStatus", "name": "indentStatus", "autoWidth": true }
        ]
    });
}