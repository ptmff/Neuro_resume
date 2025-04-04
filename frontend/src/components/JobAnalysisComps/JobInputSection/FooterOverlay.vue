<template>
    <footer class="w-full py-20 mt-40 relative text-[var(--text-secondary)] bg-[var(--bg-main)] ">
      <div class="max-w-7xl mx-auto px-6 sm:px-16">
        <!-- Основной flex-контейнер -->
        <div class="flex flex-col lg:flex-row lg:justify-between lg:items-center">
          <!-- Блок Neuro.Resume -->
          <div
            class="text-center text-xl font-bold mb-6 lg:mb-0 lg:text-left text-[var(--text-main)]"
          >
            &copy; 2025 Neuro.Resume •
          </div>
  
          <!-- Блок с колонками (Услуги и Ссылки) -->
          <div class="flex justify-center space-x-16 lg:justify-end font-semibold">
            <!-- Колонка: Услуги -->
            <div class="text-left">
              <h6 class="text-xl text-[var(--text-main)]">Услуги</h6>
              <ul class="mt-4 space-y-8 pt-4">
                <li>
                  <router-link class="hover:text-[var(--text-light)]" to="/resume">
                    Создание резюме
                  </router-link>
                </li>
                <li>
                  <router-link class="hover:text-[var(--text-light)]" to="/analyse">
                    Анализ вакансий
                  </router-link>
                </li>
              </ul>
            </div>
  
            <!-- Колонка: Ссылки -->
            <div class="text-left">
              <h6 class="text-xl text-[var(--text-main)]">Ссылки</h6>
              <ul class="mt-4 space-y-8 pt-4">
                <li>
                  <a href="https://t.me/s/jojiikk" class="hover:text-[var(--text-light)]">
                    Telegram <i class="social-icon fab fa-telegram"></i>
                  </a>
                </li>
                <li>
                  <a href="https://vk.com/jojiikk" class="hover:text-[var(--text-light)]">
                    VKontakte <i class="social-icon fab fa-vk"></i>
                  </a>
                </li>
                <li>
                  <a
                    href="https://github.com/xle6yc3k/NeuroResume"
                    class="hover:text-[var(--text-light)]"
                  >
                    GitHub <i class="social-icon fab fa-github"></i>
                  </a>
                </li>
              </ul>
            </div>
          </div>
        </div>
      </div>
    </footer>
  </template>
  
  <script setup>
  // Никакой логики не требуется — просто "визуальный" компонент
  </script>
  
  <style scoped>
  /* Нужна ли кастомизация иконок? Можно подключить Font Awesome, если ещё не подключено */
  .social-icon {
    margin-left: 0.3rem;
  }
  </style>
  