<template>
    <div class="w-full h-full relative">
      <svg
        class="absolute left-0 top-0 w-[400vw] h-full pointer-events-none z-0"
        viewBox="0 0 4000 1000"
        preserveAspectRatio="none"
      >
        <defs>
          <linearGradient id="signalGradient" x1="0" y1="0" x2="1" y2="0">
            <stop offset="0%" stop-color="var(--neon-purple)" />
            <stop offset="100%" stop-color="var(--neon-blue)" />
          </linearGradient>
          <filter id="glow">
            <feGaussianBlur stdDeviation="4" result="coloredBlur" />
            <feMerge>
              <feMergeNode in="coloredBlur" />
              <feMergeNode in="SourceGraphic" />
            </feMerge>
          </filter>
        </defs>
  
        <!-- Main trunk line -->
        <path
          d="M 100 500 H 3900"
          stroke="url(#signalGradient)"
          stroke-width="3"
          stroke-linecap="round"
          fill="none"
          filter="url(#glow)"
        />
  
        <!-- Dynamic connections between nodes -->
        <template v-for="(node, index) in graphNodes" :key="`connection-${index}`">
          <path
            v-if="node.parentId"
            :d="getBezierPath(node)"
            stroke="url(#signalGradient)"
            stroke-width="2"
            fill="none"
            filter="url(#glow)"
          />
        </template>
      </svg>
  
<!-- Floating messages container -->
<div class="absolute top-0 left-0 h-full w-[400vw] z-10">
  <FloatingMessage
    v-for="(node, index) in graphNodes"
    :key="`node-${index}`"
    :style="getMessagePosition(node)"
    :type="node.type"
  >
    {{ node.label }}
  </FloatingMessage>
</div>

    </div>
  </template>
  
  <script setup lang="ts">
  import FloatingMessage from './SignalPath/FloatingMessage.vue'
  
  interface GraphNode {
    id: string
    parentId?: string
    x: number
    y: number
    label: string
    type: 'success' | 'warning' | 'error' | 'neutral'
  }
  
  const graphNodes: GraphNode[] = [
    { id: 'root', x: 100, y: 500, label: 'Анализ вакансии', type: 'neutral' },
    { id: 'skills', parentId: 'root', x: 600, y: 300, label: 'Навыки', type: 'success' },
    { id: 'experience', parentId: 'root', x: 600, y: 700, label: 'Опыт', type: 'warning' },
    { id: 'soft', parentId: 'skills', x: 1200, y: 240, label: 'Soft Skills', type: 'warning' },
    { id: 'keywords', parentId: 'skills', x: 1200, y: 360, label: 'Ключевые слова', type: 'success' },
    { id: 'gap', parentId: 'experience', x: 1200, y: 640, label: 'Пробелы в опыте', type: 'error' },
    { id: 'match', parentId: 'experience', x: 1200, y: 760, label: 'Совпадение по опыту', type: 'success' },
    { id: 'ai', parentId: 'skills', x: 2000, y: 500, label: 'AI-обработка', type: 'neutral' },
  ]
  
  const getNodeById = (id: string): GraphNode | undefined => {
    return graphNodes.find(n => n.id === id)
  }
  
  const getMessagePosition = (node: GraphNode) => {
  const svgWidth = 4000
  const svgHeight = 1000
  return {
    position: 'absolute',
    left: `${(node.x / svgWidth) * 100}%`,
    top: `${(node.y / svgHeight) * 100}%`,
    transform: 'translate(-50%, -50%)'
  }
}

  
  const getBezierPath = (node: GraphNode): string => {
    const parent = node.parentId ? getNodeById(node.parentId) : undefined
    if (!parent) return ''
  
    const dx = node.x - parent.x
    const dy = node.y - parent.y
    const distance = Math.sqrt(dx * dx + dy * dy)
    
    // Random curvature parameters
    const curveIntensity = 0.6 + Math.random() * 0.4
    const curveDirection = Math.random() > 0.5 ? 1 : -1
    const offsetX = (dy / distance) * 150 * curveDirection * curveIntensity
    const offsetY = (dx / distance) * 100 * curveDirection * curveIntensity
  
    // Cubic bezier control points
    const cp1 = {
      x: parent.x + dx * 0.3 + offsetX,
      y: parent.y + dy * 0.3 - offsetY
    }
    
    const cp2 = {
      x: parent.x + dx * 0.7 - offsetX,
      y: parent.y + dy * 0.6 + offsetY
    }
  
    return `M ${parent.x} ${parent.y}
            C ${cp1.x} ${cp1.y},
              ${cp2.x} ${cp2.y},
              ${node.x} ${node.y}`
  }
  </script>
  
  <style scoped>
  /* Add any custom styling here */
  </style>