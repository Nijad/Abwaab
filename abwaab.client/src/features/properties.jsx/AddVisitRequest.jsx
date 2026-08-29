import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Skeleton,
  ToggleButton,
} from "@mui/material";
import React, { useEffect, useRef, useState } from "react";
import { propertyApi } from "../../api";
import { useParams } from "react-router";
import {
  PROPERTY_AVAILABLE_DATETIME,
  BOOK_APPOINTMENT,
} from "../../dataTypes/propertis";
import { formatDateWithDayAr } from "../../utils/helpers";
import { useSnackbar } from "notistack";

const AddVisitRequest = ({ open, close }) => {
  const [data, setData] = useState(PROPERTY_AVAILABLE_DATETIME);
  const [selectedDay, setSelectedDay] = useState(null);
  const [dataLoading, setDataLoading] = useState(false);
  const [saveLoading, setSaveLoading] = useState(false);
  const getId = useParams("id");
  const { enqueueSnackbar } = useSnackbar();

  const signalRef = useRef();
  console.log("day:", selectedDay);

  const fetchVisits = async () => {
    setDataLoading(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }
    try {
      signalRef.current = new AbortController();
      const resp = await propertyApi.propertyTimeSlots(
        getId.id,
        signalRef.current.signal
      );
      setData(resp.data);
      console.log(resp.data);
    } catch (error) {
      enqueueSnackbar(error.detail, { variant: "error" });
      console.log(error);
    } finally {
      setDataLoading(false);
    }
  };
  const addVisitRequest = async () => {
    setSaveLoading(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }
    try {
      signalRef.current = new AbortController();
      var selectedTime = selectedDay.dayTimes.find((t) => t.selected === true);
      if (!selectedTime || !selectedDay)
        enqueueSnackbar("يجب اختيار اليوم و الوقت", { variant: "error" });
      var date = `${selectedDay.dayDate}T${selectedTime.startTime}`;
      const body = BOOK_APPOINTMENT;
      body.propertyId = getId.id;
      body.appointmentDate = date;
      body.endTime = selectedTime.endTime;
      const resp = await propertyApi.bookAppointment(
        body,
        signalRef.current.signal
      );
      close(false);
      enqueueSnackbar(resp.data.message, { variant: "success" });
      console.log(resp.data);
    } catch (error) {
      if (error?.detail) {
        enqueueSnackbar(error.detail, { variant: "error" });
      } else {
        enqueueSnackbar(error, { variant: "error" });
      }
      console.log(error);
    } finally {
      setSaveLoading(false);
    }
  };

  const handleSelectTime = (e) => {
    // debugger;
    const dt = selectedDay.dayTimes.map((t) => {
      if (t.startTime === e.target.value) {
        return { ...t, selected: true };
      } else {
        return { ...t, selected: false };
      }
    });
    setSelectedDay((prv) => ({ ...prv, dayTimes: dt }));
  };

  useEffect(() => {
    setTimeout(() => {
      if (open) fetchVisits();
    }, 0);
    return () => {
      setSelectedDay();
    };
  }, [open]);
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
          {dataLoading && (
            <div className="flex items-center content-between">
              {[1, 2, 3].map(() => (
                <Skeleton
                  variant="rounded"
                  width={150}
                  height={40}
                  className="!rounded-full"
                  sx={{ marginInlineEnd: "10px" }}
                />
              ))}
            </div>
          )}
          {!dataLoading &&
            data.map((d) => {
              if (d.dayTimes) {
                return (
                  <ToggleButton
                    key={d.dayNumber}
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
                      minWidth: "135px",
                      marginTop: "6px",
                    }}
                    value=""
                    className="!rounded-full !me-2"
                    selected={selectedDay?.dayNumber == d.dayNumber}
                    onChange={() => setSelectedDay(d)}
                  >
                    {formatDateWithDayAr(d.dayDate)}
                  </ToggleButton>
                );
              }
            })}
        </div>
        <div className="my-3">
          {/* Select Time */}
          <p className="text-base text-navy-700 ">اختر الوقت</p>
          {!selectedDay && (
            <p className="text-base text-neutral-500 mt-4">
              حدد اليوم أولاً لتظهر الأوقات المتاحة
            </p>
          )}
          {selectedDay?.dayTimes &&
            selectedDay.dayTimes.map((t) => (
              <ToggleButton
                key={t.startTime}
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
                  minWidth: "80px",
                  marginTop: "6px",
                }}
                value={t.startTime}
                className="!rounded-full !me-2"
                selected={t.selected}
                onChange={(e) => handleSelectTime(e)}
              >
                {t.startTime.replace(":00", "")} -{" "}
                {t.endTime.replace(":00", "")}
              </ToggleButton>
            ))}
        </div>
        <p className="text-base text-neutral-500  mt-4">
          سيتم ارسال رقم التواصل فور قبول الموعد من قبل صاحب الإعلان
        </p>
      </DialogContent>
      <DialogActions
        sx={{
          "&.MuiDialogActions-root": { justifyContent: "flex-start" },
          paddingX: "20px",
        }}
      >
        <Button
          variant="contained"
          size="medium"
          color="navy"
          onClick={() => addVisitRequest()}
          loading={saveLoading}
          disabled={
            !selectedDay
              ? true
              : !selectedDay.dayTimes.find((t) => t.selected === true)
          }
        >
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
