<template>
  <div class="ai-consultant">
    <h2>ИИ-консультант</h2>
    <div class="chat-box">
      <div v-for="(message, index) in messages" :key="index" :class="['message', message.type]">
        {{ message.text }}
      </div>
    </div>
    <div class="input-box">
      <input v-model="userInput" type="text" placeholder="Задайте вопрос..." @keyup.enter="sendMessage" class="input-message"/>
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

<style scoped>
.ai-consultant {
  max-width: 970px;
  margin: auto;
  padding: 20px;
  border: 1px solid #fcf0e7;
  background-color: #fcf0e7;
  border-radius: 10px;
  text-align: center;
  color: #8b5e3c;
}
.chat-box {
  height: 300px;
  overflow-y: auto;
  border: 1px solid #ccc;
  padding: 10px;
  margin-bottom: 10px;
  background: white;
  border-radius: 5px;
}
.message {
  padding: 8px;
  margin: 5px 0;
  border-radius: 5px;
  max-width: 80%;
}
.bot {
  background-color: #fdeadb;
  text-align: left;
}
.user {
  background-color: #fad8be;
  color: #8b5e3c;
  text-align: right;
  margin-left: auto;
}
.input-box {
  display: flex;
  gap: 10px;
}
input {
  flex: 1;
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 5px;
  background-color: #fff;
  color: #8b5e3c;
}
button {
  padding: 10px 15px;
  border: none;
  background-color: #a67c52;
  color: white;
  border-radius: 5px;
  cursor: pointer;
}
button:hover {
  background-color: #8b5e3c;
}
</style>