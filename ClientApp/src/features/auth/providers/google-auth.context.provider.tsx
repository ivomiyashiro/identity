import { useEffect, useState, useCallback, type ReactNode } from "react";
import { toast } from "sonner";
import { config } from "@/lib/config/app.config";
import {
  GoogleAuthContext,
  type GoogleAuthContextType,
} from "../contexts/google-auth.context";
import { useGoogleAuthMutation } from "../hooks/mutations/use-google-auth.mutation";
import { useMeCache } from "../hooks/cache/use-me.cache";

interface GoogleCredentialResponse {
  credential: string;
  select_by: string;
}

export const GoogleAuthProvider = ({ children }: { children: ReactNode }) => {
  const [isGoogleLoaded, setIsGoogleLoaded] = useState(false);
  const [isInitialized, setIsInitialized] = useState(false);

  const user = useMeCache();
  const { mutate: authenticateWithGoogle, isPending } = useGoogleAuthMutation();

  const initializeGoogleSignIn = useCallback(() => {
    if (
      typeof window === "undefined" ||
      !window.google ||
      !config.google.clientId ||
      isInitialized
    ) {
      return;
    }

    try {
      window.google.accounts.id.initialize({
        client_id: config.google.clientId,
        callback: (response: GoogleCredentialResponse) => {
          authenticateWithGoogle(response.credential);
        },
        auto_select: false,
        cancel_on_tap_outside: true,
        use_fedcm_for_prompt: false,
      });
      setIsInitialized(true);
      setIsGoogleLoaded(true);
    } catch (error) {
      console.error("Failed to initialize Google Sign-In:", error);
      toast.error("Failed to initialize Google Sign-In");
    }
  }, [authenticateWithGoogle, isInitialized]);

  const showOneTapPrompt = useCallback(() => {
    if (
      !config.google.clientId ||
      !isGoogleLoaded ||
      user ||
      typeof window === "undefined" ||
      !window.google
    ) {
      return;
    }

    window.google.accounts.id.prompt();
  }, [isGoogleLoaded, user]);

  const showGoogleSignIn = useCallback(() => {
    if (
      !config.google.clientId ||
      !isGoogleLoaded ||
      user ||
      typeof window === "undefined" ||
      !window.google
    ) {
      return;
    }

    // Create a temporary div for the Google Sign-In button
    const tempDiv = document.createElement("div");
    tempDiv.style.position = "absolute";
    tempDiv.style.left = "-9999px";
    document.body.appendChild(tempDiv);

    window.google.accounts.id.renderButton(tempDiv, {
      theme: "outline",
      size: "large",
      type: "standard",
      width: 250,
    });

    // Trigger the button click programmatically
    const button = tempDiv.querySelector('div[role="button"]') as HTMLElement;

    if (button) {
      button.click();
      setTimeout(() => document.body.removeChild(tempDiv), 500);
    }
  }, [isGoogleLoaded, user]);

  // Check if Google is loaded and initialize
  useEffect(() => {
    const checkGoogleLoaded = () => {
      if (
        typeof window === "undefined" ||
        !window.google ||
        !config.google.clientId
      ) {
        setTimeout(checkGoogleLoaded, 100);
      }

      initializeGoogleSignIn();
    };

    checkGoogleLoaded();
  }, [initializeGoogleSignIn]);

  // Show One Tap prompt after Google is loaded and initialized
  useEffect(() => {
    if (isGoogleLoaded && !user) {
      // Small delay to ensure everything is ready
      setTimeout(showOneTapPrompt, 500);
    }
  }, [isGoogleLoaded, user, showOneTapPrompt]);

  const value: GoogleAuthContextType = {
    isPending,
    showGoogleSignIn,
    showOneTapPrompt,
  };

  return <GoogleAuthContext value={value}>{children}</GoogleAuthContext>;
};
