import { defineStore } from "pinia";
import { loginUser, registerUser } from "@/api/auth";
import type { UserRegisterDto } from "@/types/auth";

export const useAuthStore = defineStore("auth", {
  state: () => ({
    token: localStorage.getItem("token") || "",
    loading: false,
    error: "",
  }),
  actions: {
    async login(email: string, password: string) {
      this.loading = true;
      this.error = "";
      try {
        const token = await loginUser({ email, password });
        this.token = token;
        localStorage.setItem("token", token);
      } catch (e: any) {
        this.error = e.response?.data || "Login failed";
      } finally {
        this.loading = false;
      }
    },

    async register(payload: UserRegisterDto) {
      this.loading = true;
      this.error = "";
      try {
        await registerUser(payload);
      } catch (e: any) {
        this.error = e.response?.data || "Registration failed";
      } finally {
        this.loading = false;
      }
    },

    logout() {
      this.token = "";
      localStorage.removeItem("token");
    }
  },
});
