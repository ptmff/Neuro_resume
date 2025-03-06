<template>
  <footer class="footer py-5 fade-up">
    <div class="container ">
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

<style>
/* 🔹 Основные стили футера */
.footer {
  height: 500px;
  background-color: #fff;
  color: #3b2f2f;
}

footer .row {
  
}

/* 🔹 Логотип */
.footer-brand {
  font-size: 1.5rem;
  font-weight: bold;
  color: #8b5e3c;
  text-decoration: none;
}

/* 🔹 Описание */
.footer-description {
  font-size: 0.9rem;
  color: #6d4c2f;
  max-width: 250px;
}

/* 🔹 Социальные иконки */
.social-icons {
  margin: 15px 0;
}

.social-icon {
  color: #8b5e3c;
  font-size: 1.2rem;
  margin-right: 10px;
  transition: color 0.3s ease;
}

.social-icon:hover {
  color: #3b2f2f;
}

/* 🔹 Заголовки секций */
.footer-title {
  font-size: 1.1rem;
  font-weight: bold;
  color: #3b2f2f;
  margin-bottom: 15px;
}

/* 🔹 Ссылки */
.footer-links {
  list-style: none;
  padding: 0;
}

.footer-links li {
  margin-bottom: 8px;
}

.footer-links a {
  text-decoration: none;
  color: #6d4c2f;
  transition: color 0.3s ease;
}

.footer-links a:hover {
  color: #8b5e3c;
}

/* 🔹 Юридическая информация */
.terms {
  margin-top: 5px;
}

.terms a {
  font-size: 0.9rem;
  color: #6d4c2f;
  text-decoration: none;
}

.terms a:hover {
  color: #8b5e3c;
}

/* 🔹 Копирайт */
.copyright {
  font-size: 0.9rem;
  color: #6d4c2f;
}

/* 🔹 Адаптация <991px */
@media (max-width: 991px) {
    .footer {
      height: 600px;
    }
    .footer-description {
        margin: 0 auto;
    }
}

/* 🔹 Полная адаптация <768px */
@media (max-width: 768px) {
  .footer {
    height: 700px;
  }
  .footer .row {
    display: flex;
    flex-direction: column;
    align-items: center;
    text-align: center;
  }

  .footer-links {
    width: 100%;
    text-align: center;
  }
}
</style>