import { useSnackbar } from "notistack";
import { useRef, useState } from "react";
import { authApi } from "../api";
import { Button, TextField } from "@mui/material";
import ShowErrors from "../components/ShowErrors";

const Register = ({ onSuccess }) => {
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const signalRef = useRef();
  const { enqueueSnackbar } = useSnackbar();

  const register = async (e) => {
    e.preventDefault();
    setLoading(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }
    const frmdata = new FormData(e.target);
    const data = Object.fromEntries(frmdata.entries());
    try {
      signalRef.current = new AbortController();
      const resp = await authApi.register(
        ...Object.values(data),
        signalRef.current.signal
      );
      enqueueSnackbar(resp.data.message, { variant: "success" });
      if (onSuccess) onSuccess(data, resp.data);
    } catch (err) {
      //   debugger;
      //list related error codes
      if (err.errorCode === "VALIDATION_FAILED") {
        setErrors(err.errors);
        enqueueSnackbar(err.detail, { variant: "error" });
        return;
      } else if (err.errorCode === "") {
        enqueueSnackbar(err.response.data.message, { variant: "error" });
      }
    } finally {
      setLoading(false);
    }
  };
  return (
    <form method="post" onSubmit={(e) => register(e)}>
      <div className="mb-6">
        <TextField
          autoFocus
          error={errors == null ? false : errors["FirstName"] ? true : false}
          helperText={ShowErrors({ object: errors, key: "FirstName" })}
          required
          id="firstName"
          name="firstName"
          label="الاسم الأول"
          variant="filled"
          size="small"
          color="sky"
        />
      </div>
      <div className="mb-6">
        <TextField
          error={errors == null ? false : errors["LastName"] ? true : false}
          helperText={ShowErrors({ object: errors, key: "LastName" })}
          id="lastName"
          name="lastName"
          required
          label="الاسم الأخير"
          variant="filled"
          size="small"
          color="sky"
        />
      </div>
      <div className="mb-6">
        <TextField
          error={errors == null ? false : errors["Identifier"] ? true : false}
          helperText={ShowErrors({ object: errors, key: "Identifier" })}
          id="identifier"
          name="identifier"
          required
          label="البريد الإلكتروني أو رقم الموبايل"
          variant="filled"
          size="small"
          color="sky"
        />
      </div>
      <div className="mb-6">
        <TextField
          error={errors == null ? false : errors["Password"] ? true : false}
          helperText={ShowErrors({ object: errors, key: "Password" })}
          id="password"
          name="password"
          required
          label="كلمة المرور"
          variant="filled"
          type="password"
          size="small"
          color="sky"
        />
      </div>
      <div className="mb-6">
        <TextField
          error={
            errors == null ? false : errors["ConfirmPassword"] ? true : false
          }
          helperText={ShowErrors({ object: errors, key: "ConfirmPassword" })}
          id="confirmPassword"
          name="confirmPassword"
          required
          label="تأكيد كلمة المرور"
          variant="filled"
          type="password"
          size="small"
          color="sky"
        />
      </div>
      <div className="mb-6">
        <Button
          type="submit"
          variant="contained"
          color="navy"
          loading={loading}
        >
          انشاء الحساب
        </Button>
      </div>
    </form>
  );
};

export default Register;
