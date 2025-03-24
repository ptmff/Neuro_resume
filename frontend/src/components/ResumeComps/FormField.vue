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
      @input="$emit('update:modelValue', $event.target.value)"
      :required="required"
      :placeholder="placeholder"
      class="w-full bg-[var(--background-section)] bg-opacity-50 border border-[var(--background-pale)] rounded-xl px-4 py-3 text-[var(--text-light)] focus:outline-none focus:border-[var(--text-secondary)] transition-all duration-300"
    />
    <textarea
      v-else
      ref="textareaRef"
      :id="id"
      :value="modelValue"
      @input="$emit('update:modelValue', $event.target.value)"
      :required="required"
      :placeholder="placeholder"
      rows="1"
      class="w-full resize-none overflow-hidden bg-[var(--background-section)] bg-opacity-50 border border-[var(--background-pale)] rounded-xl px-4 py-3 text-[var(--text-light)] focus:outline-none focus:border-[var(--text-secondary)] transition-all duration-300"
    />
  </div>
</template>

<script setup>
import { ref, watch, onMounted } from 'vue'
const props = defineProps({
  id: {
    type: String,
    required: true,
  },
  label: {
    type: String,
    required: true,
  },
  type: {
    type: String,
    default: 'text',
  },
  modelValue: {
    type: String,
    required: true,
  },
  required: {
    type: Boolean,
    default: false,
  },
  placeholder: {
    type: String,
    default: '',
  },
  autoGrow: {
    type: Boolean,
    default: false,
  }
})


defineEmits(['update:modelValue'])

const textareaRef = ref(null)

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

const handleInput = (e) => {
  const target = e.target
  emit('update:modelValue', target.value)

  if (autoGrow && target.tagName === 'TEXTAREA') {
    target.style.height = 'auto'
    target.style.height = target.scrollHeight + 'px'
  }
}

</script>
