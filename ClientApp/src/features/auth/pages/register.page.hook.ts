import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  registerSchema,
  type RegisterSchema,
} from "../validation/register.schema";
import { useRegisterMutation } from "../hooks/mutations/use-register.mutation";
import { toast } from "sonner";
import { useNavigate } from "react-router";

export const useRegisterPage = () => {
  const navigate = useNavigate();

  const { mutate: registerUser, isPending } = useRegisterMutation({
    onSuccess: () => {
      navigate("/login");
    },
    onError: (error) => {
      toast.error(error.message);
    },
  });

  const form = useForm<RegisterSchema>({
    resolver: zodResolver(registerSchema),
    defaultValues: {
      fullName: "",
      email: "",
      password: "",
      confirmPassword: "",
    },
  });

  const onSubmit = (data: RegisterSchema) => {
    registerUser(data);
  };

  return { form, onSubmit, isPending };
};
