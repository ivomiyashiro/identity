import { useEffect, useState } from "react";
import { useGoogleAuth } from "@/features/auth/google-auth/google-auth-popup.hook";

export const GoogleSignInPopup = () => {
  const { showGoogleSignInPopup, isLoading } = useGoogleAuth();
  const [isGoogleLoaded, setIsGoogleLoaded] = useState(false);

  useEffect(() => {
    // Check if Google Sign-In is loaded
    const checkGoogleLoaded = () => {
      if (typeof window !== "undefined" && window.google) {
        setIsGoogleLoaded(true);
      } else {
        // Retry after a short delay
        setTimeout(checkGoogleLoaded, 100);
      }
    };

    checkGoogleLoaded();
  }, []);

  useEffect(() => {
    if (isGoogleLoaded && !isLoading) {
      showGoogleSignInPopup();
    }
  }, [isGoogleLoaded, isLoading, showGoogleSignInPopup]);

  return null;
};
