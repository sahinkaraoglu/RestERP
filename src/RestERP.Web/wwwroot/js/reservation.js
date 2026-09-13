document.addEventListener('DOMContentLoaded', function() {
    const form = document.getElementById('reservationForm');

    // Bugünden önceki tarihleri seçilemez yap
    const dateInput = document.getElementById('date');
    const today = new Date().toISOString().split('T')[0];
    dateInput.setAttribute('min', today);

    // Saat seçimi için kısıtlamalar
    const timeInput = document.getElementById('time');
    timeInput.addEventListener('change', function() {
        const selectedTime = this.value;
        const [hours] = selectedTime.split(':');

    });
});
