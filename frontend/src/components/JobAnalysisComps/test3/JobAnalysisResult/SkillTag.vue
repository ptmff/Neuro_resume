<script setup lang="ts">
defineProps<{ label: string }>()
</script>

<template>
  <span class="px-3 py-2 rounded-full text-white text-sm font-semibold 
  border-[var(--gradient-danger)] shadow-[0_0_8px_2px_rgba(239,83,80,0.8)] 
  hover:scale-105 transition"
  style="background: var(--gradient-danger)"
  >
    {{ label }}
  </span>
</template>
