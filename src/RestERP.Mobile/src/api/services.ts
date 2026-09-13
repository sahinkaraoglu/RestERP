import { api } from './client';
import type {
  ApplicationUser,
  CreateOrderPayload,
  Food,
  FoodCategory,
  FoodImage,
  Order,
  OrderStatus,
  Reservation,
  Table,
  TokenResponse,
} from '../types';

export const authApi = {
  login: (email: string, password: string) =>
    api.post<TokenResponse>('/api/auth/login', { email, password }),
  register: (payload: {
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    phoneNumber?: string;
    address?: string;
    password: string;
    confirmPassword: string;
  }) => api.post<TokenResponse>('/api/auth/register', payload),
  revoke: (refreshToken: string) => api.post('/api/auth/revoke-token', { refreshToken }),
};

export const foodApi = {
  list: () => api.get<Food[]>('/api/food'),
  get: (id: number) => api.get<Food>(`/api/food/${id}`),
  categories: () => api.get<FoodCategory[]>('/api/food/categories'),
  images: () => api.get<FoodImage[]>('/api/food/images'),
  create: (food: Partial<Food>) => api.post<Food>('/api/food', food),
  update: (id: number, food: Partial<Food>) => api.put(`/api/food/${id}`, { ...food, id }),
  remove: (id: number) => api.del(`/api/food/${id}`),
};

export const tableApi = {
  list: () => api.get<Table[]>('/api/table'),
  get: (id: number) => api.get<Table>(`/api/table/${id}`),
  setOccupied: (id: number, isOccupied: boolean) =>
    api.put(`/api/table/${id}/status`, isOccupied),
};

export const orderApi = {
  list: () => api.get<Order[]>('/api/order'),
  active: () => api.get<Order[]>('/api/order/active'),
  byTable: (tableId: number) => api.get<Order[]>(`/api/order/table/${tableId}`),
  byDate: (date: string) => api.get<Order[]>(`/api/order/date/${date}`),
  byDateRange: (startDate: string, endDate: string) =>
    api.get<Order[]>(`/api/order/daterange?startDate=${startDate}&endDate=${endDate}`),
  details: (id: number) => api.get<Order>(`/api/order/${id}/details`),
  create: (payload: CreateOrderPayload) => api.post<Order>('/api/order', payload),
  update: (id: number, order: Order) => api.put(`/api/order/${id}`, order),
  setStatus: (id: number, status: OrderStatus) =>
    api.put(`/api/order/${id}/status`, status),
  removeItem: (orderItemId: number) => api.del(`/api/order/item/${orderItemId}`),
};

export const reservationApi = {
  list: () => api.get<Reservation[]>('/api/reservation'),
  create: (payload: Omit<Reservation, 'id'>) =>
    api.post<Reservation>('/api/reservation', payload),
  remove: (id: number) => api.del(`/api/reservation/${id}`),
};

export const userApi = {
  list: () => api.get<ApplicationUser[]>('/api/user'),
  get: (id: number) => api.get<ApplicationUser>(`/api/user/${id}`),
  byEmail: (email: string) => api.get<ApplicationUser>(`/api/user/email/${encodeURIComponent(email)}`),
  update: (id: number, user: Partial<ApplicationUser>) =>
    api.put(`/api/user/${id}`, { ...user, id }),
  resetPassword: (id: number, newPassword: string, confirmPassword: string) =>
    api.post(`/api/user/${id}/reset-password`, { newPassword, confirmPassword }),
  remove: (id: number) => api.del(`/api/user/${id}`),
};
