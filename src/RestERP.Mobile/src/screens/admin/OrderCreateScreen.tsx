import React, { useCallback, useEffect, useMemo, useState } from 'react';
import { Alert, Pressable, ScrollView, StyleSheet, Text, View } from 'react-native';
import { useNavigation, useRoute, type RouteProp } from '@react-navigation/native';
import { foodApi, orderApi, tableApi } from '../../api/services';
import { EmptyState, Loader, PrimaryButton } from '../../components/ui';
import { colors } from '../../theme/colors';
import { formatMoney } from '../../utils/format';
import { OrderStatus, type CartItem, type Food, type FoodCategory } from '../../types';
import type { AdminStackParamList } from '../../navigation/types';

export function OrderCreateScreen() {
  const navigation = useNavigation();
  const route = useRoute<RouteProp<AdminStackParamList, 'AdminOrderCreate'>>();
  const tableId = route.params.tableId;
  const [foods, setFoods] = useState<Food[]>([]);
  const [categories, setCategories] = useState<FoodCategory[]>([]);
  const [items, setItems] = useState<CartItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    (async () => {
      try {
        const [foodData, categoryData] = await Promise.all([foodApi.list(), foodApi.categories()]);
        setFoods(foodData ?? []);
        setCategories(categoryData ?? []);
      } catch (error) {
        Alert.alert('Menü yüklenemedi', error instanceof Error ? error.message : 'API hatası');
      } finally {
        setLoading(false);
      }
    })();
  }, []);

  const add = useCallback((food: Food) => {
    setItems((current) => {
      const existing = current.find((item) => item.foodId === food.id);
      if (existing) {
        return current.map((item) =>
          item.foodId === food.id ? { ...item, quantity: item.quantity + 1 } : item,
        );
      }
      return [...current, { foodId: food.id, name: food.turkishName || food.name, quantity: 1, price: food.price }];
    });
  }, []);

  const remove = (foodId: number) => {
    setItems((current) =>
      current
        .map((item) => (item.foodId === foodId ? { ...item, quantity: item.quantity - 1 } : item))
        .filter((item) => item.quantity > 0),
    );
  };

  const total = useMemo(() => items.reduce((sum, item) => sum + item.price * item.quantity, 0), [items]);

  const submit = async () => {
    if (items.length === 0) {
      Alert.alert('Sepet boş', 'En az bir ürün ekleyin.');
      return;
    }
    setSubmitting(true);
    try {
      await orderApi.create({
        tableId,
        status: OrderStatus.New,
        totalAmount: total,
        isPaid: false,
        orderItems: items.map((item) => ({
          foodId: item.foodId,
          quantity: item.quantity,
          unitPrice: item.price,
          totalPrice: item.price * item.quantity,
          status: OrderStatus.New,
          isPaid: false,
        })),
      });
      await tableApi.setOccupied(tableId, true).catch(() => undefined);
      Alert.alert('Sipariş alındı', 'Personel siparişi oluşturuldu.');
      navigation.goBack();
    } catch (error) {
      Alert.alert('Sipariş hatası', error instanceof Error ? error.message : 'API hatası');
    } finally {
      setSubmitting(false);
    }
  };

  if (loading) {
    return <Loader />;
  }

  return (
    <View style={styles.page}>
      <ScrollView contentContainerStyle={styles.content}>
        <Text style={styles.title}>Masa {tableId} için sipariş</Text>
        {categories.map((category) => {
          const categoryFoods = foods.filter((food) => food.categoryId === category.id);
          if (categoryFoods.length === 0) {
            return null;
          }
          return (
            <View key={category.id} style={styles.category}>
              <Text style={styles.categoryTitle}>{category.turkishName || category.name}</Text>
              {categoryFoods.map((food) => {
                const qty = items.find((item) => item.foodId === food.id)?.quantity ?? 0;
                return (
                  <View key={food.id} style={styles.row}>
                    <View style={{ flex: 1 }}>
                      <Text style={styles.name}>{food.turkishName || food.name}</Text>
                      <Text style={styles.price}>{formatMoney(food.price)}</Text>
                    </View>
                    <View style={styles.stepper}>
                      <Pressable style={styles.stepBtn} onPress={() => remove(food.id)}>
                        <Text style={styles.stepText}>−</Text>
                      </Pressable>
                      <Text style={styles.qty}>{qty}</Text>
                      <Pressable style={styles.stepBtn} onPress={() => add(food)}>
                        <Text style={styles.stepText}>+</Text>
                      </Pressable>
                    </View>
                  </View>
                );
              })}
            </View>
          );
        })}
        {foods.length === 0 ? <EmptyState title="Menü boş" /> : null}
      </ScrollView>
      <View style={styles.footer}>
        <Text style={styles.total}>{items.length} ürün · {formatMoney(total)}</Text>
        <PrimaryButton title="Sipariş Oluştur" onPress={submit} loading={submitting} />
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  page: { flex: 1, backgroundColor: colors.cream },
  content: { padding: 16, paddingBottom: 24 },
  title: { color: colors.primary, fontFamily: 'Lora_600SemiBold', fontSize: 22, marginBottom: 12 },
  category: {
    backgroundColor: colors.white,
    borderRadius: 12,
    padding: 12,
    marginBottom: 12,
    borderWidth: 1,
    borderColor: colors.border,
  },
  categoryTitle: { color: colors.primary, fontWeight: '700', marginBottom: 8 },
  row: { flexDirection: 'row', alignItems: 'center', paddingVertical: 8, borderTopWidth: 1, borderTopColor: colors.border },
  name: { color: colors.primary, fontWeight: '600' },
  price: { color: colors.accent },
  stepper: { flexDirection: 'row', alignItems: 'center', gap: 8 },
  stepBtn: {
    width: 28,
    height: 28,
    borderRadius: 6,
    backgroundColor: colors.creamSoft,
    borderWidth: 1,
    borderColor: colors.borderStrong,
    alignItems: 'center',
    justifyContent: 'center',
  },
  stepText: { color: colors.primary, fontWeight: '700' },
  qty: { minWidth: 16, textAlign: 'center', color: colors.primary, fontWeight: '700' },
  footer: {
    padding: 14,
    borderTopWidth: 1,
    borderTopColor: colors.border,
    backgroundColor: colors.creamCard,
    gap: 8,
  },
  total: { color: colors.primaryMid, fontWeight: '600' },
});
