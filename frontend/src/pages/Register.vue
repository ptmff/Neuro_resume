<script setup lang="ts">
import RegisterForm from '@/components/RegisterForm.vue'
</script>

<template>
  <div class="relative min-h-screen w-full bg-gray-600/20 overflow-hidden">
    <!-- фон, если нужен -->
    <div class="absolute inset-0 z-0 opacity-40">
      <!-- background layers -->
    </div>

    <!-- форма поверх всего -->
    <RegisterForm />
  </div>
</template>
