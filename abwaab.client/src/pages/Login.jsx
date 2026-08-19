import React, { useState } from "react";
import { Button, Grid, TextField } from "@mui/material";
import useAuth from "../hooks/useAuth";
import { Link, useNavigate } from "react-router";
import Admin from "./Admin";
import { useSnackbar } from "notistack";
import img from "../assets/imgs/login.webp";
import logo from "../assets/imgs/logo.svg";
import { authApi } from "../api";

const Login = () => {
  const { login } = useAuth();
  const navigate = useNavigate();
  const { enqueueSnackbar, closeSnackbar } = useSnackbar();
  const [fdata, setFdata] = useState({ identifier: "", password: "" });
  const handleSubmit = async (e) => {
    e.preventDefault();
    const controller = new AbortController();
    var isAdmin;
    try {
      const resp = await authApi.login(fdata.identifier, fdata.password);
      console.log(resp.data);
      isAdmin = resp.data.isAdmin;
      login(resp.data);
      if (isAdmin) redirect("admin");
      else redirect("portal");
    } catch (error) {
      if (error.response.data.errorCode === "EMAIL_NOT_VERIFIED") {
        // debugger;
        console.log(e);
        navigate("/confirm-registeration", { replace: true });
        //then in confirm page, it will check the storage and reads if otp is still valid
      }
      enqueueSnackbar(error.response.data.detail, { variant: "error" });
    }

    // await axios
    //   .post(
    //     "/auth/loginuser",
    //     { ...fdata },
    //     {
    //       signal: controller.signal,
    //     }
    //   )
    //   .then((resp) => {
    //     isAdmin = resp.data.isAdmin;
    //     login(resp.data);
    //     if (isAdmin) redirect("admin");
    //     else redirect("portal");
    //   })
    //   .catch((e) => {
    //     if (e.response.data.errorCode === "EMAIL_NOT_VERIFIED") {
    //       // debugger;
    //       console.log(e);
    //       navigate("/confirm-registeration", { replace: true });
    //       enqueueSnackbar(e.response.data.detail, { variant: "error" });
    //       //then in confirm page, it will check the storage and reads if otp is still valid
    //     }
    //   })
    //   .finally(() => {});
  };
  console.log(fdata);
  const redirect = (to) => {
    navigate(`/${to}`);
  };
  const handleForgotPassword = () => {
    redirect("reset-password");
  };

  return (
    <Grid container className="bg-neutral-50 flex-wrap min-h-screen">
      <Grid container direction="column" className="ms-10 flex-1 h-full">
        <Grid>
          <Link to="/" className="m-5">
            <img src={logo} alt="abwaab-logo" className="" />
          </Link>
        </Grid>
        <Grid sx={{ padding: "30px" }} size={7}>
          <div className="bg-white p-6 rounded-3xl">
            <div className="mb-6">
              <h4 className="text-3xl text-navy-700 font-semibold">
                تسجيل الدخول
              </h4>
              <p className="text-base text-sky-700">
                عقارك القادم أقرب مما تتخيل
              </p>
            </div>
            <form method="post" onSubmit={handleSubmit} autoComplete="on">
              <div className="mb-6">
                <TextField
                  id="identifier"
                  label="البريد الإلكتروني أو رقم الموبايل"
                  name="Identifier"
                  type="text"
                  variant="filled"
                  size="small"
                  value={fdata.identifier}
                  onChange={(e) =>
                    setFdata({ ...fdata, identifier: e.target.value })
                  }
                />
              </div>
              <div className="mb-2">
                <TextField
                  id=""
                  label="كلمة المرور"
                  name="Password"
                  type="password"
                  variant="filled"
                  size="small"
                  value={fdata.password}
                  onChange={(e) =>
                    setFdata({ ...fdata, password: e.target.value })
                  }
                />
              </div>
              <div className="mb-6">
                <Button
                  type="button"
                  variant="text"
                  color="sky"
                  onClick={() => handleForgotPassword()}
                  className="mb-5"
                >
                  هل نسيت كلمة المرور؟
                </Button>
              </div>
              <div className="mb-6">
                <Button type="submit" variant="contained" color="navy">
                  دخول
                </Button>
              </div>
            </form>
            <p className="">
              ليس لديك حساب؟ قم
              <Button type="button" onClick={() => redirect("register")}>
                بالتسجيل الآن
              </Button>
            </p>
          </div>
        </Grid>
      </Grid>
      <Grid size={4}>
        <div
          className="w-full h-full"
          style={{
            backgroundImage: `url(${img})`,
            backgroundPosition: "center",
            backgroundSize: "cover",
            backgroundRepeat: "no-repeat",
          }}
        ></div>
      </Grid>
    </Grid>
  );
};

export default Login;
