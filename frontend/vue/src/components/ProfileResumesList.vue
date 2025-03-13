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
  width: 60%;
  margin: auto;
  padding: 40px 0;
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: center;
  color: #744b29;
}
.vertical-title {
  writing-mode: vertical-rl;
  transform: rotate(180deg);
  margin: 0;
  margin-right: 20px;
  padding: 15px;
  padding-right: 2%;
  padding-left: 2%;
  font-size: 44px;
  font-weight: bold;
  background-color: #744b29;
  border-radius: 30px;
  color: #fcf0e7;
  width: 10%;
  height: 315px;
}
.resume-list {
  display: flex;
  flex-direction: column;
  gap: 15px;
  margin-right: 20px;
  width: 75%;
}
.resume-card {
  border: 1px solid #faece1;
  background-color: #fcf0e7;
  padding: 15px;
  border-radius: 30px;
  text-align: left;
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
.edit-btn, .delete-btn, .create-btn {
  cursor: pointer;
  transition: background 0.3s;
  padding: 8px 12px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-size: 16px;
}
.edit-btn {
  background-color: #a67c52;
  color: white;
}
.delete-btn {
  background-color: #744b29;
  color: white;
}
.create-btn {
  background-color: #c4a484;
  color: #fcf0e7;
  height: 315px;
  font-size: 70px;
  width: 10%;
  border-radius: 30px;
  background-color: #744b29;
  padding-bottom: 20px;
}
.edit-btn:hover {
  background-color: #8d5c36;
}
.delete-btn:hover {
  background-color: #8d5c36;
}
.create-btn:hover {
  background-color: #8d5c36;
}
</style>