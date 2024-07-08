$(document).ready(function () {
    $(document).on('submit', '#deleteForm', function (e) {
        e.preventDefault(); 
        var form = this; 
        var idPublicacion = $(form).find('input[name="IdPublicacion"]').val(); // Obtener el Id de la publi

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
document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.dropbtn').forEach(function (button) {
        button.addEventListener('click', function () {
            var dropdownContent = this.nextElementSibling;
            dropdownContent.classList.toggle('show');
        });
    });

    window.addEventListener('click', function (e) {
        if (!e.target.matches('.dropbtn')) {
            document.querySelectorAll('.dropdown-content').forEach(function (dropdown) {
                dropdown.classList.remove('show');
            });
        }
    });
});

