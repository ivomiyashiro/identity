import { Link } from "react-router";
import { PageLayout } from "@/components/layouts/page.layout";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";

const RegisterPage = () => {
  return (
    <PageLayout title="Register" description="Register to your account">
      <div className="flex flex-col gap-8 md:max-w-md w-full">
        <div className="flex flex-col gap-1">
          <h1 className="text-2xl">Create an account.</h1>
          <p className="text-muted-foreground">
            Let's get you started with your account.
          </p>
        </div>
        <form className="flex flex-col">
          <div className="flex flex-col gap-4">
            <div className="flex flex-col gap-2">
              <Label>Full Name</Label>
              <Input type="text" placeholder="Full Name" />
            </div>
            <div className="flex flex-col gap-2">
              <Label>Email</Label>
              <Input type="email" placeholder="Email" />
            </div>
            <div className="flex flex-col gap-2">
              <Label>Password</Label>
              <Input type="password" placeholder="Password" />
            </div>
            <div className="flex flex-col gap-2">
              <Label>Confirm Password</Label>
              <Input type="password" placeholder="Confirm Password" />
            </div>
          </div>
          <div className="flex flex-col gap-2 mt-6">
            <Button type="submit">Register</Button>
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
