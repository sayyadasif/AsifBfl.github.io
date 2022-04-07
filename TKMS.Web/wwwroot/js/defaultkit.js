var defaultKitBaseUrl = "/DefaultKit";

$(document).ready(function () {
    $("#btnDefaultKitUpload").click(function (e) {
        var files = $('#defaultKitFileUpload').prop("files");
        if (files.length == 0) {
            displayToastr("Please select the file!", "Default Kits Upload", "Error")
            return;
        }
        showLoading();
        formData = new FormData();
        formData.append("file", files[0]);
        $('#defaultKitUploadError').html('');

        jQuery.ajax({
            type: 'POST',
            url: `${defaultKitBaseUrl}/UploadDefaultKits`,
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success) {
                    displayToastr(response.message, "Default Kits Upload", "Success")
                } else {
                    $('#defaultKitUploadError').html(response.message);
                    displayToastr("Default Kits Uplaod Error", "Default Kits Upload", "Error")
                }
                hideLoading();
                $('#defaultKitFileUpload').val(null);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $('#defaultKitFileUpload').val(null);
                displayToastr(textStatus, "Default Kits Upload", "Error")
                hideLoading();
            }
        });
    });
});