import { Link } from "react-router";
import { PageLayout } from "@/components/layouts/page.layout";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";

const LoginPage = () => {
  return (
    <PageLayout title="Login" description="Login to your account">
      <div className="flex flex-col gap-8 md:max-w-md w-full">
        <div className="flex flex-col gap-1">
          <h1 className="text-2xl">Log in to your account.</h1>
          <p className="text-muted-foreground">
            Enter your email and password to login to your account
          </p>
        </div>
        <form className="flex flex-col">
          <div className="flex flex-col gap-4">
            <div className="flex flex-col gap-2">
              <Label>Email</Label>
              <Input type="email" placeholder="Email" />
            </div>
            <div className="flex flex-col gap-2">
              <Label>Password</Label>
              <Input type="password" placeholder="Password" />
              <Link
                to="/forgot-password"
                className="text-xs hover:underline text-primary self-end"
              >
                Forgot password?
              </Link>
            </div>
          </div>
          <div className="flex flex-col gap-2 mt-6">
            <Button type="submit">Log In</Button>
            <Button type="submit" variant="outline" className="w-full">
              Log In With Google
            </Button>
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
