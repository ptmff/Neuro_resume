<template>
  <nav
    ref="navRef"
    class="navbar max-w-[700px] w-full flex justify-center fixed top-6 left-1/2 -translate-x-1/2 z-50 px-6 py-3 rounded-3xl transition-all duration-300"
    :class="{ 'bg-[var(--primary-dark)]/50 backdrop-blur-2xl shadow-lg': isScrolled, 'bg-transparent': !isScrolled }"
  >
    <ul class="relative flex font-medium text-[var(--text-light)]" ref="navList">
      <!-- Индикатор -->
      <div
        class="indicator absolute bg-[var(--background-indicator)] rounded-3xl transition-all duration-300 ease-in-out"
        :style="indicatorStyle"
      ></div>

      <li
        v-for="(link, index) in navLinks"
        :key="index"
        class="relative cursor-pointer rounded-lg transition-all duration-300 z-10"
        :class="{
          'text-[var(--text-light)]': activeIndex === index,
          'text-[var(--text-light)]/70 hover:text-[var(--text-light)]': activeIndex !== index
        }"
        @click="navigate(link.path, index)"
      >
        {{ link.name }}
      </li>
    </ul>
  </nav>
</template>

<script setup>
import { ref, onMounted, nextTick, watch, onBeforeUnmount } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const navLinks = ref([
  { name: 'Главная', path: '/' },
  { name: 'Создать резюме', path: '/resume' },
  { name: 'Анализ вакансий', path: '/analyse' },
  { name: 'Профиль', path: '/profile' },
])

const activeIndex = ref(0)
const route = useRoute()
const router = useRouter()
const isScrolled = ref(false)

const handleScroll = () => {
  isScrolled.value = window.scrollY > 50
}

onMounted(() => {
  window.addEventListener('scroll', handleScroll)
})

onBeforeUnmount(() => {
  window.removeEventListener('scroll', handleScroll)
})

const navRef = ref(null)
const navList = ref(null)
const navItems = ref([])

const indicatorStyle = ref({
  left: '0px',
  width: '0px',
  top: '0px',
  height: '40px'
})

const updateIndicator = () => {
  nextTick(() => {
    const activeEl = navItems.value[activeIndex.value]
    if (activeEl) {
      const { offsetLeft, clientWidth, offsetTop, clientHeight } = activeEl
      const indicatorHeight = 40
      indicatorStyle.value = {
        left: offsetLeft + 'px',
        width: clientWidth + 'px',
        top: (offsetTop + clientHeight / 2 - indicatorHeight / 2) + 'px',
        height: indicatorHeight + 'px'
      }
    }
  })
}

const updateActiveIndex = async () => {
  await nextTick()
  const index = navLinks.value.findIndex(link => link.path === route.path)
  if (index !== -1) {
    activeIndex.value = index
    updateIndicator()
  }
}

const navigate = (path, index) => {
  router.push(path)
  activeIndex.value = index
  updateIndicator()
}

onMounted(() => {
  nextTick(() => {
    navItems.value = navList.value.querySelectorAll('li')
    updateActiveIndex()
    window.addEventListener('resize', updateIndicator)
  })
})

onBeforeUnmount(() => {
  window.removeEventListener('resize', updateIndicator)
})

watch(route, updateActiveIndex)
</script>

<style scoped>
nav {
  backdrop-filter: blur(10px);
  transition: background 0.3s ease-in-out;
}

ul {
  gap: 1.5rem;
  padding: 0;
  margin: 0;
}

li {
  padding: 0.5rem 1.5rem;
  font-size: 1rem;
}

@media (max-width: 768px) {
  .navbar {
    max-width: 400px;
    min-width: 395px;
  }
  ul {
    gap: 0rem;
  }
  li {
    padding: 0.5rem 0.5rem;
    font-size: 0.7rem;
  }
}

.indicator {
  position: absolute;
  transition: left 0.3s ease-in-out, width 0.3s ease-in-out, top 0.3s ease-in-out;
  border-radius: 9999px;
  z-index: 0;
}
</style>