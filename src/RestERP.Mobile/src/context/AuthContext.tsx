import React, { createContext, useContext, useEffect, useMemo, useState } from 'react';
import { authApi } from '../api/services';
import { tokenStore } from '../api/tokenStore';
import { isStaffRole } from '../utils/format';
import type { UserDto } from '../types';

interface AuthContextValue {
  user: UserDto | null;
  loading: boolean;
  isAuthenticated: boolean;
  isStaff: boolean;
  login: (email: string, password: string) => Promise<void>;
  register: (payload: {
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    phoneNumber?: string;
    password: string;
    confirmPassword: string;
  }) => Promise<void>;
  logout: () => Promise<void>;
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [user, setUser] = useState<UserDto | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    tokenStore
      .getUser()
      .then((stored) => setUser(stored))
      .finally(() => setLoading(false));
  }, []);

  const value = useMemo<AuthContextValue>(
    () => ({
      user,
      loading,
      isAuthenticated: Boolean(user),
      isStaff: isStaffRole(user?.role),
      login: async (email, password) => {
        const tokens = await authApi.login(email, password);
        await tokenStore.save(tokens);
        setUser(tokens.user);
      },
      register: async (payload) => {
        const tokens = await authApi.register(payload);
        await tokenStore.save(tokens);
        setUser(tokens.user);
      },
      logout: async () => {
        try {
          const refreshToken = await tokenStore.getRefreshToken();
          if (refreshToken) {
            await authApi.revoke(refreshToken);
          }
        } catch {
          // Token already invalid — still clear local session
        }
        await tokenStore.clear();
        setUser(null);
      },
    }),
    [user, loading],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return context;
}
