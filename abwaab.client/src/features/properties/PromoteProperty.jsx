import { Button } from "@mui/material";
import React, { useRef, useState } from "react";
import PromoteIcon from "../../components/PromoteIcon";
import { useSnackbar } from "notistack";
import { propertyApi } from "../../api";

const PromoteProperty = ({ onChange, propertyId, isPromoted }) => {
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const signalRef = useRef();
  const { enqueueSnackbar } = useSnackbar();

  const Promote = async () => {
    setLoading(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }

    try {
      signalRef.current = new AbortController();
      const resp = await propertyApi.starProperty(
        { propertyId },
        signalRef.current.signal
      );
      enqueueSnackbar(resp.data.message, { variant: "success" });
      if (onChange) onChange(propertyId, true);
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
  const UnPromote = async () => {
    setLoading(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }

    try {
      signalRef.current = new AbortController();
      const resp = await propertyApi.unStarProperty(
        { propertyId },
        signalRef.current.signal
      );
      enqueueSnackbar(resp.data.message, { variant: "success" });
      if (onChange) onChange(propertyId, false);
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
      sx={{ marginX: "4px" }}
      size="medium"
      variant="contained"
      color="sky"
      startIcon={<PromoteIcon />}
      onClick={() => {
        isPromoted ? UnPromote() : Promote();
      }}
    >
      {isPromoted ? "إلغاء ترويج العقار" : "ترويج العقار"}
    </Button>
    // </div>
  );
};

export default PromoteProperty;
