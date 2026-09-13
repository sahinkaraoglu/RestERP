function toggleCategory(categoryId) {
    const container = document.getElementById(`category-${categoryId}`);
    const header = container.previousElementSibling;

    if (container.style.maxHeight) {
        container.style.maxHeight = null;
        header.classList.remove('category-active');
    } else {
        container.style.maxHeight = container.scrollHeight + "px";
        header.classList.add('category-active');
    }
}

function increaseQuantity(subCategoryId) {
    const quantityElement = document.getElementById(`quantity-${subCategoryId}`);
    let quantity = parseInt(quantityElement.innerText);
    quantityElement.innerText = quantity + 1;
    updateCart();
    event.stopPropagation();
}

function decreaseQuantity(subCategoryId) {
    const quantityElement = document.getElementById(`quantity-${subCategoryId}`);
    let quantity = parseInt(quantityElement.innerText);
    if (quantity > 0) {
        quantityElement.innerText = quantity - 1;
        updateCart();
    }
    event.stopPropagation();
}

function updateCart() {
    const cartItems = document.getElementById('cartItems');
    const cartItemsContainer = document.getElementById('cartItemsContainer');
    const emptyCartMessage = document.querySelector('.empty-cart-message');
    const cartTotalElement = document.getElementById('cartTotal');

    let items = [];
    let total = 0;

    // Tüm ürünleri topla
    const categories = document.querySelectorAll('.category');
    categories.forEach(category => {
        const categoryName = category.querySelector('.category-header h3').textContent;
        const rows = category.querySelectorAll('.subcategory-table tr');

        rows.forEach(row => {
            const name = row.querySelector('.subcategory-name span').textContent;
            const quantity = parseInt(row.querySelector('.quantity-value').textContent);
            const price = parseFloat(row.querySelector('.subcategory-price').textContent);

            if (quantity > 0) {
                items.push({
                    name: name,
                    quantity: quantity,
                    price: price,
                    total: quantity * price
                });

                total += quantity * price;
            }
        });
    });

    // Sepeti güncelle
    cartItems.innerHTML = '';

    if (items.length > 0) {
        emptyCartMessage.style.display = 'none';

        items.forEach(item => {
            const itemElement = document.createElement('div');
            itemElement.className = 'cart-item';
            itemElement.innerHTML = `
                <div class="cart-item-name">
                    <span class="cart-item-quantity">x${item.quantity}</span>
                    <span>${item.name}</span>
                </div>
                <div class="cart-item-price">${item.total.toFixed(2)} ₺</div>
            `;
            cartItems.appendChild(itemElement);
        });

        cartTotalElement.textContent = `${total.toFixed(2)} ₺`;
    } else {
        emptyCartMessage.style.display = 'block';
        cartTotalElement.textContent = '0.00 ₺';
    }
}

function getCartItems() {
    const items = [];
    const categories = document.querySelectorAll('.category');

    categories.forEach(category => {
        const rows = category.querySelectorAll('.subcategory-table tr');

        rows.forEach(row => {
            const quantityElement = row.querySelector('.quantity-value');
            if (!quantityElement) return;

            const quantity = parseInt(quantityElement.textContent);
            if (quantity <= 0) return;

            const foodId = parseInt(quantityElement.id.replace('quantity-', ''));
            const name = row.querySelector('.food-title').textContent;
            const price = parseFloat(row.querySelector('.subcategory-price').textContent);

            items.push({
                foodId: foodId,
                name: name,
                quantity: quantity,
                price: price,
                total: quantity * price
            });
        });
    });

    return items;
}

document.addEventListener('DOMContentLoaded', function() {
    // Tab değiştirme işlevselliği
    const tabButtons = document.querySelectorAll('.tab-btn');
    const tabPanes = document.querySelectorAll('.tab-pane');

    tabButtons.forEach(button => {
        button.addEventListener('click', () => {
            // Aktif tab'ı değiştir
            tabButtons.forEach(btn => btn.classList.remove('active'));
            button.classList.add('active');

            // İlgili içeriği göster
            const tabId = button.getAttribute('data-tab');
            tabPanes.forEach(pane => {
                if (pane.id === tabId) {
                    pane.classList.add('active');
                } else {
                    pane.classList.remove('active');
                }
            });
        });
    });

    // Sipariş verme işlemi
    document.getElementById('orderButton').addEventListener('click', async function() {
        let loadingToastId;
        try {
            // Kullanıcı giriş kontrolü
            const menuRoot = document.querySelector('.restaurant-menu');
            const isAuthenticated = menuRoot?.dataset.authenticated === 'true';

            if (!isAuthenticated) {
                const goToLogin = await RestERPToast.confirm('Sipariş verebilmek için önce giriş yapmalısınız.', {
                    title: 'Giriş Gerekli',
                    confirmText: 'Giriş Yap',
                    cancelText: 'İptal'
                });
                if (goToLogin) {
                    window.location.href = '/Login/Index';
                }
                return;
            }

            const cartItems = getCartItems();
            if (cartItems.length === 0) {
                RestERPToast.warning('Lütfen sepete ürün ekleyiniz!', { title: 'Sepet Boş' });
                return;
            }

            const activeTabId = document.querySelector('.tab-btn.active').getAttribute('data-tab');
            let customerInfo = {};
            let isValid = true;

            if (activeTabId === 'dine-in') {
                const tableNumber = document.getElementById('tableNumber').value;
                if (!tableNumber || tableNumber < 1) {
                    RestERPToast.error('Lütfen geçerli bir masa numarası giriniz!');
                    isValid = false;
                } else {
                    customerInfo.type = 'dine-in';
                    customerInfo.tableNumber = parseInt(tableNumber);
                }
            } else if (activeTabId === 'delivery') {
                const name = document.getElementById('deliveryName').value;
                const phone = document.getElementById('deliveryPhone').value;
                const address = document.getElementById('deliveryAddress').value;

                if (!name || name.trim() === '') {
                    RestERPToast.error('Lütfen adınızı giriniz!');
                    isValid = false;
                } else if (!phone || phone.trim() === '') {
                    RestERPToast.error('Lütfen telefon numaranızı giriniz!');
                    isValid = false;
                } else if (!address || address.trim() === '') {
                    RestERPToast.error('Lütfen adresinizi giriniz!');
                    isValid = false;
                } else {
                    customerInfo.type = 'delivery';
                    customerInfo.name = name;
                    customerInfo.phone = phone;
                    customerInfo.address = address;
                }
            }

            if (!isValid) return;

            // Sipariş notunu al
            const orderNote = document.getElementById('orderNote').value;
            if (orderNote && orderNote.trim() !== '') {
                customerInfo.note = orderNote;
            }

            // Sipariş verilerini hazırla
            const orderData = {
                items: cartItems,
                customerInfo: customerInfo
            };

            loadingToastId = RestERPToast.info('Lütfen bekleyiniz...', {
                title: 'Sipariş Oluşturuluyor',
                sticky: true
            });

            // Anti-forgery token'ı al
            const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

            // Siparişi gönder
            const response = await fetch('/Order/CreateOrder', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify(orderData)
            });

            if (!response.ok) {
                let message = `HTTP error! status: ${response.status}`;
                try {
                    const errorBody = await response.json();
                    message = errorBody.message || message;
                } catch { }
                throw new Error(message);
            }

            const result = await response.json();

            RestERPToast.dismiss(loadingToastId);

            if (result.success) {
                RestERPToast.success(result.message || 'Siparişiniz başarıyla oluşturuldu.');
                setTimeout(() => {
                    window.location.href = '/Order';
                }, 1200);
            } else {
                RestERPToast.error(result.message || 'Sipariş oluşturulurken bir hata oluştu.');
            }
        } catch (error) {
            console.error('Sipariş gönderme hatası:', error);
            if (loadingToastId) {
                RestERPToast.dismiss(loadingToastId);
            }
            RestERPToast.error(error instanceof Error ? error.message : 'Sipariş gönderilirken bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.');
        }
    });

    // İlk yüklemede sepeti güncelle
    updateCart();
});
