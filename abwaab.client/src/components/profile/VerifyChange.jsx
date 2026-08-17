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
import OtpVerification from "../OtpVerification";
import useAuth from "../../hooks/useAuth";

const VerifyChange = ({
  title,
  open,
  description,
  handleClose,
  handleSubmit,
  newIdentifier,
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
        <OtpVerification
          identifier={newIdentifier}
          onVerify={handleSubmit}
          submit_cancel_buttons={true}
        />
      </DialogContent>
      <DialogActions className="!p-5 !justify-start">
        يمكنك طلب رمز جديد بعد 05:00
      </DialogActions>
    </Dialog>
  );
};

export default VerifyChange;
