// Problem Details types (RFC 7807)
export interface ProblemDetails {
  type: string;
  title: string;
  status: number;
  detail: string;
  instance: string;
  traceId?: string;
  errors?: Record<string, string[]>;
}

export interface ApiClientConfig {
  headers?: Record<string, string>;
  timeout?: number;
  [key: string]: unknown;
}

export interface RequestConfig extends RequestInit {
  headers?: Record<string, string>;
}
