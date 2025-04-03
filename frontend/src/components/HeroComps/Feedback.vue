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
        <transition-group name="slide-fade" tag="div" class="relative h-[230px]">
          <div 
            v-for="review in visibleReviews"
            :key="review.id"
            class="absolute w-full bg-[var(--background-overlay)] backdrop-blur-sm border border-[var(--highlight)] rounded-2xl p-8 shadow-xl"
            :class="{ 'z-10': isActive(review) }"
          >
            <div class="flex flex-col md:flex-row gap-6 items-start">
              <img 
                :src="review.avatar" 
                :alt="review.name"
                class="w-20 h-20 rounded-full border-2 border-[var(--highlight)]"
              >
              <div class="text-left flex-1">
                <div class="flex flex-col md:flex-row justify-between items-start mb-4">
                  <h3 class="text-xl font-semibold text-[var(--text-light)]">{{ review.name }}</h3>
                  <div class="flex gap-1 mt-2 md:mt-0">
                    <span
                      v-for="star in 5"
                      :key="star"
                      class="text-xl transition-colors"
                      :class="star <= review.rating ? 'text-amber-400' : 'text-[var(--text-mainless)]'"
                    >
                      ★
                    </span>
                  </div>
                </div>
                <p class="text-[var(--text-mainless)] italic text-lg leading-relaxed">
                  "{{ review.text }}"
                </p>
              </div>
            </div>
          </div>
        </transition-group>

        <div class="flex justify-center items-center gap-4 mt-0">
          <button
            @click="prevSlide"
            class="p-3 rounded-full bg-[var(--background-pale)] hover:bg-[var(--highlight)] transition-all"
          >
            ←
          </button>
          
          <div class="flex gap-2">
            <button
              v-for="(_, index) in reviews"
              :key="index"
              @click="currentIndex = index"
              class="w-3 h-3 rounded-full transition-all"
              :class="currentIndex === index 
                ? 'bg-[var(--highlight)] scale-125' 
                : 'bg-[var(--text-mainless)] opacity-50'"
            />
          </div>
          
          <button
            @click="nextSlide"
            class="p-3 rounded-full bg-[var(--background-pale)] hover:bg-[var(--highlight)] transition-all"
          >
            →
          </button>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup>
import { ref, computed } from 'vue'

const reviews = ref([
  {
    id: 1,
    name: 'Анна Петрова',
    avatar: '/avatars/avatar1.png',
    text: 'Neuro.Resume полностью изменил мой подход к поиску работы. Профессиональные шаблоны и умные рекомендации помогли создать идеальное резюме!',
    rating: 5
  },
  {
    id: 2,
    name: 'Максим Иванов',
    avatar: '/avatars/avatar2.png',
    text: 'Лучший сервис для карьерного роста. Получил в 3 раза больше откликов благодаря умной оптимизации резюме!',
    rating: 4
  },
  {
    id: 3,
    name: 'Ольга Сидорова',
    avatar: '/avatars/avatar3.png',
    text: 'Интуитивно понятный интерфейс и потрясающая гибкость настроек. Смогла выделить свои сильные стороны именно так, как хотела.',
    rating: 5
  }
])

const currentIndex = ref(0)

const visibleReviews = computed(() => [reviews.value[currentIndex.value]])

const isActive = (review) => reviews.value[currentIndex.value].id === review.id

const nextSlide = () => {
  currentIndex.value = (currentIndex.value + 1) % reviews.value.length
}

const prevSlide = () => {
  currentIndex.value = (currentIndex.value - 1 + reviews.value.length) % reviews.value.length
}
</script>

<style scoped>
.slide-fade-enter-active,
.slide-fade-leave-active {
  transition: all 0.8s cubic-bezier(0.4, 0, 0.2, 1);
}

.slide-fade-enter-from {
  opacity: 0;
  transform: translateX(50px);
}

.slide-fade-leave-to {
  opacity: 0;
  transform: translateX(-50px);
}

.slide-fade-leave-active {
  position: absolute;
  width: 100%;
}
</style>