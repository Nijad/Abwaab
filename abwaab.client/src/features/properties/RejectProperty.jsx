import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField,
} from "@mui/material";
import { useRef, useState } from "react";
import AddVisitRequest from "./AddVisitRequest";
import LoginUser from "../LoginUser";
import { useNavigate } from "react-router";
import useAuth from "../../hooks/useAuth";
import { propertyApi } from "../../api";
import { enqueueSnackbar } from "notistack";

const RejectProperty = ({ propertyId }) => {
  const [rejectDialog, setRejectDialog] = useState(false);
  const navigate = useNavigate();
  const signalRef = useRef();

  const rejectProperty = async (e) => {
    e.preventDefault();
    if (signalRef.current) {
      signalRef.current.abort();
    }
    const frmdata = new FormData(e.target);
    const data = Object.fromEntries(frmdata.entries());
    try {
      signalRef.current = new AbortController();
      const resp = await propertyApi.rejectProperty(
        data.propertyId,
        data.note,
        signalRef.current.signal
      );
      navigate("/admin/pending-advertisements");
      //   enqueueSnackbar(resp.data.message, { variant: "success" });
      //   if (onSuccess) onSuccess(data.newEmail, resp.data);
    } catch (err) {
      //list related error codes
      if (err.detail) enqueueSnackbar(err.detail, { variant: "error" });
      if (!err.detail) enqueueSnackbar(err, { variant: "error" });
    }
  };

  const handleClcik = () => {
    setRejectDialog(true);
  };

  return (
    <div>
      <Button
        className="!my-3"
        size="large"
        variant="outlined"
        fullWidth
        color="error"
        onClick={handleClcik}
      >
        رفض الإعلان
      </Button>
      <Dialog
        open={rejectDialog}
        onClose={() => setRejectDialog(false)}
        sx={{
          ".MuiPaper-root": {
            paddingX: "10px",
            paddingY: "20px",
            minWidth: "30%",
            borderRadius: "24px",
            alignItems: "flex-strt",
          },
        }}
      >
        <form method="post" onSubmit={(e) => rejectProperty(e)}>
          <DialogTitle>
            <span className="text-2xl text-navy-700">رفض نشر العقار</span>
          </DialogTitle>
          <DialogContent sx={{ maxWidth: "100%" }}>
            <TextField
              sx={{ mt: 2 }}
              name="note"
              size="small"
              multiline
              fullWidth
              variant="outlined"
              label="أضف تعليق (اختياري)"
            />
            <input type="hidden" name="propertyId" value={propertyId} />
            {/* <p className="text-base text-sky-500">
              سيظهر العقار للزوار، وسيرسل اشعار بالموافقة الى المالك
            </p> */}
          </DialogContent>
          <DialogActions
            sx={{
              paddingX: 3,
              "&.MuiDialogActions-root": { justifyContent: "flex-start" },
            }}
          >
            <Button type="submit" variant="contained" color="navy">
              رفض النشر
            </Button>
            <Button
              type="button"
              variant="outlined"
              color="navy"
              onClick={() => setRejectDialog(false)}
            >
              تراجع
            </Button>
          </DialogActions>
        </form>
      </Dialog>
    </div>
  );
};

export default RejectProperty;
