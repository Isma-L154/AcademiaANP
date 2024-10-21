

function toggleEditForm(commentId) {
    const contentElement = document.getElementById(`commentContent_${commentId}`);
    const editForm = document.getElementById(`editForm_${commentId}`);

    if (editForm.style.display === 'none' || editForm.style.display === '') {
        editForm.style.display = 'block';
        contentElement.style.display = 'none';
    } else {
        editForm.style.display = 'none';
        contentElement.style.display = 'block';
    }
}
$(document).ready(function () {
    $(document).on('submit', '#deleteForm', function (e) {
        e.preventDefault();
        var form = this;
        var idComentario = $(form).find('input[name="id"]').val(); // Obtener el Id de la publi

        bootbox.confirm({
            message: "¿Seguro que quieres eliminar este comentario?",
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
                    form.submit();
                    sessionStorage.setItem('toastrMessage', 'Eliminado correctamente');
                }
            },
             backdrop: true 

        });
    });

    var toastrMessage = sessionStorage.getItem('toastrMessage');
    if (toastrMessage) {
        toastr.success(toastrMessage);
        sessionStorage.removeItem('toastrMessage');
    }
});

//Funcion para que se habilite el boton de comentar hasta que el usuario ponga algo en el campo de texto
function VerficarTexto(IdPublicacion) {
    const TextInput = document.getElementById('TextInput_' + IdPublicacion);
    const ButtonSubmit = document.getElementById('ButtonSubmit_' + IdPublicacion);

    if (TextInput.value.trim() !== '') {
        ButtonSubmit.disabled = false;
    } else {
        ButtonSubmit.disabled = true;
    }
};

// Función para mostrar u ocultar el modal por su ID
function toggleModal(modalId) {
    var modal = document.getElementById(modalId);
    if (modal.style.display === 'block') {
        modal.style.display = 'none';
    } else {
        modal.style.display = 'block';
    }
}

// Cerrar el modal si se hace clic fuera de él
window.onclick = function (event) {
    var modals = document.getElementsByClassName('modal');
    for (var i = 0; i < modals.length; i++) {
        if (event.target == modals[i]) {
            modals[i].style.display = 'none';
        }
    }
}

// Función para manejar el envío del formulario
$(document).ready(function () {
    // Selecciona todos los formularios de reporte
    $('form[id^="FormReport"]').on('submit', function (e) {
        e.preventDefault();
        var form = $(this); 
        var submitButton = form.find('button[type="submit"]'); 

        // Bloquea el botón para evitar múltiples clics
        submitButton.prop('disabled', true).text('Enviando...');
        e.target.submit();

        // Guarda el mensaje en el almacenamiento de la sesión
        sessionStorage.setItem('toastrMessage', 'Reporte Enviado');
    });

    // Mostrar el mensaje si está presente en sessionStorage
    var toastrMessage = sessionStorage.getItem('toastrMessage');
    if (toastrMessage) {
        toastr.success(toastrMessage);
        sessionStorage.removeItem('toastrMessage');
    }
});
