import React from "react";
import { createBrowserRouter, RouterProvider } from "react-router";

// Layouts
import { MainLayout } from "../layouts/MainLayout";
import { AdminLayout } from "../layouts/AdminLayout";

// Protection Guard
import { ProtectedRoute } from "./ProtectedRoute";

// Public Pages
import { HomePage } from "../pages/public/HomePage";
import { PropertiesPage } from "../pages/public/PropertiesPage";
import { PropertyDetailsPage } from "../pages/public/PropertyDetailsPage";
import { LoginPage } from "../pages/public/LoginPage";

// Dashboard Pages
import { OverviewPage } from "../pages/dashboard/OverviewPage";
import { MyPropertiesPage } from "../pages/dashboard/MyPropertiesPage";
import { ManageUsersPage } from "../pages/dashboard/ManageUsersPage";

const router = createBrowserRouter([
  // ----------------------------------------------------------------------
  // 1. PUBLIC LAYOUT GROUP
  // ----------------------------------------------------------------------
  {
    path: "/",
    element: <MainLayout />,
    children: [
      { index: true, element: <HomePage /> },
      { path: "properties", element: <PropertiesPage /> },
      { path: "properties/:id", element: <PropertyDetailsPage /> },
      { path: "login", element: <LoginPage /> },
    ],
  },

  // ----------------------------------------------------------------------
  // 2. DASHBOARD LAYOUT GROUP (Protected)
  // ----------------------------------------------------------------------
  {
    path: "/dashboard",
    element: (
      <ProtectedRoute>
        <AdminLayout />
      </ProtectedRoute>
    ),
    children: [
      { index: true, element: <OverviewPage /> },
      { path: "my-properties", element: <MyPropertiesPage /> },
      {
        path: "users",
        element: (
          <ProtectedRoute allowedRoles={["Admin"]}>
            <ManageUsersPage />
          </ProtectedRoute>
        ),
      },
    ],
  },

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
