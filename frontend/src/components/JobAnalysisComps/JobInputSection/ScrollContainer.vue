<template>
  <div
    ref="container"
    class="absolute inset-0 z-20 overflow-y-scroll no-scrollbar"
    @scroll="handleScroll"
    :class="{ 'pointer-events-auto': hasInteracted, 'pointer-events-none': !hasInteracted }"
  >
    <!-- 🔷 Блюр-фон при скролле -->
    <div
      class="fixed inset-0 z-[-1] transition duration-300 pointer-events-none"
      :class="{ 'backdrop-blur-md bg-[var(--shadow)]': scrolledEnough }"
    ></div>

    <!-- 🧱 Заглушка для прокрутки -->
    <div class="h-screen pointer-events-none"></div>

    <!-- Контейнер контента -->
    <section class="relative z-10 w-full min-h-screen flex flex-col items-center">
      <!-- Центрированный input -->
      <div class="flex-1 w-full flex items-center justify-center px-4">
        <JobInputPanel @start="handleStart" />
      </div>

      <!-- Футер -->
      <FooterOverlay />
    </section>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import JobInputPanel from './JobInputPanel.vue'
import FooterOverlay from './FooterOverlay.vue'

const emit = defineEmits(['scrolled', 'start'])

const container = ref<HTMLElement | null>(null)
const hasInteracted = ref(false)
const scrolledEnough = ref(false)

function handleScroll() {
  const el = container.value
  if (!el) return

  const max = el.scrollHeight - el.clientHeight
  const current = el.scrollTop

  if (!hasInteracted.value && current > 10) {
    hasInteracted.value = true
    emit('scrolled')
  }

  // 🎯 Блюр при пересечении 10% экрана
  scrolledEnough.value = current > window.innerHeight * 0.1
}

function handleFirstInteraction(event: Event) {
  if (!hasInteracted.value) {
    hasInteracted.value = true
  }
}

function handleStart(url: string) {
  emit('start', url)
}

onMounted(() => {
  window.addEventListener('wheel', handleFirstInteraction, { passive: true })
  window.addEventListener('touchstart', handleFirstInteraction, { passive: true }) // Добавили!
})

onUnmounted(() => {
  window.removeEventListener('wheel', handleFirstInteraction)
  window.removeEventListener('touchstart', handleFirstInteraction) // И сняли!
})

</script>

<style scoped>
.no-scrollbar::-webkit-scrollbar {
  display: none;
}
.no-scrollbar {
  -ms-overflow-style: none;
  scrollbar-width: none;
}
</style>