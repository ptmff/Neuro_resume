<template>
  <section class="mt-12">
    <h2 class="text-xl sm:text-2xl font-bold text-[var(--text-light)] mb-4">Статистика профиля</h2>
    <div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-4">
      <div
        v-for="(item, index) in stats"
        :key="index"
        class="bg-[var(--background-overlay)] border border-[var(--background-pale)] rounded-xl p-4 text-center"
      >
        <h3 class="text-2xl font-bold text-[var(--text-light)]">{{ item.value }}</h3>
        <p class="text-sm text-[var(--text-mainless)] mt-1">{{ item.label }}</p>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useProfileStore } from '@/stores/profileStore'
import { useResumeStore } from '@/stores/resumesStore'

const profileStore = useProfileStore()
const resumeStore = useResumeStore()

// ✅ Резюме считаем из resumeStore
const resumeCount = computed(() => resumeStore.resumes.length)

// ✅ Навыки суммируем по всем резюме
const totalSkills = computed(() =>
  resumeStore.resumes.reduce((acc, resume) => acc + (resume.skills?.length ?? 0), 0)
)

// ✅ Основное резюме — из profileStore
const hasMainResume = computed(() => !!profileStore.profile?.mainResumeId)

const mainResumeLabel = computed(() =>
  profileStore.profile?.mainResumeId ? `#${profileStore.profile.mainResumeId}` : 'Нет'
)

// ✅ Заполненность профиля (зависит от profile и resumeStore)
const profileCompletion = computed(() => {
  let score = 0
  const p = profileStore.profile
  if (!p) return '0%'
  if (p.name) score++
  if (p.email) score++
  if (resumeCount.value > 0) score++
  if (hasMainResume.value) score++
  return Math.round((score / 4) * 100) + '%'
})

// ✅ Объединяем статистику
const stats = computed(() => [
  { label: 'Создано резюме', value: resumeCount.value },
  { label: 'Указано навыков', value: totalSkills.value },
  { label: 'Основное резюме', value: mainResumeLabel.value },
  { label: 'Заполненность профиля', value: profileCompletion.value }
])
</script>
