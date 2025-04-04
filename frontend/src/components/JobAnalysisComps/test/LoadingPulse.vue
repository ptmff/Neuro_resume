<template>
  <div class="flex flex-col items-center justify-center gap-6 text-center animate-fade-in">
    <!-- Обёртка с эффектами -->
    <div class="relative w-28 h-28">
      <!-- Пульс -->
      <div class="absolute inset-0 rounded-full blur-2xl opacity-60 animate-pulse-slow"
           style="background: var(--gradient-primary);"></div>

      <!-- Дышащий круг -->
      <div class="absolute inset-0 rounded-full border-[3px] border-[var(--neon-blue)] bg-transparent shadow-[0_0_30px_var(--neon-purple)] animate-breathe z-10"></div>

      <!-- SVG-сетка -->
      <svg viewBox="0 0 100 100"
           class="absolute inset-0 w-full h-full animate-spin-slow z-20 opacity-40 pointer-events-none">
        <defs>
          <radialGradient id="gridFade" r="75%" cx="50%" cy="50%">
            <stop offset="0%" stop-color="#00e5ff" stop-opacity="0.3" />
            <stop offset="100%" stop-color="transparent" stop-opacity="0" />
          </radialGradient>
        </defs>

        <g stroke="url(#gridFade)" stroke-width="0.4">
          <!-- Вертикальные -->
          <line x1="10" y1="0" x2="10" y2="100" />
          <line x1="20" y1="0" x2="20" y2="100" />
          <line x1="30" y1="0" x2="30" y2="100" />
          <line x1="40" y1="0" x2="40" y2="100" />
          <line x1="50" y1="0" x2="50" y2="100" />
          <line x1="60" y1="0" x2="60" y2="100" />
          <line x1="70" y1="0" x2="70" y2="100" />
          <line x1="80" y1="0" x2="80" y2="100" />
          <line x1="90" y1="0" x2="90" y2="100" />

          <!-- Горизонтальные -->
          <line x1="0" y1="10" x2="100" y2="10" />
          <line x1="0" y1="20" x2="100" y2="20" />
          <line x1="0" y1="30" x2="100" y2="30" />
          <line x1="0" y1="40" x2="100" y2="40" />
          <line x1="0" y1="50" x2="100" y2="50" />
          <line x1="0" y1="60" x2="100" y2="60" />
          <line x1="0" y1="70" x2="100" y2="70" />
          <line x1="0" y1="80" x2="100" y2="80" />
          <line x1="0" y1="90" x2="100" y2="90" />
        </g>
      </svg>
    </div>

    <!-- Текст -->
    <div class="space-y-1 text-[var(--text-light)]">
      <p class="text-xl font-semibold tracking-wide">🤖 AI подключается…</p>
      <p class="text-sm text-[var(--text-secondary)]">Анализируем вакансию, сопоставляем с вашим резюме</p>
    </div>
  </div>
</template>

<style scoped>
@keyframes fade-in {
  from {
    opacity: 0;
    transform: translateY(10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
.animate-fade-in {
  animation: fade-in 0.6s ease-out forwards;
}

@keyframes spin-slow {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
.animate-spin-slow {
  animation: spin-slow 20s linear infinite;
}

@keyframes pulse-slow {
  0%, 100% {
    opacity: 0.6;
    transform: scale(1);
  }
  50% {
    opacity: 1;
    transform: scale(1.1);
  }
}
.animate-pulse-slow {
  animation: pulse-slow 4s ease-in-out infinite;
}

@keyframes breathe {
  0%, 100% {
    transform: scale(1);
    box-shadow: 0 0 20px var(--neon-purple);
  }
  50% {
    transform: scale(1.06);
    box-shadow: 0 0 40px var(--neon-blue);
  }
}
.animate-breathe {
  animation: breathe 2.8s ease-in-out infinite;
}
</style>
