function setRoleValue(userId, inputId) {
    var dropdown = document.getElementById('roleDropdown-' + userId);
    var input = document.getElementById(inputId + '-' + userId);
    input.value = dropdown.value;
}


let selectedForm = null;

document.querySelectorAll('.open-delete-modal').forEach(button => {
    button.addEventListener('click', function () {
        selectedForm = this.closest('.delete-user-form');
        const userEmail = selectedForm.getAttribute('data-user-email');

        // Покажи имейла в модала
        document.getElementById('userEmailToDelete').textContent = userEmail;

        const modal = new bootstrap.Modal(document.getElementById('deleteConfirmModal'));
        modal.show();
    });
});

document.getElementById('confirmDeleteBtn').addEventListener('click', function () {
    if (selectedForm) {
        selectedForm.submit();
    }
});
