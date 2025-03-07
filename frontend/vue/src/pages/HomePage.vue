<template>
  <div>
    <HeroSection />
    <FunctionalSection />
    <WorkLogic />
    <AdvantagesSection />
  </div>
</template>

<script>
import HeroSection from "../components/HeroSection.vue";
import FunctionalSection from "../components/FunctionalSection.vue";
import WorkLogic from "../components/WorkLogic.vue";
import AdvantagesSection from "../components/AdvantagesSection.vue";

export default {
  components: {
    HeroSection,
    FunctionalSection,
    WorkLogic,
    AdvantagesSection,
  },
};
</script>

<style scoped>
</style>