import { Link } from "react-router";
import { LockIcon } from "lucide-react";

import { PageLayout } from "@/components/layouts/page.layout";
import { Button } from "@/components/ui/button";

const LandingPage = () => {
  return (
    <PageLayout title="Landing Page" description="Landing Page">
      <div className="p-12 flex flex-col gap-4">
        <div className="flex items-center gap-4">
          <LockIcon className="w-8 h-8" />
          <h1 className="font-extrabold">Welcome to Identity</h1>
        </div>
        <p className="text-lg text-muted-foreground">
          A full-stack web application for managing your identity crafted with .NET 8 and
          React and following Clean Architecture.
        </p>
        <div className="flex gap-4">
          <Link to="/login">
            <Button>Login</Button>
          </Link>
          <Link to="/register">
            <Button variant="outline">Register</Button>
          </Link>
        </div>
      </div>
    </PageLayout>
  );
};

export default LandingPage;
