document.addEventListener('DOMContentLoaded', (event) => {
    const form = document.querySelector('form');
    const registerButton = document.querySelector('.button');
    const termsDialog = document.getElementById('termsDialog');
    const closeDialogButton = document.getElementById('closeDialog');
    const modalOverlay = document.getElementById('modalOverlay'); // Add this line
    const agreeCheckbox = document.getElementById('agreeCheckbox');
    const acceptButton = document.getElementById('acceptButton');

    // Function to show dialog and overlay
    const showDialog = () => {
        termsDialog.showModal(); // Show the dialog
        modalOverlay.style.display = 'block'; // Show the overlay
    };

    // Function to close dialog and overlay
    const closeDialog = () => {
        termsDialog.close();
        modalOverlay.style.display = 'none'; // Hide the overlay
    };

    registerButton.addEventListener('click', (event) => {
        showDialog();
        event.preventDefault();  // Prevent the form from submitting
    });

    // Prevent form submission
    form.addEventListener('submit', (event) => {
        event.preventDefault(); // Prevent the form from submitting
    });

    closeDialogButton.addEventListener('click', closeDialog);
    modalOverlay.addEventListener('click', closeDialog); // Optional: close dialog when overlay is clicked

    agreeCheckbox.addEventListener('change', () => {
        acceptButton.disabled = !agreeCheckbox.checked;
    });
});

