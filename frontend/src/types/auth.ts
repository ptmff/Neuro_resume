export interface UserLoginDto {
    email: string;
    password: string;
  }
  
  export interface UserRegisterDto {
    email: string
    password: string
    name?: string
    phone?: string
    city?: string
    education?: EducationDto[]
  }
  
  export interface EducationDto {
    institution: string;
    degree: string;
    field: string;
    startYear: number;
    endYear: number;
  }
  
  export interface UserDto {
    email?: string;
    name?: string;
    phone?: string;
    city?: string;
    photo?: string;
    mainResumeId?: number;
    education?: EducationDto[];
  }
  