<template>
  <div class="relative min-h-screen overflow-hidden mx-auto px-6 py-16">
    <div class="max-w-6xl mx-auto px-4 sm:px-6 py-16 space-y-16">
      <div data-aos="fade-down" data-aos-delay="100">
        <ProfileHeader />
      </div>
      <div data-aos="fade-down" data-aos-delay="200">
        <ResumeModuleList />
      </div>
      <div data-aos="fade-down" data-aos-delay="300">
        <SkillsFlow />
      </div>
      <div data-aos="fade-down" data-aos-delay="400">
        <NeuroAssistant />
      </div>
      <div data-aos="fade-down" data-aos-delay="400">
        <ProfileStats />
      </div>
    </div>
  </div>
</template>

<script setup>
import ProfileHeader from '@/components/ProfilePageComps/ProfileHeader.vue'
import ResumeModuleList from '@/components/ProfilePageComps/ResumeModuleList.vue'
import SkillsFlow from '@/components/ProfilePageComps/SkillsFlow.vue'
import NeuroAssistant from '@/components/ProfilePageComps/NeuroAssistant.vue'
import ProfileStats from '@/components/ProfilePageComps/ProfileStats.vue'
</script>

<style scoped></style>
