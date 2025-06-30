import { use } from "react";
import {
  GoogleAuthContext,
  type GoogleAuthContextType,
} from "../../contexts/google-auth.context";

export const useGoogleAuth = (): GoogleAuthContextType => {
  const context = use(GoogleAuthContext);
  if (context === undefined) {
    throw new Error("useGoogleAuth must be used within a GoogleAuthProvider");
  }
  return context;
};
