import { OrderStatus, Role } from '../types';
import { colors } from '../theme/colors';

export function formatMoney(value?: number | null): string {
  const amount = Number(value ?? 0);
  return `₺${amount.toLocaleString('tr-TR', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  })}`;
}

export function formatDate(value?: string | null): string {
  if (!value) {
    return '-';
  }
  const date = new Date(value);
  if (Number.isNaN(date.getTime())) {
    return value;
  }
  return date.toLocaleDateString('tr-TR', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
  });
}

export function formatDateTime(value?: string | null): string {
  if (!value) {
    return '-';
  }
  const date = new Date(value);
  if (Number.isNaN(date.getTime())) {
    return value;
  }
  return date.toLocaleString('tr-TR', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  });
}

export function orderStatusLabel(status: OrderStatus): string {
  switch (status) {
    case OrderStatus.New:
      return 'Yeni';
    case OrderStatus.InProgress:
      return 'Hazırlanıyor';
    case OrderStatus.Ready:
      return 'Hazır';
    case OrderStatus.Completed:
      return 'Tamamlandı';
    case OrderStatus.Cancelled:
      return 'İptal Edildi';
    default:
      return 'Bilinmiyor';
  }
}

export function orderStatusColor(status: OrderStatus): string {
  switch (status) {
    case OrderStatus.New:
      return colors.statusNew;
    case OrderStatus.InProgress:
      return colors.statusPreparing;
    case OrderStatus.Ready:
      return colors.statusReady;
    case OrderStatus.Completed:
      return colors.statusCompleted;
    case OrderStatus.Cancelled:
      return colors.statusCancelled;
    default:
      return colors.textMuted;
  }
}

export function roleLabel(role?: string | number | null): string {
  if (role === Role.Admin || role === 'Admin') {
    return 'Admin';
  }
  if (role === Role.Employee || role === 'Employee') {
    return 'Personel';
  }
  return 'Müşteri';
}

export function isStaffRole(role?: string | null): boolean {
  return role === 'Admin' || role === 'Employee';
}

export function todayIsoDate(): string {
  return new Date().toISOString().slice(0, 10);
}
