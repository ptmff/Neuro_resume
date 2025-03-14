<template>
  <div class="mb-4">
    <label :for="id" class="block text-sm font-medium text-[var(--text-secondary)] mb-2">{{ label }}</label>
    <input
      v-if="type !== 'textarea'"
      :type="type"
      :id="id"
      :value="modelValue"
      @input="$emit('update:modelValue', $event.target.value)"
      :required="required"
      :placeholder="placeholder"
      class="w-full bg-[var(--background-section)] bg-opacity-50 border border-white/10 rounded-xl px-4 py-3 text-[var(--text-light)] focus:outline-none focus:border-[var(--text-secondary)] transition-all duration-300"
    />
    <textarea
      v-else
      :id="id"
      :value="modelValue"
      @input="$emit('update:modelValue', $event.target.value)"
      :required="required"
      :placeholder="placeholder"
      rows="3"
      class="w-full bg-[var(--background-section)] bg-opacity-50 border border-white/10 rounded-xl px-4 py-3 text-[var(--text-light)] focus:outline-none focus:border-[var(--text-secondary)] transition-all duration-300"
    ></textarea>
  </div>
</template>

<script setup>
defineProps({
  id: {
    type: String,
    required: true
  },
  label: {
    type: String,
    required: true
  },
  type: {
    type: String,
    default: 'text'
  },
  modelValue: {
    type: String,
    required: true
  },
  required: {
    type: Boolean,
    default: false
  },
  placeholder: {
    type: String,
    default: ""
  }
})

defineEmits(['update:modelValue'])
</script>

