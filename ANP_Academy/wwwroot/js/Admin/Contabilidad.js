$(document).ready(function () {
    var totalOriginal = $('#totalFiltrado').data('total-general');
    $('#FiltroConta').on('click', function (e) {
        e.preventDefault();

        var fechaCorte = $('#fechaCorte').val(); //Obtenemos la fecha de corte
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


