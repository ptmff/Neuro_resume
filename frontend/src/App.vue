<script setup>
import Parallax from '@/components/Parallax.vue'
import NavBar from './components/NavBar.vue'
import Footer from './components/Footer.vue'
import { RouterView, useRoute } from 'vue-router'
import { ref, computed } from 'vue'

const phase = ref('input')
const setPhase = value => {
  phase.value = value
}

const route = useRoute()
</script>

<template>
  <div class="relative min-h-screen text-[var(--text-light)] gradient-page">
    <div class="absolute inset-0 -z-10 animated-bg"></div>

    <Parallax class="absolute inset-0" />
    <NavBar v-if="phase !== 'visualizing'" />

    <RouterView v-slot="{ Component, route }">
      <transition mode="out-in">
        <component :is="Component" v-bind="route.path === '/analyse' ? { phase, setPhase } : {}" />
      </transition>
    </RouterView>

    <Footer v-if="!(route.path === '/analyse' && (phase === 'visualizing' || phase === 'input'))" />
  </div>
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
