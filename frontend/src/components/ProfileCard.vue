<script setup>
import { ref, onMounted } from 'vue'
import { useProfileStore } from '@/stores/profile'

const store = useProfileStore()
const isEditing = ref(false)

const editedProfile = ref({ name: '', email: '' })

const labels = {
  name: 'Имя',
  email: 'Email',
}

const startEdit = () => {
  editedProfile.value = {
    name: store.profile.name,
    email: store.profile.email,
  }
  isEditing.value = true
}

const saveProfile = async () => {
  await store.updateProfile(editedProfile.value)
  isEditing.value = false
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
    class="profile-card bg-[var(--background-section)] bg-opacity-30 backdrop-blur-xl p-8 rounded-3xl border border-white/10"
  >
    <div class="flex items-center justify-between mb-8">
      <h2
        class="text-4xl font-bold bg-gradient-to-r from-[var(--text-secondary)] to-[var(--text-light)] bg-clip-text text-transparent"
      >
        Личные данные
      </h2>
      <button
        class="px-6 py-3 text-sm font-semibold rounded-full transition-all duration-300 transform hover:scale-105"
        :class="
          isEditing
            ? 'bg-green-500 text-white'
            : 'bg-[var(--background-cta)] text-[var(--text-main)] hover:bg-transparent hover:border hover:border-[var(--background-cta)]'
        "
        @click="isEditing ? saveProfile() : startEdit()"
      >
        {{ isEditing ? 'Сохранить' : 'Редактировать' }}
      </button>
    </div>

    <div class="space-y-6">
      <div v-for="key in ['name', 'email']" :key="key" class="field group">
        <label class="text-[var(--text-secondary)] text-sm font-medium mb-2 block">{{
          labels[key]
        }}</label>
        <input
          v-if="isEditing"
          v-model="editedProfile[key]"
          class="w-full bg-[var(--background-section)] bg-opacity-50 border border-white/10 rounded-xl px-4 py-3 text-[var(--text-light)] focus:outline-none focus:border-[var(--text-secondary)] transition-all duration-300"
        />
        <p
          v-else
          class="text-[var(--text-light)] text-lg px-4 py-3 bg-[var(--background-section)] bg-opacity-20 rounded-xl group-hover:bg-opacity-30 transition-all duration-300"
        >
          {{ store.profile[key] }}
        </p>
      </div>
    </div>

    <div v-if="store.mainResume?.skills?.length" class="mt-8">
      <label class="text-[var(--text-secondary)] text-sm font-medium mb-4 block">Навыки</label>
      <div class="flex flex-wrap gap-3">
        <span
          v-for="(skill, index) in store.mainResume.skills"
          :key="index"
          class="px-4 py-2 rounded-full text-sm font-medium bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] text-white transform hover:scale-105 transition-all duration-300 cursor-default"
        >
          {{ skill }}
        </span>
      </div>
    </div>

    <div class="mt-4">
      <p class="text-sm text-[var(--text-secondary)]">
        Профессия: <strong>{{ store.mainResume.job }}</strong>
      </p>
    </div>
  </div>

  <div v-else class="text-[var(--text-secondary)] text-lg text-center py-12">
    Загрузка профиля...
  </div>
</template>
