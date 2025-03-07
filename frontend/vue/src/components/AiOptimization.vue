<template>
    <div class="container p-5">
        <h2 class="title mb-5" >Оптимизация резюме с помощью AI</h2>
        <div class="ai-optimization-container p-3 mb-4">
        <div class="d-flex align-items-center mb-3">
          <div class="ai-icon me-3">
            <i class="bi bi-robot"></i>
          </div>
          <div>
            <h5 class="mb-1">AI-анализ</h5>
            <p class="mb-0">Наш алгоритм проанализировал ваше резюме и предлагает улучшения</p>
          </div>
        </div>
      </div>
      <div class="d-flex justify-content-between mt-4">
        <button type="button" class="btn btn-outline-secondary" @click="$emit('prev-step')">Назад</button>
        <button type="button" class="btn" @click="$emit('next-step')">Далее</button>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    props: {
      aiSuggestions: {
        type: Array,
        required: true
      }
    },
    emits: [ 'next-step', 'prev-step']
  };
  </script>
  
<style scoped>
  .container{
      color: #3b2f2f;
      border-radius: 10px;
      box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    }

    .title{
        font-size: 2rem;
        font-weight: bold;
    }
  .btn {
    color: white;
    background-color: #8b5e3c;
  }
  
  .btn:hover {
    background-color: #6d4c2f;
  }
  
  .btn-outline-secondary {
    color: #8b5e3c;
    border-color: #8b5e3c;
    background-color: transparent;
  }
  
  .btn-outline-secondary:hover {
    color: white;
    background-color: #8b5e3c;
  }
  
  /* AI оптимизация */

  
  .ai-icon {
    width: 40px;
    height: 40px;
    border-radius: 50%;
    background-color: #8b5e3c;
    display: flex;
    justify-content: center;
    align-items: center;
    color: white;
    font-size: 1.2rem;
  }
  
  </style>
  
  