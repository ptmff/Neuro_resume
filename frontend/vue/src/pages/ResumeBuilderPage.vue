<template>
  <div class="page">
    <ProgressBar :currentStep="currentStep" @step-click="goToStep"/>
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
    isValidEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
  },

  isValidPhone(phone) {
    const re = /^\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}$/; 
    return re.test(phone);
  },

  
  nextStep() {
    if (this.currentStep < 4) {
      if (this.currentStep === 1) {

      const isFormValid = this.resumeData.name &&
                          this.isValidEmail(this.resumeData.email) &&
                          this.isValidPhone(this.resumeData.phone) &&
                          this.resumeData.location &&
                          this.resumeData.profession &&
                          this.resumeData.education &&
                          this.resumeData.experience &&
                          this.resumeData.skills;
      if (!isFormValid) {
        alert("Некоторые поля заполнены неверно")
        return; 
      }
    }
    if (this.currentStep === 2) {
      if (this.selectedTemplate == null) {
        alert("Не выбран шаблон")
        return;
      }
    }
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

  goToStep(step) {
    if (step > this.currentStep) {
      if (this.currentStep === 1) {
        // Проверяем, заполнены ли все поля в ResumeForm
        const isFormValid = this.resumeData.name &&
                            this.isValidEmail(this.resumeData.email) &&
                            this.isValidPhone(this.resumeData.phone) &&
                            this.resumeData.location &&
                            this.resumeData.profession &&
                            this.resumeData.education &&
                            this.resumeData.experience &&
                            this.resumeData.skills;
        if (!isFormValid) {
          alert("Некоторые поля заполнены неверно")
          return; 
        }
      }
      if (this.currentStep === 2) {
        if (this.selectedTemplate == null) {
          alert("Не выбран шаблон")
          return; 
        }
      }
    }
    this.currentStep = step; 
  },

  handleSendToEmployer(email) {
    if (!isValidEmail(email)){
      alert('Пожалуйста, введите корректный email.', email);
      return;
    }
    if (!email) {
    alert('Пожалуйста, введите email работодателя.');
    return;
    }
    alert('Email успешно проверен: ' + email);

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
      this.employerEmail = email;
    },
  }  
  
};
</script>

<style scoped>
.resume-container {
  display: flex; 
  justify-content: space-between; 
  align-items: stretch; 
}

.resume-container > * {
  flex: 1; 
  margin: 0 10px;
}
</style>