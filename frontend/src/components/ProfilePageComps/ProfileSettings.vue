<template>
    <transition name="slide">
      <div v-if="visible" class="fixed inset-0 z-50 bg-[var(--background-main)] flex text-[var(--text-light)]">
        <!-- Левая колонка: Меню -->
        <aside class="w-60 min-w-[200px] border-r border-[var(--background-overlay)] p-6 flex flex-col justify-between">
          <div class="space-y-4">
            <div class='flex items-center justify-between py-3'>
                <h2 class="text-2xl font-bold text-[var(--text-light)]">Настройки</h2>
                <i class="fas fa-cog text-2xl text-[var(--text-light)] pt-1"></i>
            </div>
            <button
              v-for="item in menu"
              :key="item.id"
              @click="active = item.id"
              :class="[
                'w-full text-left px-3 py-2 rounded-lg transition-all',
                active === item.id
                  ? 'menu-btn'
                  : 'menu-btn:hover hover:bg-[var(--background-overlay)]'
              ]"
            >
              {{ item.label }}
            </button>
          </div>
  
          <button
            @click="logout"
            class="delete-btn"
          >
            Выход
          </button>
        </aside>
  
        <!-- Правая колонка: Контент -->
        <main class="flex-1 p-8 overflow-y-auto">
          <div class="flex justify-between items-center mb-6">
            <h2 class="text-2xl font-bold">{{ currentSectionTitle }}</h2>
            <button @click="close" class="text-[var(--text-subdued)] hover:text-white transition">
              <i class="fas fa-times text-xl"></i>
            </button>
          </div>
  
          <div>
            <component :is="activeComponent" />
          </div>
        </main>
      </div>
    </transition>
  </template>
  
  <script setup lang="ts">
  import { ref, computed } from 'vue'
  import { useRouter } from 'vue-router'
  import { useAuthStore } from '@/stores/authStore'
  import EditProfile from './Settings/EditProfile.vue'
  import Verification from './Settings/Verification.vue'
  import Payments from './Settings/Payments.vue'
  
  const props = defineProps<{ visible: boolean }>()
  const emit = defineEmits(['close'])
  
  const active = ref<'edit' | 'verify' | 'pay'>('edit')
  
  const menu: { id: 'edit' | 'verify' | 'pay'; label: string }[] = [
  { id: 'edit', label: 'Редактирование данных' },
  { id: 'verify', label: 'Верификация' },
  { id: 'pay', label: 'Оплата' },
]

  
  const currentSectionTitle = computed(() => menu.find(m => m.id === active.value)?.label || '')
  
  const activeComponent = computed(() => {
    switch (active.value) {
      case 'edit': return EditProfile
      case 'verify': return Verification
      case 'pay': return Payments
    }
  })
  
  const authStore = useAuthStore()
  const router = useRouter()
  const logout = () => {
  authStore.logout()
  router.push('/login')
}
  
  const close = () => emit('close')
  </script>
  
  <style scoped>
  .slide-enter-active,
  .slide-leave-active {
    transition: transform 0.4s ease;
  }
  .slide-enter-from {
    transform: translateX(100%);
  }
  .slide-leave-to {
    transform: translateX(100%);
  }
  .menu-btn {
  background: linear-gradient(135deg, #9d00ff, #00e5ff);
  background-size: 200%;
  background-position: left center;
  transition: all 0.4s ease-in-out;
  filter: brightness(1);
}

.menu-btn:hover {
  background-position: center;
  filter: brightness(1.1);
}
.delete-btn {
  @apply mt-6 text-left px-3 py-2 rounded-lg;
  background: var(--gradient-danger);
  background-size: 200%;
  background-position: left center;
  transition: all 0.4s ease-in-out;
  filter: brightness(1);
}

.delete-btn:hover {
  background-position: center;
  filter: brightness(1.1);
}
  </style>
  