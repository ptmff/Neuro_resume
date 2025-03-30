<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'
import { useProfileStore } from '@/stores/profileStore'
const profileStore = useProfileStore()

interface EducationDto {
  institution: string
  degree: string
  field: string
  startYear: number
  endYear: number
}

const step = ref(1)
const photoFile = ref<File | null>(null)
const auth = useAuthStore()
const router = useRouter()

const registerData = reactive({
  email: '',
  phone: '',
  password: '',
  confirmPassword: '',
  name: '',
  city: '',
  education: [] as EducationDto[],
})

const onPhotoChange = (e: Event) => {
  const input = e.target as HTMLInputElement
  if (!input.files?.length) return
  photoFile.value = input.files[0]
}

const addEducation = () => {
  registerData.education.push({
    institution: '',
    degree: '',
    field: '',
    startYear: new Date().getFullYear(),
    endYear: new Date().getFullYear(),
  })
}

const removeEducation = (index: number) => {
  registerData.education.splice(index, 1)
}

const nextStep = () => {
  if (step.value === 1 && registerData.password !== registerData.confirmPassword) {
    auth.error = 'Пароли не совпадают'
    return
  }
  auth.error = ''
  if (step.value < 3) step.value++
}

const prevStep = () => {
  if (step.value > 1) step.value--
}

const handleRegister = async () => {
  await auth.register({
    email: registerData.email,
    phone: registerData.phone,
    password: registerData.password,
    name: registerData.name,
    city: registerData.city,
    education: registerData.education,
  })

  if (!auth.error && photoFile.value) {
  await profileStore.uploadPhoto(photoFile.value)
}


  if (!auth.error) {
    router.push('/')
  }
}
</script>

<template>
  <div class="auth-wrapper">
    <div class="auth-box-glow">
      <div class="auth-box">
        <h2 class="auth-title">Регистрация — Шаг {{ step }} из 3</h2>

        <form @submit.prevent="step === 3 ? handleRegister() : nextStep()" class="space-y-4">
          <!-- Шаг 1 -->
          <div v-if="step === 1" class="space-y-4">
            <input v-model="registerData.email" type="email" placeholder="Email (или оставьте пустым)" class="form-input" />
            <input v-model="registerData.phone" type="text" placeholder="Телефон (или оставьте пустым)" class="form-input" />
            <input v-model="registerData.password" type="password" placeholder="Пароль" class="form-input" required />
            <input v-model="registerData.confirmPassword" type="password" placeholder="Повторите пароль" class="form-input" required />
          </div>

          <!-- Шаг 2 -->
          <div v-if="step === 2" class="space-y-4">
            <input v-model="registerData.name" type="text" placeholder="Имя" class="form-input" required />
            <input type="file" accept="image/*" class="form-input" @change="onPhotoChange" />
          </div>

          <!-- Шаг 3 -->
          <div v-if="step === 3" class="space-y-4">
            <input v-model="registerData.city" type="text" placeholder="Город (необязательно)" class="form-input" />

            <div v-for="(edu, i) in registerData.education" :key="i" class="space-y-2 border border-[var(--background-pale)] p-2 rounded-xl">
              <input v-model="edu.institution" type="text" placeholder="Учебное заведение" class="form-input" />
              <input v-model="edu.degree" type="text" placeholder="Степень" class="form-input" />
              <input v-model="edu.field" type="text" placeholder="Специальность" class="form-input" />
              <input v-model.number="edu.startYear" type="number" placeholder="Год начала" class="form-input" />
              <input v-model.number="edu.endYear" type="number" placeholder="Год окончания" class="form-input" />
              <button type="button" class="submit-btn" @click="removeEducation(i)">Удалить</button>
            </div>

            <button type="button" class="submit-btn" @click="addEducation">Добавить образование</button>
          </div>

          <!-- Навигация -->
          <div class="flex justify-between pt-4">
            <button type="button" @click="prevStep" v-if="step > 1" class="submit-btn">Назад</button>
            <button type="submit" class="submit-btn">
              {{ step === 3 ? (auth.loading ? 'Регистрируем...' : 'Зарегистрироваться') : 'Далее' }}
            </button>
          </div>

          <p v-if="auth.error" class="text-red-500 text-sm mt-4 text-center">
            {{ auth.error }}
          </p>
        </form>

        <p class="text-sm text-center mt-4 text-[var(--text-mainless)]">
          Уже есть аккаунт?
          <router-link to="/login" class="fancy-link">Войти</router-link>
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
