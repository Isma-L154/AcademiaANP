$(document).ready(function () {
    $('#deleteBtn').click(function () {
        var id = $('#IdPublicacion').val(); 
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
                    $('#IdPublicacion').val(id);
                    $('#FormDelete').submit();
                    toastr.success('Eliminado correctamente');
                } else {
                    toastr.error('Ocurrio un error al eliminar la publicacion');
                   function e (xhr, status, error) {
                        console.log(xhr.responseText);
                    }
                }
            }
        });
    });
});
