import { createApp } from 'vue';
import App from './App.vue';
import router from './router/index.js';
import './style.css';

// Импорт иконок
import '@fortawesome/fontawesome-free/css/all.min.css';

// Импорт Bootstrap 5
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap';

const app = createApp(App);
app.use(router);
app.mount('#app');
