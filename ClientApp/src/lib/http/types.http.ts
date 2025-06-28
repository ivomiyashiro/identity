import type { Result } from "../utils/result.util";
import type { ProblemDetailsError } from "./api-error.http";

// Problem Details types (RFC 7807)
export interface ProblemDetails {
    type?: string;
    title?: string;
    status?: number;
    detail?: string;
    instance?: string;
    [key: string]: unknown; // For extensions
}

// API Client configuration
export interface ApiClientConfig {
    headers?: Record<string, string>;
    timeout?: number;
    [key: string]: unknown;
}

export interface RequestConfig extends RequestInit {
    headers?: Record<string, string>;
}

// Response types
export type ApiResult<T = unknown> = Result<T, Error | ProblemDetailsError>;

// Error types
export interface ValidationError {
    [field: string]: string[];
}

export interface ProblemDetailsExtensions {
    errors?: ValidationError;
    [key: string]: unknown;
}
