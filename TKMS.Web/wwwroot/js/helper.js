function getIndentStatusColor(indentStatusId, indentStatus) {
    if (indentStatusId === 1) // Pending Approval
    {
        return `<span class="badge bg-primary">${indentStatus}</span>`;
    } else if (indentStatusId === 2) // Approved
    {
        return `<span class="badge bg-secondary">${indentStatus}</span>`;
    } else if (indentStatusId === 3) // Rejected
    {
        return `<span class="badge bg-danger">${indentStatus}</span>`;
    } else if (indentStatusId === 4) // Indent For Dispatch
    {
        return `<span class="badge bg-light partialDispatch">${indentStatus}</span>`;
    } else if (indentStatusId === 5) // Partial Dispatched
    {
        return `<span class="badge bg-light dispatched">${indentStatus}</span>`;
    } else if (indentStatusId === 6) // Dispatched
    {
        return `<span class="badge bg-light">${indentStatus}</span>`;
    } else if (indentStatusId === 7) // Received At Ro
    {
        return `<span class="badge bg-warning text-dark">${indentStatus}</span>`;
    } else if (indentStatusId === 8) // Dispatch To Branch
    {
        return `<span class="badge bg-info text-dark">${indentStatus}</span>`;
    } else if (indentStatusId === 9) // Indent Box Received
    {
        return `<span class="badge bg-success">${indentStatus}</span>`;
    } else if (indentStatusId === 10) // Cancelled
    {
        return `<span class="badge bg-dark">${indentStatus}</span>`;
    }
    return `<span class="badge bg-secondary">${indentStatus}</span>`;
}

function getKitStatusColor(kitStatusId, kitStatus) {
    if (kitStatusId === 1) // Dispatched
    {
        return `<span class="badge bg-primary">${kitStatus}</span>`;
    } else if (kitStatusId === 2) // Received
    {
        return `<span class="badge bg-secondary">${kitStatus}</span>`;
    } else if (kitStatusId === 3) // Allocated
    {
        return `<span class="badge bg-danger">${kitStatus}</span>`;
    } else if (kitStatusId === 4) // Assigned
    {
        return `<span class="badge bg-light">${kitStatus}</span>`;
    } else if (kitStatusId === 5) // Damaged
    {
        return `<span class="badge bg-light">${kitStatus}</span>`;
    } else if (kitStatusId === 6) // Destruction
    {
        return `<span class="badge bg-light">${kitStatus}</span>`;
    }

    return `<span class="badge bg-secondary">${kitStatus}</span>`;
}

function getFormatDate(date) {
    if (!date || date == '') return '';
    return moment(date).format('DD-MM-YYYY');
}

function getFormatDateTime(date) {
    if (!date || date == '') return '';
    return moment(date).format('DD-MM-YYYY hh:mm A');
}

function resizeDataTable() {
    setTimeout(function () {
        $($.fn.dataTable.tables(true)).DataTable().columns.adjust().draw();
    }, 200);
}