<template>
    <canvas ref="canvas" class="w-full h-screen fixed top-0 left-0 z-20" />
  </template>
  
  <script setup lang="ts">
  import { onMounted, ref } from 'vue'
  import * as THREE from 'three'
  import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader.js'
  
  const canvas = ref<HTMLCanvasElement | null>(null)
  
  onMounted(() => {
    if (!canvas.value) return
  
    // 🎬 Сцена и рендерер
    const scene = new THREE.Scene()
    const camera = new THREE.PerspectiveCamera(
      60,
      window.innerWidth / window.innerHeight,
      0.1,
      1000
    )
    const renderer = new THREE.WebGLRenderer({ canvas: canvas.value, alpha: true, antialias: true })
    renderer.setSize(window.innerWidth, window.innerHeight)
    renderer.setPixelRatio(window.devicePixelRatio)
  
    // 💡 Свет
    const light1 = new THREE.PointLight(0x00ffe0, 2, 100)
    light1.position.set(5, 5, 5)
    scene.add(light1)
  
    const light2 = new THREE.PointLight(0x00e0ff, 1.5, 100)
    light2.position.set(-5, -5, 5)
    scene.add(light2)
  
    const ambientLight = new THREE.AmbientLight(0xffffff, 0.4)
    scene.add(ambientLight)
  
    // 🧠 Группа для мозга
    const brainGroup = new THREE.Group()
    scene.add(brainGroup)
  
    // 🧠 Загрузка модели
    const loader = new GLTFLoader()
    let brain: THREE.Object3D | null = null
    const brainWrapper = new THREE.Object3D()

    loader.load('/models/brain.glb', (gltf) => {
    brain = gltf.scene
    brain.position.set(0, 0, 0) // Центр внутри wrapper
    brain.rotation.set(0, Math.PI, 0) // Повернуть лицом к пользователю

    brainWrapper.add(brain)
    brainWrapper.position.set(0, 0, 0)
    brainWrapper.scale.set(2.5, 2.5, 2.5)

    scene.add(brainWrapper)
    })


    // Камера ближе
    camera.position.z = 2.5 // раньше было 5

  
    // 🌌 Партиклы (отдельно от мозга)
    const particleGroup = new THREE.Group()
    const particleGeometry = new THREE.BufferGeometry()
    const particleCount = 300
    const positions = new Float32Array(particleCount * 3)
  
    for (let i = 0; i < particleCount * 3; i++) {
      positions[i] = (Math.random() - 0.5) * 12
    }
  
    particleGeometry.setAttribute('position', new THREE.BufferAttribute(positions, 3))
    const particleMaterial = new THREE.PointsMaterial({
      color: 0x00ffe0,
      size: 0.04,
      transparent: true,
      opacity: 0.7
    })
    const particles = new THREE.Points(particleGeometry, particleMaterial)
    particleGroup.add(particles)
    scene.add(particleGroup)
  
    // 📷 Камера
    camera.position.z = 5
  
    // ⏱ Живое движение
    let t = 0
    const clock = new THREE.Clock()

  
    // 🚀 Анимация
    const animate = () => {
      requestAnimationFrame(animate)
  
      // Вращение партиклов
      particleGroup.rotation.y += 0.001
  
      // Лёгкое плавание мозга
      brainGroup.rotation.y += 0.002
      brainGroup.position.y = Math.sin(t) * 0.1
      brainGroup.rotation.x = Math.sin(t / 2) * 0.03
      t += 0.01
    const time = clock.getElapsedTime()

  // 👇 Дыхание
      brainGroup.position.y = Math.sin(time * 1.5) * 0.05

  
      renderer.render(scene, camera)
    }
  
    animate(
        
    )
  
    // 🖥 Ресайз
    window.addEventListener('resize', () => {
      camera.aspect = window.innerWidth / window.innerHeight
      camera.updateProjectionMatrix()
      renderer.setSize(window.innerWidth, window.innerHeight)
    })
  })
  </script>
  
  <style scoped>
  canvas {
    pointer-events: none;
  }
  </style>
  