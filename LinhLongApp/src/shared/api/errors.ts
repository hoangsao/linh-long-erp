import axios from 'axios';

export class ApiError extends Error {
  status?: number;
  constructor(message: string, status?: number) {
    super(message);
    this.status = status;
  }
}

export function extractApiMessage(err: unknown, fallback = 'Something went wrong'): string {
  if (axios.isAxiosError(err)) {
    const data = err.response?.data as { message?: string; title?: string } | undefined;
    return data?.message ?? data?.title ?? fallback;
  }
  if (err instanceof Error) return err.message;
  return fallback;
}