<template>
    <svg class="absolute inset-0 w-full h-full pointer-events-none" viewBox="0 0 1000 1000" preserveAspectRatio="xMidYMid slice">
      <line
        v-for="(link, index) in links"
        :key="index"
        :x1="link.from.x"
        :y1="link.from.y"
        :x2="link.to.x"
        :y2="link.to.y"
        stroke="#8b5cf6"
        stroke-opacity="0.2"
        stroke-width="1"
      />
      <circle
        v-for="(node, index) in nodes"
        :key="index"
        :cx="node.x"
        :cy="node.y"
        r="4"
        :fill="node.color"
        fill-opacity="0.8"
      />
    </svg>
  </template>
  
  <script setup>
  import { ref } from 'vue'
  
  const nodes = ref(
    Array.from({ length: 15 }, () => ({
      x: Math.random() * 1000,
      y: Math.random() * 1000,
      color: ['#8b5cf6', '#06b6d4', '#ec4899'][Math.floor(Math.random() * 3)]
    }))
  )
  
  const links = ref(
    Array.from({ length: 20 }, () => {
      const from = nodes.value[Math.floor(Math.random() * nodes.value.length)]
      const to = nodes.value[Math.floor(Math.random() * nodes.value.length)]
      return { from, to }
    })
  )
  </script>
  
  <style scoped>
  svg {
    z-index: 1;
    filter: blur(0.3px);
    animation: fadeIn 1s ease forwards;
  }
  
  @keyframes fadeIn {
    from {
      opacity: 0;
    }
    to {
      opacity: 1;
    }
  }
  </style>
  