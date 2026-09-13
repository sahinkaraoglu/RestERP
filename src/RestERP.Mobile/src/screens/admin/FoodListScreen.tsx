import React, { useCallback, useMemo, useState } from 'react';
import { Alert, Pressable, ScrollView, StyleSheet, Text, TextInput, View } from 'react-native';
import { useFocusEffect, useNavigation } from '@react-navigation/native';
import type { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { foodApi } from '../../api/services';
import { Card, EmptyState, Loader, PrimaryButton } from '../../components/ui';
import { colors } from '../../theme/colors';
import { formatMoney } from '../../utils/format';
import type { Food, FoodCategory } from '../../types';
import type { AdminStackParamList } from '../../navigation/types';

export function FoodListScreen() {
  const navigation = useNavigation<NativeStackNavigationProp<AdminStackParamList>>();
  const [foods, setFoods] = useState<Food[]>([]);
  const [categories, setCategories] = useState<FoodCategory[]>([]);
  const [query, setQuery] = useState('');
  const [categoryId, setCategoryId] = useState<number | null>(null);
  const [loading, setLoading] = useState(true);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const [foodData, categoryData] = await Promise.all([foodApi.list(), foodApi.categories()]);
      setFoods(foodData ?? []);
      setCategories(categoryData ?? []);
    } catch (error) {
      Alert.alert('Menü yüklenemedi', error instanceof Error ? error.message : 'API hatası');
    } finally {
      setLoading(false);
    }
  }, []);

  useFocusEffect(
    useCallback(() => {
      load();
    }, [load]),
  );

  const filtered = useMemo(() => {
    return foods.filter((food) => {
      const matchesCategory = categoryId ? food.categoryId === categoryId : true;
      const haystack = `${food.turkishName} ${food.name} ${food.description ?? ''}`.toLowerCase();
      return matchesCategory && haystack.includes(query.toLowerCase());
    });
  }, [foods, categoryId, query]);

  const remove = (food: Food) => {
    Alert.alert('Ürünü sil', `${food.turkishName || food.name} silinsin mi?`, [
      { text: 'Vazgeç' },
      {
        text: 'Sil',
        style: 'destructive',
        onPress: async () => {
          try {
            await foodApi.remove(food.id);
            setFoods((current) => current.filter((item) => item.id !== food.id));
          } catch (error) {
            Alert.alert('Silinemedi', error instanceof Error ? error.message : 'API hatası');
          }
        },
      },
    ]);
  };

  if (loading) {
    return <Loader />;
  }

  return (
    <ScrollView style={styles.page} contentContainerStyle={styles.content}>
      <PrimaryButton title="Yeni Ürün" onPress={() => navigation.navigate('AdminFoodForm', {})} />
      <TextInput
        style={styles.search}
        placeholder="Ürün veya açıklama ara"
        placeholderTextColor={colors.textMuted}
        value={query}
        onChangeText={setQuery}
      />
      <ScrollView horizontal showsHorizontalScrollIndicator={false}>
        <Pressable style={[styles.chip, !categoryId && styles.chipActive]} onPress={() => setCategoryId(null)}>
          <Text style={[styles.chipText, !categoryId && styles.chipTextActive]}>Tümü</Text>
        </Pressable>
        {categories.map((category) => (
          <Pressable
            key={category.id}
            style={[styles.chip, categoryId === category.id && styles.chipActive]}
            onPress={() => setCategoryId(category.id)}
          >
            <Text style={[styles.chipText, categoryId === category.id && styles.chipTextActive]}>
              {category.turkishName || category.name}
            </Text>
          </Pressable>
        ))}
      </ScrollView>
      {filtered.length === 0 ? <EmptyState title="Ürün bulunamadı" /> : null}
      {filtered.map((food) => (
        <Card key={food.id}>
          <Text style={styles.name}>{food.turkishName || food.name}</Text>
          <Text style={styles.meta}>{food.name} · {formatMoney(food.price)}</Text>
          <View style={styles.actions}>
            <View style={{ flex: 1 }}>
              <PrimaryButton title="Düzenle" variant="secondary" onPress={() => navigation.navigate('AdminFoodForm', { foodId: food.id })} />
            </View>
            <View style={{ flex: 1 }}>
              <PrimaryButton title="Sil" variant="danger" onPress={() => remove(food)} />
            </View>
          </View>
        </Card>
      ))}
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  page: { flex: 1, backgroundColor: colors.cream },
  content: { padding: 16, gap: 12, paddingBottom: 32 },
  search: {
    backgroundColor: colors.white,
    borderWidth: 1,
    borderColor: colors.borderStrong,
    borderRadius: 8,
    paddingHorizontal: 12,
    paddingVertical: 10,
    color: colors.text,
  },
  chip: {
    borderWidth: 1,
    borderColor: colors.borderStrong,
    backgroundColor: colors.creamInput,
    borderRadius: 20,
    paddingHorizontal: 12,
    paddingVertical: 8,
    marginRight: 8,
  },
  chipActive: { backgroundColor: colors.primary, borderColor: colors.primary },
  chipText: { color: colors.primaryMid },
  chipTextActive: { color: colors.white },
  name: { color: colors.primary, fontWeight: '700', fontSize: 16 },
  meta: { color: colors.textMuted, marginVertical: 6 },
  actions: { flexDirection: 'row', gap: 8, marginTop: 8 },
});
