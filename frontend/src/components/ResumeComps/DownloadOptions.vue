<script setup lang="ts">
import { useResumeStore } from '@/stores/resumesStore'
import { useProfileStore } from '@/stores/profileStore'
import FormField from './FormField.vue'
import { computed, ref, watch } from 'vue'
import html2pdf from 'html2pdf.js'

interface Resume {
  id: number
  title: string
  date: string
  city: string
  job: string
  name?: string
  email?: string
  phone?: string
  profession?: string
  location?: string
  experience: {
    company: string
    position: string
    startDate: string
    endDate: string
    description: string
  }[]
  education: {
    institution: string
    degree: string
    field: string
    startYear: number
    endYear: number
  }[]
  skills: string[]
  template: string
  description: string
}

interface Profile {
  email: string
  name?: string
  phone?: string
  city?: string
  photo?: string
  education?: {
    institution: string
    degree: string
    field: string
    startYear: number
    endYear: number
  }[]
  mainResumeId?: number | null
}

interface DownloadOptionsProps {
  employerEmail?: string
  resumeData?: Partial<Resume>
}

const props = defineProps<DownloadOptionsProps>()

const emit = defineEmits<{
  (e: 'download-resume', format: 'pdf' | 'docx'): void
  (e: 'update:employerEmail', email: string): void
  (e: 'send-to-employer', email: string): void
  (e: 'create-new-resume'): void
  (e: 'prev-step'): void
  (e: 'update:modelValue', resume: Partial<Resume>): void
}>()

const resumeStore = useResumeStore()
const profileStore = useProfileStore()
const profile = computed(() => profileStore.profile)

// Функция для обработки изменения email с правильной типизацией
const handleEmailChange = (event: Event) => {
  const target = event.target as HTMLInputElement
  emit('update:employerEmail', target.value)
}

// Добавляем computed свойство для валидации email
const isValidEmail = computed(() => {
  if (!props.employerEmail) return false;
  const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailPattern.test(props.employerEmail);
});

const generatePDF = () => {
  alert('Кнопка нажата!')
  const element = document.getElementById('resume-content')
  html2pdf().from(element).save('myDocument.pdf')
}

const sendToEmployer = (): void => {
  if (!isValidEmail.value) {
    alert('Пожалуйста, введите корректный email.')
    return
  }

  emit('send-to-employer', props.employerEmail || '')
  alert('Резюме успешно отправлено на адрес: ' + props.employerEmail)
}

const saveResume = async (): Promise<void> => {
  if (!props.resumeData) {
    alert('Нет данных для сохранения')
    return
  }

  const resume = {
    id: props.resumeData.id || Date.now(),
    title: props.resumeData.title || 'Новое резюме',
    date: new Date(props.resumeData.date || Date.now()).toISOString(),
    city: props.resumeData.city || '',
    job: props.resumeData.job || '',
    skills: props.resumeData.skills || [],
    experience: props.resumeData.experience || [],
    education: props.resumeData.education || [], // обязательно, иначе TS-ошибка
    template: props.resumeData.template || 'default',
    description: props.resumeData.description || '',
  } satisfies Resume

  const exists = resumeStore.resumes.find(r => r.id === resume.id)

  try {
    if (exists) {
      await resumeStore.updateResume(resume)
    } else {
      await resumeStore.addResume(resume)

      if (profile.value && typeof profileStore.setMainResume === 'function') {
        await profileStore.setMainResume(resume.id)
      }
    }

    resumeStore.setResumeForEdit(resume)
    emit('update:modelValue', resume)
    alert('Резюме сохранено!')
  } catch (err) {
    alert('Ошибка при сохранении резюме.')
    console.error(err)
  }
}
</script>

<template>
  <div class="bg-[var(--background-section)] bg-opacity-30 backdrop-blur-xl p-8 rounded-3xl border border-[var(--background-pale)]">
    <h2 class="text-4xl font-bold bg-gradient-to-r from-[var(--text-secondary)] to-[var(--text-light)] bg-clip-text text-transparent mb-8">
      Ваше резюме готово!
    </h2>

    <div class="download-options rounded-2xl bg-gradient-to-br from-[var(--neon-purple)]/10 to-[var(--neon-blue)]/10">
      <h3 class="text-2xl font-semibold text-[var(--text-light)] mb-4">Скачать и сохранить:</h3>
      <div class="flex space-x-4 mb-8">
        <button @click="generatePDF" class="btn btn-primary flex-1">
          <i class="fas fa-file-pdf mr-2"></i>
          
          <span>Скачать PDF</span>
        </button>
        <button @click="saveResume" class="btn btn-primary flex-1">
          <i class="fas fa-save mr-2"></i>
          <span>Сохранить резюме</span>
        </button>
      </div>

      <h3 class="text-2xl font-semibold text-[var(--text-light)] mb-4">Отправить работодателю</h3>
      <div class="relative">
        <input
          :value="employerEmail || ''"
          @input="handleEmailChange"
          type="email"
          placeholder="employer@company.com"
          class="w-full bg-[var(--background-job-analysis)] text-[var(--text-light)] rounded-xl py-4 pl-5 pr-14 text-lg border transition-all duration-300"
          :class="[
            'border-[var(--background-pale)] focus:outline-none focus:ring-2 focus:ring-[var(--background-cta)]',
            { 'border-green-500 focus:ring-green-500': isValidEmail && employerEmail, 
              'border-red-500 focus:ring-red-500': !isValidEmail && employerEmail && employerEmail.trim() }
          ]"
          required
        />
        
        <button
          v-if="isValidEmail"
          @click="sendToEmployer"
          class="absolute top-1/2 right-2 -translate-y-1/2 bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] text-white p-3 rounded-lg hover:scale-105 active:scale-95 transition"
        >
          <i class="fas fa-location-arrow rotate-45"></i>
        </button>
      </div>
      
      <!-- Подсказка о валидации -->
      <div v-if="employerEmail && !isValidEmail" class="mt-2 text-red-500 text-sm">
        <i class="fas fa-exclamation-circle mr-1"></i>
        Пожалуйста, введите корректный email
      </div>
      
      <!-- Подсказка о готовности к отправке -->
      <div v-if="isValidEmail" class="mt-2 text-green-500 text-sm">
        <i class="fas fa-check-circle mr-1"></i>
        Email валиден, можно отправлять
      </div>
    </div>

    <div class="flex justify-between mt-6 flex-wrap gap-4">
      <button @click="$emit('prev-step')" class="btn btn-secondary">Назад</button>
      <button @click="$emit('create-new-resume')" class="btn btn-primary">Создать новое резюме</button>
    </div>
  </div>
</template>

<style scoped>
.btn {
  @apply px-6 py-3 font-semibold rounded-full transition-all duration-300 transform hover:scale-105 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-[var(--neon-purple)];
}

.btn-primary {
  @apply bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] text-[var(--text-light)];
}

.btn-secondary {
  @apply bg-[var(--background-section)] bg-opacity-50 text-[var(--text-light)] border border-[var(--background-pale)];
}

</style>