import { createContext } from "react";

export interface GoogleAuthContextType {
  isPending: boolean;
  showGoogleSignIn: () => void;
  showOneTapPrompt: () => void;
}

export const GoogleAuthContext = createContext<
  GoogleAuthContextType | undefined
>(undefined);
