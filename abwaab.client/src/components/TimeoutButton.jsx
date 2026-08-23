import { useState, useEffect } from "react";
import { Button, Typography, Box } from "@mui/material";
import RefreshIcon from "@mui/icons-material/Refresh";

export const TimeoutButton = ({
  label = "",
  buttonLabel = "إعادة الإرسال",
  seconds = 197,
  onResend,
}) => {
  const [timeLeft, setTimeLeft] = useState(seconds);

  useEffect(() => {
    // Reset timer state whenever initial `seconds` prop changes
    setTimeout(() => {
      setTimeLeft(seconds);
    }, 0);
  }, [seconds]);

  useEffect(() => {
    if (timeLeft <= 0) return;

    const intervalId = setInterval(() => {
      setTimeLeft((prevTime) => prevTime - 1);
    }, 1000);

    return () => clearInterval(intervalId);
  }, [timeLeft]);

  // Helper to format total seconds into MM:SS
  const formatTime = (totalSeconds) => {
    const minutes = Math.floor(totalSeconds / 60);
    const secs = totalSeconds % 60;
    const formattedMinutes = String(minutes).padStart(2, "0");
    const formattedSeconds = String(secs).padStart(2, "0");
    return `${formattedMinutes}:${formattedSeconds}`;
  };

  const handleButtonClick = () => {
    if (onResend) {
      onResend();
    }
    // Restart countdown after button click
    setTimeLeft(seconds);
  };

  return (
    <Box
      className="flex items-center justify-start py-2 my-3 "
      sx={{ marginX: 0 }}
    >
      {timeLeft > 0 ? (
        <Typography variant="body1" className="text-gray-600 font-medium">
          {label}
          <span className="font-semibold text-blue-600 px-2">
            {formatTime(timeLeft)}
          </span>
        </Typography>
      ) : (
        <Button
          variant="text"
          color={"sky"}
          endIcon={<RefreshIcon />}
          onClick={handleButtonClick}
          className="capitalize font-semibold shadow-mdhover:shadow-l transition-all"
        >
          {buttonLabel}
        </Button>
      )}
    </Box>
  );
};
export default TimeoutButton;
