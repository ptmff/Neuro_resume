<script setup lang="ts">
import { ref, computed } from 'vue'
import { useProfileStore } from '@/stores/profileStore'
import { useResumeStore } from '@/stores/resumesStore'
import { useAppStore } from '@/stores/appStore'

const appStore = useAppStore()
const isAppReady = computed(() => appStore.isAppReady)

const profileStore = useProfileStore()
const resumeStore = useResumeStore()

const profile = computed(() => profileStore.profile)
const isProfileLoading = computed(() => profileStore.loading)

const isEditing = ref(false)

// Локальное редактируемое состояние
const editedName = ref('')
const editedEmail = ref('')

const startEdit = () => {
  if (profile.value) {
    editedName.value = profile.value.name || ''
    editedEmail.value = profile.value.email || ''
    isEditing.value = true
  }
}

const saveProfile = async () => {
  await profileStore.updateProfile({
    name: editedName.value,
    email: editedEmail.value,
  })
  isEditing.value = false
}

const hasEducation = computed(() => {
  const education = profile.value?.education
  return Array.isArray(education) && education.length > 0
})

const mainResume = computed(() => {
  if (!profile.value || !resumeStore.resumes.length) return null
  return resumeStore.resumes.find(r => r.id === profile.value!.mainResumeId) || null
})
</script>

<template>
  <!-- 🌀 Лоадер -->
  <div
    v-if="isProfileLoading || !profile || !isAppReady"
    class="min-h-[300px] w-full flex items-center justify-center"
  >
    <div class="animate-spin rounded-full h-12 w-12 border-t-4 border-[var(--text-light)]" />
  </div>

  <!-- 👤 Профиль -->
  <section
    v-else
    class="flex flex-col md:flex-row items-center md:items-start gap-6 md:gap-10"
  >
    <!-- Аватар -->
    <div
      class="relative w-28 h-28 md:w-32 md:h-32 rounded-full overflow-hidden border-2 border-[var(--profile-avatar-border)] shadow-md"
    >
      <img
        :src="profile.photo || '/placeholder.jpg'"
        alt="Avatar"
        class="w-full h-full object-cover"
      />
      <div class="absolute inset-0 rounded-full bg-[var(--background-cta)] opacity-20 blur-2xl animate-pulse" />
    </div>

    <!-- Информация -->
    <div class="text-center md:text-left w-full max-w-xl">
      <h1 class="text-2xl sm:text-3xl font-bold text-[var(--text-light)] mb-2">
        {{ isEditing ? 'Редактирование профиля' : `Привет, ${profile.name || 'Гость'}!` }}
      </h1>

      <!-- Имя и Email -->
      <div class="space-y-4 mb-4">
        <div>
          <label class="text-[var(--text-mainless)] text-sm">Имя</label>
          <div v-if="isEditing">
            <input
              v-model="editedName"
              class="w-full bg-[var(--background-pale)] border border-[var(--background-pale)] rounded-xl px-4 py-2 text-[var(--text-light)] focus:outline-none focus:border-[var(--background-cta)]"
            />
          </div>
          <p v-else class="text-[var(--text-light)] text-base">{{ profile.name || '-' }}</p>
        </div>

        <div>
          <label class="text-[var(--text-mainless)] text-sm">Email</label>
          <div v-if="isEditing">
            <input
              v-model="editedEmail"
              class="w-full bg-[var(--background-pale)] border border-[var(--background-pale)] rounded-xl px-4 py-2 text-[var(--text-light)] focus:outline-none focus:border-[var(--background-cta)]"
            />
          </div>
          <p v-else class="text-[var(--text-light)] text-base">{{ profile.email || '-' }}</p>
        </div>
      </div>

      <!-- Доп. данные -->
      <div class="space-y-1 mb-4 text-sm text-[var(--text-subdued)]">
        <p v-if="profile.phone">📞 {{ profile.phone }}</p>
        <p v-if="profile.city">📍 {{ profile.city }}</p>
        <p v-if="mainResume?.job">
          💼 Профессия: <strong>{{ mainResume.job }}</strong>
        </p>
      </div>

      <!-- Образование -->
      <div v-if="hasEducation" class="mb-4">
        <p class="text-sm text-[var(--text-mainless)] mb-1">🎓 Образование:</p>
        <ul class="list-disc list-inside text-[var(--text-mainless)] text-sm space-y-1">
          <li
            v-for="(edu, index) in profile.education || []"
            :key="index"
          >
            {{ edu.institution }} — {{ edu.degree }} ({{ edu.startYear }}–{{ edu.endYear }})
          </li>
        </ul>
      </div>

      <!-- Кнопка -->
      <button
        class="px-5 py-2 text-sm font-semibold rounded-full transition-all duration-300"
        :class="isEditing ? 'bg-[var(--profile-is-editing)] text-[var(--text-light)]' : 'bg-[var(--background-cta)] text-[var(--text-light)] hover:bg-[var(--background-cta-hover)]'"
        @click="isEditing ? saveProfile() : startEdit()"
      >
        {{ isEditing ? 'Сохранить' : 'Редактировать' }}
      </button>
    </div>
  </section>
</template>
