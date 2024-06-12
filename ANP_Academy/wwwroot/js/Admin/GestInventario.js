//Funcion para poder realizar busquedas en especifico
document.getElementById('BotonBuscar').addEventListener('click', function (event) {
    event.preventDefault();
    var textoBusqueda = document.getElementById('InputTexto').value.toLowerCase();
    var filas = document.querySelectorAll('#TablaInventario tbody tr');
    filas.forEach(function (fila) { //Iteramos sobre cada una de las filas que en este caso seria las <tr>
        var coincide = false;

        //Buscamos todas las <td> dentro de las filas, cada una de estas van a ser celdas y vamos a iterar sobre ellas
        fila.querySelectorAll('td').forEach(function (celda) {
            if (celda.textContent.toLowerCase().indexOf(textoBusqueda) > -1) { //Convertimos a Lowercase todo para evitar errores y buscamos si el texto proporcionado esta dentro de alguna de estas celdas
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
