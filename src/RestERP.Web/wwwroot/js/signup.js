document.getElementById('phoneNumber').addEventListener('input', function(e) {
    let value = e.target.value.replace(/\D/g, ''); // Sadece rakamları al
    if (value.length > 11) {
        value = value.slice(0, 11);
    }
    e.target.value = value;
});
