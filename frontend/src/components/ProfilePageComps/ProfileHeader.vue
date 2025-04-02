<script setup lang="ts">
import { ref, computed } from 'vue'
import { useProfileStore } from '@/stores/profileStore'
import { useResumeStore } from '@/stores/resumesStore'
import { useAppStore } from '@/stores/appStore'
import { useAuthStore } from '@/stores/authStore'
import { useRouter } from 'vue-router'
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

const authStore = useAuthStore()
const router = useRouter()

const logout = () => {
  authStore.logout()
  router.push('/login')
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
  <div class="profile-box-glow">
    <div class='profile-box p-6 rounded-xl'>
      <div
        v-if="isProfileLoading || !profile || !isAppReady"
        class="min-h-[300px] w-full flex items-center justify-center"
      >
        <div class="animate-spin rounded-full h-12 w-12 border-t-4 border-[var(--text-light)]" />
      </div>

      <section
        v-else
        class="flex flex-col md:flex-row items-center md:items-start gap-8"
      >
      <!-- Аватар -->
      <div
        class="relative group w-28 md:w-32 aspect-square rounded-full overflow-hidden border-2 border-[var(--profile-avatar-border)] cursor-pointer"
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

        <Teleport to="body">
          <AvatarPicker v-if="showAvatarPicker" @close="closeAvatarPicker" />
        </Teleport>

        <!-- Правая часть: инфо -->
        <div class="w-full max-w-4xl flex flex-col gap-2 relative">
          <div class="flex items-start justify-between">
            <div>
              <h1 class="text-2xl sm:text-3xl font-bold text-[var(--text-light)]">
                {{ isEditing ? 'Редактирование профиля' : `Привет, ${profile.name || 'Гость'}!` }}
              </h1>
              <p v-if="mainResume?.job" class="text-sm text-[var(--text-subdued)] pt-2">
                💼 {{ mainResume.job }}
              </p>
            </div>
            <button
              class="p-2 rounded-full"
              title="Настройки"
            >
              <i class="fas fa-cog text-2xl text-[var(--text-light)] rotate-twice-on-hover"></i>
            </button>
          </div>

          <div class="grid grid-cols-2 gap-x-4 gap-y-2 w-max mt-2">
            <div class="min-w-[150px]">
              <label class="text-[var(--text-mainless)] text-xs">Email</label>
              <p class="text-[var(--text-light)] text-sm">{{ profile.email || '-' }}</p>
            </div>
            <div class="min-w-[150px]">
              <label class="text-[var(--text-mainless)] text-xs">Телефон</label>
              <p class="text-[var(--text-light)] text-sm">{{ profile.phone || '-' }}</p>
            </div>
            <div class="col-span-2">
              <label class="text-[var(--text-mainless)] text-xs">Город</label>
              <p class="text-[var(--text-light)] text-sm">{{ profile.city || '-' }}</p>
            </div>
          </div>

          <!-- <div class="flex gap-4 mt-4">
            <button
              class="px-5 py-2 text-sm font-semibold rounded-full transition-all duration-300"
              :class="isEditing ? 'bg-[var(--profile-is-editing)] text-[var(--text-light)]' : 'bg-[var(--background-cta)] text-[var(--text-light)] hover:bg-[var(--background-cta-hover)]'"
              @click="isEditing ? saveProfile() : startEdit()"
            >
              {{ isEditing ? 'Сохранить' : 'Редактировать' }}
            </button>

            <button
              v-if="isEditing"
              class="px-5 py-2 text-sm font-semibold rounded-full bg-red-600 text-white hover:bg-red-700 transition-all duration-300"
              @click="logout"
            >
              Выйти
            </button>
          </div> -->
        </div>
      </section>
    </div>
  </div>
</template>

<style scoped>
.form-input {
  @apply w-full bg-[var(--background-pale)] border border-[var(--background-pale)] rounded-xl px-4 py-2 text-[var(--text-light)] focus:outline-none focus:border-[var(--background-cta)];
}
.profile-box-glow {
  @apply w-full;
  position: relative;
  border-radius: 1.5rem;
  padding: 2px;
  background: var(--gradient-primary);
  animation: glow 3s ease-in-out infinite alternate;
}
.profile-box {
  background-color: var(--background-section);
  border-radius: 1.5rem;
  padding: 2rem;
  position: relative;
  z-index: 1;
  @apply w-full;
}

@keyframes glow {
  0% {
    filter: drop-shadow(0 0 6px var(--neon-blue));
  }
  100% {
    filter: drop-shadow(0 0 10px var(--accent));
  }
}

@keyframes rotate-twice {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(-720deg);
  }
}

.rotate-twice-on-hover:hover {
  animation: rotate-twice 1.5s ease-out 1;
}
</style>
