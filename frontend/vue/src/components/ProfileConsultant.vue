<template>
  <div class="ai-consultant">
    <h2>ИИ-консультант</h2>
    <div class="chat-box">
      <div v-for="(message, index) in messages" :key="index" :class="['message', message.type]">
        {{ message.text }}
      </div>
    </div>
    <div class="input-box">
      <input v-model="userInput" type="text" placeholder="Задайте вопрос..." @keyup.enter="sendMessage" />
      <button @click="sendMessage">Отправить</button>
    </div>
  </div>
</template>

  
<script setup>
import { ref } from 'vue';

const messages = ref([
  { text: 'Привет! Я ваш ИИ-консультант. Чем могу помочь?', type: 'bot' }
]);
const userInput = ref('');

const sendMessage = () => {
  if (userInput.value.trim() === '') return;
  
  messages.value.push({ text: userInput.value, type: 'user' });
  
  setTimeout(() => {
    messages.value.push({ text: 'Спасибо за ваш вопрос! Я обрабатываю ваш запрос...', type: 'bot' });
  }, 1000);
  
  userInput.value = '';
};
</script>
