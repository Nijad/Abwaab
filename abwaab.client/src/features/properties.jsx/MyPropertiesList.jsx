import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  TextField,
} from "@mui/material";
import { useEffect, useRef, useState } from "react";
import ShowErrors from "../../components/ShowErrors";
import { useSnackbar } from "notistack";
import { profileApi, propertyApi } from "../../api";
import UserProperty from "../../components/UserProperty";
import { useNavigate } from "react-router";
import HomeIcon from "../../components/HomeIcon";

const dataTest = [
  {
    propertyId: "1231-sdf1121-14fs",
    coverImage:
      "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRiBv7lL1ce0REGvvOPJsrBALnXqqjgD7svLzpZmlf0gg&s=10",
    title: "شقة سكنية بمشروع دمر",
    address: "دمشق مشروع دمر الجزيرة 3",
    propertyType: "شقة سكنية",
    propertyFinishing: "سوبر ديلوكس",
    price: "3000000000",
    areaInSquareMeter: 155,
    visitRequests: 3,
  },
];

const MyPropertiesList = ({ onPromote, onEdit, onVisitPreview, onSuccess }) => {
  const [data, setData] = useState({});
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const signalRef = useRef();
  const { enqueueSnackbar } = useSnackbar();
  const navigate = useNavigate();

  const fetchMyProperties = async () => {
    setLoading(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }
    try {
      signalRef.current = new AbortController();
      const resp = await propertyApi.get(signalRef.current.signal);
      //   enqueueSnackbar(resp.data.message, { variant: "success" });
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
  useEffect(() => {
    setTimeout(() => {
      fetchMyProperties();
    }, 0);

    return () => {
      signalRef.current.abort();
    };
  }, []);
  return (
    <>
      {dataTest.length === 0 && (
        <div
          id="prop-area"
          className="flex items-center justify-center w-full flex-1"
        >
          <div className="rounded-2xl border-neutral-400 bg-white text-center p-10">
            <HomeIcon />
            <h5 className="text-xl font-semibold text-navy-700 p-4 my-2">
              لاتوجد عقارات مضافة بعد
            </h5>
            <p className="text-base text-neutral-700 p-3">
              ابدأ بإضافة أول عقار ليظهر هنا وتتمكن من تعديله أو الترويح له
              لاحقا
            </p>
            <Button
              size="small"
              color="navy"
              variant="contained"
              onClick={() => navigate("add")}
              sx={{ marginY: "8px" }}
            >
              إضافة عقار
            </Button>
          </div>
        </div>
      )}
      {dataTest.map((itm) => (
        <UserProperty
          data={itm}
          onEdit={onEdit}
          onPromote={onPromote}
          onVisitPreview={onVisitPreview}
        />
      ))}
    </>
  );
};

export default MyPropertiesList;
