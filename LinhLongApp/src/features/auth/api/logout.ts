import { api } from '../../../shared/api/http';

export async function logoutApi(): Promise<void> {
  await api.post('/api/auth/logout');
}