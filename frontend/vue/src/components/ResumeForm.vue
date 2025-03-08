<template>
    <div class="container mt-5">
        <h2 class="title mb-5" >Создать резюме</h2>
        <form @submit.prevent="$emit('next-step')">
            <FormField
                id="name"
                label="Имя и Фамилия"
                v-model="resumeData.name"
                placeholder="Имя Фамилия"
                required
            />
            <FormField
                id="email"
                label="Email"
                type="email"
                v-model="resumeData.email"
                placeholder="name@example.com"
                required
            />
            <FormField
                id="phone"
                label="Телефон"
                type="tel"
                v-model="resumeData.phone"
                placeholder="+7 (999) 999 99-99"
                required
            />
            <FormField
                id="profession"
                label="Профессия"
                v-model="resumeData.profession"
                placeholder="Профессия"
                required
            />
            <FormField
                id="education"
                label="Образование"
                type="textarea"
                v-model="resumeData.education"
                placeholder="Образование"
                required
            />
            <FormField
                id="experience"
                label="Опыт работы"
                type="textarea"
                v-model="resumeData.experience"
                placeholder="Опыт работы"
                required
            />
            <FormField
                id="skills"
                label="Навыки"
                type="textarea"
                v-model="resumeData.skills"
                placeholder="Навыки"
                required
            />
            <div class="d-flex justify-content-between mt-4">
                <button type="button" class="btn btn-outline-secondary" @click="$emit('prev-step')">Назад</button>
                <button type="button" class="btn" @click="$emit('next-step')">Далее</button>            </div>        </form>
    </div>
</template>
  
<script>

import FormField from './FormField.vue';

export default {
    components:{
        FormField
    },
    props: {
        resumeData: {
        type: Object,
        required: true
        }
    },
    emits: ['next-step']
    
    };
</script>

<style scoped>
    .card {
    margin-top: 20px;
    }
    .container{
        color: #3b2f2f;
    }

    .title{
        font-size: 2rem;
        font-weight: bold;
    }

    .btn{
        color: white;
        background-color: #8b5e3c;
    }

    .btn:hover{
        background-color: #6d4c2f;
    }
</style>