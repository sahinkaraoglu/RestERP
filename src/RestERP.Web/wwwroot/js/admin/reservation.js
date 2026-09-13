$(document).ready(function () {
    if (!$('#reservationTable').length) {
        return;
    }

    $('#reservationTable').DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.13.7/i18n/tr.json"
        },
        "responsive": true,
        "pageLength": 10,
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "Tümü"]],
        "order": [[0, "desc"]]
    });
});

async function deleteReservation(id) {
    const confirmed = await RestERPToast.confirm('Bu rezervasyonu silmek istediğinizden emin misiniz?', {
        title: 'Emin misiniz?',
        confirmText: 'Evet, sil',
        cancelText: 'İptal'
    });

    if (!confirmed) {
        return;
    }

    $.post(document.querySelector('.person-container').dataset.deleteUrl, { id: id })
        .done(function () {
            RestERPToast.success('Rezervasyon silindi.');
            setTimeout(() => location.reload(), 900);
        })
        .fail(function () {
            RestERPToast.error('Rezervasyon silinirken bir hata oluştu.');
        });
}
