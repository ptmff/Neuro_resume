<template>
  <div ref="container" class="absolute inset-0 overflow-hidden select-none">
    <img
      v-for="(layer, index) in layers"
      :key="index"
      :src="layer.src"
      class="absolute transition-transform duration-500 ease-out will-change-transform"
      :style="getStyle(layer)"
      :class="layer.color"
      alt=""
    />
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue'

// Статически импортируем SVG-файлы
import ai from '@/assets/objects/ai.svg'
import brain from '@/assets/objects/brain.svg'
import brush from '@/assets/objects/brush.svg'
import bubble from '@/assets/objects/bubble.svg'
import clockCode from '@/assets/objects/clock-code.svg'
import cloudCode from '@/assets/objects/cloud-code.svg'
import code from '@/assets/objects/code.svg'
import cpu from '@/assets/objects/cpu.svg'
import cpu2 from '@/assets/objects/cpu-2.svg'
import device from '@/assets/objects/device-desktop-code.svg'
import dna from '@/assets/objects/dna.svg'
import gear from '@/assets/objects/gear.svg'
import pencil from '@/assets/objects/pencil.svg'
import ruler from '@/assets/objects/ruler.svg'
import sparkles from '@/assets/objects/sparkles.svg'

const container = ref(null)
const mouse = ref({ x: 0, y: 0 })
const isReady = ref(false)

const icons = [
  ai,
  brain,
  brush,
  bubble,
  clockCode,
  cloudCode,
  code,
  cpu,
  cpu2,
  device,
  dna,
  gear,
  pencil,
  ruler,
  sparkles,
]

// Вместо text-цвета — задаём filter (можно задать кастомные классы)
const colorFilters = ['filter-violet', 'filter-cyan', 'filter-pink']

const getRandomIcon = () => {
  const index = Math.floor(Math.random() * icons.length)
  return icons[index]
}

const layers = Array.from({ length: 40 }, () => {
  const size = Math.floor(Math.random() * 60) + 16
  const top = Math.random() * 100
  const left = Math.random() * 100
  const opacity = (Math.random() * 0.3 + 0.1).toFixed(2)
  const rotation = Math.floor(Math.random() * 60) - 30
  const speed = Math.floor(Math.random() * 40) + 40
  const color = colorFilters[Math.floor(Math.random() * colorFilters.length)]

  return {
    src: getRandomIcon(),
    speed,
    top,
    left,
    size,
    opacity,
    rotation,
    color,
  }
})

onMounted(() => {
  isReady.value = true
  window.addEventListener('mousemove', handleMouse)
})
onUnmounted(() => {
  window.removeEventListener('mousemove', handleMouse)
})

const handleMouse = e => {
  if (!container.value) return
  const { width, height, left, top } = container.value.getBoundingClientRect()
  const sensitivity = 0.2
  mouse.value.x = (e.clientX - left - width / 2) / (width * sensitivity)
  mouse.value.y = (e.clientY - top - height / 2) / (height * sensitivity)
}

const getStyle = layer => {
  const x = (mouse.value.x || 0) * layer.speed
  const y = (mouse.value.y || 0) * layer.speed
  return {
    transform: `translate(${x}px, ${y}px) rotate(${layer.rotation}deg)`,
    top: `${layer.top}%`,
    left: `${layer.left}%`,
    width: `${layer.size}px`,
    opacity: layer.opacity,
  }
}
</script>

<style scoped>
.filter-violet {
  filter: brightness(1.2) saturate(2) drop-shadow(0 0 4px #a78bfa);
}
.filter-cyan {
  filter: brightness(1.2) saturate(2) drop-shadow(0 0 4px #22d3ee);
}
.filter-pink {
  filter: brightness(1.2) saturate(2) drop-shadow(0 0 4px #f472b6);
}
</style>
