import { Navigate, Outlet } from "react-router";
import { useMeCache } from "../auth/hooks/cache/use-me.cache";

export const PublicRoute = ({
  redirectIfAuthenticated = false,
  redirectTo = "/private",
}: {
  redirectIfAuthenticated?: boolean;
  redirectTo?: string;
}) => {
  const user = useMeCache();

  if (redirectIfAuthenticated && user) {
    return <Navigate to={redirectTo} />;
  }

  return (
    <>
      <Outlet />
    </>
  );
};
