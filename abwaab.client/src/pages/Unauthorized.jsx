import { Button } from "@mui/material";
import { useNavigate } from "react-router";

const Unauthorized = () => {
  const navigate = useNavigate();
  return (
    <div className="flex flex-col gap-4 h-screen items-center justify-center text-xl font-bold">
      <p className="">غير مسموح لك بالوصول الى هذه الصفحة.</p>
      <Button
        variant="contained"
        type="buton"
        onClick={() => navigate("/", { replace: true })}
        color="navy"
        size="medium"
      >
        عودة الى الصفحة الرئيسية
      </Button>
    </div>
  );
};
export default Unauthorized;
