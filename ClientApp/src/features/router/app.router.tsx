import { createBrowserRouter, RouterProvider } from "react-router";

import { PublicRoute } from "./public.route";
import { PrivateRoute } from "./private.route";

const router = createBrowserRouter([
  {
    element: <PublicRoute redirectIfAuthenticated />,
    children: [
      {
        path: "/",
        lazy: () =>
          import("@/features/landing/pages/landing.page").then((module) => ({
            element: <module.default />,
          })),
      },
      {
        path: "/login",
        lazy: () =>
          import("@/features/auth/pages/login.page").then((module) => ({
            element: <module.default />,
          })),
      },
      {
        path: "/register",
        lazy: () =>
          import("@/features/auth/pages/register.page").then((module) => ({
            element: <module.default />,
          })),
      },
    ],
  },
  {
    element: <PrivateRoute />,
    children: [
      {
        path: "/private",
        lazy: () =>
          import("@/features/private/pages/private.page").then((module) => ({
            element: <module.default />,
          })),
      },
    ],
  },
]);

export const Router = () => {
  return <RouterProvider router={router} />;
};
