import { api } from '../../../shared/api/http';

export async function refreshApi(): Promise<void> {
  await api.post('/api/auth/refresh');
}