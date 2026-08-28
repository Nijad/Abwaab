import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  ToggleButton,
} from "@mui/material";
import React from "react";

const AddVisitRequest = ({ open, close }) => {
  return (
    <Dialog
      open={open}
      onClose={() => close(false)}
      sx={{
        ".MuiPaper-root": {
          paddingX: "15px",
          paddingY: "20px",
          minWidth: "40%",
          borderRadius: "18px",
        },
      }}
    >
      <DialogTitle>
        <h3 className="text-xl text-navy-600 font-semibold mb-3">
          حجز موعد للمعاينة
        </h3>
        <p className="text-base text-neutral-500  mb-1">
          اختر يوماً، ثم اختر الوقت المناسب من المواعيدادناه للمعاينة
        </p>
      </DialogTitle>
      <DialogContent>
        {/* Select Day */}
        <div className="my-3">
          <p className="text-base text-navy-700 ">اختر اليوم</p>
          <ToggleButton
            key={"testkey"}
            sx={{
              paddingX: "12px",
              paddingY: "2px",
              minHeight: "40px",
              "&.Mui-selected": {
                backgroundColor: "#0D2A4A",
                color: "white",
              },
              "&.Mui-selected:hover": {
                backgroundColor: "#0a1e33",
              },
              minWidth: "100px",
              marginTop: "6px",
            }}
            value=""
            className="!rounded-full !me-2"
            selected={true}
            onChange={() => null}
          >
            الأحد، 23 آب
          </ToggleButton>
          <ToggleButton
            key={"testkey"}
            sx={{
              paddingX: "12px",
              paddingY: "2px",
              minHeight: "40px",
              "&.Mui-selected": {
                backgroundColor: "#0D2A4A",
                color: "white",
              },
              "&.Mui-selected:hover": {
                backgroundColor: "#0a1e33",
              },
              minWidth: "100px",
              marginTop: "6px",
            }}
            value=""
            className="!rounded-full !me-2"
            selected={false}
            onChange={() => null}
          >
            الأحد، 23 آب
          </ToggleButton>
        </div>
        <div className="my-3">
          {/* Select Time */}
          <p className="text-base text-navy-700 ">اختر الوقت</p>
          <ToggleButton
            key={"testkey"}
            sx={{
              paddingX: "12px",
              paddingY: "2px",
              minHeight: "40px",
              "&.Mui-selected": {
                backgroundColor: "#0D2A4A",
                color: "white",
              },
              "&.Mui-selected:hover": {
                backgroundColor: "#0a1e33",
              },
              minWidth: "100px",
              marginTop: "6px",
            }}
            value=""
            className="!rounded-full !me-2"
            selected={true}
            onChange={() => null}
          >
            الأحد، 23 آب
          </ToggleButton>
          <ToggleButton
            key={"testkey"}
            sx={{
              paddingX: "12px",
              paddingY: "2px",
              minHeight: "40px",
              "&.Mui-selected": {
                backgroundColor: "#0D2A4A",
                color: "white",
              },
              "&.Mui-selected:hover": {
                backgroundColor: "#0a1e33",
              },
              minWidth: "100px",
              marginTop: "6px",
            }}
            value=""
            className="!rounded-full !me-2"
            selected={false}
            onChange={() => null}
          >
            الأحد، 23 آب
          </ToggleButton>
        </div>
        <p className="text-base text-neutral-500  mt-4">
          سيتم إرسال رقم صاحب العقار برسالة نصية قبل ساعتان من الموعد المحدد
        </p>
      </DialogContent>
      <DialogActions
        sx={{
          "&.MuiDialogActions-root": { justifyContent: "flex-start" },
          paddingX: "20px",
        }}
      >
        <Button variant="contained" size="medium" color="navy">
          إرسال طلب المعاينة
        </Button>
        <Button
          variant="outlined"
          size="medium"
          color="navy"
          onClick={() => close(false)}
        >
          إلغاء
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default AddVisitRequest;
