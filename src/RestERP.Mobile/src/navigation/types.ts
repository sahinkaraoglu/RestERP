import type { NavigatorScreenParams } from '@react-navigation/native';

export type CustomerTabParamList = {
  Home: undefined;
  Menu: undefined;
  Orders: undefined;
  Reservation: undefined;
};

export type AdminStackParamList = {
  AdminPanel: undefined;
  AdminFood: undefined;
  AdminFoodForm: { foodId?: number };
  AdminOrders: undefined;
  AdminOrderCreate: { tableId: number };
  AdminTables: undefined;
  AdminCheckout: { tableId: number };
  AdminUsers: undefined;
  AdminUserForm: { userId?: number };
  AdminReservations: undefined;
  AdminReport: undefined;
};

export type RootStackParamList = {
  Main: NavigatorScreenParams<CustomerTabParamList>;
  Login: undefined;
  SignUp: undefined;
  ViewOrder: { tableId: number };
  AccessDenied: undefined;
  Admin: NavigatorScreenParams<AdminStackParamList>;
};

declare global {
  namespace ReactNavigation {
    interface RootParamList extends RootStackParamList {}
  }
}
