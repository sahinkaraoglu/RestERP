import React, { useState } from 'react';
import { Alert, ScrollView, StyleSheet, Text, View } from 'react-native';
import { Card, Divider, Field, PrimaryButton } from '../components/ui';
import { reservationApi } from '../api/services';
import { colors } from '../theme/colors';
import { todayIsoDate } from '../utils/format';

export function ReservationScreen() {
  const [form, setForm] = useState({
    name: '',
    phone: '',
    date: todayIsoDate(),
    time: '19:00',
    guests: '2',
    notes: '',
  });
  const [loading, setLoading] = useState(false);

  const set = (key: keyof typeof form, value: string) => {
    setForm((current) => ({ ...current, [key]: value }));
  };

  const onSubmit = async () => {
    if (!form.name || !form.phone || !form.date || !form.time) {
      Alert.alert('Eksik bilgi', 'Ad, telefon, tarih ve saat zorunludur.');
      return;
    }
    const guests = Number(form.guests);
    if (!guests || guests < 1 || guests > 10) {
      Alert.alert('Misafir sayısı', 'Misafir sayısı 1 ile 10 arasında olmalıdır.');
      return;
    }

    setLoading(true);
    try {
      await reservationApi.create({
        name: form.name,
        phone: form.phone,
        date: new Date(`${form.date}T${form.time}:00`).toISOString(),
        time: form.time,
        guests,
        notes: form.notes || null,
      });
      Alert.alert('Rezervasyon alındı', 'Rezervasyonunuz başarıyla oluşturuldu.');
      setForm({ name: '', phone: '', date: todayIsoDate(), time: '19:00', guests: '2', notes: '' });
    } catch (error) {
      Alert.alert('Rezervasyon hatası', error instanceof Error ? error.message : 'Rezervasyon oluşturulamadı');
    } finally {
      setLoading(false);
    }
  };

  return (
    <ScrollView style={styles.page} contentContainerStyle={styles.content}>
      <View style={styles.header}>
        <Text style={styles.title}>Rezervasyon</Text>
        <Text style={styles.subtitle}>Masınızı şimdiden ayırtın</Text>
      </View>
      <Card>
        <Field label="Ad Soyad" value={form.name} onChangeText={(v) => set('name', v)} />
        <Field
          label="Telefon"
          value={form.phone}
          onChangeText={(v) => set('phone', v.replace(/\D/g, '').slice(0, 11))}
          keyboardType="phone-pad"
        />
        <Field label="Tarih (YYYY-AA-GG)" value={form.date} onChangeText={(v) => set('date', v)} />
        <Field label="Saat (SS:DD)" value={form.time} onChangeText={(v) => set('time', v)} />
        <Field label="Misafir sayısı (1-10)" value={form.guests} onChangeText={(v) => set('guests', v)} keyboardType="number-pad" />
        <Field label="Not" value={form.notes} onChangeText={(v) => set('notes', v)} multiline />
        <PrimaryButton title="Rezervasyon Yap" onPress={onSubmit} loading={loading} />
      </Card>
      <Card>
        <Text style={styles.infoTitle}>Rezervasyon Kuralları</Text>
        <Divider />
        <Text style={styles.info}>• En az 2 saat önceden rezervasyon yapınız.</Text>
        <Text style={styles.info}>• Maksimum misafir sayısı 10’dur.</Text>
        <Text style={styles.info}>• Onay SMS ile iletilir.</Text>
        <Text style={styles.info}>• 15 dakikadan fazla gecikmede masa serbest kalabilir.</Text>
      </Card>
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
    gap: 14,
  },
  header: {
    backgroundColor: colors.primary,
    borderRadius: 12,
    padding: 20,
  },
  title: {
    color: colors.white,
    fontSize: 24,
    fontFamily: 'Lora_600SemiBold',
  },
  subtitle: {
    color: colors.accentSoft,
    marginTop: 4,
  },
  infoTitle: {
    color: colors.primary,
    fontFamily: 'Lora_600SemiBold',
    fontSize: 16,
  },
  info: {
    color: colors.text,
    marginBottom: 6,
  },
});
