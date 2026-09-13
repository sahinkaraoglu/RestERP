import React from 'react';
import { Pressable, StyleSheet, Text, View } from 'react-native';
import { useNavigation } from '@react-navigation/native';
import type { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import { Divider } from '../components/ui';
import { useAuth } from '../context/AuthContext';
import { colors } from '../theme/colors';
import type { RootStackParamList } from './types';

export function BrandHeader() {
  const insets = useSafeAreaInsets();
  const navigation = useNavigation<NativeStackNavigationProp<RootStackParamList>>();
  const { isAuthenticated, isStaff, user, logout } = useAuth();

  return (
    <View style={[styles.wrap, { paddingTop: Math.max(insets.top, 12) }]}>
      <Text style={styles.brand}>RestERP Restaurant</Text>
      <Divider />
      <View style={styles.row}>
        {isAuthenticated ? (
          <>
            {isStaff ? (
              <Pressable style={styles.btn} onPress={() => navigation.navigate('Admin', { screen: 'AdminPanel' })}>
                <Text style={styles.btnText}>Panel</Text>
              </Pressable>
            ) : null}
            <Text style={styles.welcome} numberOfLines={1}>
              Hoş geldin, {user?.userName || user?.firstName}
            </Text>
            <Pressable style={styles.btn} onPress={logout}>
              <Text style={styles.btnText}>Çıkış Yap</Text>
            </Pressable>
          </>
        ) : (
          <Pressable style={styles.btn} onPress={() => navigation.navigate('Login')}>
            <Text style={styles.btnText}>Giriş</Text>
          </Pressable>
        )}
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  wrap: {
    backgroundColor: colors.creamCard,
    paddingHorizontal: 16,
    paddingBottom: 10,
    borderBottomWidth: 1,
    borderBottomColor: colors.border,
  },
  brand: {
    color: colors.primary,
    fontFamily: 'Lora_500Medium',
    fontSize: 22,
    textAlign: 'center',
    letterSpacing: 1,
  },
  row: {
    flexDirection: 'row',
    justifyContent: 'center',
    alignItems: 'center',
    flexWrap: 'wrap',
    gap: 8,
  },
  welcome: {
    color: colors.primaryMid,
    fontWeight: '500',
    maxWidth: 140,
  },
  btn: {
    borderWidth: 1,
    borderColor: colors.borderStrong,
    backgroundColor: colors.creamInput,
    borderRadius: 6,
    paddingHorizontal: 10,
    paddingVertical: 6,
  },
  btnText: {
    color: colors.primaryMid,
  },
});
