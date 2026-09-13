import React, { useEffect, useState } from 'react';
import { Alert, ScrollView, StyleSheet, Text } from 'react-native';
import { useNavigation, useRoute, type RouteProp } from '@react-navigation/native';
import { authApi, userApi } from '../../api/services';
import { Card, Field, Loader, PrimaryButton } from '../../components/ui';
import { colors } from '../../theme/colors';
import { Role } from '../../types';
import type { AdminStackParamList } from '../../navigation/types';

export function UserFormScreen() {
  const navigation = useNavigation();
  const route = useRoute<RouteProp<AdminStackParamList, 'AdminUserForm'>>();
  const userId = route.params?.userId;
  const [loading, setLoading] = useState(Boolean(userId));
  const [saving, setSaving] = useState(false);
  const [form, setForm] = useState({
    firstName: '',
    lastName: '',
    userName: '',
    email: '',
    phoneNumber: '',
    roleType: String(Role.Customer),
    isActive: 'true',
    password: '',
    confirmPassword: '',
  });

  useEffect(() => {
    if (!userId) {
      return;
    }
    (async () => {
      try {
        const user = await userApi.get(userId);
        setForm({
          firstName: user.firstName ?? '',
          lastName: user.lastName ?? '',
          userName: user.userName ?? '',
          email: user.email ?? '',
          phoneNumber: user.phoneNumber ?? '',
          roleType: String(user.roleType ?? Role.Customer),
          isActive: user.isActive ? 'true' : 'false',
          password: '',
          confirmPassword: '',
        });
      } catch (error) {
        Alert.alert('Kullanıcı yüklenemedi', error instanceof Error ? error.message : 'API hatası');
      } finally {
        setLoading(false);
      }
    })();
  }, [userId]);

  const set = (key: keyof typeof form, value: string) => {
    setForm((current) => ({ ...current, [key]: value }));
  };

  const onSave = async () => {
    if (!form.firstName || !form.lastName || !form.email) {
      Alert.alert('Eksik bilgi', 'Ad, soyad ve e-posta zorunludur.');
      return;
    }

    setSaving(true);
    try {
      if (userId) {
        await userApi.update(userId, {
          firstName: form.firstName,
          lastName: form.lastName,
          userName: form.userName,
          email: form.email,
          phoneNumber: form.phoneNumber,
          roleType: Number(form.roleType) as Role,
          isActive: form.isActive === 'true',
        });
      } else {
        if (!form.password || form.password !== form.confirmPassword) {
          Alert.alert('Şifre', 'Yeni kullanıcı için şifreler eşleşmelidir.');
          setSaving(false);
          return;
        }
        await authApi.register({
          firstName: form.firstName,
          lastName: form.lastName,
          userName: form.userName || form.email.split('@')[0],
          email: form.email,
          phoneNumber: form.phoneNumber,
          password: form.password,
          confirmPassword: form.confirmPassword,
        });
      }
      navigation.goBack();
    } catch (error) {
      Alert.alert('Kaydedilemedi', error instanceof Error ? error.message : 'API hatası');
    } finally {
      setSaving(false);
    }
  };

  if (loading) {
    return <Loader />;
  }

  return (
    <ScrollView contentContainerStyle={styles.page}>
      <Card>
        <Text style={styles.title}>{userId ? 'Kullanıcı Düzenle' : 'Yeni Kullanıcı'}</Text>
        <Field label="Ad" value={form.firstName} onChangeText={(v) => set('firstName', v)} />
        <Field label="Soyad" value={form.lastName} onChangeText={(v) => set('lastName', v)} />
        <Field label="Kullanıcı Adı" value={form.userName} onChangeText={(v) => set('userName', v)} autoCapitalize="none" />
        <Field label="E-posta" value={form.email} onChangeText={(v) => set('email', v)} autoCapitalize="none" keyboardType="email-address" />
        <Field label="Telefon" value={form.phoneNumber} onChangeText={(v) => set('phoneNumber', v)} keyboardType="phone-pad" />
        {userId ? (
          <>
            <Field label="Rol (1 Admin, 2 Personel, 3 Müşteri)" value={form.roleType} onChangeText={(v) => set('roleType', v)} keyboardType="number-pad" />
            <Field label="Aktif (true/false)" value={form.isActive} onChangeText={(v) => set('isActive', v)} autoCapitalize="none" />
          </>
        ) : (
          <>
            <Field label="Şifre" value={form.password} onChangeText={(v) => set('password', v)} secureTextEntry />
            <Field label="Şifre Tekrar" value={form.confirmPassword} onChangeText={(v) => set('confirmPassword', v)} secureTextEntry />
            <Text style={styles.hint}>Yeni kullanıcı API üzerinden Müşteri olarak kaydedilir.</Text>
          </>
        )}
        <PrimaryButton title="Kaydet" onPress={onSave} loading={saving} />
      </Card>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  page: { flexGrow: 1, backgroundColor: colors.cream, padding: 16 },
  title: { color: colors.primary, fontFamily: 'Lora_600SemiBold', fontSize: 22, textAlign: 'center', marginBottom: 12 },
  hint: { color: colors.textMuted, marginBottom: 12 },
});
