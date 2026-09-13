import { Platform } from 'react-native';
import Constants from 'expo-constants';

function getLanHost(): string {
  // Android emülatör: bilgisayarın localhost'una 10.0.2.2 ile ulaşılır
  if (Platform.OS === 'android' && !Constants.isDevice) {
    return '10.0.2.2';
  }

  const hostUri =
    Constants.expoConfig?.hostUri ??
    Constants.linkingUri?.replace(/^exp:\/\//, '').split('/')[0];

  if (hostUri) {
    const host = hostUri.split(':')[0];
    if (host && host !== 'localhost' && host !== '127.0.0.1') {
      return host;
    }
  }

  return Platform.OS === 'android' ? '10.0.2.2' : 'localhost';
}

const host = getLanHost();

/** RestERP.API — http://localhost:5050 */
export const API_BASE_URL = `http://${host}:5050`;

/** RestERP.Web static images — http://localhost:5158 */
export const WEB_BASE_URL = `http://${host}:5158`;

export function resolveImageUrl(path?: string | null): string | undefined {
  if (!path) {
    return undefined;
  }
  if (path.startsWith('http://') || path.startsWith('https://')) {
    return path;
  }
  return `${WEB_BASE_URL}${path.startsWith('/') ? path : `/${path}`}`;
}
