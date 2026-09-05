import { axiosPrivate } from "../services/axios";
export const notificationApi = {
  userNotifcations: (signal) =>
    axiosPrivate.get("/api/Appointment/UserAppointments", {
      signal: signal,
    }),
  deleteNotifcation: (id, signal) =>
    axiosPrivate.delete(`/api/notification/deletenotification?id=${id}`, {
      signal: signal,
    }),
};
