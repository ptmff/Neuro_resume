<template>
  <section class="w-full py-20 px-6 md:px-12">
    <div class="max-w-6xl mx-auto">
      <h2 class="text-3xl md:text-4xl font-bold text-[var(--text-light)] mb-12 text-center">
        Почему Neuro.Resume?
      </h2>
      <div class="grid grid-cols-1 md:grid-cols-2 gap-10 overflow-hidden">
        <div
          v-for="(item, index) in benefits"
          :key="index"
          :data-aos="index % 2 === 0 ? 'fade-right' : 'fade-left'"
          data-aos-duration="800"
          :class="[
            'flex items-start gap-5 flex-wrap max-w-full min-w-0',
            index % 2 === 1 ? 'flex-row-reverse text-right' : 'text-left',
          ]"
        >
          <div
            class="w-12 h-12 flex-shrink-0 bg-[var(--icons-bg-adventages)] rounded-xl flex items-center justify-center"
          >
            <component :is="item.icon" class="text-[var(--text-secondary)] w-6 h-6" />
          </div>
          <div class="break-words min-w-0">
            <h3 class="text-lg font-semibold text-[var(--text-light)]">{{ item.title }}</h3>
            <p class="text-[var(--text-mainless)] text-sm mt-1">{{ item.desc }}</p>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { Sparkles, Settings, FileText, ShieldCheck } from 'lucide-vue-next'

interface Benefit {
  icon: typeof Sparkles;
  title: string;
  desc: string;
}

const benefits: Benefit[] = [
  {
    icon: Sparkles,
    title: 'Искусственный интеллект',
    desc: 'Подбираем формулировки и оптимизируем структуру.',
  },
  {
    icon: FileText,
    title: 'Современные шаблоны',
    desc: 'Минималистичный и профессиональный стиль.',
  },
  {
    icon: ShieldCheck,
    title: 'Безопасность данных',
    desc: 'Никаких утечек. Только ты и твоё резюме.',
  },
  {
    icon: Settings,
    title: 'Гибкость',
    desc: 'Ты управляешь всем — от стиля до содержания.',
  },
]
</script>
