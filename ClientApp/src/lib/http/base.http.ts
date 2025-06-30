import { tryCatch, type Result } from "../utils/try-catch.util";
import type {
  ApiClientConfig,
  ProblemDetails,
  RequestConfig,
} from "./types.http";
import { parseSuccessResponse } from "./utilts.http";

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
  ): Promise<Result<T, Error>> {
    const url = `${this.baseURL}${endpoint}`;
    const config: RequestConfig = {
      ...this.defaultOptions,
      ...options,
      headers: {
        ...this.defaultOptions.headers,
        ...options.headers,
      },
    };

    const { data, error } = await tryCatch(fetch(url, config));

    if (error) {
      return { data: null, error };
    }

    const { data: parsedData, error: parsedError } = await tryCatch(
      parseSuccessResponse<T>(data)
    );

    if (parsedError) {
      return { data: null, error: parsedError };
    }

    if (!data.ok) {
      const problemDetails = parsedData as ProblemDetails;
      return { data: null, error: new Error(problemDetails.detail) };
    }

    return { data: parsedData, error: null };
  }

  // HTTP methods with proper typing
  async get<T = unknown>(
    endpoint: string,
    options: Omit<RequestConfig, "method" | "body"> = {}
  ): Promise<Result<T, Error>> {
    return await this.request<T>(endpoint, {
      ...options,
      method: "GET",
    });
  }

  async post<D = unknown, T = unknown>(
    endpoint: string,
    data?: D,
    options: Omit<RequestConfig, "method" | "body"> = {}
  ): Promise<Result<T, Error>> {
    return await this.request<T>(endpoint, {
      ...options,
      method: "POST",
      body: data ? JSON.stringify(data) : undefined,
    });
  }

  async put<D = unknown, T = unknown>(
    endpoint: string,
    data?: D,
    options: Omit<RequestConfig, "method" | "body"> = {}
  ): Promise<Result<T, Error>> {
    return await this.request<T>(endpoint, {
      ...options,
      method: "PUT",
      body: data ? JSON.stringify(data) : undefined,
    });
  }

  async patch<D = unknown, T = unknown>(
    endpoint: string,
    data?: D,
    options: Omit<RequestConfig, "method" | "body"> = {}
  ): Promise<Result<T, Error>> {
    return await this.request<T>(endpoint, {
      ...options,
      method: "PATCH",
      body: data ? JSON.stringify(data) : undefined,
    });
  }

  async delete<T = unknown>(
    endpoint: string,
    options: Omit<RequestConfig, "method" | "body"> = {}
  ): Promise<Result<T, Error>> {
    return await this.request<T>(endpoint, {
      ...options,
      method: "DELETE",
    });
  }
}

export const httpClient = new HttpClient();
