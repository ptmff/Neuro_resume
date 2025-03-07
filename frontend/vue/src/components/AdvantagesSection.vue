<template>
    <section class="advantages fade-up">
      <div class="container">
        <h2 class="section-title text-center">Почему выбирают NeuroResume?</h2>
        
        <div class="advantages-content">
          <!-- Список преимуществ -->
          
          
        </div>
      </div>
    </section>
</template>

<script setup>
import { ref } from 'vue';
import aiGen1 from '../assets/ai-gen1.jpg'

const activeIndex = ref(0);

const setActiveAdvantage = (index) => {
  activeIndex.value = index;
};
</script>