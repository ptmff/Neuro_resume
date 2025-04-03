<template>
  <section class="w-full py-24 px-6 md:px-12" data-aos="fade-up" data-aos-duration="1200">
    <div class="max-w-6xl mx-auto text-center">
      <h2 class="text-3xl md:text-4xl font-bold text-[var(--text-light)] mb-8">
        Отзывы наших пользователей
      </h2>
      <p class="text-[var(--text-mainless)] mb-10 max-w-2xl mx-auto">
        Узнайте, что говорят те, кто уже создал резюме мечты с Neuro.Resume
      </p>

      <div class="relative max-w-4xl mx-auto">
        <div class="relative h-[230px]">
          <div 
            class="w-full bg-[var(--background-overlay)] backdrop-blur-sm border border-[var(--highlight)] rounded-2xl p-8 shadow-xl"
          >
            <div class="flex flex-col md:flex-row gap-6 items-start">
              <img 
                src="#" 
                alt="Анна Петрова"
                class="w-20 h-20 rounded-full border-2 border-[var(--highlight)]"
              >
              <div class="text-left flex-1">
                <div class="flex flex-col md:flex-row justify-between items-start mb-4">
                  <h3 class="text-xl font-semibold text-[var(--text-light)]">Анна Петрова</h3>
                </div>
                <p class="text-[var(--text-mainless)] italic text-lg leading-relaxed">
                  "Neuro.Resume полностью изменил мой подход к поиску работы. Профессиональные шаблоны и умные рекомендации помогли создать идеальное резюме!"
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup>
import { ref, computed } from 'vue'

// Оставлен один отзыв для примера
const reviews = ref([
{
  id: 1,
  name: 'Анна Петрова',
  avatar: '/avatars/anna-p.jpg',
  text: 'Neuro.Resume полностью изменил мой подход к поиску работы. Профессиональные шаблоны и умные рекомендации помогли создать идеальное резюме!',
  rating: 5
}
])
</script>