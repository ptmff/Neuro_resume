<template>
  <div class="page">
    <ProgressBar />
    <div class="resume-container">
      <ResumeForm v-if="currentStep === 1"  :resumeData="resumeData" @next-step="nextStep" v-model="resumeData"/>
      <TemplateSelection
        v-if="currentStep === 2"
        :templates="templates"
        :selectedTemplate="selectedTemplate"
        @select-template="selectTemplate"
        @next-step="nextStep"
        @prev-step="prevStep"
      />
      <AiOptimization
        v-if="currentStep === 3" 
        @next-step="nextStep"
        @prev-step="prevStep"
      />
      <DownloadOptions
        v-if="currentStep === 4" 
        @prev-step="prevStep"
        :employerEmail="employerEmail"
      @update:employerEmail="updateEmployerEmail"
      @send-to-employer="handleSendToEmployer"
      @download-resume="handleDownloadResume"
      @create-new-resume="createNewResume"
       />
      <ResumePreview :resumeData="resumeData" />
    </div>
  </div>
</template>

<script>
import ResumeForm from '../components/ResumeForm.vue';
import ProgressBar from '../components/ProgressBar.vue';
import ResumePreview from '../components/ResumePreview.vue';
import TemplateSelection from '../components/TemplateSelection.vue';
import AiOptimization from '../components/AiOptimization.vue';
import DownloadOptions from '../components/DownloadOptions.vue';


export default {
  components: {
    ResumeForm,
    ProgressBar,
    ResumePreview,
    TemplateSelection,
    AiOptimization,
    DownloadOptions
  },
  data() {
    return {
      employerEmail: '',
      currentStep: 1,
      selectedTemplate: null,
      templates: [
        { 
          name: 'Классический', 
          image: '/placeholder.svg?height=200&width=150'
        },
        { 
          name: 'Современный', 
          image: '/placeholder.svg?height=200&width=150'
        },
        { 
          name: 'Креативный', 
          image: '/placeholder.svg?height=200&width=150'
        },
        { 
          name: 'Минималистичный', 
          image: '/placeholder.svg?height=200&width=150'
        },
        { 
          name: 'Профессиональный', 
          image: '/placeholder.svg?height=200&width=150'
        },
        { 
          name: 'Технический', 
          image: '/placeholder.svg?height=200&width=150'
        }
      ],
      resumeData: {
        name: '',
        email: '',
        phone: '',
        location: '',
        profession: '',
        education: '',
        experience: '',
        skills: ''
      }
    };
  },
  methods: {
    selectTemplate(index) {
      this.selectedTemplate = index;
    },
    scrollToTop(){
      window.scrollTo({
        top: 0,
        behavior: 'smooth'
      })
    },

  
  nextStep() {
      if (this.currentStep < 4) {
        this.currentStep++;
        this.scrollToTop();
      }
    },
    prevStep() {
      if (this.currentStep > 1) {
        this.currentStep--;
        this.scrollToTop();
      }
    },

  
  handleSendToEmployer(email) {
    if (!isValidEmail(email)){
      alert('Пожалуйста, введите корректный email.', email);
      return;
    }
    if (!email) {
    alert('Пожалуйста, введите email работодателя.');
    return;
    alert('Email успешно проверен: ' + email);
  }
      // Логика отправки email работодателю
      
      // Здесь вы можете добавить код для отправки email
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
        skills: ''
      };
      this.currentStep = 1;
    },
    updateEmployerEmail(email) {
      this.employerEmail = email; // Обновление email работодателя
    },
  }  
  
};
</script>

<style scoped>
.resume-container {
  display: flex; /* Используем Flexbox для размещения компонентов по горизонтали */
  justify-content: space-between; /* Распределяем пространство между компонентами */
  align-items: stretch; /* Выравниваем компоненты по верхнему краю */
}

.resume-container > * {
  flex: 1; /* Каждый компонент занимает равное пространство */
  margin: 0 10px; /* Добавляем отступы между компонентами */
}
</style>