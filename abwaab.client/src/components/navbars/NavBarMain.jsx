import React from "react";
import { NavLink } from "react-router";

const NavBarMain = () => {
  return (
    <div>
      NavBarMain
      <NavLink to={"login"} end className={"mx-3"}>
        Login
      </NavLink>
    </div>
  );
};

export default NavBarMain;
