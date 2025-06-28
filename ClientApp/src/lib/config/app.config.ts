import { z } from "zod";

// Define the configuration schema
const configSchema = z.object({
    httpClient: z.object({
        timeout: z.number().min(1000, "Timeout must be at least 1000ms").default(10000),
    }),
    environment: z.enum(["development", "production", "test"]).default("development"),
});

// Define the type from the schema
export type AppConfig = z.infer<typeof configSchema>;

// Parse and validate environment variables
const parseConfig = (): AppConfig => {
    try {
        return configSchema.parse({
            httpClient: {
                timeout: import.meta.env.VITE_HTTP_TIMEOUT,
            },
            environment: import.meta.env.VITE_ENVIRONMENT || import.meta.env.MODE,
        });
    } catch (error) {
        if (error instanceof z.ZodError) {
            error.errors.forEach((err) => {
                console.error(`- ${err.path.join(".")}: ${err.message}`);
            });
            throw new Error(
                "Invalid configuration. Please check your environment variables."
            );
        }
        throw error;
    }
};

// Export the validated configuration
export const config = parseConfig();
