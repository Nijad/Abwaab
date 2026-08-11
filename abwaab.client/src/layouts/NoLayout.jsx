import React from "react";
import { Outlet } from "react-router";

const NoLayout = () => {
  return (
    <div className="p-0 m-0 ">
      <Outlet />
    </div>
  );
};

export default NoLayout;
