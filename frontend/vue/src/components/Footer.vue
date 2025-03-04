<template>
  <footer class="footer py-5 fade-up">
    <div class="container fixed-bottom">
      <div class="row align-items-center justify-content-center">
        <!-- Левая часть: Логотип и текст -->
        <div class="col-lg-4 col-md-12 text-center text-lg-start">
          <router-link class="footer-brand" to="/">
              <span style="color: #8B5E3C;">Neuro</span><span style="color: #5A3E2B;">.Resume</span>
          </router-link>
          <p class="footer-description">
            AI-помощник в создании профессионального резюме за несколько минут.
          </p>
          <div class="social-icons">
            <a href="#" class="social-icon"><i class="fab fa-telegram"></i></a>
            <a href="#" class="social-icon"><i class="fab fa-vk"></i></a>
            <a href="#" class="social-icon"><i class="fab fa-github"></i></a>
          </div>
        </div>

        <!-- Средняя колонка: Наши ссылки -->
        <div class="col-lg-4 col-md-6 text-center">
          <h6 class="footer-title">Наши ссылки</h6>
          <ul class="footer-links fw-semibold">
              <li class="nav-item">
                  <router-link class="nav-link" to="/">Главная</router-link>
              </li>
              <li class="nav-item">
                  <router-link class="nav-link" to="/resume-builder">Конструктор резюме</router-link>
              </li>
              <li class="nav-item">
                  <router-link class="nav-link" to="/profile">Профиль</router-link>
              </li>
          </ul>
        </div>

        <!-- Правая колонка: Наши сервисы -->
        <div class="col-lg-4 col-md-6 text-center">
          <h6 class="footer-title">Наши сервисы</h6>
          <ul class="footer-links fw-semibold">
              <li class="nav-item">
                  <router-link class="nav-link" to="/resume-builder">Создание резюме</router-link>
              </li>
              <li class="nav-item">
                  <router-link class="nav-link" to="/analyse">Анализ вакансий</router-link>
              </li>
              <li class="nav-item">
                  <router-link class="nav-link" to="/comb-check">Рассчёт совместимости</router-link>
              </li>
          </ul>
        </div>
      </div>

      <!-- Копирайт и Terms & Conditions -->
      <div class="text-center mt-4">
        <p class="copyright">© 2025 Neuro.Resume. Все права защищены.</p>
        <p class="terms"><a href="#">Terms & Conditions</a></p>
      </div>
    </div>
  </footer>
</template>