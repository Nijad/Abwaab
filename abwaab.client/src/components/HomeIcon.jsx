import { SvgIcon } from "@mui/material";

const HomeIcon = () => {
  return (
    <SvgIcon sx={{ width: "4em", height: "4em" }}>
      <svg
        width="72"
        height="72"
        viewBox="0 0 72 72"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
      >
        <path
          d="M0 8C0 3.58172 3.58172 0 8 0H64C68.4183 0 72 3.58172 72 8V64C72 68.4183 68.4183 72 64 72H8C3.58172 72 0 68.4183 0 64V8Z"
          fill="#EDF8FD"
        />
        <path
          d="M22.5 33.75L36 22.5L49.5 33.75V48H39V39H33V48H22.5V33.75Z"
          stroke="#3598C9"
          stroke-width="2.4"
          stroke-linejoin="round"
        />
        <path
          d="M45 25.5V30"
          stroke="#3598C9"
          stroke-width="2.4"
          stroke-linecap="round"
        />
      </svg>
    </SvgIcon>
  );
};

export default HomeIcon;
