<template>
    <div class="bg-[var(--background-section)] bg-opacity-30 backdrop-blur-xl p-8 rounded-3xl border border-white/10">
      <h2 class="text-4xl font-bold bg-gradient-to-r from-[var(--text-secondary)] to-[var(--text-light)] bg-clip-text text-transparent mb-8">
        Ваше резюме готово!
      </h2>
      <div class="download-options p-6 rounded-2xl bg-gradient-to-br from-[var(--neon-purple)]/10 to-[var(--neon-blue)]/10">
        <h3 class="text-2xl font-semibold text-[var(--text-light)] mb-4">Выберите формат для скачивания:</h3>
        <div class="flex space-x-4 mb-8">
          <button @click="$emit('download-resume', 'pdf')" class="btn btn-primary flex-1">
            <i class="fas fa-file-pdf mr-2"></i>
            <span>Скачать PDF</span>
          </button>
          <button @click="$emit('download-resume', 'docx')" class="btn btn-primary flex-1">
            <i class="fas fa-file-word mr-2"></i>
            <span>Скачать DOCX</span>
          </button>
        </div>
        
        <h3 class="text-2xl font-semibold text-[var(--text-light)] mb-4">Отправить работодателю</h3>
        <div class="flex items-center space-x-4"> <!-- Используем flex для выравнивания -->
          <FormField
            class="flex-grow"
            id="employerEmail"
            label="Email работодателя"
            type="email"
            :modelValue="employerEmail"
            @update:modelValue="$emit('update:employerEmail', $event)"
            placeholder="employer@company.com"
          />
          <button @click="sendToEmployer" class="btn btn-primary h-[50px]"> 
            <i class="fas fa-paper-plane mr-2 "></i>
            <span>Отправить</span>
          </button>
        </div>
      </div>
      <div class="flex justify-between mt-3">
        <button @click="$emit('prev-step')" class="btn btn-secondary">Назад</button>
        <button @click="$emit('create-new-resume')" class="btn btn-primary">
          Создать новое резюме
        </button>
      </div>
    </div>
  </template>
  
  <script>
  import FormField from './FormField.vue';
  
  export default {
    components: {
      FormField
    },
    props: {
      employerEmail: {
        type: String,
        required: true
      }
    },
    emits: [
      'download-resume',  
      'update:employerEmail', 
      'send-to-employer', 
      'create-new-resume', 
      'prev-step'
    ],
    methods: {
      sendToEmployer() {
        if (!this.employerEmail) {
          alert('Пожалуйста, введите email работодателя.');
          return;
        }
  
        const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailPattern.test(this.employerEmail)) {
          alert('Пожалуйста, введите корректный email.');
          return;
        }
        alert('Email успешно проверен: ' + this.employerEmail);
      }
    }
  };
  </script>
  
  <style scoped>
  .btn {
    @apply px-6 py-3 font-semibold rounded-full transition-all duration-300 transform hover:scale-105 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-[var(--neon-purple)];
  }
  
  .btn-primary {
    @apply bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] text-white;
  }
  
  .btn-secondary {
    @apply bg-[var(--background-section)] bg-opacity-50 text-[var(--text-light)] border border-white/10;
  }
  </style>