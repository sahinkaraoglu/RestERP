import React, { useEffect, useState } from 'react';
import { Alert, ScrollView, StyleSheet, Text } from 'react-native';
import { useNavigation, useRoute, type RouteProp } from '@react-navigation/native';
import { foodApi } from '../../api/services';
import { Card, Field, Loader, PrimaryButton } from '../../components/ui';
import { colors } from '../../theme/colors';
import type { FoodCategory } from '../../types';
import type { AdminStackParamList } from '../../navigation/types';

export function FoodFormScreen() {
  const navigation = useNavigation();
  const route = useRoute<RouteProp<AdminStackParamList, 'AdminFoodForm'>>();
  const foodId = route.params?.foodId;
  const [categories, setCategories] = useState<FoodCategory[]>([]);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [form, setForm] = useState({
    turkishName: '',
    name: '',
    description: '',
    price: '',
    categoryId: '',
  });

  useEffect(() => {
    (async () => {
      try {
        const categoryData = await foodApi.categories();
        setCategories(categoryData ?? []);
        if (foodId) {
          const food = await foodApi.get(foodId);
          setForm({
            turkishName: food.turkishName ?? '',
            name: food.name ?? '',
            description: food.description ?? '',
            price: String(food.price ?? ''),
            categoryId: String(food.categoryId ?? ''),
          });
        } else if (categoryData?.[0]) {
          setForm((current) => ({ ...current, categoryId: String(categoryData[0].id) }));
        }
      } catch (error) {
        Alert.alert('Form yüklenemedi', error instanceof Error ? error.message : 'API hatası');
      } finally {
        setLoading(false);
      }
    })();
  }, [foodId]);

  const set = (key: keyof typeof form, value: string) => {
    setForm((current) => ({ ...current, [key]: value }));
  };

  const onSave = async () => {
    if (!form.turkishName || !form.name || !form.price || !form.categoryId) {
      Alert.alert('Eksik bilgi', 'Türkçe ad, İngilizce ad, fiyat ve kategori zorunludur.');
      return;
    }
    setSaving(true);
    try {
      const payload = {
        turkishName: form.turkishName,
        name: form.name,
        description: form.description,
        price: Number(form.price),
        categoryId: Number(form.categoryId),
      };
      if (foodId) {
        await foodApi.update(foodId, payload);
      } else {
        await foodApi.create(payload);
      }
      navigation.goBack();
    } catch (error) {
      Alert.alert('Kaydedilemedi', error instanceof Error ? error.message : 'API hatası');
    } finally {
      setSaving(false);
    }
  };

  if (loading) {
    return <Loader />;
  }

  return (
    <ScrollView contentContainerStyle={styles.page}>
      <Card>
        <Text style={styles.title}>{foodId ? 'Ürün Düzenle' : 'Yeni Ürün'}</Text>
        <Field label="Türkçe Ad" value={form.turkishName} onChangeText={(v) => set('turkishName', v)} />
        <Field label="İngilizce Ad" value={form.name} onChangeText={(v) => set('name', v)} />
        <Field label="Açıklama" value={form.description} onChangeText={(v) => set('description', v)} multiline />
        <Field label="Fiyat" value={form.price} onChangeText={(v) => set('price', v)} keyboardType="decimal-pad" />
        <Field
          label={`Kategori ID (${categories.map((c) => `${c.id}:${c.turkishName}`).join(', ')})`}
          value={form.categoryId}
          onChangeText={(v) => set('categoryId', v)}
          keyboardType="number-pad"
        />
        <PrimaryButton title="Kaydet" onPress={onSave} loading={saving} />
      </Card>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  page: {
    flexGrow: 1,
    backgroundColor: colors.cream,
    padding: 16,
  },
  title: {
    color: colors.primary,
    fontFamily: 'Lora_600SemiBold',
    fontSize: 22,
    marginBottom: 12,
    textAlign: 'center',
  },
});
