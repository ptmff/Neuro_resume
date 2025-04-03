// src/types/types.ts

// Образование
export interface Education {
  institution: string
  degree: string
  field: string
  startYear: number
  endYear: number
}

// Опыт работы
export interface Experience {
  company: string
  position: string
  startDate: string // YYYY-MM
  endDate: string // YYYY-MM
  description: string
}

// Тип одного резюме
export interface Resume {
  id: number;
  title: string;
  job: string;
  city: string;
  date: string;
  skills: string[];
  experience: Experience[];
  education: Education[];
  template: string;
  description?: string;
}

// Тип профиля пользователя
export interface UserProfile {
  id: number
  name: string
  email?: string
  phone?: string
  city?: string
  education?: Education[]
  mainResumeId: number | null
  resumes: number[]
  photo?: string
}

// Тип для сокращённого редактирования
export type EditableProfileFields = Pick<UserProfile, 'name' | 'email'>

// Алиас для удобства совместимости
export type Profile = UserProfile
