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
      })
      .catch((e) => {
        console.log(e);
        enqueueSnackbar(e);
      })
      .finally(() => {
        if (isAdmin) redirect("admin");
        else redirect("portal");
      });
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
      <form method="post" onSubmit={handleSubmit}>
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
