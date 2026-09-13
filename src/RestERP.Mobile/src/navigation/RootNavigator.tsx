import React from 'react';
import { NavigationContainer, DefaultTheme } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import Ionicons from '@expo/vector-icons/Ionicons';
import { HomeScreen } from '../screens/HomeScreen';
import { MenuScreen } from '../screens/MenuScreen';
import { OrdersScreen } from '../screens/OrdersScreen';
import { ReservationScreen } from '../screens/ReservationScreen';
import { LoginScreen } from '../screens/LoginScreen';
import { SignUpScreen } from '../screens/SignUpScreen';
import { ViewOrderScreen } from '../screens/ViewOrderScreen';
import { AccessDeniedScreen } from '../screens/AccessDeniedScreen';
import { PanelScreen } from '../screens/admin/PanelScreen';
import { FoodListScreen } from '../screens/admin/FoodListScreen';
import { FoodFormScreen } from '../screens/admin/FoodFormScreen';
import { OrderListScreen } from '../screens/admin/OrderListScreen';
import { OrderCreateScreen } from '../screens/admin/OrderCreateScreen';
import { TableScreen } from '../screens/admin/TableScreen';
import { CheckoutScreen } from '../screens/admin/CheckoutScreen';
import { UserListScreen } from '../screens/admin/UserListScreen';
import { UserFormScreen } from '../screens/admin/UserFormScreen';
import { ReservationListScreen } from '../screens/admin/ReservationListScreen';
import { ReportScreen } from '../screens/admin/ReportScreen';
import { colors } from '../theme/colors';
import { BrandHeader } from './AppHeader';
import type { AdminStackParamList, CustomerTabParamList, RootStackParamList } from './types';

const Stack = createNativeStackNavigator<RootStackParamList>();
const Tabs = createBottomTabNavigator<CustomerTabParamList>();
const AdminStack = createNativeStackNavigator<AdminStackParamList>();

const navTheme = {
  ...DefaultTheme,
  colors: {
    ...DefaultTheme.colors,
    background: colors.cream,
    card: colors.creamCard,
    text: colors.primary,
    border: colors.border,
    primary: colors.primary,
  },
};

function CustomerTabs() {
  return (
    <Tabs.Navigator
      screenOptions={({ route }) => ({
        header: () => <BrandHeader />,
        tabBarActiveTintColor: colors.primary,
        tabBarInactiveTintColor: colors.textMuted,
        tabBarStyle: {
          backgroundColor: colors.creamCard,
          borderTopColor: colors.border,
        },
        tabBarIcon: ({ color, size }) => {
          const icons: Record<string, keyof typeof Ionicons.glyphMap> = {
            Home: 'home-outline',
            Menu: 'restaurant-outline',
            Orders: 'receipt-outline',
            Reservation: 'calendar-outline',
          };
          return <Ionicons name={icons[route.name] ?? 'ellipse-outline'} size={size} color={color} />;
        },
      })}
    >
      <Tabs.Screen name="Home" component={HomeScreen} options={{ title: 'Anasayfa' }} />
      <Tabs.Screen name="Menu" component={MenuScreen} options={{ title: 'Menü' }} />
      <Tabs.Screen name="Orders" component={OrdersScreen} options={{ title: 'Sipariş' }} />
      <Tabs.Screen name="Reservation" component={ReservationScreen} options={{ title: 'Rezervasyon' }} />
    </Tabs.Navigator>
  );
}

function AdminNavigator() {
  return (
    <AdminStack.Navigator
      screenOptions={{
        headerStyle: { backgroundColor: colors.primary },
        headerTintColor: colors.white,
        headerTitleStyle: { fontFamily: 'Lora_600SemiBold' },
        contentStyle: { backgroundColor: colors.cream },
      }}
    >
      <AdminStack.Screen name="AdminPanel" component={PanelScreen} options={{ title: 'Yönetim Paneli' }} />
      <AdminStack.Screen name="AdminFood" component={FoodListScreen} options={{ title: 'Menü Yönetimi' }} />
      <AdminStack.Screen name="AdminFoodForm" component={FoodFormScreen} options={{ title: 'Ürün' }} />
      <AdminStack.Screen name="AdminOrders" component={OrderListScreen} options={{ title: 'Sipariş Takibi' }} />
      <AdminStack.Screen name="AdminOrderCreate" component={OrderCreateScreen} options={{ title: 'Sipariş Al' }} />
      <AdminStack.Screen name="AdminTables" component={TableScreen} options={{ title: 'Masa Planı' }} />
      <AdminStack.Screen name="AdminCheckout" component={CheckoutScreen} options={{ title: 'Hesap Kapat' }} />
      <AdminStack.Screen name="AdminUsers" component={UserListScreen} options={{ title: 'Kullanıcılar' }} />
      <AdminStack.Screen name="AdminUserForm" component={UserFormScreen} options={{ title: 'Kullanıcı' }} />
      <AdminStack.Screen name="AdminReservations" component={ReservationListScreen} options={{ title: 'Rezervasyonlar' }} />
      <AdminStack.Screen name="AdminReport" component={ReportScreen} options={{ title: 'Raporlar' }} />
    </AdminStack.Navigator>
  );
}

export function RootNavigator() {
  return (
    <NavigationContainer theme={navTheme}>
      <Stack.Navigator
        screenOptions={{
          headerStyle: { backgroundColor: colors.creamCard },
          headerTintColor: colors.primary,
          contentStyle: { backgroundColor: colors.cream },
        }}
      >
        <Stack.Screen name="Main" component={CustomerTabs} options={{ headerShown: false }} />
        <Stack.Screen name="Login" component={LoginScreen} options={{ title: 'Giriş' }} />
        <Stack.Screen name="SignUp" component={SignUpScreen} options={{ title: 'Kayıt Ol' }} />
        <Stack.Screen name="ViewOrder" component={ViewOrderScreen} options={{ title: 'Masa Siparişi' }} />
        <Stack.Screen name="AccessDenied" component={AccessDeniedScreen} options={{ title: 'Erişim Reddedildi' }} />
        <Stack.Screen name="Admin" component={AdminNavigator} options={{ headerShown: false }} />
      </Stack.Navigator>
    </NavigationContainer>
  );
}
