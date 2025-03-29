<template>
  <div class="bg-[var(--background-section)] bg-opacity-30 backdrop-blur-xl p-8 rounded-3xl border border-[var(--background-pale)]">
    <h2 class="text-4xl font-bold bg-gradient-to-r from-[var(--text-secondary)] to-[var(--text-light)] bg-clip-text text-transparent mb-8">
      Ваше резюме готово!
    </h2>

    <div class="download-options p-6 rounded-2xl bg-gradient-to-br from-[var(--neon-purple)]/10 to-[var(--neon-blue)]/10">
      <h3 class="text-2xl font-semibold text-[var(--text-light)] mb-4">Выберите формат для скачивания:</h3>
      <div class="flex space-x-4 mb-8">
        <button @click="generatePDF" class="btn btn-primary flex-1">
          <i class="fas fa-file-pdf mr-2"></i>
          <span>Скачать PDF</span>
        </button>
        <button @click="$emit('download-resume', 'docx')" class="btn btn-primary flex-1">
          <i class="fas fa-file-word mr-2"></i>
          <span>Скачать DOCX</span>
        </button>
      </div>

      <h3 class="text-2xl font-semibold text-[var(--text-light)] mb-4">Отправить работодателю</h3>
      <div class="flex items-center space-x-4">
        <FormField
          class="flex-grow"
          id="employerEmail"
          label="Email работодателя"
          type="email"
          :modelValue="employerEmail || ''"
          @update:modelValue="$emit('update:employerEmail', $event)"
          placeholder="employer@company.com"
        />
        <button @click="sendToEmployer" class="btn btn-primary h-[50px]">
          <i class="fas fa-paper-plane mr-2"></i>
          <span>Отправить</span>
        </button>
      </div>
    </div>

    <div class="flex justify-between mt-6 flex-wrap gap-4">
      <button @click="$emit('prev-step')" class="btn btn-secondary">Назад</button>
      <button @click="saveResume" class="btn btn-primary">💾 Сохранить резюме</button>
      <button @click="$emit('create-new-resume')" class="btn btn-primary">Создать новое резюме</button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useResumeStore } from '@/stores/resumesStore'
import { useProfileStore } from '@/stores/profileStore'
import FormField from './FormField.vue'
import { computed } from 'vue'
import html2pdf from 'html2pdf.js'

//const resumeContent = ref(null)
const generatePDF = () => {
  alert('Кнопка нажата!')
  const element = document.getElementById('resume-content')
  html2pdf().from(element).save('myDocument.pdf')
}

// Define the Resume interface based on your actual store structure
interface Resume {
  id: number;
  title: string;
  date: string;
  city: string;
  job: string;
  name?: string;
  email?: string;
  phone?: string;
  profession?: string;
  location?: string;
  experience: {
    company: string;
    position: string;
    startDate: string;
    endDate: string;
    description: string;
  }[];
  education: {
    institution: string;
    degree: string;
    field: string;
    startYear: number;
    endYear: number;
  }[];
  skills: string[];
  template: string;
}

// Define the Profile interface based on your actual store structure
interface Profile {
  email: string;
  name?: string;
  phone?: string;
  city?: string;
  photo?: string;
  education?: {
    institution: string;
    degree: string;
    field: string;
    startYear: number;
    endYear: number;
  }[];
  mainResumeId?: number | null;
  // Note: removed resumes property as it doesn't exist in your actual Profile type
}

interface DownloadOptionsProps {
  employerEmail?: string;
  resumeData?: Partial<Resume>;
}

const props = defineProps<DownloadOptionsProps>()

const emit = defineEmits<{
  (e: 'download-resume', format: 'pdf' | 'docx'): void;
  (e: 'update:employerEmail', email: string): void;
  (e: 'send-to-employer', email: string): void;
  (e: 'create-new-resume'): void;
  (e: 'prev-step'): void;
  (e: 'update:modelValue', resume: Partial<Resume>): void;
}>()

const resumeStore = useResumeStore()
const profileStore = useProfileStore()
const profile = computed(() => profileStore.profile)

const sendToEmployer = (): void => {
  if (!props.employerEmail) {
    alert('Пожалуйста, введите email работодателя.')
    return
  }

  const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  if (!emailPattern.test(props.employerEmail)) {
    alert('Пожалуйста, введите корректный email.')
    return
  }

  alert('Email успешно проверен: ' + props.employerEmail)
}

const saveResume = async (): Promise<void> => {
  if (!props.resumeData) {
    alert('Нет данных для сохранения')
    return
  }

  // Create a complete Resume object with default values for required fields
  const resume = {
    id: props.resumeData.id || Date.now(),
    title: props.resumeData.title || 'Новое резюме',
    date: props.resumeData.date || new Date().toISOString().slice(0, 10),
    city: props.resumeData.city || '',
    job: props.resumeData.job || '',
    experience: props.resumeData.experience || [],
    education: props.resumeData.education || [],
    skills: props.resumeData.skills || [],
    template: props.resumeData.template || 'default',
    ...props.resumeData
  } as Resume;

  // Check if resume already exists
  const exists = resumeStore.resumes.find(r => r.id === resume.id);

  if (exists) {
    await resumeStore.updateResume(resume);
  } else {
    await resumeStore.addResume(resume);

    // Set this resume as the main resume in the profile if profile exists
    if (profile.value && typeof profileStore.setMainResume === 'function') {
      await profileStore.setMainResume(resume.id);
    }
  }

  resumeStore.setResumeForEdit(resume);
  emit('update:modelValue', resume);

  alert('Резюме сохранено!');
}
</script>

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