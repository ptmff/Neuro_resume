<template>
  <section
    v-if="isAppReady"
    class="flex flex-col md:flex-row items-center md:items-start gap-6 md:gap-10"
  >
    <!-- Аватар -->
    <div class="relative w-28 h-28 md:w-32 md:h-32 rounded-full overflow-hidden border-2 border-violet-500/50 shadow-md">
      <img
        :src="profile?.photo || '/placeholder.jpg'"
        alt="Avatar"
        class="w-full h-full object-cover"
      />
      <div class="absolute inset-0 rounded-full bg-violet-500 opacity-20 blur-2xl animate-pulse"></div>
    </div>

    <!-- Информация -->
    <div class="text-center md:text-left w-full max-w-xl">
      <h1 class="text-2xl sm:text-3xl font-bold text-white mb-2">
        {{ isEditing ? 'Редактирование профиля' : `Привет, ${profile?.name || 'Гость'}!` }}
      </h1>

      <!-- Имя и Email -->
      <div class="space-y-4 mb-4">
        <div v-for="key in editableKeys" :key="key">
          <label class="text-white/60 text-sm">{{ labels[key] }}</label>
          <div v-if="isEditing">
            <input
              v-model="editedProfile[key]"
              class="w-full bg-white/10 border border-white/10 rounded-xl px-4 py-2 text-white focus:outline-none focus:border-violet-500"
            />
          </div>
          <p v-else class="text-white text-base">{{ profile?.[key] || '-' }}</p>
        </div>
      </div>

      <!-- Доп. данные -->
      <div class="space-y-1 mb-4 text-sm text-white/80">
        <p v-if="profile?.phone">📞 {{ profile.phone }}</p>
        <p v-if="profile?.city">📍 {{ profile.city }}</p>
        <p v-if="mainResume?.job">
          💼 Профессия: <strong>{{ mainResume.job }}</strong>
        </p>
      </div>

      <!-- Образование -->
      <div v-if="hasEducation" class="mb-4">
        <p class="text-sm text-white/60 mb-1">🎓 Образование:</p>
        <ul class="list-disc list-inside text-white/90 text-sm space-y-1">
          <li v-for="(edu, index) in profile?.education || []" :key="index">
            {{ edu.institution }} — {{ edu.degree }} ({{ edu.startYear }}–{{ edu.endYear }})
          </li>
        </ul>
      </div>

      <!-- Кнопка -->
      <button
        class="px-5 py-2 text-sm font-semibold rounded-full transition-all duration-300"
        :class="isEditing ? 'bg-green-500 text-white' : 'bg-violet-600 text-white hover:bg-violet-700'"
        @click="isEditing ? saveProfile() : startEdit()"
      >
        {{ isEditing ? 'Сохранить' : 'Редактировать' }}
      </button>
    </div>
  </section>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useProfileStore } from '@/stores/profileStore'
import { useResumeStore } from '@/stores/resumesStore'
import { useAppStore } from '@/stores/appStore'
import type { EditableProfileFields } from '@/types/types'

const appStore = useAppStore()
const isAppReady = computed(() => appStore.isAppReady)

const profileStore = useProfileStore()
const resumeStore = useResumeStore()

const profile = computed(() => profileStore.profile)
const editedProfile = computed(() => profileStore.editedProfile)
const isEditing = computed(() => profileStore.isEditing)

const hasEducation = computed(() => {
  const education = profile.value?.education
  return Array.isArray(education) && education.length > 0
})

const mainResume = computed(() => {
  if (!profile.value || !resumeStore.resumes.length) return null
  return resumeStore.resumes.find(r => r.id === profile.value!.mainResumeId) || null
})

const startEdit = () => profileStore.startEdit()
const saveProfile = () => profileStore.saveProfile()

const editableKeys = ['name', 'email'] as const

type EditableKey = typeof editableKeys[number]

const labels: Record<EditableKey, string> = {
  name: 'Имя',
  email: 'Email'
}
</script>
