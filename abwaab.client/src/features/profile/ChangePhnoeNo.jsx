import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  TextField,
} from "@mui/material";
import React from "react";
import ShowErrors from "../../components/ShowErrors";

const ChangePhnoeNo = ({
  title,
  open,
  description,
  handleClose,
  handleSubmit,
  errors = null,
}) => {
  return (
    <Dialog
      open={open}
      onClose={() => handleClose()}
      sx={{ ".MuiPaper-root": { borderRadius: "20px" } }}
    >
      <DialogTitle>{title}</DialogTitle>
      <DialogContent>
        <DialogContentText>{description}</DialogContentText>
        <form
          method="post"
          onSubmit={(e) => handleSubmit(e)}
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
        >
          إرسال رمز التحقق
        </Button>
        <Button onClick={handleClose}>إلغاء</Button>
      </DialogActions>
    </Dialog>
  );
};

export default ChangePhnoeNo;
