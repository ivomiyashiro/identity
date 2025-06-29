import { useEffect, useState } from "react";
import { toast } from "sonner";
import { useNavigate } from "react-router";
import { googleAuth } from "@/features/auth/google-auth/google-auth.api";
import { useCommand } from "@/hooks/use-command";
import { config } from "@/lib/config/app.config";

export const useGoogleAuthButton = () => {
  const navigate = useNavigate();
  const [isGoogleLoaded, setIsGoogleLoaded] = useState(false);

  const { mutate: authenticateWithGoogle, isLoading } = useCommand(
    async (credential: string) => await googleAuth({ googleToken: credential }),
    {
      onSuccess: (result) => {
        if (result.isNewUser) {
          toast.success("Account created successfully! Welcome!");
        } else {
          toast.success("Welcome back!");
        }
        navigate("/");
      },
      onError: (error) => {
        toast.error(error.message || "Google authentication failed");
      },
    }
  );

  useEffect(() => {
    const checkGoogleLoaded = () => {
      if (
        typeof window !== "undefined" &&
        window.google &&
        config.google.clientId
      ) {
        // Initialize Google Sign-In
        try {
          window.google.accounts.id.initialize({
            client_id: config.google.clientId,
            callback: (response: { credential: string }) => {
              authenticateWithGoogle(response.credential);
            },
            auto_select: false,
            cancel_on_tap_outside: true,
            use_fedcm_for_prompt: false,
          });
          setIsGoogleLoaded(true);
        } catch (error) {
          console.error("Failed to initialize Google Sign-In:", error);
          toast.error("Failed to initialize Google Sign-In");
        }
      } else if (!config.google.clientId) {
        toast.error("Google Client ID not configured");
      } else {
        setTimeout(checkGoogleLoaded, 100);
      }
    };

    checkGoogleLoaded();
  }, [authenticateWithGoogle]);

  const handleGoogleSignIn = () => {
    if (!isGoogleLoaded) {
      toast.error("Google Sign-In is not ready yet. Please try again.");
      return;
    }

    if (typeof window !== "undefined" && window.google) {
      window.google.accounts.id.prompt((notification) => {
        if (notification.isNotDisplayed()) {
          toast.error(
            "Google Sign-In popup was blocked. Please allow popups or try clicking the button again."
          );
        } else if (notification.isSkippedMoment()) {
          toast.error(
            "Google Sign-In was cancelled or timed out. Please try again."
          );
        }
      });
    }
  };

  return {
    handleGoogleSignIn,
    isLoading: isLoading || !isGoogleLoaded,
  };
};
