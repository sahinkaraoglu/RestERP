document.addEventListener('DOMContentLoaded', function() {
    const filterButtons = document.querySelectorAll('.filter-btn');
    filterButtons.forEach(button => {
        button.addEventListener('click', function() {
            filterButtons.forEach(btn => btn.classList.remove('active'));
            this.classList.add('active');

            const category = this.getAttribute('data-category');
            filterMenuItems(category);
        });
    });

    function filterMenuItems(category) {
        const menuCategories = document.querySelectorAll('.menu-category');

        if (category === 'all') {
            menuCategories.forEach(item => {
                item.style.display = 'block';
            });
        } else {
            menuCategories.forEach(item => {
                if (item.getAttribute('data-category') === category) {
                    item.style.display = 'block';
                } else {
                    item.style.display = 'none';
                }
            });
        }
    }

    const searchInput = document.getElementById('menuSearch');
    searchInput.addEventListener('input', function() {
        const searchTerm = this.value.toLowerCase();
        const menuItems = document.querySelectorAll('.menu-item');

        menuItems.forEach(item => {
            const menuName = item.querySelector('.menu-name').textContent.toLowerCase();
            const menuDesc = item.querySelector('.menu-desc').textContent.toLowerCase();

            if (menuName.includes(searchTerm) || menuDesc.includes(searchTerm)) {
                item.style.display = 'block';
            } else {
                item.style.display = 'none';
            }
        });
    });

    const saveCategoryBtn = document.getElementById('saveCategoryBtn');
    saveCategoryBtn.addEventListener('click', function() {
        const categoryName = document.getElementById('categoryName').value;
        const categoryNameEn = document.getElementById('categoryNameEn').value;

        if (categoryName && categoryNameEn) {
            RestERPToast.success('Kategori başarıyla eklendi: ' + categoryName);
            $('#addCategoryModal').modal('hide');
            window.location.href = '/Admin/Menu';
        }
    });

    const saveItemBtn = document.getElementById('saveItemBtn');
    saveItemBtn.addEventListener('click', function() {
        const category = document.getElementById('itemCategory').value;
        const name = document.getElementById('itemName').value;
        const nameEn = document.getElementById('itemNameEn').value;
        const desc = document.getElementById('itemDesc').value;
        const price = document.getElementById('itemPrice').value;

        if (category && name && nameEn && price) {
            RestERPToast.success('Ürün başarıyla eklendi: ' + name);
            $('#addItemModal').modal('hide');
        }
    });

    const confirmDeleteBtn = document.getElementById('confirmDeleteBtn');
    confirmDeleteBtn.addEventListener('click', function() {
        const id = document.getElementById('deleteItemId').value;

        RestERPToast.success('Ürün başarıyla silindi');
        $('#deleteConfirmModal').modal('hide');
    });
});

function editMenuItem(id) {
    window.location.href = `/Admin/Menu/Edit?id=${id}`;
}

async function deleteMenuItem(id) {
    const confirmed = await RestERPToast.confirm('Bu ürünü silmek istediğinizden emin misiniz?', {
        title: 'Emin misiniz?',
        confirmText: 'Evet, sil',
        cancelText: 'İptal'
    });

    if (!confirmed) {
        return;
    }

    $.ajax({
        url: '/Admin/Menu/Delete',
        type: 'POST',
        data: { id: id },
        success: function(response) {
            if (response.success) {
                RestERPToast.success(response.message || 'Ürün başarıyla silindi.');
                setTimeout(() => window.location.reload(), 900);
            } else {
                RestERPToast.error(response.message || 'Ürün silinemedi.');
            }
        },
        error: function() {
            RestERPToast.error('Bir hata oluştu. Lütfen tekrar deneyin.');
        }
    });
}
