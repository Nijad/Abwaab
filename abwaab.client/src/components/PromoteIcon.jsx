import { SvgIcon } from "@mui/material";

const PromoteIcon = ({ sx }) => {
  return (
    <SvgIcon sx={sx}>
      <svg
        width="20"
        height="20"
        viewBox="0 0 20 20"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
      >
        <path
          d="M3.33334 10.8333V7.5C3.33334 6.58333 4.08334 5.83333 5.00001 5.83333H7.50001L15 2.5V15.8333L7.50001 12.5H5.00001C4.08334 12.5 3.33334 11.75 3.33334 10.8333Z"
          stroke="white"
          strokeWidth="1.5"
          strokeLinejoin="round"
        />
        <path
          d="M7.5 12.4997L8.75 16.6663H6.25L5 12.4997M16.6667 7.08301C17.75 8.74967 17.75 9.58301 16.6667 11.2497"
          stroke="white"
          strokeWidth="1.5"
          strokeLinecap="round"
          strokeLinejoin="round"
        />
      </svg>
    </SvgIcon>
  );
};

export default PromoteIcon;
