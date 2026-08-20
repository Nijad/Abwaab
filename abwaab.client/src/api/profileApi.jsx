import { axiosPrivate } from "../services/axios";

export const profileApi = {
  getProfileData: (signal) =>
    axiosPrivate.get("/api/Profile/ProfileData", { signal: signal }),

  getNotificationWays: (signal) =>
    axiosPrivate.get("/api/Profile/NotificationWays", { signal: signal }),

  updateUser: (firstName, lastName, signal) =>
    axiosPrivate.post(
      "/api/Profile/UpdateUser",
      {
        firstName,
        lastName,
      },
      { signal: signal }
    ),

  changePassword: (currentPassword, newPassword, confirmPassword, signal) =>
    axiosPrivate.post(
      "/api/Profile/ChangePassword",
      {
        currentPassword,
        newPassword,
        confirmPassword,
      },
      { signal: signal }
    ),

  initiateEmailChange: (newEmail, currentPassword, signal) =>
    axiosPrivate.post(
      "/api/Profile/initiate-email-change",
      {
        newEmail,
        currentPassword,
      },
      { signal: signal }
    ),

  confirmEmailChange: (newEmail, code, signal) =>
    axiosPrivate.post(
      "/api/Profile/confirm-email-change",
      {
        newEmail,
        code,
      },
      { signal: signal }
    ),

  cancelEmailChange: (signal) =>
    axiosPrivate.post("/api/Profile/cancel-email-change", { signal: signal }),

  initiatePhoneChange: (newPhoneNo, currentPassword, signal) =>
    axiosPrivate.post(
      "/api/Profile/initiate-phone-change",
      {
        newPhoneNo,
        currentPassword,
      },
      { signal: signal }
    ),

  confirmPhoneChange: (newPhoneNo, code, signal) =>
    axiosPrivate.post(
      "/api/Profile/confirm-phone-change",
      {
        newPhoneNo,
        code,
      },
      { signal: signal }
    ),

  cancelPhoneChange: (signal) =>
    axiosPrivate.post("/api/Profile/cancel-phone-change", { signal: signal }),

  subscribeNotificationWay: (notifiactionWayId, signal) =>
    axiosPrivate.post(
      "/api/Profile/SubscribeNotificationWay",
      {
        notifiactionWayId,
      },
      { signal: signal }
    ),

  unsubscribeNotificationWay: (notifiactionWayId, signal) =>
    axiosPrivate.post(
      "/api/Profile/UnsubscribeNotificationWay",
      {
        notifiactionWayId,
      },
      { signal: signal }
    ),

  upgradePlan: (planId, signal) =>
    axiosPrivate.post(
      "/api/Profile/UpgradePlan",
      {
        planId,
      },
      { signal: signal }
    ),
};
