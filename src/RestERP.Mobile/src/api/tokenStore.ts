import { Platform } from 'react-native';
import * as SecureStore from 'expo-secure-store';
import AsyncStorage from '@react-native-async-storage/async-storage';
import type { TokenResponse } from '../types';

const ACCESS_KEY = 'resterp.accessToken';
const REFRESH_KEY = 'resterp.refreshToken';
const USER_KEY = 'resterp.user';

const memory = new Map<string, string>();

async function setItem(key: string, value: string) {
  if (Platform.OS === 'web') {
    await AsyncStorage.setItem(key, value);
    return;
  }
  try {
    await SecureStore.setItemAsync(key, value);
  } catch {
    memory.set(key, value);
  }
}

async function getItem(key: string) {
  if (Platform.OS === 'web') {
    return AsyncStorage.getItem(key);
  }
  try {
    return await SecureStore.getItemAsync(key);
  } catch {
    return memory.get(key) ?? null;
  }
}

async function deleteItem(key: string) {
  if (Platform.OS === 'web') {
    await AsyncStorage.removeItem(key);
    return;
  }
  try {
    await SecureStore.deleteItemAsync(key);
  } catch {
    memory.delete(key);
  }
}

export const tokenStore = {
  async save(tokens: TokenResponse) {
    await setItem(ACCESS_KEY, tokens.accessToken);
    await setItem(REFRESH_KEY, tokens.refreshToken);
    await setItem(USER_KEY, JSON.stringify(tokens.user));
  },

  async getAccessToken() {
    return getItem(ACCESS_KEY);
  },

  async getRefreshToken() {
    return getItem(REFRESH_KEY);
  },

  async getUser() {
    const raw = await getItem(USER_KEY);
    return raw ? JSON.parse(raw) : null;
  },

  async clear() {
    await deleteItem(ACCESS_KEY);
    await deleteItem(REFRESH_KEY);
    await deleteItem(USER_KEY);
  },
};
