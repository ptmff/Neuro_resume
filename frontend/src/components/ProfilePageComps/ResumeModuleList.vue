<script setup lang="ts">
import { computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useProfileStore } from '@/stores/profileStore'
import { useResumeStore } from '@/stores/resumesStore'
import type { Resume } from '@/types/types'

const profileStore = useProfileStore()
const resumeStore = useResumeStore()
const router = useRouter()

const profile = computed(() => profileStore.profile)
const resumes = computed(() => resumeStore.resumes)

// Все резюме пользователя
const userResumes = computed((): Resume[] => resumes.value)

const formatDate = (dateString: string) => dateString.split('T')[0]

// Редактировать резюме
const editResume = (resume: Resume) => {
  resumeStore.setResumeForEdit(resume)
  router.push('/resume')
}

// Создать новое резюме
const createNewResume = () => {
  resumeStore.setResumeForEdit(null)
  router.push('/resume')
}

// Сделать резюме основным
const setAsMain = async (id: number) => {
  await profileStore.updateProfile({ mainResumeId: id })
}

// Удалить резюме
const deleteResume = async (id: number) => {
  await resumeStore.deleteResume(id)

  // Если удалённое резюме было основным — сбрасываем mainResumeId
  if (profile.value?.mainResumeId === id) {
    await profileStore.updateProfile({ mainResumeId: null })
  }
}

// Загрузка при монтировании
onMounted(async () => {
  if (!profile.value) await profileStore.fetchProfile()
  if (!resumes.value.length) await resumeStore.fetchResumes()
})
</script>

<template>
  <section class="flex flex-col gap-6">
    <!-- Заголовок -->
    <div>
      <h2 class="text-xl sm:text-2xl font-bold text-[var(--text-light)]">Мои резюме</h2>
      <p class="text-sm text-[var(--text-mainless)]">Управляй всеми своими резюме в одном месте</p>
    </div>

    <!-- Список резюме -->
    <div v-if="userResumes.length" class="flex flex-col gap-3">
      <div
        v-for="resume in userResumes"
        :key="resume.id"
        class="flex justify-between items-center bg-[var(--background-overlay)] hover:bg-[var(--background-pale)] transition border border-[var(--background-pale)] rounded-xl px-4 py-3"
      >
        <div>
          <h3 class="text-[var(--text-light)] font-semibold text-base sm:text-lg flex items-center gap-2">
            {{ resume.title }}
            <span
              v-if="resume.id === profile?.mainResumeId"
              class="text-[var(--background-cta)] text-base"
            >
              ★
            </span>
          </h3>
          <p class="text-xs text-[var(--text-mainless)] mt-1">{{ formatDate(resume.date) }}</p>
        </div>

        <div class="flex items-center gap-2">
          <button
            @click="setAsMain(resume.id)"
            class="p-2 rounded-full bg-[var(--background-pale)] hover:bg-[var(--background-indicator)] transition"
            title="Сделать основным"
          >
            <i class="fas fa-star text-[var(--text-light)] text-sm"></i>
          </button>
          <button
            @click="editResume(resume)"
            class="p-2 rounded-full bg-[var(--background-pale)] hover:bg-[var(--background-indicator)] transition"
            title="Редактировать"
          >
            <i class="fas fa-edit text-[var(--text-light)] text-sm"></i>
          </button>
          <button
            @click="deleteResume(resume.id)"
            class="p-2 rounded-full bg-[var(--background-pale)] hover:bg-[var(--background-indicator)] transition"
            title="Удалить"
          >
            <i class="fas fa-trash text-[var(--background-cta)] text-sm"></i>
          </button>
        </div>
      </div>

      <!-- Кнопка "Создать резюме" -->
      <div
        class="bg-[var(--background-overlay)] hover:bg-[var(--background-pale)] border border-dashed border-[var(--background-pale)] rounded-xl px-4 py-6 flex flex-col items-center justify-center text-[var(--text-light)] transition cursor-pointer"
        @click="createNewResume"
      >
        <i class="fas fa-plus text-xl mb-1"></i>
        <span class="text-sm">Создать новое резюме</span>
      </div>
    </div>

<!-- Если нет резюме -->
<div v-else class="flex flex-col items-center justify-center gap-4 py-10 text-[var(--text-light)]">
  <p class="text-sm text-[var(--text-mainless)]">У вас пока нет резюме.</p>
  <button
    class="flex items-center gap-2 px-5 py-2 text-sm font-semibold rounded-full bg-[var(--background-cta)] hover:bg-[var(--background-cta-hover)] transition"
    @click="createNewResume"
  >
    <i class="fas fa-plus"></i>
    <span>Создать новое резюме</span>
  </button>
</div>

  </section>
</template>
