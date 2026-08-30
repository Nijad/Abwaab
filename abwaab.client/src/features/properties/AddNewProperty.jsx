import { Button } from "@mui/material";
import { useRef, useState } from "react";
import PromoteIcon from "../../components/PromoteIcon";
import { useSnackbar } from "notistack";
import { propertyApi } from "../../api";
import { useNavigate } from "react-router";

const AddNewProperty = () => {
  const [loading, setLoading] = useState(false);
  const signalRef = useRef();
  const { enqueueSnackbar } = useSnackbar();
  const navigate = useNavigate();

  const AddProperty = async () => {
    setLoading(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }

    try {
      signalRef.current = new AbortController();
      const resp = await propertyApi.addProperty(signalRef.current.signal);
      enqueueSnackbar(resp.data.message, { variant: "success" });
      navigate(`add/${resp.data.propertyId}`);
      //   if (onSuccess) onSuccess(data.newEmail, resp.data);
    } catch (err) {
      //list related error codes
      if (err.errorCode === "VALIDATION_FAILED") {
        // setErrors(err.errors);
        enqueueSnackbar(err.detail, { variant: "error" });
        return;
      } else if (err.errorCode) {
        enqueueSnackbar(err.detail, { variant: "error" });
      } else {
        enqueueSnackbar(err, { variant: "error" });
      }
    } finally {
      setLoading(false);
    }
  };
  return (
    // <div>
    <Button
      loading={loading}
      disabled={loading}
      sx={{ marginY: "8px" }}
      size="small"
      variant="contained"
      color="sky"
      startIcon={<PromoteIcon />}
      onClick={() => AddProperty()}
    >
      إضافة عقار
    </Button>
    // </div>
  );
};

export default AddNewProperty;
