<template>
    <div class="preview-container mt-5">
      <h2 class="mb-5">Предпросмотр</h2>
      <div class="resume-preview p-3">
        <div class="resume-header p-2">
          <h3>{{ resumeData.name || 'Имя Фамилия' }}</h3>
          <p class="profession">{{ resumeData.profession || 'Профессия' }}</p>
        </div>
        <div class="resume-contact mb-3">
          <p v-if="resumeData.email"><i class="bi bi-envelope me-2"></i>{{ resumeData.email }}</p>
          <p v-if="resumeData.phone"><i class="bi bi-telephone me-2"></i>{{ resumeData.phone }}</p>
          <p v-if="resumeData.location"><i class="bi bi-geo-alt me-2"></i>{{ resumeData.location }}</p>
        </div>
        <div class="resume-section mb-3" v-if="resumeData.education">
          <h5>Образование</h5>
          <p>{{ resumeData.education }}</p>
        </div>
        <div class="resume-section mb-3" v-if="resumeData.experience">
          <h5>Опыт работы</h5>
          <p>{{ resumeData.experience }}</p>
        </div>
        <div class="resume-section" v-if="resumeData.skills">
          <h5>Навыки</h5>
          <p>{{ resumeData.skills }}</p>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    props: {
      resumeData: {
        type: Object,
        required: true
      }
    }
  };
  </script>
  
<style scoped>
  .preview-container{
    color:#3b2f2f
  }
  h2{
    font-size: 2rem;
    font-weight: bold;
    padding-left: 1rem;
  }

  .preview-container {
    background-color: #fff;
    border-radius: 8px;
    padding-right: 2rem;
  }
  
  .resume-preview {
    background-color: #f9f5f0;
    border-radius: 6px;
    min-height: 640px;
  }
  

  </style>
  
  