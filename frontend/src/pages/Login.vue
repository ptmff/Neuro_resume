<script setup lang="ts">
import LoginForm from '@/components/LoginForm.vue'
</script>

<template>
  <div class="relative min-h-screen w-full bg-gray-600/20 overflow-hidden">
    <!-- фон или анимации -->
    <div class="absolute inset-0 z-0 opacity-40">
      <!-- background layers, stars, градиенты и т.д. -->
    </div>

    <!-- форма логина над фоном -->
    <LoginForm />
  </div>
</template>
