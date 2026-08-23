// import React from "react";
import { Button, Grid } from "@mui/material";
import useAuth from "../hooks/useAuth";
import { Link, useNavigate } from "react-router";
import { useSnackbar } from "notistack";
import img from "../assets/imgs/login.webp";
import logo from "../assets/imgs/logo.svg";
import LoginUser from "../features/LoginUser";

const Login = () => {
  const { login, setIdentifier } = useAuth();
  const navigate = useNavigate();
  const { enqueueSnackbar } = useSnackbar();

  const hanldleLogin = (data, response) => {
    var isAdmin;
    try {
      isAdmin = response.isAdmin;
      login(response);
      setIdentifier(data.identifier);
      if (isAdmin) navigate("/admin", { replace: true });
      else navigate("/portal", { replace: true });
    } catch (error) {
      console.log(error);
      enqueueSnackbar(error, { variant: "error" });
    }
  };

  const handleForgotPassword = () => {
    navigate("/reset-password", { replace: true });
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
            <LoginUser onSuccess={hanldleLogin} />
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
            <p className="">
              ليس لديك حساب؟ قم
              <Button
                type="button"
                onClick={() => navigate("/registeration", { relative: true })}
              >
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
