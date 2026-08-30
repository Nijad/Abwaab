import useAuth from "../hooks/useAuth";

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

export const avatarString = (name) => {
  var parts = name.split(" ");
  if (parts.count > 1) {
    return `${name.split(" ")[0][0]}${name.split(" ")[1][0]}`;
  } else {
    return name[0];
  }
};

export const timeSlots = (before = null, after = null) => {
  const slots = [
    "09:00:00",
    "09:30:00",
    "10:00:00",
    "10:30:00",
    "11:00:00",
    "11:30:00",
    "12:00:00",
    "12:30:00",
    "13:00:00",
    "13:30:00",
    "14:00:00",
    "14:30:00",
    "15:00:00",
    "15:30:00",
    "16:00:00",
    "16:30:00",
    "17:00:00",
    "17:30:00",
    "18:00:00",
    "18:30:00",
    "19:00:00",
    "19:30:00",
    "20:00:00",
    "20:30:00",
    "21:00:00",
  ];
  const result = [];
  // debugger;
  if (before) {
    for (const slot of slots) {
      if (before !== slot) {
        result.push(slot);
      } else break;
    }
    return result;
  } else if (after) {
    var indx = slots.findIndex((s) => s === after);
    if (indx !== -1) {
      for (indx++; indx < slots.length; indx++) {
        result.push(slots[indx]);
      }
    }
    return result;
  } else return slots;
};

/**
 * Generates an array of 30-minute time slots between two times.
 * @param {string} startTime - Format "HH:MM" (e.g., "10:30")
 * @param {string} endTime - Format "HH:MM" (e.g., "15:00" or "03:00")
 * @returns {Array<{startTime: string, endTime: string}>}
 */
export function generateTimeSlots(day, startTime, endTime) {
  const slots = [];

  // Helper to convert "HH:MM" string to total minutes from midnight
  const parseMinutes = (timeStr) => {
    const [hours, minutes] = timeStr.split(":").map(Number);
    return hours * 60 + minutes;
  };

  // Helper to convert total minutes back to double-digit "HH:MM" format
  const formatTime = (totalMinutes) => {
    const hours = Math.floor(totalMinutes / 60) % 24;
    const minutes = totalMinutes % 60;
    const formattedHours = String(hours).padStart(2, "0");
    const formattedMinutes = String(minutes).padStart(2, "0");
    return `${formattedHours}:${formattedMinutes}:00`;
  };

  let startMins = parseMinutes(startTime);
  let endMins = parseMinutes(endTime);

  // Handle overnight/over-midnight periods (e.g., 22:00 to 02:00)
  if (endMins < startMins) {
    endMins += 24 * 60;
  }

  const SLOT_DURATION = 30; // in minutes

  // Generate slots in 30-minute increments
  while (startMins + SLOT_DURATION <= endMins) {
    const slotStart = formatTime(startMins);
    const slotEnd = formatTime(startMins + SLOT_DURATION);

    slots.push({
      timeSlotId: null,
      day: day.dayIndex,
      dayName: day.dayName,
      startTime: slotStart,
      endTime: slotEnd,
      notes: "",
    });

    startMins += SLOT_DURATION;
  }

  return slots;
}

/**
 * Generate list of week days with from | to times.
 * @param {string} startTime - Format "HH:MM" (e.g., "10:30")
 * @param {string} endTime - Format "HH:MM" (e.g., "15:00" or "03:00")
 * @returns {Array<{startTime: string, endTime: string}>}
 */
export function collapseTimeSlots(timeSlots = [], weekDaysList, endTime) {
  // debugger;
  const slots = {};
  if (timeSlots.length == 0) {
    for (const day of weekDaysList) {
      // slots.push({ [day.dayIndex]: { name: day.dayName, from: "", to: "" } });
      slots[day.dayIndex] = {
        name: day.dayName,
        checked: false,
        startTime: "",
        endTime: "",
      };
    }
    return slots;
  }

  // Helper to convert "HH:MM" string to total minutes from midnight
  const parseMinutes = (timeStr) => {
    const [hours, minutes] = timeStr.split(":").map(Number);
    return hours * 60 + minutes;
  };

  // Helper to convert total minutes back to double-digit "HH:MM" format
  const formatTime = (totalMinutes) => {
    const hours = Math.floor(totalMinutes / 60) % 24;
    const minutes = totalMinutes % 60;
    const formattedHours = String(hours).padStart(2, "0");
    const formattedMinutes = String(minutes).padStart(2, "0");
    return `${formattedHours}:${formattedMinutes}:00`;
  };

  for (const day of weekDaysList) {
    const daySlots = timeSlots.filter((ts) => ts.day === day.dayIndex);
    const temp = [];
    if (daySlots.length > 0) {
      for (const ds of daySlots) {
        temp.push(parseMinutes(ds.startTime));
        temp.push(parseMinutes(ds.endTime));
      }
      slots[day.dayIndex] = {
        name: day.dayName,
        checked: true,
        startTime: formatTime(Math.min(...temp)),
        endTime: formatTime(Math.max(...temp)),
      };
    } else {
      slots[day.dayIndex] = {
        name: day.dayName,
        checked: false,
        startTime: "",
        endTime: "",
      };
    }
  }

  // let startMins = parseMinutes(startTime);
  // let endMins = parseMinutes(endTime);

  // // Handle overnight/over-midnight periods (e.g., 22:00 to 02:00)
  // if (endMins < startMins) {
  //   endMins += 24 * 60;
  // }

  // const SLOT_DURATION = 30; // in minutes

  // // Generate slots in 30-minute increments
  // while (startMins + SLOT_DURATION <= endMins) {
  //   const slotStart = formatTime(startMins);
  //   const slotEnd = formatTime(startMins + SLOT_DURATION);

  //   slots.push({
  //     timeSlotId: null,
  //     day: day.dayIndex,
  //     dayName: day.dayName,
  //     startTime: slotStart,
  //     endTime: slotEnd,
  //     notes: "",
  //   });

  //   startMins += SLOT_DURATION;
  // }

  return slots;
}
export function formatDateWithDayAr(timestamp) {
  const date = new Date(timestamp);

  // Arrays for day and month names
  const days = [
    "الأحد",
    "الإثنين",
    "الثلاثاء",
    "الأربعاء",
    "الخميس",
    "الجمعة",
    "السبت",
  ];
  const months = [
    "كانون الثاني",
    "شباط",
    "آذار",
    "نيسان",
    "ايار",
    "حزيران",
    "تموز",
    "آب",
    "ايلول",
    "تشرين الأول",
    "تشرين الثاني",
    "كانون الأول",
  ];

  // Get date components
  const dayName = days[date.getDay()]; // Get day name (0-6)
  const day = date.getDate(); // Get day of month (1-31)
  const month = months[date.getMonth()]; // Get month name
  const year = date.getFullYear(); // Get full year

  // Format time components
  let hours = date.getHours();
  const ampm = hours >= 12 ? "م" : "ص";
  hours = hours % 12;
  hours = hours ? hours : 12; // Convert 0 to 12

  const minutes = date.getMinutes().toString().padStart(2, "0");
  const seconds = date.getSeconds().toString().padStart(2, "0");

  // Combine all components
  return `${dayName}, ${day} ${month}`;
}
