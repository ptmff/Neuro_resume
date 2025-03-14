<template>
  <div class="animation-container" @mousemove="handleMouseMove" @mouseleave="resetTransform">
    <!-- Background layer that reacts to mouse movements -->
    <div class="animated-background" :style="bgStyle"></div>

    <!-- Optional content overlay -->
    <div class="content">
      <h1>Interactive 3D Background</h1>
      <p>Move your mouse to see the effect!</p>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'

// Reactive style for background transformation
const bgStyle = ref({
  transform: 'rotateX(0deg) rotateY(0deg) scale(1)',
})

// Handle mouse movement to calculate rotation based on cursor position
const handleMouseMove = e => {
  const container = e.currentTarget
  const rect = container.getBoundingClientRect()
  const centerX = rect.width / 2
  const centerY = rect.height / 2
  const mouseX = e.clientX - rect.left
  const mouseY = e.clientY - rect.top

  // Calculate differences from the center
  const diffX = mouseX - centerX
  const diffY = mouseY - centerY

  // Adjust sensitivity (here max rotation is 10deg on each axis)
  const rotateY = (diffX / centerX) * 10
  const rotateX = -(diffY / centerY) * 10 // invert for natural tilt

  bgStyle.value = {
    transform: `rotateX(${rotateX}deg) rotateY(${rotateY}deg) scale(1.05)`,
  }
}

// Reset the transform when the mouse leaves the container
const resetTransform = () => {
  bgStyle.value = {
    transform: 'rotateX(0deg) rotateY(0deg) scale(1)',
  }
}
</script>

<style scoped>
.animation-container {
  width: 100vw;
  height: 100vh;
  perspective: 1000px; /* enables 3D effect */
  overflow: hidden;
  position: relative;
}

/* The background layer */
.animated-background {
  position: absolute;
  top: -5%;
  left: -5%;
  width: 110%; /* slightly larger to cover any gaps during rotation */
  height: 110%;
  background: url('https://example.com/your-background-image.jpg') no-repeat center center/cover;
  transition: transform 0.3s ease;
  will-change: transform;
}

/* Optional overlay content */
.content {
  position: relative;
  z-index: 10;
  text-align: center;
  top: 50%;
  transform: translateY(-50%);
  color: #fff;
  text-shadow: 0 2px 4px rgba(0, 0, 0, 0.5);
}
</style>
