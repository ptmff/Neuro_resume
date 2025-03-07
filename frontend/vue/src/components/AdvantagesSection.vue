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

<style>
/* 🔹 Основная секция */
.advantages {
  background-color: #f5ebe0;
  padding: 60px 0;
}

/* 🔹 Заголовок */
.section-title {
  font-size: 2rem;
  font-weight: bold;
  color: #3b2f2f;
  margin-bottom: 40px;
}

/* 🔹 Контейнер преимуществ */
.advantages-content {
  display: flex;
  justify-content: space-between;
  align-items: center;
  max-width: 1000px;
  margin: 0 auto;
  gap: 30px;
}

/* 🔹 Список преимуществ */
.advantages-list {
  list-style: none;
  padding: 0;
  width: 40%;
}

.advantages-list li {
  display: flex;
  align-items: center;
  padding: 15px;
  font-size: 1.2rem;
  color: #6d4c2f;
  cursor: pointer;
  transition: 0.3s;
  border-radius: 8px;
  margin-bottom: 10px;
}

.advantages-list li:hover,
.advantages-list li.active {
  background: #8b5e3c;
  color: #fff;
}

/* 🔹 Иконки */
.advantage-icon {
  font-size: 1.5rem;
  margin-right: 10px;
}

/* 🔹 Описание с изображением */
.advantage-description {
  width: 55%;
  background: linear-gradient(135deg, rgba(139, 94, 60, 0.1), rgba(139, 94, 60, 0.2));
  backdrop-filter: blur(10px);
  padding: 20px;
  border-radius: 10px;
  display: flex;
  align-items: center;
  gap: 20px;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.15);
  transition: transform 0.3s ease, opacity 0.3s ease;
}

.advantage-image {
  flex: 0 0 80px;
}

.advantage-image img {
  width: 80px;
  height: 80px;
  object-fit: contain;
}

/* 🔹 Текст */
.advantage-text {
  flex: 1;
}

.advantage-text h4 {
  font-size: 1.5rem;
  font-weight: bold;
  color: #3b2f2f;
  margin-bottom: 10px;
}

.advantage-text p {
  font-size: 1rem;
  color: #6d4c2f;
}

/* 🔹 Анимация появления */
.fade-enter-active, .fade-leave-active {
  transition: opacity 0.5s, transform 0.3s ease-out;
}
.fade-enter, .fade-leave-to {
  opacity: 0;
  transform: translateY(10px);
}

/* 🔹 Адаптация */
@media (max-width: 768px) {
  .advantages-content {
    flex-direction: column;
    text-align: center;
  }

  .advantages-list {
    width: 100%;
  }

  .advantage-description {
    width: 100%;
    flex-direction: column;
    align-items: center;
    text-align: center;
  }

  .advantage-image img {
    width: 60px;
    height: 60px;
  }
}
</style>