import React, { useCallback, useMemo, useState } from 'react';
import { Alert, ScrollView, StyleSheet, Text, View } from 'react-native';
import { useFocusEffect } from '@react-navigation/native';
import { orderApi } from '../../api/services';
import { Card, EmptyState, Loader } from '../../components/ui';
import { colors } from '../../theme/colors';
import { formatDateTime, formatMoney, todayIsoDate } from '../../utils/format';
import { OrderStatus, type Order } from '../../types';

export function ReportScreen() {
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      setOrders((await orderApi.list()) ?? []);
    } catch (error) {
      Alert.alert('Rapor yüklenemedi', error instanceof Error ? error.message : 'API hatası');
    } finally {
      setLoading(false);
    }
  }, []);

  useFocusEffect(
    useCallback(() => {
      load();
    }, [load]),
  );

  const summary = useMemo(() => {
    const today = todayIsoDate();
    const valid = orders.filter((order) => order.status !== OrderStatus.Cancelled);
    const todayOrders = valid.filter((order) => (order.orderDate || '').startsWith(today));
    const paid = valid.filter((order) => order.isPaid);
    const topMap = new Map<string, number>();
    for (const order of valid) {
      for (const item of order.orderItems ?? []) {
        const name = item.food?.turkishName || item.food?.name || `Ürün #${item.foodId}`;
        topMap.set(name, (topMap.get(name) ?? 0) + item.quantity);
      }
    }
    const topProducts = [...topMap.entries()].sort((a, b) => b[1] - a[1]).slice(0, 5);
    return {
      totalOrders: valid.length,
      todayOrders: todayOrders.length,
      revenue: valid.reduce((sum, order) => sum + (order.totalAmount || 0), 0),
      paidRevenue: paid.reduce((sum, order) => sum + (order.totalAmount || 0), 0),
      topProducts,
    };
  }, [orders]);

  if (loading) {
    return <Loader />;
  }

  return (
    <ScrollView style={styles.page} contentContainerStyle={styles.content}>
      <View style={styles.header}>
        <Text style={styles.title}>Raporlar</Text>
        <Text style={styles.subtitle}>Sipariş verilerinden oluşturulan özet</Text>
      </View>
      <View style={styles.stats}>
        <Card style={styles.statCard}>
          <Text style={styles.statValue}>{summary.todayOrders}</Text>
          <Text style={styles.statLabel}>Bugünkü Sipariş</Text>
        </Card>
        <Card style={styles.statCard}>
          <Text style={styles.statValue}>{summary.totalOrders}</Text>
          <Text style={styles.statLabel}>Toplam Sipariş</Text>
        </Card>
        <Card style={styles.statCard}>
          <Text style={styles.statValue}>{formatMoney(summary.revenue)}</Text>
          <Text style={styles.statLabel}>Toplam Ciro</Text>
        </Card>
        <Card style={styles.statCard}>
          <Text style={styles.statValue}>{formatMoney(summary.paidRevenue)}</Text>
          <Text style={styles.statLabel}>Tahsil Edilen</Text>
        </Card>
      </View>
      <Card>
        <Text style={styles.section}>En çok satanlar</Text>
        {summary.topProducts.length === 0 ? <EmptyState title="Veri yok" /> : null}
        {summary.topProducts.map(([name, qty]) => (
          <Text key={name} style={styles.row}>{name} · {qty} adet</Text>
        ))}
      </Card>
      <Card>
        <Text style={styles.section}>Son siparişler</Text>
        {orders.slice(0, 12).map((order) => (
          <Text key={order.id} style={styles.row}>
            {order.orderNumber || `#${order.id}`} · Masa {order.tableId ?? '-'} · {formatDateTime(order.orderDate)} · {formatMoney(order.totalAmount)}
          </Text>
        ))}
      </Card>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  page: { flex: 1, backgroundColor: colors.cream },
  content: { padding: 16, gap: 12, paddingBottom: 32 },
  header: { backgroundColor: colors.primary, borderRadius: 12, padding: 20 },
  title: { color: colors.white, fontFamily: 'Lora_600SemiBold', fontSize: 24 },
  subtitle: { color: colors.accentSoft, marginTop: 4 },
  stats: { flexDirection: 'row', flexWrap: 'wrap', gap: 10 },
  statCard: { width: '47%', alignItems: 'center' },
  statValue: { color: colors.primary, fontWeight: '700', fontSize: 16, textAlign: 'center' },
  statLabel: { color: colors.textMuted, textAlign: 'center', marginTop: 4 },
  section: { color: colors.primary, fontFamily: 'Lora_600SemiBold', fontSize: 16, marginBottom: 8 },
  row: { color: colors.text, marginBottom: 6 },
});
