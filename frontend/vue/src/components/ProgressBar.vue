<template>
    <div class="progress-container p-5">
      <div class="progress-steps">
        <div class="step">
          <div class="step-icon">
            <i class="bi bi-person-fill"></i>
          </div>
          <div class="step-text">Данные</div>
        </div>
        <div class="step-line"></div>
        <div class="step">
          <div class="step-icon">
            <i class="bi bi-palette-fill"></i>
          </div>
          <div class="step-text">Стиль</div>
        </div>
        <div class="step-line"></div>
        <div class="step">
          <div class="step-icon">
            <i class="bi bi-pencil-fill"></i>
          </div>
          <div class="step-text">AI</div>
        </div>
        <div class="step-line"></div>
        <div class="step">
          <div class="step-icon">
            <i class="bi bi-download"></i>
          </div>
          <div class="step-text">Скачать</div>
        </div>
      </div>
    </div>
  </template>
  
  
  
  
    
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

    </style>