import React, { useCallback, useEffect, useMemo, useState } from 'react';
import { Alert, Pressable, ScrollView, StyleSheet, Text, View } from 'react-native';
import { useFocusEffect, useNavigation } from '@react-navigation/native';
import type { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { orderApi, tableApi } from '../api/services';
import { Card, EmptyState, Loader, PrimaryButton, StatusBadge } from '../components/ui';
import { useAuth } from '../context/AuthContext';
import { colors } from '../theme/colors';
import { formatDateTime, formatMoney, orderStatusColor, orderStatusLabel } from '../utils/format';
import type { Order, Table } from '../types';
import type { RootStackParamList } from '../navigation/types';

export function OrdersScreen() {
  const navigation = useNavigation<NativeStackNavigationProp<RootStackParamList>>();
  const { isAuthenticated } = useAuth();
  const [orders, setOrders] = useState<Order[]>([]);
  const [tables, setTables] = useState<Table[]>([]);
  const [tableId, setTableId] = useState<number | null>(null);
  const [loading, setLoading] = useState(true);

  const load = useCallback(async () => {
    if (!isAuthenticated) {
      setLoading(false);
      return;
    }
    setLoading(true);
    try {
      const [orderData, tableData] = await Promise.all([orderApi.active(), tableApi.list()]);
      setOrders(orderData ?? []);
      setTables(tableData ?? []);
    } catch (error) {
      Alert.alert('Siparişler yüklenemedi', error instanceof Error ? error.message : 'API hatası');
    } finally {
      setLoading(false);
    }
  }, [isAuthenticated]);

  useFocusEffect(
    useCallback(() => {
      load();
    }, [load]),
  );

  useEffect(() => {
    if (!isAuthenticated) {
      navigation.navigate('AccessDenied');
    }
  }, [isAuthenticated, navigation]);

  const visible = useMemo(
    () => (tableId ? orders.filter((order) => order.tableId === tableId) : orders),
    [orders, tableId],
  );

  if (!isAuthenticated) {
    return (
      <View style={styles.page}>
        <EmptyState title="Giriş gerekli" subtitle="Siparişleri görmek için giriş yapın." />
        <PrimaryButton title="Giriş Yap" onPress={() => navigation.navigate('Login')} />
      </View>
    );
  }

  if (loading) {
    return <Loader />;
  }

  return (
    <ScrollView style={styles.page} contentContainerStyle={styles.content}>
      <ScrollView horizontal showsHorizontalScrollIndicator={false} style={styles.filters}>
        <Pressable style={[styles.chip, !tableId && styles.chipActive]} onPress={() => setTableId(null)}>
          <Text style={[styles.chipText, !tableId && styles.chipTextActive]}>Tüm masalar</Text>
        </Pressable>
        {tables.map((table) => (
          <Pressable
            key={table.id}
            style={[styles.chip, tableId === table.id && styles.chipActive]}
            onPress={() => setTableId(table.id)}
          >
            <Text style={[styles.chipText, tableId === table.id && styles.chipTextActive]}>Masa {table.id}</Text>
          </Pressable>
        ))}
      </ScrollView>

      {visible.length === 0 ? (
        <EmptyState title="Aktif sipariş yok" subtitle="Menüden yeni sipariş verebilirsiniz." />
      ) : (
        visible.map((order) => (
          <Card key={order.id} style={styles.card}>
            <View style={styles.cardHead}>
              <Text style={styles.orderNo}>{order.orderNumber || `Sipariş #${order.id}`}</Text>
              <StatusBadge label={orderStatusLabel(order.status)} color={orderStatusColor(order.status)} />
            </View>
            <Text style={styles.meta}>Masa {order.tableId ?? '-'} · {formatDateTime(order.orderDate)}</Text>
            {(order.orderItems ?? []).map((item, index) => (
              <Text key={`${order.id}-${item.foodId}-${index}`} style={styles.item}>
                {item.quantity}x {item.food?.turkishName || item.food?.name || `Ürün #${item.foodId}`} · {formatMoney(item.totalPrice)}
              </Text>
            ))}
            <Text style={styles.total}>Toplam: {formatMoney(order.totalAmount)}</Text>
            {order.tableId ? (
              <PrimaryButton
                title="Masayı Görüntüle"
                variant="secondary"
                onPress={() => navigation.navigate('ViewOrder', { tableId: order.tableId! })}
              />
            ) : null}
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
    padding: 16,
  },
  content: {
    paddingBottom: 24,
    gap: 12,
  },
  filters: {
    marginBottom: 4,
  },
  chip: {
    borderWidth: 1,
    borderColor: colors.borderStrong,
    backgroundColor: colors.creamInput,
    borderRadius: 20,
    paddingHorizontal: 12,
    paddingVertical: 8,
    marginRight: 8,
  },
  chipActive: {
    backgroundColor: colors.primary,
    borderColor: colors.primary,
  },
  chipText: {
    color: colors.primaryMid,
  },
  chipTextActive: {
    color: colors.white,
  },
  card: {
    marginBottom: 4,
  },
  cardHead: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 6,
  },
  orderNo: {
    color: colors.primary,
    fontWeight: '700',
    fontSize: 16,
  },
  meta: {
    color: colors.textMuted,
    marginBottom: 8,
  },
  item: {
    color: colors.text,
    marginBottom: 4,
  },
  total: {
    color: colors.accent,
    fontWeight: '700',
    marginVertical: 10,
  },
});
