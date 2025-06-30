import { httpClient } from "@/lib/http/base.http";
import type { ApiResult } from "@/lib/http/types.http";

export const logout = async (): Promise<ApiResult<void>> => {
  return await httpClient.post("/auth/logout");
};
