//Funcion para que se habilite el boton de comentar hasta que el usuario ponga algo en el campo de texto
function VerficarTexto(IdPublicacion) {
    const TextInput = document.getElementById('TextInput_' + IdPublicacion);
    const ButtonSubmit = document.getElementById('ButtonSubmit_' + IdPublicacion);

    //Que no sea vacio
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
//Funcion para que muestre un mensaje al usuario de que se envio correctamente
$(document).ready(function () {
    $('#FormReport').on('submit', function (e) {
        e.preventDefault();
         e.target.submit();
        sessionStorage.setItem('toastrMessage', 'Reporte Enviado');
    });
    var toastrMessage = sessionStorage.getItem('toastrMessage');
    if (toastrMessage) {
        toastr.success(toastrMessage);
        sessionStorage.removeItem('toastrMessage');
    }
});
