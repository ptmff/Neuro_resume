<script setup>
defineProps({
  visible: Boolean,
})
</script>

<template>
  <footer
    class="w-full py-20 fixed bottom-0 left-0 z-20 bg-[var(--bg-main)] text-[var(--text-secondary)] transition-transform duration-500"
    :class="{ 'translate-y-0': visible, 'translate-y-full': !visible }"
  >
    <div class="max-w-7xl mx-auto px-6 sm:px-16 transition-all duration-500">
      <!-- Верхний блок: логотип + миссия -->
      <div class="text-center mb-12">
        <h2 class="text-3xl sm:text-4xl font-bold text-[var(--text-main)] mb-4">
          Спасибо за использование Neuro.Resume!
        </h2>
        <p class="text-[var(--text-mainless)] max-w-xl mx-auto text-base sm:text-lg">
          Надеемся, что наш анализ поможет тебе найти работу мечты 💼
        </p>
      </div>

      <!-- Блок с колонками -->
      <div class="flex flex-col md:flex-row justify-between gap-12 text-sm sm:text-base">
        <!-- Колонка 1 -->
        <div class="flex-1">
          <h6 class="text-xl font-semibold text-[var(--text-main)] mb-4">Навигация</h6>
          <ul class="space-y-4">
            <li>
              <router-link class="hover:text-[var(--text-light)]" to="/">Главная</router-link>
            </li>
            <li>
              <router-link class="hover:text-[var(--text-light)]" to="/resume"
                >Создать резюме</router-link
              >
            </li>
            <li>
              <router-link class="hover:text-[var(--text-light)]" to="/analyse"
                >Анализ вакансий</router-link
              >
            </li>
            <li>
              <router-link class="hover:text-[var(--text-light)]" to="/profile"
                >Профиль</router-link
              >
            </li>
          </ul>
        </div>

        <!-- Колонка 2 -->
        <div class="flex-1">
          <h6 class="text-xl font-semibold text-[var(--text-main)] mb-4">Полезные ссылки</h6>
          <ul class="space-y-4">
            <li>
              <a
                href="https://t.me/s/jojiikk"
                target="_blank"
                class="hover:text-[var(--text-light)]"
                >Telegram</a
              >
            </li>
            <li>
              <a
                href="https://vk.com/jojiikk"
                target="_blank"
                class="hover:text-[var(--text-light)]"
                >VK</a
              >
            </li>
            <li>
              <a
                href="https://github.com/xle6yc3k/NeuroResume"
                target="_blank"
                class="hover:text-[var(--text-light)]"
                >GitHub</a
              >
            </li>
          </ul>
        </div>

        <!-- Колонка 3 -->
        <div class="flex-1">
          <h6 class="text-xl font-semibold text-[var(--text-main)] mb-4">Контакты</h6>
          <p class="text-white/70 mb-2">По вопросам сотрудничества:</p>
          <p class="text-white font-mono">alex@example.com</p>
        </div>
      </div>

      <!-- Нижняя подпись -->
      <div class="mt-20 text-center text-[var(--icons-bfr-hover)] text-sm">
        © 2025 Neuro.Resume • Сделано с любовью и Vue
      </div>
    </div>
  </footer>
</template>

<style scoped>
footer {
  animation: fadeInUp 1s ease-out;
}
@keyframes fadeInUp {
  0% {
    opacity: 0;
    transform: translateY(60px);
  }
  100% {
    opacity: 1;
    transform: translateY(0);
  }
}
</style>
