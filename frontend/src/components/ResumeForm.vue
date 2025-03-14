<template>
    <div class="bg-[var(--background-section)] bg-opacity-30 backdrop-blur-xl p-8 rounded-3xl border border-white/10">
      <h2 class="text-4xl font-bold bg-gradient-to-r from-[var(--text-secondary)] to-[var(--text-light)] bg-clip-text text-transparent mb-8">
        Заполните данные
      </h2>
      <form @submit.prevent="$emit('next-step')">
        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <FormField
            id="name"
            label="Имя и Фамилия"
            v-model="resumeData.name"
            placeholder="Имя Фамилия"
            required
            @input="validateName"
          />
          <FormField
            id="email"
            label="Email"
            type="email"
            v-model="resumeData.email"
            placeholder="name@example.com"
            required
          />
          <FormField
            id="phone"
            label="Телефон"
            type="tel"
            v-model="resumeData.phone"
            placeholder="+7 (999) 999-99-99"
            required
            @input="formatPhone"
          />
          <FormField
            id="location"
            label="Город"
            v-model="resumeData.location"
            placeholder="Москва"
            required
          />
        </div>
        
        <FormField
          id="profession"
          label="Профессия"
          v-model="resumeData.profession"
          placeholder="Профессия"
          required
          class="mt-6"
        />
        <FormField
          id="education"
          label="Образование"
          type="textarea"
          v-model="resumeData.education"
          placeholder="Например: МГУ, Факультет информатики, 2015-2019"
          required
          class="mt-6"
        />
        <FormField
          id="experience"
          label="Опыт работы"
          type="textarea"
          v-model="resumeData.experience"
          placeholder="Опишите ваш опыт работы, должности и обязанности"
          required
          class="mt-6"
        />
        <FormField
          id="skills"
          label="Навыки"
          type="textarea"
          v-model="resumeData.skills"
          placeholder="Перечислите ваши профессиональные навыки"
          required
          class="mt-6"
        />
        <div class="flex justify-between mt-8">
          <button type="button" class="btn btn-secondary" disabled>Назад</button>
          <button type="submit" class="btn btn-primary">Далее</button>
        </div>
      </form>
    </div>
  </template>
 <script>

 import FormField from './FormField.vue';
 
 export default {
     components:{
         FormField
     },
     props: {
         resumeData: {
         type: Object,
         required: true
         }
     },
     emits: ['next-step'],
     methods: {
         formatPhone(event) {
             let value = event.target.value.replace(/\D/g, ''); // Убираем всё, кроме цифр
 
             if (value.startsWith('8')) {
                 value = '7' + value.slice(1);
             }
             
             if (value.length > 11) {
                 value = value.slice(0, 11); // Ограничение 11 цифр
             }
 
             let formatted = '+7 ';
             if (value.length > 1) formatted += `(${value.slice(1, 4)}`;
             if (value.length > 4) formatted += `) ${value.slice(4, 7)}`;
             if (value.length > 7) formatted += `-${value.slice(7, 9)}`;
             if (value.length > 9) formatted += `-${value.slice(9, 11)}`;
 
             this.resumeData.phone = formatted;
         },
         validateName(event) {
             let value = event.target.value;
             value = value.replace(/[^А-Яа-яЁёA-Za-z\s-]/g, ''); // Разрешаем буквы, пробел и дефис
             value = value.replace(/-{2,}/g, '-'); // Убираем двойные дефисы
             value = value.replace(/^\-|\-$/g, ''); // Убираем дефисы в начале и конце
             this.resumeData.name = value;
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
    @apply bg-[var(--background-section)] bg-opacity-50 text-[var(--text-light)];
  }
  </style>
  
  