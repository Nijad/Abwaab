import { Button } from "@mui/material";
import React, { useRef, useState } from "react";
import { useNavigate } from "react-router";
import MyPropertiesList from "../features/properties.jsx/MyPropertiesList";
import { propertyApi } from "../api";
import { useSnackbar } from "notistack";
import PreviewPropertyVisits from "../features/properties.jsx/PreviewPropertyVisits";

const MyProperties = () => {
  const [showVisits, setShowVisits] = useState(false);
  const navigate = useNavigate();
  const signalRef = useRef();
  const { enqueueSnackbar } = useSnackbar();

  const rejectVisit = async (id) => {
    if (signalRef.current) {
      signalRef.current.abort();
    }
    try {
      signalRef.current = new AbortController();
      const resp = await propertyApi.rejectVisit(signalRef.current.signal);
    } catch (err) {
      //list related error codes
      enqueueSnackbar(err, { variant: "error" });
    } finally {
    }
  };
  const promoteProperty = async (id) => {
    if (signalRef.current) {
      signalRef.current.abort();
    }
    try {
      signalRef.current = new AbortController();
      const resp = await propertyApi.starProperty(signalRef.current.signal);
    } catch (err) {
      //list related error codes
      enqueueSnackbar(err, { variant: "error" });
    } finally {
    }
  };

  return (
    <div className="flex flex-col items-center py-8 px-28 flex-grow">
      <div className="flex justify-between items-center w-full">
        <div className="">
          <h4 className="text-navy-700 font-semibold text-[32px]">
            إدارة العقارات
          </h4>
          <p className="text-neutral-700 text-lg">
            أضف عقاراتك وتحكم بها من مساحة واحدة
          </p>
        </div>
        <div className="">
          <Button
            size="small"
            color="navy"
            variant="contained"
            onClick={() => navigate("add")}
          >
            إضافة عقار
          </Button>
        </div>
      </div>
      <MyPropertiesList
        onEdit={(id) => navigate(`/portal/my-properties/edit/${id}`)}
        onPromote={promoteProperty}
        onSuccess={null}
        onVisitPreview={() => setShowVisits(true)}
      />
      {showVisits && (
        <PreviewPropertyVisits
          onClose={() => setShowVisits(false)}
          onReject={rejectVisit}
        />
      )}
    </div>
  );
};

export default MyProperties;
