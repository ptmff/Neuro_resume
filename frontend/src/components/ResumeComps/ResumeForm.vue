<template>
  <div class="bg-[var(--background-section)] bg-opacity-30 backdrop-blur-xl p-8 rounded-3xl border border-[var(--background-pale)]">
    <h2 class="text-4xl font-bold bg-gradient-to-r from-[var(--text-secondary)] to-[var(--text-light)] bg-clip-text text-transparent mb-8">
      Заполните данные
    </h2>

    <form @submit.prevent="convertDataAndNext">
      <!-- 📌 Основные поля -->
      <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
        <FormField id="title" label="Название резюме" v-model="resumeData.title" placeholder="Например: Резюме для Frontend" required />
        <FormField id="city" label="Город" v-model="resumeData.city" placeholder="Москва" required />
        <FormField id="job" label="Профессия" v-model="resumeData.job" placeholder="Frontend-разработчик" required />
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
                  <FormField v-model="element.company" label="Компания" :id="`company-${index}`" />
                  <FormField v-model="element.position" label="Должность" :id="`position-${index}`" />
                  <FormField v-model="element.startDate" label="Дата начала" type="month" :id="`start-${index}`" />
                  <FormField v-model="element.endDate" label="Дата окончания" type="month" :id="`end-${index}`" />
                  <FormField v-model="element.description" label="Описание" type="textarea" :id="`desc-${index}`" class="md:col-span-2" />
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

<script setup>
import { ref, watch } from 'vue'
import draggable from 'vuedraggable'
import FormField from './FormField.vue'

const props = defineProps({
  resumeData: {
    type: Object,
    required: true
  }
})

const emit = defineEmits(['next-step', 'update:modelValue'])

const experience = ref([...props.resumeData.experience ?? []])
const skills = ref([...props.resumeData.skills ?? []])
const newSkill = ref('')

// 📌 Обновление preview
const syncDataToPreview = () => {
  props.resumeData.experience = experience.value
  props.resumeData.skills = skills.value
  emit('update:modelValue', props.resumeData)
}

// Авто-слежение за перемещением опыта
watch(experience, () => {
  syncDataToPreview()
}, { deep: true })

// Методы
const addExperience = () => {
  experience.value.push({
    company: '',
    position: '',
    startDate: '',
    endDate: '',
    description: ''
  })
}

const removeExperience = (index) => {
  experience.value.splice(index, 1)
}

const addSkill = () => {
  const skill = newSkill.value.trim()
  if (skill && !skills.value.includes(skill)) {
    skills.value.push(skill)
    syncDataToPreview()
  }
  newSkill.value = ''
}

const removeSkill = (index) => {
  skills.value.splice(index, 1)
  syncDataToPreview()
}

const handleComma = (event) => {
  if (event.key === ',') {
    event.preventDefault()
    addSkill()
  }
}

// 🚀 Далее
const convertDataAndNext = () => {
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
</style>
