<script setup lang="ts">
defineProps<{
  id: string
  title: string
  description: string
  status: 'pending' | 'active' | 'done' | 'skipped'
  progress: number
}>()
</script>

<template>
  <div
    class="relative bg-[#111] p-4 rounded-xl border border-white/10 shadow-md transition-all duration-500"
    :class="{
      'ring-2 ring-neon-blue scale-[1.015]': status === 'active',
      'opacity-50': status === 'skipped'
    }"
    :data-phase="id"
  >
    <!-- Заголовок и описание -->
    <div class="text-sm font-semibold text-white/80">{{ title }}</div>
    <div class="text-xs text-white/50">{{ description }}</div>

    <!-- Прогресс-бар -->
    <div class="h-2 mt-3 bg-white/10 rounded-full overflow-hidden">
      <div
        class="h-full transition-all duration-300"
        :class="{
          'bg-neon-blue animate-pulse': status === 'active',
          'bg-done': status === 'done',
          'bg-skipped': status === 'skipped',
          'bg-white/20': status === 'pending'
        }"
        :style="{ width: progress + '%' }"
      ></div>
    </div>

    <!-- Статус -->
    <div class="absolute top-2 right-4 text-xs text-white/60 uppercase tracking-wide">
      {{ status }}
    </div>
  </div>
</template>

<style scoped>
.bg-neon-blue {
  background: var(--neon-blue);
}
.bg-done {
  background: var(--gradient-primary)
}
.bg-skipped {
  background: var(--gradient-danger)
}
</style>
