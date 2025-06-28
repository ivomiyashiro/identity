import { failure, success } from "../utils/result.util";
import { ProblemDetailsError } from "./api-error.http";
import type { ApiClientConfig, ApiResult, RequestConfig } from "./types.http";
import { handleErrorResponse, parseSuccessResponse } from "./utilts.http";

class HttpClient {
    private readonly baseURL: string;
    private readonly defaultOptions: ApiClientConfig;

    constructor(baseURL: string = "/api", defaultOptions: ApiClientConfig = {}) {
        this.baseURL = baseURL;
        this.defaultOptions = {
            headers: {
                "Content-Type": "application/json",
                Accept: "application/json, application/problem+json",
            },
            ...defaultOptions,
        };
    }

    async request<T = unknown>(
        endpoint: string,
        options: RequestConfig = {}
    ): Promise<T> {
        const url = `${this.baseURL}${endpoint}`;
        const config: RequestConfig = {
            ...this.defaultOptions,
            ...options,
            headers: {
                ...this.defaultOptions.headers,
                ...options.headers,
            },
        };

        try {
            const response = await fetch(url, config);

            if (!response.ok) {
                await handleErrorResponse(response);
            }

            // Handle successful responses
            return await parseSuccessResponse<T>(response);
        } catch (error) {
            if (error instanceof ProblemDetailsError) {
                throw error;
            }

            throw new Error(`Network error: ${(error as Error).message}`);
        }
    }

    // HTTP methods with proper typing
    async get<T = unknown>(
        endpoint: string,
        options: Omit<RequestConfig, "method" | "body"> = {}
    ): Promise<ApiResult<T>> {
        try {
            const response = await this.request<T>(endpoint, {
                ...options,
                method: "GET",
            });
            return success(response);
        } catch (error) {
            return failure(error as Error | ProblemDetailsError);
        }
    }

    async post<D = unknown, T = unknown>(
        endpoint: string,
        data?: D,
        options: Omit<RequestConfig, "method" | "body"> = {}
    ): Promise<ApiResult<T>> {
        try {
            const response = await this.request<T>(endpoint, {
                ...options,
                method: "POST",
                body: data ? JSON.stringify(data) : undefined,
            });
            return success(response);
        } catch (error) {
            return failure(error as Error | ProblemDetailsError);
        }
    }

    async put<D = unknown, T = unknown>(
        endpoint: string,
        data?: D,
        options: Omit<RequestConfig, "method" | "body"> = {}
    ): Promise<ApiResult<T>> {
        try {
            const response = await this.request<T>(endpoint, {
                ...options,
                method: "PUT",
                body: data ? JSON.stringify(data) : undefined,
            });
            return success(response);
        } catch (error) {
            return failure(error as Error | ProblemDetailsError);
        }
    }

    async patch<D = unknown, T = unknown>(
        endpoint: string,
        data?: D,
        options: Omit<RequestConfig, "method" | "body"> = {}
    ): Promise<ApiResult<T>> {
        try {
            const response = await this.request<T>(endpoint, {
                ...options,
                method: "PATCH",
                body: data ? JSON.stringify(data) : undefined,
            });
            return success(response);
        } catch (error) {
            return failure(error as Error | ProblemDetailsError);
        }
    }

    async delete<T = unknown>(
        endpoint: string,
        options: Omit<RequestConfig, "method" | "body"> = {}
    ): Promise<ApiResult<T>> {
        try {
            const response = await this.request<T>(endpoint, {
                ...options,
                method: "DELETE",
            });
            return success(response);
        } catch (error) {
            return failure(error as Error | ProblemDetailsError);
        }
    }
}

export const httpClient = new HttpClient();
