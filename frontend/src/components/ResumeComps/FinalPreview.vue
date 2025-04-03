<template>
  <div class="w-[210mm]" :class="{ 'dragging-active': isDragging }">
    <h2 class="text-4xl font-bold bg-gradient-to-r from-[var(--text-secondary)] to-[var(--text-light)] bg-clip-text text-transparent mb-8">
      Ваше резюме выглядит так
    </h2>
    
    <div 
      class="w-[210mm] min-h-[296mm] p-[20mm] mx-auto bg-white shadow-lg relative overflow-hidden" 
      ref="resumeContent" 
      id="resume-content"
      :class="[`template-${resumeData.template || 'классический'}`]"
    >
      <!-- Шапка резюме (всегда фиксирована вверху) -->
      <div class="mb-4" :class="{ 'text-center': isCreativeOrModern, 'border-bottom-gradient': isModern }">
        <h1 class="text-3xl font-bold m-0 mb-1" :class="getHeaderClass()">{{ resumeData.name || profile?.name || 'Имя Фамилия' }}</h1>
        <p class="text-lg m-0 mb-4" :class="getSubheaderClass()">{{ resumeData.job || profile?.profession || 'Профессия' }}</p>
        
        <div class="flex flex-wrap gap-4" :class="{ 'justify-center': isCreativeOrModern }">
          <div v-if="resumeData.email || profile?.email" class="flex items-center gap-2 text-sm" :class="getContactClass()">
            <i :class="getIconClass('envelope')"></i>
            <span>{{ resumeData.email || profile?.email }}</span>
          </div>
          <div v-if="resumeData.phone || profile?.phone" class="flex items-center gap-2 text-sm" :class="getContactClass()">
            <i :class="getIconClass('phone')"></i>
            <span>{{ resumeData.phone || profile?.phone }}</span>
          </div>
          <div v-if="resumeData.city || profile?.city" class="flex items-center gap-2 text-sm" :class="getContactClass()">
            <i :class="getIconClass('map-marker-alt')"></i>
            <span>{{ resumeData.city || profile?.city }}</span>
          </div>
        </div>
      </div>
      
      <div :class="getSeparatorClass()"></div>
      
      <!-- Перетаскиваемые секции -->
      <div class="flex flex-col gap-5">
        <template v-for="(section, index) in sections" :key="section.id">
          <div class="relative group resume-section" 
               :data-section-id="section.id"
               :class="{ 
                 [`dragging-${resumeData.template}`]: draggedIndex === index, 
                 [`drag-over-${resumeData.template}`]: dragOverIndex === index && draggedIndex !== index,
                 [`drag-after-${resumeData.template}`]: dragOverIndex === index - 1 && draggedIndex !== index - 1 && draggedIndex !== null && draggedIndex > index
               }">
            <!-- Кнопка для перетаскивания -->
            <div class="drag-handle absolute -left-8 top-1 opacity-0 group-hover:opacity-100 cursor-move text-gray-400 hover:text-gray-600 transition-opacity"
                 :class="getDragHandleClass()"
                 @mousedown="startDrag($event, index)">
              <i class="fas fa-grip-vertical"></i>
            </div>
            
            <!-- Секция "О себе" -->
            <div v-if="section.id === 'about'" class="mb-5" v-show="resumeData.description">
              <h2 :class="getSectionHeaderClass()">Обо мне</h2>
              <p :class="getTextClass()">{{ resumeData.description }}</p>
            </div>
            
            <!-- Секция "Опыт работы" -->
            <div v-if="section.id === 'experience'" class="mb-5" v-show="resumeData.experience?.length">
              <h2 :class="getSectionHeaderClass()">Опыт работы</h2>
              <div class="mb-4" v-for="(exp, i) in resumeData.experience" :key="i">
                <div :class="{ 'flex justify-between mb-1': !isCreative, 'mb-2': isCreative }">
                  <h3 :class="getItemHeaderClass()">
                    {{ exp.company }} : {{ exp.position}}
                  </h3>
                  <span :class="getDateClass()">{{ exp.startDate }} – {{ exp.endDate }}</span>
                </div>
                <p :class="getTextClass()">{{ exp.description }}</p>
              </div>
            </div>
            
            <!-- Секция "Образование" -->
            <div v-if="section.id === 'education'" class="mb-5" v-show="profile?.education?.length">
              <h2 :class="getSectionHeaderClass()">Образование</h2>
              <div class="mb-2.5" v-for="(edu, i) in profile?.education || []" :key="i">
                <h3 :class="getItemHeaderClass()">{{ edu.institution }}</h3>
                <p :class="getTextClass()">{{ edu.degree }} ({{ edu.startYear }}–{{ edu.endYear }})</p>
              </div>
            </div>
            
            <!-- Секция "Навыки" -->
            <div v-if="section.id === 'skills'" class="mb-5" v-show="resumeData.skills?.length">
              <h2 :class="getSectionHeaderClass()">Профессиональные навыки</h2>
              <div class="flex flex-wrap gap-2.5">
                <div 
                  v-for="(skill, index) in resumeData.skills" 
                  :key="skill + index"
                  :class="getSkillClass()"
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
      <button @click="generatePDF" class="btn-download">
        <i class="fas fa-file-pdf mr-2"></i> Скачать PDF
      </button>
    </div>
    
    <div class="flex justify-between mt-8">
      <button @click="handlePrevStep" class="btn btn-secondary">Назад</button>
      <button @click="handleNextStep" class="btn btn-primary">Далее</button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onBeforeUnmount } from 'vue'
import { useProfileStore } from '@/stores/profileStore'
import { useResumeStore } from '@/stores/resumesStore'
import html2pdf from 'html2pdf.js'

interface Section {
  id: string;
  title: string;
}

interface StoreEducation {
  institution: string;
  degree: string;
  field: string;
  startYear: number;
  endYear: number;
}

interface Experience {
  id?: string;
  company: string;
  position: string;
  startDate: string;
  endDate: string;
  description: string;
}

interface ResumeData {
  id?: number;
  title?: string;
  name?: string;
  email?: string;
  phone?: string;
  city?: string;
  job?: string;
  description?: string;
  experience?: Experience[];
  skills?: string[];
  template?: string;
  date?: string;
  sectionsOrder?: string[];
  [key: string]: any;
}

interface StoreProfile {
  email: string;
  name?: string;
  phone?: string;
  city?: string;
  photo?: string;
  education?: StoreEducation[];
  mainResumeId?: number | null;
  resumes?: number[];
  profession?: string;
}

interface ResumePreviewProps {
  resumeData: ResumeData;
}

const props = defineProps<ResumePreviewProps>()
const emit = defineEmits<{
  (e: 'next-step'): void;
  (e: 'prev-step'): void;
  (e: 'update:sections-order', order: string[]): void 
}>()

const profileStore = useProfileStore()
const resumeStore = useResumeStore()
const profile = computed<StoreProfile | null>(() => profileStore.profile)
const resumeContent = ref<HTMLElement | null>(null)

const sections = ref<Section[]>([
  { id: 'about', title: 'Обо мне' },
  { id: 'experience', title: 'Опыт работы' },
  { id: 'education', title: 'Образование' },
  { id: 'skills', title: 'Профессиональные навыки' }
])

const draggedIndex = ref<number | null>(null)
const dragOverIndex = ref<number | null>(null)
const isDragging = ref(false)
const dragStartY = ref(0)
const dragCurrentY = ref(0)



// Вычисляемые свойства для определения текущего шаблона
const currentTemplate = computed(() => {
  return (props.resumeData.template || 'классический').toLowerCase()
})

const isClassic = computed(() => currentTemplate.value === 'классический')
const isModern = computed(() => currentTemplate.value === 'современный')
const isCreative = computed(() => currentTemplate.value === 'креативный')
const isMinimalist = computed(() => currentTemplate.value === 'минималистичный')
const isProfessional = computed(() => currentTemplate.value === 'профессиональный')
const isTechnical = computed(() => currentTemplate.value === 'технический')

const isCreativeOrModern = computed(() => isCreative.value || isModern.value)

// Методы для стилизации в зависимости от шаблона
const getHeaderClass = () => {
  if (isClassic.value) return 'text-2xl font-bold text-black mb-1 font-serif'
  if (isModern.value) return 'text-gray-800'
  if (isCreative.value) return 'text-blue-700 font-bold'
  if (isMinimalist.value) return 'text-gray-900 font-light'
  if (isProfessional.value) return 'text-gray-800 font-semibold border-b-2 border-gray-300 pb-2'
  if (isTechnical.value) return 'text-gray-800 font-mono'
  return 'text-gray-800'
}

const getSubheaderClass = () => {
  if (isClassic.value) return 'text-base text-gray-700 font-serif'
  if (isModern.value) return 'text-gray-600'
  if (isCreative.value) return 'text-blue-500'
  if (isMinimalist.value) return 'text-gray-600 font-light'
  if (isProfessional.value) return 'text-gray-600 uppercase tracking-wider text-sm'
  if (isTechnical.value) return 'text-gray-600 font-mono'
  return 'text-gray-600'
}

const getContactClass = () => {
  if (isClassic.value) return 'text-black font-serif'
  if (isModern.value) return 'text-gray-600'
  if (isCreative.value) return 'text-blue-600'
  if (isMinimalist.value) return 'text-gray-500'
  if (isProfessional.value) return 'text-gray-700'
  if (isTechnical.value) return 'text-gray-600 font-mono'
  return 'text-gray-600'
}

const getIconClass = (icon: string) => {
  const baseClass = `fas fa-${icon}`
  if (isClassic.value) return `${baseClass} text-black`
  if (isModern.value) return `${baseClass} text-[var(--neon-purple)]`
  if (isCreative.value) return `${baseClass} text-blue-500`
  if (isMinimalist.value) return `${baseClass} text-gray-400`
  if (isProfessional.value) return `${baseClass} text-gray-500`
  if (isTechnical.value) return `${baseClass} text-gray-600`
  return `${baseClass} text-[var(--neon-purple)]`
}

const getSeparatorClass = () => {
  if (isClassic.value) return 'h-px bg-gray-300 my-4'
  if (isModern.value) return 'h-0.5 bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] my-4'
  if (isCreative.value) return 'h-1 bg-blue-500 my-4 w-1/3 mx-auto'
  if (isMinimalist.value) return 'h-px bg-gray-200 my-4'
  if (isProfessional.value) return 'h-0.5 bg-gray-300 my-4'
  if (isTechnical.value) return 'h-0.5 bg-gray-400 my-4 border-dashed'
  return 'h-0.5 bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] my-4'
}

const getSectionHeaderClass = () => {
  if (isClassic.value) return 'text-base font-bold text-black m-0 mb-3 pb-1 border-b border-gray-300 font-serif'
  if (isModern.value) return 'text-lg font-semibold text-[var(--neon-purple)] m-0 mb-4 pb-1 border-b border-gray-200'
  if (isCreative.value) return 'text-lg font-bold text-blue-600 m-0 mb-4 pb-1 border-b-2 border-blue-200'
  if (isMinimalist.value) return 'text-lg font-normal text-gray-800 m-0 mb-4 pb-1'
  if (isProfessional.value) return 'text-lg font-semibold text-gray-700 m-0 mb-4 pb-1 uppercase tracking-wider'
  if (isTechnical.value) return 'text-lg font-mono font-semibold text-gray-800 m-0 mb-4 pb-1 border-b border-gray-300'
  return 'text-lg font-semibold text-[var(--neon-purple)] m-0 mb-4 pb-1 border-b border-gray-200'
}

const getItemHeaderClass = () => {
  if (isClassic.value) return 'text-base font-bold text-black m-0 font-serif'
  if (isModern.value) return 'text-base font-semibold text-gray-800 m-0'
  if (isCreative.value) return 'text-base font-semibold text-blue-700 m-0'
  if (isMinimalist.value) return 'text-base font-medium text-gray-800 m-0'
  if (isProfessional.value) return 'text-base font-semibold text-gray-800 m-0'
  if (isTechnical.value) return 'text-base font-mono font-semibold text-gray-800 m-0'
  return 'text-base font-semibold text-gray-800 m-0'
}

const getDateClass = () => {
  if (isClassic.value) return 'text-sm text-gray-600 italic font-serif'
  if (isModern.value) return 'text-sm text-gray-500'
  if (isCreative.value) return 'text-sm text-blue-500'
  if (isMinimalist.value) return 'text-sm text-gray-400'
  if (isProfessional.value) return 'text-sm text-gray-600 font-medium'
  if (isTechnical.value) return 'text-sm text-gray-500 font-mono'
  return 'text-sm text-gray-500'
}

const getTextClass = () => {
  if (isClassic.value) return 'text-sm text-black m-0 leading-relaxed font-serif'
  if (isModern.value) return 'text-sm text-gray-600 m-0 leading-relaxed'
  if (isCreative.value) return 'text-sm text-gray-600 m-0 leading-relaxed'
  if (isMinimalist.value) return 'text-sm text-gray-600 m-0 leading-relaxed'
  if (isProfessional.value) return 'text-sm text-gray-700 m-0 leading-relaxed'
  if (isTechnical.value) return 'text-sm text-gray-600 m-0 leading-relaxed font-mono'
  return 'text-sm text-gray-600 m-0 leading-relaxed'
}

const getSkillClass = () => {
  if (isClassic.value) return 'px-2 py-1 text-sm text-black bg-gray-100 border border-gray-300 rounded'
  if (isModern.value) return 'px-3 py-1 text-sm text-gray-600'
  if (isCreative.value) return 'px-3 py-1 text-sm text-white bg-blue-500 rounded-full'
  if (isMinimalist.value) return 'px-3 py-1 text-sm text-gray-600 border border-gray-200 rounded'
  if (isProfessional.value) return 'px-3 py-1 text-sm text-gray-700 bg-gray-100 rounded'
  if (isTechnical.value) return 'px-3 py-1 text-sm text-gray-100 bg-gray-700 font-mono rounded'
  return 'px-3 py-1 text-sm text-gray-600'
}

// Новый метод для стилизации ручки перетаскивания
const getDragHandleClass = () => {
  if (isClassic.value) return 'hover:bg-gray-100'
  if (isModern.value) return 'hover:bg-purple-100'
  if (isCreative.value) return 'hover:bg-blue-100'
  if (isMinimalist.value) return 'hover:bg-gray-50'
  if (isProfessional.value) return 'hover:bg-gray-200'
  if (isTechnical.value) return 'hover:bg-gray-700 hover:text-white'
  return 'hover:bg-gray-200'
}

const generatePDF = (): void => {
  const element = document.getElementById('resume-content')
  if (element) {
    html2pdf().from(element).save('myDocument.pdf')
  }
}

onMounted(() => {
  const savedOrder = props.resumeData.sectionsOrder
  if (savedOrder && Array.isArray(savedOrder)) {
    const orderedSections: Section[] = []
    
    savedOrder.forEach(id => {
      const section = sections.value.find(s => s.id === id)
      if (section) {
        orderedSections.push(section)
      }
    })
    
    // Добавляем отсутствующие секции (если есть)
    sections.value.forEach(section => {
      if (!orderedSections.some(s => s.id === section.id)) {
        orderedSections.push(section)
      }
    })
    
    sections.value = orderedSections
  }
  
  document.addEventListener('mousemove', handleDrag)
  document.addEventListener('mouseup', endDrag)
})

onBeforeUnmount(() => {
  document.removeEventListener('mousemove', handleDrag)
  document.removeEventListener('mouseup', endDrag)
})

const startDrag = (event: MouseEvent, index: number): void => {
  event.preventDefault()
  
  draggedIndex.value = index
  isDragging.value = true
  dragStartY.value = event.clientY
  dragCurrentY.value = event.clientY
  
  document.body.classList.add('dragging-cursor')
}

const handleDrag = (e: MouseEvent): void => {
  if (!isDragging.value || draggedIndex.value === null) return
  
  e.preventDefault()
  dragCurrentY.value = e.clientY
  
  const sectionElements = document.querySelectorAll('.resume-section')
  
  let targetIndex: number | null = null
  sectionElements.forEach((el, index) => {
    const rect = el.getBoundingClientRect()
    const middleY = rect.top + rect.height / 2
    
    if (e.clientY >= rect.top && e.clientY <= rect.bottom) {
      if (e.clientY < middleY) {
        targetIndex = index
      } else {
        targetIndex = index
      }
    }
  })
  
  if (targetIndex !== null) {
    dragOverIndex.value = targetIndex
  }
}

const endDrag = (): void => {
  if (!isDragging.value || draggedIndex.value === null) return
  
  if (dragOverIndex.value !== null && dragOverIndex.value !== draggedIndex.value) {
    const newSections = [...sections.value]
    const [movedItem] = newSections.splice(draggedIndex.value, 1)
    newSections.splice(dragOverIndex.value, 0, movedItem)
    sections.value = newSections
    emit('update:sections-order', newSections.map(s => s.id))
  }
  
  isDragging.value = false
  draggedIndex.value = null
  dragOverIndex.value = null
  document.body.classList.remove('dragging-cursor')
}

const handleNextStep = (): void => {
  emit('next-step')
}

const handlePrevStep = (): void => {
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

.drag-handle {
  @apply w-6 h-6 flex items-center justify-center rounded-full hover:bg-gray-200;
}

.dragging-active {
  user-select: none;
}

:global(.dragging-cursor) {
  cursor: grabbing !important;
}

.resume-section {
  position: relative;
  transition: transform 0.2s, box-shadow 0.2s, background-color 0.2s, margin 0.3s;
  border-radius: 8px;
  padding: 12px;
  margin: 0;
}

/* Классический стиль - традиционный, сдержанный */
.dragging-классический {
  opacity: 0.9;
  transform: scale(0.99);
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  z-index: 10;
  background-color: rgba(240, 240, 240, 0.8);
  border: 1px dashed #999;
}

.drag-over-классический {
  margin-top: 20px;
  position: relative;
}

.drag-over-классический::before {
  content: "";
  position: absolute;
  top: -10px;
  left: 0;
  right: 0;
  height: 1px;
  background: #999;
  border-radius: 0;
}

.drag-after-классический {
  margin-bottom: 20px;
  position: relative;
}

.drag-after-классический::after {
  content: "";
  position: absolute;
  bottom: -10px;
  left: 0;
  right: 0;
  height: 1px;
  background: #999;
  border-radius: 0;
}

/* Современный стиль - плавный, с градиентами */
.dragging-современный {
  opacity: 0.85;
  transform: scale(0.98) translateY(4px);
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.15);
  z-index: 10;
  background-color: rgba(var(--neon-purple-rgb, 156, 39, 176), 0.05);
  border: 1px dashed rgba(var(--neon-purple-rgb, 156, 39, 176), 0.3);
}

.drag-over-современный {
  margin-top: 20px;
  position: relative;
}

.drag-over-современный::before {
  content: "";
  position: absolute;
  top: -10px;
  left: 0;
  right: 0;
  height: 2px;
  background: linear-gradient(90deg, var(--neon-purple, #9c27b0), var(--neon-blue, #2196f3));
  box-shadow: 0 0 8px rgba(var(--neon-purple-rgb, 156, 39, 176), 0.6);
  border-radius: 2px;
  animation: pulse 1.5s infinite;
}

.drag-after-современный {
  margin-bottom: 20px;
  position: relative;
}

.drag-after-современный::after {
  content: "";
  position: absolute;
  bottom: -10px;
  left: 0;
  right: 0;
  height: 2px;
  background: linear-gradient(90deg, var(--neon-purple, #9c27b0), var(--neon-blue, #2196f3));
  box-shadow: 0 0 8px rgba(var(--neon-purple-rgb, 156, 39, 176), 0.6);
  border-radius: 2px;
  animation: pulse 1.5s infinite;
}

/* Креативный стиль - яркий, игривый */
.dragging-креативный {
  opacity: 0.9;
  transform: rotate(-1deg) scale(0.98);
  box-shadow: 0 10px 25px rgba(52, 152, 219, 0.3);
  z-index: 10;
  background-color: rgba(52, 152, 219, 0.05);
  border: 2px dashed #3498db;
  transition: all 0.3s cubic-bezier(0.175, 0.885, 0.32, 1.275);
}

.drag-over-креативный {
  margin-top: 25px;
  position: relative;
}

.drag-over-креативный::before {
  content: "";
  position: absolute;
  top: -12px;
  left: 0;
  right: 0;
  height: 3px;
  background: #3498db;
  border-radius: 3px;
  animation: bounce 1s infinite;
}

.drag-after-креативный {
  margin-bottom: 25px;
  position: relative;
}

.drag-after-креативный::after {
  content: "";
  position: absolute;
  bottom: -12px;
  left: 0;
  right: 0;
  height: 3px;
  background: #3498db;
  border-radius: 3px;
  animation: bounce 1s infinite;
}

@keyframes bounce {
  0%, 100% {
    transform: scaleX(0.95);
  }
  50% {
    transform: scaleX(1.05);
  }
}

/* Минималистичный стиль - очень простой, тонкий */
.dragging-минималистичный {
  opacity: 0.95;
  transform: scale(0.99);
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  z-index: 10;
  background-color: rgba(245, 245, 245, 0.8);
  border: 1px solid #eee;
}

.drag-over-минималистичный {
  margin-top: 15px;
  position: relative;
}

.drag-over-минималистичный::before {
  content: "";
  position: absolute;
  top: -7px;
  left: 10%;
  right: 10%;
  height: 1px;
  background: #ddd;
}

.drag-after-минималистичный {
  margin-bottom: 15px;
  position: relative;
}

.drag-after-минималистичный::after {
  content: "";
  position: absolute;
  bottom: -7px;
  left: 10%;
  right: 10%;
  height: 1px;
  background: #ddd;
}

/* Профессиональный стиль - строгий, деловой */
.dragging-профессиональный {
  opacity: 0.9;
  transform: scale(0.98);
  box-shadow: 0 6px 15px rgba(0, 0, 0, 0.1);
  z-index: 10;
  background-color: rgba(240, 240, 240, 0.9);
  border: 1px solid #34495e;
}

.drag-over-профессиональный {
  margin-top: 20px;
  position: relative;
}

.drag-over-профессиональный::before {
  content: "";
  position: absolute;
  top: -10px;
  left: 0;
  right: 0;
  height: 2px;
  background: #34495e;
  border-radius: 0;
}

.drag-after-профессиональный {
  margin-bottom: 20px;
  position: relative;
}

.drag-after-профессиональный::after {
  content: "";
  position: absolute;
  bottom: -10px;
  left: 0;
  right: 0;
  height: 2px;
  background: #34495e;
  border-radius: 0;
}

/* Технический стиль - как код, с терминальной эстетикой */
.dragging-технический {
  opacity: 0.9;
  transform: translateY(2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
  z-index: 10;
  background-color: rgba(52, 73, 94, 0.516);
  border: 1px dashed #95a5a6;
  color: #ecf0f1;
}

.drag-over-технический {
  margin-top: 20px;
  position: relative;
}

.drag-over-технический::before {
  content: "// INSERT_BEFORE";
  position: absolute;
  top: -18px;
  left: 0;
  right: 0;
  font-family: monospace;
  font-size: 12px;
  color: #95a5a6;
  text-align: left;
  border-top: 1px dashed #95a5a6;
  padding-top: 2px;
}

.drag-after-технический {
  margin-bottom: 20px;
  position: relative;
}

.drag-after-технический::after {
  content: "// INSERT_AFTER";
  position: absolute;
  bottom: -18px;
  left: 0;
  right: 0;
  font-family: monospace;
  font-size: 12px;
  color: #95a5a6;
  text-align: left;
  border-bottom: 1px dashed #95a5a6;
  padding-bottom: 2px;
}

@keyframes pulse {
  0% {
    opacity: 0.6;
    transform: scaleX(0.98);
  }
  50% {
    opacity: 1;
    transform: scaleX(1);
  }
  100% {
    opacity: 0.6;
    transform: scaleX(0.98);
  }
}

.template-классический {
  @apply bg-white;
  font-family: 'Times New Roman', Times, serif;
}

.template-креативный .border-bottom-gradient {
  border-bottom: 2px solid #3498db;
}

.template-минималистичный .border-bottom-gradient {
  border-bottom: 1px solid #e0e0e0;
}

.template-профессиональный .border-bottom-gradient {
  border-bottom: 2px solid #34495e;
}

.template-технический .border-bottom-gradient {
  border-bottom: 2px dashed #7f8c8d;
}
</style>