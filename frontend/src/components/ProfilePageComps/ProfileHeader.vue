<script setup lang="ts">
import { ref, computed } from 'vue'
import { useProfileStore } from '@/stores/profileStore'
import { useResumeStore } from '@/stores/resumesStore'
import { useAppStore } from '@/stores/appStore'
import AvatarPicker from '@/components/ProfilePageComps/AvatarPicker.vue'

const showAvatarPicker = ref(false)
const openAvatarPicker = () => {
  showAvatarPicker.value = true
}

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
const editedPhone = ref('')
const editedCity = ref('')
const editedPhoto = ref('')

const startEdit = () => {
  if (profile.value) {
    editedName.value = profile.value.name || ''
    editedEmail.value = profile.value.email || ''
    editedPhone.value = profile.value.phone || ''
    editedCity.value = profile.value.city || ''
    editedPhoto.value = profile.value.photo || ''
    isEditing.value = true
  }
}

const closeAvatarPicker = async () => {
  showAvatarPicker.value = false
  await profileStore.fetchProfile()
}

const saveProfile = async () => {
  if (!profile.value) return

  const updatedProfile = {
    ...profile.value,
    name: editedName.value,
    email: editedEmail.value,
    phone: editedPhone.value,
    city: editedCity.value,
    photo: editedPhoto.value,
  }

  await profileStore.updateProfile(updatedProfile)
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
  <div
    v-if="isProfileLoading || !profile || !isAppReady"
    class="min-h-[300px] w-full flex items-center justify-center"
  >
    <div class="animate-spin rounded-full h-12 w-12 border-t-4 border-[var(--text-light)]" />
  </div>

  <section
    v-else
    class="flex flex-col md:flex-row items-center md:items-start gap-6 md:gap-10"
  >
<!-- Аватар -->
<div
  class="relative group w-28 h-28 md:w-32 md:h-32 rounded-full overflow-hidden border-2 border-[var(--profile-avatar-border)] cursor-pointer"
  @click="openAvatarPicker"
>
  <img
    :src="isEditing ? editedPhoto : profile.photo || '/placeholder.png'"
    alt="Avatar"
    class="w-full h-full object-cover"
  />
  <div
  class="absolute inset-0 flex items-center justify-center bg-black/50 opacity-0 group-hover:opacity-100 transition-opacity"
>
  <svg class="w-8 h-8 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 4v16h16V4H4zm4 4l4 4m0 0l4-4m-4 4v8" />
  </svg>
</div>

</div>

<!-- Модалка -->
<Teleport to="body">
  <AvatarPicker v-if="showAvatarPicker" @close="closeAvatarPicker" />
</Teleport>


    <!-- Информация -->
    <div class="text-center md:text-left w-full max-w-xl">
      <h1 class="text-2xl sm:text-3xl font-bold text-[var(--text-light)] mb-2">
        {{ isEditing ? 'Редактирование профиля' : `Привет, ${profile.name || 'Гость'}!` }}
      </h1>

      <div class="space-y-4 mb-4">
        <!-- Имя -->
        <div>
          <label class="text-[var(--text-mainless)] text-sm">Имя</label>
          <div v-if="isEditing">
            <input v-model="editedName" class="form-input" />
          </div>
          <p v-else class="text-[var(--text-light)] text-base">{{ profile.name || '-' }}</p>
        </div>

        <!-- Email -->
        <div>
          <label class="text-[var(--text-mainless)] text-sm">Email</label>
          <div v-if="isEditing">
            <input v-model="editedEmail" class="form-input" />
          </div>
          <p v-else class="text-[var(--text-light)] text-base">{{ profile.email || '-' }}</p>
        </div>

        <!-- Телефон -->
        <div>
          <label class="text-[var(--text-mainless)] text-sm">Телефон</label>
          <div v-if="isEditing">
            <input v-model="editedPhone" class="form-input" />
          </div>
          <p v-else class="text-[var(--text-light)] text-base">{{ profile.phone || '-' }}</p>
        </div>

        <!-- Город -->
        <div>
          <label class="text-[var(--text-mainless)] text-sm">Город</label>
          <div v-if="isEditing">
            <input v-model="editedCity" class="form-input" />
          </div>
          <p v-else class="text-[var(--text-light)] text-base">{{ profile.city || '-' }}</p>
        </div>
      </div>

      <!-- Образование -->
      <div v-if="hasEducation" class="mb-4">
        <p class="text-sm text-[var(--text-mainless)] mb-1">🎓 Образование:</p>
        <ul class="list-disc list-inside text-[var(--text-mainless)] text-sm space-y-1">
          <li v-for="(edu, index) in profile.education || []" :key="index">
            {{ edu.institution }} — {{ edu.degree }} ({{ edu.startYear }}–{{ edu.endYear }})
          </li>
        </ul>
      </div>

      <!-- Профессия -->
      <p v-if="mainResume?.job" class="text-sm text-[var(--text-subdued)] mb-4">
        💼 Профессия: <strong>{{ mainResume.job }}</strong>
      </p>

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

<style scoped>
.form-input {
  @apply w-full bg-[var(--background-pale)] border border-[var(--background-pale)] rounded-xl px-4 py-2 text-[var(--text-light)] focus:outline-none focus:border-[var(--background-cta)];
}
</style>
