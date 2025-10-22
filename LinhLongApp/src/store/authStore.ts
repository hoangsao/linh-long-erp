import { create } from 'zustand';
import type { UserDto } from '../features/auth/types/UserDto';
import { meApi } from '../features/auth/api/me';
import { loginApi } from '../features/auth/api/login';
import { logoutApi } from '../features/auth/api/logout';

type AuthState = {
  user: UserDto | null;
  loading: boolean;
  fetchMe: () => Promise<void>;
  login: (username: string, password: string) => Promise<void>;
  logout: () => Promise<void>;
  hasRole: (roles: readonly string[] | string) => boolean;
};

export const useAuthStore = create<AuthState>((set, get) => ({
  user: null,
  loading: true,

  fetchMe: async (): Promise<void> => {
    set({ loading: true });
    try {
      const me = await meApi();
      set({ user: me, loading: false });
    } catch {
      set({ user: null, loading: false });
    }
  },

  login: async (username: string, password: string): Promise<void> => {
    await loginApi(username, password);
    await get().fetchMe();
  },

  logout: async (): Promise<void> => {
    try { await logoutApi(); } finally { set({ user: null }); }
  },

  hasRole: (roles: readonly string[] | string): boolean => {
    const u = get().user;
    if (!u) return false;
    const required = Array.isArray(roles) ? roles : [roles];
    return required.some((r) => u.roles.includes(r));
  },
}));