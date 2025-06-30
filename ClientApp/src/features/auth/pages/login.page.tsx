import { useState } from "react";
import { Link } from "react-router";
import {
  EyeIcon,
  EyeOffIcon,
  Loader2Icon,
  LockIcon,
  MailIcon,
} from "lucide-react";

import { useLoginPage } from "./login.page.hook";

import { PageLayout } from "@/components/layouts/page.layout";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { InputError } from "@/components/ui/input-error";
import { GoogleAuthButton } from "@/features/auth/components/google-auth-button";

const LoginPage = () => {
  const [showPassword, setShowPassword] = useState(false);
  const { form, onSubmit, isPending } = useLoginPage();

  return (
    <PageLayout title="Login" description="Login to your account">
      <div className="flex flex-col gap-8 md:max-w-sm w-full">
        <div className="flex flex-col gap-1">
          <h1 className="text-2xl">Log in to your account.</h1>
          <p className="text-muted-foreground">
            Enter your email and password to login to your account
          </p>
        </div>
        <form className="flex flex-col" onSubmit={form.handleSubmit(onSubmit)}>
          <div className="flex flex-col gap-4">
            <div className="flex flex-col gap-2">
              <Label>Email</Label>
              <Input
                icon={MailIcon}
                type="email"
                placeholder="Email"
                {...form.register("email")}
              />
              {form.formState.errors.email && (
                <InputError error={form.formState.errors.email.message} />
              )}
            </div>
            <div className="flex flex-col gap-2">
              <Label>Password</Label>
              <Input
                icon={LockIcon}
                type={showPassword ? "text" : "password"}
                placeholder="Password"
                actionIcon={showPassword ? <EyeOffIcon /> : <EyeIcon />}
                action={() => {
                  setShowPassword(!showPassword);
                }}
                {...form.register("password")}
              />
              {form.formState.errors.password && (
                <InputError error={form.formState.errors.password.message} />
              )}
              <Link
                to="/forgot-password"
                className="text-xs hover:underline text-primary self-end"
              >
                Forgot password?
              </Link>
            </div>
          </div>
          <div className="flex flex-col gap-2 mt-6">
            <Button type="submit" disabled={isPending}>
              {isPending ? (
                <>
                  <Loader2Icon className="animate-spin" />
                  <span>Logging in...</span>
                </>
              ) : (
                "Log In"
              )}
            </Button>
            <GoogleAuthButton />
          </div>
        </form>
        <div className="flex flex-col gap-2">
          <p className="text-muted-foreground text-center text-sm">
            Don't have an account?{" "}
            <Link to="/register" className="text-primary hover:underline">
              Register
            </Link>
          </p>
        </div>
      </div>
    </PageLayout>
  );
};

export default LoginPage;
