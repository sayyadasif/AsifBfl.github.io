var userBaseUrl = "/User";

$(document).ready(function () {

    $("#userDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": `${userBaseUrl}/GetUsers`,
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        "columns": [
            {
                "data": "staffId", "name": "staffId",
                render: function (row, type, data) {
                    return `<a href='/User/Manage/${data.userId}'>${data.staffId}</a>`;
                }
            },
            {
                "data": "fullName", "name": "fullName",
                render: function (row, type, data) {
                    return `<a href='/User/Manage/${data.userId}'>${data.fullName}</a>`;
                }
            },
            { "data": "branchName", "name": "BranchName", "autoWidth": true },
            { "data": "mobileNo", "name": "mobileNo", "autoWidth": true },
            {
                data: 'roles',
                render: function (data) {
                    var roles = `<ul>`
                    $.each(data, function (index, value) {
                        roles = `${roles}<li>${value}</li>`
                    });
                    return `${roles}</ul>`;
                },
            },
            {
                data: 'isActive',
                render: function (data) {
                    return data ? "Yes" : "No";
                },
                "width": "10px",
                sClass: "text-center",
            }
        ]
    });

    $("#btnUpload").click(function (e) {
        var files = $('#fileUpload').prop("files");
        if (files.length == 0) {
            displayToastr("Please select the file!", "User", "Error")
            return;
        }
        showLoading();
        formData = new FormData();
        formData.append("file", files[0]);
        $('#userUploadError').html('');

        jQuery.ajax({
            type: 'POST',
            url: `${userBaseUrl}/UploadUsers`,
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success) {
                    $('#userDatatable').DataTable().ajax.reload();
                    $('#fileUpload').val('');
                    displayToastr(response.message, "User", "Success")
                } else {
                    $("#userUploadError").html(response.message);
                    displayToastr("User Upload Error", "User", "Error")
                }
                $('#fileUpload').val(null);
                hideLoading();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                displayToastr(textStatus, "User", "Error")
                $('#fileUpload').val(null);
                hideLoading();
            }
        });
    });

    viewPassword(eval($('#RoleTypeId').find("option:selected").val()));
});

function GetRoles(obj) {
    var roleTypeId = $(obj).find("option:selected").val();
    if (roleTypeId == '') {
        displayToastr('Please select Role Type!', "User", "Error")
        return;
    }

    viewPassword(eval(roleTypeId));

    $.ajax({
        url: `${userBaseUrl}/GetRoles`,
        type: 'POST',
        data: { roleTypeId },
        success: function (result) {
            if (result) {
                var items = '';
                $.each(result, function (index, role) {
                    items += `<label><input name="SelectedRoles" type="checkbox" value="${role.id}" checked />&nbsp;${role.text}</label><span>&nbsp;&nbsp;</span>`;
                });
                $("#userRoles").html(items);
            }
        },
        error: function (error) {
            displayToastr(error.data, "User", "Error")
        },
    });
}

function showPassword(visible) {
    if (visible) {
        $('.password-user').show()
        $('#Password').val('');
    } else {
        $('.password-user').hide()
        $('#Password').val($('#OldPassword').val(''));
    }
}

function validateUser() {
    if ($('#RoleTypeId').find("option:selected").val() != '') {
        var ids = [];
        $("input:checkbox[name=SelectedRoles]:checked").each(function () {
            ids.push($(this).val());
        });
        if (ids.length === 0) {
            displayToastr('Please select Role(s)', "User Role", "Error")
            return false;
        }
    }

    if ($("#formUser").valid()) {
        showLoading();
        return true;
    }
    return false;
}

function viewPassword(roleTypeId) {

    $('.password-detail').hide()
    $('.password-edit').hide()
    $('.password-user').hide()

    if (roleTypeId === 4 || roleTypeId === 6) {
        $('.password-detail').show()
        if (eval($('#UserId').val()) === 0) {
            $('.password-user').show()
        } else {
            $('.password-edit').show()
        }
    }
}