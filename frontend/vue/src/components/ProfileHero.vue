<template>
    <div class="profile-info">
      <img :src="user.avatar" alt="Аватар" class="avatar" />
      <div class="profile-details">
        <h2 class="name">{{ user.name }}</h2>
        <p class="email">{{ user.email }}</p>
        <p class="profession">{{ user.profession }}</p>
        <button class="edit-button" @click="editProfile">Редактировать</button>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue';
  
  // Данные пользователя
  const user = ref({
    name: 'Иван Иванов',
    profession: 'Фронтенд-разработчик',
    email: 'ivan@example.com',
    avatar: 'https://images.icon-icons.com/1999/PNG/512/avatar_man_people_person_profile_user_icon_123385.png',
  });
  
  // Метод редактирования профиля
  const editProfile = () => alert('Редактирование профиля');
  </script>

    
<style scoped>
.profile-info {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 150px;
  padding: 40px 80px;
  text-align: center;
  color: #744b29;
  background-color: #fcf0e7;
  margin: 0 200px;
  border-radius: 30px;
}
.avatar {
  width: 200px;
  height: 200px;
  border-radius: 50%;
}
</style>