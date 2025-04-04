<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useProfileStore } from '@/stores/profileStore'

const profileStore = useProfileStore()
const loading = ref(true)
const verifyingEmail = ref(false)
const verifyingPhone = ref(false)
const verificationSent = ref({
  email: false,
  phone: false
})

// Get profile data
onMounted(async () => {
  if (!profileStore.profile) {
    await profileStore.fetchProfile()
  }
  loading.value = false
})

// Computed properties to check if email/phone exists and if they're verified
const hasEmail = computed(() => !!profileStore.profile?.email)
const hasPhone = computed(() => !!profileStore.profile?.phone)
const isEmailVerified = computed(() => !!profileStore.profile?.emailVerified)
const isPhoneVerified = computed(() => !!profileStore.profile?.phoneVerified)

// Email to display
const emailDisplay = computed(() => {
  if (!hasEmail.value) return 'Не указан'
  return profileStore.profile?.email
})

// Phone to display (formatted)
const phoneDisplay = computed(() => {
  if (!hasPhone.value) return 'Не указан'
  return profileStore.profile?.phone
})

const verifyEmail = async () => {
  if (!hasEmail.value) return
  
  try {
    verifyingEmail.value = true
    // Assuming there's a method in profileStore to send verification email
    await profileStore.sendEmailVerification()
    verificationSent.value.email = true
  } catch (error) {
    console.error('Failed to send email verification:', error)
  } finally {
    verifyingEmail.value = false
  }
}

const verifyPhone = async () => {
  if (!hasPhone.value) return
  
  try {
    verifyingPhone.value = true
    // Assuming there's a method in profileStore to send SMS verification
    await profileStore.sendPhoneVerification()
    verificationSent.value.phone = true
  } catch (error) {
    console.error('Failed to send phone verification:', error)
  } finally {
    verifyingPhone.value = false
  }
}
</script>

<template>
  <div class="space-y-6">
    <h2 class="text-2xl font-semibold text-[var(--text-light)]">Подтверждение контактов</h2>
    
    <div v-if="loading" class="py-8 text-center">
      <div class="inline-block h-8 w-8 animate-spin rounded-full border-4 border-solid border-current border-r-transparent align-[-0.125em] motion-reduce:animate-[spin_1.5s_linear_infinite]"></div>
      <p class="mt-2 text-[var(--text-mainless)]">Загрузка данных...</p>
    </div>
    
    <template v-else>
      <!-- Email Verification -->
      <div class="bg-[var(--background-pale)] p-4 rounded-xl border border-[var(--background-overlay)]">
        <p class="text-[var(--text-light)] mb-2">Почта:</p>
        <div class="flex justify-between items-center">
          <span class="text-[var(--text-mainless)]">{{ emailDisplay }}</span>
          
          <div v-if="hasEmail">
            <button
              v-if="!isEmailVerified"
              class="px-4 py-2 rounded-full text-sm font-medium transition-all duration-200"
              :class="verificationSent.email ? 'bg-yellow-600 text-white' : 'bg-[var(--background-cta)] text-white hover:brightness-110'"
              @click="verifyEmail"
              :disabled="verifyingEmail"
            >
              <span v-if="verifyingEmail">Отправка...</span>
              <span v-else-if="verificationSent.email">Отправлено</span>
              <span v-else>Подтвердить</span>
            </button>
            <span v-else class="px-4 py-2 rounded-full text-sm font-medium bg-green-600 text-white">
              Подтверждено
            </span>
          </div>
        </div>
      </div>

      <!-- Phone Verification -->
      <div class="bg-[var(--background-pale)] p-4 rounded-xl border border-[var(--background-overlay)]">
        <p class="text-[var(--text-light)] mb-2">Телефон:</p>
        <div class="flex justify-between items-center">
          <span class="text-[var(--text-mainless)]">{{ phoneDisplay }}</span>
          
          <div v-if="hasPhone">
            <button
              v-if="!isPhoneVerified"
              class="px-4 py-2 rounded-full text-sm font-medium transition-all duration-200"
              :class="verificationSent.phone ? 'bg-yellow-600 text-white' : 'bg-[var(--background-cta)] text-white hover:brightness-110'"
              @click="verifyPhone"
              :disabled="verifyingPhone"
            >
              <span v-if="verifyingPhone">Отправка...</span>
              <span v-else-if="verificationSent.phone">Отправлено</span>
              <span v-else>Подтвердить</span>
            </button>
            <span v-else class="px-4 py-2 rounded-full text-sm font-medium bg-green-600 text-white">
              Подтверждено
            </span>
          </div>
          
          <router-link 
            v-else 
            to="/profile/edit" 
            class="px-4 py-2 rounded-full text-sm font-medium bg-[var(--background-cta)] text-white hover:brightness-110 transition-all duration-200"
          >
            Добавить
          </router-link>
        </div>
      </div>
      
      <!-- Verification Instructions -->
      <div v-if="hasEmail && !isEmailVerified && verificationSent.email" class="mt-4 p-4 bg-yellow-50 text-yellow-800 rounded-xl">
        <p class="text-sm">
          Мы отправили письмо с инструкциями по подтверждению на адрес <strong>{{ emailDisplay }}</strong>. 
          Пожалуйста, проверьте вашу почту и следуйте инструкциям в письме.
        </p>
      </div>
      
      <div v-if="hasPhone && !isPhoneVerified && verificationSent.phone" class="mt-4 p-4 bg-yellow-50 text-yellow-800 rounded-xl">
        <p class="text-sm">
          Мы отправили SMS с кодом подтверждения на номер <strong>{{ phoneDisplay }}</strong>.
          Введите полученный код:
        </p>
        <div class="mt-2 flex gap-2">
          <input type="text" maxlength="6" placeholder="Код из SMS" class="form-input" />
          <button class="px-4 py-2 rounded-lg bg-[var(--background-cta)] text-white hover:brightness-110 transition-all duration-200">
            Проверить
          </button>
        </div>
      </div>
      
      <!-- No Contact Info Warning -->
      <div v-if="!hasEmail && !hasPhone" class="mt-6 p-4 bg-red-50 text-red-800 rounded-xl">
        <p class="font-medium">Внимание!</p>
        <p class="mt-1 text-sm">
          У вас не указаны контактные данные. Для безопасности вашего аккаунта рекомендуем 
          <router-link to="/profile/edit" class="underline font-medium">добавить</router-link> 
          и подтвердить хотя бы один способ связи.
        </p>
      </div>
    </template>
  </div>
</template>

<style scoped>
.form-input {
  @apply bg-[var(--background-pale)] border border-[var(--background-pale)] rounded-xl px-4 py-2 text-[var(--text-light)] focus:outline-none focus:border-[var(--background-cta)];
}
</style>