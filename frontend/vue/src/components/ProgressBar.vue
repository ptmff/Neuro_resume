<template>
  <div class="progress-container p-5">
    <div class="progress-steps">
      <div class="step" :class="{ 'active': currentStep >= 1 }" @click="goToStep(1)">
        <div class="step-icon">
          <i class="bi bi-person-fill"></i>
        </div>
        <div class="step-text">Данные</div>
      </div>
      <div class="step-line" :class="{ 'active': currentStep >= 2 }"></div>
      <div class="step" :class="{ 'active': currentStep >= 2 }" @click="goToStep(2)">
        <div class="step-icon">
          <i class="bi bi-palette-fill"></i>
        </div>
        <div class="step-text">Стиль</div>
      </div>
      <div class="step-line" :class="{ 'active': currentStep >= 3 }"></div>
      <div class="step" :class="{ 'active': currentStep >= 3 }" @click="goToStep(3)">
        <div class="step-icon">
          <i class="bi bi-pencil-fill"></i>
        </div>
        <div class="step-text">AI</div>
      </div>
      <div class="step-line" :class="{ 'active': currentStep >= 4 }"></div>
      <div class="step" :class="{ 'active': currentStep >= 4 }" @click="goToStep(4)">
        <div class="step-icon">
          <i class="bi bi-download"></i>
        </div>
        <div class="step-text">Скачать</div>
      </div>
    </div>
  </div>
</template>
  
  
<script>
export default {
  props: {
    currentStep: {
      type: Number,
      required: true
    },

  },

  methods:{
    goToStep(step){
      
      this.$emit('step-click', step);
    }
  }
};
</script>
  
    
<style scoped>


.progress-steps {
    display: flex;
    justify-content: space-between;
    align-items: center;
    width: 100%;
  }
  
  .step {
    display: flex;
    flex-direction: column;
    align-items: center;
    position: relative;
    z-index: 1;
    cursor: pointer;
    
  }
  
  .step-icon {
    width: 50px;
    height: 50px;
    border-radius: 50%;
    background-color: #e0d5c8;
    display: flex;
    justify-content: center;
    align-items: center;
    margin-bottom: 8px;
    color: #8b5e3c;
    font-size: 1.2rem;
    position: relative; 
    transition: all 1.2s;
}




.step-icon::after {
    content: '';
    width: 30px; 
    height: 30px; 
    border-radius: 50%;
    background-color: rgba(139, 94, 60, 0.8); 
    position: absolute; 
    transition: opacity 0.3s; 
    opacity: 0; 
}

.step-icon:hover::after {
    opacity: 1;
}

.step.active .step-icon {
    background-color: #8b5e3c;
    color: white;
    transition: all 1.2s;
}
  
  .step-text {
    font-size: 0.9rem;
    font-weight: 500;
  }
  
  .step-line {
    flex-grow: 1;
    height: 3px;
    background-color: #e0d5c8;
    margin: 0 10px;
    position: relative;
    top: -15px;
    z-index: 0;
    transition: all 1.2s;
  }
  
  .step-line.active {
    background-color: #8b5e3c;
    transition: all 1.2s;
  }

  @media (max-width: 768px) {
    .progress-steps {
      flex-direction: column;
      align-items: flex-start;
    }
    
    .step {
      flex-direction: row;
      width: 100%;
    }
    
    .step-icon {
      margin-right: 15px;
      margin-bottom: 0;
    }
    
    .step-line {
      width: 3px;
      height: 20px;
      margin: 15px 0;
      margin-left: 25px;
      top: 0;
    }
  }
</style>