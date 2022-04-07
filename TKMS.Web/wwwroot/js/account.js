$(document).ready(function () {
    $('#StaffId').focus();
});

function validateLogin() {
    if ($("#formLogin").valid()) {
        showLoading();
        return true;
    }
    return false;
}