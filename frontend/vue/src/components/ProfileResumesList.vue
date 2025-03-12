<template>
    <div class="resume-container">
      <h2>Мои резюме</h2>
      <div v-if="resumes.length" class="resume-list">
        <div v-for="resume in resumes" :key="resume.id" class="resume-card">
          <div class="resume-info">
            <div>
              <h3>{{ resume.title }}</h3>
              <p>Дата создания: {{ resume.date }}</p>
              <p :class="statusClass(resume.status)">Статус: {{ resume.status }}</p>
            </div>
            <div class="buttons">
              <button class="edit-btn" @click="editResume(resume.id)">Редактировать</button>
              <button class="delete-btn" @click="deleteResume(resume.id)">Удалить</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </template>
