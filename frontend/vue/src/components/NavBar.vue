<template>
  <nav class="navbar navbar-expand-lg navbar-light bg-white shadow-sm fixed-top">
    <div class="container">
      <!-- Логотип -->
      <router-link class="navbar-brand fade-in-left" to="/">
        <span style="color: #8B5E3C;">Neuro</span><span style="color: #5A3E2B;">.Resume</span>
      </router-link>

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

</style>