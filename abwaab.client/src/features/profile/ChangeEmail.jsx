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

const ChangeEmail = ({
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
        >
          إرسال رمز التحقق
        </Button>
        <Button onClick={handleClose}>إلغاء</Button>
      </DialogActions>
    </Dialog>
  );
};

export default ChangeEmail;
