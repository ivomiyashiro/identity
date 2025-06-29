import { z } from "zod";

// Define the configuration schema
const configSchema = z.object({
  environment: z
    .enum(["development", "production", "test"])
    .default("development"),
  google: z.object({
    clientId: z.string().min(1, "Google client ID is required"),
  }),
});

// Define the type from the schema
export type AppConfig = z.infer<typeof configSchema>;

// Parse and validate environment variables
const parseConfig = (): AppConfig => {
  try {
    return configSchema.parse({
      environment: import.meta.env.VITE_ENVIRONMENT || import.meta.env.MODE,
      google: {
        clientId: import.meta.env.VITE_GOOGLE_CLIENT_ID,
      },
    });
  } catch (error) {
    if (error instanceof z.ZodError) {
      error.errors.forEach((err) => {
        console.error(`- ${err.path.join(".")}: ${err.message}`);
      });
      console.error(error);
      throw new Error(
        "Invalid configuration. Please check your environment variables."
      );
    }
    throw error;
  }
};

// Export the validated configuration
export const config = parseConfig();
