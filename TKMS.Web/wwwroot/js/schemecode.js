var schemeCodeBaseUrl = "/SchemeCode";

$(document).ready(function () {

    $("#schemeCodeDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": `${schemeCodeBaseUrl}/GetSchemeCodes`,
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        "columns": [
            {
                "data": "schemeCodeName", "name": "schemeCodeName",
                render: function (row, type, data) {
                    return `<a href='/SchemeCode/Manage/${data.schemeCodeId}'>${data.schemeCodeName}</a>`;
                }
            },
            {
                data: 'schemeC5Codes',
                render: function (data) {
                    var c5Codes = `<ul>`
                    $.each(data, function (index, value) {
                        c5Codes = `${c5Codes}<li>${value}</li>`
                    });
                    return `${c5Codes}</ul>`;
                },
            },
        ]
    });
});

function validateSchemeCode() {
    var ids = [];
    $("input:checkbox[name=SelectedC5Codes]:checked").each(function () {
        ids.push($(this).val());
    });
    if (ids.length === 0) {
        displayToastr('Please select C5 Code(s)', "Scheme Code C5 Code", "Error")
        return false;
    }

    if ($("#formSchemeCode").valid()) {
        showLoading();
        return true;
    }
    return false;
}
