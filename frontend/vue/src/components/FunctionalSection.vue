<template>
    <section class="functional-section fade-up">
      <div class="container-fluid px-3 px-md-5">
        <div class="row justify-content-center">
          <div class="col-lg-4 col-md-6 col-sm-12 d-flex justify-content-center"
               v-for="(feature, index) in features" :key="index">
            <div class="functional-item text-center">
              <i :class="feature.icon" class="feature-icon"></i>
              <h4 class="feature-title">{{ feature.title }}</h4>
              <p class="feature-description">{{ feature.description }}</p>
            </div>
          </div>
        </div>
      </div>
    </section>
  </template>
  
  <script>
  export default {
    name: "FunctionalSection",
    setup() {
      const features = [
        { icon: "fas fa-bolt", title: "Быстро и просто", description: "Создавайте резюме за пару минут, без сложных форм." },
        { icon: "fas fa-brain", title: "AI-анализ", description: "Наш алгоритм анализирует ваш опыт и предлагает лучшие формулировки." },
        { icon: "fas fa-file-alt", title: "Готовые шаблоны", description: "Выбирайте стильные и профессиональные шаблоны." }
      ];
  
      return { features };
    }
  };
  </script>

<style>
/* 🔹 Основная секция */
.functional-section {
  background-color: #fff;
  padding: 60px 0;
}

/* 🔹 Контейнер */
.container-fluid {
  max-width: 1200px;
}

/* 🔹 Карточки */
.functional-item {
  padding: 20px;
  border: 2px solid #8b5e3c;
  border-radius: 12px;
  margin: 15px;
  text-align: center;
  min-height: 180px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  width: 100%;
  max-width: 350px;
}

/* 🔹 Иконка */
.feature-icon {
    font-size: 3rem;
    color: #8b5e3c;
    margin-bottom: 15px;
  }
  
  /* 🔹 Текст */
  .feature-title {
    font-size: 1.5rem;
    color: #3b2f2f;
    font-weight: bold;
  }
  
  .feature-description {
    font-size: 1rem;
    color: #6d4c2f;
  }
  /* 🔹 Адаптивность */
  @media (max-width: 768px) {
    .functional-item {
      max-width: 90%;
    }
  }
  
  @media (max-width: 576px) {
    .functional-item {
      max-width: 95%;
    }
  }
  
  /* 🔹 Анимация появления вверх */
  .fade-up {
    opacity: 0;
    transform: translateY(50px);
    animation: fadeUp 1s ease-out forwards;
  }
  
  @keyframes fadeUp {
    from {
      opacity: 0;
      transform: translateY(50px);
    }
    to {
      opacity: 1;
      transform: translateY(0);
    }
  }
</style>