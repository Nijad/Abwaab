import React from "react";
import { createBrowserRouter, RouterProvider } from "react-router";

// Layouts
import MainLayout from "../layouts/MainLayout";
import AdminLayout from "../layouts/AdminLayout";

// Protection Guard
// import { ProtectedRoute } from "./ProtectedRoute";

// Public Pages
import Home from "../pages/Home";
import Properties from "../pages/Properties";
import Login from "../pages/Login";
import PropertyDetails from "../pages/PropertyDetails";

// Dashboard Pages
// import { OverviewPage } from "../pages/dashboard/OverviewPage";
// import { MyPropertiesPage } from "../pages/dashboard/MyPropertiesPage";
// import { ManageUsersPage } from "../pages/dashboard/ManageUsersPage";

const router = createBrowserRouter([
  // ----------------------------------------------------------------------
  // 1. PUBLIC LAYOUT GROUP
  // ----------------------------------------------------------------------
  {
    path: "/",
    element: <MainLayout />,
    children: [
      { index: true, element: <Home /> },
      { path: "properties", element: <Properties /> },
      { path: "properties/:id", element: <PropertyDetails /> },
      { path: "login", element: <Login /> },
    ],
  },

  // ----------------------------------------------------------------------
  // 2. DASHBOARD LAYOUT GROUP (Protected)
  // ----------------------------------------------------------------------
  // {
  //   path: "/dashboard",
  //   element: (
  //     <ProtectedRoute>
  //       <AdminLayout />
  //     </ProtectedRoute>
  //   ),
  //   children: [
  //     { index: true, element: <OverviewPage /> },
  //     { path: "my-properties", element: <MyPropertiesPage /> },
  //     {
  //       path: "users",
  //       element: (
  //         <ProtectedRoute allowedRoles={["Admin"]}>
  //           <ManageUsersPage />
  //         </ProtectedRoute>
  //       ),
  //     },
  //   ],
  // },

  // ----------------------------------------------------------------------
  // 3. FALLBACK / 404 ROUTE
  // ----------------------------------------------------------------------
  {
    path: "*",
    element: (
      <div className="flex h-screen items-center justify-center text-xl font-bold">
        404 - الصفحة غير موجودة
      </div>
    ),
  },
]);

export const AppRouter = () => {
  return <RouterProvider router={router} />;
};
