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

/* Прогресс-бар */
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
    position: relative; /* Добавлено для позиционирования псевдоэлемента */
    transition: all 1.2s;
}



/* Псевдоэлемент для меньшего кружочка */
.step-icon::after {
    content: '';
    width: 30px; /* Размер меньшего кружочка */
    height: 30px; /* Размер меньшего кружочка */
    border-radius: 50%;
    background-color: rgba(139, 94, 60, 0.8); /* Темный цвет */
    position: absolute; /* Позиционирование относительно родителя */
    transition: opacity 0.3s; /* Плавный переход */
    opacity: 0; /* Скрыт по умолчанию */
}

.step-icon:hover::after {
    opacity: 1; /* Показать при наведении */
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

</style>