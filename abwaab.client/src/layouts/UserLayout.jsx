import React from "react";
import { Navigate, Outlet } from "react-router";

const UserLayout = () => {
  return (
    <div>
      <Navigate to={"profile"} replace />
      <Outlet />
    </div>
  );
};

export default UserLayout;
