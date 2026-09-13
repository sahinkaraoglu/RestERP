import { API_BASE_URL } from '../config';
import { tokenStore } from './tokenStore';

export class ApiError extends Error {
  status: number;
  body: string;

  constructor(status: number, body: string) {
    super(body || `İstek başarısız (${status})`);
    this.status = status;
    this.body = body;
  }
}

function parseMessage(text: string): string {
  if (!text) {
    return 'Beklenmeyen bir hata oluştu.';
  }
  try {
    const json = JSON.parse(text);
    if (typeof json === 'string') {
      return json;
    }
    if (json.message) {
      return json.message;
    }
    if (json.title) {
      const details = json.errors
        ? Object.values(json.errors).flat().join(' ')
        : '';
      return details ? `${json.title} ${details}` : json.title;
    }
    return text;
  } catch {
    return text;
  }
}

async function refreshAccessToken(): Promise<string | null> {
  const refreshToken = await tokenStore.getRefreshToken();
  if (!refreshToken) {
    return null;
  }

  const response = await fetch(`${API_BASE_URL}/api/auth/refresh-token`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json', Accept: 'application/json' },
    body: JSON.stringify({ refreshToken }),
  });

  if (!response.ok) {
    await tokenStore.clear();
    return null;
  }

  const data = await response.json();
  await tokenStore.save(data);
  return data.accessToken as string;
}

export async function apiRequest<T>(
  path: string,
  options: RequestInit = {},
  retry = true,
): Promise<T> {
  const token = await tokenStore.getAccessToken();
  const headers: Record<string, string> = {
    Accept: 'application/json',
    ...(options.body ? { 'Content-Type': 'application/json' } : {}),
    ...(options.headers as Record<string, string> | undefined),
  };

  if (token) {
    headers.Authorization = `Bearer ${token}`;
  }

  const response = await fetch(`${API_BASE_URL}${path}`, {
    ...options,
    headers,
  });

  if (response.status === 401 && retry) {
    const nextToken = await refreshAccessToken();
    if (nextToken) {
      return apiRequest<T>(path, options, false);
    }
  }

  if (response.status === 204) {
    return undefined as T;
  }

  const text = await response.text();
  if (!response.ok) {
    throw new ApiError(response.status, parseMessage(text));
  }

  if (!text) {
    return undefined as T;
  }

  return JSON.parse(text) as T;
}

export const api = {
  get: <T>(path: string) => apiRequest<T>(path),
  post: <T>(path: string, body?: unknown) =>
    apiRequest<T>(path, { method: 'POST', body: body !== undefined ? JSON.stringify(body) : undefined }),
  put: <T>(path: string, body?: unknown) =>
    apiRequest<T>(path, { method: 'PUT', body: body !== undefined ? JSON.stringify(body) : undefined }),
  del: <T>(path: string) => apiRequest<T>(path, { method: 'DELETE' }),
};
