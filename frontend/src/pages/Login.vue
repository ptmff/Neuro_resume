<template>
    <div class="min-h-screen flex items-center justify-center bg-gradient-to-br from-darkBlue to-darkPurple">
      <div class="bg-white/5 backdrop-blur-lg border border-white/10 p-8 rounded-2xl shadow-xl w-full max-w-md">
        <h2 class="text-3xl font-bold text-center text-white mb-6">Вход в аккаунт</h2>
  
        <form @submit.prevent="handleLogin" class="flex flex-col gap-4">
          <input
            v-model="email"
            type="email"
            placeholder="Email"
            class="input-style"
            required
          />
          <input
            v-model="password"
            type="password"
            placeholder="Пароль"
            class="input-style"
            required
          />
          <button
            type="submit"
            class="bg-gradient-to-r from-pink to-cyan text-white py-2 px-4 rounded-xl hover:brightness-125 transition"
          >
            Войти
          </button>
          <p v-if="error" class="text-red-400 text-sm text-center">{{ error }}</p>
        </form>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref } from 'vue';
  import { useAuthStore } from '@/stores/authStore';
  import { useRouter } from 'vue-router';
  
  const auth = useAuthStore();
  const router = useRouter();
  
  const email = ref('');
  const password = ref('');
  const error = ref('');
  
  const handleLogin = async () => {
    error.value = '';
    const success = await auth.login({ email: email.value, password: password.value });
    if (success) {
      router.push('/profile');
    } else {
      error.value = 'Неверный email или пароль';
    }
  };
  </script>
  
  <style scoped>
  .input-style {
    @apply bg-white/10 text-white placeholder-white/60 border border-white/20 rounded-xl px-4 py-2 focus:outline-none focus:ring-2 focus:ring-pink-500;
  }
  </style>
  