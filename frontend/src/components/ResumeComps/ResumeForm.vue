<template>
  <div class="bg-[var(--background-section)] bg-opacity-30 backdrop-blur-xl p-8 rounded-3xl border border-[var(--background-pale)]">
    <h2 class="text-4xl font-bold bg-gradient-to-r from-[var(--text-secondary)] to-[var(--text-light)] bg-clip-text text-transparent mb-8">
      Заполните данные
    </h2>

    <form @submit.prevent="convertDataAndNext">
      <!-- 📌 Основные поля -->
      <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
        <FormField id="title" label="Название резюме" :modelValue="safeValue(resumeData.title)" @update:modelValue="updateField('title', $event)" placeholder="Например: Резюме для Frontend" required />
        <FormField id="city" label="Город" :modelValue="safeValue(resumeData.city)" @update:modelValue="updateField('city', $event)" placeholder="Москва" required />
        <FormField id="job" label="Профессия" :modelValue="safeValue(resumeData.job)" @update:modelValue="updateField('job', $event)" placeholder="Frontend-разработчик" required />
      </div>

      <!-- Поле "О себе" -->
       <div class="mt-6">
        <FormField
          id="description"
          label="О себе (необязательно)"
          :modelValue="safeValue(resumeData.description)"
          @update:modelValue="updateField('description', $event)"
          type="textarea"
          autoGrow
          placeholder="Кратко расскажите о себе, что вас отличает"
        />
       </div>

      <!-- 🧠 Опыт работы -->
      <div class="mt-6">
        <label class="block text-sm font-medium text-[var(--text-secondary)] mb-2">Опыт работы</label>
        <draggable v-model="experience" group="experience" item-key="id" handle=".drag-handle">
          <template #item="{ element, index }">
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4 relative bg-[var(--background-section)] rounded-xl p-4 border border-[var(--background-pale)]">
              <div class="absolute left-2 top-0 bottom-0 w-3 flex items-center justify-center cursor-move drag-handle z-10 gap-1">
                <div class="w-1 h-20 bg-gradient-to-b from-[var(--neon-purple)] to-[var(--neon-blue)] rounded-full"></div>
                <div class="w-1 h-20 bg-gradient-to-b from-[var(--neon-purple)] to-[var(--neon-blue)] rounded-full"></div>
              </div>
              <div class="w-full md:col-span-2 pl-4">
                <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <FormField :modelValue="safeValue(element.company)" @update:modelValue="updateExperienceField(index, 'company', $event)" label="Компания" :id="`company-${index}`" />
                  <FormField :modelValue="safeValue(element.position)" @update:modelValue="updateExperienceField(index, 'position', $event)" label="Должность" :id="`position-${index}`" />
                  <FormField :modelValue="safeValue(element.startDate)" @update:modelValue="updateExperienceField(index, 'startDate', $event)" label="Дата начала" type="month" :id="`start-${index}`" />
                  <FormField :modelValue="safeValue(element.endDate)" @update:modelValue="updateExperienceField(index, 'endDate', $event)" label="Дата окончания" type="month" :id="`end-${index}`" />
                  <FormField autoGrow :modelValue="safeValue(element.description)" @update:modelValue="updateExperienceField(index, 'description', $event)" label="Описание" type="textarea" :id="`desc-${index}`" class="md:col-span-2" />
                </div>
                <button
                  type="button"
                  class="absolute top-2 right-2 text-xs text-[var(--resume-red)] hover:text-[var(--resume-red-hover)] transition"
                  @click="removeExperience(index)"
                >
                  ✕
                </button>
              </div>
            </div>
          </template>
        </draggable>
        <button type="button" class="btn btn-secondary" @click="addExperience">+ Добавить опыт</button>
      </div>

      <!-- 💡 Навыки -->
      <div class="mt-6">
        <label for="skillInput" class="block text-sm font-medium text-[var(--text-secondary)] mb-2">Навыки</label>
        <input
          id="skillInput"
          v-model="newSkill"
          @keydown.enter.prevent="addSkill"
          @keydown="handleComma"
          placeholder="Введите навык и нажмите Enter или запятую"
          class="w-full bg-[var(--background-section)] border border-[var(--background-pale)] rounded-xl px-4 py-2 text-[var(--text-light)] focus:outline-none focus:border-[var(--background-cta)]"
        />
        <div class="flex flex-wrap gap-2 mt-3">
          <span
            v-for="(skill, index) in skills"
            :key="index"
            class="px-4 py-1 rounded-full bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] text-[var(--text-light)] text-sm"
          >
            {{ skill }}
            <button class="ml-2 text-[var(--text-mainless)] hover:text-[var(--text-light)]" @click.prevent="removeSkill(index)">×</button>
          </span>
        </div>
      </div>

      <!-- 🔘 Кнопки -->
      <div class="flex justify-end mt-8">
        <button type="submit" class="btn btn-primary">Далее</button>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
import draggable from 'vuedraggable'
import FormField from './FormField.vue'

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
  experience?: Array<Experience>;
  skills?: Array<string>;
  template?: string;
  date?: string;
  sectionsOrder?: Array<string>;
  [key: string]: any;
}

interface ResumeFormProps {
  resumeData: ResumeData;
}

const props = defineProps<ResumeFormProps>()

const emit = defineEmits<{
  (e: 'next-step'): void;
  (e: 'update:modelValue', value: ResumeData): void;
}>()

const experience = ref<Experience[]>([...(props.resumeData.experience || [])])
const skills = ref<string[]>([...(props.resumeData.skills || [])])
const newSkill = ref('')

// Функция для безопасного получения значения (всегда возвращает строку)
const safeValue = (value: any): string => {
  return value || ''
}

// Обновление полей resumeData
const updateField = (field: string, value: string): void => {
  props.resumeData[field] = value
  syncDataToPreview()
}

// Обновление полей опыта работы
const updateExperienceField = (index: number, field: string, value: string): void => {
  if (experience.value[index]) {
    experience.value[index][field as keyof Experience] = value as any
    syncDataToPreview()
  }
}

// 📌 Обновление preview
const syncDataToPreview = (): void => {
  props.resumeData.experience = experience.value
  props.resumeData.skills = skills.value
  emit('update:modelValue', props.resumeData)
}

// Авто-слежение за перемещением опыта
watch(experience, () => {
  syncDataToPreview()
}, { deep: true })

// Методы
const addExperience = (): void => {
  experience.value.push({
    id: Date.now().toString(),
    company: '',
    position: '',
    startDate: '',
    endDate: '',
    description: ''
  })
}

const removeExperience = (index: number): void => {
  experience.value.splice(index, 1)
}

const addSkill = (): void => {
  const skill = newSkill.value.trim()
  if (skill && !skills.value.includes(skill)) {
    skills.value.push(skill)
    syncDataToPreview()
  }
  newSkill.value = ''
}

const removeSkill = (index: number): void => {
  skills.value.splice(index, 1)
  syncDataToPreview()
}

const handleComma = (event: KeyboardEvent): void => {
  if (event.key === ',') {
    event.preventDefault()
    addSkill()
  }
}

// 🚀 Далее
const convertDataAndNext = (): void => {
  syncDataToPreview()
  props.resumeData.date = new Date().toISOString().slice(0, 10)
  emit('next-step')
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
textarea::-webkit-scrollbar {
  width: 6px;
}
textarea::-webkit-scrollbar-track {
  background: transparent;
}
textarea::-webkit-scrollbar-thumb {
  background-color: var(--neon-purple);
  border-radius: 9999px;
}
</style>