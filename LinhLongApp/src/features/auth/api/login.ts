import { api } from '../../../shared/api/http';

export async function loginApi(username: string, password: string): Promise<void> {
  await api.post('/api/auth/login', { username, password });
}