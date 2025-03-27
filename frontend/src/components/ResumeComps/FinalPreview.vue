<template>
  <div class="w-[210mm]">
    <div class="w-[210mm] min-h-[297mm] p-[20mm] mx-auto bg-white shadow-lg relative overflow-hidden" ref="resumeContent">
      <!-- Шапка резюме (всегда фиксирована вверху) -->
      <div class="mb-4">
        <h1 class="text-3xl font-bold text-gray-800 m-0 mb-1">{{ resumeData.name || profile?.name || 'Имя Фамилия' }}</h1>
        <p class="text-lg text-gray-600 m-0 mb-4">{{ resumeData.job || profile?.profession || 'Профессия' }}</p>
        
        <div class="flex flex-wrap gap-4">
          <div v-if="resumeData.email || profile?.email" class="flex items-center gap-2 text-sm text-gray-600">
            <i class="fas fa-envelope text-[var(--neon-purple)]"></i>
            <span>{{ resumeData.email || profile?.email }}</span>
          </div>
          <div v-if="resumeData.phone || profile?.phone" class="flex items-center gap-2 text-sm text-gray-600">
            <i class="fas fa-phone text-[var(--neon-purple)]"></i>
            <span>{{ resumeData.phone || profile?.phone }}</span>
          </div>
          <div v-if="resumeData.city || profile?.city" class="flex items-center gap-2 text-sm text-gray-600">
            <i class="fas fa-map-marker-alt text-[var(--neon-purple)]"></i>
            <span>{{ resumeData.city || profile?.city }}</span>
          </div>
        </div>
      </div>
      
      <div class="h-0.5 bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] my-4"></div>
      
      <!-- Перетаскиваемые секции -->
      <div class="flex flex-col gap-5">
        <template v-for="(section, index) in sections" :key="section.id">
          <div class="relative group resume-section" :data-section-id="section.id">
            <!-- Кнопка для перетаскивания -->
            <div class="drag-handle absolute -left-8 top-1 opacity-0 group-hover:opacity-100 cursor-move text-gray-400 hover:text-gray-600 transition-opacity"
                 @mousedown="startDrag(index)">
              <i class="fas fa-grip-vertical"></i>
            </div>
            
            <!-- Секция "О себе" -->
            <div v-if="section.id === 'about'" class="mb-5" v-show="resumeData.description">
              <h2 class="text-lg font-semibold text-[var(--neon-purple)] m-0 mb-4 pb-1 border-b border-gray-200">Обо мне</h2>
              <p class="text-sm text-gray-600 m-0 leading-relaxed">{{ resumeData.description }}</p>
            </div>
            
            <!-- Секция "Опыт работы" -->
            <div v-if="section.id === 'experience'" class="mb-5" v-show="resumeData.experience?.length">
              <h2 class="text-lg font-semibold text-[var(--neon-purple)] m-0 mb-4 pb-1 border-b border-gray-200">Опыт работы</h2>
              <div class="mb-4" v-for="(exp, i) in resumeData.experience" :key="i">
                <div class="flex justify-between mb-1">
                  <h3 class="text-base font-semibold text-gray-800 m-0">{{ exp.position }} | {{ exp.company }}</h3>
                  <span class="text-sm text-gray-500">{{ exp.startDate }} – {{ exp.endDate }}</span>
                </div>
                <p class="text-sm text-gray-600 m-0 mt-1 leading-relaxed">{{ exp.description }}</p>
              </div>
            </div>
            
            <!-- Секция "Образование" -->
            <div v-if="section.id === 'education'" class="mb-5" v-show="profile?.education?.length">
              <h2 class="text-lg font-semibold text-[var(--neon-purple)] m-0 mb-4 pb-1 border-b border-gray-200">Образование</h2>
              <div class="mb-2.5" v-for="(edu, i) in profile.education" :key="i">
                <h3 class="text-base font-semibold text-gray-800 m-0">{{ edu.institution }}</h3>
                <p class="text-sm text-gray-600 mt-0.5 m-0">{{ edu.degree }} ({{ edu.startYear }}–{{ edu.endYear }})</p>
              </div>
            </div>
            
            <!-- Секция "Навыки" -->
            <div v-if="section.id === 'skills'" class="mb-5" v-show="resumeData.skills?.length">
              <h2 class="text-lg font-semibold text-[var(--neon-purple)] m-0 mb-4 pb-1 border-b border-gray-200">Профессиональные навыки</h2>
              <div class="flex flex-wrap gap-2.5">
                <div 
                  v-for="(skill, index) in resumeData.skills" 
                  :key="skill + index"
                  class="px-3 py-1 border border-gray-200 rounded-full text-sm text-gray-600"
                >
                  {{ skill }}
                </div>
              </div>
            </div>
          </div>
        </template>
      </div>
      
    </div>
    
    <!-- Инструкции по перетаскиванию -->
    <div class="mt-4 text-center text-sm text-[var(--text-light)] opacity-80">
      <p>Наведите на секцию и перетащите <i class="fas fa-grip-vertical mx-1"></i> чтобы изменить порядок разделов</p>
    </div>
    
    <div class="flex justify-between mt-8">
      <button @click="handlePrevStep" class="btn btn-secondary">Назад</button>
      <button @click="handleNextStep" class="btn btn-primary">Далее</button>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onBeforeUnmount } from 'vue'
import { useProfileStore } from '@/stores/profileStore'
import { useResumeStore } from '@/stores/resumesStore'

const props = defineProps({
  resumeData: {
    type: Object,
    required: true
  }
})

const emit = defineEmits(['next-step', 'prev-step'])

const profileStore = useProfileStore()
const resumeStore = useResumeStore()
const profile = computed(() => profileStore.profile)
const resumeContent = ref(null)

// Определение секций с возможностью перемещения
const sections = ref([
  { id: 'about', title: 'Обо мне' },
  { id: 'experience', title: 'Опыт работы' },
  { id: 'education', title: 'Образование' },
  { id: 'skills', title: 'Профессиональные навыки' }
])

// Загрузка сохраненного порядка секций при монтировании
onMounted(() => {
  const savedOrder = props.resumeData.sectionsOrder
  if (savedOrder && Array.isArray(savedOrder) && savedOrder.length === sections.value.length) {
    // Создаем новый массив секций в сохраненном порядке
    const orderedSections = []
    savedOrder.forEach(id => {
      const section = sections.value.find(s => s.id === id)
      if (section) {
        orderedSections.push(section)
      }
    })
    
    // Проверяем, что все секции были найдены
    if (orderedSections.length === sections.value.length) {
      sections.value = orderedSections
    }
  }
  
  // Добавляем обработчики для перетаскивания
  document.addEventListener('mousemove', handleDrag)
  document.addEventListener('mouseup', endDrag)
})

onBeforeUnmount(() => {
  // Удаляем обработчики при размонтировании компонента
  document.removeEventListener('mousemove', handleDrag)
  document.removeEventListener('mouseup', endDrag)
})

// Переменные для отслеживания перетаскивания
const draggedIndex = ref(null)
const dragOverIndex = ref(null)
const isDragging = ref(false)

// Начало перетаскивания
const startDrag = (index) => {
  draggedIndex.value = index
  isDragging.value = true
  
  // Добавляем класс к перетаскиваемому элементу
  const sectionElements = document.querySelectorAll('.resume-section')
  if (sectionElements[index]) {
    sectionElements[index].classList.add('dragging')
  }
}

// Процесс перетаскивания
const handleDrag = (e) => {
  if (!isDragging.value) return
  
  e.preventDefault()
  
  const sectionElements = document.querySelectorAll('.resume-section')
  
  // Находим элемент, над которым находится курсор
  let targetIndex = null
  sectionElements.forEach((el, index) => {
    const rect = el.getBoundingClientRect()
    if (e.clientY >= rect.top && e.clientY <= rect.bottom) {
      targetIndex = index
    }
  })
  
  if (targetIndex !== null && targetIndex !== draggedIndex.value) {
    dragOverIndex.value = targetIndex
    
    // Визуально показываем, куда будет перемещен элемент
    sectionElements.forEach((el, index) => {
      el.classList.remove('drag-over')
      if (index === targetIndex) {
        el.classList.add('drag-over')
      }
    })
  }
}

// Завершение перетаскивания
const endDrag = () => {
  if (!isDragging.value) return
  
  // Если есть целевой индекс, перемещаем элемент
  if (dragOverIndex.value !== null && dragOverIndex.value !== draggedIndex.value) {
    const newSections = [...sections.value]
    const [movedItem] = newSections.splice(draggedIndex.value, 1)
    newSections.splice(dragOverIndex.value, 0, movedItem)
    sections.value = newSections
    
    // Сохраняем новый порядок
    saveSectionsOrder()
  }
  
  // Сбрасываем состояние
  const sectionElements = document.querySelectorAll('.resume-section')
  sectionElements.forEach(el => {
    el.classList.remove('dragging', 'drag-over')
  })
  
  isDragging.value = false
  draggedIndex.value = null
  dragOverIndex.value = null
}

// Сохранение порядка секций
const saveSectionsOrder = () => {
  const order = sections.value.map(section => section.id)
  const updatedResumeData = { ...props.resumeData, sectionsOrder: order }
  resumeStore.setResumeForEdit(updatedResumeData)
}

// Обработчики для кнопок навигации
const handleNextStep = () => {
  emit('next-step')
}

const handlePrevStep = () => {
  emit('prev-step')
}
</script>

<style scoped>
.btn {
  @apply px-6 py-3 font-semibold rounded-full transition-all duration-300 transform hover:scale-105 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-[var(--neon-purple)];
}
.btn-primary {
  @apply bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] text-[var(--text-light)];
}
.btn-secondary {
  @apply bg-[var(--background-section)] bg-opacity-50 text-[var(--text-light)];
}

/* Стили для перетаскивания */
.drag-handle {
  @apply w-6 h-6 flex items-center justify-center rounded-full hover:bg-gray-200;
}

.resume-section {
  transition: transform 0.2s, box-shadow 0.2s;
}

.resume-section.dragging {
  opacity: 0.7;
  transform: scale(0.98);
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  z-index: 10;
}

.resume-section.drag-over {
  border-top: 2px dashed var(--neon-purple);
  padding-top: 8px;
}
</style>