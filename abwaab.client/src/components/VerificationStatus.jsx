import { Circle } from "@mui/icons-material";
import React from "react";

const VerificationStatus = ({ isVerified = false, label }) => {
  const colors = isVerified
    ? "bg-success-100 text-success-600"
    : "bg-warning-100 text-warning-500";
  return (
    <p
      className={`${colors} font-semibold text-sm rounded-full w-fit py-1 px-3 mx-0`}
    >
      {label}
      <Circle sx={{ width: "13px", marginInlineStart: 1 }} />
    </p>
  );
};

export default VerificationStatus;
