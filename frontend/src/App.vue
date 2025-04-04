<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { useAppStore } from '@/stores/appStore'

import NavBar from '@/components/NavBar.vue'
import Footer from '@/components/Footer.vue'
import Parallax from '@/components/Parallax.vue'

import { RouterView } from 'vue-router'

const route = useRoute()
const appStore = useAppStore()

const phase = ref('input')
const setPhase = (value: string) => {
  phase.value = value
}

const hideLayout = computed(() =>
  route.path === '/login' || route.path === '/register'
)

// Инициализация при загрузке приложения
onMounted(async () => {
  await appStore.initialize()
})

const isLoading = computed(() => appStore.isAppLoading)
</script>

<template>
  <!-- 🌀 Прелоадер, пока грузится профиль и резюме -->
  <div
    v-if="isLoading"
    class="fixed inset-0 flex items-center justify-center z-50 bg-[var(--background-main)]"
  >
    <div class="animate-spin rounded-full h-16 w-16 border-t-4 border-[var(--text-light)]"></div>
  </div>

  <!-- Основной сайт -->
  <template v-else>
  <div class="relative min-h-screen text-[var(--text-light)] gradient-page">
    <div class="absolute inset-0 -z-10 animated-bg"></div>
    <Parallax class="absolute inset-0" />
    <NavBar v-if="!hideLayout" />
    <RouterView />
    <Footer v-if="!hideLayout" />
  </div>
</template>

</template>

<style scoped>
.v-enter-active,
.v-leave-active {
  transition: opacity 0.3s ease;
}
.v-enter-from,
.v-leave-to {
  opacity: 0;
}
</style>
