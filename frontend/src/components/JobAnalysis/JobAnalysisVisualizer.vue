<template>
    <div class="fixed inset-0 z-50 bg-[var(--background-page)]">
      <JobCore />
      <FloatingWords />
      <SkillLinkMap />
      <LoadingPulse class="absolute inset-0" />
    </div>
  </template>
  
  
  <script setup>
  import JobCore from './JobAnalysisVisualizer/JobCore.vue'
  import FloatingWords from './JobAnalysisVisualizer/FloatingWords.vue'
  import SkillLinkMap from './JobAnalysisVisualizer/SkillLinkMap.vue'
  import LoadingPulse from './LoadingPulse.vue'
  </script>
  