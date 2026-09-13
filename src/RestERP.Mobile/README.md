# RestERP Mobile

React Native (Expo) istemcisi. Web uygulamasındaki müşteri ve yönetim ekranlarını aynı RestERP API üzerinden sunar.

## Gereksinimler

- Node.js 20+
- Çalışan `RestERP.API` (`http://localhost:5000`)
- Yemek görselleri için `RestERP.Web` (`http://localhost:5158`)

## Çalıştırma

```bash
cd src/RestERP.Mobile
npm install
npm start
```

Ardından Expo Go veya emülatör ile açın.

- Android emülatör: API adresi otomatik olarak makine IP / `10.0.2.2:5000`
- Fiziksel cihaz: Expo ile aynı ağdaki bilgisayar IP’si kullanılır (`src/config.ts`)

## Test kullanıcıları

| E-posta | Şifre | Rol |
|---|---|---|
| admin@resterp.com | Admin123! | Admin |
| employee@resterp.com | Employee123! | Personel |
| customer@test.com | Customer123! | Müşteri |

## Ekranlar

Müşteri: Anasayfa, Menü (sepet + sipariş), Siparişler, Rezervasyon, Giriş / Kayıt

Yönetim (Admin / Personel): Panel, Menü, Sipariş, Masa, Kullanıcı, Rezervasyon, Rapor
