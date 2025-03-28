<template>
  <div class="bg-[var(--background-section)] bg-opacity-30 backdrop-blur-xl p-8 rounded-3xl border border-[var(--background-pale)]">
    <h2 class="text-4xl font-bold bg-gradient-to-r from-[var(--text-secondary)] to-[var(--text-light)] bg-clip-text text-transparent mb-8">
      Выберите стиль
    </h2>

    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-10">
      <div
        v-for="(template, index) in templates"
        :key="index"
        class="template-card group cursor-pointer w-full max-w-[350px] mx-auto border-2 transition-all duration-300 h-[100px]"
        :class="{
          'border-[var(--neon-purple)]': selectedTemplate === index,
          'border-[var(--background-pale)]': selectedTemplate !== index
        }"
        @click="select(index)"
      >
        <div class="relative overflow-hidden rounded-xl h-full">
          <img
            :src="template.image"
            class="w-full h-full object-cover transition-transform duration-300 group-hover:scale-110"
            :alt="template.name"
          />
          <div
            class="absolute inset-0 bg-gradient-to-t from-black/70 to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-300 flex items-end justify-center p-4"
          >
            <span class="text-white text-lg font-semibold">{{ template.name }}</span>
          </div>
        </div>
      </div>
    </div>

    <div class="flex justify-between mt-8">
      <button @click="$emit('prev-step')" class="btn btn-secondary">Назад</button>
      <button @click="$emit('next-step')" class="btn btn-primary">Далее</button>
    </div>
  </div>
</template>

<script setup lang="ts">
interface Template {
  name: string;
  image: string;
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
  experience?: Array<Experience>;
  skills?: Array<string>;
  template?: string;
  date?: string;
  sectionsOrder?: Array<string>;
  [key: string]: any;
}

interface TemplateSelectionProps {
  templates: Template[];
  selectedTemplate: number;
  resumeData: ResumeData;
}

const props = defineProps<TemplateSelectionProps>()

const emit = defineEmits<{
  (e: 'select-template', index: number): void;
  (e: 'next-step'): void;
  (e: 'prev-step'): void;
  (e: 'update:modelValue', value: ResumeData): void;
}>()

const select = (index: number): void => {
  emit('select-template', index)

  const updatedResume: ResumeData = {
    ...props.resumeData,
    template: props.templates[index].name.toLowerCase()
  }

  // Обновляем только локально (в родителе и предпросмотре)
  emit('update:modelValue', updatedResume)
}
</script>

<style scoped>
.template-card {
  @apply overflow-hidden rounded-xl transition-all duration-300 transform hover:scale-105;
}
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