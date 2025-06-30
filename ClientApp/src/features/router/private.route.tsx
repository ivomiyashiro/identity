import { Navigate, Outlet, useLocation } from "react-router";
import { useMeCache } from "../auth/hooks/cache/use-me.cache";

export const PrivateRoute = ({
  redirectTo = "/login",
}: {
  redirectTo?: string;
}) => {
  const user = useMeCache();
  const location = useLocation();

  if (!user) {
    return <Navigate to={redirectTo} state={{ from: location }} replace />;
  }

  return (
    <>
      <Outlet />
    </>
  );
};
