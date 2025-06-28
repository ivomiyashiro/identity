import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { registerSchema, type RegisterSchema } from "./register.schema";
import { useCommand } from "@/hooks/use-command";
import { toast } from "sonner";
import { useNavigate } from "react-router";
import { register } from "./register.api";

export const useRegisterPage = () => {
    const navigate = useNavigate();

    const form = useForm<RegisterSchema>({
        resolver: zodResolver(registerSchema),
        defaultValues: {
            fullName: "",
            email: "",
            password: "",
            confirmPassword: "",
        },
    });

    const { mutate: registerUser, isLoading } = useCommand(
        async (data: RegisterSchema) => await register(data),
        {
            onSuccess: () => {
                navigate("/login");
            },
            onError: (error) => {
                toast.error(error.message);
            },
        }
    );

    const onSubmit = async (data: RegisterSchema) => {
        await registerUser(data);
    };

    return { form, onSubmit, isLoading };
};
