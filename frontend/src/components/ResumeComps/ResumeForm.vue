<template>
  <div class="bg-[var(--background-section)] bg-opacity-30 backdrop-blur-xl p-8 rounded-3xl border border-white/10">
    <h2 class="text-4xl font-bold bg-gradient-to-r from-[var(--text-secondary)] to-[var(--text-light)] bg-clip-text text-transparent mb-8">
      Заполните данные
    </h2>

    <!-- 📋 Форма -->
    <form @submit.prevent="convertDataAndNext">
      <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
        <FormField
          id="title"
          label="Название резюме"
          v-model="resumeData.title"
          placeholder="Например: Резюме для Frontend"
          required
        />
        <FormField
          id="city"
          label="Город"
          v-model="resumeData.city"
          placeholder="Москва"
          required
        />
        <FormField
          id="job"
          label="Профессия"
          v-model="resumeData.job"
          placeholder="Frontend-разработчик"
          required
        />
      </div>

      <!-- 🧠 Опыт работы -->
      <div class="mt-6">
        <label class="block text-sm text-white/70 mb-2">Опыт работы</label>
        <div v-for="(exp, index) in experience" :key="index" class="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4">
          <FormField v-model="exp.company" label="Компания" :id="`company-${index}`" />
          <FormField v-model="exp.position" label="Должность" :id="`position-${index}`" />
          <FormField v-model="exp.startDate" label="Дата начала" type="month" :id="`start-${index}`" />
          <FormField v-model="exp.endDate" label="Дата окончания" type="month" :id="`end-${index}`" />
          <FormField v-model="exp.description" label="Описание" type="textarea" :id="`desc-${index}`" class="md:col-span-2" />
        </div>
        <button type="button" class="btn btn-secondary" @click="addExperience">+ Добавить опыт</button>
      </div>

      <!-- 💡 Навыки -->
      <div class="mt-6">
        <label for="skillInput" class="block text-sm text-white/70 mb-2">Навыки</label>
        <input
          id="skillInput"
          v-model="newSkill"
          @keydown.enter.prevent="addSkill"
          @keydown="handleComma"
          placeholder="Введите навык и нажмите Enter или запятую"
          class="w-full bg-white/10 border border-white/10 rounded-xl px-4 py-2 text-white focus:outline-none focus:border-violet-500"
        />
        <div class="flex flex-wrap gap-2 mt-3">
          <span
            v-for="(skill, index) in skills"
            :key="index"
            class="px-4 py-1 rounded-full bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] text-white text-sm"
          >
            {{ skill }}
            <button class="ml-2 text-white/60 hover:text-white" @click.prevent="removeSkill(index)">×</button>
          </span>
        </div>
      </div>

      <!-- 🔘 Кнопки -->
      <div class="flex justify-between mt-8">
        <button type="button" class="btn btn-secondary" disabled>Назад</button>
        <button type="submit" class="btn btn-primary">Далее</button>
      </div>
    </form>
  </div>
</template>

<script>
import FormField from './FormField.vue'

export default {
  components: { FormField },
  props: {
    resumeData: {
      type: Object,
      required: true
    }
  },
  emits: ['next-step', 'update:modelValue'],
  data() {
    return {
      experience: this.resumeData.experience || [],
      skills: this.resumeData.skills || [],
      newSkill: ''
    }
  },
  methods: {
    addExperience() {
      this.experience.push({
        company: '',
        position: '',
        startDate: '',
        endDate: '',
        description: ''
      })
    },
    addSkill() {
      const skill = this.newSkill.trim()
      if (skill && !this.skills.includes(skill)) {
        this.skills.push(skill)
      }
      this.newSkill = ''
    },
    removeSkill(index) {
      this.skills.splice(index, 1)
    },
    convertDataAndNext() {
      this.resumeData.experience = this.experience
      this.resumeData.skills = this.skills
      this.resumeData.date = new Date().toISOString().slice(0, 10)

      this.$emit('update:modelValue', this.resumeData)
      this.$emit('next-step')
    },
    handleComma(event) {
      if (event.key === ',') {
        event.preventDefault()
        this.addSkill()
      }
    }
  }
}
</script>

<style scoped>
.btn {
  @apply px-6 py-3 font-semibold rounded-full transition-all duration-300 transform hover:scale-105 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-[var(--neon-purple)];
}
.btn-primary {
  @apply bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] text-white;
}
.btn-secondary {
  @apply bg-[var(--background-section)] bg-opacity-50 text-[var(--text-light)];
}
</style>
