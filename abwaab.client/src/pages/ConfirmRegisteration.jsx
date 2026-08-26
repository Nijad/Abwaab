import { Button, Grid } from "@mui/material";
import { useEffect, useRef } from "react";
import useAuth from "../hooks/useAuth";
import { Link, useNavigate } from "react-router";
import { useSnackbar } from "notistack";
import img from "../assets/imgs/register.webp";
import logo from "../assets/imgs/logo.svg";
import { authApi } from "../api";
import VerifyAccount from "../features/VerifyAccount";
import TimeoutButton from "../components/TimeoutButton";

const ConfirmRegisteration = () => {
  const { user, codeRemainingSeconds, setRemainingSeconds, login } = useAuth();
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

  const handleConfirmation = (identifier, response) => {
    login(response);
    navigate("/portal", { replace: true });
  };

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
    } catch (error) {
      enqueueSnackbar(error.message, { variant: "error" });
    }
  };

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
            {user && (
              <>
                <div className="mb-6">
                  <h4 className="text-3xl text-teal-400 font-semibold">
                    تأكيد الحساب
                  </h4>
                  <p>
                    أرسلنا رمز تحقق مكوناً من 6 أرقام الى
                    {user?.identifierType == "email"
                      ? " البريد الإلكتروني"
                      : user?.identifierType == "phone"
                      ? " رقم الموبايل"
                      : ""}
                  </p>
                  <p className="py-3">{user?.identifier}</p>
                </div>

                <div className="flex items-center">
                  <VerifyAccount
                    identifier={user?.identifier}
                    onSuccess={handleConfirmation}
                  />
                </div>
                <div className="">
                  <TimeoutButton
                    label="لم يصلك الرمز؟"
                    onResend={resendCode}
                    seconds={codeRemainingSeconds}
                    key={"to-btn1"}
                  />
                </div>
              </>
            )}
            {!user && (
              <div className="mb-6">
                <h4 className="text-3xl text-teal-400 font-semibold">
                  تأكيد الحساب
                </h4>
                <p className="py-3">
                  يجب القيام بتسجيل حساب أولا{" "}
                  <Button
                    variant="text"
                    onClick={() =>
                      navigate("/registeration", { replace: true })
                    }
                  >
                    قم بالتسجيل الآن
                  </Button>
                </p>
              </div>
            )}
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
