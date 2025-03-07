<template>
  <div>
    <HeroSection />
    <FunctionalSection />
    <WorkLogic />
  </div>
</template>

<script>
import HeroSection from "../components/HeroSection.vue";
import FunctionalSection from "../components/FunctionalSection.vue";
import WorkLogic from "../components/WorkLogic.vue";

export default {
  components: {
    HeroSection,
    FunctionalSection,
    WorkLogic,
  },
};
</script>

<style scoped>
</style>