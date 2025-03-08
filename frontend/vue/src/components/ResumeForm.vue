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
                @input="validateName"
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
                @input="formatPhone"
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
    emits: ['next-step'],
    methods: {
        formatPhone(event) {
            let value = event.target.value.replace(/\D/g, ''); // Убираем всё, кроме цифр

            if (value.startsWith('8')) {
                value = '7' + value.slice(1);
            }
            
            if (value.length > 11) {
                value = value.slice(0, 11); // Ограничение 11 цифр
            }

            let formatted = '+7 ';
            if (value.length > 1) formatted += `(${value.slice(1, 4)}`;
            if (value.length > 4) formatted += `) ${value.slice(4, 7)}`;
            if (value.length > 7) formatted += `-${value.slice(7, 9)}`;
            if (value.length > 9) formatted += `-${value.slice(9, 11)}`;

            this.resumeData.phone = formatted;
        },
        validateName(event) {
            let value = event.target.value;
            value = value.replace(/[^А-Яа-яЁёA-Za-z\s-]/g, ''); // Разрешаем буквы, пробел и дефис
            value = value.replace(/-{2,}/g, '-'); // Убираем двойные дефисы
            value = value.replace(/^\-|\-$/g, ''); // Убираем дефисы в начале и конце
            this.resumeData.name = value;
        }
    }
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