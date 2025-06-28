import { config } from "./app.config";

export const isDevelopment = () => config.environment === "development";
export const isProduction = () => config.environment === "production";
export const isTest = () => config.environment === "test";
