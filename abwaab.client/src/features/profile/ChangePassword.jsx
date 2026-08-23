import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  TextField,
} from "@mui/material";
import React, { useRef, useState } from "react";
import ShowErrors from "../../components/ShowErrors";
import { profileApi } from "../../api";
import { useSnackbar } from "notistack";

const ChangePassword = ({ onClose, onSucces: onSuccess }) => {
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const signalRef = useRef();
  const { enqueueSnackbar } = useSnackbar();

  const modifyPassword = async (e) => {
    e.preventDefault();
    setLoading(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }
    const frmdata = new FormData(e.target);
    const data = Object.fromEntries(frmdata.entries());
    try {
      signalRef.current = new AbortController();
      const resp = await profileApi.changePassword(
        ...Object.values(data),
        signalRef.current.signal
      );
      enqueueSnackbar(resp.data.message, { variant: "success" });
      if (onSuccess) onSuccess(resp.data);
      onSuccess(resp.data);
    } catch (err) {
      //list related error codes
      if (err.errorCode === "VALIDATION_FAILED") {
        setErrors(err.errors);
        enqueueSnackbar(err.detail, { variant: "error" });
        return;
      } else if (err.errorCode === "FAILED_CHANGE_PASSWORD") {
        enqueueSnackbar(err.detail, { variant: "error" });
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
      <DialogTitle>تغيير كلمة المرور</DialogTitle>
      <DialogContent>
        <DialogContentText>
          كلمة المرور يجب ان لا تقل عن 8 محارف، تحوي احرف كبيرة، احرف صغيرة ،
          ارقام ورمز خاص
        </DialogContentText>
        <form
          method="post"
          onSubmit={(e) => modifyPassword(e)}
          id="subscription-form"
        >
          <TextField
            required
            autoComplete="new-password"
            error={
              errors == null ? false : errors["CurrentPassword"] ? true : false
            }
            helperText={ShowErrors({ object: errors, key: "CurrentPassword" })}
            margin="dense"
            id="currentPassword"
            name="currentPassword"
            label="كلمة المرور الحالية"
            type="password"
            fullWidth
            variant="standard"
            size="medium"
            placeholder="أدخل كلمة المرور الحالية"
          />
          <TextField
            required
            autoComplete="new-password"
            error={
              errors == null ? false : errors["NewPassword"] ? true : false
            }
            helperText={ShowErrors({ object: errors, key: "NewPassword" })}
            margin="dense"
            id="newPassword"
            name="newPassword"
            label="كلمة المرور الجديدة"
            type="password"
            fullWidth
            variant="standard"
            size="medium"
            placeholder="أدخل كلمة المرور الجديدة"
          />
          <TextField
            required
            autoComplete="new-password"
            error={
              errors == null
                ? false
                : errors["ConfirmNewPassword"]
                ? true
                : false
            }
            helperText={ShowErrors({
              object: errors,
              key: "ConfirmNewPassword",
            })}
            margin="dense"
            id="confirmPassword"
            name="confirmPassword"
            label="تأكيد كلمة المرور الجديدة"
            type="password"
            fullWidth
            variant="standard"
            size="medium"
            placeholder="أدخل كلمة المرور الجديدة "
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
          حفظ كلمة المرور
        </Button>
        <Button onClick={() => onClose()}>إلغاء</Button>
      </DialogActions>
    </Dialog>
  );
};

export default ChangePassword;
