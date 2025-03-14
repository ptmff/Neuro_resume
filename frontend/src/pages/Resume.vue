<template>
  <div
    class="min-h-screen bg-gradient-to-br from-[var(--background-start)] to-[var(--background-end)]"
  >
    <div class="container mx-auto px-4 py-20">
      <!-- Увеличен паддинг по вертикали -->
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
          <ResumePreview :resumeData="resumeData" :selectedTemplate="selectedTemplate" />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
//frontend\src\components\ResumeComps\DownloadOptions.vue
import ResumeForm from '../components/ResumeComps/ResumeForm.vue'
import ProgressBar from '../components/ResumeComps/ProgressBar.vue'
import ResumePreview from '../components/ResumeComps/ResumePreview.vue'
import TemplateSelection from '../components/ResumeComps/TemplateSelection.vue'
import AiOptimization from '../components/ResumeComps/AiOptimization.vue'
import DownloadOptions from '../components/ResumeComps/DownloadOptions.vue'

export default {
  components: {
    ResumeForm,
    ProgressBar,
    ResumePreview,
    TemplateSelection,
    AiOptimization,
    DownloadOptions,
  },
  data() {
    return {
      employerEmail: '',
      currentStep: 1,
      selectedTemplate: null,
      templates: [
        {
          name: 'Классический',
          image: '/placeholder.svg?height=200&width=150',
        },
        {
          name: 'Современный',
          image: '/placeholder.svg?height=200&width=150',
        },
        {
          name: 'Креативный',
          image: '/placeholder.svg?height=200&width=150',
        },
        {
          name: 'Минималистичный',
          image: '/placeholder.svg?height=200&width=150',
        },
        {
          name: 'Профессиональный',
          image: '/placeholder.svg?height=200&width=150',
        },
        {
          name: 'Технический',
          image: '/placeholder.svg?height=200&width=150',
        },
      ],
      resumeData: {
        name: '',
        email: '',
        phone: '',
        location: '',
        profession: '',
        education: '',
        experience: '',
        skills: '',
      },
    }
  },
  computed: {
    currentStepComponent() {
      switch (this.currentStep) {
        case 1:
          return ResumeForm
        case 2:
          return TemplateSelection
        case 3:
          return AiOptimization
        case 4:
          return DownloadOptions
        default:
          return null
      }
    },
  },
  methods: {
    selectTemplate(index) {
      this.selectedTemplate = index
    },
    scrollToTop() {
      window.scrollTo({
        top: 0,
        behavior: 'smooth',
      })
    },
    isValidEmail(email) {
      const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
      return re.test(email)
    },
    isValidPhone(phone) {
      const re = /^\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}$/
      return re.test(phone)
    },
    nextStep() {
      if (this.currentStep < 4) {
        if (this.currentStep === 1) {
          const isFormValid =
            this.resumeData.name &&
            this.isValidEmail(this.resumeData.email) &&
            this.isValidPhone(this.resumeData.phone) &&
            this.resumeData.location &&
            this.resumeData.profession &&
            this.resumeData.education &&
            this.resumeData.experience &&
            this.resumeData.skills
          if (!isFormValid) {
            alert('Некоторые поля заполнены неверно')
            return
          }
        }
        if (this.currentStep === 2) {
          if (this.selectedTemplate == null) {
            alert('Не выбран шаблон')
            return
          }
        }
        this.currentStep++
        this.scrollToTop()
      }
    },
    prevStep() {
      if (this.currentStep > 1) {
        this.currentStep--
        this.scrollToTop()
      }
    },
    goToStep(step) {
      if (step > this.currentStep) {
        if (this.currentStep === 1) {
          const isFormValid =
            this.resumeData.name &&
            this.isValidEmail(this.resumeData.email) &&
            this.isValidPhone(this.resumeData.phone) &&
            this.resumeData.location &&
            this.resumeData.profession &&
            this.resumeData.education &&
            this.resumeData.experience &&
            this.resumeData.skills
          if (!isFormValid) {
            alert('Некоторые поля заполнены неверно')
            return
          }
        }
        if (this.currentStep === 2) {
          if (this.selectedTemplate == null) {
            alert('Не выбран шаблон')
            return
          }
        }
      }
      this.currentStep = step
    },
    handleSendToEmployer(email) {
      if (!this.isValidEmail(email)) {
        alert('Пожалуйста, введите корректный email.')
        return
      }
      if (!email) {
        alert('Пожалуйста, введите email работодателя.')
        return
      }
      alert('Email успешно проверен: ' + email)
    },
    createNewResume() {
      this.resumeData = {
        name: '',
        email: '',
        phone: '',
        location: '',
        profession: '',
        education: '',
        experience: '',
        skills: '',
      }
      this.currentStep = 1
    },
    updateEmployerEmail(email) {
      this.employerEmail = email
    },
  },
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
  padding-top: 95px; /* Увеличьте верхний отступ, чтобы избежать перекрытия */
}
</style>
