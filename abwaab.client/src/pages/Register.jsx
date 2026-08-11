import { Button, Grid, TextField } from "@mui/material";
import { useSnackbar } from "notistack";
import React, { useState } from "react";
import { useNavigate } from "react-router";
import axios from "../services/axios";
import useAuth from "../hooks/useAuth";
import img from "../assets/imgs/register.webp";

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
    await axios
      .post(
        "/auth/registeruser",
        { ...fdata },
        {
          signal: controller.signal,
        }
      )
      .then((resp) => {
        console.log(resp.data);
        setIdentifier(fdata.Identifier);
        setRemainingSeconds(resp.data.codeTimeOutInMinuts * 60);
        // sessionStorage.setItem("ced",e.response.data.expiryAt);
        navigate("/confirm-registeration", { replace: true });
      })
      .catch((e) => {
        // debugger;
        if (e.response.data.errorCode === "USER_ALREADY_EXIST") {
          console.log(e);
          //save cde: code expiry date in session storage
          // sessionStorage.setItem("ced",e.response.data.expiryDate);
          enqueueSnackbar(e.response.data.detail, { variant: "error" });
          navigate("/login", { replace: true });
          enqueueSnackbar(
            "قم بتسجيل الدخول باستخدام البريد الالكتروني/ رقم الموبايل",
            {
              variant: "info",
              transitionDuration: { enter: "600", exit: "1200" },
            }
          );
        }
      });
  };
  return (
    <Grid container sx={{ height: "100v" }}>
      <Grid size={8} container direction="column">
        <Grid>Logo</Grid>
        <Grid sx={{ padding: "50px" }}>
          <form method="post" onSubmit={(e) => handleSubmit(e)}>
            <div className="">
              <TextField
                id="firstName"
                name="FirstName"
                label="الأسم الأول"
                variant="standard"
                value={fdata.firstName}
                onChange={(e) =>
                  setFData({ ...fdata, [e.target.name]: e.target.value })
                }
              />
            </div>
            <div className="">
              <TextField
                id="lastName"
                name="LastName"
                label="الأسم الأخير"
                variant="standard"
                size="medium"
                value={fdata.lastName}
                onChange={(e) =>
                  setFData({ ...fdata, [e.target.name]: e.target.value })
                }
              />
            </div>
            <div className="">
              <TextField
                id="identifier"
                name="Identifier"
                label="البريد الإلكتروني/ رقم الموبايل"
                variant="standard"
                size="medium"
                value={fdata.identifier}
                onChange={(e) =>
                  setFData({ ...fdata, [e.target.name]: e.target.value })
                }
              />
            </div>
            <div className="">
              <TextField
                id="password"
                name="Password"
                label="كلمة المرور"
                variant="standard"
                type="password"
                size="medium"
                value={fdata.password}
                onChange={(e) =>
                  setFData({ ...fdata, [e.target.name]: e.target.value })
                }
              />
            </div>
            <div className="">
              <TextField
                id="confirmPassword"
                name="ConfirmPassword"
                label="تأكيد كلمة المرور"
                variant="standard"
                type="password"
                size="medium"
                value={fdata.confirmPassword}
                onChange={(e) =>
                  setFData({ ...fdata, [e.target.name]: e.target.value })
                }
              />
            </div>
            <div className="">
              <Button type="submit" variant="contained" color="navy">
                تسجيل
              </Button>
            </div>
          </form>
          <div className="">
            <p className="">
              لديك حساب بالفعل!
              <Button
                type="button"
                variant="text"
                size="small"
                onClick={() => redirect("login")}
              >
                قم بتسجيل الدخول
              </Button>
            </p>
          </div>
        </Grid>
      </Grid>
      <Grid size={4} sx={{ height: "100" }}>
        <div
          className="w-full h-[100vh]"
          style={{
            backgroundImage: `url(${img})`,
            backgroundPosition: "center",
          }}
        >
          {/* <img
            src={img}
            alt="register"
            className="h-full max-w-full"
            style={{ objectFit: "fill" }}
          /> */}
        </div>
      </Grid>
    </Grid>
  );
};

export default Register;
