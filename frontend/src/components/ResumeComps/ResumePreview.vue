<template>
  <div class="preview-wrapper">
    <h2 class="text-4xl font-bold bg-gradient-to-r from-[var(--text-secondary)] to-[var(--text-light)] bg-clip-text text-transparent mb-8">
      Предпросмотр
    </h2>

    <transition name="fade-slide">
      <div class="resume-preview shadow-xl">
        <!-- Заголовок -->
        <div class="text-center mb-6">
          <h3 class="text-3xl font-bold text-[var(--text-main)]">
            {{ resumeData.name || profile?.name || 'Имя Фамилия' }}
          </h3>
          <p class="text-lg text-[var(--text-mainless)]">
            {{ resumeData.job || 'Профессия' }}
          </p>
        </div>

        <!-- Контакты -->
        <h4 class="section-heading">📋 Личные данные</h4>
        <div class="space-y-2 text-sm text-[var(--text-main)] mb-6">
          <p v-if="resumeData.email || profile?.email">📧 {{ resumeData.email || profile?.email }}</p>
          <p v-if="resumeData.phone || profile?.phone">📞 {{ resumeData.phone || profile?.phone }}</p>
          <p v-if="resumeData.city || profile?.city">📍 {{ resumeData.city || profile?.city }}</p>
        </div>

        <!-- Образование -->
        <div v-if="profile?.education?.length" class="mb-6">
          <h4 class="section-heading">🎓 Образование</h4>
          <ul class="list-disc list-inside text-sm text-[var(--text-main)] space-y-1">
            <li v-for="(edu, i) in profile.education" :key="i">
              {{ edu.institution }} — {{ edu.degree }} ({{ edu.startYear }}–{{ edu.endYear }})
            </li>
          </ul>
        </div>

        <!-- О себе -->
        <h4 class="section-heading">😎 О себе</h4>
         <div class="mb-6 text-sm text-[var(--text-main)] space-y-1">
          {{ resumeData?.description }}
         </div>

        <!-- Опыт -->
        <div v-if="resumeData.experience?.length" class="mb-6">
          <h4 class="section-heading">💼 Опыт работы</h4>
          <div class="timeline">
            <div v-for="(exp, i) in resumeData.experience" :key="i" class="timeline-entry">
              <div class="dot" />
              <div class="content">
                <div class="font-semibold text-[var(--text-main)]">
                  {{ exp.company }} — {{ exp.position }}
                </div>
                <div class="text-xs text-[var(--text-mainless)]">{{ exp.startDate }} – {{ exp.endDate }}</div>
                <p class="text-sm mt-1 text-[var(--text-main)]">{{ exp.description }}</p>
              </div>
            </div>
          </div>
        </div>

        <!-- Навыки -->
        <div v-if="resumeData.skills?.length">
          <h4 class="section-heading">🛠️ Навыки</h4>
          <transition-group name="chip" tag="div" class="flex flex-wrap gap-2">
            <span
              v-for="(skill, index) in resumeData.skills"
              :key="skill + index"
              class="skill-chip"
            >
              <i class="fas fa-code mr-1"></i>{{ skill }}
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
</style>