import { axiosPrivate } from "../services/axios";
export const appointmentsApi = {
  userAppointments: (signal) =>
    axiosPrivate.post("/api/Appointment/UserAppointments", {
      signal: signal,
    }),
};
