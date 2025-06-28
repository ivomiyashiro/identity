import type { ProblemDetails, ProblemDetailsExtensions } from "./types.http";

export class ProblemDetailsError extends Error {
    public readonly type?: string;
    public readonly title?: string;
    public readonly status?: number;
    public readonly detail?: string;
    public readonly instance?: string;
    public readonly extensions: ProblemDetailsExtensions;

    constructor(problemDetails: ProblemDetails) {
        super(problemDetails.title || problemDetails.detail || "API Error");
        this.name = "ProblemDetailsError";
        this.type = problemDetails.type;
        this.title = problemDetails.title;
        this.status = problemDetails.status;
        this.detail = problemDetails.detail;
        this.instance = problemDetails.instance;
        this.extensions = {};

        // Handle extensions (additional properties)
        Object.keys(problemDetails).forEach((key) => {
            if (!["type", "title", "status", "detail", "instance"].includes(key)) {
                this.extensions[key] = problemDetails[key];
            }
        });
    }
}
