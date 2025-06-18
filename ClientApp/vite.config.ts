import { defineConfig } from "vite";
import react from "@vitejs/plugin-react-swc";
import { env } from "process";
import * as fs from "fs";
import path from "path";
import child_process from "child_process";

// Get the certificate for the development server
const baseFolder =
  process.env.APPDATA !== undefined && process.env.APPDATA !== ""
    ? `${process.env.APPDATA}/ASP.NET/https`
    : `${process.env.HOME}/.aspnet/https`;

// Create the certificate folder if it doesn't exist
if (!fs.existsSync(baseFolder)) {
  fs.mkdirSync(baseFolder, { recursive: true });
}

// Create the certificate file if it doesn't exist
const certificateName = "fitnessapp.client";
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
  if (
    0 !==
    child_process.spawnSync(
      "dotnet",
      [
        "dev-certs",
        "https",
        "--export-path",
        certFilePath,
        "--format",
        "Pem",
        "--no-password",
      ],
      { stdio: "inherit" }
    ).status
  ) {
    throw new Error("Could not create certificate.");
  }
}

// Get the target URL for the proxy
const target = env.ASPNETCORE_HTTPS_PORT
  ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
  : env.ASPNETCORE_URLS
  ? env.ASPNETCORE_URLS.split(";")[0]
  : "https://localhost:7255";

export default defineConfig(({ mode }) => ({
  plugins: [react()],
  server: {
    proxy: {
      "/api": {
        target,
        secure: false,
      },
    },
    https:
      mode === "development"
        ? {
            key: fs.readFileSync(keyFilePath),
            cert: fs.readFileSync(certFilePath),
          }
        : undefined,
  },
  build: {
    outDir: "../ClientApp/wwwroot",
    emptyOutDir: true,
  },
}));
