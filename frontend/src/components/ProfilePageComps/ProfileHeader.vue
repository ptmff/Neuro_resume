<template>
  <section class="flex flex-col md:flex-row items-center md:items-start gap-6 md:gap-10">
    <!-- Аватар -->
    <div
      class="relative w-28 h-28 md:w-32 md:h-32 rounded-full overflow-hidden border-2 border-violet-500/50 shadow-md"
    >
      <img
        :src="profileStore.profile?.photo"
        alt="Avatar"
        class="w-full h-full object-cover"
      />
      <div
        class="absolute inset-0 rounded-full bg-violet-500 opacity-20 blur-2xl animate-pulse"
      ></div>
    </div>

    <!-- Информация -->
    <div class="text-center md:text-left w-full max-w-xl">
      <h1 class="text-2xl sm:text-3xl font-bold text-white mb-2">
        {{
          profileStore.isEditing
            ? 'Редактирование профиля'
            : `Привет, ${profileStore.profile?.name || 'Гость'}!`
        }}
      </h1>

      <!-- Имя и Email -->
      <div class="space-y-4 mb-4">
        <div v-for="key in ['name', 'email']" :key="key">
          <label class="text-white/60 text-sm">{{ labels[key] }}</label>
          <div v-if="profileStore.isEditing">
            <input
              v-model="profileStore.editedProfile[key]"
              class="w-full bg-white/10 border border-white/10 rounded-xl px-4 py-2 text-white focus:outline-none focus:border-violet-500"
            />
          </div>
          <p v-else class="text-white text-base">{{ profileStore.profile?.[key] || '-' }}</p>
        </div>
      </div>

      <!-- Доп. данные -->
      <div class="space-y-1 mb-4 text-sm text-white/80">
        <p v-if="profileStore.profile?.phone">📞 {{ profileStore.profile.phone }}</p>
        <p v-if="profileStore.profile?.city">📍 {{ profileStore.profile.city }}</p>
        <p v-if="resumeStore.mainResume?.job">
          💼 Профессия: <strong>{{ resumeStore.mainResume.job }}</strong>
        </p>
      </div>

      <!-- Образование -->
      <div v-if="profileStore.profile?.education?.length" class="mb-4">
        <p class="text-sm text-white/60 mb-1">🎓 Образование:</p>
        <ul class="list-disc list-inside text-white/90 text-sm space-y-1">
          <li
            v-for="(edu, index) in profileStore.profile.education"
            :key="index"
          >
            {{ edu.institution }} — {{ edu.degree }} ({{ edu.startYear }}–{{ edu.endYear }})
          </li>
        </ul>
      </div>

      <!-- Кнопка -->
      <button
        class="px-5 py-2 text-sm font-semibold rounded-full transition-all duration-300"
        :class="profileStore.isEditing ? 'bg-green-500 text-white' : 'bg-violet-600 text-white hover:bg-violet-700'"
        @click="profileStore.isEditing ? profileStore.saveProfile() : profileStore.startEdit()"
      >
        {{ profileStore.isEditing ? 'Сохранить' : 'Редактировать' }}
      </button>
    </div>
  </section>
</template>

<script setup>
import { onMounted } from 'vue'
import { useProfileStore } from '@/stores/profile'
import { useResumeStore } from '@/stores/resumes'

const profileStore = useProfileStore()
const resumeStore = useResumeStore()

const labels = {
  name: 'Имя',
  email: 'Email',
}

onMounted(async () => {
  if (!profileStore.profile) await profileStore.fetchProfile()
  if (!resumeStore.resumes.length) await resumeStore.fetchResumes()
})
</script>

<style scoped></style>
