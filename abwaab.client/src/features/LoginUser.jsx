import { Button, TextField } from "@mui/material";
import { useRef, useState } from "react";
import ShowErrors from "../components/ShowErrors";
import { useSnackbar } from "notistack";
import { authApi } from "../api";
import { useNavigate } from "react-router";
import useAuth from "../hooks/useAuth";

const LoginUser = ({ onSuccess }) => {
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const signalRef = useRef();
  const { enqueueSnackbar } = useSnackbar();
  const navigate = useNavigate();
  const { setIdentifier } = useAuth();

  const login = async (e) => {
    e.preventDefault();
    setLoading(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }
    const frmdata = new FormData(e.target);
    const data = Object.fromEntries(frmdata.entries());
    try {
      signalRef.current = new AbortController();
      const resp = await authApi.login(
        ...Object.values(data),
        signalRef.current.signal
      );
      enqueueSnackbar(resp.data.message, { variant: "success" });
      if (onSuccess) onSuccess(data, resp.data);
    } catch (err) {
      setIdentifier(data.identifier);
      console.log(data.identifier);
      //list related error codes
      if (err.errorCode === "VALIDATION_FAILED") {
        setErrors(err.errors);
        enqueueSnackbar(err.detail, { variant: "error" });
        return;
      } else if (
        err.errorCode === "PHONE_NOT_VERIFIED" ||
        err.errorCode === "EMAIL_NOT_VERIFIED"
      ) {
        navigate("/confirm-registeration", { replace: true });
        enqueueSnackbar(err.detail, { variant: "error" });
      } else {
        enqueueSnackbar(err.detail, { variant: "error" });
      }
    } finally {
      setLoading(false);
    }
  };
  return (
    <form method="post" onSubmit={(e) => login(e)} id="subscription-form">
      <div className="mb-2">
        <TextField
          autoFocus
          error={errors == null ? false : errors["Identifier"] ? true : false}
          helperText={ShowErrors({ object: errors, key: "Identifier" })}
          required
          margin="dense"
          id="identifier"
          name="identifier"
          label="البريد الإلكتروني أو رقم الموبايل"
          type="text"
          fullWidth
          variant="filled"
          size="small"
        />
      </div>
      <div className="mb-2">
        <TextField
          required
          error={errors == null ? false : errors["Password"] ? true : false}
          helperText={ShowErrors({ object: errors, key: "Password" })}
          margin="dense"
          id="password"
          name="password"
          label="كلمة المرور"
          type="password"
          fullWidth
          variant="filled"
          size="small"
          autoComplete="new-password"
        />
      </div>
      <div className="mb-2">
        <Button
          type="submit"
          variant="contained"
          color="navy"
          loading={loading}
        >
          دخول
        </Button>
      </div>
    </form>
  );
};

export default LoginUser;
