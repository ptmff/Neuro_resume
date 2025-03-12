<template>
  <div class="page">
    <ProfileHero/>
    <ProfileResumesList/>
  </div>
</template>
  
<script>
import ProfileHero from '../components/ProfileHero.vue'; 
import ProfileResumesList from '../components/ProfileResumesList.vue'; 

export default {
  components: {
    ProfileHero,
    ProfileResumesList,
  },
};
</script>