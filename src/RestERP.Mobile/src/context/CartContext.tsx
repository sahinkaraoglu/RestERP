import React, { createContext, useContext, useMemo, useState } from 'react';
import type { CartItem, Food } from '../types';

interface CartContextValue {
  items: CartItem[];
  note: string;
  tableNumber: string;
  setNote: (value: string) => void;
  setTableNumber: (value: string) => void;
  add: (food: Food) => void;
  remove: (foodId: number) => void;
  setQuantity: (foodId: number, quantity: number) => void;
  clear: () => void;
  total: number;
  count: number;
}

const CartContext = createContext<CartContextValue | undefined>(undefined);

export function CartProvider({ children }: { children: React.ReactNode }) {
  const [items, setItems] = useState<CartItem[]>([]);
  const [note, setNote] = useState('');
  const [tableNumber, setTableNumber] = useState('');

  const value = useMemo<CartContextValue>(() => {
    const add = (food: Food) => {
      setItems((current) => {
        const existing = current.find((item) => item.foodId === food.id);
        if (existing) {
          return current.map((item) =>
            item.foodId === food.id ? { ...item, quantity: item.quantity + 1 } : item,
          );
        }
        return [
          ...current,
          {
            foodId: food.id,
            name: food.turkishName || food.name,
            quantity: 1,
            price: food.price,
          },
        ];
      });
    };

    const remove = (foodId: number) => {
      setItems((current) =>
        current
          .map((item) =>
            item.foodId === foodId ? { ...item, quantity: item.quantity - 1 } : item,
          )
          .filter((item) => item.quantity > 0),
      );
    };

    const setQuantity = (foodId: number, quantity: number) => {
      setItems((current) => {
        if (quantity <= 0) {
          return current.filter((item) => item.foodId !== foodId);
        }
        return current.map((item) =>
          item.foodId === foodId ? { ...item, quantity } : item,
        );
      });
    };

    const total = items.reduce((sum, item) => sum + item.price * item.quantity, 0);
    const count = items.reduce((sum, item) => sum + item.quantity, 0);

    return {
      items,
      note,
      tableNumber,
      setNote,
      setTableNumber,
      add,
      remove,
      setQuantity,
      clear: () => {
        setItems([]);
        setNote('');
      },
      total,
      count,
    };
  }, [items, note, tableNumber]);

  return <CartContext.Provider value={value}>{children}</CartContext.Provider>;
}

export function useCart() {
  const context = useContext(CartContext);
  if (!context) {
    throw new Error('useCart must be used within CartProvider');
  }
  return context;
}
