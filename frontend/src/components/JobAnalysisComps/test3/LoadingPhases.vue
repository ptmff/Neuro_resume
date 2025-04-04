<script setup lang="ts">
import { useAnalysisStore } from '@/stores/test3-analysisStore'
import PhaseBar from './LoadingPhases/PhaseBar.vue'

const store = useAnalysisStore()
</script>

<template>
  <div class="flex flex-col gap-4 w-full max-w-2xl mx-auto py-6">
    <PhaseBar
      v-for="phase in store.phases"
      :key="phase.id"
      :id="phase.id"
      :title="phase.title"
      :description="phase.description"
      :status="phase.status"
      :progress="phase.visualProgress"
    />
  </div>
</template>
