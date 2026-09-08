import React from "react";
import LabelTag from "./LabelTag";
import { Button } from "@mui/material";
import { EditOutlined } from "@mui/icons-material";
import PromoteIcon from "./PromoteIcon";
import PromoteProperty from "../features/properties/PromoteProperty";
import PreviewPropertyVisits from "../features/properties/PreviewPropertyVisits";
import { Link } from "react-router";

const UserProperty = ({ data, onPromote, onEdit, onVisitPreview }) => {
  return (
    <div className="p-4 my-4 border border-neutral-400 w-full rounded-lg">
      <div className="flex items-center border-b border-b-neutral-400 gap-3 py-3">
        <div className="rounded-lg overflow-hidden">
          <img
            src={`${import.meta.env.VITE_API_BASE_URL}${data.coverImage}`}
            alt="house"
            className="w-[120px] h-[120px]"
          />
        </div>
        <div className="flex-1">
          <LabelTag
            label={data.propertyType}
            classes="bg-sky-100 text-navy-700 px-3 min-w-10"
          />
          <Link
            to={`/portal/properties/${data.propertyId}`}
            className="text-3xl text-neutral-900 text-ellipsis "
          >
            {data.title}
          </Link>
        </div>
        <div className="min-w-[30%] flex flex-wrap items-center justify-between">
          <div className=" text- text-end pb-5">
            <LabelTag
              label={data.propertyState}
              classes="rounded-full px-3 bg-sky-100 text-navy-700 inline-block"
            />
          </div>
          <div className="">
            <p className="text-neutral-700 text-xs">المساحة</p>
            <p className="text-navy-700 text-base">
              {data.areaInSquareMeter} م<sup>2</sup>
            </p>
          </div>
          <div className="">
            <p className="text-neutral-700 text-xs">السعر</p>
            <p className="text-navy-700 text-base">
              {data.price?.toLocaleString()} دولار امريكي
            </p>
          </div>
        </div>
        <div className="">
          <PromoteProperty propertyId={data.propertyId} />
          {/* <Button
            sx={{ marginX: "4px" }}
            size="medium"
            variant="contained"
            color="sky"
            startIcon={<PromoteIcon />}
            onClick={() => onPromote(data.propertyId)}
          >
            ترويج العقار
          </Button> */}
          <Button
            sx={{ marginX: "4px" }}
            size="medium"
            variant="outlined"
            color="navy"
            startIcon={<EditOutlined />}
            onClick={() => onEdit()}
          >
            تعديل
          </Button>
        </div>
      </div>
      <div className="flex justify-between items-center mt-4">
        {data.visitRequest > 0 && (
          <p className="text-neutral-900 text-base">
            لديك {data.visitRequests} طلبات لمعاينة هذا العقار
          </p>
        )}
        {data.visitRequest == 0 && (
          <p className="">لا توجد طلبات معاينة لهذا العقار</p>
        )}
        <PreviewPropertyVisits disabled />
      </div>
    </div>
  );
};

export default UserProperty;
