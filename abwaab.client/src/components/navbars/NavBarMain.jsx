import React, { useState } from "react";
import AppBar from "@mui/material/AppBar";
import Box from "@mui/material/Box";
import Toolbar from "@mui/material/Toolbar";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import Menu from "@mui/material/Menu";
import MenuIcon from "@mui/icons-material/Menu";
import Container from "@mui/material/Container";
import Avatar from "@mui/material/Avatar";
import Button from "@mui/material/Button";
import Tooltip from "@mui/material/Tooltip";
import MenuItem from "@mui/material/MenuItem";
import AdbIcon from "@mui/icons-material/Adb";
import { Link, NavLink, useNavigate } from "react-router";
import logo from "../../assets/imgs/logo.svg";
import useAuth from "../../hooks/useAuth";
import { ChevronLeft } from "@mui/icons-material";
import { replace } from "stylis";

const pages = {
  public: [
    {
      to: "/",
      label: "الرئيسية",
    },
    {
      to: "/properties",
      label: "العقارات",
    },
    {
      to: "/subscriptions",
      label: "الإشتراكات",
    },
    {
      to: "about-us",
      label: "عن أبواب",
    },
  ],
  user: [
    {
      to: "/portal/",
      label: "الرئيسية",
    },
    {
      to: "/portal/properties",
      label: "العقارات",
    },
    {
      to: "/portal/my-properties",
      label: "عقاراتي",
    },
    {
      to: "/portal/my-subscriptions",
      label: "إشتراكاتي",
    },
  ],
  admin: [
    {
      to: "",
      label: "",
    },
    {
      to: "",
      label: "",
    },
    {
      to: "",
      label: "",
    },
    {
      to: "",
      label: "",
    },
  ],
};
const settings = ["الملف الشخصي", "Account", "Dashboard", "Logout"];

const NavBarMain = () => {
  const [anchorElNav, setAnchorElNav] = useState(null);
  const [anchorElUser, setAnchorElUser] = useState(null);
  const { user, isAdmin, isAuthenticated, logout } = useAuth();
  const navigate = useNavigate();

  const handleOpenNavMenu = (event) => {
    setAnchorElNav(event.currentTarget);
  };
  const handleOpenUserMenu = (event) => {
    setAnchorElUser(event.currentTarget);
  };

  const handleCloseNavMenu = () => {
    setAnchorElNav(null);
  };

  const handleCloseUserMenu = () => {
    setAnchorElUser(null);
  };
  const navigateToProfile = () => {
    setAnchorElUser(null);
    navigate("/portal/profile", { replace });
  };
  const logoutHandler = () => {
    logout();
    // navigate("/portal/profile", { replace });
  };

  return (
    <AppBar position="static" color="inherit" className="md:px-10 px-2">
      <Container maxWidth="xl" className="" sx={{ paddingX: "8px" }}>
        <Toolbar disableGutters>
          {/* menu for tablet and mobile view */}
          <Box sx={{ flexGrow: 0, display: { xs: "flex", md: "none" } }}>
            <IconButton
              size="large"
              aria-label="account of current user"
              aria-controls="menu-appbar"
              aria-haspopup="true"
              onClick={handleOpenNavMenu}
              color="inherit"
            >
              <MenuIcon />
            </IconButton>
            <Menu
              id="menu-appbar"
              anchorEl={anchorElNav}
              anchorOrigin={{
                vertical: "bottom",
                horizontal: "left",
              }}
              keepMounted
              transformOrigin={{
                vertical: "top",
                horizontal: "left",
              }}
              open={Boolean(anchorElNav)}
              onClose={handleCloseNavMenu}
              sx={{ display: { xs: "block", md: "none" } }}
            >
              {/* {pages.map((page) => (
                <MenuItem key={page} onClick={handleCloseNavMenu}>
                  <Typography sx={{ textAlign: "center" }}>{page}</Typography>
                </MenuItem>
              ))} */}
            </Menu>
          </Box>
          <Link to="/" className="basis-2 flex-grow md:flex-grow-0">
            <img src={logo} alt="abwaab-logo" className="max-w-[100px]" />
          </Link>
          {/* menu items for desktop view */}
          <Box
            className="flex-grow-0 hidden md:flex md:flex-1 gap-3 justify-start mx-5 text-navy-500"
            // sx={{
            //   flexGrow: 1,
            //   display: { xs: "none", md: "flex" },
            //   justifyContent: "start",
            // }}
          >
            {!isAuthenticated &&
              pages.public.map((page) => (
                <Link
                  className=""
                  key={`link-${page.to}`}
                  onClick={handleCloseNavMenu}
                  to={page.to}
                >
                  {page.label}
                </Link>
              ))}
            {isAuthenticated &&
              !isAdmin &&
              pages.user.map((page) => (
                <Link
                  className=""
                  key={`link-${page.to}`}
                  onClick={handleCloseNavMenu}
                  to={page.to}
                >
                  {page.label}
                </Link>
              ))}
            {isAuthenticated &&
              isAdmin &&
              pages.admin.map((page) => (
                <Link
                  className=""
                  key={`link-${page.to}`}
                  onClick={handleCloseNavMenu}
                  to={page.to}
                >
                  {page.label}
                </Link>
              ))}
          </Box>
          {!isAuthenticated && (
            <NavLink to={"login"} end className={"mx-3 text-navy-500"}>
              تسجيل الدخول
            </NavLink>
          )}
          {isAuthenticated && (
            <Box sx={{ flexGrow: 0, position: "relative" }}>
              <Tooltip title="Open settings">
                <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
                  <Avatar alt="حسام حبال" src="/static/images/avatar/2.jpg" />
                </IconButton>
              </Tooltip>
              <Menu
                sx={{
                  position: "absolute",
                  top: "6%",
                  right: "3%",
                  ".MuiList-root": { padding: "6px" },
                  ".MuiPaper-root": { borderRadius: "12px" },
                }}
                id="menu-appbar"
                anchorEl={anchorElUser}
                anchorOrigin={{
                  vertical: "top",
                  horizontal: "right",
                }}
                keepMounted
                transformOrigin={{
                  vertical: "top",
                  horizontal: "right",
                }}
                open={Boolean(anchorElUser)}
                onClose={handleCloseUserMenu}
              >
                <MenuItem
                  sx={{
                    minWidth: "250px",
                    padding: 1,
                    borderRadius: "8px",
                  }}
                  key={`setting-profile`}
                  className="hover:!bg-sky-50 !border-neutral-900 !border-4"
                  onClick={navigateToProfile}
                >
                  <div className="flex w-full justify-between items-center">
                    <div className="">
                      <p className="font-semibold text-navy-900 text-[15px]">
                        الملف الشخصي
                      </p>
                      <p className="text-neutral-800 text-xs">
                        إدارة بيانات الحساب
                      </p>
                    </div>
                    <div className="">
                      <ChevronLeft />
                    </div>
                  </div>
                </MenuItem>
                <MenuItem
                  sx={{
                    minWidth: "250px",
                    padding: 1,
                    borderRadius: "8px",
                  }}
                  key={`setting-logout`}
                  className="hover:!bg-sky-50 !border-neutral-900 !border-4"
                  onClick={logoutHandler}
                >
                  <div className="flex w-full justify-between items-center">
                    <div className="">
                      <p className="font-semibold text-error-600 text-[15px]">
                        تسجيل الخروج
                      </p>
                      <p className="text-neutral-800 text-xs">
                        الخروج من الحساب
                      </p>
                    </div>
                    <div className="">
                      <ChevronLeft />
                    </div>
                  </div>
                </MenuItem>
              </Menu>
            </Box>
          )}
        </Toolbar>
      </Container>
    </AppBar>
  );
};
export default NavBarMain;
