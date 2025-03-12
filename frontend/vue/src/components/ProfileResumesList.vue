<template>
  <div class="resume-container">
    <h2 class="vertical-title">Мои резюме</h2>
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
    <button class="create-btn" @click="createResume">+</button>
  </div>
</template>

<script setup>
import { ref } from 'vue';

const resumes = ref([
  { id: 1, title: 'Разработчик Vue.js', date: '08.03.2025', status: 'Опубликовано' },
  { id: 2, title: 'Фронтенд-разработчик', date: '13.03.2025', status: 'Черновик' }
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

<style scoped>
.resume-container {
  margin: 0 280px;
  padding: 40px 80px;
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: space-between;
  color: #744b29;
}
.vertical-title {
  writing-mode: vertical-rl;
  transform: rotate(180deg);
  margin: 0;
  margin-right: 30px;
  font-size: 48px;
  font-weight: bold;
}
.resume-list {
  display: flex;
  flex-direction: column;
  gap: 15px;
  margin-right: 30px;
}
.resume-card {
  border: 1px solid #faece1;
  background-color: #fcf0e7;
  padding: 15px;
  border-radius: 8px;
  text-align: left;
  width: 600px;
  height: 150px;
}
.resume-info {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
.published {
  color: #643d1e;
}
.draft {
  color: #a87d5a;
}
.buttons {
  display: flex;
  flex-direction: column;
  gap: 10px;
}
</style>