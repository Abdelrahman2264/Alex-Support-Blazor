window.ShowToastr = function (type, message) {
    if (type === "success") {
        toastr.success(message);
    }
    if (type == "error") {
        toastr.error(message);
    }
}

function ShowConfirmationModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('bsConfirmationModal')).show();
}

function HideConfirmationModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('bsConfirmationModal')).hide();
}
function ShowAssignTicketFormModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('AssignTicket')).show();
}

function HideAssignTicketFormModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('AssignTicket')).hide();
}