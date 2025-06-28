import { httpClient } from "@/lib/http/base.http";
import type { ApiResult } from "@/lib/http/types.http";

type LoginRequest = {
    email: string;
    password: string;
};

type LoginResponse = {
    fullName: string;
    email: string;
    password: string;
};

export const login = async (data: LoginRequest): Promise<ApiResult<LoginResponse>> => {
    return await httpClient.post("/auth/login", data);
};
