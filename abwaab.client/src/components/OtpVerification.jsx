import React, { useState, useRef } from "react";
import { Button, Grid, TextField } from "@mui/material";
import { detectIdentifierType } from "../utils/helpers";

/**
 * Reusable 6-Digit OTP Verification Component
 *
 * @param {string} identifier - The identifier either email or phone number (e.g., "someone@gmail.com")
 * @param {function} onVerify - Callback function triggered when submitting the code
 * @param {function} onResend - Optional callback for resending the code
 */
const OtpVerification = ({ identifier, onVerify, onResend }) => {
  const [otp, setOtp] = useState(["", "", "", "", "", ""]);
  const inputRefs = useRef([]);
  const identifierType = detectIdentifierType(identifier);

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

  // Submit Handler
  const handleSubmit = (e) => {
    e.preventDefault();
    const completeOtp = otp.join("");
    if (completeOtp.length === 6) {
      if (onVerify) onVerify(completeOtp);
    }
  };

  const isComplete = otp.join("").length === 6;

  return (
    <div className="mx-auto w-full max-w-md rounded-2xl bg-white p-0 shadow-sm dark:bg-slate-900 my-3">
      {/* Title & Sent Destination */}
      <div className="mb-6 ">
        <p className="text-base text-sky-700">
          أرسلنا رمز تحقق مكوناً من 6 أرقام الى
          {identifierType === "email"
            ? " البريد الإلكتروني"
            : identifierType === "phone"
            ? " الرقم"
            : ""}
        </p>
        <p className="text-navy-700 font-semibold text-lg text-end" dir="ltr">
          {identifier}
        </p>
        <p className="mt-2 text-xs text-neutral-700 ">
          *تنتهي صلاحية الرمز بعد 5 دقائق.
        </p>
      </div>

      {/* 6-Digit Inputs Grid */}
      <form onSubmit={handleSubmit}>
        <div className="mb-8 flex justify-between gap-2 sm:gap-3" dir="ltr">
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

        {/* Submit Button */}
        <Button
          type="submit"
          variant="contained"
          disabled={!isComplete}
          color="navy"
          fullWidth
          sx={{ padding: 1.5 }}
          className="disabled:!bg-neutral-200"
        >
          تحقق ومتابعة
        </Button>
        {/* <button
          type="submit"
          disabled={!isComplete}
          className={`w-full rounded-xl py-3.5 text-base font-bold text-white shadow-md transition-all ${
            isComplete
              ? "bg-[#0F2847] hover:bg-[#163862] active:scale-[0.99]"
              : "cursor-not-allowed bg-slate-300 dark:bg-slate-700"
          }`}
        >
          تحقق والمتابعة
        </button> */}
      </form>
    </div>
  );
};

export default OtpVerification;
