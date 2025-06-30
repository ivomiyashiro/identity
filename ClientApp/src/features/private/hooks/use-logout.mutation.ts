import { AUTH_CACHE_KEY } from "@/features/auth/hooks/cache/use-me.cache";
import { httpClient } from "@/lib/http/base.http";
import { useMutation, useQueryClient } from "@tanstack/react-query";

export const useLogoutMutation = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: () => {
      return httpClient.post("/auth/logout");
    },
    onSuccess: () => {
      window.location.reload();
      queryClient.invalidateQueries({ queryKey: [AUTH_CACHE_KEY] });
    },
  });
};
