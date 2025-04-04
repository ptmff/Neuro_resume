<script setup lang="ts">
defineProps<{ text: string }>()
</script>

<template>
  <div class="bg-[#1a1a1a] border border-white/10 p-4 rounded-xl shadow-md hover:shadow-lg transition text-sm text-white/80">
    <i class="fas fa-lightbulb text-[var(--neon-yellow)] mr-2"></i>
    {{ text }}
  </div>
</template>
