<template>
  <h2>Мои резюме</h2>
  <div class="resume-container">
    <div v-if="resumes.length" class="resume-list">
      <div v-for="resume in resumes" :key="resume.id" class="resume-card">
        <div class="resume-info">
          <div>
            <h3>{{ resume.title }}</h3>
            <p>Дата создания: {{ resume.date }}</p>
            <p :class="statusClass(resume.status)">Статус: {{ resume.status }}</p>
          </div>
          <div class="buttons">
            <button class="edit-btn" @click="editResume(resume.id)">Редактировать</button>
            <button class="delete-btn" @click="deleteResume(resume.id)">Удалить</button>
          </div>
        </div>
      </div>
    </div>
    <p v-else>У вас нет созданных резюме.</p>
    <button class="create-btn" @click="createResume">Создать новое резюме</button>
  </div>
</template>

<script setup>
import { ref } from 'vue';

const resumes = ref([
  { id: 1, title: 'Разработчик Vue.js', date: '2024-03-10', status: 'Опубликовано' },
  { id: 2, title: 'Фронтенд-разработчик', date: '2024-02-20', status: 'Черновик' }
]);
  
const editResume = (id) => alert(`Редактирование резюме ID: ${id}`);
const deleteResume = (id) => resumes.value = resumes.value.filter(r => r.id !== id);
const createResume = () => alert('Создание нового резюме');

const statusClass = (status) => {
  return {
    'published': status === 'Опубликовано',
    'draft': status === 'Черновик'
  };
};
</script>
  