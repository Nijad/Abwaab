import React from "react";
import LabelTag from "./LabelTag";
import { Button } from "@mui/material";
import { EditOutlined } from "@mui/icons-material";
import PromoteIcon from "./PromoteIcon";

const UserProperty = ({ data, onPromote, onEdit, onVisitPreview }) => {
  return (
    <div className="p-4 my-4 border border-neutral-400 w-full rounded-lg">
      <div className="flex items-center border-b border-b-neutral-400 gap-3 py-3">
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
          <Button
            sx={{ marginX: "4px" }}
            size="medium"
            variant="contained"
            color="sky"
            startIcon={<PromoteIcon />}
            onClick={() => onPromote(data.propertyId)}
          >
            ترويج العقار
          </Button>
          <Button
            sx={{ marginX: "4px" }}
            size="medium"
            variant="outlined"
            color="navy"
            startIcon={<EditOutlined />}
            onClick={() => onEdit(data.propertyId)}
          >
            تعديل
          </Button>
        </div>
      </div>
      <div className="flex justify-between items-center mt-4">
        {data.visitRequests > 0 && (
          <React.Fragment>
            <p className="text-neutral-900 text-base">
              لديك {data.visitRequests} طلبات لمعاينة هذا العقار
            </p>
            <Button
              size="medium"
              variant="outlined"
              color="navy"
              onClick={() => onVisitPreview(data.propertyId)}
            >
              عرض طلبات المعاينة
            </Button>
          </React.Fragment>
        )}
        {data.visitRequests === 0 && (
          <p className="">لا توجد طلبات معاينة لهذا العقار</p>
        )}
      </div>
    </div>
  );
};

export default UserProperty;
