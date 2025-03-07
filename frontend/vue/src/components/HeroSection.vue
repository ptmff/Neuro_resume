<template>
    <section class="hero-section">
      <div class="hero-content">
        <!-- Изображение куба -->
        <img class="hero-cube fade-in-right" src="../assets/cube1.png" alt="3D Cube">
        
        <!-- Основной текстовый блок -->
        <div class="hero-text fade-in-left">
          <h1 class="hero-title">Построй своё резюме с искусством и точностью</h1>
          <p class="hero-subtitle">Не просто создавай резюме, а делай его запоминающимся с AI-анализом</p>
          <div class="hero-buttons">
            <router-link to="/resume-builder" class="btn hero-btn rounded-4">Начать</router-link>
          </div>
        </div>
      </div>
    </section>
  </template>
  
  <style>
  /* 🔹 Основная секция */
  .hero-section {
    position: relative;
    background-color: #ece7e2;
    padding: 100px 0;
    width: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
    overflow: hidden;
    min-height: 600px;
  }
  
  /* 🔹 Контейнер для контента */
  .hero-content {
    display: flex;
    align-items: center;
    justify-content: space-between;
    max-width: 1400px;
    width: 100%;
    padding: 0 50px;
    position: relative;
    z-index: 2;
  }
  
  /* 🔹 Изображение куба */
  .hero-cube {
    position: absolute;
    right: -10%;
    top: 50%;
    transform: translateY(-50%) rotate(30deg);
    width: 70vw;
    max-width: 900px;
    z-index: 1000;
    filter: grayscale(80%) sepia(50%) saturate(60%) hue-rotate(10deg) brightness(110%);
  }
  
  /* 🔹 Текстовый блок */
  .hero-text {
    max-width: 600px;
    z-index: 2;
  }
  
  .hero-title {
    font-size: 3rem;
    color: #3b2f2f;
    font-weight: bold;
  }
  
  .hero-subtitle {
    font-size: 1.2rem;
    color: #6d4c2f;
    margin-bottom: 30px;
  }
  
  /* 🔹 Кнопки */
  .hero-buttons {
    display: flex;
    gap: 20px;
  }
  
  .hero-btn {
    color: #3b2f2f !important;
    border: 2px solid #3b2f2f !important;
    padding: 10px 18px !important;
    font-size: 1rem !important;
    text-decoration: none;
    transition: 0.3s;
  }
  
  .hero-btn:hover {
    background-color: #3b2f2f !important;
    color: #ece7e2 !important;
  }
  
  /* 🔹 Адаптивность */
  @media (max-width: 768px) {
    .hero-content {
      flex-direction: column;
      text-align: center;
    }
  
    /* Выравнивание кнопок по центру */
    .hero-buttons {
      display: flex;
      flex-direction: column;
      align-items: center;
      text-align: center;
    }
  
    .hero-btn {
      width: 100%;
      max-width: 250px;
    }
  }
  
  /* 🔹 Блюр куба при наложении текста на куб */
  @media (max-width: 945px) {
    .hero-cube {
      filter: grayscale(85%) sepia(40%) brightness(115%) contrast(85%) blur(5px);
    }
  }
  </style>
  