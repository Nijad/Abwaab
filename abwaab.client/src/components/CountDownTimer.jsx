import React, { useState, useEffect, useRef } from "react";
// import { Clock, AlertTriangle } from "lucide-react";

/**
 * Reusable Countdown Timer Component
 *
 * @param {number} initialSeconds - Total countdown duration in seconds
 * @param {function} onComplete - Callback function fired when timer hits 0
 * @param {boolean} autoStart - Automatically start counting on mount
 */
const CountdownTimer = ({
  initialSeconds = 1800, // Default 30 minutes
  onComplete,
  autoStart = true,
}) => {
  const [secondsLeft, setSecondsLeft] = useState(initialSeconds);
  const [isRunning, setIsRunning] = useState(autoStart);
  const timerRef = useRef(null);

  // Sync state if initialSeconds prop updates dynamically
  useEffect(() => {
    const to = setTimeout(() => {
      setSecondsLeft(initialSeconds);
    }, 0);
    return () => clearTimeout(to);
  }, [initialSeconds]);

  // Main countdown logic
  useEffect(() => {
    if (isRunning && secondsLeft > 0) {
      timerRef.current = setInterval(() => {
        setSecondsLeft((prev) => prev - 1);
      }, 1000);
    } else if (secondsLeft === 0) {
      setIsRunning(false);
      if (onComplete) onComplete();
    }

    return () => clearInterval(timerRef.current);
  }, [isRunning, secondsLeft, onComplete]);

  // Format total seconds into HH:MM:SS
  const formatTime = (totalSeconds) => {
    const hours = Math.floor(totalSeconds / 3600);
    const minutes = Math.floor((totalSeconds % 3600) / 60);
    const seconds = totalSeconds % 60;

    const pad = (num) => String(num).padStart(2, "0");

    return `${pad(hours)}:${pad(minutes)}:${pad(seconds)}`;
  };

  // Calculate percentage remaining for the progress bar
  const progressPercentage = Math.max(
    0,
    Math.min(100, (secondsLeft / initialSeconds) * 100)
  );

  // Dynamic visual indicators based on urgency
  const isWarning = secondsLeft <= 300 && secondsLeft > 0; // Less than 5 mins
  const isExpired = secondsLeft === 0;

  return (
    <div className="w-full max-w-md rounded-xl border border-slate-200 bg-white p-4 shadow-sm dark:border-slate-800 dark:bg-slate-900">
      {/* Header Info */}
      <div className="mb-2 flex items-center justify-between">
        <span className="flex items-center gap-1.5 text-xs font-semibold text-slate-500 dark:text-slate-400">
          {/* <Clock className="h-4 w-4" /> */}
          {isExpired ? "الوقت المتبقي" : "الوقت المتبقي"}
        </span>

        {/* Time Display */}
        <span
          className={`font-mono text-lg font-bold transition-colors ${
            isExpired
              ? "text-red-600 dark:text-red-400"
              : isWarning
              ? "animate-pulse text-amber-500"
              : "text-slate-900 dark:text-white"
          }`}
        >
          {formatTime(secondsLeft)}
        </span>
      </div>

      {/* Progress Bar Container */}
      <div className="relative h-2.5 w-full overflow-hidden rounded-full bg-slate-100 dark:bg-slate-800">
        <div
          className={`h-full rounded-full transition-all duration-1000 ease-linear ${
            isExpired
              ? "bg-red-500"
              : isWarning
              ? "bg-amber-500"
              : "bg-blue-600"
          }`}
          style={{ width: `${progressPercentage}%` }}
        />
      </div>

      {/* Warning Banner for Low Time */}
      {isWarning && !isExpired && (
        <p className="mt-2 flex items-center gap-1 text-[11px] text-amber-600 dark:text-amber-400">
          {/* <AlertTriangle className="h-3 w-3 shrink-0" /> */}
          تنبيه: أوشكت الجلسة على الانتهاء!
        </p>
      )}
    </div>
  );
};
export default CountdownTimer;
