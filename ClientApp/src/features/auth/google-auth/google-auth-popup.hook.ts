import { useCallback } from "react";
import { toast } from "sonner";
import { useNavigate } from "react-router";
import { googleAuth } from "./google-auth.api";
import { useCommand } from "@/hooks/use-command";
import { config } from "@/lib/config/app.config";

interface GoogleCredentialResponse {
  credential: string;
  select_by: string;
}

export const useGoogleAuth = () => {
  const navigate = useNavigate();

  const { mutate: authenticateWithGoogle, isLoading } = useCommand(
    async (credential: string) => await googleAuth({ googleToken: credential }),
    {
      onSuccess: () => navigate("/"),
      onError: (error) => {
        toast.error(error.message || "Google authentication failed");
      },
    }
  );

  const initializeGoogleSignIn = useCallback(() => {
    if (typeof window === "undefined" || !window.google) {
      return;
    }

    window.google.accounts.id.initialize({
      client_id: config.google.clientId,
      callback: (response: GoogleCredentialResponse) => {
        authenticateWithGoogle(response.credential);
      },
      auto_select: false,
      cancel_on_tap_outside: true,
      use_fedcm_for_prompt: false,
    });
  }, [authenticateWithGoogle]);

  const showGoogleSignInPopup = useCallback(() => {
    if (!config.google.clientId) {
      toast.error(
        "Google Client ID not configured. Please check your environment variables."
      );
      return;
    }

    if (typeof window === "undefined" || !window.google) {
      return;
    }

    // Initialize if not already done
    initializeGoogleSignIn();

    // Trigger the sign-in popup
    window.google.accounts.id.prompt((notification) => {
      if (notification.isNotDisplayed()) {
        toast.error(
          "Google Sign-In popup was blocked. Please allow popups or try clicking the button again."
        );
      }
    });
  }, [initializeGoogleSignIn]);

  return {
    showGoogleSignInPopup,
    isLoading,
  };
};
