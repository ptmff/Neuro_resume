import axios from "@/api";
import type { UserLoginDto, UserRegisterDto, UserDto } from "@/types/auth";

export async function loginUser(data: UserLoginDto): Promise<string> {
  const res = await axios.post("/api/Auth/login", data);
  return res.data;
}

export async function registerUser(data: UserRegisterDto): Promise<UserDto> {
  const res = await axios.post("/api/Auth/register", data);
  return res.data;
}
