<template>
  <section class="flex flex-col gap-6">
    <div>
      <h2 class="text-xl sm:text-2xl font-bold text-[var(--text-light)] mb-2">Навыки</h2>
      <p class="text-sm text-w[var(--text-mainless)] mb-4">Твои ключевые компетенции</p>
    </div>

    <div class="flex flex-wrap gap-3">
      <span
        v-for="(skill, index) in skills"
        :key="index"
        class="bg-gradient-to-r from-violet-500 to-cyan-400 text-[var(--text-light)] text-sm px-4 py-1.5 rounded-full shadow-md hover:scale-105 hover:brightness-110 transition-transform cursor-pointer"
      >
        {{ skill }}
      </span>
    </div>
  </section>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useResumeStore } from '@/stores/resumesStore'

const resumeStore = useResumeStore()

const skills = computed(() => resumeStore.mainResume?.skills ?? [])
</script>
