<template>
  <div class="min-h-screen bg-gradient-to-br from-[var(--background-start)] to-[var(--background-end)]">
    <div class="container mx-auto px-4 py-20">
      <ProgressBar :currentStep="currentStep" @step-click="goToStep" />

      <div class="mt-8 grid grid-cols-1 lg:grid-cols-2 gap-8">
        <div class="space-y-8">
          <transition name="fade" mode="out-in">
            <component
              :is="currentStepComponent"
              :key="currentStep"
              :resumeData="resumeData"
              :templates="templates"
              :selectedTemplate="selectedTemplate"
              :aiSuggestions="aiSuggestions"
              :employerEmail="employerEmail"
              @next-step="nextStep"
              @prev-step="prevStep"
              @update:modelValue="updateResumeData"
              @select-template="selectTemplate"
              @apply-suggestion="applySuggestion"
              @update:employerEmail="updateEmployerEmail"
              @send-to-employer="handleSendToEmployer"
              @download-resume="handleDownloadResume"
              @create-new-resume="createNewResume"
            />
          </transition>
        </div>

        <div class="lg:sticky lg:top-8 lg:self-start">
          <ResumePreview :resumeData="resumeData" :selectedTemplate="selectedTemplate" v-if="currentStep!=4"/>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, watch } from 'vue'
import { useResumeStore } from '@/stores/resumesStore'

import ResumeForm from '../components/ResumeComps/ResumeForm.vue'
import ProgressBar from '../components/ResumeComps/ProgressBar.vue'
import ResumePreview from '../components/ResumeComps/ResumePreview.vue'
import TemplateSelection from '../components/ResumeComps/TemplateSelection.vue'
import AiOptimization from '../components/ResumeComps/AiOptimization.vue'
import DownloadOptions from '../components/ResumeComps/DownloadOptions.vue'
import FinalPreview from '@/components/ResumeComps/FinalPreview.vue'

const resumeStore = useResumeStore()

const resumeData = computed(() =>
  resumeStore.resumeToEdit || {
    title: '',
    city: '',
    job: '',
    experience: [],
    skills: [],
    template: ''
  }
)

const employerEmail = ref('')
const currentStep = ref(1)
const selectedTemplate = ref(null)

const templates = [
  { name: 'Классический', image: '/placeholder.svg?height=200&width=150' },
  { name: 'Современный', image: '/placeholder.svg?height=200&width=150' },
  { name: 'Креативный', image: '/placeholder.svg?height=200&width=150' },
  { name: 'Минималистичный', image: '/placeholder.svg?height=200&width=150' },
  { name: 'Профессиональный', image: '/placeholder.svg?height=200&width=150' },
  { name: 'Технический', image: '/placeholder.svg?height=200&width=150' }
]

const currentStepComponent = computed(() => {
  switch (currentStep.value) {
    case 1: return ResumeForm
    case 2: return TemplateSelection
    case 3: return AiOptimization
    case 4: return FinalPreview
    case 5: return DownloadOptions
    default: return null
  }
})

// 💡 Автосинхронизация шаблона с selectedTemplate
watch(
  () => resumeData.value.template,
  (newTemplate) => {
    const index = templates.findIndex(t => t.name.toLowerCase() === newTemplate?.toLowerCase())
    if (index !== -1) {
      selectedTemplate.value = index
    }
  },
  { immediate: true }
)

const isEditing = computed(() => !!resumeData.value.id)

const selectTemplate = (index) => {
  selectedTemplate.value = index
  resumeData.value.template = templates[index].name.toLowerCase()
  resumeStore.setResumeForEdit({ ...resumeData.value })
}

const nextStep = () => {
  if (currentStep.value === 1) {
    const r = resumeData.value
    const valid = r.title && r.city && r.job && Array.isArray(r.experience) && r.experience.length && r.skills?.length
    if (!valid) {
      alert('Пожалуйста, заполните все поля')
      return
    }
  }
  if (currentStep.value === 2 && selectedTemplate.value === null) {
    alert('Выберите шаблон резюме')
    return
  }
  currentStep.value++
  scrollToTop()
}

const prevStep = () => {
  if (currentStep.value > 1) currentStep.value--
  scrollToTop()
}

const goToStep = (step) => {
  currentStep.value = step
  scrollToTop()
}

const scrollToTop = () => {
  window.scrollTo({ top: 0, behavior: 'smooth' })
}

const updateResumeData = (updated) => {
  resumeStore.setResumeForEdit(updated)
}

const createNewResume = () => {
  resumeStore.setResumeForEdit({
    title: '',
    city: '',
    job: '',
    experience: [],
    skills: [],
    template: ''
  })
  currentStep.value = 1
}

const updateEmployerEmail = (email) => {
  employerEmail.value = email
}

const handleSendToEmployer = (email) => {
  const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  if (!re.test(email)) {
    alert('Некорректный email')
    return
  }
  alert('Email подтвержден: ' + email)
}
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
.container {
  padding-top: 95px;
}
</style>
