<template>
  <div class="page">
    <ProfileHero/>
    <ProfileResumesList/>
    <ProfileConsultant/>
  </div>
</template>
  
<script>
import ProfileHero from '../components/ProfileHero.vue'; 
import ProfileResumesList from '../components/ProfileResumesList.vue'; 
import ProfileConsultant from '../components/ProfileConsultant.vue'; 

export default {
  components: {
    ProfileHero,
    ProfileResumesList,
    ProfileConsultant,
  },
};
</script>