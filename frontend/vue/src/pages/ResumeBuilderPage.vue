<template>
  <div class="page">
    <ProgressBar/>
    <ResumeForm/>
  </div>
</template>

<script>
import ResumeForm from '../components/ResumeForm.vue';
import ProgressBar from '../components/ProgressBar.vue';

export default{
components:{
  ResumeForm,
  ProgressBar
},
};
</script>

<style scoped>
</style>