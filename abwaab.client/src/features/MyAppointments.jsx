import { useEffect, useRef, useState } from "react";
import PropTypes from "prop-types";
import { useSnackbar } from "notistack";
import { useNavigate } from "react-router";
import { appointmentsApi } from "../api";
import { Box, Tab, Tabs } from "@mui/material";
import AppointmentCard from "../components/AppintmentCard";

const data = {
  receivedAppointments: [
    {
      appointmentDate: "05/05/2026",
      dayName: "",
      appointments: [
        {
          appointmentId: "",
          fromTime: "10:00",
          endTime: "",
          appointmentState: "",
          appointmentDirection: "",
          cancelable: true,
          comments: "",
          firstname: "",
          lastname: "",
          email: "",
          phoneNo: "",
          propertyId: "",
          propertyTitle: "",
          coverPath:
            "https://images.unsplash.com/photo-1545324418-cc1a3fa10c00?w=200&h=200&fit=crop",
          address: "المزرعة",
          area: 0,
          price: 300000,
        },
        {
          appointmentId: "",
          fromTime: "05:30",
          endTime: "",
          appointmentState: "",
          appointmentDirection: "",
          cancelable: true,
          comments: "",
          firstname: "",
          lastname: "",
          email: "",
          phoneNo: "",
          propertyId: "",
          propertyTitle: "شقة عصرية في المزرعة",
          coverPath:
            "https://images.unsplash.com/photo-1545324418-cc1a3fa10c00?w=200&h=200&fit=crop",
          address: "دمشق  المزرعة",
          area: 150,
          price: 2000000,
        },
      ],
    },
  ],
  requestedAppointments: [
    {
      appointmentDate: "05/05/2026",
      dayName: "",
      appointments: [
        {
          appointmentId: "",
          fromTime: "10:00",
          endTime: "",
          appointmentState: "",
          appointmentDirection: "",
          cancelable: true,
          comments: "",
          firstname: "",
          lastname: "",
          email: "",
          phoneNo: "",
          propertyId: "",
          propertyTitle: "",
          coverPath: "",
          address: "",
          area: 0,
          price: 0,
        },
      ],
    },
  ],
};

function CustomTabPanel(props) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`simple-tabpanel-${index}`}
      aria-labelledby={`simple-tab-${index}`}
      {...other}
    >
      {value === index && <Box sx={{ pt: 3 }}>{children}</Box>}
    </div>
  );
}

CustomTabPanel.propTypes = {
  children: PropTypes.node,
  index: PropTypes.number.isRequired,
  value: PropTypes.number.isRequired,
};

function a11yProps(index) {
  return {
    id: `simple-tab-${index}`,
    "aria-controls": `simple-tabpanel-${index}`,
  };
}

const MyAppointmnets = ({
  onAddProperty,
  onPromote,
  // onEdit,
  onVisitPreview,
  onSuccess,
}) => {
  const [data, setData] = useState();
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const signalRef = useRef();
  const { enqueueSnackbar } = useSnackbar();
  const navigate = useNavigate();

  const [value, setValue] = useState(0);

  const handleChange = (event, newValue) => {
    setValue(newValue);
  };

  const fetchMyAppointments = async () => {
    setLoading(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }
    try {
      signalRef.current = new AbortController();
      const resp = await appointmentsApi.userAppointments(
        signalRef.current.signal
      );
      //   enqueueSnackbar(resp.data.message, { variant: "success" });
      // const d = [...resp.data.receivedAppointments];
      // const f = { ...resp.data };
      // f.requestedAppointments = d;
      // setData(f);
      setData(resp.data);
      if (onSuccess) onSuccess(resp.data);
    } catch (err) {
      //list related error codes
      enqueueSnackbar(err.detail, { variant: "error" });
      // if (err.errorCode === "VALIDATION_FAILED") {
      //   setErrors(err.errors);
      //   return;
      // } else if (err.errorCode === "") {
      //   enqueueSnackbar(err.response.data.message, { variant: "error" });
      // }
    } finally {
      setLoading(false);
    }
  };

  const acceptAppointment = async (id) => {
    setLoading(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }
    try {
      signalRef.current = new AbortController();
      const resp = await appointmentsApi.confirmAppointments(
        id,
        signalRef.current.signal
      );
      enqueueSnackbar(resp.data.message, { variant: "success" });
      fetchMyAppointments();
      // if (onSuccess) onSuccess(resp.data);
    } catch (err) {
      //list related error codes
      enqueueSnackbar(err.detail, { variant: "error" });
      // if (err.errorCode === "VALIDATION_FAILED") {
      //   setErrors(err.errors);
      //   return;
      // } else if (err.errorCode === "") {
      //   enqueueSnackbar(err.response.data.message, { variant: "error" });
      // }
    } finally {
      setLoading(false);
    }
  };

  const rejectAppointment = async (id) => {
    setLoading(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }
    try {
      signalRef.current = new AbortController();
      const resp = await appointmentsApi.refuseAppointments(
        id,
        signalRef.current.signal
      );
      enqueueSnackbar(resp.data.message, { variant: "success" });
      fetchMyAppointments();
      // if (onSuccess) onSuccess(resp.data);
    } catch (err) {
      //list related error codes
      enqueueSnackbar(err.detail, { variant: "error" });
      // if (err.errorCode === "VALIDATION_FAILED") {
      //   setErrors(err.errors);
      //   return;
      // } else if (err.errorCode === "") {
      //   enqueueSnackbar(err.response.data.message, { variant: "error" });
      // }
    } finally {
      setLoading(false);
    }
  };

  const cancelAppointment = async (id) => {
    setLoading(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }
    try {
      signalRef.current = new AbortController();
      const resp = await appointmentsApi.cancelAppointments(
        id,
        signalRef.current.signal
      );
      enqueueSnackbar(resp.data.message, { variant: "success" });
      fetchMyAppointments();
      // if (onSuccess) onSuccess(resp.data);
    } catch (err) {
      //list related error codes
      enqueueSnackbar(err.detail, { variant: "error" });
      // if (err.errorCode === "VALIDATION_FAILED") {
      //   setErrors(err.errors);
      //   return;
      // } else if (err.errorCode === "") {
      //   enqueueSnackbar(err.response.data.message, { variant: "error" });
      // }
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    setTimeout(() => {
      fetchMyAppointments();
    }, 0);

    return () => {
      if (signalRef.current) {
        signalRef.current.abort();
      }
    };
  }, []);
  return (
    <div className="w-full">
      <Box sx={{ width: "100%" }}>
        <Box sx={{ borderBottom: 1, borderColor: "divider" }}>
          <Tabs
            value={value}
            onChange={handleChange}
            aria-label="appointmentTypes"
            variant="standard"
          >
            <Tab key={"tab1"} label="مواعيدي" {...a11yProps(0)} />
            <Tab key={"tab2"} label="طلبات معاينة" {...a11yProps(1)} />
          </Tabs>
        </Box>

        <CustomTabPanel key="requested" value={value} index={0}>
          {data?.requestedAppointments?.length === 0 && (
            <div className="">
              <p className="">لا يوجد لديك طلبات بعد</p>
            </div>
          )}
          {data?.requestedAppointments?.map((day, ind) => (
            <AppointmentCard
              key={`day-${ind}`}
              day={day}
              type={"received"}
              onAccept={acceptAppointment}
              onReject={rejectAppointment}
              onCancel={cancelAppointment}
            />
          ))}
        </CustomTabPanel>
        <CustomTabPanel key="received" value={value} index={1}>
          {data?.receivedAppointments?.length === 0 && (
            <div className="">
              <p className="">لا يوجد لديك طلبات بعد</p>
            </div>
          )}
          {data?.receivedAppointments?.map((day, ind) => (
            <AppointmentCard
              key={`day-${ind}`}
              day={day}
              type={"received"}
              onAccept={acceptAppointment}
              onReject={rejectAppointment}
              onCancel={cancelAppointment}
            />
          ))}
        </CustomTabPanel>
      </Box>
    </div>
  );
};

export default MyAppointmnets;
