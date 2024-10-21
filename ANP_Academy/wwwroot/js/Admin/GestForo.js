//Funcion para conservar publicacion
    $(document).ready(function () {
        
        $('.conservar-btn').on('click', function () {
            var publicacionId = $(this).data('id'); // Obtener el id de la publicación

            // Mostrar popup de confirmación con Bootbox
            bootbox.confirm({
                message: "¿Estás seguro de que deseas conservar esta publicación?",
                buttons: {
                    confirm: {
                        label: 'Sí',
                        className: 'btn-success'
                    },
                    cancel: {
                        label: 'No',
                        className: 'btn-danger'
                    }
                },
                callback: function (result) {
                    if (result) {
                        // Si el usuario confirma, realiza una llamada AJAX al controlador
                        $.ajax({
                            url: '/Admin/ConservarPublicacion', 
                            type: 'POST',
                            data: { id: publicacionId },
                            success: function (response) {
                                
                                toastr.success('La publicación ha sido conservada exitosamente.');
                                //Recargar la pagina luego de que se haya realizado el Post
                                setTimeout(function () {
                                    location.reload();  
                                }, 1500);
                            },
                            error: function (error) {
                                
                                toastr.error('Ocurrió un error al conservar la publicación.');
                            }
                        });
                    }
                }
            });
        });
    });

    //Funcion para eliminar publicacion desde la gestion del foro
function eliminarPublicacion(reportId, publiId) {
    bootbox.confirm({
        title: "Confirmar acción",
        message: "¿Estás seguro de que quieres eliminar esta publicación?",
        buttons: {
            confirm: {
                label: 'Sí',
                className: 'btn-danger'
            },
            cancel: {
                label: 'No',
                className: 'btn-secondary'
            }
        },
        callback: function (result) {
            if (result) {
                
                $.ajax({
                    url: '/Admin/EliminarPublicacion', 
                    type: 'POST',
                    data: { reportId: reportId, publiId: publiId },
                    success: function (response) {
                        toastr.success('La publicación ha sido eliminada exitosamente.');

                        // Recargar la página después de que el POST se complete
                        setTimeout(function () {
                            location.reload();  
                        }, 1500);  
                    },
                    error: function (error) {
                        toastr.error('Ocurrió un error al eliminar la publicación.');
                    }
                });
            }
        }
    });
}

