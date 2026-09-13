import React, { useState } from 'react';
import { Alert, ScrollView, StyleSheet, Text } from 'react-native';
import { useNavigation } from '@react-navigation/native';
import type { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { Card, Divider, Field, PrimaryButton } from '../components/ui';
import { useAuth } from '../context/AuthContext';
import { colors } from '../theme/colors';
import type { RootStackParamList } from '../navigation/types';

export function SignUpScreen() {
  const navigation = useNavigation<NativeStackNavigationProp<RootStackParamList>>();
  const { register } = useAuth();
  const [form, setForm] = useState({
    userName: '',
    email: '',
    firstName: '',
    lastName: '',
    phoneNumber: '',
    password: '',
    confirmPassword: '',
  });
  const [loading, setLoading] = useState(false);

  const set = (key: keyof typeof form, value: string) => {
    setForm((current) => ({ ...current, [key]: value }));
  };

  const onSubmit = async () => {
    if (!form.userName || !form.email || !form.firstName || !form.lastName || !form.password) {
      Alert.alert('Eksik bilgi', 'Zorunlu alanları doldurun.');
      return;
    }
    if (form.password !== form.confirmPassword) {
      Alert.alert('Şifre uyuşmazlığı', 'Şifreler aynı olmalıdır.');
      return;
    }
    setLoading(true);
    try {
      await register({
        ...form,
        phoneNumber: form.phoneNumber || undefined,
      });
      navigation.goBack();
    } catch (error) {
      Alert.alert('Kayıt başarısız', error instanceof Error ? error.message : 'Kayıt oluşturulamadı');
    } finally {
      setLoading(false);
    }
  };

  return (
    <ScrollView contentContainerStyle={styles.page} keyboardShouldPersistTaps="handled">
      <Card>
        <Text style={styles.title}>RestERP</Text>
        <Divider />
        <Text style={styles.subtitle}>Yeni Hesap Oluştur</Text>
        <Field label="Kullanıcı Adı" value={form.userName} onChangeText={(v) => set('userName', v)} autoCapitalize="none" />
        <Field label="E-posta" value={form.email} onChangeText={(v) => set('email', v)} autoCapitalize="none" keyboardType="email-address" />
        <Field label="Ad" value={form.firstName} onChangeText={(v) => set('firstName', v)} />
        <Field label="Soyad" value={form.lastName} onChangeText={(v) => set('lastName', v)} />
        <Field
          label="Telefon Numarası"
          value={form.phoneNumber}
          onChangeText={(v) => set('phoneNumber', v.replace(/\D/g, '').slice(0, 11))}
          keyboardType="phone-pad"
          placeholder="05XX XXX XX XX"
        />
        <Field label="Şifre" value={form.password} onChangeText={(v) => set('password', v)} secureTextEntry />
        <Field label="Şifre Tekrar" value={form.confirmPassword} onChangeText={(v) => set('confirmPassword', v)} secureTextEntry />
        <PrimaryButton title="Kayıt Ol" onPress={onSubmit} loading={loading} />
        <Text style={styles.footer}>
          Zaten hesabınız var mı?{' '}
          <Text style={styles.link} onPress={() => navigation.replace('Login')}>
            Giriş Yap
          </Text>
        </Text>
      </Card>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  page: {
    flexGrow: 1,
    backgroundColor: colors.cream,
    padding: 20,
  },
  title: {
    fontSize: 26,
    color: colors.primary,
    fontFamily: 'Lora_600SemiBold',
    textAlign: 'center',
  },
  subtitle: {
    textAlign: 'center',
    color: colors.textSoft,
    marginBottom: 16,
  },
  footer: {
    marginTop: 18,
    textAlign: 'center',
    color: colors.text,
  },
  link: {
    color: colors.primary,
    fontWeight: '700',
  },
});
