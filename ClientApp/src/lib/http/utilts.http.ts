import { ProblemDetailsError } from "./api-error.http";
import type { ProblemDetails } from "./types.http";

const isProblemDetails = (obj: unknown): obj is ProblemDetails => {
    return (
        obj !== null &&
        typeof obj === "object" &&
        ("type" in obj ||
            "title" in obj ||
            "status" in obj ||
            "detail" in obj ||
            "instance" in obj)
    );
};

export const handleErrorResponse = async (response: Response): Promise<never> => {
    const contentType = response.headers.get("content-type");

    // Check if response is Problem Details format
    if (
        contentType?.includes("application/problem+json") ||
        contentType?.includes("application/json")
    ) {
        try {
            const problemDetails: ProblemDetails = await response.json();

            // Validate it's actually a Problem Details response
            if (isProblemDetails(problemDetails)) {
                throw new ProblemDetailsError(problemDetails);
            }
        } catch (parseError) {
            // If JSON parsing fails, fall back to generic error
            if (parseError instanceof ProblemDetailsError) {
                throw parseError;
            }
        }
    }

    // Fallback for non-Problem Details errors
    const errorText = await response.text();
    throw new Error(`HTTP ${response.status}: ${errorText || response.statusText}`);
};

export const parseSuccessResponse = async <T>(response: Response): Promise<T> => {
    const contentType = response.headers.get("content-type");

    if (contentType?.includes("application/json")) {
        return await response.json();
    }

    if (contentType?.includes("text/")) {
        return (await response.text()) as unknown as T;
    }

    return (await response.blob()) as unknown as T;
};
