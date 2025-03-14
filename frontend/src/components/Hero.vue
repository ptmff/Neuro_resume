<template>
  <section
    class="relative w-full min-h-screen flex items-center justify-center overflow-hidden bg-[var(--background-main)]"
  >
    <!-- Фоновое видео -->
    <video
      :src="footage"
      ref="videoRef"
      class="absolute top-0 left-0 w-full h-full object-cover filter blur-xl rotate-180"
      muted
      playsinline
      autoplay
      loop
    >
      Ваш браузер не поддерживает видео.
    </video>

    <!-- Градиентный оверлей для плавного перехода -->
    <div class="video-overlay"></div>

    <!-- Оверлей для затемнения видео -->
    <div class="absolute top-0 left-0 w-full h-full bg-[var(--background-fade)]"></div>

    <!-- Контент Hero (заголовок и кнопка) -->
    <div
      ref="contentRef"
      class="relative z-10 flex flex-col items-center justify-center text-center opacity-0"
    >
      <div class="max-w-2xl px-4">
        <h1 ref="titleRef" class="text-5xl md:text-6xl font-bold mb-6 text-[var(--text-light)]">
          Neuro.Resume
        </h1>
        <p ref="subtitleRef" class="text-xl md:text-2xl mb-8 text-[var(--text-light)]">
          Построй своё резюме с искусством и точностью
        </p>
        <a href="#projects" class="relative inline-block">
          <div class="relative">
            <button
              ref="buttonRef"
              class="px-6 py-3 bg-[var(--background-cta)] text-[var(--text-main)] font-semibold rounded-full shadow-lg hover:shadow-xl hover:bg-transparent hover:border-[var(--background-cta)] hover:border-1 transition-all duration-300 transform hover:scale-105"
            >
              <span class="opacity-0">Вперёд</span>
            </button>
            <span
              ref="buttonTextRef"
              class="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 opacity-0 text-[var(--text-main)] font-semibold whitespace-nowrap z-10"
            >
              Вперёд
            </span>
          </div>
        </a>
      </div>
    </div>
  </section>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue'
import gsap from 'gsap'
import footage from '@/assets/video/footage1.mp4'

const videoRef = ref(null)
const contentRef = ref(null)
const titleRef = ref(null)
const subtitleRef = ref(null)
const buttonRef = ref(null)
const buttonTextRef = ref(null)

onMounted(() => {
  const video = videoRef.value

  const handleTimeUpdate = () => {
    if (video.currentTime >= video.duration - 0.5) {
      const remainingTime = video.duration - video.currentTime
      const slowdownFactor = Math.max(0.1, remainingTime / 0.5)
      video.playbackRate = slowdownFactor

      if (video.currentTime >= video.duration - 0.1) {
        video.currentTime = 0
        video.playbackRate = 1
      }
    }
  }

  video.addEventListener('loadedmetadata', () => {
    video.play()
  })

  video.addEventListener('timeupdate', handleTimeUpdate)

  // GSAP анимация появления
  const tl = gsap.timeline({
    defaults: { ease: 'power3.out' },
  })

  tl.to(contentRef.value, {
    opacity: 1,
    duration: 0.5,
  })
    .from(
      titleRef.value,
      {
        y: 50,
        autoAlpha: 0,
        duration: 0.8,
      },
      '-=0.3'
    )
    .from(
      subtitleRef.value,
      {
        y: 30,
        autoAlpha: 0,
        duration: 0.8,
      },
      '-=0.5'
    )
    .fromTo(
      buttonRef.value,
      {
        x: 100,
        autoAlpha: 0,
        scale: 0.8,
      },
      {
        x: 0,
        autoAlpha: 1,
        scale: 1,
        duration: 0.8,
      },
      '-=0.4'
    )
    .fromTo(
      buttonTextRef.value,
      {
        x: -100,
        autoAlpha: 0,
      },
      {
        x: 0,
        autoAlpha: 1,
        duration: 0.8,
      },
      '-=0.8'
    )

  // Удаляем обработчики при размонтировании
  onUnmounted(() => {
    video.removeEventListener('timeupdate', handleTimeUpdate)
    tl.kill()
  })
})
</script>

<style scoped>
:global(html) {
  scroll-behavior: smooth;
}

/* Градиентный оверлей для плавного перехода видео в фон */
.video-overlay {
  position: absolute;
  bottom: 0;
  width: 100%;
  height: 100%;
  background: linear-gradient(to bottom, rgba(0, 0, 0, 0) 0%, #0b0b24 98%);
  pointer-events: none;
}
</style>
