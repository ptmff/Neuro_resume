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
