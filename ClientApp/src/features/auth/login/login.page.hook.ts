import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { loginSchema, type LoginSchema } from "./login.schema";
import { login } from "./login.api";
import { useCommand } from "@/hooks/use-command";
import { toast } from "sonner";
import { useNavigate } from "react-router";

export const useLoginPage = () => {
    const navigate = useNavigate();

    const form = useForm<LoginSchema>({
        resolver: zodResolver(loginSchema),
        defaultValues: {
            email: "",
            password: "",
        },
    });

    const { mutate: loginUser, isLoading } = useCommand(
        async (data: LoginSchema) => await login(data),
        {
            onSuccess: () => {
                navigate("/");
            },
            onError: (error) => {
                toast.error(error.message);
            },
        }
    );

    const onSubmit = async (data: LoginSchema) => {
        await loginUser(data);
    };

    return { form, onSubmit, isLoading };
};
