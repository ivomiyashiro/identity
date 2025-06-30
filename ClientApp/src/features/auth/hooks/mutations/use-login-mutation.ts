import { useMutation } from "@tanstack/react-query";
import { login } from "../../api/login.api";
import type { LoginSchema } from "../../validation/login.schema";

export const useLoginMutation = ({
  onSuccess,
  onError,
}: {
  onSuccess: () => void;
  onError: (error: Error) => void;
}) => {
  return useMutation({
    mutationFn: async (data: LoginSchema) => await login(data),
    onSuccess,
    onError,
  });
};
