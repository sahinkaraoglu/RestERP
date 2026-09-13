document.addEventListener('DOMContentLoaded', function() {
    const modal = document.getElementById('tableModal');
    const tableNumber = document.getElementById('tableNumber');
    const closeBtn = document.querySelector('.close-btn');
    const viewOrderBtn = document.querySelector('.view-btn');
    const checkoutBtn = document.querySelector('.checkout-btn');
    const orderBtn = document.querySelector('.order-btn');

    function getTableIds() {
        return Array.from(document.querySelectorAll('.table-item[id^="table-"]'))
            .map(function (el) {
                return el.id.replace('table-', '');
            })
            .filter(Boolean);
    }

    function checkAllTableStatuses() {
        getTableIds().forEach(function (tableId) {
            checkTableStatus(tableId);
        });
    }

    checkAllTableStatuses();

    function checkTableStatus(tableId) {
        fetch(`/api/orders/table/${tableId}`)
            .then(response => response.json())
            .then(orders => {
                const hasActiveOrder = orders.some(order =>
                    order.status !== 'Completed' && order.status !== 'Cancelled' && !order.isPaid
                );
                const tableBox = document.querySelector(`#table-${tableId} .table-box`);
                const tableStatus = document.querySelector(`#table-${tableId} .table-status`);
                if (hasActiveOrder) {
                    tableBox.classList.add('occupied');
                    tableStatus.classList.remove('available');
                    tableStatus.classList.add('occupied');
                } else {
                    tableBox.classList.remove('occupied');
                    tableStatus.classList.remove('occupied');
                    tableStatus.classList.add('available');
                }
            })
            .catch(error => {
                console.error('Masa durumu kontrol edilirken hata:', error);
            });
    }

    window.handleTableClick = function(number) {
        tableNumber.textContent = number;
        modal.style.display = 'block';
        fetch(`/api/orders/table/${number}`)
            .then(response => response.json())
            .then(orders => {
                const hasActiveOrder = orders.some(order =>
                    order.status !== 'Completed' && order.status !== 'Cancelled' && !order.isPaid
                );
                if (hasActiveOrder) {
                    viewOrderBtn.disabled = false;
                    viewOrderBtn.style.opacity = '1';
                    checkoutBtn.disabled = false;
                    checkoutBtn.style.opacity = '1';
                    orderBtn.disabled = true;
                    orderBtn.style.opacity = '0.5';
                } else {
                    viewOrderBtn.disabled = true;
                    viewOrderBtn.style.opacity = '0.5';
                    checkoutBtn.disabled = true;
                    checkoutBtn.style.opacity = '0.5';
                    orderBtn.disabled = false;
                    orderBtn.style.opacity = '1';
                }
            })
            .catch(error => {
                console.error('Sipariş durumu kontrol edilirken hata:', error);
            });
    };

    orderBtn.addEventListener('click', function() {
        const tableNo = tableNumber.textContent;
        window.location.href = `/Admin/Order/Create?tableId=${tableNo}`;
    });

    viewOrderBtn.addEventListener('click', function() {
        const tableNo = tableNumber.textContent;
        window.location.href = `/Order/ViewOrder?tableId=${tableNo}`;
    });

    checkoutBtn.addEventListener('click', function() {
        if (checkoutBtn.disabled) return;

        const tableNo = tableNumber.textContent;

        RestERPSwal.confirm({
            title: 'Hesap kapatılsın mı?',
            text: `Masa ${tableNo} için hesabı kapatmak istiyor musunuz?`,
            icon: 'question'
        }).then((result) => {
            if (!result.isConfirmed) return;

            fetch('/api/orders/checkout/' + tableNo, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' }
            })
            .then(async response => {
                let data;
                try {
                    data = await response.json();
                } catch {
                    throw new Error('Sunucudan beklenmeyen cevap alındı.');
                }
                return data;
            })
            .then(data => {
                if (data.success) {
                    RestERPSwal.success({ text: 'Hesap kapatıldı!' }).then(() => {
                        modal.style.display = 'none';
                        checkTableStatus(tableNo);
                    });
                } else {
                    RestERPSwal.error({ text: data.message || 'Hesap kapatılamadı!' });
                }
            })
            .catch(error => {
                RestERPSwal.error({ text: 'Bir hata oluştu: ' + error.message });
            });
        });
    });

    closeBtn.addEventListener('click', function() {
        modal.style.display = 'none';
    });

    window.addEventListener('click', function(event) {
        if (event.target == modal) {
            modal.style.display = 'none';
        }
    });

    setInterval(function() {
        checkAllTableStatuses();
    }, 30000);
});
