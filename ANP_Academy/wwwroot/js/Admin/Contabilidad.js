document.addEventListener('DOMContentLoaded', function () {
    const hoy = new Date();
    const year = hoy.getFullYear();
    const month = (hoy.getMonth() + 1).toString().padStart(2, '0');
    const mesActual = `${year}-${month}`;
    const fechaCorteInput = document.getElementById('fechaCorte');

    // Definir el límite máximo de fecha
    fechaCorteInput.setAttribute('max', mesActual);
    let errorShown = false;
    // Verificar compatibilidad de `month` en el navegador
    if (fechaCorteInput.type !== 'month') {
        fechaCorteInput.type = 'text';
        fechaCorteInput.placeholder = 'YYYY-MM';


        // Validación del formato en Firefox
        fechaCorteInput.addEventListener('input', function (e) {
            const valor = e.target.value;
            const formatoValido = /^\d{4}-(0[1-9]|1[0-2])$/;
            const caracteresInvalidos = /[^0-9-]/; // Expresión regular para caracteres no válidos

            if (caracteresInvalidos.test(valor)) {
                if (!errorShown) { 
                    toastr.error("Formato inválido. Solo se permiten números y '-' en YYYY-MM");
                    errorShown = true; 
                }
                fechaCorteInput.setCustomValidity('Formato inválido. Solo se permiten números y "-" en YYYY-MM');
            } else if (!formatoValido.test(valor)) {
                if (!errorShown) { 
                    errorShown = true; 
                }
                fechaCorteInput.setCustomValidity('Formato inválido. Usa YYYY-MM');
            } else if (valor > mesActual) {
                toastr.error("No puedes seleccionar un mes futuro");
                fechaCorteInput.setCustomValidity('No puedes seleccionar un mes futuro.');
            } else {
                fechaCorteInput.setCustomValidity(''); // Entrada válida
                errorShown = false; 
            }
        });
    }
    var totalOriginal = $('#totalFiltrado').data('total-general');
    $('#FiltroConta').on('click', function (e) {
        e.preventDefault();
        var fechaCorte = $('#fechaCorte').val(); //Obtenemos la fecha de corte

        if (!fechaCorte || fechaCorte > mesActual) {
            toastr.error("Seleccione una fecha válida (no futura)");
            return; // No aplicar el filtro si la fecha no es válida
        }

        if (fechaCorte) {
            $('#fechaDescarga').val(fechaCorte); // Actualiza fecha en el boton de descarga
            var totalFiltrado = 0;

            $('#ContaTable tbody tr').each(function () {
                //Obtenemos los valores que son los que vamos a ocupar para el filtro y los unicos que vamos a modificar
                var fechaFila = $(this).data('fecha');
                var cantidadText = $(this).find('.cantidad').text().trim();
                var precioText = $(this).find('.precio').text().trim().replace('₡', '').replace(/,/g, '');

                var cantidad = parseInt(cantidadText, 10);
                var precio = parseFloat(precioText);

                //Verificamos si la fehca que me estan pasando coincide con la fecha de las facturas
                if (fechaFila && fechaFila.startsWith(fechaCorte)) {
                    $(this).find('.cantidad').show();
                    $(this).find('.total').show();

                    var totalFila = cantidad * precio;
                    $(this).find('.total').text('₡' + totalFila.toLocaleString());

                    totalFiltrado += totalFila;
                    //Si todo es correcto, volvemos a realizar los calculos y modificamos la tabla para que se reflejen los cambios
                } else {
                    $(this).find('.cantidad').text('0');
                    $(this).find('.total').text('₡0');
                    //Si el valor no coincide, le ponemos un 0 tanto a la cantidad como al total, lo tomamos como si este fuera un null
                }
            });
            //Ocultamos el boton de busqueda y habilitamos el de RESET, ademas de modificar el total general de la tabla
            $('#totalFiltrado').find('h3').text('Recaudación total filtrada: ₡' + totalFiltrado.toLocaleString());
            if (totalFiltrado == 0) { //Si el total es 0/No hay facturas, mostrar una notificacion al usuario
                toastr.error("No existen facturas en esta fecha");
            }
            $('#FiltroConta').hide();
            $('#ResetFiltro').show();
        } else {
            toastr.error("Seleccione una fecha de corte");
        }
    });
    //Creamos una funcion para que cuando aparezca este boton de RESET nada mas haga un 'rollback' y ponga los valores como si no tuvieran el filtro
    $('#ResetFiltro').on('click', function () {
        $('#fechaCorte').val('');
        $('#ContaTable tbody tr').each(function () {
            $(this).find('.cantidad').text($(this).data('cantidad-original'));
            $(this).find('.total').text($(this).data('total-original'));
        });
        $('#totalFiltrado').find('h3').text('Recaudación total: ₡' + totalOriginal);
        $('#FiltroConta').show();
        $('#ResetFiltro').hide();
    });

});

//$(document).ready(function () {
    
//});


