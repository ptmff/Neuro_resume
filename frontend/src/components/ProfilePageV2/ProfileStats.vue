<template>
    <section class="mt-12">
      <h2 class="text-xl sm:text-2xl font-bold text-white mb-4">Статистика профиля</h2>
      <div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-4">
            <div
                v-for="(item, index) in stats"
                :key="index"
                class="bg-white/5 border border-white/10 rounded-xl p-4 text-center"
            >
                <h3 class="text-2xl font-bold text-white">{{ item.value }}</h3>
                <p class="text-sm text-white/60 mt-1">{{ item.label }}</p>
            </div>
      </div>
    </section>
  </template>
  
  <script setup>
  import { computed } from 'vue'
  import { useProfileStore } from '@/stores/profile'
  
  const store = useProfileStore()
  const resumeCount = computed(() => store.profile?.resumes?.length || 0)

  const totalSkills = computed(() => 
    store.allResumes.reduce((acc, r) => acc + (r.skills?.length || 0), 0)
  )

  const hasMainResume = computed(() => !!store.profile?.mainResumeId)

  const mainResumeLabel = computed(() => {
  return store.profile?.mainResumeId ? `#${store.profile.mainResumeId}` : 'Нет'
})
  
  const profileCompletion = computed(() => {
    let score = 0
    if (store.profile?.name) score += 1
    if (store.profile?.email) score += 1
    if (resumeCount.value > 0) score += 1
    if (hasMainResume.value) score += 1
    return Math.round((score / 4) * 100) + '%'
  })

  const stats = computed(() => [
  { label: 'Создано резюме', value: resumeCount.value },
  { label: 'Указано навыков', value: totalSkills.value },
  { label: 'Основное резюме', value: mainResumeLabel.value },
  { label: 'Заполненность профиля', value: profileCompletion.value }
])

  </script>
  
  <style scoped>
  </style>