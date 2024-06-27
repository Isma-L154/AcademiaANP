function showModal(suscripcionId) {
    $('#myModal').modal('show');
}

function closeModal() {
    $('#myModal').modal('hide');
}

function confirmDelete(id) {
    $('#deleteSuscripcionId').val(id);
    $('#confirmDeleteModal').modal('show');
}

$(document).ready(function () {
    $('#confirmDeleteModal .btn-secondary').on('click', function () {
        $('#confirmDeleteModal').modal('hide');
    });
});