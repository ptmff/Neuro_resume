import axios, { AxiosInstance } from "axios";
import { useAuthStore } from "@/stores/authStore";

const api: AxiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_BASE as string,
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true,
});

// Добавляем интерцептор токена
api.interceptors.request.use((config) => {
  const authStore = useAuthStore(); // доступ к store

  if (authStore.token) {
    config.headers.Authorization = `Bearer ${authStore.token}`;
  }

  return config;
});

export default api;
