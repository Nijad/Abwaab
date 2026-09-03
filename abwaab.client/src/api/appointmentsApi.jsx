import { axiosPrivate } from "../services/axios";
export const appointmentsApi = {
  userAppointments: (signal) =>
    axiosPrivate.get("/api/Appointment/UserAppointments", {
      signal: signal,
    }),
};
