<template>
    <div class="profile-info">
      <img :src="user.avatar" alt="Аватар" class="avatar" />
      <div class="profile-details">
        <h2>{{ user.name }}</h2>
        <p>{{ user.email }}</p>
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

  