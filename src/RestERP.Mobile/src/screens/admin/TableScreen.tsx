import React, { useCallback, useState } from 'react';
import { Alert, Modal, Pressable, ScrollView, StyleSheet, Text, View } from 'react-native';
import { useFocusEffect, useNavigation } from '@react-navigation/native';
import type { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { orderApi, tableApi } from '../../api/services';
import { Loader, PrimaryButton } from '../../components/ui';
import { colors } from '../../theme/colors';
import { OrderStatus, type Order, type Table } from '../../types';
import type { AdminStackParamList } from '../../navigation/types';

export function TableScreen() {
  const navigation = useNavigation<NativeStackNavigationProp<AdminStackParamList>>();
  const [tables, setTables] = useState<Table[]>([]);
  const [occupiedIds, setOccupiedIds] = useState<number[]>([]);
  const [selected, setSelected] = useState<Table | null>(null);
  const [loading, setLoading] = useState(true);

  const load = useCallback(async () => {
    try {
      const [tableData, activeOrders] = await Promise.all([tableApi.list(), orderApi.active().catch(() => [] as Order[])]);
      setTables(tableData ?? []);
      const busy = (activeOrders ?? [])
        .filter((order) => order.tableId && order.status !== OrderStatus.Cancelled)
        .map((order) => order.tableId!) ;
      setOccupiedIds([...new Set(busy)]);
    } catch (error) {
      Alert.alert('Masalar yüklenemedi', error instanceof Error ? error.message : 'API hatası');
    } finally {
      setLoading(false);
    }
  }, []);

  useFocusEffect(
    useCallback(() => {
      setLoading(true);
      load();
      const timer = setInterval(load, 30000);
      return () => clearInterval(timer);
    }, [load]),
  );

  if (loading && tables.length === 0) {
    return <Loader />;
  }

  return (
    <ScrollView style={styles.page} contentContainerStyle={styles.content}>
      <Text style={styles.title}>Masa Planı</Text>
      <View style={styles.grid}>
        {tables.map((table) => {
          const occupied = occupiedIds.includes(table.id) || table.isOccupied;
          return (
            <Pressable
              key={table.id}
              style={[styles.table, occupied ? styles.occupied : styles.available]}
              onPress={() => setSelected(table)}
            >
              <Text style={styles.tableNo}>Masa {table.id}</Text>
              <Text style={styles.tableStatus}>{occupied ? 'Dolu' : 'Boş'}</Text>
            </Pressable>
          );
        })}
      </View>

      <Modal visible={Boolean(selected)} transparent animationType="fade" onRequestClose={() => setSelected(null)}>
        <Pressable style={styles.overlay} onPress={() => setSelected(null)}>
          <Pressable style={styles.modal} onPress={() => undefined}>
            <Text style={styles.modalTitle}>MASA {selected?.id}</Text>
            <PrimaryButton
              title="Sipariş Al"
              onPress={() => {
                const id = selected?.id;
                setSelected(null);
                if (id) {
                  navigation.navigate('AdminOrderCreate', { tableId: id });
                }
              }}
            />
            <View style={{ height: 8 }} />
            <PrimaryButton
              title="Siparişleri Görüntüle"
              variant="secondary"
              onPress={() => {
                const id = selected?.id;
                setSelected(null);
                if (id) {
                  navigation.navigate('AdminOrders');
                }
              }}
            />
            <View style={{ height: 8 }} />
            <PrimaryButton
              title="Hesap Kapat"
              variant="danger"
              onPress={() => {
                const id = selected?.id;
                setSelected(null);
                if (id) {
                  navigation.navigate('AdminCheckout', { tableId: id });
                }
              }}
            />
          </Pressable>
        </Pressable>
      </Modal>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  page: { flex: 1, backgroundColor: colors.cream },
  content: { padding: 16, paddingBottom: 32 },
  title: { color: colors.primary, fontFamily: 'Lora_600SemiBold', fontSize: 24, marginBottom: 16 },
  grid: { flexDirection: 'row', flexWrap: 'wrap', gap: 12 },
  table: {
    width: '47%',
    minHeight: 110,
    borderRadius: 12,
    alignItems: 'center',
    justifyContent: 'center',
  },
  available: { backgroundColor: colors.success },
  occupied: { backgroundColor: colors.warning },
  tableNo: { color: colors.white, fontSize: 18, fontWeight: '700' },
  tableStatus: { color: colors.white, marginTop: 4 },
  overlay: {
    flex: 1,
    backgroundColor: colors.overlay,
    justifyContent: 'center',
    padding: 24,
  },
  modal: {
    backgroundColor: colors.white,
    borderRadius: 12,
    padding: 20,
  },
  modalTitle: {
    color: colors.primary,
    fontFamily: 'Lora_600SemiBold',
    fontSize: 22,
    textAlign: 'center',
    marginBottom: 16,
  },
});
