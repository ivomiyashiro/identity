import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { loginSchema, type LoginSchema } from "../validation/login.schema";
import { toast } from "sonner";
import { useNavigate } from "react-router";
import { useLoginMutation } from "../hooks/mutations/use-login-mutation";

export const useLoginPage = () => {
  const navigate = useNavigate();

  const { mutate: loginUser, isPending } = useLoginMutation({
    onSuccess: () => {
      navigate("/");
    },
    onError: (error) => {
      toast.error(error.message);
    },
  });

  const form = useForm<LoginSchema>({
    resolver: zodResolver(loginSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  });

  const onSubmit = (data: LoginSchema) => {
    loginUser(data);
  };

  return { form, onSubmit, isPending };
};
