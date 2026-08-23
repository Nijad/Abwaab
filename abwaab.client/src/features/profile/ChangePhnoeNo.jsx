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
import { useSnackbar } from "notistack";
import { profileApi } from "../../api";

const ChangePhnoeNo = ({ title, description, onClose, onSuccess }) => {
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const signalRef = useRef();
  const { enqueueSnackbar } = useSnackbar();
  const modifyPhoneNo = async (e) => {
    e.preventDefault();
    setLoading(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }
    const frmdata = new FormData(e.target);
    const data = Object.fromEntries(frmdata.entries());
    try {
      signalRef.current = new AbortController();
      const resp = await profileApi.initiatePhoneChange(
        ...Object.values(data),
        signalRef.current.signal
      );
      enqueueSnackbar(resp.data.message, { variant: "success" });
      if (onSuccess) onSuccess(data.NewPhoneNo, resp.data);
      onSuccess(resp.data);
    } catch (err) {
      //list related error codes
      if (err.errorCode === "VALIDATION_FAILED") {
        setErrors(err.errors);
        enqueueSnackbar(err.detail, { variant: "error" });
        return;
      } else if (err.errorCode === "") {
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
      <DialogTitle>{title}</DialogTitle>
      <DialogContent>
        <DialogContentText>{description}</DialogContentText>
        <form
          method="post"
          onSubmit={(e) => modifyPhoneNo(e)}
          id="subscription-form"
          autoComplete="off"
        >
          <TextField
            autoFocus
            error={errors == null ? false : errors["NewPhoneNo"] ? true : false}
            helperText={ShowErrors({ object: errors, key: "NewPhoneNo" })}
            required
            margin="dense"
            id="newPhoneNo"
            name="newPhoneNo"
            label="رقم الموبايل الجديد"
            type="tel"
            fullWidth
            variant="standard"
            size="medium"
            placeholder={"+9639XXXXXXX"}
            autoComplete="one-time-code"
          />
          <TextField
            autoComplete="new-password"
            error={
              errors == null ? false : errors["CurrentPassword"] ? true : false
            }
            helperText={ShowErrors({ object: errors, key: "CurrentPassword" })}
            required
            margin="dense"
            id="currentPassword"
            name="currentPassword"
            label="كلمة المرور"
            type="password"
            fullWidth
            variant="standard"
            size="medium"
            placeholder="أدخل كلمة المرور الحالية"
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
        <Button onClick={onClose}>إلغاء</Button>
      </DialogActions>
    </Dialog>
  );
};

export default ChangePhnoeNo;
