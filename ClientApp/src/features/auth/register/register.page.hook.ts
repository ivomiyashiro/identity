import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { registerSchema, type RegisterSchema } from "./register.schema";

export const useRegisterPage = () => {
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
    console.log(data);
  };

  return { form, onSubmit };
};
