import { Button, TextField } from "@mui/material";
import { useSnackbar } from "notistack";
import React, { useState } from "react";
import { useNavigate } from "react-router";
import axios from "../services/axios";

const Register = () => {
  const navigate = useNavigate();
  const { enqueueSnackbar, closeSnackbar } = useSnackbar();

  const [fdata, setFData] = useState({
    firstName: "",
    lastName: "",
    identifier: "",
    password: "",
    confirmPassword: "",
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
        redirect("confirm-account");
      })
      .catch((e) => {
        console.log(e);
        enqueueSnackbar(e);
      });
  };
  return (
    <div>
      <form method="post" onSubmit={(e) => handleSubmit(e)}>
        <div className="">
          <label htmlFor="firstName"></label>
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
    </div>
  );
};

export default Register;
