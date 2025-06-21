import { Navigate, Outlet, useLocation } from "react-router";
import { useAuthStore } from "@/features/auth/store/auth.store";

export const PrivateRoute = ({ redirectTo = "/login" }: { redirectTo?: string }) => {
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
  const location = useLocation();

  if (!isAuthenticated) {
    return <Navigate to={redirectTo} state={{ from: location }} replace />;
  }

  return (
    <>
      <Outlet />
    </>
  );
};
