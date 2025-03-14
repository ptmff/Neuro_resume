<template>
  <section class="flex flex-col md:flex-row items-center md:items-start gap-6 md:gap-10">
    <!-- Аватар -->
    <div
      class="relative w-28 h-28 md:w-32 md:h-32 rounded-full overflow-hidden border-2 border-violet-500/50 shadow-md"
    >
      <img
        src="https://api.dicebear.com/7.x/shapes/svg?seed=alex"
        alt="Avatar"
        class="w-full h-full object-cover"
      />
      <!-- Светящийся ореол -->
      <div
        class="absolute inset-0 rounded-full bg-violet-500 opacity-20 blur-2xl animate-pulse"
      ></div>
    </div>

    <!-- Приветствие и инфо -->
    <div class="text-center md:text-left w-full max-w-xl">
      <h1 class="text-2xl sm:text-3xl font-bold text-white mb-2">
        {{
          store.isEditing ? 'Редактирование профиля' : `Привет, ${store.profile?.name || 'Гость'}!`
        }}
      </h1>

      <!-- Email и Имя -->
      <div class="space-y-4 mb-4">
        <div v-for="key in ['name', 'email']" :key="key">
          <label class="text-white/60 text-sm">{{ labels[key] }}</label>
          <div v-if="store.isEditing">
            <input
              v-model="store.editedProfile[key]"
              class="w-full bg-white/10 border border-white/10 rounded-xl px-4 py-2 text-white focus:outline-none focus:border-violet-500"
            />
          </div>
          <p v-else class="text-white text-base">{{ store.profile?.[key] || '-' }}</p>
        </div>
      </div>

      <!-- Профессия -->
      <div v-if="store.mainResume?.job" class="mb-4">
        <p class="text-sm text-white/70">
          Профессия: <strong>{{ store.mainResume.job }}</strong>
        </p>
      </div>

      <!-- Кнопка редактирования -->
      <button
        class="px-5 py-2 text-sm font-semibold rounded-full transition-all duration-300"
        :class="
          store.isEditing
            ? 'bg-green-500 text-white'
            : 'bg-violet-600 text-white hover:bg-violet-700'
        "
        @click="store.isEditing ? store.saveProfile() : store.startEdit()"
      >
        {{ store.isEditing ? 'Сохранить' : 'Редактировать' }}
      </button>
    </div>
  </section>
</template>

<script setup>
import { onMounted } from 'vue'
import { useProfileStore } from '@/stores/profile'

const store = useProfileStore()

const labels = {
  name: 'Имя',
  email: 'Email',
}

onMounted(() => {
  if (!store.profile) {
    store.fetchProfile()
  }
})
</script>

<style scoped></style>
