
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

//Funcion para poder realizar busquedas en especifico
document.getElementById('BotonBuscar').addEventListener('click', function (event) {
    event.preventDefault();
    var textoBusqueda = document.getElementById('InputTexto').value.toLowerCase();
    var filas = document.querySelectorAll('#TablaUsuarios tbody tr');

    filas.forEach(function (fila) {
        var coincide = false;
        fila.querySelectorAll('td').forEach(function (celda) {
            if (celda.textContent.toLowerCase().indexOf(textoBusqueda) > -1) {
                coincide = true;
            }
        });
        if (coincide) {
            fila.style.display = '';
        } else {
            fila.style.display = 'none';
        }
    });
});
