<template>
  <div class="min-h-screen bg-gradient-to-br from-[var(--background-start)] to-[var(--background-end)]">
    <div class="container mx-auto px-4 py-20">
      <ProgressBar :current-step="currentStep" @step-click="goToStep" />

      <!-- Для шага 4 (FinalPreview) используем полную ширину и центрирование -->
      <div v-if="currentStep === 4" class="mt-8 flex justify-center">
        <FinalPreview
          :resume-data="resumeData"
          @next-step="nextStep"
          @prev-step="prevStep"
          @update:sections-order="updateSectionsOrder"
        />
      </div>
      
      <!-- Для остальных шагов используем двухколоночную сетку -->
      <div v-else class="mt-8 grid grid-cols-1 lg:grid-cols-2 gap-8">
        <div class="space-y-8">
          <transition name="fade" mode="out-in">
            <component
              :is="currentStepComponent"
              :key="currentStep"
              :resume-data="resumeData"
              :templates="templates"
              :selected-template="selectedTemplate"
              :ai-suggestions="aiSuggestions"
              :employer-email="employerEmail"
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
          <ResumePreview :resume-data="resumeData" :selected-template="selectedTemplate" />
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { ref, computed, watch } from 'vue'
import { useResumeStore } from '@/stores/resumesStore'

import ResumeForm from '../components/ResumeComps/ResumeForm.vue'
import ProgressBar from '../components/ResumeComps/ProgressBar.vue'
import ResumePreview from '../components/ResumeComps/ResumePreview.vue'
import TemplateSelection from '../components/ResumeComps/TemplateSelection.vue'
import AiOptimization from '../components/ResumeComps/AiOptimization.vue'
import DownloadOptions from '../components/ResumeComps/DownloadOptions.vue'
import FinalPreview from '@/components/ResumeComps/FinalPreview.vue'

interface Template {
  name: string
  image: string
}

interface AiSuggestion {
  id: string
  type: string
  title: string
  description: string
  before: any
  after: any
  confidence: number
  reasoning: string
}

interface Experience {
  id?: string
  company: string
  position: string
  startDate: string
  endDate: string
  description: string
}

interface StoreEducation {
  institution: string
  degree: string
  field: string
  startYear: number
  endYear: number
}

interface ResumeData {
  id?: number
  title?: string
  name?: string
  email?: string
  phone?: string
  city?: string
  job?: string
  description?: string
  experience?: Experience[]
  education?: StoreEducation[]
  skills?: string[]
  template?: string
  date?: string
  sectionsOrder?: string[]
  [key: string]: any
}

interface Resume extends Omit<ResumeData, 'id'> {
  id: number
  title: string
  city: string
  job: string
  experience: Experience[]
  education: StoreEducation[]
  skills: string[]
  template: string
  date: string
}

const resumeStore = useResumeStore()

const defaultResume: Resume = {
  id: Date.now(),
  title: '',
  city: '',
  job: '',
  experience: [],
  education: [],
  skills: [],
  template: '',
  date: new Date().toISOString()
}

const resumeData = computed<ResumeData>(() => 
  resumeStore.resumeToEdit ? { ...resumeStore.resumeToEdit } : { ...defaultResume }
)

const employerEmail = ref<string>('')
const currentStep = ref<number>(1)
const selectedTemplate = ref<number | null>(null)
const aiSuggestions = ref<AiSuggestion[]>([])

const templates: Template[] = [
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

// Автосинхронизация шаблона с selectedTemplate
watch(
  () => resumeData.value.template,
  (newTemplate: string | undefined) => {
    if (!newTemplate) return
    
    const index = templates.findIndex(t => 
      t.name.toLowerCase() === newTemplate.toLowerCase()
    )
    if (index !== -1) {
      selectedTemplate.value = index
    }
  },
  { immediate: true }
)

const isEditing = computed(() => !!resumeData.value.id)

const selectTemplate = (index: number): void => {
  selectedTemplate.value = index
  const templateName = templates[index].name.toLowerCase()
  updateResumeData({
    ...resumeData.value,
    template: templateName
  })
}

const nextStep = (): void => {
  if (currentStep.value === 1) {
    const r = resumeData.value
    const valid = r.title && r.city && r.job && 
                 Array.isArray(r.experience) && 
                 r.experience.length > 0 && 
                 r.skills?.length
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

const prevStep = (): void => {
  if (currentStep.value > 1) currentStep.value--
  scrollToTop()
}

const goToStep = (step: number): void => {
  currentStep.value = step
  scrollToTop()
}

const scrollToTop = (): void => {
  window.scrollTo({ top: 0, behavior: 'smooth' })
}

const updateSectionsOrder = (newOrder: string[]) => {
  updateResumeData({
    ...resumeData.value,
    sectionsOrder: newOrder
  })
}

const updateResumeData = (updated: ResumeData): void => {
  const completeResume: Resume = {
    ...defaultResume,
    ...updated,
    id: updated.id || defaultResume.id,
    title: updated.title || defaultResume.title,
    city: updated.city || defaultResume.city,
    job: updated.job || defaultResume.job,
    experience: updated.experience || defaultResume.experience,
    education: updated.education || defaultResume.education,
    skills: updated.skills || defaultResume.skills,
    template: updated.template || defaultResume.template,
    date: updated.date || defaultResume.date
  }
  resumeStore.setResumeForEdit(completeResume)
}

const applySuggestion = (suggestion: AiSuggestion): void => {
  const notification = document.createElement('div')
  notification.className = 'fixed bottom-4 right-4 bg-green-500 text-white px-4 py-2 rounded-lg shadow-lg z-50 flex items-center'
  notification.innerHTML = `
    <i class="fas fa-check-circle mr-2"></i>
    <span>Рекомендация применена!</span>
  `
  document.body.appendChild(notification)
  
  setTimeout(() => {
    notification.classList.add('opacity-0', 'transition-opacity', 'duration-500')
    setTimeout(() => {
      document.body.removeChild(notification)
    }, 500)
  }, 3000)
  
  console.log('Применена рекомендация:', suggestion)
}

const createNewResume = (): void => {
  resumeStore.setResumeForEdit({ ...defaultResume, id: Date.now() })
  currentStep.value = 1
}

const updateEmployerEmail = (email: string): void => {
  employerEmail.value = email
}

const handleSendToEmployer = (email: string): void => {
  const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  if (!re.test(email)) {
    alert('Некорректный email')
    return
  }
  alert('Email подтвержден: ' + email)
}

const handleDownloadResume = (): void => {
  console.log('Download resume')
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