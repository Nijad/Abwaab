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
import ResetPassword from "../pages/ResetPassword";
import ProtectedRoute from "./ProtectedRoute";
import UserLayout from "../layouts/UserLayout";
import Profile from "../pages/Profile";
import MyProperties from "../pages/MyProperties";
import ManageUsers from "../pages/ManageUsers";
import Admin from "../pages/Admin";
import Register from "../pages/Register";
import ConfirmRegisteration from "../pages/ConfirmRegisteration";
import NoLayout from "../layouts/NoLayout";
import Subscriptions from "../pages/Subscriptions";
import AboutUs from "../pages/AboutUs";

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
      { path: "subscriptions", element: <Subscriptions /> },
      { path: "about-us", element: <AboutUs /> },
      { path: "test", element: <Profile /> },
    ],
  },
  {
    path: "/",
    element: <NoLayout />,
    children: [
      { path: "login", element: <Login /> },
      { path: "register", element: <Register /> },
      { path: "confirm-registeration", element: <ConfirmRegisteration /> },
      { path: "reset-password", element: <ResetPassword /> },
    ],
  },

  // ----------------------------------------------------------------------
  // 2. DASHBOARD LAYOUT GROUP (Protected)
  // ----------------------------------------------------------------------
  {
    path: "/admin",
    element: (
      <ProtectedRoute isAdminRoute={true}>
        <AdminLayout />
      </ProtectedRoute>
    ),
    children: [
      { index: true, element: <Admin /> },
      {
        path: "users",
        element: <ManageUsers />,
      },
    ],
  },

  // ----------------------------------------------------------------------
  // 3. FALLBACK / 404 ROUTE
  // ----------------------------------------------------------------------

  {
    path: "/portal",
    element: (
      <ProtectedRoute isAdminRoute={false}>
        <UserLayout />
      </ProtectedRoute>
    ),
    children: [
      { index: true, element: <Home /> },
      { path: "profile", element: <Profile /> },
      { path: "properties", element: <Properties /> },
      { path: "properties/:id", element: <PropertyDetails /> },
      { path: "my-properties", element: <MyProperties /> },
      { path: "my-properties/:id", element: <PropertyDetails /> },
      { path: "profile", element: <Profile /> },
    ],
  },
  // ----------------------------------------------------------------------
  // 4. FALLBACK / 404 ROUTE
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
