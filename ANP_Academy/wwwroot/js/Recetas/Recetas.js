$(document).ready(function () {
    $('.marcar-leido-no-leido').click(function () {
        var idReceta = $(this).data('id');
        var $button = $(this);
        var $card = $button.closest('.card');

        // Determinar la acción basada en el texto del botón
        var esLeido = $button.hasClass('btn-no-leido');
        var actionUrl = esLeido ? '@Url.Action("MarcarComoNoLeido", "Recetas")' : '@Url.Action("MarcarComoLeida", "Recetas")';

        $.ajax({
            url: actionUrl,
            type: 'POST',
            data: { idReceta: idReceta },
            success: function (response) {
                if (response.success) {
                    if (esLeido) {
                        // Cambiar a estado no leído
                        $card.removeClass('leido');
                        $button.removeClass('btn-no-leido').addClass('btn-leido').text('Marcar como leído');
                        $card.find('.checkmark').remove();
                    } else {
                        // Cambiar a estado leído
                        $card.addClass('leido');
                        $button.removeClass('btn-leido').addClass('btn-no-leido').text('Marcar como no leído');
                        $card.append('<div class="checkmark">✓</div>');
                    }
                } else {
                    console.log('Error al cambiar el estado de la receta');
                }
            },
            error: function (xhr, status, error) {
                console.log('Error al cambiar el estado de la receta:', error);
            }
        });
    });

    // Script para manejar la calificación con estrellas
    $('input[type="radio"]').change(function () {
        var rating = $(this).val();
        var idReceta = $(this).attr('name').split('-')[1];
        var token = $('input[name="__RequestVerificationToken"]').val();
        var $spinner = $(this).closest('.rating').find('.loading-spinner');
        var $ratingAverage = $(this).closest('.rating').find('.rating-average');
        var $message = $(this).closest('.rating').find('.temp-message');
        var $starsContainer = $(this).closest('.rating');

        // Mostrar el spinner de carga
        $spinner.show();

        $.ajax({
            url: '@Url.Action("RateReceta", "Recetas")',
            type: 'POST',
            headers: {
                'RequestVerificationToken': token
            },
            data: {
                __RequestVerificationToken: token,
                idReceta: idReceta,
                rating: rating
            },
            success: function (response) {
                // Actualizar el promedio de calificación
                if (response.newRating !== undefined) {
                    $ratingAverage.text('(' + response.newRating.toFixed(1) + ')');
                    // Actualizar las estrellas
                    var fullStars = Math.floor(response.newRating);
                    var hasHalfStar = response.newRating - fullStars >= 0.5;

                    $starsContainer.find('label').each(function (index) {
                        if (index < fullStars) {
                            $(this).removeClass('half-star').addClass('full-star').text('★');
                        } else if (index === fullStars && hasHalfStar) {
                            $(this).removeClass('full-star').addClass('half-star').text('☆');
                        } else {
                            $(this).removeClass('full-star half-star').text('☆');
                        }
                    });
                }
                // Mostrar el mensaje temporal
                $message.show().delay(2000).fadeOut();
            },
            error: function (xhr, status, error) {
                alert('Hubo un error al enviar su calificación.');
                console.log("Error: " + error);
                console.log("Status: " + status);
                console.log(xhr.responseText);
            },
            complete: function () {
                // Ocultar el spinner de carga
                $spinner.hide();
            }
        });
    });
});
