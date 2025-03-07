<template>
    <section>
      <div v-for="(feature, index) in features" :key="index">
        <i :class="feature.icon"></i>
        <h4>{{ feature.title }}</h4>
        <p>{{ feature.description }}</p>
      </div>
    </section>
  </template>
  
  <script>
  export default {
    name: "FunctionalSection",
    setup() {
      return {
        features: [
          { icon: "fas fa-bolt", title: "Быстро и просто", description: "Создавайте резюме за пару минут." },
          { icon: "fas fa-brain", title: "AI-анализ", description: "Алгоритм анализирует ваш опыт." },
          { icon: "fas fa-file-alt", title: "Готовые шаблоны", description: "Выбирайте стильные шаблоны." }
        ]
      };
    }
  };
  </script>
  