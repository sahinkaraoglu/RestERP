(function (window, document) {
    const ICONS = {
        success: 'bi-check-lg',
        error: 'bi-x-lg',
        warning: 'bi-exclamation-lg',
        info: 'bi-info-lg'
    };

    const TITLES = {
        success: 'Başarılı',
        error: 'Hata',
        warning: 'Uyarı',
        info: 'Bilgi'
    };

    const DEFAULT_DURATION = {
        success: 3500,
        error: 5000,
        warning: 4500,
        info: 4000
    };

    let host = null;
    let toastId = 0;

    function ensureHost() {
        if (host && document.body.contains(host)) {
            return host;
        }

        host = document.createElement('div');
        host.className = 'resterp-toast-host';
        host.setAttribute('aria-live', 'polite');
        host.setAttribute('aria-atomic', 'true');
        document.body.appendChild(host);
        return host;
    }

    function escapeHtml(value) {
        return String(value ?? '')
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;');
    }

    function removeToast(element) {
        if (!element || element.classList.contains('is-leaving')) {
            return;
        }

        element.classList.add('is-leaving');
        window.setTimeout(() => element.remove(), 180);
    }

    function show(message, type, options) {
        const resolvedType = ICONS[type] ? type : 'info';
        const settings = options || {};
        const duration = settings.sticky
            ? 0
            : (settings.duration ?? DEFAULT_DURATION[resolvedType]);

        const toast = document.createElement('div');
        toast.className = `resterp-toast resterp-toast--${resolvedType}`;
        toast.dataset.toastId = String(++toastId);
        toast.setAttribute('role', resolvedType === 'error' ? 'alert' : 'status');

        const title = settings.title || TITLES[resolvedType];
        toast.innerHTML = `
            <div class="resterp-toast-icon"><i class="bi ${ICONS[resolvedType]}"></i></div>
            <div class="resterp-toast-content">
                <div class="resterp-toast-title">${escapeHtml(title)}</div>
                <div class="resterp-toast-message">${escapeHtml(message)}</div>
            </div>
            <button type="button" class="resterp-toast-close" aria-label="Kapat">&times;</button>
        `;

        toast.querySelector('.resterp-toast-close').addEventListener('click', () => removeToast(toast));
        ensureHost().appendChild(toast);

        if (duration > 0) {
            window.setTimeout(() => removeToast(toast), duration);
        }

        return toast.dataset.toastId;
    }

    function confirm(message, options) {
        const settings = options || {};

        return new Promise((resolve) => {
            const toast = document.createElement('div');
            toast.className = 'resterp-toast resterp-toast--warning';
            toast.setAttribute('role', 'alertdialog');
            toast.innerHTML = `
                <div class="resterp-toast-icon"><i class="bi ${ICONS.warning}"></i></div>
                <div class="resterp-toast-content">
                    <div class="resterp-toast-title">${escapeHtml(settings.title || 'Onay')}</div>
                    <div class="resterp-toast-message">${escapeHtml(message)}</div>
                    <div class="resterp-toast-actions">
                        <button type="button" class="resterp-toast-btn-cancel">${escapeHtml(settings.cancelText || 'İptal')}</button>
                        <button type="button" class="resterp-toast-btn-confirm">${escapeHtml(settings.confirmText || 'Onayla')}</button>
                    </div>
                </div>
            `;

            const finish = (accepted) => {
                removeToast(toast);
                resolve(accepted);
            };

            toast.querySelector('.resterp-toast-btn-cancel').addEventListener('click', () => finish(false));
            toast.querySelector('.resterp-toast-btn-confirm').addEventListener('click', () => finish(true));
            ensureHost().appendChild(toast);
        });
    }

    function showServerToasts() {
        const source = document.getElementById('resterp-server-toasts');
        if (!source) {
            return;
        }

        const success = source.dataset.success;
        const error = source.dataset.error;
        const info = source.dataset.info;
        const warning = source.dataset.warning;

        if (success) RestERPToast.success(success);
        if (error) RestERPToast.error(error);
        if (info) RestERPToast.info(info);
        if (warning) RestERPToast.warning(warning);
    }

    const RestERPToast = {
        show,
        success: (message, options) => show(message, 'success', options),
        error: (message, options) => show(message, 'error', options),
        warning: (message, options) => show(message, 'warning', options),
        info: (message, options) => show(message, 'info', options),
        confirm,
        dismiss: (id) => {
            const toast = document.querySelector(`[data-toast-id="${id}"]`);
            removeToast(toast);
        }
    };

    window.RestERPToast = RestERPToast;
    window.showToast = function (message, type) {
        RestERPToast.show(message, type === 'error' ? 'error' : type || 'success');
    };

    document.addEventListener('DOMContentLoaded', showServerToasts);
})(window, document);
