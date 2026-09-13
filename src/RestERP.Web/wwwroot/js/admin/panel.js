$(document).ready(function() {
    $.get('/Admin/Reservation/GetReservationStats', function(data) {
        $('#todayReservations').text(data.todayCount);
        $('#tomorrowReservations').text(data.tomorrowCount);
    });
});
