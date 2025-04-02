<script setup lang="ts">
defineProps<{ label: string }>()
</script>

<template>
  <span class="tag-chip">
    {{ label }}
  </span>
</template>

<style scoped>
.tag-chip {
  display: inline-block;
  padding: 0.5rem 0.75rem;
  border-radius: 9999px;
  font-weight: 600;
  font-size: 0.875rem;
  color: white;
  background: var(--gradient-danger);
  border: 1px solid var(--resume-red);
  box-shadow: 0 0 8px 2px rgba(239, 83, 80, 0.8);
  cursor: pointer;
  transition: transform 0.1s ease;
}

.tag-chip:hover {
  transform: scale(1.1);
}
</style>
