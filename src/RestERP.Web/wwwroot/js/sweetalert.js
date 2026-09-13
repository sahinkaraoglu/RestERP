(function (window) {
    if (typeof Swal === 'undefined') {
        return;
    }

    const baseConfig = {
        customClass: {
            popup: 'resterp-swal-popup',
            title: 'resterp-swal-title',
            htmlContainer: 'resterp-swal-text',
            confirmButton: 'resterp-swal-btn resterp-swal-btn-confirm',
            cancelButton: 'resterp-swal-btn resterp-swal-btn-cancel'
        },
        buttonsStyling: false,
        color: '#5d4b38'
    };

    const RestERPSwal = Swal.mixin(baseConfig);

    RestERPSwal.confirm = function (options) {
        return RestERPSwal.fire(Object.assign({
            showCancelButton: true,
            confirmButtonText: 'Evet',
            cancelButtonText: 'Hayır',
            customClass: Object.assign({}, baseConfig.customClass, {
                confirmButton: 'resterp-swal-btn resterp-swal-btn-confirm resterp-swal-btn-primary'
            })
        }, options));
    };

    RestERPSwal.confirmDanger = function (options) {
        return RestERPSwal.fire(Object.assign({
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Evet, iptal et',
            cancelButtonText: 'Vazgeç',
            customClass: Object.assign({}, baseConfig.customClass, {
                confirmButton: 'resterp-swal-btn resterp-swal-btn-confirm resterp-swal-btn-danger'
            })
        }, options));
    };

    RestERPSwal.success = function (options) {
        const config = typeof options === 'string' ? { text: options } : options;
        return RestERPSwal.fire(Object.assign({
            icon: 'success',
            title: 'Başarılı!',
            confirmButtonText: 'Tamam'
        }, config));
    };

    RestERPSwal.error = function (options) {
        const config = typeof options === 'string' ? { text: options } : options;
        return RestERPSwal.fire(Object.assign({
            icon: 'error',
            title: 'Hata!',
            confirmButtonText: 'Tamam',
            customClass: Object.assign({}, baseConfig.customClass, {
                confirmButton: 'resterp-swal-btn resterp-swal-btn-confirm resterp-swal-btn-danger'
            })
        }, config));
    };

    window.RestERPSwal = RestERPSwal;
    window.Swal = RestERPSwal;
})(window);
