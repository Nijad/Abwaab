import { useState, useRef, useEffect } from "react";

/**
 * Reusable 6-Digit OTP Input Component
 *
 * @param {function} onChange - Callback function triggered when entring code
 */
const OtpVerification = ({ onChange }) => {
  const [otp, setOtp] = useState(["", "", "", "", "", ""]);
  const inputRefs = useRef([]);

  // Handle single digit input
  const handleChange = (e, index) => {
    const value = e.target.value;

    // Only allow numbers
    if (value && !/^\d+$/.test(value)) return;

    const newOtp = [...otp];
    // Take only the last entered digit if user types multiple characters
    newOtp[index] = value.substring(value.length - 1);
    setOtp(newOtp);

    // Automatically move focus to the next input if a digit was entered
    if (value && index < 5) {
      inputRefs.current[index + 1]?.focus();
    }
  };

  // Handle Backspace navigation
  const handleKeyDown = (e, index) => {
    if (e.key === "Backspace") {
      if (otp[index] === "" && index > 0) {
        // If current input is empty, move back and focus previous input
        inputRefs.current[index - 1]?.focus();
      }
    }
  };

  // Handle Pasting full 6-digit codes (e.g., "137952")
  const handlePaste = (e) => {
    e.preventDefault();
    const pastedData = e.clipboardData.getData("text").trim();

    // Check if pasted content consists of 6 digits
    if (/^\d{6}$/.test(pastedData)) {
      const digits = pastedData.split("");
      setOtp(digits);
      // Focus the last input element
      inputRefs.current[5]?.focus();
    }
  };
  useEffect(() => {
    onChange(otp.join(""));
  }, [otp, onChange]);

  return (
    <div className="mx-auto w-full max-w-md rounded-2xl bg-white p-0  dark:bg-slate-900 my-3">
      {/* 6-Digit Inputs Grid */}
      <div className="mb-3 flex justify-between gap-2 sm:gap-3" dir="ltr">
        {otp.map((digit, index) => (
          <input
            key={index}
            type="text"
            inputMode="numeric"
            maxLength={1}
            value={digit}
            ref={(el) => (inputRefs.current[index] = el)}
            onChange={(e) => handleChange(e, index)}
            onKeyDown={(e) => handleKeyDown(e, index)}
            onPaste={handlePaste}
            className={`h-14 w-12 rounded-xl bg-neutral-100 text-center text-xl font-bold transition-all focus:outline-teal-400 focus:border-tea sm:h-16 sm:w-14 ${
              digit
                ? "border-emerald-500 bg-emerald-50/20 text-slate-900 dark:bg-slate-800 dark:text-white"
                : "border-slate-100 bg-slate-100/70 text-slate-900 focus:border-emerald-500 focus:bg-white focus:ring-2 focus:ring-emerald-500/20 dark:border-slate-800 dark:bg-slate-800 dark:text-white"
            }`}
          />
        ))}
      </div>
    </div>
  );
};

export default OtpVerification;
