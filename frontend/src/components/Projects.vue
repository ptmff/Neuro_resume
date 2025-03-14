<template>
  <section id="projects" class="max-w-6xl mt-16 mx-auto px-10">
    <h2 class="text-5xl font-bold text-[var(--text-light)] font-acorn mb-12">Как это работает?</h2>
    <div class="grid gap-12 masonry-grid">
      <template v-for="(project, index) in projects" :key="index">
        <div
          :ref="el => projectRefs[index] = el"
          class="relative p-8 rounded-3xl shadow-2xl text-[var(--text-light)] transition-all duration-300 group hover:scale-105 hover:shadow-3xl overflow-hidden opacity-0"
          :class="getSizeClass(index)"
          :style="{ background: project.bgColor }"
        >
          <!-- Заголовок -->
          <h3 class="text-2xl font-bold font-gt">{{ project.title }}</h3>
          <p class="font-semibold">{{ project.description }}</p>

          <!-- Фоновая иконка (до наведения) -->
          <div class="absolute bottom-[-20px] right-[-20px] text-[var(--icons-bfr-hover)] transition-all duration-300 group-hover:opacity-100 group-hover:bottom-6 group-hover:right-6 group-hover:text-[var(--text-light)]">
            <i :class="`${project.icon} text-8xl`"></i>
          </div>
        </div>
      </template>
    </div>
  </section>
</template>

<script setup>
import { ref, onMounted, onUnmounted, computed } from "vue";
import gsap from "gsap";
import { ScrollTrigger } from "gsap/ScrollTrigger";

gsap.registerPlugin(ScrollTrigger);

const projects = [
  { icon: 'fas fa-user', title: 'Заполни данные', description: 'Введи информацию о себе для генерации резюме.', bgColor: "linear-gradient(135deg, #C19BFF, #9D00FF)" },
  { icon: 'fas fa-palette', title: 'Выбери стиль', description: 'Настрой визуальный стиль резюме под себя.', bgColor: "linear-gradient(135deg, #A0E6E3, #4CB8C4)" },
  { icon: 'fas fa-magic', title: 'Оптимизируй', description: 'Используй AI для улучшения и подбора ключевых слов.', bgColor: "linear-gradient(135deg, #F8C8A0, #D87C5B)" },
  { icon: 'fas fa-download', title: 'Скачай и отправь', description: 'Скачай PDF и отправь работодателю!', bgColor: "linear-gradient(135deg, #7289DA, #4B6FA4)" },
];

const windowWidth = ref(window.innerWidth);
const projectRefs = ref([]);

const updateWindowWidth = () => {
  windowWidth.value = window.innerWidth;
};

onMounted(() => {
  window.addEventListener("resize", updateWindowWidth);
  
  projectRefs.value.forEach((element, index) => {
    const xStart = index % 2 === 0 ? -50 : 50; // Reduced animation distance
    
    gsap.fromTo(element,
      {
        x: xStart,
        opacity: 0
      },
      {
        x: 0,
        opacity: 1,
        duration: 1,
        ease: "power2.out",
        scrollTrigger: {
          trigger: element,
          start: "top bottom-=100",
          end: "top center",
          toggleActions: "play none none reverse"
        }
      }
    );
  });
});

onUnmounted(() => {
  window.removeEventListener("resize", updateWindowWidth);
  ScrollTrigger.getAll().forEach(trigger => trigger.kill());
});

const getSizeClass = computed(() => (index) => {
  if (windowWidth.value < 951) return "w-full h-[350px]";

  const isOddRow = Math.floor(index / 2) % 2 === 0;
  return index % 2 === 0
    ? isOddRow
      ? "col-span-2 h-[350px]"
      : "col-span-3 h-[350px]"
    : isOddRow
      ? "col-span-3 h-[350px]"
      : "col-span-2 h-[350px]";
});
</script>

<style scoped>
.masonry-grid {
  display: grid;
  grid-template-columns: repeat(5, 1fr);
  gap: 60px;
}

.group:hover .icon-animation {
  transform: rotate(10deg) scale(1.2);
  transition: transform 0.3s ease-in-out;
}

@media (max-width: 950px) {
  .masonry-grid {
    grid-template-columns: 1fr;
    gap: 30px;
  }
}
</style>