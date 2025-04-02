<script setup lang="ts">
import { ref, reactive, watch } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'
import { useProfileStore } from '@/stores/profileStore'
import { Cropper } from 'vue-advanced-cropper'
import 'vue-advanced-cropper/dist/style.css'
import { usePhoneFormat } from '@/composables/usePhoneFormat'

const profileStore = useProfileStore()
const auth = useAuthStore()
const router = useRouter()
const { formatPhoneNumber, calculateCursorPosition, normalizePhoneNumber } = usePhoneFormat()

// For displaying formatted phone in the input
const formattedPhone = ref('')
const isPhoneFormatting = ref(false)

interface EducationDto {
  institution: string
  degree: string
  field: string
  startYear: number
  endYear: number
}

const step = ref(1)
const photoFile = ref<File | null>(null)
const photoPreview = ref<string>('')
const cropper = ref()

const registerData = reactive({
  email: '',
  phone: '',
  password: '',
  confirmPassword: '',
  name: '',
  city: '',
  education: [] as EducationDto[],
})

const educationEntry = reactive<EducationDto>({
  institution: '',
  degree: '',
  field: '',
  startYear: new Date().getFullYear(),
  endYear: new Date().getFullYear(),
})

const croppedImage = ref<File | null>(null)

// Initialize phone field
watch(
  () => registerData.phone,
  (newPhone) => {
    if (!isPhoneFormatting.value && newPhone !== formattedPhone.value) {
      formattedPhone.value = formatPhoneNumber(newPhone)
    }
  },
  { immediate: true }
)

const onPhotoChange = (e: Event) => {
  const input = e.target as HTMLInputElement
  if (!input.files?.length) return
  photoFile.value = input.files[0]
  photoPreview.value = URL.createObjectURL(photoFile.value)
}

const getCroppedImage = (): Promise<File | null> => {
  return new Promise((resolve) => {
    const result = cropper.value?.getResult()
    if (!result) return resolve(null)
    const canvas = result.canvas
    canvas?.toBlob((blob: BlobPart) => {
      if (blob) {
        const file = new File([blob], 'cropped.png', { type: 'image/png' })
        croppedImage.value = file
        resolve(file)
      } else {
        resolve(null)
      }
    }, 'image/png')
  })
}

const prevStep = () => {
  if (step.value > 1) step.value--
}

const nextStep = () => {
  if (!registerData.email && !registerData.phone) {
    auth.error = 'Укажите хотя бы email или телефон'
    return
  }
  if (registerData.password !== registerData.confirmPassword) {
    auth.error = 'Пароли не совпадают'
    return
  }
  auth.error = ''
  step.value++
}

const handlePhoneInput = (e: Event) => {
  const input = e.target as HTMLInputElement
  const cursorPosition = input.selectionStart
  const previousValue = formattedPhone.value
  
  // Flag to prevent watch from triggering during manual formatting
  isPhoneFormatting.value = true
  
  // Format the phone number
  const formattedValue = formatPhoneNumber(input.value)
  formattedPhone.value = formattedValue
  registerData.phone = formattedValue
  
  // Calculate new cursor position
  const newPosition = calculateCursorPosition(previousValue, formattedValue, cursorPosition)
  
  // After Vue updates the DOM
  setTimeout(() => {
    input.setSelectionRange(newPosition, newPosition)
    isPhoneFormatting.value = false
  }, 0)
}

const handleRegister = async () => {
  auth.error = ''
  registerData.education = [educationEntry]
  
  // Normalize phone number for submission
  if (registerData.phone) {
    registerData.phone = normalizePhoneNumber(registerData.phone)
  }

  // Кроп фото перед регистрацией
  if (photoFile.value) {
    await getCroppedImage()
  }

  await auth.register({
    email: registerData.email,
    phone: registerData.phone,
    password: registerData.password,
    name: registerData.name,
    city: registerData.city,
    education: registerData.education,
  })

  if (!auth.error && croppedImage.value) {
    await profileStore.uploadPhoto(croppedImage.value)
  }

  if (!auth.error) {
    router.push('/')
  }
}
</script>

<template>
  <div class="auth-wrapper large-mode">
    <div class="auth-box-glow">
      <div class="auth-box">
        <h2 class="auth-title">Регистрация</h2>

        <form @submit.prevent="step === 2 ? handleRegister() : nextStep()" class="space-y-4 flex flex-col justify-between">
          <!-- Шаг 1 -->
          <div v-if="step === 1" class="grid grid-cols-2 gap-8">
            <div class="flex flex-col gap-2">
              <label class="form-label">Почта</label>
              <input v-model="registerData.email" type="email" placeholder="Email" class="form-input" />
              <label class="form-label">Телефон</label>
              <input
                v-model="formattedPhone"
                @input="handlePhoneInput"
                placeholder="+7 (___) ___-__-__"
                class="form-input"
              />
              <label class="form-label">Пароль</label>
              <input v-model="registerData.password" type="password" placeholder="Пароль" class="form-input" required />
              <input v-model="registerData.confirmPassword" type="password" placeholder="Повторите пароль" class="form-input" required />
            </div>

            <div class="flex flex-col gap-2">
              <div class="education-block">
                <h3 class="education-title">Образование</h3>
                <input v-model="educationEntry.institution" type="text" placeholder="Учебное заведение" class="form-input my-2" />
                <input v-model="educationEntry.degree" type="text" placeholder="Степень" class="form-input mb-2" />
                <input v-model="educationEntry.field" type="text" placeholder="Специальность" class="form-input mb-2" />
                <div class="flex gap-4">
                  <input v-model.number="educationEntry.startYear" type="number" placeholder="Год начала" class="form-input w-1/2" />
                  <input v-model.number="educationEntry.endYear" type="number" placeholder="Год окончания" class="form-input w-1/2" />
                </div>
              </div>
              <label class="form-label">Город</label>
              <input v-model="registerData.city" type="text" placeholder="Город" class="form-input" />
            </div>
          </div>

          <!-- Шаг 2 (финальный) -->
          <div v-if="step === 2" class="flex gap-6 min-h-[480px]">
            <div class="flex-1 flex items-center justify-center bg-[var(--background-main)] rounded-2xl overflow-hidden border border-[var(--background-pale)] relative">
              <div v-if="photoPreview" class="relative max-w-full max-h-full overflow-hidden rounded-xl">
                <div class="flex-1 bg-[var(--background-main)] rounded-2xl border border-[var(--background-pale)] overflow-hidden relative">
                  <Cropper
                    ref="cropper"
                    :src="photoPreview"
                    :stencil-props="{
                      aspectRatio: 1,
                      movable: true,
                      resizable: true,
                      lines: {
                        color: 'rgba(255, 255, 255, 0.4)',
                        width: 1
                      }
                    }"
                    :autoZoom="true"
                    :resizeImage="true"
                    :transformImage="true"
                    class="w-full h-full"
                  />
                </div>
              </div>
              <div v-else class="text-[var(--text-mainless)] text-center p-4">Нет фото</div>
            </div>

            <div class="flex-1 flex flex-col gap-4 justify-center">
              <input v-model="registerData.name" type="text" placeholder="Имя" class="form-input" required />
              <label class="form-input text-center cursor-pointer">
                <span v-if="!photoFile">Загрузить фото</span>
                <span v-else>Сменить фото</span>
                <input type="file" accept="image/*" class="hidden w-full" @change="onPhotoChange" />
              </label>
            </div>
          </div>

          <!-- Кнопки -->
          <div class="flex justify-between pt-4 gap-4">
            <button type="button" @click="prevStep" v-if="step > 1" class="submit-btn w-full">Назад</button>
            <button type="submit" class="submit-btn w-full">
              {{ step === 2 ? (auth.loading ? 'Регистрируем...' : 'Завершить регистрацию') : 'Далее' }}
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

.auth-wrapper.large-mode {
  max-width: 100vw;
  max-height: 100vh;
}

.auth-box-glow {
  @apply w-full max-w-5xl;
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
  min-height: 540px;
}

@keyframes glow {
  0% {
    filter: drop-shadow(0 0 6px var(--neon-blue));
  }
  100% {
    filter: drop-shadow(0 0 10px var(--accent));
  }
}

.education-block {
  @apply rounded-2xl bg-transparent shadow-inner;
}

.education-title {
  @apply text-lg ml-2 font-semibold text-[var(--text-main)];
}

.auth-title {
  @apply text-2xl font-semibold text-center text-[var(--text-light)] mb-6;
}

.form-input {
  @apply w-full bg-[var(--background-pale)] border border-[var(--background-pale)] rounded-xl px-4 py-2 text-[var(--text-light)] focus:outline-none focus:border-[var(--background-cta)] mb-3;
}

.form-label {
  @apply text-lg font-semibold text-[var(--text-main)];
}

.submit-btn {
  @apply text-white font-semibold py-2 rounded-xl;
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

