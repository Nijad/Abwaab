import React from "react";
import { Navigate, Outlet } from "react-router";
import NavBarMain from "../components/navbars/NavBarMain";
import Footer from "../components/Footer";

const UserLayout = () => {
  return (
    <div className="flex flex-col min-h-screen">
      <div className="">
        <NavBarMain />
      </div>
      <div className="flex-1 bg-neutral-50">
        <Outlet />
      </div>
      <div className="">
        <Footer />
      </div>
    </div>
  );
};

export default UserLayout;
