<template>
  <div class="preview-wrapper">
    <h2 class="text-4xl font-bold bg-gradient-to-r from-[var(--text-secondary)] to-[var(--text-light)] bg-clip-text text-transparent mb-8">
      Предпросмотр
    </h2>

    <transition name="fade-slide">
      <div class="resume-preview shadow-xl" :class="`template-${resumeData.template}`">
        <!-- Заголовок -->
        <div class="text-center mb-6">
          <h3 :class="getHeaderClass()">
            {{ resumeData.name || profile?.name || 'Имя Фамилия' }}
          </h3>
          <p :class="getSubheaderClass()">
            {{ resumeData.job || 'Профессия' }}
          </p>
        </div>

        <!-- Контакты -->
        <h4 :class="getSectionHeaderClass()">{{ getContactIcon() }} Личные данные</h4>
        <div class="space-y-2 mb-6" :class="getTextClass()">
          <p v-if="resumeData.email || profile?.email">{{ getEmailIcon() }} {{ resumeData.email || profile?.email }}</p>
          <p v-if="resumeData.phone || profile?.phone">{{ getPhoneIcon() }} {{ resumeData.phone || profile?.phone }}</p>
          <p v-if="resumeData.city || profile?.city">{{ getLocationIcon() }} {{ resumeData.city || profile?.city }}</p>
        </div>

        <!-- Образование -->
        <div v-if="profile?.education?.length" class="mb-6">
          <h4 :class="getSectionHeaderClass()">{{ getEducationIcon() }} Образование</h4>
          <ul class="list-disc list-inside space-y-1" :class="getTextClass()">
            <li v-for="(edu, i) in profile.education" :key="i">
              {{ edu.institution }} — {{ edu.degree }} ({{ edu.startYear }}–{{ edu.endYear }})
            </li>
          </ul>
        </div>

        <!-- О себе -->
        <h4 :class="getSectionHeaderClass()">{{ getAboutIcon() }} О себе</h4>
         <div class="mb-6 space-y-1" :class="getTextClass()">
          {{ resumeData?.description }}
         </div>

        <!-- Опыт -->
        <div v-if="resumeData.experience?.length" class="mb-6">
          <h4 :class="getSectionHeaderClass()">{{ getExperienceIcon() }} Опыт работы</h4>
          <div :class="getTimelineClass()">
            <div v-for="(exp, i) in resumeData.experience" :key="i" class="timeline-entry">
              <div :class="getDotClass()" />
              <div class="content">
                <div :class="getItemHeaderClass()">
                  {{ exp.company }} — {{ exp.position }}
                </div>
                <div :class="getDateClass()">{{ exp.startDate }} – {{ exp.endDate }}</div>
                <p class="mt-1" :class="getTextClass()">{{ exp.description }}</p>
              </div>
            </div>
          </div>
        </div>

        <!-- Навыки -->
        <div v-if="resumeData.skills?.length">
          <h4 :class="getSectionHeaderClass()">{{ getSkillsIcon() }} Навыки</h4>
          <transition-group name="chip" tag="div" class="flex flex-wrap gap-2">
            <span
              v-for="(skill, index) in resumeData.skills"
              :key="skill + index"
              :class="getSkillClass()"
            >
              <i :class="getSkillIconClass()"></i>{{ skill }}
            </span>
          </transition-group>
        </div>
      </div>
    </transition>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useProfileStore } from '@/stores/profileStore'

// Определяем интерфейсы прямо в компоненте, соответствующие фактической структуре данных
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

// Обновленный интерфейс Profile, соответствующий фактической структуре данных из хранилища
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

const profileStore = useProfileStore()
// Используем тип StoreProfile для соответствия фактической структуре данных
const profile = computed<StoreProfile | null>(() => profileStore.profile)

// Вычисляемые свойства для определения текущего шаблона
const currentTemplate = computed(() => {
  return (props.resumeData.template || 'базовый').toLowerCase()
})

const isBased = computed(() => currentTemplate.value === 'базовый')
const isClassic = computed(() => currentTemplate.value === 'классический')
const isModern = computed(() => currentTemplate.value === 'современный')
const isCreative = computed(() => currentTemplate.value === 'креативный')
const isMinimalist = computed(() => currentTemplate.value === 'минималистичный')
const isProfessional = computed(() => currentTemplate.value === 'профессиональный')
const isTechnical = computed(() => currentTemplate.value === 'технический')

// Методы для стилизации в зависимости от шаблона
const getHeaderClass = () => {
  if (isClassic.value) return 'text-3xl font-bold text-black mb-1'
  if (isModern.value) return 'text-3xl font-bold text-black]'
  if (isCreative.value) return 'text-3xl font-bold text-blue-600'
  if (isMinimalist.value) return 'text-3xl font-light text-gray-800'
  if (isProfessional.value) return 'text-3xl font-semibold text-gray-800'
  if (isTechnical.value) return 'text-3xl font-mono font-bold text-gray-800'
  return 'text-3xl font-bold text-[var(--text-main)]'
}

const getSubheaderClass = () => {
  if (isClassic.value) return 'text-lg text-gray-700'
  if (isModern.value) return 'text-lg text-gray-600'
  if (isCreative.value) return 'text-lg text-blue-500'
  if (isMinimalist.value) return 'text-lg text-gray-600 font-light'
  if (isProfessional.value) return 'text-lg text-gray-600 uppercase tracking-wider'
  if (isTechnical.value) return 'text-lg text-gray-600 font-mono'
  return 'text-lg text-[var(--text-mainless)]'
}

const getSectionHeaderClass = () => {
  if (isClassic.value) return 'text-lg font-bold mb-2 text-black-600'
  if (isModern.value) return 'text-lg font-semibold mb-2 text-purple-600'
  if (isCreative.value) return 'text-lg font-semibold mb-2 text-blue-600'
  if (isMinimalist.value) return 'text-lg font-normal mb-2 text-gray-800'
  if (isProfessional.value) return 'text-lg font-semibold mb-2 text-gray-700 uppercase tracking-wider'
  if (isTechnical.value) return 'text-lg font-mono font-semibold mb-2 text-gray-800'
  return 'section-heading'
}

const getTextClass = () => {
  if (isClassic.value) return 'text-sm text-black'
  if (isModern.value) return 'text-sm text-gray-600'
  if (isCreative.value) return 'text-sm text-gray-600'
  if (isMinimalist.value) return 'text-sm text-gray-600'
  if (isProfessional.value) return 'text-sm text-gray-700'
  if (isTechnical.value) return 'text-sm text-gray-600 font-mono'
  return 'text-sm text-[var(--text-main)]'
}

const getItemHeaderClass = () => {
  if (isClassic.value) return 'font-bold text-black'
  if (isModern.value) return 'font-semibold text-black-600'
  if (isCreative.value) return 'font-semibold text-blue-700'
  if (isMinimalist.value) return 'font-medium text-gray-800'
  if (isProfessional.value) return 'font-semibold text-gray-800'
  if (isTechnical.value) return 'font-mono font-semibold text-gray-800'
  return 'font-semibold text-[var(--text-main)]'
}

const getDateClass = () => {
  if (isClassic.value) return 'text-xs text-gray-600 '
  if (isModern.value) return 'text-xs text-gray-500'
  if (isCreative.value) return 'text-xs text-blue-500'
  if (isMinimalist.value) return 'text-xs text-gray-400'
  if (isProfessional.value) return 'text-xs text-gray-600 font-medium'
  if (isTechnical.value) return 'text-xs text-gray-500 font-mono'
  return 'text-xs text-[var(--text-mainless)]'
}

const getTimelineClass = () => {
  if (isClassic.value) return 'space-y-4 ml-2'
  if (isModern.value) return 'timeline'
  if (isCreative.value) return 'space-y-6 relative border-l-2 pl-4 border-blue-500'
  if (isMinimalist.value) return 'space-y-6 relative border-l pl-4 border-gray-200'
  if (isProfessional.value) return 'space-y-6 relative border-l-2 pl-4 border-gray-400'
  if (isTechnical.value) return 'space-y-6 relative border-l-2 pl-4 border-gray-500 border-dashed'
  return 'timeline'
}

const getDotClass = () => {
  if (isClassic.value) return 'hidden'
  if (isModern.value) return 'dot'
  if (isCreative.value) return 'absolute -left-[9px] top-1.5 w-3 h-3 bg-blue-500 rounded-full'
  if (isMinimalist.value) return 'absolute -left-[9px] top-1.5 w-2 h-2 bg-gray-300 rounded-full'
  if (isProfessional.value) return 'absolute -left-[9px] top-1.5 w-3 h-3 bg-gray-500 rounded-full'
  if (isTechnical.value) return 'absolute -left-[9px] top-1.5 w-3 h-3 bg-gray-600 rounded-sm'
  return 'dot'
}

const getSkillClass = () => {
  if (isClassic.value) return 'px-2 py-1 bg-gray-100 text-black text-xs rounded border border-gray-300'
  if (isModern.value) return 'skill-chip'
  if (isCreative.value) return 'px-3 py-1 rounded-full text-white text-xs font-medium flex items-center gap-1 transition transform bg-blue-500'
  if (isMinimalist.value) return 'px-3 py-1 rounded-full text-gray-700 text-xs font-medium flex items-center gap-1 transition transform border border-gray-200'
  if (isProfessional.value) return 'px-3 py-1 rounded-sm text-gray-700 text-xs font-medium flex items-center gap-1 transition transform bg-gray-100'
  if (isTechnical.value) return 'px-3 py-1 rounded-none text-gray-100 text-xs font-mono flex items-center gap-1 transition transform bg-gray-700'
  return 'skill-chip'
}

const getSkillIconClass = () => {
  if (isClassic.value) return 'hidden'
  if (isModern.value) return 'fas fa-code mr-1'
  if (isCreative.value) return 'fas fa-check-circle mr-1'
  if (isMinimalist.value) return 'fas fa-circle mr-1'
  if (isProfessional.value) return 'fas fa-check mr-1'
  if (isTechnical.value) return 'fas fa-terminal mr-1'
  return 'fas fa-code mr-1'
}

// Иконки для разделов в зависимости от шаблона
const getContactIcon = () => isBased.value ? '📋' : ''
const getEducationIcon = () => isBased.value ? '🎓' : ''
const getAboutIcon = () => isBased.value ? '😎' : '' 
const getExperienceIcon = () => isBased.value ? '💼' : '' 
const getSkillsIcon = () => isBased.value ? '🛠️' : '' 
const getEmailIcon = () => isBased.value ? '📧' : '' 
const getPhoneIcon = () => isBased.value ? '📞' : '' 
const getLocationIcon = () => isBased.value ? '📍' : '' 
</script>

<style scoped>
.preview-wrapper {
  @apply bg-[var(--background-section)] bg-opacity-30 backdrop-blur-xl p-8 rounded-3xl border border-[var(--background-pale)];
}

.resume-preview {
  @apply p-6 rounded-2xl text-gray-800 transition-all duration-500 bg-[var(--background-section)];
  border: 2px solid transparent;
  background-clip: padding-box;
  position: relative;
}
.resume-preview::before {
  content: "";
  position: absolute;
  top: -2px; left: -2px; right: -2px; bottom: -2px;
  z-index: -1;
  border-radius: 1rem;
  background: var(--gradient-primary);
  animation: glow 3s ease-in-out infinite alternate;
}

@keyframes glow {
  0% {
    filter: brightness(1.1) drop-shadow(0 0 6px var(--neon-blue));
  }
  100% {
    filter: brightness(1.4) drop-shadow(0 0 10px var(--neon-purple));
  }
}

.section-heading {
  @apply text-lg font-semibold mb-2 text-[var(--text-secondary)];
}

.timeline {
  @apply space-y-6 relative border-l-2 pl-4 border-[var(--accent)];
}
.timeline-entry {
  @apply relative;
}
.timeline-entry .dot {
  @apply absolute -left-[9px] top-1.5 w-3 h-3 bg-[var(--accent)] rounded-full;
}
.timeline-entry .content {
  @apply ml-2;
}

.skill-chip {
  @apply px-3 py-1 rounded-full text-[var(--text-light)] text-xs font-medium flex items-center gap-1 transition transform;
  background: linear-gradient(135deg, var(--neon-purple), var(--neon-blue));
}
.skill-chip:hover {
  transform: scale(1.05);
}

.fade-slide-enter-active {
  transition: all 0.5s ease;
}
.fade-slide-enter-from {
  opacity: 0;
  transform: translateY(20px);
}

.chip-enter-active,
.chip-leave-active {
  transition: all 0.3s ease;
}
.chip-enter-from {
  opacity: 0;
  transform: scale(0.8);
}
.chip-leave-to {
  opacity: 0;
  transform: scale(0.8);
}

/* Стили для разных шаблонов */
.template-классический {
  @apply bg-white;
  font-family: 'Times New Roman', Times, serif;
}
.template-классический::before {
  background: none;
  border: 1px solid #ccc;
  filter: none;
  animation: none;
}

.template-современный {
  @apply bg-white;
}
.template-современный::before {
  background: linear-gradient(135deg, #9b59b6, #8e44ad);
}

.template-креативный {
  @apply bg-white;
}
.template-креативный::before {
  background: linear-gradient(135deg, #3498db, #2980b9);
}

.template-минималистичный {
  @apply bg-white;
  border: 1px solid #e0e0e0;
}
.template-минималистичный::before {
  background: none;
  filter: none;
  animation: none;
}

.template-профессиональный {
  @apply bg-white;
}
.template-профессиональный::before {
  background: linear-gradient(135deg, #34495e, #2c3e50);
}

.template-технический {
  @apply bg-white text-gray-200;
}
.template-технический::before {
  background: linear-gradient(135deg, #7f8c8d, #95a5a6);
}
</style>