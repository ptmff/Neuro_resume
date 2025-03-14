<script setup>
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useProfileStore } from '@/stores/profile'
import api from '@/api' // ✅ заменили axios

const store = useProfileStore()
const router = useRouter()

const addResume = () => {
  router.push('/resume')
}

const editResume = id => {
  router.push({ path: '/resume', query: { id } })
}

const deleteResume = async id => {
  // Удаляем из стейта
  store.allResumes = store.allResumes.filter(r => r.id !== id)
  store.profile.resumes = store.profile.resumes.filter(rid => rid !== id)

  try {
    // Обновляем profile.json
    await api.patch('/profile', {
      resumes: store.profile.resumes,
    })

    // Обновляем resumes.json
    await api.patch('/resumes', {
      resumes: store.allResumes,
    })
  } catch (err) {
    console.error('Ошибка при сохранении:', err)
  }
}

onMounted(() => {
  if (!store.profile) {
    store.fetchProfile()
  }
})
</script>

<template>
  <div
    v-if="store.profile"
    class="bg-[var(--background-section)] bg-opacity-30 backdrop-blur-xl p-8 rounded-3xl border border-white/10"
  >
    <h2
      class="text-4xl font-bold bg-gradient-to-r from-[var(--text-secondary)] to-[var(--text-light)] bg-clip-text text-transparent mb-8"
    >
      Ваши резюме
    </h2>

    <div class="grid grid-cols-1 gap-6">
      <div
        v-for="resume in store.allResumes.filter(r => store.profile.resumes.includes(r.id))"
        :key="resume.id"
        class="resume-card group relative overflow-hidden"
      >
        <div
          class="absolute inset-0 bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] opacity-20 group-hover:opacity-30 transition-all duration-300"
        ></div>
        <div class="relative p-6 flex items-center justify-between">
          <div>
            <h3 class="text-xl font-bold text-[var(--text-light)]">{{ resume.title }}</h3>
            <p class="text-sm text-[var(--text-secondary)]">{{ resume.date }}</p>
          </div>
          <div class="flex gap-3 items-center">
            <button
              @click="store.setMainResume(resume.id)"
              :class="
                resume.id === store.profile.mainResumeId
                  ? 'text-[var(--background-cta)]'
                  : 'text-gray-400'
              "
              class="transition-colors duration-200"
              title="Сделать основным"
            >
              <i class="fas fa-star"></i>
            </button>
            <button
              @click="editResume(resume.id)"
              class="action-btn bg-[var(--background-section)] bg-opacity-50 hover:bg-opacity-100"
            >
              <i class="fas fa-edit"></i>
            </button>
            <button
              @click="deleteResume(resume.id)"
              class="action-btn bg-opacity-80 hover:bg-opacity-30 bg-[var(--background-cta)]"
            >
              <i class="fas fa-trash-alt"></i>
            </button>
          </div>
        </div>
      </div>

      <div class="resume-card group cursor-pointer" @click="addResume">
        <div
          class="absolute inset-0 bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] opacity-10 group-hover:opacity-20 transition-all duration-300"
        ></div>
        <div class="relative p-6 flex items-center justify-center">
          <div class="text-center">
            <i
              class="fas fa-plus text-4xl text-[var(--text-light)] mb-2 group-hover:scale-110 transition-transform duration-300"
            ></i>
            <p class="text-[var(--text-light)]">Создать новое резюме</p>
          </div>
        </div>
      </div>
    </div>
  </div>

  <div v-else class="text-center text-[var(--text-secondary)] py-8 text-lg">Загрузка данных...</div>
</template>

<style scoped>
.resume-card {
  @apply relative rounded-2xl transition-all duration-300 border border-white/10;
}

.resume-card:hover {
  @apply transform scale-[1.02];
}

.action-btn {
  @apply p-2 rounded-lg text-[var(--text-light)] transition-all duration-300;
}
</style>
