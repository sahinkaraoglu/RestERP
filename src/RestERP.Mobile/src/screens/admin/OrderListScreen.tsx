import React, { useCallback, useMemo, useState } from 'react';
import { Alert, ScrollView, StyleSheet, Text, View } from 'react-native';
import { useFocusEffect, useNavigation } from '@react-navigation/native';
import type { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { orderApi } from '../../api/services';
import { Card, EmptyState, Loader, PrimaryButton, StatusBadge } from '../../components/ui';
import { colors } from '../../theme/colors';
import { formatDateTime, formatMoney, orderStatusColor, orderStatusLabel } from '../../utils/format';
import { OrderStatus, type Order } from '../../types';
import type { AdminStackParamList } from '../../navigation/types';

export function OrderListScreen() {
  const navigation = useNavigation<NativeStackNavigationProp<AdminStackParamList>>();
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      setOrders((await orderApi.active()) ?? []);
    } catch (error) {
      Alert.alert('Siparişler yüklenemedi', error instanceof Error ? error.message : 'API hatası');
    } finally {
      setLoading(false);
    }
  }, []);

  useFocusEffect(
    useCallback(() => {
      load();
    }, [load]),
  );

  const grouped = useMemo(() => {
    const map = new Map<number, Order[]>();
    for (const order of orders) {
      const key = order.tableId ?? 0;
      map.set(key, [...(map.get(key) ?? []), order]);
    }
    return [...map.entries()].sort((a, b) => a[0] - b[0]);
  }, [orders]);

  const cancelItem = async (itemId?: number) => {
    if (!itemId) {
      return;
    }
    try {
      await orderApi.removeItem(itemId);
      load();
    } catch (error) {
      Alert.alert('İptal edilemedi', error instanceof Error ? error.message : 'API hatası');
    }
  };

  const cancelOrder = async (order: Order) => {
    try {
      await orderApi.setStatus(order.id, OrderStatus.Cancelled);
      load();
    } catch (error) {
      Alert.alert('İptal edilemedi', error instanceof Error ? error.message : 'API hatası');
    }
  };

  if (loading) {
    return <Loader />;
  }

  return (
    <ScrollView style={styles.page} contentContainerStyle={styles.content}>
      {grouped.length === 0 ? <EmptyState title="Aktif sipariş yok" /> : null}
      {grouped.map(([tableId, tableOrders]) => (
        <View key={tableId} style={styles.group}>
          <Text style={styles.groupTitle}>Masa {tableId || '-'}</Text>
          {tableOrders.map((order) => (
            <Card key={order.id}>
              <View style={styles.head}>
                <Text style={styles.orderNo}>{order.orderNumber || `#${order.id}`}</Text>
                <StatusBadge label={orderStatusLabel(order.status)} color={orderStatusColor(order.status)} />
              </View>
              <Text style={styles.meta}>{formatDateTime(order.orderDate)}</Text>
              {(order.orderItems ?? [])
                .filter((item) => item.status !== OrderStatus.Cancelled)
                .map((item, index) => (
                  <View key={`${order.id}-${index}`} style={styles.itemRow}>
                    <Text style={styles.item}>
                      {item.quantity}x {item.food?.turkishName || item.food?.name || `Ürün #${item.foodId}`} · {formatMoney(item.totalPrice)}
                    </Text>
                    {item.id ? (
                      <Text style={styles.cancelLink} onPress={() => cancelItem(item.id)}>
                        İptal
                      </Text>
                    ) : null}
                  </View>
                ))}
              <Text style={styles.total}>Toplam: {formatMoney(order.totalAmount)}</Text>
              <View style={styles.actions}>
                <View style={{ flex: 1 }}>
                  <PrimaryButton title="İptal Et" variant="danger" onPress={() => cancelOrder(order)} />
                </View>
                {order.tableId ? (
                  <View style={{ flex: 1 }}>
                    <PrimaryButton
                      title="Hesap Kapat"
                      variant="secondary"
                      onPress={() => navigation.navigate('AdminCheckout', { tableId: order.tableId! })}
                    />
                  </View>
                ) : null}
              </View>
            </Card>
          ))}
        </View>
      ))}
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  page: { flex: 1, backgroundColor: colors.cream },
  content: { padding: 16, gap: 12, paddingBottom: 32 },
  group: { gap: 10 },
  groupTitle: { color: colors.primary, fontFamily: 'Lora_600SemiBold', fontSize: 18 },
  head: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center' },
  orderNo: { color: colors.primary, fontWeight: '700' },
  meta: { color: colors.textMuted, marginVertical: 6 },
  itemRow: { flexDirection: 'row', justifyContent: 'space-between', marginBottom: 4 },
  item: { color: colors.text, flex: 1 },
  cancelLink: { color: colors.danger, fontWeight: '600' },
  total: { color: colors.accent, fontWeight: '700', marginVertical: 10 },
  actions: { flexDirection: 'row', gap: 8 },
});
