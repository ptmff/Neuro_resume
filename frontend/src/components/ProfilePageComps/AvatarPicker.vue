<script setup lang="ts">
import { ref, computed, watch, nextTick } from 'vue'
import { useProfileStore } from '@/stores/profileStore'

const emit = defineEmits(['close'])

const profileStore = useProfileStore()
const current = computed(() => profileStore.profile?.photo || '')
const selected = ref(current.value)

const defaultAvatar = '/placeholder.png'
const presetAvatars = [
  '/avatars/avatar1.png',
  '/avatars/avatar2.png',
  '/avatars/avatar3.png',
  '/avatars/avatar4.png',
  '/avatars/avatar5.png',
]

watch(() => profileStore.profile?.photo, newVal => {
  selected.value = newVal || defaultAvatar
})

const selectAvatar = (url: string) => {
  selected.value = url
}

const resetAvatar = () => {
  selected.value = defaultAvatar
}

const uploadCustomAvatar = async (event: Event) => {
  const file = (event.target as HTMLInputElement).files?.[0]
  if (file) {
    await profileStore.uploadPhoto(file)
    await profileStore.fetchProfile()
    emit('close')
  }
}

const saveAvatar = async () => {
  if (!selected.value) return

  // Если выбран один из пресетов
  if (selected.value.startsWith('/avatars/')) {
    const response = await fetch(selected.value)
    const blob = await response.blob()
    const file = new File([blob], 'avatar.png', { type: blob.type })

    await profileStore.uploadPhoto(file)
  }

  // Закрыть окно до загрузки
  emit('close')

  // Обновить профиль
  await profileStore.fetchProfile()
}


</script>

<template>
    <!-- Задний слой с блюром -->
    <div class="fixed inset-0 z-50 flex items-center justify-center backdrop-blur-sm bg-black/30">
      <!-- Модальное окно -->
      <div
        class="relative flex flex-col md:flex-row gap-8 bg-[var(--background-main)] p-8 rounded-3xl w-[95vw] max-w-5xl shadow-2xl border border-[var(--background-cta)] transition-all duration-300"
      >
        <!-- ❌ Кнопка закрытия -->
        <button
          class="absolute top-4 right-4 text-[var(--text-subdued)] hover:text-[var(--text-light)] transition duration-200"
          @click="$emit('close')"
        >
          <svg xmlns="http://www.w3.org/2000/svg" class="w-7 h-7" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
  
        <!-- Левая часть — аватары -->
        <div class="w-full md:w-1/2">
          <h2 class="text-2xl text-[var(--text-light)] font-bold mb-6">Выберите аватар</h2>
          <div class="grid grid-cols-3 gap-5">
            <img
              v-for="(avatar, index) in presetAvatars"
              :key="index"
              :src="avatar"
              :class="[
                'w-24 h-24 rounded-full cursor-pointer border-4 object-cover transition-all duration-200 transform',
                selected === avatar ? 'border-[var(--accent)] scale-110 shadow-lg' : 'border-transparent opacity-80 hover:opacity-100 hover:scale-105'
              ]"
              @click="selectAvatar(avatar)"
            />
          </div>
        </div>
  
        <!-- Правая часть — действия -->
        <div class="w-full md:w-1/2 flex flex-col justify-center gap-4">
          <button
            class="w-full bg-[var(--background-cta)] text-white py-3 rounded-xl transition-all duration-200 hover:brightness-110 active:scale-95"
            @click="$refs.fileInput.click()"
          >
            Загрузить свой
          </button>
          <input
            type="file"
            accept="image/*"
            class="hidden"
            ref="fileInput"
            @change="uploadCustomAvatar"
          />
  
          <button
            class="w-full bg-[var(--resume-red)] text-white py-3 rounded-xl transition-all duration-200 hover:brightness-110 active:scale-95"
            @click="resetAvatar"
          >
            Сбросить на дефолтный
          </button>
  
          <button
            class="w-full bg-[var(--accent)] text-white py-3 rounded-xl transition-all duration-200 hover:brightness-110 active:scale-95"
            @click="saveAvatar"
          >
            Сохранить
          </button>
        </div>
      </div>
    </div>
  </template>
  