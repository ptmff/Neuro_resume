<template>
    <div
    class="absolute flex items-center justify-center px-4 py-2 rounded-full text-sm font-medium text-[var(--text-light)] backdrop-blur-md transition-all duration-500 animate-fade-up"
    :class="[
        'floating-message',
        type === 'success' ? 'bg-[var(--neon-blue)]/20 border border-[var(--neon-blue)] text-[var(--neon-blue)]' :
        type === 'warning' ? 'bg-yellow-500/20 border border-yellow-500 text-yellow-400' :
        type === 'error' ? 'bg-red-500/20 border border-red-500 text-red-400' :
        'bg-[var(--text-light)]/10 border border-[var(--text-light)] text-[var(--text-light)]'
      ]"
    >
      <slot />
    </div>
  </template>
  
  <script setup lang="ts">
  
  const props = defineProps({
    type: { type: String, default: 'neutral' },
  })
  </script>  
  
  <style scoped>
  @keyframes fade-up {
    0% {
      opacity: 0;
      transform: translateY(10px) scale(0.98);
    }
    100% {
      opacity: 1;
      transform: translateY(0) scale(1);
    }
  }
  
  .animate-fade-up {
    animation: fade-up 0.8s ease-out;
  }
  </style>
  