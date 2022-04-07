var cardTypeBaseUrl = "/CardType";


$(document).ready(function () {

    $("#cardTypeDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": `${cardTypeBaseUrl}/GetCardTypes`,
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        "columns": [
            {
                "data": "cardTypeName", "name": "cardTypeName",
                render: function (row, type, data) {
                    return `<a href='/CardType/Manage/${data.cardTypeId}'>${data.cardTypeName}</a>`;
                }
            },
        ]
    });
});