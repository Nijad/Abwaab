import { useEffect, useRef, useState } from "react";
import { useSnackbar } from "notistack";
import { appointmentsApi, profileApi, propertyApi } from "../../api";
import UserProperty from "../../components/UserProperty";
import { useNavigate } from "react-router";
import HomeIcon from "../../components/HomeIcon";
import AddNewProperty from "./AddNewProperty";

const MyPropertiesList = ({
  onAddProperty,
  onPromote,
  // onEdit,
  onVisitPreview,
  onSuccess,
}) => {
  const [data, setData] = useState([]);
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
      const resp = await propertyApi.userProperties(signalRef.current.signal);
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

  const handlePromote = (id, value) => {
    setData((prv) =>
      prv.map((itm) => {
        if (itm.propertyId == id) {
          return { ...itm, isStard: value };
        } else return itm;
      })
    );
  };
  useEffect(() => {
    setTimeout(() => {
      fetchMyProperties();
    }, 0);

    return () => {
      if (signalRef.current) {
        signalRef.current.abort();
      }
    };
  }, []);
  return (
    <>
      {data.length === 0 && (
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
            <AddNewProperty />
            {/* <Button
              size="small"
              color="navy"
              variant="contained"
              onClick={() => onAddProperty()}
              sx={{ marginY: "8px" }}
            >
              إضافة عقار
            </Button> */}
          </div>
        </div>
      )}
      {data.map((itm) => (
        <UserProperty
          key={`prop-${itm.propertyId}`}
          data={itm}
          onEdit={() => navigate(`edit/${itm.propertyId}`)}
          onChange={handlePromote}
          onVisitPreview={onVisitPreview}
          onAccept={acceptAppointment}
          onReject={rejectAppointment}
        />
      ))}
    </>
  );
};

export default MyPropertiesList;
