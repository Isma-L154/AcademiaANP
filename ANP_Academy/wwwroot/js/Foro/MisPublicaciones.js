
//This function is for a validation of the user, so they can confirm if they want to delete the post
$(document).ready(function () {
    $('#deleteForm').on('submit', function (e) {
        e.preventDefault();
        bootbox.confirm({
            message: "Seguro que quieres eliminar esta publicacion?",
            buttons: {
                confirm: {
                    label: 'Si',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result) {
                    e.target.submit();
                    toastr.success('Eliminado correctamente');
                } else {
                    toastr.error('Ocurrio un error al eliminar la publicacion');
                }
            }
        });
    });
});
