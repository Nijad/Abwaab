export const parseJwt = (token) => {
  try {
    const base64Url = token.split(".")[1];
    const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split("")
        .map((c) => "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2))
        .join("")
    );
    return JSON.parse(jsonPayload);
  } catch (e) {
    return null;
  }
};
/**
 * Detects whether an input string is an email, a mobile number, or invalid.
 *
 * @param {string} input - The string to evaluate (e.g., email or phone)
 * @returns {'email' | 'phone' | 'invalid'}
 */
export const detectIdentifierType = (input) => {
  if (!input || typeof input !== "string") return "invalid";

  const trimmedInput = input.trim();

  // 1. Regex for Email validation
  const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

  // 2. Regex for Mobile Phone Numbers
  // Supports international formats (e.g., +963912345678, 00963912345678, 0912345678, +1 123-456-7890)
  // Accepts 7 to 15 digits (E.164 standard) with optional +, spaces, dashes, or parentheses.
  const phoneRegex =
    /^(\+?\d{1,4}[-.\s]?)?(\(?\d{1,4}\)?[-.\s]?)?\d{3,4}[-.\s]?\d{3,4}$/;

  if (emailRegex.test(trimmedInput)) {
    return "email";
  }

  // Ensure it has enough digits to be a real phone number
  const digitCount = (trimmedInput.match(/\d/g) || []).length;
  if (phoneRegex.test(trimmedInput) && digitCount >= 7 && digitCount <= 15) {
    return "phone";
  }

  return "invalid";
};

// Format total seconds into HH:MM:SS
export const formatTime = (totalSeconds) => {
  const hours = Math.floor(totalSeconds / 3600);
  const minutes = Math.floor((totalSeconds % 3600) / 60);
  const seconds = totalSeconds % 60;

  const pad = (num) => String(num).padStart(2, "0");

  return `${pad(hours)}:${pad(minutes)}:${pad(seconds)}`;
};
