import React, { useCallback, useState } from 'react';
import { Alert, ScrollView, StyleSheet, Text } from 'react-native';
import { useFocusEffect, useRoute, type RouteProp } from '@react-navigation/native';
import { orderApi } from '../api/services';
import { Card, EmptyState, Loader, StatusBadge } from '../components/ui';
import { colors } from '../theme/colors';
import { formatDateTime, formatMoney, orderStatusColor, orderStatusLabel } from '../utils/format';
import { OrderStatus, type Order } from '../types';
import type { RootStackParamList } from '../navigation/types';

export function ViewOrderScreen() {
  const route = useRoute<RouteProp<RootStackParamList, 'ViewOrder'>>();
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const data = await orderApi.byTable(route.params.tableId);
      setOrders(
        (data ?? []).filter(
          (order) =>
            !order.isPaid &&
            order.status !== OrderStatus.Completed &&
            order.status !== OrderStatus.Cancelled,
        ),
      );
    } catch (error) {
      Alert.alert('Siparişler yüklenemedi', error instanceof Error ? error.message : 'API hatası');
    } finally {
      setLoading(false);
    }
  }, [route.params.tableId]);

  useFocusEffect(
    useCallback(() => {
      load();
    }, [load]),
  );

  if (loading) {
    return <Loader />;
  }

  return (
    <ScrollView style={styles.page} contentContainerStyle={styles.content}>
      <Text style={styles.title}>Masa {route.params.tableId}</Text>
      {orders.length === 0 ? (
        <EmptyState title="Aktif sipariş yok" subtitle="Bu masada açık hesap bulunmuyor." />
      ) : (
        orders.map((order) => (
          <Card key={order.id}>
            <Text style={styles.orderNo}>{order.orderNumber || `Sipariş #${order.id}`}</Text>
            <Text style={styles.meta}>{formatDateTime(order.orderDate || order.createdDate)}</Text>
            <StatusBadge label={orderStatusLabel(order.status)} color={orderStatusColor(order.status)} />
            {(order.orderItems ?? []).map((item, index) => (
              <Text key={`${order.id}-${index}`} style={styles.item}>
                {item.quantity}x {item.food?.turkishName || item.food?.name || `Ürün #${item.foodId}`} · {formatMoney(item.totalPrice)}
              </Text>
            ))}
            <Text style={styles.total}>Toplam: {formatMoney(order.totalAmount)}</Text>
          </Card>
        ))
      )}
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  page: {
    flex: 1,
    backgroundColor: colors.cream,
  },
  content: {
    padding: 16,
    gap: 12,
  },
  title: {
    fontSize: 24,
    color: colors.primary,
    fontFamily: 'Lora_600SemiBold',
  },
  orderNo: {
    fontWeight: '700',
    color: colors.primary,
    marginBottom: 4,
  },
  meta: {
    color: colors.textMuted,
    marginBottom: 8,
  },
  item: {
    color: colors.text,
    marginTop: 6,
  },
  total: {
    marginTop: 10,
    color: colors.accent,
    fontWeight: '700',
  },
});
