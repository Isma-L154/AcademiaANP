
//Metodo para poder desactivar o activar los Usuarios
$(document).ready(function () {
    $(".boton-estado").click(function (e) {
        e.preventDefault();
        var userId = $(this).data("id");
        var url = $(this).data("url");

        $.post(url, { id: userId})
            .done(function (nuevoEstado) {
                alert("Estado cambiado con éxito");
                location.reload();
            })
            .fail(function () {
                alert("Fallo al cambiar el estado del usuario");
            });
    });
});

