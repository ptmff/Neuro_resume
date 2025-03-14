<template>
  <div
    class="bg-[var(--background-section)] bg-opacity-30 backdrop-blur-xl p-8 rounded-3xl border border-white/10"
  >
    <h2
      class="text-4xl font-bold bg-gradient-to-r from-[var(--text-secondary)] to-[var(--text-light)] bg-clip-text text-transparent mb-8"
    >
      Предпросмотр
    </h2>
    <div class="resume-preview p-6 bg-white rounded-xl shadow-lg">
      <div class="resume-header mb-6">
        <h3 class="text-3xl font-bold text-gray-800">{{ resumeData.name || 'Имя Фамилия' }}</h3>
        <p class="text-xl text-gray-600">{{ resumeData.profession || 'Профессия' }}</p>
      </div>
      <div class="resume-contact mb-6 text-gray-700">
        <p v-if="resumeData.email"><i class="fas fa-envelope mr-2"></i>{{ resumeData.email }}</p>
        <p v-if="resumeData.phone"><i class="fas fa-phone mr-2"></i>{{ resumeData.phone }}</p>
        <p v-if="resumeData.location">
          <i class="fas fa-map-marker-alt mr-2"></i>{{ resumeData.location }}
        </p>
      </div>
      <div class="resume-section mb-6" v-if="resumeData.education">
        <h4 class="text-xl font-semibold text-gray-800 mb-2">Образование</h4>
        <p class="text-gray-700">{{ resumeData.education }}</p>
      </div>
      <div class="resume-section mb-6" v-if="resumeData.experience">
        <h4 class="text-xl font-semibold text-gray-800 mb-2">Опыт работы</h4>
        <p class="text-gray-700">{{ resumeData.experience }}</p>
      </div>
      <div class="resume-section" v-if="resumeData.skills">
        <h4 class="text-xl font-semibold text-gray-800 mb-2">Навыки</h4>
        <p class="text-gray-700">{{ resumeData.skills }}</p>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    resumeData: {
      type: Object,
      required: true,
    },
  },
}
</script>

<style scoped>
.resume-preview {
  max-height: 600px;
  overflow-y: auto;
}
</style>
