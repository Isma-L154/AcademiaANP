var botonEstado = document.getElementById("BotonCambio")
//Metodo para poder desactivar o activar los Usuarios 
$(document).ready(function () {
    $(".boton-estado").click(function (e) {
        e.preventDefault(); 
        var userId = $(this).data("id"); 
        var url = $(this).data("url");
        var estado = $(this).data("estado");

        $.post(url, {id: userId })
            .done(function () {
                alert("Estado cambiado con exito");
             })
             .fail(function() {
                 alert("Fallo al cambiar el estado del usuario");
             });
    });
});



