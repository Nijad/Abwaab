import { alpha, styled } from "@mui/material/styles";
import FormGroup from "@mui/material/FormGroup";
import FormControlLabel from "@mui/material/FormControlLabel";
import Switch from "@mui/material/Switch";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import { teal } from "@mui/material/colors";

const TealSwitch = styled(Switch)(({ theme }) => ({
  "& .MuiSwitch-switchBase.Mui-checked": {
    color: "#fff",
    "&:hover": {
      backgroundColor: alpha(teal[600], theme.palette.action.hoverOpacity),
    },
  },
  "& .MuiSwitch-switchBase.Mui-checked + .MuiSwitch-track": {
    backgroundColor: "#169A94",
    opacity: 1,
  },
  "&.MuiSwitch-root": {
    margin: "3px",
  },
}));

const StyledSwitch = ({ checked, onChange, name }) => {
  // return <Switch sx={{ m: 1 }} checked={} />;
  return (
    <TealSwitch
      sx={{ m: 1 }}
      checked={checked}
      onChange={onChange}
      name={name}
    />
  );
};
export default StyledSwitch;
