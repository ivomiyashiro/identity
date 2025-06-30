import { httpClient } from "@/lib/http/base.http";
import type { ApiResult } from "@/lib/http/types.http";

type GetMeResponse = {
  id: string;
  email: string;
  name: string;
  createdAt: Date;
};

export const getMe = async (): Promise<ApiResult<GetMeResponse>> => {
  return await httpClient.get("/auth/me");
};
