import { Navigate, Outlet } from "react-router";
import { useAuthStore } from "@/features/auth/store/auth.store";

export const PublicRoute = ({
  redirectIfAuthenticated = false,
  redirectTo = "/private",
}: {
  redirectIfAuthenticated?: boolean;
  redirectTo?: string;
}) => {
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);

  if (redirectIfAuthenticated && isAuthenticated) {
    return <Navigate to={redirectTo} />;
  }

  return (
    <>
      <Outlet />
    </>
  );
};
