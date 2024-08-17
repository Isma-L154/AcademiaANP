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

//Funcion para poder mostrarle un bootbox al usuario antes de que elimine el comentario, es utilizando las librerias de JS
$(document).ready(function () {
    $(document).on('submit', '#deleteForm', function (e) {
        e.preventDefault();
        var form = this;
        var idComentario = $(form).find('input[name="id"]').val(); // Obtener el Id del Comentario

        bootbox.confirm({
            message: "¿Seguro que quieres eliminar esta publicación?",
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
            }
        });
    });

    var toastrMessage = sessionStorage.getItem('toastrMessage');
    if (toastrMessage) {
        toastr.success(toastrMessage);
        sessionStorage.removeItem('toastrMessage');
    }
});

//Funcion para editar un comentario
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