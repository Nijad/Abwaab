import { useEffect, useRef, useState } from "react";
import { useSnackbar } from "notistack";
import { profileApi, propertyApi } from "../../api";
import UserProperty from "../../components/UserProperty";
import { useNavigate } from "react-router";
import HomeIcon from "../../components/HomeIcon";
import AddNewProperty from "./AddNewProperty";
import AdminProperty from "../../components/AdminProperty";

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
      const resp = await propertyApi.pendingProperties(
        signalRef.current.signal
      );
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
          </div>
        </div>
      )}
      {data.map((itm) => (
        <AdminProperty
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
