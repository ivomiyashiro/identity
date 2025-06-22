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
          import("@/features/landing/landing.page").then((module) => ({
            element: <module.default />,
          })),
      },
      {
        path: "/login",
        lazy: () =>
          import("@/features/auth/login/login.page").then((module) => ({
            element: <module.default />,
          })),
      },
      {
        path: "/register",
        lazy: () =>
          import("@/features/auth/register/register.page").then((module) => ({
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
          import("@/features/private/private.page").then((module) => ({
            element: <module.default />,
          })),
      },
    ],
  },
]);

export const Router = () => {
  return <RouterProvider router={router} />;
};
