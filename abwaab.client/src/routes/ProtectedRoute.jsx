import React from "react";
import useAuth from "../hooks/useAuth";
import { Navigate } from "react-router";

const ProtectedRoute = ({ children, isAdminRoute }) => {
  const { isAdmin, isAuthenticated } = useAuth();
  if (!isAuthenticated) return <Navigate to="/login" replace />;
  if (isAdminRoute && !isAdmin)
    return <Navigate to={"/unauthorized"} replace />;
  if (!isAdminRoute && !isAdmin) return children;
  //   if (isAdminRoute && isAdmin) return children;
  //   if (!isAdminRoute && isAdmin) return children;

  if (isAdmin & (isAdminRoute || !isAdminRoute)) return children;

  return <div>ProtectedRoute</div>;
};

export default ProtectedRoute;
