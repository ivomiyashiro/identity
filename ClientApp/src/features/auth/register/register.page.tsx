import { Link } from "react-router";
import { LockIcon, MailIcon, UserIcon } from "lucide-react";

import { useRegisterPage } from "./register.page.hook";

import { PageLayout } from "@/components/layouts/page.layout";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { InputError } from "@/components/ui/input-error";

const RegisterPage = () => {
    const { form, onSubmit, isLoading } = useRegisterPage();

    return (
        <PageLayout title="Register" description="Register to your account">
            <div className="flex flex-col gap-8 md:max-w-md w-full">
                <div className="flex flex-col gap-1">
                    <h1 className="text-2xl">Create an account.</h1>
                    <p className="text-muted-foreground">
                        Let's get you started with your account.
                    </p>
                </div>
                <form className="flex flex-col" onSubmit={form.handleSubmit(onSubmit)}>
                    <div className="flex flex-col gap-4">
                        <div className="flex flex-col gap-2">
                            <Label>Full Name</Label>
                            <Input
                                type="text"
                                placeholder="Full Name"
                                icon={UserIcon}
                                {...form.register("fullName")}
                            />
                            {form.formState.errors.fullName && (
                                <InputError
                                    error={form.formState.errors.fullName.message}
                                />
                            )}
                        </div>
                        <div className="flex flex-col gap-2">
                            <Label>Email</Label>
                            <Input
                                type="email"
                                placeholder="Email"
                                icon={MailIcon}
                                {...form.register("email")}
                            />
                            {form.formState.errors.email && (
                                <InputError error={form.formState.errors.email.message} />
                            )}
                        </div>
                        <div className="flex flex-col gap-2">
                            <Label>Password</Label>
                            <Input
                                type="password"
                                placeholder="Password"
                                icon={LockIcon}
                                {...form.register("password")}
                            />
                            {form.formState.errors.password && (
                                <InputError
                                    error={form.formState.errors.password.message}
                                />
                            )}
                        </div>
                        <div className="flex flex-col gap-2">
                            <Label>Confirm Password</Label>
                            <Input
                                type="password"
                                placeholder="Confirm Password"
                                icon={LockIcon}
                                {...form.register("confirmPassword")}
                            />
                            {form.formState.errors.confirmPassword && (
                                <InputError
                                    error={form.formState.errors.confirmPassword.message}
                                />
                            )}
                        </div>
                    </div>
                    <div className="flex flex-col gap-2 mt-6">
                        <Button type="submit" disabled={isLoading}>
                            {isLoading ? "Registering..." : "Register"}
                        </Button>
                        <Button type="submit" variant="outline" className="w-full">
                            Register With Google
                        </Button>
                    </div>
                </form>
                <div className="flex flex-col gap-2">
                    <p className="text-muted-foreground text-center text-sm">
                        Already have an account?{" "}
                        <Link to="/login" className="text-primary hover:underline">
                            Login
                        </Link>
                    </p>
                </div>
            </div>
        </PageLayout>
    );
};

export default RegisterPage;
