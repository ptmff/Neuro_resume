<template>
  <nav class="navbar navbar-expand-lg navbar-light bg-white shadow-sm fixed-top">
    <div class="container">
      <!-- Логотип -->
      <router-link class="navbar-brand fade-in-left" to="/">
        <span style="color: #8B5E3C;">Neuro</span><span style="color: #5A3E2B;">.Resume</span>
      </router-link>

      <!-- Бургер-меню для мобильных устройств -->
      <button class="navbar-toggler border-0 fade-in-right" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
        <div class="burger-menu">
          <span class="line1"></span>
          <span class="line2"></span>
          <span class="line3"></span>
        </div>
      </button>

      <!-- Основная навигация -->
      <div class="collapse navbar-collapse justify-content-end" id="navbarNav">
        <ul class="navbar-nav fade-in-right">
          <li class="nav-item">
            <router-link class="nav-link" to="/">Главная</router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/resume-builder">Создать резюме</router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/analyse">Анализ вакансий</router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/comb-check">Рассчёт совместимости</router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/profile">Профиль</router-link>
          </li>
        </ul>
      </div>
    </div>
  </nav>
</template>

<style>
/* 🔹 Стили лого */
.navbar-brand {
  font-size: 1.5rem;
  font-weight: bold;
}

/* 🔹 Стили для навигации */
.navbar-nav .nav-link {
  color: #5A3E2B;
  position: relative;
  text-decoration: none;
}

/* 🔹 Подчеркивание при наведении */
.navbar-nav .nav-link::after {
  content: '';
  display: block;
  width: 0;
  height: 2px;
  background: #8B5E3C;
  transition: width 0.3s ease-in-out;
}

.navbar-nav .nav-link:hover::after,
.navbar-nav .router-link-active::after {
  width: 100%;
}

/* 🔹 Бургер-меню */
.burger-menu {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  width: 30px;
  height: 20px;
  cursor: pointer;
  align-items: flex-end;
}

.burger-menu span {
  display: block;
  height: 3px;
  background: #5A3E2B;
  transition: width 0.3s ease-in-out;
}

.burger-menu .line1 {
  width: 100%;
}

.burger-menu .line2 {
  width: 50%;
}

.burger-menu .line3 {
  width: 75%;
}

/* 🔹 Анимация изменения ширины линий при наведении */
.burger-menu:hover .line1 {
  width: 75%;
}

.burger-menu:hover .line2 {
  width: 100%;
}

.burger-menu:hover .line3 {
  width: 50%;
}

/* 🔹 -бордер у кнопки бургера */
.navbar-toggler {
  border: none !important;
  box-shadow: none !important;
}

/* 🔹 Анимации */
.fade-in-left {
  opacity: 0;
  transform: translateX(-50px);
  animation: fadeInLeft 1s ease-out forwards;
}

.fade-in-right {
  opacity: 0;
  transform: translateX(50px);
  animation: fadeInRight 1s ease-out forwards;
}

/* 🔹 Логика анимаций */
@keyframes fadeInLeft {
  from {
    opacity: 0;
    transform: translateX(-50px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

@keyframes fadeInRight {
  from {
    opacity: 0;
    transform: translateX(50px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}
</style>