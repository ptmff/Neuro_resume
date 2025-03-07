<template>
    <div class="container p-5">
      <h2 class="title mb-5">Ваше резюме готово!</h2>
      <div class="download-options p-3 mb-4">
        <div class="subtitle ">
          <h4 class="mb-3">Выберите формат для скачивания:</h4>
          <div class="download-buttons">
          <button class="btn download-btn" @click="$emit('download-resume', 'pdf')">
            <i class="fas fa-file-pdf"></i>
            <span>Скачать PDF</span>
          </button>
          <button class="btn download-btn" @click="$emit('download-resume', 'docx')">
            <i class="fas fa-file-word"></i>
            <span>Скачать DOCX</span>
          </button>
  
        </div>
        
        </div>
        
        <div class="send-to-employer mt-5">
          <h3 class="subtitle mb-3">Отправить работодателю</h3>
          <div class="email-input-group">
            <FormField class="form-field"
              id="employerEmail"
              label="Email работодателя"
              type="email"
              :modelValue="employerEmail"
              @update:modelValue="$emit('update:employerEmail', $event)"
              placeholder="employer@company.com"
            />
            <button class="btn send-btn" @click="sendToEmployer">
              <i class="fas fa-paper-plane"></i>
              <span>Отправить</span>
            </button>
          </div>
        </div>
      </div>
      <div class="d-flex justify-content-between mt-4">
          <button type="button" class="btn btn-outline-secondary" @click="$emit('prev-step')">Назад</button>
          <button type="button" class="btn" @click="$emit('create-new-resume')">Содать новое резюме</button>
        </div>
    </div>
  </template>
  
  <script>
  import FormField from './FormField.vue';
  
  export default {
    components: {
      FormField
    },
    props: {
      employerEmail: {
        type: String,
        required: true
      }
    },
    emits: [
      'download-resume',  
      'update:employerEmail', 
      'send-to-employer', 
      'create-new-resume', 
      'prev-step'
    ]
  };
  </script>
  
  <style scoped>
  .container {
    border-radius: 10px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    color: #3b2f2f;
  }
  
  .title{
    font-size: 2rem;
    font-weight: bold;
  }
  
  .subtitle {
    font-size: 1.5rem;
    color: #5a4f4f;
    margin-bottom: 1rem;
  }
  
  .download-options {
    background-color: #f9f5f0;
    border-radius: 8px;
    padding: 2rem;
  }
  
  .download-buttons {
    display: flex;
    justify-content: space-between;
    gap: 1rem;
    margin-bottom: 2rem;
  }
  
  .btn {
      color: white;
      background-color: #8b5e3c;
    }
    
  .btn:hover {
    background-color: #6d4c2f;
  }
  
  .btn-outline-secondary, .send-btn {
    color: #8b5e3c;
    border-color: #8b5e3c;
    background-color: transparent;
  }
  
  .btn-outline-secondary:hover, .send-btn:hover {
    color: white;
    background-color: #8b5e3c;
  }
  
  .download-btn {
    background-color: #8b5e3c;
    color: #ffffff;
    flex: 1;
  }
  
  .send-btn{
    flex: 1;
    max-height: 2.4rem;
    max-width: 120px;
    margin-top: 2rem;
    margin-right: 6rem;
  
  }

  </style>
  
  