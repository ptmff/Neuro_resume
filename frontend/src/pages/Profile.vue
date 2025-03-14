<script setup>
import ProfileCard from '../components/ProfileCard.vue'
import UserResume from '../components/UserResume.vue'
import { onMounted } from 'vue'
import gsap from 'gsap'

onMounted(() => {
  gsap.from('.profile-container > *', {
    y: 50,
    opacity: 0,
    duration: 0.8,
    stagger: 0.2,
    ease: 'power3.out',
  })
})
</script>

<template>
  <div class="profile-container max-w-7xl mx-auto px-6 py-32 grid grid-cols-1 lg:grid-cols-2 gap-8">
    <ProfileCard />
    <UserResume />
  </div>
</template>

<style scoped>
.profile-container {
  min-height: calc(100vh - 120px);
}
</style>
