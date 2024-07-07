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
//Funcion para Mostar el formulario de "Reportar"
function toggleReportForm(button) {
    var form = button.nextElementSibling.querySelector('.report-form');
    if (form.style.display === "none" || form.style.display === "") {
        form.style.display = "block";
    } else {
        form.style.display = "none";
    }
}

//Funcion para que muestre un mensaje al usuario de que se envio correctamente
$(document).ready(function () {
    $('#FormReport').on('submit', function (e) {
        e.preventDefault();
        e.target.submit();
        sessionStorage.setItem('toastrMessage', 'Reporte Enviado');
    });
});
document.addEventListener('DOMContentLoaded', function () {
    var toastrMessage = sessionStorage.getItem('toastrMessage');
    if (toastrMessage) {
        toastr.success(toastrMessage);
        sessionStorage.removeItem('toastrMessage'); 
    }
});