<template>
  <div class="mb-4">
    <label :for="id" class="block text-sm font-medium text-[var(--text-secondary)] mb-2">{{
      label
    }}</label>
    <input
      v-if="type !== 'textarea'"
      :type="type"
      :id="id"
      :value="modelValue"
      @input="$emit('update:modelValue', ($event.target as HTMLInputElement).value)"
      :required="required"
      :placeholder="placeholder"
      class="w-full bg-[var(--background-section)] bg-opacity-50 border border-[var(--background-pale)] rounded-xl px-4 py-3 text-[var(--text-light)] focus:outline-none focus:border-[var(--text-secondary)] transition-all duration-300"
    />
    <textarea
      v-else
      ref="textareaRef"
      :id="id"
      :value="modelValue"
      @input="$emit('update:modelValue', ($event.target as HTMLTextAreaElement).value)"
      :required="required"
      :placeholder="placeholder"
      rows="1"
      class="w-full resize-none overflow-hidden bg-[var(--background-section)] bg-opacity-50 border border-[var(--background-pale)] rounded-xl px-4 py-3 text-[var(--text-light)] focus:outline-none focus:border-[var(--text-secondary)] transition-all duration-300"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, watch, onMounted } from 'vue'

interface FormFieldProps {
  id: string;
  label: string;
  type?: string;
  modelValue: string;
  required?: boolean;
  placeholder?: string;
  autoGrow?: boolean;
}

const props = withDefaults(defineProps<FormFieldProps>(), {
  type: 'text',
  required: false,
  placeholder: '',
  autoGrow: false
})

defineEmits<{
  (e: 'update:modelValue', value: string): void
}>()

const textareaRef = ref<HTMLTextAreaElement | null>(null)

watch(() => props.modelValue, () => {
  if (props.autoGrow && textareaRef.value) {
    textareaRef.value.style.height = 'auto'
    textareaRef.value.style.height = textareaRef.value.scrollHeight + 'px'
  }
})

onMounted(() => {
  if (props.autoGrow && textareaRef.value) {
    textareaRef.value.style.height = 'auto'
    textareaRef.value.style.height = textareaRef.value.scrollHeight + 'px'
  }
})
</script>