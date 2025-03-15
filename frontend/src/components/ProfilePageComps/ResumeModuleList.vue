<template>
  <section class="flex flex-col gap-6">
    <!-- Заголовок -->
    <div>
      <h2 class="text-xl sm:text-2xl font-bold text-white">Мои резюме</h2>
      <p class="text-sm text-white/60">Управляй всеми своими резюме в одном месте</p>
    </div>

    <!-- Список -->
    <div v-if="userResumes.length" class="flex flex-col gap-3">
      <div
        v-for="resume in userResumes"
        :key="resume.id"
        class="flex justify-between items-center bg-white/5 hover:bg-white/10 transition border border-white/10 rounded-xl px-4 py-3"
      >
        <div>
          <h3 class="text-white font-semibold text-base sm:text-lg flex items-center gap-2">
            {{ resume.title }}
            <span
              v-if="resume.id === profileStore.profile?.mainResumeId"
              class="text-violet-400 text-base"
            >
              ★
            </span>
          </h3>
          <p class="text-xs text-white/50 mt-1">{{ resume.date }}</p>
        </div>

        <div class="flex items-center gap-2">
          <button
            @click="setAsMain(resume.id)"
            class="p-2 rounded-full bg-white/10 hover:bg-white/20 transition"
            title="Сделать основным"
          >
            <i class="fas fa-star text-white text-sm"></i>
          </button>
          <button
            @click="editResume(resume)"
            class="p-2 rounded-full bg-white/10 hover:bg-white/20 transition"
            title="Редактировать"
          >
            <i class="fas fa-edit text-white text-sm"></i>
          </button>
          <button
            @click="deleteResume(resume.id)"
            class="p-2 rounded-full bg-white/10 hover:bg-white/20 transition"
            title="Удалить"
          >
            <i class="fas fa-trash text-pink-400 text-sm"></i>
          </button>
        </div>
      </div>

      <div
        class="bg-white/5 hover:bg-white/10 border border-dashed border-white/10 rounded-xl px-4 py-6 flex flex-col items-center justify-center text-white transition cursor-pointer"
        @click="addResume"
      >
        <i class="fas fa-plus text-xl mb-1"></i>
        <span class="text-sm">Создать новое резюме</span>
      </div>
    </div>

    <!-- Пока ничего нет -->
    <div v-else class="text-white/50 text-sm">
      Загрузка резюме или резюме отсутствуют.
    </div>
  </section>
</template>

<script setup>
import { onMounted, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useProfileStore } from '@/stores/profile'
import { useResumeStore } from '@/stores/resumes'

const profileStore = useProfileStore()
const resumeStore = useResumeStore()
const router = useRouter()

const userResumes = computed(() => {
  const profile = profileStore.profile
  if (!profile) return []
  return resumeStore.resumes.filter(r => profile.resumes.includes(r.id))
})

const editResume = resume => {
  resumeStore.setResumeForEdit(resume)
  router.push('/resume')
}

const addResume = () => {
  resumeStore.setResumeForEdit(null)
  router.push('/resume')
}

const setAsMain = async id => {
  await profileStore.setMainResume(id)
}

const deleteResume = async id => {
  await resumeStore.deleteResume(id)
}

onMounted(async () => {
  await Promise.all([
    profileStore.fetchProfile(),
    resumeStore.fetchResumes()
  ])
})

</script>

<style scoped></style>
