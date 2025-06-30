import { useMutation } from "@tanstack/react-query";
import { register } from "../../api/register.api";
import type { RegisterSchema } from "../../validation/register.schema";

export const useRegisterMutation = ({
  onSuccess,
  onError,
}: {
  onSuccess: () => void;
  onError: (error: Error) => void;
}) => {
  return useMutation({
    mutationFn: async (data: RegisterSchema) => await register(data),
    onSuccess,
    onError,
  });
};
