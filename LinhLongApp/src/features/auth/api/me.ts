import { api } from '../../../shared/api/http';
import type { UserDto } from '../types/UserDto';

export async function meApi(): Promise<UserDto> {
  const res = await api.get<UserDto>('/api/auth/me');
  return res.data;
}