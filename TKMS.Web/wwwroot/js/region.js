var regionBaseUrl = "/Region";


$(document).ready(function () {

    $("#regionDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": `${regionBaseUrl}/GetRegions`,
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        "columns": [
            {
                "data": "systemRoId", "name": "systemRoId",
                render: function (row, type, data) {
                    return `<a href='/Region/Manage/${data.regionId}'>${data.systemRoId}</a>`;
                }
            },
            {
                "data": "regionName", "name": "regionName",
                render: function (row, type, data) {
                    return `<a href='/Region/Manage/${data.regionId}'>${data.regionName}</a>`;
                }
            },
            { "data": "region", "name": "region", "autoWidth": true },
            { "data": "zone", "name": "zone", "autoWidth": true },
            { "data": "district", "name": "district", "autoWidth": true },
            { "data": "state", "name": "state", "autoWidth": true },
            { "data": "pinCode", "name": "pinCode", "autoWidth": true },
        ]
    });

    $("#btnUpload").click(function (e) {
        var files = $('#fileUpload').prop("files");
        if (files.length == 0) {
            displayToastr("Please select the file!", "Region", "Error")
            return;
        }
        showLoading();
        formData = new FormData();
        formData.append("file", files[0]);
        $('#regionUploadError').html('');

        jQuery.ajax({
            type: 'POST',
            url: `${regionBaseUrl}/UploadRegions`,
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success) {
                    $('#regionDatatable').DataTable().ajax.reload();
                    displayToastr(response.message, "Region", "Success")
                } else {
                    $("#regionUploadError").html(response.message);
                    displayToastr("Regions Upload Error", "Region", "Error")
                }
                $('#fileUpload').val(null);
                hideLoading();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                displayToastr(textStatus, "Region", "Error")
                $('#fileUpload').val(null);
                hideLoading();
            }
        });
    });
});