<template>
  <div class="page">
    <ProgressBar/>
    <ResumeForm :resumeData="resumeData"/>
    <ResumePreview :resumeData="resumeData"/>
  </div>
</template>

<script>
import ResumeForm from '../components/ResumeForm.vue';
import ProgressBar from '../components/ProgressBar.vue';
import ResumePreview from '../components/ResumePreview.vue';

export default{
components:{
  ResumeForm,
  ProgressBar,
  ResumePreview
},
 data() {
  return{
    resumeData: {
        name: '',
        email: '',
        phone: '',
        location: '',
        profession: '',
        education: '',
        experience: '',
        skills: ''
      }
  }
 }

};
</script>

<style scoped>
</style>