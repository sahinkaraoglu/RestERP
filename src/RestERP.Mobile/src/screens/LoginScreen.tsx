import React, { useState } from 'react';
import { Alert, Pressable, ScrollView, StyleSheet, Text, View } from 'react-native';
import { useNavigation } from '@react-navigation/native';
import type { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { Card, Divider, Field, PrimaryButton } from '../components/ui';
import { useAuth } from '../context/AuthContext';
import { colors } from '../theme/colors';
import type { RootStackParamList } from '../navigation/types';

export function LoginScreen() {
  const navigation = useNavigation<NativeStackNavigationProp<RootStackParamList>>();
  const { login } = useAuth();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [secure, setSecure] = useState(true);
  const [loading, setLoading] = useState(false);

  const onSubmit = async () => {
    if (!email || !password) {
      Alert.alert('Eksik bilgi', 'E-posta ve şifre zorunludur.');
      return;
    }
    setLoading(true);
    try {
      await login(email.trim(), password);
      navigation.goBack();
    } catch (error) {
      Alert.alert('Giriş başarısız', error instanceof Error ? error.message : 'Geçersiz email veya şifre');
    } finally {
      setLoading(false);
    }
  };

  return (
    <ScrollView contentContainerStyle={styles.page} keyboardShouldPersistTaps="handled">
      <Card>
        <Text style={styles.title}>Giriş Yap</Text>
        <Divider />
        <Field
          label="E-posta"
          value={email}
          onChangeText={setEmail}
          autoCapitalize="none"
          keyboardType="email-address"
        />
        <View>
          <Field
            label="Şifre"
            value={password}
            onChangeText={setPassword}
            secureTextEntry={secure}
          />
          <Pressable onPress={() => setSecure((value) => !value)}>
            <Text style={styles.toggle}>{secure ? 'Şifreyi göster' : 'Şifreyi gizle'}</Text>
          </Pressable>
        </View>
        <PrimaryButton title="Giriş Yap" onPress={onSubmit} loading={loading} />
        <Text style={styles.footer}>
          Hesabınız yok mu?{' '}
          <Text style={styles.link} onPress={() => navigation.replace('SignUp')}>
            Kayıt Ol
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
    justifyContent: 'center',
  },
  title: {
    fontSize: 26,
    color: colors.primary,
    fontFamily: 'Lora_600SemiBold',
    textAlign: 'center',
  },
  toggle: {
    color: colors.primaryMid,
    textAlign: 'right',
    marginTop: -8,
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
