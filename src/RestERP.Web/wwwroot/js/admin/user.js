function scrollToPersonnel() {
    document.querySelector('.card-title:contains("Personel Listesi")').scrollIntoView({
        behavior: 'smooth'
    });
}

async function deletePerson(id) {
    const confirmed = await RestERPToast.confirm('Bu işlem geri alınamaz!', {
        title: 'Emin misiniz?',
        confirmText: 'Evet, sil',
        cancelText: 'İptal'
    });

    if (!confirmed) {
        return;
    }

    $.ajax({
        url: document.querySelector('.person-container').dataset.deleteUrl,
        type: 'POST',
        data: { id: id },
        success: function() {
            RestERPToast.success('Kullanıcı başarıyla silindi.');
            setTimeout(() => window.location.reload(), 900);
        },
        error: function() {
            RestERPToast.error('Kullanıcı silinirken bir hata oluştu.');
        }
    });
}
