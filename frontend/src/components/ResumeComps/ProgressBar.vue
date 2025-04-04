<template>
  <div class="p-5">
    <div class="flex justify-between items-center w-full">
      <!-- Шаг 1: Данные -->
      <div 
        class="flex flex-col items-center relative z-10 cursor-pointer"
        :class="{ 'active': currentStep >= 1 }"
        @click="goToStep(1)"
      >
        <div 
          class="w-[50px] h-[50px] rounded-full flex justify-center items-center mb-2 transition-all duration-300"
          :class="{
            'bg-[var(--background-pale)] shadow-[0_0_10px_var(--progressbar-shadow)] hover:shadow-[0_0_20px_var(--progressbar-shadow-hover)]': currentStep < 1,
            'bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] text-[var(--text-light)] shadow-[0_0_10px_var(--progressbar-shadow)]': currentStep >= 1
          }"
        >
          <i class="fas fa-user text-[1.2rem]" :class="currentStep >= 1 ? 'opacity-100' : 'opacity-40'"></i>
        </div>
        <div class="text-[0.9rem] font-medium">Данные</div>
      </div>

      <!-- Линия между шагами 1 и 2 -->
      <div 
        class="flex-grow h-[3px] mx-[10px] relative -top-[15px] z-0 transition-all duration-500"
        :class="currentStep >= 2 ? 'bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)]' : 'bg-[var(--background-pale)]'"
      ></div>

      <!-- Шаг 2: Стиль -->
      <div 
        class="flex flex-col items-center relative z-10 cursor-pointer"
        :class="{ 'active': currentStep >= 2 }"
        @click="goToStep(2)"
      >
        <div 
          class="w-[50px] h-[50px] rounded-full flex justify-center items-center mb-2 transition-all duration-300"
          :class="{
            'bg-[var(--background-pale)] shadow-[0_0_10px_var(--progressbar-shadow)] hover:shadow-[0_0_20px_var(--progressbar-shadow-hover)]': currentStep < 2,
            'bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] text-[var(--text-light)] shadow-[0_0_10px_var(--progressbar-shadow)]': currentStep >= 2
          }"
        >
          <i class="fas fa-palette text-[1.2rem]" :class="currentStep >= 2 ? 'opacity-100' : 'opacity-40'"></i>
        </div>
        <div class="text-[0.9rem] font-medium">Стиль</div>
      </div>

      <!-- Линия между шагами 2 и 3 -->
      <div 
        class="flex-grow h-[3px] mx-[10px] relative -top-[15px] z-0 transition-all duration-500"
        :class="currentStep >= 3 ? 'bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)]' : 'bg-[var(--background-pale)]'"
      ></div>

      <!-- Шаг 3: AI -->
      <div 
        class="flex flex-col items-center relative z-10 cursor-pointer"
        :class="{ 'active': currentStep >= 3 }"
        @click="goToStep(3)"
      >
        <div 
          class="w-[50px] h-[50px] rounded-full flex justify-center items-center mb-2 transition-all duration-300"
          :class="{
            'bg-[var(--background-pale)] shadow-[0_0_10px_var(--progressbar-shadow)] hover:shadow-[0_0_20px_var(--progressbar-shadow-hover)]': currentStep < 3,
            'bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] text-[var(--text-light)] shadow-[0_0_10px_var(--progressbar-shadow)]': currentStep >= 3
          }"
        >
          <i class="fas fa-robot text-[1.2rem]" :class="currentStep >= 3 ? 'opacity-100' : 'opacity-40'"></i>
        </div>
        <div class="text-[0.9rem] font-medium">AI</div>
      </div>

      <!-- Линия между шагами 3 и 4 -->
      <div 
        class="flex-grow h-[3px] mx-[10px] relative -top-[15px] z-0 transition-all duration-500"
        :class="currentStep >= 4 ? 'bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)]' : 'bg-[var(--background-pale)]'"
      ></div>

      <!-- Шаг 4: Обзор -->
      <div 
        class="flex flex-col items-center relative z-10 cursor-pointer"
        :class="{ 'active': currentStep >= 4 }"
        @click="goToStep(4)"
      >
        <div 
          class="w-[50px] h-[50px] rounded-full flex justify-center items-center mb-2 transition-all duration-300"
          :class="{
            'bg-[var(--background-pale)] shadow-[0_0_10px_var(--progressbar-shadow)] hover:shadow-[0_0_20px_var(--progressbar-shadow-hover)]': currentStep < 4,
            'bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] text-[var(--text-light)] shadow-[0_0_10px_var(--progressbar-shadow)]': currentStep >= 4
          }"
        >
          <i class="fas fa-eye text-[1.2rem]" :class="currentStep >= 4 ? 'opacity-100' : 'opacity-40'"></i>
        </div>
        <div class="text-[0.9rem] font-medium">Обзор</div>
      </div>

      <!-- Линия между шагами 4 и 5 -->
      <div 
        class="flex-grow h-[3px] mx-[10px] relative -top-[15px] z-0 transition-all duration-500"
        :class="currentStep >= 5 ? 'bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)]' : 'bg-[var(--background-pale)]'"
      ></div>

      <!-- Шаг 5: Скачать -->
      <div 
        class="flex flex-col items-center relative z-10 cursor-pointer"
        :class="{ 'active': currentStep >= 5 }"
        @click="goToStep(5)"
      >
        <div 
          class="w-[50px] h-[50px] rounded-full flex justify-center items-center mb-2 transition-all duration-300"
          :class="{
            'bg-[var(--background-pale)] shadow-[0_0_10px_var(--progressbar-shadow)] hover:shadow-[0_0_20px_var(--progressbar-shadow-hover)]': currentStep < 5,
            'bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] text-[var(--text-light)] shadow-[0_0_10px_var(--progressbar-shadow)]': currentStep >= 5
          }"
        >
          <i class="fas fa-download text-[1.2rem]" :class="currentStep >= 5 ? 'opacity-100' : 'opacity-40'"></i>
        </div>
        <div class="text-[0.9rem] font-medium">Скачать</div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
interface ProgressBarProps {
  currentStep: number;
}

const props = defineProps<ProgressBarProps>()

const emit = defineEmits<{
  (e: 'step-click', step: number): void;
}>()

const goToStep = (step: number): void => {
  emit('step-click', step)
}
</script>

<style>


/* Адаптивность для мобильных устройств */
@media (max-width: 768px) {
  .flex.justify-between {
    flex-direction: column;
    align-items: flex-start;
  }
  
  .flex.flex-col {
    flex-direction: row;
    width: 100%;
  }
  
  .w-\[50px\] {
    margin-right: 15px;
    margin-bottom: 0;
  }
  
  .h-\[3px\] {
    width: 3px;
    height: 20px;
    margin: 15px 0;
    margin-left: 25px;
    top: 0;
  }
}
</style>