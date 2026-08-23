import { Button, Grid } from "@mui/material";
import { Link, useNavigate } from "react-router";
import useAuth from "../hooks/useAuth";
import img from "../assets/imgs/register.webp";
import logo from "../assets/imgs/logo.svg";
import Register from "../features/Register";

const Registeration = () => {
  const navigate = useNavigate();
  const { setIdentifier, setRemainingSeconds } = useAuth();

  const handleRegister = (data, response) => {
    setIdentifier(data.identifier);
    setRemainingSeconds(response.codeTimeOutInMinuts * 60);
    navigate("/confirm-registeration", { replace: true });
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
            <Register onSuccess={handleRegister} />
            <div className="">
              <p className="">
                لديك حساب بالفعل؟ قم بـ
                <Button
                  type="button"
                  variant="text"
                  size="small"
                  onClick={() => navigate("/login", { replace: true })}
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

export default Registeration;
