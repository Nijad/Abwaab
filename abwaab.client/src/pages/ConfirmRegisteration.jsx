import { Button, Grid, TextField } from "@mui/material";
import React, { useEffect, useRef, useState } from "react";
import useAuth from "../hooks/useAuth";
import CountdownTimer from "../components/CountDownTimer";
import axios from "../services/axios";
import { Link, useNavigate } from "react-router";
import { useSnackbar } from "notistack";
import img from "../assets/imgs/register.webp";
import logo from "../assets/imgs/logo.svg";
import { detectIdentifierType, formatTime } from "../utils/helpers";
import OtpVerification from "../components/OtpVerification";
import { authApi } from "../api";

const ConfirmRegisteration = () => {
  const { user, codeRemainingSeconds, setRemainingSeconds, login } = useAuth();
  const [fdata, setFdata] = useState({
    Identifier: user === null ? null : user.identifier,
    Code: "",
  });
  const timerRef = useRef(null);
  const ControllerRef = useRef(null);

  const navigate = useNavigate();
  const { enqueueSnackbar } = useSnackbar();

  useEffect(() => {
    return () => {
      if (ControllerRef.current) {
        ControllerRef.current.abort();
      }
    };
  }, []);

  useEffect(() => {
    if (codeRemainingSeconds > 0) {
      timerRef.current = setInterval(() => {
        setRemainingSeconds((prev) => prev - 1);
      }, 1000);
    } else if (codeRemainingSeconds === 0) {
      clearInterval(timerRef.current);
    }

    return () => {
      clearInterval(timerRef.current);
    };
  }, [codeRemainingSeconds, setRemainingSeconds]);

  const resendCode = async () => {
    if (ControllerRef.current) {
      ControllerRef.current.abort();
    }
    ControllerRef.current = new AbortController();
    try {
      const response = await authApi.resendCode(
        user.identifier,
        ControllerRef.current.signal
      );
      setRemainingSeconds(response.data.codeTimeOutInMinuts * 60);
      enqueueSnackbar(response.data.message, { variant: "success" });

      // await axios
      //   .post("/auth/resendcode", { Identifier: user.identifier })
      //   .then((resp) => {
      //     setRemainingSeconds(resp.data.codeTimeOutInMinuts * 60);
      //     enqueueSnackbar(resp.data.message, { variant: "success" });
      //     console.log(resp.data);
      //   })
      //   .catch((err) => {});
    } catch (error) {}
  };
  const handleFormSubmit = async (code) => {
    if (ControllerRef.current) {
      ControllerRef.current.abort();
    }
    ControllerRef.current = new AbortController();
    try {
      const response = await authApi.verifyAccount(
        user.identifier,
        code,
        ControllerRef.current.signal
      );
      console.log(response.data);
      // login(resp.data);
      navigate("/portal", { replace: true });
      enqueueSnackbar("تم تأكيد الحساب بنجاح", { variant: "success" });
    } catch (error) {
      enqueueSnackbar(error.response.data.detail, { variant: "error" });
    }

    // try {
    //   await axios
    //     .post("/auth/verifyaccount", {
    //       Identifier: user.identifier,
    //       Code: code,
    //     })
    //     .then((resp) => {
    //       console.log(resp.data);
    //       // login(resp.data);
    //       navigate("/portal", { replace: true });
    //       enqueueSnackbar("تم تأكيد الحساب بنجاح", { variant: "success" });
    //     })
    //     .catch((err) => {
    //       enqueueSnackbar(err.response.data.detail, { variant: "error" });
    //     });
    // } catch (error) {
    //   enqueueSnackbar(error, { variant: "error" });
    // }
  };

  const handleCountdownEnd = () => {
    setRemainingSeconds(0);
  };

  // if (!user) {
  //   return <div>يجب انشاء حساب اولا</div>;
  // }

  // check in auth context first then in session storage for code expiration date
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
                تأكيد الحساب
              </h4>
            </div>
            {user && (
              <>
                <div className="flex items-center">
                  <OtpVerification
                    identifier={user.identifier}
                    onVerify={handleFormSubmit}
                    submit_cancel_buttons={false}
                  />
                </div>
                <div className="">
                  <p className="px-2 text-neutral-700 text-sm">
                    لم يصلك الرمز؟
                    {codeRemainingSeconds > 0 ? (
                      <Button
                        type="button"
                        variant="text"
                        size="small"
                        onClick={() => resendCode()}
                        disabled
                      >
                        <span className="underline font-medium text-[13px] text-neutral-500">
                          إعادة الإرسال {formatTime(codeRemainingSeconds)}
                        </span>
                      </Button>
                    ) : (
                      <Button
                        type="button"
                        variant="text"
                        size="small"
                        onClick={() => resendCode()}
                        className="!p-3"
                      >
                        إعادة الإرسال
                      </Button>
                    )}
                  </p>
                </div>
              </>
            )}
            {!user && <p className="text-sky-700">يجب أنشاء حساب أولاً.</p>}
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

export default ConfirmRegisteration;
