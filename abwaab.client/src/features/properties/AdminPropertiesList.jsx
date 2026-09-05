import { useEffect, useRef, useState } from "react";
import { useSnackbar } from "notistack";
import { profileApi, propertyApi } from "../../api";
import UserProperty from "../../components/UserProperty";
import { useNavigate } from "react-router";
import HomeIcon from "../../components/HomeIcon";
import AddNewProperty from "./AddNewProperty";

const AdminPropertiesList = ({
  onPromote,
  // onEdit,
  onVisitPreview,
}) => {
  const [data, setData] = useState([]);
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const signalRef = useRef();
  const { enqueueSnackbar } = useSnackbar();
  const navigate = useNavigate();

  const fetchPeningProperties = async () => {
    setLoading(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }
    try {
      signalRef.current = new AbortController();
      const resp = await propertyApi.userProperties(signalRef.current.signal);
      //   enqueueSnackbar(resp.data.message, { variant: "success" });
      setData(resp.data);
    } catch (err) {
      if (err.detail) enqueueSnackbar(err.detail, { variant: "error" });
      if (!err.detail) enqueueSnackbar(err, { variant: "error" });
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    setTimeout(() => {
      fetchPeningProperties();
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
          data={itm}
          onEdit={() => navigate(`edit/${itm.propertyId}`)}
          onPromote={onPromote}
          onVisitPreview={onVisitPreview}
        />
      ))}
    </>
  );
};

export default AdminPropertiesList;
