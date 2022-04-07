var branchBaseUrl = "/Branch";

$(document).ready(function () {
    $("#branchDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": `${branchBaseUrl}/GetBranches`,
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        "columns": [
            {
                "data": "branchName", "name": "branchName",
                render: function (row, type, data) {
                    return `<a href='/Branch/Manage/${data.branchId}'>${data.branchName}</a>`;
                }
            },
            {
                "data": "branchCode", "name": "branchCode",
                render: function (row, type, data) {
                    return `<a href='/Branch/Manage/${data.branchId}'>${data.branchCode}</a>`;
                }
            },
            { "data": "branchType", "name": "BranchType", "autoWidth": true },
            { "data": "regionName", "name": "RegionName", "autoWidth": true },
            { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
            { "data": "iblBranchName", "name": "iblBranchName", "autoWidth": true },
            { "data": "ifscCode", "name": "ifscCode", "autoWidth": true },
        ]
    });

    $("#btnBranchUpload").click(function (e) {
        var files = $('#fileUpload').prop("files");
        if (files.length == 0) {
            displayToastr("Please select the file!", "Branch", "Error")
            return;
        }
        showLoading();
        formData = new FormData();
        formData.append("file", files[0]);
        $("#branchUploadError").html('');

        jQuery.ajax({
            type: 'POST',
            url: `${branchBaseUrl}/UploadBranches`,
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success) {
                    $('#branchDatatable').DataTable().ajax.reload();
                    displayToastr(response.message, "Branch", "Success")
                } else {
                    $("#branchUploadError").html(response.message);
                    displayToastr("Branch Upload Error", "Branch", "Error")
                }
                $('#fileUpload').val(null);
                hideLoading();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                displayToastr(textStatus, "Branch", "Error")
                $('#fileUpload').val(null);
                hideLoading();
            }
        });
    });
});

function GetC5Codes(obj) {
    var schemeCodeId = $(obj).find("option:selected").val();
    $.ajax({
        url: `${branchBaseUrl}/GetC5Codes`,
        type: 'POST',
        data: { 'schemeCodeId': schemeCodeId },
        success: function (result) {
            if (result) {
                $('#C5CodeId').html('');
                $('#CardTypeId').html('');
                if (result.c5Codes.length != 1) {
                    $('#C5CodeId').append($("<option></option>").val
                        ('').html('Select C5 Code'));
                    $('#CardTypeId').append($("<option></option>").val
                        ('').html('Select Card Type'));
                } else {
                    $.each(result.cardType, function (key, value) {
                        $('#CardTypeId').append($("<option></option>").val
                            (value.id).html(value.text));
                    });
                }
                $.each(result.c5Codes, function (key, value) {
                    $('#C5CodeId').append($("<option></option>").val
                        (value.id).html(value.text));
                });
            }
        },
        error: function (error) {
            displayToastr(error.data, "C5 Codes Detail", "Error")
        },
    });
}

function GetCardType(obj) {
    var c5CodeId = $(obj).find("option:selected").val();
    $.ajax({
        url: `${branchBaseUrl}/GetCardType`,
        type: 'POST',
        data: { 'c5CodeId': c5CodeId },
        success: function (result) {
            if (result) {
                $('#CardTypeId').html('');
                $.each(result, function (key, value) {
                    $('#CardTypeId').append($("<option></option>").val
                        (value.id).html(value.text));
                });
            }
        },
        error: function (error) {
            displayToastr(error.data, "Card Type Detail", "Error")
        },
    });
}