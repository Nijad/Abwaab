import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
} from "@mui/material";
import { useState } from "react";
import AddVisitRequest from "./AddVisitRequest";
import LoginUser from "../LoginUser";
import { useNavigate } from "react-router";
import useAuth from "../../hooks/useAuth";

const VisitReservationButton = () => {
  const { isAuthenticated } = useAuth();
  const [showLogin, setShowLogin] = useState(false);
  const [showVisits, setShowVisits] = useState(false);
  const navigate = useNavigate();
  const handleLogin = () => {
    setShowVisits(true);
    setShowLogin(false);
  };
  const handleClcik = () => {
    if (isAuthenticated) {
      setShowVisits(true);
    } else {
      setShowLogin(true);
    }
  };

  return (
    <div>
      <Button
        className="!my-3"
        size="medium"
        variant="contained"
        fullWidth
        color="navy"
        onClick={handleClcik}
      >
        حجز موعد للمعاينة
      </Button>
      <AddVisitRequest open={showVisits} close={setShowVisits} />
      <Dialog
        open={showLogin}
        onClose={() => setShowLogin(false)}
        sx={{
          ".MuiPaper-root": {
            paddingX: "10px",
            paddingY: "20px",
            minWidth: "30%",
            borderRadius: "24px",
            alignItems: "flex-strt",
          },
        }}
      >
        <DialogTitle>
          <h3 className="text-2xl text-navy-700">تسجيل الدخول</h3>
          <p className="text-base text-sky-500">
            سجّل الدخول لإكمال حجز موعد المعاينة
          </p>
        </DialogTitle>
        <DialogContent sx={{ maxWidth: "100%" }}>
          <LoginUser onSuccess={handleLogin} btnLabel="تسجيل الدخول" />
        </DialogContent>
        <DialogActions
          sx={{ "&.MuiDialogActions-root": { justifyContent: "flex-start" } }}
        >
          <p className="px-4">
            ليس لديك حساب؟ قم
            <Button
              type="button"
              onClick={() => navigate("/registeration", { relative: true })}
            >
              بالتسجيل الآن
            </Button>
          </p>
        </DialogActions>
      </Dialog>
    </div>
  );
};

export default VisitReservationButton;
