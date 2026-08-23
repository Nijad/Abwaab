import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
} from "@mui/material";
import { useRef, useState } from "react";
import OtpVerification from "../../components/OtpVerification";
import { useSnackbar } from "notistack";
import { profileApi } from "../../api";

const VerifyChangePhone = ({ onClose, onSuccess, newPhone }) => {
  // const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const [code, setCode] = useState("");

  const signalRef = useRef();
  const { enqueueSnackbar } = useSnackbar();

  const verify = async () => {
    setLoading(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }
    try {
      signalRef.current = new AbortController();
      const resp = await profileApi.confirmPhoneChange(
        newPhone,
        code,
        signalRef.current.signal
      );
      enqueueSnackbar(resp.data.message, { variant: "success" });
      if (onSuccess) onSuccess(newPhone, resp.data);
    } catch (err) {
      //list related error codes
      enqueueSnackbar(err.detail, { variant: "error" });
      // if (err.errorCode === "") {
      //   setErrors(err.errors);
      //   return;
      // } else if (err.errorCode === "INVALID_CODE_OR_EMAIL_MISSMATCH") {
      //   enqueueSnackbar(err.detail, { variant: "error" });
      // }
    } finally {
      setLoading(false);
    }
  };
  return (
    <Dialog
      open={true}
      onClose={() => onClose()}
      sx={{ ".MuiPaper-root": { borderRadius: "20px" } }}
    >
      <DialogTitle>تحقق من البريد الإلكتروني الجديد</DialogTitle>
      <DialogContent>
        <DialogContentText>
          أرسلنا رمزًا إلى {newPhone}، سيبقى رقمك الحالي فعالًا حتى نجاح التحقق.
        </DialogContentText>
        <OtpVerification onChange={setCode} />
      </DialogContent>
      <DialogActions
        sx={{ flexDirection: "column", alignItems: "flex-start" }}
        className="!px-5 "
      >
        <div className="flex items-center gap-3">
          <Button
            type="submit"
            variant="contained"
            disabled={code.length !== 6}
            loading={loading}
            color="navy"
            sx={{ padding: 1.5 }}
            className="disabled:!bg-neutral-200"
            onClick={() => verify()}
          >
            تأكيد ومتابعة
          </Button>
          <Button
            type="button"
            variant="text"
            color="navy"
            sx={{ padding: 1.5 }}
            className="disabled:!bg-neutral-200"
            onClick={() => onClose()}
          >
            الغاء الأمر
          </Button>
        </div>
        {/* <TimeoutButton seconds={300} /> */}
      </DialogActions>
    </Dialog>
  );
};

export default VerifyChangePhone;
