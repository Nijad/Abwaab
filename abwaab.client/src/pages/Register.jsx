import { Button, Grid, TextField } from "@mui/material";
import { useSnackbar } from "notistack";
import React, { useState } from "react";
import { Link, useNavigate } from "react-router";
import axios from "../services/axios";
import useAuth from "../hooks/useAuth";
import img from "../assets/imgs/register.webp";
import logo from "../assets/imgs/logo.svg";
import { authApi } from "../api";

const Register = () => {
  const navigate = useNavigate();
  const { setIdentifier, setRemainingSeconds } = useAuth();
  const { enqueueSnackbar } = useSnackbar();

  const [fdata, setFData] = useState({
    FirstName: "",
    LastName: "",
    Identifier: "",
    Password: "",
    ConfirmPassword: "",
  });
  const redirect = (to) => {
    navigate(`/${to}`);
  };
  const handleSubmit = async (e) => {
    e.preventDefault();
    const controller = new AbortController();
    try {
      const response = await authApi.register(
        ...Object.values(fdata),
        controller.signal
      );
      console.log(response.data);
      setIdentifier(fdata.Identifier);
      setRemainingSeconds(response.data.codeTimeOutInMinuts * 60);
      // sessionStorage.setItem("ced",e.response.data.expiryAt);
      navigate("/confirm-registeration", { replace: true });
    } catch (error) {
      // debugger;
      if (error.response.data.errorCode === "USER_ALREADY_EXIST") {
        console.log(error);
        //save cde: code expiry date in session storage
        // sessionStorage.setItem("ced",e.response.data.expiryDate);
        enqueueSnackbar(error.response.data.detail, { variant: "error" });
        navigate("/login", { replace: true });
        enqueueSnackbar(
          "قم بتسجيل الدخول باستخدام البريد الالكتروني/ رقم الموبايل",
          {
            variant: "info",
            transitionDuration: { enter: "600", exit: "1200" },
          }
        );
      }
    }

    // await axios
    //   .post(
    //     "/auth/registeruser",
    //     { ...fdata },
    //     {
    //       signal: controller.signal,
    //     }
    //   )
    //   .then((resp) => {
    //     console.log(resp.data);
    //     setIdentifier(fdata.Identifier);
    //     setRemainingSeconds(resp.data.codeTimeOutInMinuts * 60);
    //     // sessionStorage.setItem("ced",e.response.data.expiryAt);
    //     navigate("/confirm-registeration", { replace: true });
    //   })
    //   .catch((e) => {
    //     // debugger;
    //     if (e.response.data.errorCode === "USER_ALREADY_EXIST") {
    //       console.log(e);
    //       //save cde: code expiry date in session storage
    //       // sessionStorage.setItem("ced",e.response.data.expiryDate);
    //       enqueueSnackbar(e.response.data.detail, { variant: "error" });
    //       navigate("/login", { replace: true });
    //       enqueueSnackbar(
    //         "قم بتسجيل الدخول باستخدام البريد الالكتروني/ رقم الموبايل",
    //         {
    //           variant: "info",
    //           transitionDuration: { enter: "600", exit: "1200" },
    //         }
    //       );
    //     }
    //   });
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
              <h4 className="text-3xl text-teal-400 font-semibold">
                إنشاء حساب
              </h4>
              <p className="text-base text-sky-700">
                خطوتك الأولى نحو العقار الذي يليق بطموحك
              </p>
            </div>
            <form method="post" onSubmit={(e) => handleSubmit(e)}>
              <div className="mb-6">
                <TextField
                  id="firstName"
                  name="FirstName"
                  label="الأسم الأول"
                  variant="filled"
                  size="small"
                  color="sky"
                  value={fdata.FirstName}
                  onChange={(e) =>
                    setFData({ ...fdata, [e.target.name]: e.target.value })
                  }
                />
              </div>
              <div className="mb-6">
                <TextField
                  id="lastName"
                  name="LastName"
                  label="الأسم الأخير"
                  variant="filled"
                  size="small"
                  value={fdata.LastName}
                  onChange={(e) =>
                    setFData({ ...fdata, [e.target.name]: e.target.value })
                  }
                />
              </div>
              <div className="mb-6">
                <TextField
                  id="identifier"
                  name="Identifier"
                  label="البريد الإلكتروني أو رقم الموبايل"
                  variant="filled"
                  size="small"
                  value={fdata.Identifier}
                  onChange={(e) =>
                    setFData({ ...fdata, [e.target.name]: e.target.value })
                  }
                />
              </div>
              <div className="mb-6">
                <TextField
                  id="password"
                  name="Password"
                  label="كلمة المرور"
                  variant="filled"
                  type="password"
                  size="small"
                  value={fdata.Password}
                  onChange={(e) =>
                    setFData({ ...fdata, [e.target.name]: e.target.value })
                  }
                />
              </div>
              <div className="mb-6">
                <TextField
                  id="confirmPassword"
                  name="ConfirmPassword"
                  label="تأكيد كلمة المرور"
                  variant="filled"
                  type="password"
                  size="small"
                  value={fdata.ConfirmPassword}
                  onChange={(e) =>
                    setFData({ ...fdata, [e.target.name]: e.target.value })
                  }
                />
              </div>
              <div className="mb-6">
                <Button type="submit" variant="contained" color="navy">
                  انشاء الحساب
                </Button>
              </div>
            </form>
            <div className="">
              <p className="">
                لديك حساب بالفعل؟ قم بـ
                <Button
                  type="button"
                  variant="text"
                  size="small"
                  onClick={() => redirect("login")}
                >
                  بتسجيل الدخول
                </Button>
              </p>
            </div>
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

export default Register;
