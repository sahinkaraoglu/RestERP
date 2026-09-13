document.addEventListener('DOMContentLoaded', function() {
    const addItemForm = document.getElementById('addItemForm');
    addItemForm.addEventListener('submit', function(e) {
        e.preventDefault();

        const formData = new FormData(addItemForm);

        const name = formData.get('Name');
        const turkishName = formData.get('TurkishName');
        const categoryId = formData.get('CategoryId');
        const price = formData.get('Price');

        if (!name || !turkishName || !categoryId || !price) {
            RestERPToast.warning('Lütfen tüm zorunlu alanları doldurun (Kategori, Türkçe ve İngilizce ürün adı, fiyat)');
            return;
        }

        const jsonData = {};
        for (const [key, value] of formData.entries()) {
            if (key === 'IsActive' || key === 'IsVegan' || key === 'IsVegetarian' || key === 'IsGlutenFree' || key === 'IsSpicy') {
                jsonData[key] = true;
            } else if (key === 'CategoryId') {
                jsonData[key] = parseInt(value);
            } else if (key === 'Price') {
                jsonData[key] = parseFloat(value.replace(',', '.'));
            } else {
                jsonData[key] = value;
            }
        }

        if (!formData.has('IsActive')) jsonData['IsActive'] = false;
        if (!formData.has('IsVegan')) jsonData['IsVegan'] = false;
        if (!formData.has('IsVegetarian')) jsonData['IsVegetarian'] = false;
        if (!formData.has('IsGlutenFree')) jsonData['IsGlutenFree'] = false;
        if (!formData.has('IsSpicy')) jsonData['IsSpicy'] = false;

        fetch('/Admin/Menu/AddMenuItem', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(jsonData)
        })
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }

            return response.json().catch(err => {
                throw new Error('JSON parse hatası: ' + err.message);
            });
        })
        .then(data => {
            if (data.success) {
                RestERPToast.success(data.message || 'Ürün başarıyla eklendi!');
                setTimeout(function() {
                    window.location.href = '/Admin/Menu';
                }, 1200);
            } else {
                RestERPToast.error(data.message || 'İşlem sırasında bir hata oluştu.');
            }
        })
        .catch(error => {
            console.error("Fetch hatası:", error);
            RestERPToast.error('Hata: ' + error.message);
        });
    });

    const priceInput = document.getElementById('itemPrice');
    priceInput.addEventListener('input', function(e) {
        this.value = this.value.replace(/[^0-9.]/g, '');
    });
});
