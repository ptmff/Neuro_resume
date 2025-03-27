<script setup lang="ts">
import Parallax from '@/components/Parallax.vue'
import NavBar from './components/NavBar.vue'
import Footer from './components/Footer.vue'
import { RouterView, useRoute } from 'vue-router'
import { ref, computed, onMounted } from 'vue'
import { useAppStore } from '@/stores/appStore'

const phase = ref('input')
const setPhase = (value: string) => {
  phase.value = value
}

const route = useRoute()
const appStore = useAppStore()

// Запускаем инициализацию при монтировании
onMounted(async () => {
  await appStore.initialize()
})

const isLoading = computed(() => appStore.isAppLoading)
</script>

<template>
  <!-- 🌀 Прелоадер на весь экран -->
  <div
    v-if="isLoading"
    class="fixed inset-0 flex items-center justify-center z-50 bg-[var(--background-main)]"
  >
    <div class="animate-spin rounded-full h-16 w-16 border-t-4 border-[var(--text-light)]"></div>
  </div>

  <!-- Основной сайт отрисовывается только после загрузки -->
  <template v-else>
    <div class="relative min-h-screen text-[var(--text-light)] gradient-page">
      <div class="absolute inset-0 -z-10 animated-bg"></div>
      <Parallax class="absolute inset-0" />

      <NavBar/>

      <RouterView v-slot="{ Component, route }">
        <transition mode="out-in">
          <component
            :is="Component"
            v-bind="route.path === '/analyse' ? { phase, setPhase } : {}"
          />
        </transition>
      </RouterView>

      <Footer
        v-if="!(route.path === '/test2')"
      />
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
