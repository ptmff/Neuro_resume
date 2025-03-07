<template>
    <div class="container p-5">
    <h2 class="title mb-5" >Выберите стиль</h2>
      <div class="row">
        <div class="col-md-6 mb-3" v-for="(template, index) in templates" :key="index">
          <div 
            class="template-card p-5" 
            :class="{ 'selected': selectedTemplate === index }"
            @click="$emit('select-template', index)"
          >
            <img :src="template.image" class="img-fluid mb-2" alt="Шаблон резюме">
            <h5 class="template-name">{{ template.name }}</h5>
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
      templates: {
        type: Array,
        required: true
      },
      selectedTemplate: {
        type: Number,
        required: true
      }
    },
    emits: ['select-template', 'next-step', 'prev-step']
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
  
  /* Шаблоны */
  .template-card {
    border: 2px solid #e0d5c8;
    border-radius: 6px;
    cursor: pointer;
    transition: all 0.3s;
  }
  
  .template-name {
    font-size: 0.9rem;
    text-align: center;
    margin-bottom: 0;
  }
  </style>
  
  