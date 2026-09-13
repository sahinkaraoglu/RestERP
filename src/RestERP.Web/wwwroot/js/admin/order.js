function getOrderPage() {
    return document.querySelector('.order-container');
}

function confirmCancelOrder(orderId, tableNumber) {
    RestERPSwal.confirmDanger({
        title: 'Emin misiniz?',
        text: `Masa ${tableNumber} için tüm siparişi iptal etmek istediğinize emin misiniz?`
    }).then((result) => {
        if (!result.isConfirmed) return;

        $.ajax({
            url: getOrderPage().dataset.cancelUrl,
            type: 'POST',
            data: {
                id: orderId,
                cancelAll: true,
                tableNumber: tableNumber
            },
            success: function (response) {
                if (response.success) {
                    RestERPSwal.success({ text: 'Sipariş başarıyla iptal edildi.' }).then(() => location.reload());
                } else {
                    RestERPSwal.error({ text: response.message || 'Sipariş iptal edilirken bir hata oluştu.' });
                }
            },
            error: function () {
                RestERPSwal.error({ text: 'Bir hata oluştu.' });
            }
        });
    });
}

async function cancelOrderItem(orderId, orderItemId) {
    const confirmed = await RestERPToast.confirm('Bu ürünü iptal etmek istediğinize emin misiniz?', {
        title: 'Emin misiniz?',
        confirmText: 'Evet, iptal et',
        cancelText: 'Vazgeç'
    });

    if (!confirmed) {
        return;
    }

    $.ajax({
        url: getOrderPage().dataset.cancelItemUrl,
        type: 'POST',
        data: { orderId: orderId, orderItemId: orderItemId },
        success: function (result) {
            if (result.success) {
                RestERPToast.success(result.message || 'Ürün iptal edildi.');
                setTimeout(() => location.reload(), 900);
            } else {
                RestERPToast.error(result.message || 'Ürün iptal edilirken bir hata oluştu.');
            }
        },
        error: function () {
            RestERPToast.error('Bir hata oluştu.');
        }
    });
}
