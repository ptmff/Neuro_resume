<template>
    <div class="p-6 text-[var(--text-light)] space-y-6">
      <h2 class="text-2xl font-bold mb-4">Платёжные настройки</h2>
  
      <!-- Подписка -->
      <div class="bg-[var(--background-overlay)] rounded-xl p-4 border border-[var(--background-pale)]">
        <h3 class="text-xl font-semibold mb-2">Текущая подписка</h3>
        <p class="text-sm text-[var(--text-subdued)]">Вы используете бесплатный план.</p>
        <button
          class="mt-4 px-4 py-2 rounded-full bg-[var(--background-cta)] text-white text-sm hover:bg-[var(--background-cta-hover)] transition"
        >
          Обновить до Premium
        </button>
      </div>
  
      <!-- История платежей -->
      <div class="bg-[var(--background-overlay)] rounded-xl p-4 border border-[var(--background-pale)]">
        <h3 class="text-xl font-semibold mb-2">История платежей</h3>
        <ul class="text-sm space-y-2">
          <li v-for="payment in payments" :key="payment.id" class="flex justify-between border-b border-[var(--background-pale)] pb-1">
            <span>{{ payment.date }}</span>
            <span>{{ payment.amount }} ₽</span>
          </li>
          <li v-if="payments.length === 0" class="text-[var(--text-mainless)] italic">Нет записей об оплате.</li>
        </ul>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref } from 'vue'
  
  const payments = ref([
    // Мок-данные
    { id: 1, date: '2024-12-01', amount: 990 },
    { id: 2, date: '2025-01-01', amount: 990 },
    { id: 3, date: '2025-02-01', amount: 990 },
  ])
  </script>