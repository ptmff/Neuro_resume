<template>
  <section class="flex flex-col gap-6">
    <!-- Блок заголовка -->
    <div>
      <h2 class="text-xl sm:text-2xl font-bold text-white">Мои резюме</h2>
      <p class="text-sm text-white/60">Управляй всеми своими резюме в одном месте</p>
    </div>

    <!-- Список резюме -->
    <div class="flex flex-col gap-4">
      <div
        v-for="resume in store.allResumes.filter(r => store.profile?.resumes?.includes(r.id))"
        :key="resume.id"
        class="relative group bg-gradient-to-r from-violet-800/30 to-indigo-800/30 border border-white/10 rounded-xl px-4 py-3 flex items-center justify-between hover:shadow-lg transition-shadow"
      >
        <div>
          <h3 class="text-white font-semibold text-base sm:text-lg">
            {{ resume.title }}
            <span v-if="resume.id === store.profile.mainResumeId" class="ml-2 text-violet-400"
              >★</span
            >
          </h3>
          <p class="text-xs text-white/50 mt-1">{{ resume.date }}</p>
        </div>

        <!-- Действия -->
        <div class="flex items-center gap-2">
          <button
            @click="store.setMainResume(resume.id)"
            class="p-2 rounded-full bg-white/10 hover:bg-white/20 transition"
            title="Сделать основным"
          >
            <i class="fas fa-star text-white text-sm"></i>
          </button>
          <button
            @click="editResume(resume)"
            class="p-2 rounded-full bg-white/10 hover:bg-white/20 transition"
          >
            <i class="fas fa-edit text-white text-sm"></i>
          </button>
          <button
            @click="store.deleteResume(resume.id)"
            class="p-2 rounded-full bg-white/10 hover:bg-white/20 transition"
          >
            <i class="fas fa-trash text-pink-400 text-sm"></i>
          </button>
        </div>
      </div>

      <!-- Добавить новое резюме -->
      <div
        class="bg-gradient-to-r from-violet-800/20 to-indigo-800/20 border border-dashed border-white/10 rounded-xl px-4 py-6 flex flex-col items-center justify-center text-white hover:bg-white/5 transition cursor-pointer"
        @click="addResume"
      >
        <i class="fas fa-plus text-xl mb-2"></i>
        <span class="text-sm">Создать новое резюме</span>
      </div>
    </div>
  </section>
</template>

<script setup>
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useProfileStore } from '@/stores/profile'

const store = useProfileStore()
const router = useRouter()

const editResume = resume => {
  store.setResumeForEdit(resume)
  router.push('/resume')
}

const addResume = () => {
  store.setResumeForEdit(null)
  router.push('/resume')
}

onMounted(() => {
  if (!store.profile) {
    store.fetchProfile()
  }
})
</script>

<style scoped></style>
