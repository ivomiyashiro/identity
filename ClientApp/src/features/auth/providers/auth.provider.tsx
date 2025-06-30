import { Loader2 } from "lucide-react";
import { useMeQuery } from "../hooks/queries/use-me.query";

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const { isLoading } = useMeQuery();

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-screen gap-2">
        <Loader2 className="animate-spin" /> Loading...
      </div>
    );
  }

  return <div>{children}</div>;
};
