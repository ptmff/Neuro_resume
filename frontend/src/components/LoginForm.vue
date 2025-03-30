<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'
import { useProfileStore } from '@/stores/profileStore'

const emailOrPhone = ref('')
const password = ref('')
const auth = useAuthStore()
const router = useRouter()
const profileStore = useProfileStore()

const handleLogin = async () => {
  const isEmail = emailOrPhone.value.includes('@')

  const payload = {
    email: isEmail ? emailOrPhone.value : '',
    phone: isEmail ? '' : emailOrPhone.value,
    password: password.value,
  }

  await auth.login(payload)

  if (!auth.error) {
    await profileStore.fetchProfile()
    router.push('/')
  }
}
</script>

<template>
  <div class="auth-wrapper">
    <div class="auth-box-glow">
      <div class="auth-box">
        <h2 class="auth-title">
          Войти в аккаунт
        </h2>

        <form @submit.prevent="handleLogin" class="space-y-4">
          <input
            v-model="emailOrPhone"
            type="text"
            placeholder="Email или телефон"
            class="form-input"
            required
          />
          <input
            v-model="password"
            type="password"
            placeholder="Пароль"
            class="form-input"
            required
          />
          <button
            type="submit"
            :disabled="auth.loading"
            class="submit-btn"
          >
            {{ auth.loading ? 'Вход...' : 'Войти' }}
          </button>
        </form>

        <p v-if="auth.error" class="text-red-500 text-sm mt-4 text-center">
          {{ auth.error }}
        </p>

        <p class="text-sm text-center mt-4 text-[var(--text-mainless)]">
          Нет аккаунта?
          <router-link to="/register" class="fancy-link">Зарегистрироваться</router-link>
        </p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.auth-wrapper {
  @apply fixed inset-0 z-[999] flex items-center justify-center p-4;
}

.auth-box-glow {
  @apply w-full max-w-md;
  position: relative;
  border-radius: 1.5rem;
  padding: 2px;
  background: var(--gradient-primary);
  animation: glow 3s ease-in-out infinite alternate;
}

.auth-box {
  background-color: var(--background-section);
  border-radius: 1.5rem;
  padding: 2rem;
  position: relative;
  z-index: 1;
  @apply w-full;
}

@keyframes glow {
  0% {
    filter: brightness(1.1) drop-shadow(0 0 6px var(--neon-blue));
  }
  100% {
    filter: brightness(1.4) drop-shadow(0 0 10px var(--accent));
  }
}

.auth-title {
  @apply text-2xl font-semibold text-center text-[var(--text-light)] mb-6;
}

.form-input {
  @apply w-full bg-[var(--background-pale)] border border-[var(--background-pale)] rounded-xl px-4 py-2 text-[var(--text-light)] focus:outline-none focus:border-[var(--background-cta)];
}

.submit-btn {
  @apply w-full text-white font-semibold py-2 rounded-xl;
  background: linear-gradient(135deg, #9d00ff, #00e5ff);
  background-size: 200%;
  background-position: left center;
  transition: all 0.4s ease-in-out;
  filter: brightness(1);
}

.submit-btn:hover {
  background-position: center;
  filter: brightness(1.1);
}

.fancy-link {
  @apply relative font-semibold text-[var(--text-light)] transition-all duration-300;
  background: var(--text-mainless);
  background-clip: text;
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  display: inline;
  padding: 0;
  line-height: 1;
}

.fancy-link::after {
  content: "";
  position: absolute;
  left: 0;
  bottom: 0;
  height: 2px;
  width: 100%;
  background: var(--gradient-primary);
  transform: scaleX(0);
  transform-origin: left;
  transition: transform 0.4s ease-in-out;
  border-radius: 9999px;
}

.fancy-link:hover::after {
  transform: scaleX(1);
}

</style>