import React from 'react';
import { ImageBackground, ScrollView, StyleSheet, Text, View } from 'react-native';
import { useNavigation } from '@react-navigation/native';
import type { BottomTabNavigationProp } from '@react-navigation/bottom-tabs';
import { Divider, PrimaryButton } from '../components/ui';
import { colors } from '../theme/colors';
import type { CustomerTabParamList } from '../navigation/types';

export function HomeScreen() {
  const navigation = useNavigation<BottomTabNavigationProp<CustomerTabParamList>>();

  return (
    <ScrollView style={styles.page} contentContainerStyle={styles.content}>
      <ImageBackground
        source={{ uri: 'https://images.unsplash.com/photo-1414235077428-338989a2e8c0?w=1400' }}
        style={styles.hero}
        imageStyle={styles.heroImage}
      >
        <View style={styles.heroOverlay}>
          <Text style={styles.heroTitle}>RestERP Restaurant</Text>
          <Divider />
          <Text style={styles.heroSubtitle}>Modern Lezzetler, Eşsiz Deneyim</Text>
          <View style={styles.heroButtons}>
            <PrimaryButton title="Menüye Gözat" onPress={() => navigation.navigate('Menu')} />
            <View style={{ height: 10 }} />
            <PrimaryButton
              title="Rezervasyon Yap"
              variant="secondary"
              onPress={() => navigation.navigate('Reservation')}
            />
          </View>
        </View>
      </ImageBackground>

      <View style={styles.welcome}>
        <Text style={styles.welcomeTitle}>Hoş Geldiniz</Text>
        <Divider />
        <Text style={styles.welcomeText}>
          RestERP Restaurant, modern mutfağın en seçkin lezzetlerini geleneksel tatlarla
          buluşturan özel bir mekandır. Şeflerimiz tarafından özenle hazırlanan menümüz,
          taze ve kaliteli malzemelerle mevsimsel olarak güncellenmektedir.
        </Text>
        <Text style={styles.welcomeText}>
          Sıcak ve şık atmosferimizde, dostlarınızla keyifli anlar yaşamanız için tüm
          detayları düşündük. Profesyonel ekibimiz, size unutulmaz bir yemek deneyimi
          sunmak için hizmetinizdedir.
        </Text>
      </View>

      <View style={styles.footer}>
        <Text style={styles.footerTitle}>RestERP Restaurant</Text>
        <Text style={styles.footerMark}>❦</Text>
        <Text style={styles.footerText}>Türk ve dünya mutfağından eşsiz lezzetler sunan restoranımıza hoş geldiniz.</Text>
        <Text style={styles.footerText}>Adres: İstanbul, Türkiye</Text>
        <Text style={styles.footerText}>Telefon: +90 555 123 4567</Text>
        <Text style={styles.footerText}>E-posta: info@resterp.com</Text>
        <Text style={styles.footerCopy}>© 2026 - RestERP Modern bir Restoran ERP uygulamasıdır.</Text>
      </View>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  page: {
    flex: 1,
    backgroundColor: colors.cream,
  },
  content: {
    paddingBottom: 32,
  },
  hero: {
    minHeight: 420,
    justifyContent: 'center',
  },
  heroImage: {
    resizeMode: 'cover',
  },
  heroOverlay: {
    backgroundColor: 'rgba(0,0,0,0.45)',
    paddingHorizontal: 24,
    paddingVertical: 48,
  },
  heroTitle: {
    color: colors.white,
    fontSize: 34,
    fontFamily: 'Lora_700Bold',
    textAlign: 'center',
    textShadowColor: 'rgba(0,0,0,0.8)',
    textShadowOffset: { width: 2, height: 2 },
    textShadowRadius: 8,
  },
  heroSubtitle: {
    color: colors.white,
    fontSize: 20,
    fontFamily: 'Lora_400Regular',
    textAlign: 'center',
    marginBottom: 20,
  },
  heroButtons: {
    marginTop: 8,
  },
  welcome: {
    padding: 24,
  },
  welcomeTitle: {
    fontSize: 26,
    color: colors.primary,
    fontFamily: 'Lora_600SemiBold',
    textAlign: 'center',
  },
  welcomeText: {
    color: colors.text,
    fontSize: 15,
    lineHeight: 24,
    marginBottom: 12,
    textAlign: 'center',
  },
  footer: {
    marginHorizontal: 16,
    backgroundColor: colors.creamCard,
    borderRadius: 12,
    borderWidth: 1,
    borderColor: colors.border,
    padding: 20,
    alignItems: 'center',
  },
  footerTitle: {
    color: colors.primaryMid,
    fontFamily: 'Lora_600SemiBold',
    fontSize: 18,
  },
  footerMark: {
    color: colors.accent,
    marginVertical: 6,
  },
  footerText: {
    color: colors.textMuted,
    textAlign: 'center',
    marginBottom: 4,
  },
  footerCopy: {
    color: colors.textMuted,
    marginTop: 10,
    fontSize: 12,
    textAlign: 'center',
  },
});
