var c5CodeBaseUrl = "/C5Code";


$(document).ready(function () {

    $("#c5CodeDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": `${c5CodeBaseUrl}/GetC5Codes`,
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        "columns": [
            {
                "data": "c5CodeName", "name": "c5CodeName",
                render: function (row, type, data) {
                    return `<a href='/C5Code/Manage/${data.c5CodeId}'>${data.c5CodeName}</a>`;
                }
            },
            { "data": "cardTypeName", "name": "cardTypeName", "autoWidth": true },
        ]
    });
});