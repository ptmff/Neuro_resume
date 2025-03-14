<template>
  <div class="resume-container">
    <div class="vertical-title">
      <p class="vertical-titlee">Мои резюме</p>
    </div>
    <div v-if="resumes.length" class="resume-list">
      <div v-for="resume in resumes" :key="resume.id" class="resume-card">
        <div class="resume-info">
          <div>
            <h3>{{ resume.title }}</h3>
            <p class="date">Дата создания: {{ resume.date }}</p>
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
    <div class="create-btn">
      <router-link class="nav-link" to="/resume-builder">
        <button class="create-btnn" @click="createResume">+</button>
      </router-link>
    </div>
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
  margin-right: 2.5%;
  font-size: 44px;
  font-weight: bold;
  background-color: #8b5e3c;
  border-radius: 30px;
  color: #f9f5f0;
  width: 10%;
  height: 315px;
  display: flex;
  justify-content: space-around;
  align-items: center;
}
.vertical-titlee {
  margin: 0;
}
.resume-list {
  display: flex;
  flex-direction: column;
  gap: 15px;
  margin-right: 2.5%;
  width: 75%;
}
.resume-card {
  background-color: #f9f5f0;
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
.date {
  color: #666;
}
.published {
  color: #5e5957;
}
.draft {
  color: #968d8a;
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
  background-color: #a57f62;
  color: #f9f5f0;
}
.delete-btn {
  background-color: #8b5e3c;
  color: #f9f5f0;
}
.create-btn {
  background-color: #c4a484;
  color: #f9f5f0;
  height: 315px;
  font-size: 70px;
  width: 10%;
  border-radius: 30px;
  background-color: #8b5e3c;
  display: flex;
  justify-content: space-around;
  align-items: center;
}
.create-btnn {
  background: none;
  border: none;
  color: #f9f5f0;
  margin: 0;
}
.edit-btn:hover {
  background-color: #8b664a;
}
.delete-btn:hover {
  background-color: #af805b;
}
.create-btn:hover {
  background-color: #af805b;
}
</style>