export enum Role {
  Admin = 1,
  Employee = 2,
  Customer = 3,
}

export enum OrderStatus {
  New = 0,
  InProgress = 1,
  Completed = 2,
  Cancelled = 3,
  Ready = 4,
}

export interface UserDto {
  id: number;
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  role: string;
}

export interface TokenResponse {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
  refreshTokenExpiresAt: string;
  user: UserDto;
}

export interface FoodCategory {
  id: number;
  name: string;
  turkishName: string;
  description?: string | null;
  isDeleted?: boolean;
}

export interface FoodImage {
  id: number;
  path: string;
  foodId: number;
}

export interface Food {
  id: number;
  name: string;
  turkishName: string;
  description?: string | null;
  price: number;
  categoryId: number;
  category?: FoodCategory | null;
  images?: FoodImage[];
  isDeleted?: boolean;
}

export interface Table {
  id: number;
  isOccupied: boolean;
  isPaid: boolean;
  isDeleted?: boolean;
}

export interface OrderItem {
  id?: number;
  orderId?: number;
  foodId: number;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
  isPaid?: boolean;
  status?: OrderStatus;
  food?: Food | null;
}

export interface Order {
  id: number;
  orderNumber: string;
  orderDate: string;
  tableId?: number | null;
  customerId?: number | null;
  employeeId?: number | null;
  status: OrderStatus;
  totalAmount: number;
  isPaid: boolean;
  orderItems: OrderItem[];
  createdDate?: string | null;
}

export interface Reservation {
  id: number;
  name: string;
  phone: string;
  date: string;
  time: string;
  guests: number;
  notes?: string | null;
}

export interface ApplicationUser {
  id: number;
  userName?: string | null;
  email?: string | null;
  phoneNumber?: string | null;
  firstName: string;
  lastName: string;
  address?: string | null;
  isActive: boolean;
  roleType: Role;
}

export interface CartItem {
  foodId: number;
  name: string;
  quantity: number;
  price: number;
}

export interface CreateOrderPayload {
  tableId?: number | null;
  customerId?: number | null;
  employeeId?: number | null;
  status: OrderStatus;
  totalAmount: number;
  isPaid: boolean;
  orderItems: OrderItem[];
}
