<template>
    <section>
      <h2>Как это работает?</h2>
      <div v-for="(step, index) in steps" :key="index">
        <i :class="step.icon"></i>
        <h4>{{ step.title }}</h4>
        <p>{{ step.text }}</p>
      </div>
    </section>
  </template>
  
  <script setup>
  import { ref, onMounted } from "vue";

    const steps = [
    { icon: "fas fa-user", title: "Заполни данные", text: "Введи информацию о себе." },
    { icon: "fas fa-palette", title: "Выбери стиль", text: "Настрой резюме под себя." },
    { icon: "fas fa-magic", title: "Оптимизируй", text: "Используй AI для улучшения." },
    { icon: "fas fa-download", title: "Скачай и отправь", text: "Скачай PDF и отправь работодателю." }
    ];

    const stepElements = ref([]);

onMounted(() => {
  const observer = new IntersectionObserver((entries) => {
    entries.forEach((entry) => {
      if (entry.isIntersecting) {
        entry.target.classList.add("appear");
      }
    });
  }, { threshold: 0.3 });

  stepElements.value.forEach(el => observer.observe(el));
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

  </style>
  