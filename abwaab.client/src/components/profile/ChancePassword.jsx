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

const ChangePassword = ({ open, handleClose, handleSubmit }) => {
  return (
    <Dialog
      open={open}
      onClose={() => handleClose()}
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
          onSubmit={(e) => handleSubmit(e)}
          id="subscription-form"
        >
          <TextField
            required
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
        >
          حفظ كلمة المرور
        </Button>
        <Button onClick={handleClose}>إلغاء</Button>
      </DialogActions>
    </Dialog>
  );
};

export default ChangePassword;
