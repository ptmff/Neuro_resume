import { defineStore } from "pinia";
import api from "@/api";

interface Profile {
  email: string;
  name?: string;
  phone?: string;
  city?: string;
  photo?: string;
  education?: {
    institution: string;
    degree: string;
    field: string;
    startYear: number;
    endYear: number;
  }[];
  mainResumeId?: number | null;
}

export const useProfileStore = defineStore("profile", {
  state: () => ({
    profile: null as Profile | null,
    loading: false,
    error: null as string | null,
  }),

  actions: {
    async fetchProfile() {
      try {
        this.loading = true;
        const res = await api.get("/Profile");
        this.profile = res.data;
      } catch (err: any) {
        this.error = err.response?.data || "Ошибка загрузки профиля";
      } finally {
        this.loading = false;
      }
    },

    async updateProfile(updatedData: Partial<Profile>) {
      try {
        const res = await api.patch("/Profile", updatedData);
        this.profile = res.data;
      } catch (err: any) {
        this.error = err.response?.data || "Ошибка обновления профиля";
      }
    },

    async deleteProfile() {
      try {
        await api.delete("/Profile");
        this.profile = null;
      } catch (err: any) {
        this.error = err.response?.data || "Ошибка удаления профиля";
      }
    },

    clearProfile() {
      this.profile = null;
      this.error = null;
    },

    setMainResume(id: number | null) {
      if (this.profile) {
        this.profile.mainResumeId = id;
      }
    }
  },
});
