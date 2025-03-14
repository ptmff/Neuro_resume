export interface Resume {
  id: number
  title: string
}

export interface Profile {
  name: string
  email?: string
  mainResumeId: number
  resumes: number[]
}
