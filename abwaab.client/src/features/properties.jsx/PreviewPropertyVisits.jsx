import { Close } from "@mui/icons-material";
import {
  Box,
  Button,
  Dialog,
  DialogContent,
  DialogTitle,
  IconButton,
  Skeleton,
  Typography,
} from "@mui/material";
import LabelTag from "../../components/LabelTag";
import React, { useEffect, useRef, useState } from "react";
import { useSnackbar } from "notistack";
import { propertyApi } from "../../api";

const dataTest = {
  title: "دمشق – مشروع دمر، شارع الجلاء",
  id: "10248",
  propertyType: "شقة سكنية",
  area: "180 م²",
  imageUrl:
    "https://images.unsplash.com/photo-1545324418-cc1a3fa10c00?auto=format&fit=crop&w=300&q=80",
  requests: [
    {
      id: 1,
      name: "لينا الخطيب",
      phone: "0935-123-456",
      date: "الأحد، 23 آب",
      time: "3:30 م",
    },
    {
      id: 2,
      name: "سامر الحسن",
      phone: "0944-782-310",
      date: "الاثنين، 24 آب",
      time: "2:00 م",
    },
    {
      id: 3,
      name: "نور المصري",
      phone: "0991-640-228",
      date: "الأربعاء، 26 آب",
      time: "5:00 م",
    },
    {
      id: 3,
      name: "نور المصري",
      phone: "0991-640-228",
      date: "الأربعاء، 26 آب",
      time: "5:00 م",
    },
    {
      id: 3,
      name: "نور المصري",
      phone: "0991-640-228",
      date: "الأربعاء، 26 آب",
      time: "5:00 م",
    },
  ],
};

const PreviewPropertyVisits = ({ onReject }) => {
  const [show, setShow] = useState(false);
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const signalRef = useRef();
  const { enqueueSnackbar } = useSnackbar();

  const getPropVisits = async () => {
    setLoading(true);
    setShow(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }

    try {
      signalRef.current = new AbortController();
      const resp = await propertyApi.getPropertyVisitRequests(
        signalRef.current.signal
      );
      setData(resp.data);
    } catch (err) {
      //list related error codes
      enqueueSnackbar(err, { variant: "error" });
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    return () => {
      if (signalRef.current) {
        signalRef.current.abort();
      }
    };
  }, []);

  return (
    <React.Fragment>
      <Button
        size="medium"
        variant="outlined"
        color="navy"
        onClick={() => getPropVisits()}
      >
        عرض طلبات المعاينة
      </Button>
      <Dialog
        open={show}
        onClose={() => setShow(false)}
        maxWidth="md"
        fullWidth
        className=" "
        PaperProps={{
          className: "rounded-2xl p-4 shadow-xl font-sans ",
        }}
      >
        {/* Header */}
        <DialogTitle className="flex items-center justify-between pb-4 pt-2 px-2 font-bold text-neutral-900 text-xl">
          طلبات معاينة العقار
          <IconButton
            onClick={() => setShow(false)}
            className=" hover:bg-gray-100 p-1.5 text-neutral-700"
            size="small"
            sx={{ border: "1px solid #e5ecef" }}
          >
            <Close fontSize="small" />
          </IconButton>
        </DialogTitle>

        <DialogContent className="space-y-6 px-2 pb-2  ">
          {/* Property Info Card */}
          <Box className="bg-neutral-50 border border-neutral-200 rounded-xl p-4 flex items-center justify-start gap-5">
            {/* Metadata Chips & Title */}

            {/* Property Image */}
            <img
              src={dataTest.imageUrl}
              alt={dataTest.title}
              className="size-24 rounded-xl object-cover shadow-sm border border-neutral-200"
            />
            <Box className="flex flex-col items-start gap-0">
              <Typography
                variant="h6"
                className="font-bold text-neutral-900 text-lg"
              >
                {dataTest.title}
              </Typography>
              <Box className="flex items-center gap-2 flex-wrap">
                <LabelTag
                  label={dataTest.id}
                  classes="border border-neutral-300 px-4 rounded-full"
                />
                <LabelTag
                  label={dataTest.propertyType}
                  classes="border border-neutral-300 px-4 rounded-full"
                />
                <LabelTag
                  label={dataTest.area}
                  classes="border border-neutral-300 px-4 rounded-full"
                />
              </Box>
            </Box>
          </Box>

          {/* Requests Section Title */}
          <Box>
            <Typography
              variant="h6"
              className="font-bold text-neutral-900 text-lg mb-4 text-end"
            >
              طلبات المعاينة ({dataTest.requests.length})
            </Typography>
            {loading &&
              ["", "", ""].map((a) => (
                <Skeleton variant="rounded" height={80} className="my-3" />
              ))}

            {/* Requests List */}
            {!loading && (
              <Box className="space-y-3">
                {dataTest.requests.map((request) => (
                  <Box
                    key={request.id}
                    className="border border-neutral-200 rounded-xl p-4 flex items-center justify-between hover:border-neutral-300 transition-colors bg-white"
                  >
                    {/* User Info */}
                    <Box className="text-right">
                      <Typography
                        variant="body1"
                        className="font-bold text-neutral-900"
                      >
                        {request.name}
                      </Typography>
                      <Typography
                        variant="body2"
                        sx={{ direction: "ltr" }}
                        className="text-neutral-400 font-medium text-sm "
                      >
                        {request.phone}
                      </Typography>
                    </Box>

                    {/* Date & Time Badges */}
                    <Box className="flex items-center gap-3">
                      <LabelTag
                        label={request.date}
                        classes="border border-neutral-300 bg-neutral-100 rounded-full px-5"
                      />
                      <LabelTag
                        label={request.time}
                        classes="border border-neutral-300 bg-neutral-100 rounded-full px-5"
                      />
                    </Box>
                    {/* Reject Button */}
                    <Button
                      variant="outlined"
                      color="error"
                      onClick={() => onReject && onReject(request.id)}
                      className="border-red-700 text-red-700 hover:bg-red-50 hover:border-red-800 font-medium text-sm rounded-lg px-4 py-1.5 capitalize"
                    >
                      رفض الطلب
                    </Button>
                  </Box>
                ))}
              </Box>
            )}
          </Box>
        </DialogContent>
      </Dialog>
    </React.Fragment>
  );
};

export default PreviewPropertyVisits;
