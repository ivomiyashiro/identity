import { useMutation } from "@tanstack/react-query";
import { googleAuth } from "../../api/google-auth.api";
import { toast } from "sonner";
import { AUTH_CACHE_KEY } from "../cache/use-me.cache";

export const useGoogleAuthMutation = () => {
  return useMutation({
    mutationKey: [AUTH_CACHE_KEY],
    mutationFn: (googleToken: string) => googleAuth({ googleToken }),
    onSuccess: () => {
      window.location.href = "/";
    },
    onError: (error) => {
      toast.error(error.message || "Google authentication failed");
    },
  });
};
