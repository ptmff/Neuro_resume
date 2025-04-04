<template>
    <div class="space-y-6">
      <h2 class="text-xl font-bold text-[var(--text-light)]">Ваши личные данные</h2>
  
      <div>
        <label class="block text-sm text-[var(--text-mainless)] mb-1">Имя</label>
        <input v-model="name" class="form-input" type="text" placeholder="Введите имя" />
      </div>
  
      <div>
        <label class="block text-sm text-[var(--text-mainless)] mb-1">Email</label>
        <input v-model="email" class="form-input" type="email" placeholder="Введите email" />
      </div>
  
      <div>
        <label class="block text-sm text-[var(--text-mainless)] mb-1">Телефон</label>
        <input 
          v-model="formattedPhone" 
          @input="handlePhoneInput" 
          class="form-input" 
          type="tel" 
          placeholder="+7 (___) ___-__-__" 
        />
      </div>
  
      <div>
        <label class="block text-sm text-[var(--text-mainless)] mb-1">Город</label>
        <input v-model="city" class="form-input" type="text" placeholder="Введите город" />
      </div>
  
      <!-- Education Section -->
      <div class="mt-8">
        <div class="flex items-center justify-between mb-2">
          <h2 class="text-xl font-bold text-[var(--text-light)]">Образование</h2>
          <button 
            v-if="education.length > 0" 
            @click="addNewEducation" 
            class="text-sm bg-[var(--background-cta)] text-white px-3 py-1 rounded-lg hover:brightness-110 transition"
          >
            Добавить
          </button>
        </div>
  
        <div v-if="education.length === 0" class="text-center py-6 bg-[var(--background-pale)] rounded-xl">
          <p class="text-[var(--text-mainless)]">Нет данных об образовании</p>
          <button 
            @click="addNewEducation" 
            class="mt-2 bg-[var(--background-cta)] text-white px-4 py-2 rounded-lg hover:brightness-110 transition"
          >
            Добавить образование
          </button>
        </div>
  
        <div v-for="(edu, index) in education" :key="index" class="bg-[var(--background-pale)] p-4 rounded-xl mb-4">
          <div class="flex justify-between items-center mb-3">
            <h3 class="font-semibold text-[var(--text-light)]">Образование {{ index + 1 }}</h3>
            <button 
              @click="removeEducation(index)" 
              class="text-red-500 hover:text-red-600 transition"
            >
              Удалить
            </button>
          </div>
  
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label class="block text-sm text-[var(--text-mainless)] mb-1">Учебное заведение</label>
              <input v-model="edu.institution" class="form-input" type="text" placeholder="Название учебного заведения" />
            </div>
            
            <div>
              <label class="block text-sm text-[var(--text-mainless)] mb-1">Степень</label>
              <input v-model="edu.degree" class="form-input" type="text" placeholder="Бакалавр, магистр и т.д." />
            </div>
            
            <div>
              <label class="block text-sm text-[var(--text-mainless)] mb-1">Специальность</label>
              <input v-model="edu.field" class="form-input" type="text" placeholder="Направление обучения" />
            </div>
            
            <div class="grid grid-cols-2 gap-2">
              <div>
                <label class="block text-sm text-[var(--text-mainless)] mb-1">Год начала</label>
                <input v-model.number="edu.startYear" class="form-input" type="number" placeholder="Год" />
              </div>
              <div>
                <label class="block text-sm text-[var(--text-mainless)] mb-1">Год окончания</label>
                <input v-model.number="edu.endYear" class="form-input" type="number" placeholder="Год" />
              </div>
            </div>
          </div>
        </div>
      </div>
  
      <button
        @click="saveChanges"
        class="bg-[var(--background-cta)] text-white px-4 py-2 rounded-xl mt-4 hover:brightness-110 active:scale-95 transition w-full"
      >
        Сохранить изменения
      </button>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref, reactive } from 'vue'
  import { useProfileStore } from '@/stores/profileStore'
  import { usePhoneFormat } from '@/composables/usePhoneFormat'
  
  const profileStore = useProfileStore()
  const profile = profileStore.profile
  const { formatPhoneNumber, calculateCursorPosition, normalizePhoneNumber } = usePhoneFormat()
  
  // For phone formatting
  const formattedPhone = ref(formatPhoneNumber(profile?.phone || ''))
  const isPhoneFormatting = ref(false)
  
  // Basic profile data
  const name = ref(profile?.name || '')
  const email = ref(profile?.email || '')
  const phone = ref(profile?.phone || '')
  const city = ref(profile?.city || '')
  
  // Education data
  interface EducationDto {
    institution: string
    degree: string
    field: string
    startYear: number
    endYear: number
  }
  
  const education = reactive<EducationDto[]>(
    profile?.education && profile.education.length > 0
      ? [...profile.education]
      : []
  )
  
  // Phone formatting handler
  const handlePhoneInput = (e: Event) => {
    const input = e.target as HTMLInputElement
    const cursorPosition = input.selectionStart
    const previousValue = formattedPhone.value
    
    // Flag to prevent watch from triggering during manual formatting
    isPhoneFormatting.value = true
    
    // Format the phone number
    const formattedValue = formatPhoneNumber(input.value)
    formattedPhone.value = formattedValue
    phone.value = formattedValue
    
    // Calculate new cursor position
    const newPosition = calculateCursorPosition(previousValue, formattedValue, cursorPosition)
    
    // After Vue updates the DOM
    setTimeout(() => {
      if (cursorPosition !== null) {
        input.setSelectionRange(newPosition, newPosition)
      }
      isPhoneFormatting.value = false
    }, 0)
  }
  
  // Education management
  const addNewEducation = () => {
    education.push({
      institution: '',
      degree: '',
      field: '',
      startYear: new Date().getFullYear() - 4,
      endYear: new Date().getFullYear(),
    })
  }
  
  const removeEducation = (index: number) => {
    education.splice(index, 1)
  }
  
  // Save all changes
  const saveChanges = async () => {
    // Normalize phone number for submission
    const normalizedPhone = normalizePhoneNumber(phone.value)
    
    await profileStore.updateProfile({
      name: name.value,
      email: email.value,
      phone: normalizedPhone,
      city: city.value,
      education: education
    })
    
    await profileStore.fetchProfile()
  }
  </script>
  
  <style scoped>
  .form-input {
    @apply w-full bg-[var(--background-pale)] border border-[var(--background-pale)] rounded-xl px-4 py-2 text-[var(--text-light)] focus:outline-none focus:border-[var(--background-cta)];
  }
  </style>