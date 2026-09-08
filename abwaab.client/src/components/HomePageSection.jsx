import { Button } from "@mui/material";
import React from "react";

const HomePageSection = ({
  title = "",
  description = "",
  showAllBtn,
  type = "",
  children,
}) => {
  return (
    <div className="max-w-7xl text-start w-full my-10">
      <div className="flex justify-between items-center">
        <div className="">
          <h3 className="text-navy-700 font-semibold text-[32px] ">{title}</h3>
          <p className="text-neutral-600  text-base mb-3">{description}</p>
        </div>
        <Button
          variant="outlined"
          color="navy"
          size="medium"
          onClick={() => showAllBtn && showAllBtn(type)}
        >
          عرض الكل
        </Button>
      </div>
      <div className="flex justify-between items-center gap-6 overflow-x-auto py-3">
        {children}
      </div>
    </div>
  );
};

export default HomePageSection;
