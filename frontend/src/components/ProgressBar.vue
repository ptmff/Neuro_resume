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
    methods: {
      goToStep(step) {
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
    background-color: rgba(255, 255, 255, 0.1);    
    display: flex;
    justify-content: center;
    align-items: center;
    margin-bottom: 8px;
    color: #ffffff;
    font-size: 1.2rem;
    position: relative; 
    transition: all 0.3s;
    box-shadow: 0 0 10px rgba(0, 255, 255, 0.5);
}

.step-icon:hover {
  box-shadow: 0 0 20px rgba(0, 255, 255, 1); /* Увеличение неонового эффекта при наведении */
}


.step.active .step-icon {
    background: linear-gradient(90deg, var(--neon-purple), var(--neon-blue));
    color: white;
    transition: all 1.2s;
    box-shadow: 0 0 10px rgba(0, 255, 255, 0.5);}
  
  .step-text {
    font-size: 0.9rem;
    font-weight: 500;
  }
  
  .step-line {
    flex-grow: 1;
    height: 3px;
    background-color: rgba(255, 255, 255, 0.1);
    margin: 0 10px;
    position: relative;
    top: -15px;
    z-index: 0;
    transition: all 1.2s;
  }
  
  .step-line.active {
    background: linear-gradient(90deg, var(--neon-purple), var(--neon-blue));    
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