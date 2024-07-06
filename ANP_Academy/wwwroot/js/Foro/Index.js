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
//Funicon para Mostar el formulario de "Reportar"
function toggleReportForm(button) {
    var form = button.nextElementSibling;
    if (form.style.display === "none") {
        form.style.display = "block";
    } else {
        form.style.display = "none";
    }
}