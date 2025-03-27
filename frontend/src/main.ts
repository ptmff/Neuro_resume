import { createApp } from 'vue'
import './style.css'
import router from './router/index'
import App from './App.vue'

import '@fortawesome/fontawesome-free/css/all.min.css'
import 'aos/dist/aos.css'
import AOS from 'aos'
AOS.init()

import { createPinia } from 'pinia'

const app = createApp(App)
const pinia = createPinia()

app.use(pinia)
app.use(router)
app.mount('#app')
