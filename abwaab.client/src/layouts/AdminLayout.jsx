import React from "react";
import NavBarMain from "../components/navbars/NavBarMain";
import { Outlet } from "react-router";
import Footer from "../components/Footer";

const AdminLayout = () => {
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

export default AdminLayout;
