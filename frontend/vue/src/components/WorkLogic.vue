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
  const steps = [
    { icon: "fas fa-user", title: "Заполни данные", text: "Введи информацию о себе." },
    { icon: "fas fa-palette", title: "Выбери стиль", text: "Настрой резюме под себя." },
  ];
  </script>
  
  
  <style>
  section { text-align: center; }
  h2 { font-size: 1.5rem; }
  i { font-size: 1.2rem; margin-right: 5px; }
  </style>
  