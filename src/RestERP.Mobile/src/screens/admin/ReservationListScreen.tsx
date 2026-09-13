import React, { useCallback, useState } from 'react';
import { Alert, ScrollView, StyleSheet, Text } from 'react-native';
import { useFocusEffect } from '@react-navigation/native';
import { reservationApi } from '../../api/services';
import { Card, EmptyState, Loader, PrimaryButton } from '../../components/ui';
import { colors } from '../../theme/colors';
import { formatDate } from '../../utils/format';
import type { Reservation } from '../../types';

export function ReservationListScreen() {
  const [reservations, setReservations] = useState<Reservation[]>([]);
  const [loading, setLoading] = useState(true);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      setReservations((await reservationApi.list()) ?? []);
    } catch (error) {
      Alert.alert('Rezervasyonlar yüklenemedi', error instanceof Error ? error.message : 'API hatası');
    } finally {
      setLoading(false);
    }
  }, []);

  useFocusEffect(
    useCallback(() => {
      load();
    }, [load]),
  );

  const remove = (item: Reservation) => {
    Alert.alert('Rezervasyonu sil', `${item.name} kaydı silinsin mi?`, [
      { text: 'Vazgeç' },
      {
        text: 'Sil',
        style: 'destructive',
        onPress: async () => {
          try {
            await reservationApi.remove(item.id);
            load();
          } catch (error) {
            Alert.alert('Silinemedi', error instanceof Error ? error.message : 'API hatası');
          }
        },
      },
    ]);
  };

  if (loading) {
    return <Loader />;
  }

  return (
    <ScrollView style={styles.page} contentContainerStyle={styles.content}>
      {reservations.length === 0 ? <EmptyState title="Rezervasyon yok" /> : null}
      {reservations.map((item) => (
        <Card key={item.id}>
          <Text style={styles.name}>{item.name}</Text>
          <Text style={styles.meta}>{item.phone}</Text>
          <Text style={styles.meta}>{formatDate(item.date)} · {item.time} · {item.guests} kişi</Text>
          {item.notes ? <Text style={styles.meta}>{item.notes}</Text> : null}
          <PrimaryButton title="Sil" variant="danger" onPress={() => remove(item)} />
        </Card>
      ))}
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  page: { flex: 1, backgroundColor: colors.cream },
  content: { padding: 16, gap: 12, paddingBottom: 32 },
  name: { color: colors.primary, fontWeight: '700', fontSize: 16, marginBottom: 4 },
  meta: { color: colors.textMuted, marginBottom: 4 },
});
