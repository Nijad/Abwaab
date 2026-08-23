import { Button } from "@mui/material";
import { useRef, useState } from "react";
import OtpVerification from "../components/OtpVerification";
import { useSnackbar } from "notistack";
import { authApi } from "../api";

const VerifyAccount = ({ identifier, onSuccess }) => {
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
      const resp = await authApi.verifyAccount(
        identifier,
        code,
        signalRef.current.signal
      );
      enqueueSnackbar(resp.data.message, { variant: "success" });
      if (onSuccess) onSuccess(identifier, resp.data);
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
    <div className="w-100">
      <OtpVerification onChange={setCode} />
      <div className="flex items-center gap-3">
        <Button
          type="submit"
          variant="contained"
          disabled={code.length !== 6}
          loading={loading}
          color="navy"
          sx={{ padding: 1.5 }}
          className="disabled:!bg-neutral-200"
          fullWidth
          onClick={() => verify()}
        >
          تأكيد ومتابعة
        </Button>
      </div>
    </div>
  );
};

export default VerifyAccount;
