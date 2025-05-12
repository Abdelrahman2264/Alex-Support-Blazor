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
function ShowCloseTicketFormModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('CloseTicket')).show();
}

function HideCloseTicketFormModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('CloseTicket')).hide();
}function ShowAddSolutionFormModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('AddSolution')).show();
}

function HideAddSolutionFormModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('AddSolution')).hide();
}function ShowAddTaskFormModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('AddTask')).show();
}

function HideAddTaskFormModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('AddTask')).hide();
}