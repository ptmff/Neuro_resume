<script setup lang="ts">
import { ref } from "vue";
import { useAuthStore } from "@/stores/authStore";
import { useRouter } from "vue-router";

const auth = useAuthStore();
const router = useRouter();

const email = ref("");
const password = ref("");
const name = ref("");
const phone = ref("");
const city = ref("");

const submit = async () => {
  await auth.register({
    email: email.value,
    password: password.value,
    name: name.value,
    phone: phone.value,
    city: city.value,
  });
  if (!auth.error) {
    router.push("/login");
  }
};
</script>

<template>
  <form @submit.prevent="submit">
    <input v-model="email" placeholder="Email" />
    <input v-model="password" type="password" placeholder="Password" />
    <input v-model="name" placeholder="Name" />
    <input v-model="phone" placeholder="Phone (optional)" />
    <input v-model="city" placeholder="City (optional)" />
    <button :disabled="auth.loading">Register</button>
    <p v-if="auth.error">{{ auth.error }}</p>
  </form>
</template>
