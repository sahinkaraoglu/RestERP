document.getElementById('phoneNumber').addEventListener('input', function(e) {
    let value = e.target.value.replace(/\D/g, '');
    if (value.length > 11) {
        value = value.slice(0, 11);
    }
    e.target.value = value;
});

function togglePasswordVisibility(inputId, buttonId) {
    const input = document.getElementById(inputId);
    const button = document.getElementById(buttonId);
    const icon = button.querySelector('i');

    button.addEventListener('click', function() {
        if (input.type === 'password') {
            input.type = 'text';
            icon.classList.remove('bi-eye');
            icon.classList.add('bi-eye-slash');
        } else {
            input.type = 'password';
            icon.classList.remove('bi-eye-slash');
            icon.classList.add('bi-eye');
        }
    });
}

function checkPasswordMatch() {
    const password = document.getElementById('password');
    const confirmPassword = document.getElementById('confirmPassword');
    const passwordMatch = document.getElementById('passwordMatch');
    const submitButton = document.getElementById('submitButton');

    function validatePasswords() {
        if (confirmPassword.value && password.value !== confirmPassword.value) {
            passwordMatch.classList.remove('d-none');
            submitButton.disabled = true;
        } else {
            passwordMatch.classList.add('d-none');
            submitButton.disabled = false;
        }
    }

    password.addEventListener('input', validatePasswords);
    confirmPassword.addEventListener('input', validatePasswords);
}

document.querySelector('form').addEventListener('submit', function(e) {
    const roleSelect = document.getElementById('RoleType');
    if (!roleSelect.value) {
        e.preventDefault();
        RestERPToast.warning('Lütfen bir pozisyon seçiniz!');
        roleSelect.focus();
    }
});

togglePasswordVisibility('password', 'togglePassword');
togglePasswordVisibility('confirmPassword', 'toggleConfirmPassword');
checkPasswordMatch();
