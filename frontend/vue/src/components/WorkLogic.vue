<template>
    <section class="how-it-works fade-in-left">
      <div class="container">
        <h2 class="section-title text-center">Как это работает?</h2>
  
        <div class="timeline">
          <div 
            v-for="(step, index) in steps" 
            :key="index" 
            class="timeline-step"
            :class="{ 'active': isVisible, 'left': index % 2 === 0, 'right': index % 2 !== 0 }"
            ref="stepElements"
          >
            <!-- Левая часть: иконка + линия (линия только на мобилке) -->
            <div class="timeline-left">
              <div class="icon-circle">
                <i :class="step.icon"></i>
              </div>
              <div v-if="index !== steps.length - 1" class="timeline-line"></div>
            </div>
  
            <!-- Текстовый блок -->
            <div class="timeline-text">
              <h4 class="step-title">{{ step.title }}</h4>
              <p class="step-text">{{ step.text }}</p>
            </div>
          </div>
        </div>
      </div>
    </section>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue';
  
  const steps = ref([
    { icon: 'fas fa-user', title: 'Заполни данные', text: 'Введи информацию о себе для генерации резюме.' },
    { icon: 'fas fa-palette', title: 'Выбери стиль', text: 'Настрой визуальный стиль резюме под себя.' },
    { icon: 'fas fa-magic', title: 'Оптимизируй', text: 'Используй AI для улучшения и подбора ключевых слов.' },
    { icon: 'fas fa-download', title: 'Скачай и отправь', text: 'Скачай PDF и отправь работодателю!' },
  ]);
  
  const stepElements = ref([]);
  const isVisible = ref(false);
  
  onMounted(() => {
    const observer = new IntersectionObserver((entries) => {
      entries.forEach((entry) => {
        if (entry.isIntersecting) {
          isVisible.value = true;
        }
      });
    }, { threshold: 0.3 });
  
    stepElements.value.forEach((el) => observer.observe(el));
  });
  </script>
  
  
  <style>
  section { text-align: center; }
  h2 { font-size: 1.5rem; }
  i { font-size: 1.2rem; margin-right: 5px; }

  div {
  opacity: 0;
  transform: translateY(20px);
  transition: opacity 0.5s ease, transform 0.5s ease;
  }

  div.appear {
  opacity: 1;
  transform: translateY(0);
  }

@media (min-width: 768.1px) {
  .timeline {
    display: flex;
    flex-direction: column;
    position: relative;
  }

  .timeline-step {
    width: 100%;
    display: flex;
    align-items: flex-start;
    position: relative;
  }

  .timeline-line {
    display: none;
  }

  .timeline-step.left {
    flex-direction: row;
  }

  .timeline-step.right {
    flex-direction: row-reverse;
  }

  .timeline-step.left .timeline-text {
    text-align: left;

    margin-left: 20px;
  }

  .timeline-step.right .timeline-text {
    text-align: right;
    margin-right: 20px;
  }
}

@media (max-width: 768px) {
  .timeline {
    max-width: 600px;
  }

  .timeline-step {
    flex-direction: row;
    align-items: flex-start;
  }

  .timeline-left {
    margin-right: 10px;
  }

  .icon-circle {
    width: 40px;
    height: 40px;
    font-size: 1.2rem;
  }

  .timeline-text {
    text-align: left;
    max-width: 250px;
  }
}
  </style>
  