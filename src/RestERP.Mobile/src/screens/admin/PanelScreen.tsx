import React, { useCallback, useState } from 'react';
import { Alert, Pressable, ScrollView, StyleSheet, Text, View } from 'react-native';
import { useFocusEffect, useNavigation } from '@react-navigation/native';
import type { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { foodApi, orderApi, reservationApi, tableApi, userApi } from '../../api/services';
import { Loader } from '../../components/ui';
import { colors } from '../../theme/colors';
import { formatMoney, todayIsoDate } from '../../utils/format';
import { OrderStatus } from '../../types';
import type { AdminStackParamList } from '../../navigation/types';

interface Stats {
  menuItemCount: number;
  categoryCount: number;
  totalTables: number;
  occupiedTables: number;
  occupiedOrders: number;
  occupancy: number;
  allTotal: number;
  allActive: number;
  todayOrderCount: number;
  todayRevenue: number;
  monthRevenue: number;
  reservationToday: number;
  reservationTomorrow: number;
}

export function PanelScreen() {
  const navigation = useNavigation<NativeStackNavigationProp<AdminStackParamList>>();
  const [stats, setStats] = useState<Stats | null>(null);
  const [loading, setLoading] = useState(true);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const today = todayIsoDate();
      const monthStart = `${today.slice(0, 7)}-01`;
      const [foods, categories, tables, activeOrders, users, todayOrders, monthOrders, reservations] =
        await Promise.all([
          foodApi.list(),
          foodApi.categories(),
          tableApi.list(),
          orderApi.active(),
          userApi.list(),
          orderApi.byDate(today).catch(() => []),
          orderApi.byDateRange(monthStart, today).catch(() => []),
          reservationApi.list().catch(() => []),
        ]);

      const occupiedTables = (tables ?? []).filter((table) => table.isOccupied).length;
      const tomorrow = new Date();
      tomorrow.setDate(tomorrow.getDate() + 1);
      const tomorrowIso = tomorrow.toISOString().slice(0, 10);

      setStats({
        menuItemCount: foods?.length ?? 0,
        categoryCount: categories?.length ?? 0,
        totalTables: tables?.length ?? 0,
        occupiedTables,
        occupiedOrders: activeOrders?.length ?? 0,
        occupancy: tables?.length ? Math.round((occupiedTables / tables.length) * 100) : 0,
        allTotal: users?.length ?? 0,
        allActive: (users ?? []).filter((user) => user.isActive).length,
        todayOrderCount: todayOrders?.length ?? 0,
        todayRevenue: (todayOrders ?? [])
          .filter((order) => order.status !== OrderStatus.Cancelled)
          .reduce((sum, order) => sum + (order.totalAmount || 0), 0),
        monthRevenue: (monthOrders ?? [])
          .filter((order) => order.status !== OrderStatus.Cancelled)
          .reduce((sum, order) => sum + (order.totalAmount || 0), 0),
        reservationToday: (reservations ?? []).filter((item) => item.date?.startsWith(today)).length,
        reservationTomorrow: (reservations ?? []).filter((item) => item.date?.startsWith(tomorrowIso)).length,
      });
    } catch (error) {
      Alert.alert('Panel yüklenemedi', error instanceof Error ? error.message : 'API hatası');
    } finally {
      setLoading(false);
    }
  }, []);

  useFocusEffect(
    useCallback(() => {
      load();
    }, [load]),
  );

  if (loading || !stats) {
    return <Loader />;
  }

  const cards = [
    {
      title: 'Menü Yönetimi',
      desc: 'Yemek kategorileri ve ürünleri ekleyin, fiyatları düzenleyin',
      stats: [
        [stats.menuItemCount, 'Menü Ürünü'],
        [stats.categoryCount, 'Kategori'],
      ],
      action: 'Menü Yönetimi',
      onPress: () => navigation.navigate('AdminFood'),
      color: '#8d6e4c',
    },
    {
      title: 'Sipariş Takibi',
      desc: 'Siparişleri görüntüleyin, durumlarını takip edin',
      stats: [
        [stats.occupiedTables, 'Aktif Masa'],
        [stats.occupiedOrders, 'Sipariş'],
      ],
      action: 'Siparişleri Yönet',
      onPress: () => navigation.navigate('AdminOrders'),
      color: '#6d5a45',
    },
    {
      title: 'Masa Yönetimi',
      desc: 'Masaları düzenleyin, durumlarını kontrol edin',
      stats: [
        [stats.totalTables, 'Toplam Masa'],
        [`${stats.occupancy}%`, `Doluluk (${stats.occupiedTables}/${stats.totalTables})`],
      ],
      action: 'Masa Planı',
      onPress: () => navigation.navigate('AdminTables'),
      color: '#4CAF50',
    },
    {
      title: 'Personel ve Müşteri',
      desc: 'Personel ve müşteri kayıtlarını yönetin',
      stats: [
        [stats.allTotal, 'Kayıt'],
        [stats.allActive, 'Aktif'],
      ],
      action: 'Kullanıcılar',
      onPress: () => navigation.navigate('AdminUsers'),
      color: '#2196F3',
    },
    {
      title: 'Rezervasyonlar',
      desc: 'Bugün ve yarınki rezervasyonları takip edin',
      stats: [
        [stats.reservationToday, 'Bugün'],
        [stats.reservationTomorrow, 'Yarın'],
      ],
      action: 'Rezervasyonlar',
      onPress: () => navigation.navigate('AdminReservations'),
      color: '#b09a7b',
    },
    {
      title: 'Raporlar',
      desc: 'Sipariş ve ciro özetlerini görüntüleyin',
      stats: [
        [stats.todayOrderCount, 'Bugünkü Sipariş'],
        [formatMoney(stats.monthRevenue), 'Aylık Ciro'],
      ],
      action: 'Raporlar',
      onPress: () => navigation.navigate('AdminReport'),
      color: '#5d4b38',
    },
  ];

  return (
    <ScrollView style={styles.page} contentContainerStyle={styles.content}>
      <View style={styles.header}>
        <Text style={styles.title}>RestERP Yönetim Paneli</Text>
        <Text style={styles.subtitle}>Restoranınızı tek yerden kolayca yönetin</Text>
        <View style={styles.quick}>
          <View>
            <Text style={styles.quickValue}>{stats.todayOrderCount}</Text>
            <Text style={styles.quickLabel}>Bugünkü Sipariş</Text>
          </View>
          <View>
            <Text style={styles.quickValue}>{formatMoney(stats.todayRevenue)}</Text>
            <Text style={styles.quickLabel}>Günlük Gelir</Text>
          </View>
        </View>
      </View>
      {cards.map((card) => (
        <Pressable key={card.title} style={styles.card} onPress={card.onPress}>
          <View style={[styles.icon, { backgroundColor: card.color }]} />
          <Text style={styles.cardTitle}>{card.title}</Text>
          <Text style={styles.cardDesc}>{card.desc}</Text>
          <View style={styles.stats}>
            {card.stats.map(([value, label]) => (
              <View key={String(label)} style={styles.stat}>
                <Text style={styles.statValue}>{value}</Text>
                <Text style={styles.statLabel}>{label}</Text>
              </View>
            ))}
          </View>
          <Text style={styles.action}>{card.action}</Text>
        </Pressable>
      ))}
      <Text style={styles.hint}>Admin ve personel hesapları bu paneli görür.</Text>
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
    paddingBottom: 32,
  },
  header: {
    backgroundColor: colors.primary,
    borderRadius: 14,
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
  quick: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop: 16,
  },
  quickValue: {
    color: colors.white,
    fontSize: 20,
    fontWeight: '700',
  },
  quickLabel: {
    color: colors.accentSoft,
  },
  card: {
    backgroundColor: colors.white,
    borderRadius: 12,
    padding: 16,
    borderWidth: 1,
    borderColor: colors.border,
  },
  icon: {
    width: 36,
    height: 36,
    borderRadius: 8,
    marginBottom: 10,
  },
  cardTitle: {
    color: colors.primary,
    fontSize: 18,
    fontFamily: 'Lora_600SemiBold',
  },
  cardDesc: {
    color: colors.textMuted,
    marginVertical: 6,
  },
  stats: {
    flexDirection: 'row',
    gap: 16,
    marginBottom: 10,
  },
  stat: {
    flex: 1,
  },
  statValue: {
    color: colors.primary,
    fontWeight: '700',
    fontSize: 16,
  },
  statLabel: {
    color: colors.textMuted,
    fontSize: 12,
  },
  action: {
    color: colors.accent,
    fontWeight: '700',
  },
  hint: {
    color: colors.textMuted,
    textAlign: 'center',
    fontSize: 12,
  },
});
