document.getElementById('PhoneNumber').addEventListener('input', function(e) {
    let value = e.target.value.replace(/\D/g, '');
    if (value.length > 11) {
        value = value.slice(0, 11);
    }
    e.target.value = value;
});

document.querySelector('form').addEventListener('submit', function(e) {
    const roleSelect = document.getElementById('RoleType');
    if (!roleSelect.value) {
        e.preventDefault();
        RestERPToast.warning('Lütfen bir pozisyon seçiniz!');
        roleSelect.focus();
    }

    const newPassword = document.getElementById('newPassword').value;
    const confirmPassword = document.getElementById('confirmPassword').value;

    if (newPassword || confirmPassword) {
        if (newPassword.length < 6) {
            e.preventDefault();
            RestERPToast.warning('Yeni şifre en az 6 karakter uzunluğunda olmalıdır!');
            document.getElementById('newPassword').focus();
            return;
        }

        if (newPassword !== confirmPassword) {
            e.preventDefault();
            RestERPToast.warning('Yeni şifreler eşleşmiyor!');
            document.getElementById('confirmPassword').focus();
            return;
        }
    }
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
    const newPassword = document.getElementById('newPassword');
    const confirmPassword = document.getElementById('confirmPassword');
    const passwordMatch = document.getElementById('passwordMatch');

    function validatePasswords() {
        if (confirmPassword.value && newPassword.value !== confirmPassword.value) {
            passwordMatch.classList.remove('d-none');
        } else {
            passwordMatch.classList.add('d-none');
        }
    }

    newPassword.addEventListener('input', validatePasswords);
    confirmPassword.addEventListener('input', validatePasswords);
}

togglePasswordVisibility('newPassword', 'toggleNewPassword');
togglePasswordVisibility('confirmPassword', 'toggleConfirmPassword');
checkPasswordMatch();
