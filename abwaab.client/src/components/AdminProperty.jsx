import React from "react";
import LabelTag from "./LabelTag";
import { Button } from "@mui/material";
import { EditOutlined } from "@mui/icons-material";
import PromoteIcon from "./PromoteIcon";
import PromoteProperty from "../features/properties/PromoteProperty";
import PreviewPropertyVisits from "../features/properties/PreviewPropertyVisits";
import { useNavigate } from "react-router";

const UserProperty = ({ data, onPromote, onEdit, onVisitPreview }) => {
  const navigate = useNavigate();
  return (
    <div className="p-4 my-4 border border-neutral-400 w-full rounded-lg">
      <div className="flex items-center gap-3 py-3">
        <div className="rounded-lg overflow-hidden">
          <img
            src={data.coverImage}
            alt="house"
            className="w-[120px] h-[120px]"
          />
        </div>
        <div className="flex-1">
          <LabelTag
            label={data.propertyType}
            classes="bg-sky-50 text-navy-700"
          />
          <h3 className="text-3xl text-neutral-900 text-ellipsis">
            {data.title}
          </h3>
        </div>
        <div className="min-w-[27%] flex items-center">
          <div className="min-w-[50%]">
            <p className="text-neutral-700 text-xs">المساحة</p>
            <p className="text-navy-700 text-base">
              {data.areaInSquareMeter} م<sup>2</sup>
            </p>
          </div>
          <div className="min-w-[50%]">
            <p className="text-neutral-700 text-xs">السعر</p>
            <p className="text-navy-700 text-base">{data.price} ليرة سورية</p>
          </div>
        </div>
        <div className="">
          <LabelTag
            label={data.propertyStat}
            className="rounded-full bg-navy-600 text-white"
          />
          <Button
            sx={{ marginX: "4px" }}
            size="medium"
            variant="outlined"
            color="navy"
            startIcon={<EditOutlined />}
            onClick={() => navigate(`properties/${data.propertyId}`)}
          >
            تعديل
          </Button>
        </div>
      </div>
    </div>
  );
};

export default UserProperty;
