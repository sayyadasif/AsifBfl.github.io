var iblBranchBaseUrl = "/IblBranch";


$(document).ready(function () {

    $("#iblBranchDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": `${iblBranchBaseUrl}/GetIblBranches`,
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        "columns": [
            {
                "data": "iblBranchName", "name": "iblBranchName",
                render: function (row, type, data) {
                    return `<a href='/IblBranch/Manage/${data.iblBranchId}'>${data.iblBranchName}</a>`;
                }
            },
            { "data": "iblBranchCode", "name": "iblBranchCode", "autoWidth": true },
            {
                data: 'branches',
                render: function (data) {
                    var branches = `<ul>`
                    $.each(data, function (index, value) {
                        branches = `${branches}<li>${value}</li>`
                    });
                    return `${branches}</ul>`;
                },
            },
        ]
    });

    $("#btnUpload").click(function (e) {
        var files = $('#fileUpload').prop("files");
        if (files.length == 0) {
            displayToastr("Please select the file!", "IBL Branch", "Error")
            return;
        }
        showLoading();
        formData = new FormData();
        formData.append("file", files[0]);
        $('#iblBranchUploadError').html('');

        jQuery.ajax({
            type: 'POST',
            url: `${iblBranchBaseUrl}/UploadIblBranches`,
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success) {
                    $('#iblBranchDatatable').DataTable().ajax.reload();
                    displayToastr(response.message, "IBL Branch", "Success")
                } else {
                    $("#iblBranchUploadError").html(response.message);
                    displayToastr("IblBranchs Upload Error", "IBL Branch", "Error")
                }
                $('#fileUpload').val(null);
                hideLoading();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                displayToastr(textStatus, "IBL Branch", "Error")
                $('#fileUpload').val(null);
                hideLoading();
            }
        });
    });

    $("#btnUploadMapping").click(function (e) {
        var files = $('#fileUploadMapping').prop("files");
        if (files.length == 0) {
            displayToastr("Please select the file!", "IBL Branch Mapping", "Error")
            return;
        }
        showLoading();
        formData = new FormData();
        formData.append("file", files[0]);
        $('#iblBranchUploadMappingError').html('');

        jQuery.ajax({
            type: 'POST',
            url: `${iblBranchBaseUrl}/UploadIblBranchMapping`,
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success) {
                    $('#iblBranchDatatable').DataTable().ajax.reload();
                    displayToastr(response.message, "IBL Branch Mapping", "Success")
                } else {
                    $("#iblBranchUploadMappingError").html(response.message);
                    displayToastr("IBL Branch Mapping Upload Error", "IBL Branch Mapping", "Error")
                }
                $('#fileUploadMapping').val(null);
                hideLoading();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                displayToastr(textStatus, "IBL Branch Mapping", "Error")
                $('#fileUploadMapping').val(null);
                hideLoading();
            }
        });
    });
});