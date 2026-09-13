document.addEventListener('DOMContentLoaded', function() {
    const priceInput = document.querySelector('input[name="Price"]');
    priceInput.addEventListener('input', function() {
        this.value = this.value.replace(/[^0-9.]/g, '');
    });
});
