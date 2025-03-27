<script setup lang="ts">
import { ref } from "vue";
import { useAuthStore } from "@/stores/authStore";
import { useRouter } from "vue-router";

const email = ref("");
const password = ref("");
const auth = useAuthStore();
const router = useRouter();

const submit = async () => {
  await auth.login(email.value, password.value);
  if (!auth.error) {
    router.push("/profile"); // или куда нужно
  }
};
</script>

<template>
  <form @submit.prevent="submit">
    <input v-model="email" placeholder="Email" />
    <input v-model="password" type="password" placeholder="Password" />
    <button :disabled="auth.loading">Login</button>
    <p v-if="auth.error">{{ auth.error }}</p>
  </form>
</template>
