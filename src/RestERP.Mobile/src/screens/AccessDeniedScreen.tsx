import React from 'react';
import { StyleSheet, Text, View } from 'react-native';
import { useNavigation } from '@react-navigation/native';
import type { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { PrimaryButton } from '../components/ui';
import { colors } from '../theme/colors';
import type { RootStackParamList } from '../navigation/types';

export function AccessDeniedScreen() {
  const navigation = useNavigation<NativeStackNavigationProp<RootStackParamList>>();

  return (
    <View style={styles.page}>
      <Text style={styles.title}>Erişim Reddedildi</Text>
      <Text style={styles.text}>Bu sayfayı görüntülemek için giriş yapmanız gerekir.</Text>
      <PrimaryButton title="Giriş Yap" onPress={() => navigation.navigate('Login')} />
      <View style={{ height: 10 }} />
      <PrimaryButton title="Ana Sayfaya Dön" variant="secondary" onPress={() => navigation.navigate('Main', { screen: 'Home' })} />
    </View>
  );
}

const styles = StyleSheet.create({
  page: {
    flex: 1,
    backgroundColor: colors.cream,
    justifyContent: 'center',
    padding: 24,
    gap: 10,
  },
  title: {
    fontSize: 26,
    color: colors.primary,
    fontFamily: 'Lora_600SemiBold',
    textAlign: 'center',
    marginBottom: 8,
  },
  text: {
    color: colors.text,
    textAlign: 'center',
    marginBottom: 16,
  },
});
