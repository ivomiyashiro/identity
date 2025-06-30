import { useQueryClient } from "@tanstack/react-query";
import type { User } from "@/types/user.type";

export const AUTH_CACHE_KEY = "auth-user";

export const useMeCache = () => {
  const queryClient = useQueryClient();
  return queryClient.getQueryData<User>([AUTH_CACHE_KEY]);
};
