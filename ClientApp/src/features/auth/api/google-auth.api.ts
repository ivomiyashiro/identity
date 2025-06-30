import { httpClient } from "@/lib/http/base.http";
import type { ApiResult } from "@/lib/http/types.http";

type GoogleAuthRequest = {
  googleToken: string;
};

export type GoogleAuthResponse = {
  id: string;
  email: string;
  fullName: string;
  createdAt: Date;
};

export const googleAuth = async (
  data: GoogleAuthRequest
): Promise<ApiResult<GoogleAuthResponse>> => {
  return await httpClient.post("/auth/google", data);
};
