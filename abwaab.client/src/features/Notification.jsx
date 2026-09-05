import { Close, NotificationsOutlined } from "@mui/icons-material";
import { Badge, IconButton, Menu, MenuItem } from "@mui/material";
import React, { useEffect, useRef, useState } from "react";
import { useNavigate } from "react-router";
import useAuth from "../hooks/useAuth";
import { notificationApi } from "../api";
import { enqueueSnackbar } from "notistack";

const dataTest = [
  {
    id: 0,
    title: "لديك طلب زيارة احد عقاراتك",
    description:
      "قام المستخدم حسام حبال بطلب معاينة عقارك. قم بزيارة صفحة مواعيدي",
    date: "13/05/2026",
    isRead: false,
  },
  {
    id: 1,
    title: "لديك طلب زيارة احد عقاراتك",
    description:
      "قام المستخدم سامر فوزي بطلب معاينة عقارك. قم بزيارة صفحة مواعيدي",
    date: "14/05/2026",
    isRead: false,
  },
  {
    id: 2,
    title: "لديك طلب زيارة احد عقاراتك",
    description:
      "قام المستخدم وائل حداد بطلب معاينة عقارك. قم بزيارة صفحة مواعيدي",
    date: "15/05/2026",
    isRead: true,
  },
];

const Notification = () => {
  const [anchorElNotfication, setAnchorElNotfication] = useState(null);
  const [data, setData] = useState(null);
  const { isAdmin } = useAuth();
  const signalRef = useRef();
  const navigate = useNavigate();

  const fetchMyNotifications = async () => {
    if (signalRef.current) {
      signalRef.current.abort();
    }
    try {
      signalRef.current = new AbortController();
      const resp = await notificationApi.userNotifcations(
        signalRef.current.signal
      );
      //   enqueueSnackbar(resp.data.message, { variant: "success" });
      // setData(resp.data);
    } catch (err) {
      //list related error codes
      //   enqueueSnackbar(err.detail, { variant: "error" });
      // if (err.errorCode === "VALIDATION_FAILED") {
      //   setErrors(err.errors);
      //   return;
      // } else if (err.errorCode === "") {
      //   enqueueSnackbar(err.response.data.message, { variant: "error" });
      // }
    }
  };
  const deleteNotification = async () => {
    if (signalRef.current) {
      signalRef.current.abort();
    }
    try {
      signalRef.current = new AbortController();
      const resp = await notificationApi.deleteNotifcation(
        signalRef.current.signal
      );
      //   enqueueSnackbar(resp.data.message, { variant: "success" });
      // setData(resp.data);
    } catch (err) {
      //list related error codes
      if (err.detail) enqueueSnackbar(err.detail, { variant: "error" });
      if (!err.detail) enqueueSnackbar(err, { variant: "error" });
      // if (err.errorCode === "VALIDATION_FAILED") {
      //   setErrors(err.errors);
      //   return;
      // } else if (err.errorCode === "") {
      //   enqueueSnackbar(err.response.data.message, { variant: "error" });
      // }
    }
  };
  const clearAll = async () => {
    if (signalRef.current) {
      signalRef.current.abort();
    }
    try {
      signalRef.current = new AbortController();
      const resp = await notificationApi.deleteNotifcation(
        signalRef.current.signal
      );
      //   enqueueSnackbar(resp.data.message, { variant: "success" });
      // setData(resp.data);
    } catch (err) {
      //list related error codes
      if (err.detail) enqueueSnackbar(err.detail, { variant: "error" });
      if (!err.detail) enqueueSnackbar(err, { variant: "error" });
      // if (err.errorCode === "VALIDATION_FAILED") {
      //   setErrors(err.errors);
      //   return;
      // } else if (err.errorCode === "") {
      //   enqueueSnackbar(err.response.data.message, { variant: "error" });
      // }
    }
  };

  const handleOpenNotificationMenu = (event) => {
    setAnchorElNotfication(event.currentTarget);
  };

  const handleCloseNotificationMenu = () => {
    setAnchorElNotfication(null);
  };
  const navigateToNotification = () => {
    setAnchorElNotfication(null);
    switch (isAdmin) {
      case true:
        navigate("/admin/notification");
        break;
      case false:
        navigate("/portal/notification");
        break;
      default:
        break;
    }
  };
  useEffect(() => {
    setTimeout(() => {
      fetchMyNotifications();
    }, 0);

    return () => {
      if (signalRef.current) {
        signalRef.current.abort();
      }
    };
  });

  return (
    <React.Fragment>
      <IconButton
        size="large"
        color="navy"
        onClick={handleOpenNotificationMenu}
      >
        <Badge badgeContent={5} color="teal">
          <NotificationsOutlined />
        </Badge>
      </IconButton>
      <Menu
        sx={{
          position: "absolute",
          top: "6%",
          right: "8%",
          ".MuiList-root": { padding: "6px" },
          ".MuiPaper-root": { borderRadius: "12px" },
        }}
        id="menu-appbar"
        anchorEl={anchorElNotfication}
        anchorOrigin={{
          vertical: "top",
          horizontal: "right",
        }}
        keepMounted
        transformOrigin={{
          vertical: "top",
          horizontal: "right",
        }}
        open={Boolean(anchorElNotfication)}
        onClose={handleCloseNotificationMenu}
      >
        <div className="overflow-y-scroll max-h-96">
          {dataTest.map((n) => (
            <MenuItem
              sx={{
                minWidth: "250px",
                padding: 1,
                borderRadius: "8px",
                marginY: 1,
              }}
              key={`notif${n.id}`}
              className={`hover:!bg-sky-300 !border-neutral-900 !border-4 ${
                n.isRead ? "" : "!bg-sky-200"
              }`}
              onClick={deleteNotification}
            >
              {/* <div
                className={`flex w-full justify-between items-center rounded-lg p-2 ${
                  n.isRead ? "" : "bg-sky-200"
                  }`}
              > */}
              <div className="">
                <p className="font-semibold text-navy-900 text-[15px]">
                  {n.title}
                </p>
                <p className="text-neutral-800 text-xs mb-1">{n.description}</p>
                <p className="text-neutral-800 text-xs mb-1">{n.date}</p>
                {/* </div> */}
              </div>
            </MenuItem>
          ))}
        </div>
        <MenuItem
          sx={{
            minWidth: "250px",
            padding: 1,
            borderRadius: "8px",
            marginY: 1,
            justifyContent: "center",
          }}
          onClick={clearAll}
        >
          قراءة الكل
        </MenuItem>
      </Menu>
    </React.Fragment>
  );
};

export default Notification;
