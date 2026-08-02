import React from "react";
import { Outlet } from "react-router";
import NavBarMain from "../components/navbars/NavBarMain";
import Footer from "../components/Footer";

const MainLayout = () => {
  return (
    <div className="flex flex-col">
      <div className="">
        <NavBarMain />
      </div>
      <div className="flex-1">
        <Outlet />
      </div>
      <div className="">
        <Footer />
      </div>
    </div>
  );
};

export default MainLayout;
