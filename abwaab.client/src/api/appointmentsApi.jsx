import { axiosPrivate } from "../services/axios";
export const appointmentsApi = {
  userAppointments: (signal) =>
    axiosPrivate.get("/api/Appointment/UserAppointments", {
      signal: signal,
    }),
  confirmAppointments: (appointmentId, signal) =>
    axiosPrivate.put(
      "/api/Appointment/ConfirmAppointment",
      { appointmentId },
      {
        signal: signal,
      }
    ),
  refuseAppointments: (appointmentId, signal) =>
    axiosPrivate.put(
      "/api/Appointment/RefuseAppointment",
      { appointmentId, comment: "" },
      {
        signal: signal,
      }
    ),
};
