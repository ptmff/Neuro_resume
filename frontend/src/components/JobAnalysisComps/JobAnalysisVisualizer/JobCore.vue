<template>
  <div class="relative flex items-center justify-center w-full h-[300px] sm:h-[400px] md:h-[500px]">
    <!-- Пульсирующие круги -->
    <div
      v-for="n in 3"
      :key="n"
      class="absolute w-32 h-32 rounded-full bg-gradient-to-br from-violet-500 to-indigo-500 opacity-30 animate-ping"
      :style="{
        animationDelay: `${n * 0.5}s`,
        width: `${8 + n * 4}rem`,
        height: `${8 + n * 4}rem`,
      }"
    />

    <!-- Центр круга -->
    <div
      class="relative z-10 w-24 h-24 rounded-full bg-gradient-to-br from-purple-600 to-indigo-600 shadow-xl flex items-center justify-center"
    >
      <i class="fas fa-brain text-white text-3xl"></i>
    </div>
  </div>
</template>

<script setup>
// Пока без логики, только визуальный эффект
</script>

<style scoped>
@keyframes pulse {
  0% {
    transform: scale(1);
    opacity: 0.3;
  }
  100% {
    transform: scale(2);
    opacity: 0;
  }
}
</style>
