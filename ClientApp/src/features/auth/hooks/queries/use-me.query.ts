import { useQuery } from "@tanstack/react-query";
import { AUTH_CACHE_KEY } from "../cache/use-me.cache";
import { httpClient } from "@/lib/http/base.http";

type GetMeResponse = {
  id: string;
  email: string;
  name: string;
  createdAt: Date;
};

export const useMeQuery = () => {
  return useQuery({
    queryKey: [AUTH_CACHE_KEY],
    queryFn: async () => {
      const result = await httpClient.get<GetMeResponse>("/auth/me");
      return result.data;
    },
  });
};
