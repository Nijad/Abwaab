import { Button } from "@mui/material";
import React, { useRef, useState } from "react";
import { useNavigate } from "react-router";
import MyPropertiesList from "../features/properties/MyPropertiesList";
import { propertyApi } from "../api";
import { useSnackbar } from "notistack";
import PreviewPropertyVisits from "../features/properties/PreviewPropertyVisits";
import AddNewProperty from "../features/properties/AddNewProperty";
import useAuth from "../hooks/useAuth";
import AdminPropertiesList from "../features/properties/AdminPropertiesList";

const MyProperties = () => {
  const { isAdmin } = useAuth();
  const [showVisits, setShowVisits] = useState(false);
  const [loading, setLoading] = useState(false);
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

  const addNewProperty = async () => {
    setLoading(true);
    if (signalRef.current) {
      signalRef.current.abort();
    }
    try {
      signalRef.current = new AbortController();
      const resp = await propertyApi.addProperty(signalRef.current.signal);
      enqueueSnackbar(resp.data.message, { variant: "success" });
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

  return (
    <div className="flex flex-col items-center py-8 px-28 flex-grow">
      <div className="flex justify-between items-center w-full">
        <div className="">
          <h4 className="text-navy-700 font-semibold text-[32px]">
            إدارة العقارات
          </h4>
          {!isAdmin && (
            <p className="text-neutral-700 text-lg">
              أضف عقاراتك وتحكم بها من مساحة واحدة
            </p>
          )}
          {isAdmin && (
            <p className="text-neutral-700 text-lg">
              استعراض طلبات نشر العقارات
            </p>
          )}
        </div>
        {!isAdmin && (
          <div className="">
            <AddNewProperty />
          </div>
        )}
      </div>
      {!isAdmin && (
        <MyPropertiesList
          onAddProperty={addNewProperty}
          onEdit={(id) => navigate(`/portal/my-properties/edit/${id}`)}
          onPromote={promoteProperty}
          onSuccess={null}
          onVisitPreview={() => setShowVisits(true)}
        />
      )}
      {isAdmin && (
        <AdminPropertiesList
          onAddProperty={addNewProperty}
          onEdit={(id) => navigate(`/portal/my-properties/edit/${id}`)}
          onPromote={promoteProperty}
          onSuccess={null}
          onVisitPreview={() => setShowVisits(true)}
        />
      )}
    </div>
  );
};

export default MyProperties;
