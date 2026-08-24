import { Button } from "@mui/material";
import React from "react";
import { useNavigate } from "react-router";
import MyPropertiesList from "../features/properties.jsx/MyPropertiesList";

const MyProperties = () => {
  const navigate = useNavigate();

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
      <MyPropertiesList />
    </div>
  );
};

export default MyProperties;
