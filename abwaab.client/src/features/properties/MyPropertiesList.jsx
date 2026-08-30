import { useEffect, useRef, useState } from "react";
import { useSnackbar } from "notistack";
import { profileApi, propertyApi } from "../../api";
import UserProperty from "../../components/UserProperty";
import { useNavigate } from "react-router";
import HomeIcon from "../../components/HomeIcon";
import AddNewProperty from "./AddNewProperty";

const dataTest = [
  {
    propertyId: "d6a58b8b-2fba-49d4-8bac-4e738c781a64",
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
  {
    propertyId: "8baa7af8-4c2d-4cfc-afbc-e34ba2371c96",
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
  {
    propertyId: "s23-cxvz-dsaf34",
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

const MyPropertiesList = ({
  onAddProperty,
  onPromote,
  // onEdit,
  onVisitPreview,
  onSuccess,
}) => {
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
      {dataTest.map((itm) => (
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

export default MyPropertiesList;
