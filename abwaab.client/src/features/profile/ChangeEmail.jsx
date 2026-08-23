import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  TextField,
} from "@mui/material";
import { useRef, useState } from "react";
import ShowErrors from "../../components/ShowErrors";
import { useSnackbar } from "notistack";
import { profileApi } from "../../api";

const ChangeEmail = ({ title, description, onClose, onSuccess }) => {
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const signalRef = useRef();
  const { enqueueSnackbar } = useSnackbar();

  const modifyEmail = async (e) => {
    e.preventDefault();
    setLoading(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }
    const frmdata = new FormData(e.target);
    const data = Object.fromEntries(frmdata.entries());
    try {
      signalRef.current = new AbortController();
      const resp = await profileApi.initiateEmailChange(
        ...Object.values(data),
        signalRef.current.signal
      );
      enqueueSnackbar(resp.data.message, { variant: "success" });
      if (onSuccess) onSuccess(data.newEmail, resp.data);
    } catch (err) {
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
    <Dialog
      open={true}
      onClose={() => onClose()}
      sx={{ ".MuiPaper-root": { borderRadius: "20px" } }}
    >
      <DialogTitle>{title}</DialogTitle>
      <DialogContent>
        <DialogContentText>{description}</DialogContentText>
        <form
          method="post"
          onSubmit={(e) => modifyEmail(e)}
          id="subscription-form"
        >
          <TextField
            autoFocus
            error={errors == null ? false : errors["NewEmail"] ? true : false}
            helperText={ShowErrors({ object: errors, key: "NewEmail" })}
            required
            margin="dense"
            id="newEmail"
            name="newEmail"
            label="البريد الإلكتروني الجديد"
            type="email"
            fullWidth
            variant="standard"
            size="medium"
          />
          <TextField
            required
            error={
              errors == null ? false : errors["CurrentPassword"] ? true : false
            }
            helperText={ShowErrors({ object: errors, key: "CurrentPassword" })}
            margin="dense"
            id="currentPassword"
            name="currentPassword"
            label="كلمة المرور"
            type="password"
            fullWidth
            variant="standard"
            size="medium"
            placeholder="أدخل كلمة المرور الحالية"
            autoComplete="new-password"
          />
        </form>
      </DialogContent>
      <DialogActions className="!p-5 !justify-start">
        <Button
          type="submit"
          form="subscription-form"
          color="navy"
          variant="contained"
          loading={loading}
        >
          إرسال رمز التحقق
        </Button>
        <Button onClick={() => onClose()}>إلغاء</Button>
      </DialogActions>
    </Dialog>
  );
};

export default ChangeEmail;
