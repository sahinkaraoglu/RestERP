import React, { useCallback, useState } from 'react';
import { Alert, ScrollView, StyleSheet, Text } from 'react-native';
import { useFocusEffect, useNavigation, useRoute, type RouteProp } from '@react-navigation/native';
import { orderApi, tableApi } from '../../api/services';
import { Card, EmptyState, Loader, PrimaryButton } from '../../components/ui';
import { colors } from '../../theme/colors';
import { formatMoney } from '../../utils/format';
import { OrderStatus, type Order } from '../../types';
import type { AdminStackParamList } from '../../navigation/types';

export function CheckoutScreen() {
  const navigation = useNavigation();
  const route = useRoute<RouteProp<AdminStackParamList, 'AdminCheckout'>>();
  const tableId = route.params.tableId;
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const data = await orderApi.byTable(tableId);
      setOrders(
        (data ?? []).filter(
          (order) => !order.isPaid && order.status !== OrderStatus.Completed && order.status !== OrderStatus.Cancelled,
        ),
      );
    } catch (error) {
      Alert.alert('Hesap yüklenemedi', error instanceof Error ? error.message : 'API hatası');
    } finally {
      setLoading(false);
    }
  }, [tableId]);

  useFocusEffect(
    useCallback(() => {
      load();
    }, [load]),
  );

  const checkout = async () => {
    if (orders.length === 0) {
      Alert.alert('Açık hesap yok', 'Bu masada kapatılacak aktif bir hesap yok.');
      return;
    }
    setSaving(true);
    try {
      for (const order of orders) {
        const next: Order = {
          ...order,
          isPaid: true,
          status: OrderStatus.Completed,
          orderItems: (order.orderItems ?? []).map((item) => ({ ...item, isPaid: true })),
        };
        await orderApi.update(order.id, next);
      }
      await tableApi.setOccupied(tableId, false).catch(() => undefined);
      Alert.alert('Hesap kapatıldı', 'Masa ödemesi tamamlandı.');
      navigation.goBack();
    } catch (error) {
      Alert.alert('İşlem başarısız', error instanceof Error ? error.message : 'API hatası');
    } finally {
      setSaving(false);
    }
  };

  if (loading) {
    return <Loader />;
  }

  const total = orders.reduce((sum, order) => sum + (order.totalAmount || 0), 0);

  return (
    <ScrollView contentContainerStyle={styles.page}>
      <Text style={styles.title}>Masa {tableId} hesap kapat</Text>
      {orders.length === 0 ? <EmptyState title="Kapatılacak hesap yok" /> : null}
      {orders.map((order) => (
        <Card key={order.id}>
          <Text style={styles.orderNo}>{order.orderNumber || `#${order.id}`}</Text>
          {(order.orderItems ?? []).map((item, index) => (
            <Text key={`${order.id}-${index}`} style={styles.item}>
              {item.quantity}x {item.food?.turkishName || item.food?.name || `Ürün #${item.foodId}`} · {formatMoney(item.totalPrice)}
            </Text>
          ))}
          <Text style={styles.total}>{formatMoney(order.totalAmount)}</Text>
        </Card>
      ))}
      <Text style={styles.grand}>Genel Toplam: {formatMoney(total)}</Text>
      <PrimaryButton title="Hesabı Kapat" onPress={checkout} loading={saving} disabled={orders.length === 0} />
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  page: { flexGrow: 1, backgroundColor: colors.cream, padding: 16, gap: 12 },
  title: { color: colors.primary, fontFamily: 'Lora_600SemiBold', fontSize: 22 },
  orderNo: { color: colors.primary, fontWeight: '700', marginBottom: 8 },
  item: { color: colors.text, marginBottom: 4 },
  total: { color: colors.accent, fontWeight: '700', marginTop: 8 },
  grand: { color: colors.primary, fontSize: 18, fontWeight: '700', textAlign: 'center' },
});
