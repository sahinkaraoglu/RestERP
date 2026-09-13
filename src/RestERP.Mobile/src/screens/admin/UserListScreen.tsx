import React, { useCallback, useMemo, useState } from 'react';
import { Alert, ScrollView, StyleSheet, Text, View } from 'react-native';
import { useFocusEffect, useNavigation } from '@react-navigation/native';
import type { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { userApi } from '../../api/services';
import { Card, EmptyState, Loader, PrimaryButton } from '../../components/ui';
import { colors } from '../../theme/colors';
import { roleLabel } from '../../utils/format';
import { Role, type ApplicationUser } from '../../types';
import type { AdminStackParamList } from '../../navigation/types';

export function UserListScreen() {
  const navigation = useNavigation<NativeStackNavigationProp<AdminStackParamList>>();
  const [users, setUsers] = useState<ApplicationUser[]>([]);
  const [loading, setLoading] = useState(true);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      setUsers((await userApi.list()) ?? []);
    } catch (error) {
      Alert.alert('Kullanıcılar yüklenemedi', error instanceof Error ? error.message : 'API hatası');
    } finally {
      setLoading(false);
    }
  }, []);

  useFocusEffect(
    useCallback(() => {
      load();
    }, [load]),
  );

  const staff = useMemo(
    () => users.filter((user) => user.roleType === Role.Admin || user.roleType === Role.Employee),
    [users],
  );
  const customers = useMemo(() => users.filter((user) => user.roleType === Role.Customer), [users]);

  const remove = (user: ApplicationUser) => {
    Alert.alert('Kullanıcıyı sil', `${user.firstName} ${user.lastName} pasifleştirilsin mi?`, [
      { text: 'Vazgeç' },
      {
        text: 'Sil',
        style: 'destructive',
        onPress: async () => {
          try {
            await userApi.remove(user.id);
            load();
          } catch (error) {
            Alert.alert('Silinemedi', error instanceof Error ? error.message : 'API hatası');
          }
        },
      },
    ]);
  };

  const renderUser = (user: ApplicationUser) => (
    <Card key={user.id}>
      <Text style={styles.name}>{user.firstName} {user.lastName}</Text>
      <Text style={styles.meta}>{user.userName} · {roleLabel(user.roleType)}</Text>
      <Text style={styles.meta}>{user.email} · {user.phoneNumber || '-'}</Text>
      <Text style={styles.meta}>{user.isActive ? 'Aktif' : 'Pasif'}</Text>
      <View style={styles.actions}>
        <View style={{ flex: 1 }}>
          <PrimaryButton title="Düzenle" variant="secondary" onPress={() => navigation.navigate('AdminUserForm', { userId: user.id })} />
        </View>
        <View style={{ flex: 1 }}>
          <PrimaryButton title="Sil" variant="danger" onPress={() => remove(user)} />
        </View>
      </View>
    </Card>
  );

  if (loading) {
    return <Loader />;
  }

  return (
    <ScrollView style={styles.page} contentContainerStyle={styles.content}>
      <PrimaryButton title="Yeni Kullanıcı" onPress={() => navigation.navigate('AdminUserForm', {})} />
      <Text style={styles.section}>Personel / Admin</Text>
      {staff.length === 0 ? <EmptyState title="Personel kaydı yok" /> : staff.map(renderUser)}
      <Text style={styles.section}>Müşteriler</Text>
      {customers.length === 0 ? <EmptyState title="Müşteri kaydı yok" /> : customers.map(renderUser)}
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  page: { flex: 1, backgroundColor: colors.cream },
  content: { padding: 16, gap: 12, paddingBottom: 32 },
  section: { color: colors.primary, fontFamily: 'Lora_600SemiBold', fontSize: 18, marginTop: 8 },
  name: { color: colors.primary, fontWeight: '700', fontSize: 16 },
  meta: { color: colors.textMuted, marginTop: 3 },
  actions: { flexDirection: 'row', gap: 8, marginTop: 10 },
});
