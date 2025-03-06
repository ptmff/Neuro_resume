<template>
    <div class="mb-3">
      <label :for="id" class="form-label">{{ label }}</label>
      <input
        v-if="type === 'text' || type === 'email' || type === 'tel'"
        :type="type"
        class="form-control"
        :id="id"
        :value="modelValue"
        @input="$emit('update:modelValue', $event.target.value)"
        :required="required"
        :placeholder="placeholder"

      />
      <textarea
        v-if="type === 'textarea'"
        class="form-control"
        :id="id"
        :value="modelValue"
        @input="$emit('update:modelValue', $event.target.value)"
        :required="required"
        :placeholder="placeholder"
        rows="3"
      ></textarea>
    </div>
</template>

<script>
export default {
  props: {
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
    placeholder:{
        type: String,
        default: ""
    }
  },
  data() {
    return {
      isFocused: false // Состояние фокуса
    };
  }
};
</script>

<style scoped>

.form-control:focus {
  box-shadow: 0 0 0 0.2rem rgba(139, 94, 60, 0.25); /* Коричневая тень при фокусе */
  border-color: #8b5e3c;
}

</style>