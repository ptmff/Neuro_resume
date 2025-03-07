<template>
    <section class="advantages fade-up">
      <div class="container">
        <h2 class="section-title text-center">Почему выбирают NeuroResume?</h2>
        
        <div class="advantages-content">
          <!-- Список преимуществ -->
          <ul class="advantages-list">
            <li 
              v-for="(advantage, index) in advantages" 
              :key="index" 
              @mouseover="setActiveAdvantage(index)"
              :class="{ 'active': activeIndex === index }"
            >
              <i :class="advantage.icon" class="advantage-icon"></i>
              <span>{{ advantage.title }}</span>
            </li>
          </ul>
          
          <!-- Описание преимущества -->
          <transition name="fade">
            <div class="advantage-description" v-if="advantages[activeIndex]">
              <div class="advantage-image">
                <img :src="advantages[activeIndex].image" :alt="advantages[activeIndex].title" />
              </div>
              <div class="advantage-text">
                <h4>{{ advantages[activeIndex].title }}</h4>
                <p>{{ advantages[activeIndex].description }}</p>
              </div>
            </div>
          </transition>
        </div>
      </div>
    </section>
</template>

<script setup>
import { ref } from 'vue';
import aiGen1 from '../assets/ai-gen1.jpg'

const advantages = ref([
  { 
    icon: "fas fa-robot", 
    title: "Автоматизация", 
    description: "Экономьте время с AI, который анализирует и оптимизирует ваше резюме.",
    image: aiGen1
  },
  { 
    icon: "fas fa-chart-line", 
    title: "Оптимизация", 
    description: "Алгоритмы AI помогают сделать ваше резюме более заметным для ATS.",
    image: aiGen1  
  },
  { 
    icon: "fas fa-search", 
    title: "Анализ", 
    description: "AI анализирует вакансии и подбирает ключевые навыки для вашего резюме.",
    image: aiGen1
  },
  { 
    icon: "fas fa-user-check", 
    title: "Персонализация", 
    description: "Настраивайте стиль и содержание резюме под ваши цели и предпочтения.",
    image: aiGen1
  },
  { 
    icon: "fas fa-file-alt", 
    title: "Гибкость", 
    description: "Создавайте несколько версий резюме для разных вакансий.",
    image: aiGen1
  },
  { 
    icon: "fas fa-briefcase", 
    title: "Профессионализм", 
    description: "Структурированное и оформленное резюме повышает шансы на успех.",
    image: aiGen1
  }
]);

const activeIndex = ref(0);

const setActiveAdvantage = (index) => {
  activeIndex.value = index;
};
</script>