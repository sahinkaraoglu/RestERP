import React, { useCallback, useEffect, useMemo, useState } from 'react';
import {
  Alert,
  Image,
  Pressable,
  ScrollView,
  StyleSheet,
  Text,
  TextInput,
  View,
} from 'react-native';
import { useNavigation } from '@react-navigation/native';
import type { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { foodApi, orderApi, userApi } from '../api/services';
import { resolveImageUrl } from '../config';
import { EmptyState, Loader, PrimaryButton } from '../components/ui';
import { useAuth } from '../context/AuthContext';
import { useCart } from '../context/CartContext';
import { colors } from '../theme/colors';
import { formatMoney } from '../utils/format';
import { OrderStatus, type Food, type FoodCategory, type FoodImage } from '../types';
import type { RootStackParamList } from '../navigation/types';

export function MenuScreen() {
  const navigation = useNavigation<NativeStackNavigationProp<RootStackParamList>>();
  const { isAuthenticated, user } = useAuth();
  const cart = useCart();
  const [foods, setFoods] = useState<Food[]>([]);
  const [categories, setCategories] = useState<FoodCategory[]>([]);
  const [images, setImages] = useState<FoodImage[]>([]);
  const [openCategories, setOpenCategories] = useState<number[]>([]);
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const [foodData, categoryData, imageData] = await Promise.all([
        foodApi.list(),
        foodApi.categories(),
        foodApi.images().catch(() => [] as FoodImage[]),
      ]);
      setFoods(foodData ?? []);
      setCategories(categoryData ?? []);
      setImages(imageData ?? []);
      setOpenCategories((categoryData ?? []).slice(0, 1).map((c) => c.id));
    } catch (error) {
      Alert.alert('Menü yüklenemedi', error instanceof Error ? error.message : 'API hatası');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    load();
  }, [load]);

  const foodsByCategory = useMemo(() => {
    return categories.map((category) => ({
      category,
      items: foods.filter((food) => food.categoryId === category.id && !food.isDeleted),
    }));
  }, [categories, foods]);

  const imageFor = (food: Food) => {
    const nested = food.images?.[0]?.path;
    const fallback = images.find((image) => image.foodId === food.id)?.path;
    return resolveImageUrl(nested || fallback);
  };

  const submitOrder = async () => {
    if (!isAuthenticated) {
      Alert.alert('Giriş gerekli', 'Sipariş verebilmek için giriş yapmalısınız.', [
        { text: 'Vazgeç' },
        { text: 'Giriş Yap', onPress: () => navigation.navigate('Login') },
      ]);
      return;
    }
    const tableId = Number(cart.tableNumber);
    if (!tableId || tableId < 1) {
      Alert.alert('Masa numarası', 'Lütfen geçerli bir masa numarası girin.');
      return;
    }
    if (cart.items.length === 0) {
      Alert.alert('Sepet boş', 'Sipariş vermek için ürün ekleyin.');
      return;
    }

    setSubmitting(true);
    try {
      let customerId = user?.id;
      if (user?.email) {
        try {
          const profile = await userApi.byEmail(user.email);
          customerId = profile.id;
        } catch {
          customerId = user.id;
        }
      }

      await orderApi.create({
        tableId,
        customerId,
        status: OrderStatus.New,
        totalAmount: cart.total,
        isPaid: false,
        orderItems: cart.items.map((item) => ({
          foodId: item.foodId,
          quantity: item.quantity,
          unitPrice: item.price,
          totalPrice: item.price * item.quantity,
          status: OrderStatus.New,
          isPaid: false,
        })),
      });
      cart.clear();
      Alert.alert('Sipariş alındı', 'Siparişiniz başarıyla oluşturuldu.');
      navigation.navigate('Main', { screen: 'Orders' });
    } catch (error) {
      Alert.alert('Sipariş hatası', error instanceof Error ? error.message : 'Sipariş oluşturulamadı');
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
        {foodsByCategory.map(({ category, items }) => {
          const open = openCategories.includes(category.id);
          return (
            <View key={category.id} style={styles.category}>
              <Pressable
                style={styles.categoryHeader}
                onPress={() =>
                  setOpenCategories((current) =>
                    current.includes(category.id)
                      ? current.filter((id) => id !== category.id)
                      : [...current, category.id],
                  )
                }
              >
                <Text style={styles.categoryTitle}>{category.turkishName || category.name}</Text>
                <Text style={styles.categoryToggle}>{open ? '−' : '+'}</Text>
              </Pressable>
              {open
                ? items.map((food) => {
                    const qty = cart.items.find((item) => item.foodId === food.id)?.quantity ?? 0;
                    const uri = imageFor(food);
                    return (
                      <View key={food.id} style={styles.foodRow}>
                        {uri ? <Image source={{ uri }} style={styles.foodImage} /> : <View style={styles.foodImage} />}
                        <View style={styles.foodInfo}>
                          <Text style={styles.foodName}>{food.turkishName || food.name}</Text>
                          {food.description ? <Text style={styles.foodDesc}>{food.description}</Text> : null}
                          <Text style={styles.foodPrice}>{formatMoney(food.price)}</Text>
                        </View>
                        <View style={styles.stepper}>
                          <Pressable style={styles.stepBtn} onPress={() => cart.remove(food.id)}>
                            <Text style={styles.stepText}>−</Text>
                          </Pressable>
                          <Text style={styles.qty}>{qty}</Text>
                          <Pressable style={styles.stepBtn} onPress={() => cart.add(food)}>
                            <Text style={styles.stepText}>+</Text>
                          </Pressable>
                        </View>
                      </View>
                    );
                  })
                : null}
            </View>
          );
        })}
        {foods.length === 0 ? <EmptyState title="Menü boş" subtitle="API'den ürün alınamadı." /> : null}
      </ScrollView>

      <View style={styles.cart}>
        <Text style={styles.cartTitle}>Sipariş Özeti</Text>
        <Text style={styles.cartMeta}>{cart.count} ürün · {formatMoney(cart.total)}</Text>
        <TextInput
          style={styles.input}
          placeholder="Masa numarası"
          placeholderTextColor={colors.textMuted}
          keyboardType="number-pad"
          value={cart.tableNumber}
          onChangeText={cart.setTableNumber}
        />
        <TextInput
          style={[styles.input, styles.note]}
          placeholder="Sipariş notu"
          placeholderTextColor={colors.textMuted}
          value={cart.note}
          onChangeText={cart.setNote}
          multiline
        />
        <PrimaryButton title="Sipariş Ver" onPress={submitOrder} loading={submitting} disabled={cart.items.length === 0} />
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  page: {
    flex: 1,
    backgroundColor: colors.cream,
  },
  content: {
    padding: 12,
    paddingBottom: 24,
  },
  category: {
    backgroundColor: colors.white,
    borderRadius: 12,
    marginBottom: 12,
    overflow: 'hidden',
    borderWidth: 1,
    borderColor: colors.border,
  },
  categoryHeader: {
    padding: 14,
    backgroundColor: colors.creamSoft,
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
  categoryTitle: {
    color: colors.primary,
    fontFamily: 'Lora_600SemiBold',
    fontSize: 18,
  },
  categoryToggle: {
    color: colors.accent,
    fontSize: 22,
  },
  foodRow: {
    flexDirection: 'row',
    padding: 12,
    gap: 10,
    borderTopWidth: 1,
    borderTopColor: colors.border,
    alignItems: 'center',
  },
  foodImage: {
    width: 64,
    height: 64,
    borderRadius: 8,
    backgroundColor: colors.creamSoft,
  },
  foodInfo: {
    flex: 1,
  },
  foodName: {
    color: colors.primary,
    fontWeight: '600',
  },
  foodDesc: {
    color: colors.textMuted,
    fontSize: 12,
    marginTop: 2,
  },
  foodPrice: {
    color: colors.accent,
    marginTop: 4,
    fontWeight: '700',
  },
  stepper: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 8,
  },
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
  stepText: {
    color: colors.primary,
    fontSize: 16,
    fontWeight: '700',
  },
  qty: {
    minWidth: 16,
    textAlign: 'center',
    color: colors.primary,
    fontWeight: '700',
  },
  cart: {
    backgroundColor: colors.creamCard,
    borderTopWidth: 1,
    borderTopColor: colors.border,
    padding: 14,
    gap: 8,
  },
  cartTitle: {
    color: colors.primary,
    fontFamily: 'Lora_600SemiBold',
    fontSize: 16,
  },
  cartMeta: {
    color: colors.textSoft,
    marginBottom: 4,
  },
  input: {
    backgroundColor: colors.white,
    borderWidth: 1,
    borderColor: colors.borderStrong,
    borderRadius: 8,
    paddingHorizontal: 12,
    paddingVertical: 10,
    color: colors.text,
  },
  note: {
    minHeight: 60,
    textAlignVertical: 'top',
  },
});
