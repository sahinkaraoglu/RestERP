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

function increaseQuantity(foodId) {
    const quantityElement = document.getElementById(`quantity-${foodId}`);
    let quantity = parseInt(quantityElement.innerText);
    quantityElement.innerText = quantity + 1;
    updateCart();
    event.stopPropagation();
}

function decreaseQuantity(foodId) {
    const quantityElement = document.getElementById(`quantity-${foodId}`);
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

    const categories = document.querySelectorAll('.category-card');
    categories.forEach(category => {
        const foodCards = category.querySelectorAll('.food-card');

        foodCards.forEach(card => {
            const name = card.querySelector('h4').textContent;
            const quantity = parseInt(card.querySelector('.quantity-value').textContent);
            const price = parseFloat(card.querySelector('.food-price').textContent);

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

document.getElementById('orderButton').addEventListener('click', async function() {
    const tableNumber = document.getElementById('tableNumber').value;
    const orderNote = document.getElementById('orderNote').value;

    if (!tableNumber) {
        RestERPToast.error('Lütfen masa numarası giriniz.');
        return;
    }

    const orderItems = [];
    const categories = document.querySelectorAll('.category-card');
    let hasItems = false;

    categories.forEach(category => {
        const foodCards = category.querySelectorAll('.food-card');

        foodCards.forEach(card => {
            const quantity = parseInt(card.querySelector('.quantity-value').textContent);
            if (quantity > 0) {
                hasItems = true;
                const foodId = card.querySelector('.quantity-value').id.split('-')[1];
                const price = parseFloat(card.querySelector('.food-price').textContent);

                orderItems.push({
                    foodId: parseInt(foodId),
                    quantity: quantity,
                    price: price
                });
            }
        });
    });

    if (!hasItems) {
        RestERPToast.error('Lütfen en az bir ürün seçiniz.');
        return;
    }

    try {
        console.log('Gönderilen veri:', {
            items: orderItems,
            customerInfo: {
                type: 'dine-in',
                tableNumber: parseInt(tableNumber),
                note: orderNote
            }
        });

        const response = await fetch('/Admin/Order/CreateOrder', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                items: orderItems,
                customerInfo: {
                    type: 'dine-in',
                    tableNumber: parseInt(tableNumber),
                    note: orderNote
                }
            })
        });

        if (!response.ok) {
            const errorData = await response.text();
            console.error('Sunucu yanıtı:', response.status, errorData);
            throw new Error(`HTTP error! status: ${response.status}, message: ${errorData}`);
        }

        const result = await response.json();

        if (result.success) {
            RestERPToast.success('Sipariş başarıyla oluşturuldu!');
            setTimeout(() => {
                window.location.href = '/Admin/Table/Index';
            }, 1200);
        } else {
            RestERPToast.error('Sipariş oluşturulurken bir hata oluştu: ' + result.message);
        }
    } catch (error) {
        console.error('Sipariş oluşturma hatası:', error);
        RestERPToast.error('Sipariş oluşturulurken bir hata oluştu. Lütfen tekrar deneyiniz.');
    }
});

document.addEventListener('DOMContentLoaded', function() {
    const tableNumber = document.getElementById('tableNumber');
    if (!tableNumber.value) {
        RestERPToast.error('Masa numarası bulunamadı!');
        window.location.href = '/Admin/Table/Index';
    }
});
