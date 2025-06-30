import { httpClient } from "@/lib/http/base.http";
import type { RegisterSchema } from "../validation/register.schema";
import type { ApiResult } from "@/lib/http/types.http";

type RegisterResponse = {
  id: string;
  fullName: string;
  email: string;
  createdAt: string;
};

export const register = async (
  data: RegisterSchema
): Promise<ApiResult<RegisterResponse>> => {
  return await httpClient.post("/auth/register", data);
};
