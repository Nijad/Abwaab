import React, { useState } from "react";
import axios from "../services/axios";
import { Button } from "@mui/material";
import useAuth from "../hooks/useAuth";
import { useNavigate } from "react-router";
import Admin from "./Admin";
import { useSnackbar } from "notistack";

const Login = () => {
  // const axiosPrivate = useAxiosPrivate();
  const { login } = useAuth();
  const navigate = useNavigate();
  const { enqueueSnackbar, closeSnackbar } = useSnackbar();
  const [fdata, setFdata] = useState({ identifier: "", password: "" });
  const handleSubmit = async (e) => {
    e.preventDefault();
    const controller = new AbortController();
    var isAdmin;
    await axios
      .post(
        "/auth/loginuser",
        { ...fdata },
        {
          signal: controller.signal,
        }
      )
      .then((resp) => {
        isAdmin = resp.data.isAdmin;
        login(resp.data);
        if (isAdmin) redirect("admin");
        else redirect("portal");
      })
      .catch((e) => {
        if (e.response.data.errorCode === "EMAIL_NOT_VERIFIED") {
          // debugger;
          console.log(e);
          navigate("/confirm-registeration", { replace: true });
          enqueueSnackbar(e.response.data.detail, { variant: "error" });
          //then in confirm page, it will check the storage and reads if otp is still valid
        }
      })
      .finally(() => {});
  };
  console.log(fdata);
  const redirect = (to) => {
    navigate(`/${to}`);
  };
  const handleForgotPassword = () => {
    redirect("reset-password");
  };

  return (
    <div className="">
      <form method="post" onSubmit={handleSubmit} autoComplete="off">
        <div className="">
          <label>البريد الإلكتروني/ الموبايل</label>
          <input
            type="text"
            name="Identifier"
            value={fdata.identifier}
            onChange={(e) => setFdata({ ...fdata, identifier: e.target.value })}
          />
        </div>
        <div className="">
          <label>كلمة المرور</label>
          <input
            type="password"
            name="Password"
            value={fdata.password}
            onChange={(e) => setFdata({ ...fdata, password: e.target.value })}
          />
        </div>
        {/* <input type="submit" /> */}
        <Button type="submit" variant="contained" color="navy">
          دخول
        </Button>
      </form>
      <Button
        type="button"
        variant="text"
        color="sky"
        onClick={() => handleForgotPassword()}
      >
        هل نسيت كلمة المرور؟
      </Button>
      <p className="">
        ليس لديك حساب؟{" "}
        <Button type="button" onClick={() => redirect("register")}>
          قم بالتسجيل الآن
        </Button>{" "}
      </p>
    </div>
  );
};

export default Login;
