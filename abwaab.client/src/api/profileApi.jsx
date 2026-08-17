import { axiosPrivate } from "../services/axios";

export const profileApi = {
  getProfileData: () => axiosPrivate.get("/api/Profile/ProfileData"),

  getNotificationWays: () => axiosPrivate.get("/api/Profile/NotificationWays"),

  updateUser: (firstName, lastName) =>
    axiosPrivate.post("/api/Profile/UpdateUser", {
      firstName,
      lastName,
    }),

  changePassword: (currentPassword, newPassword, confirmPassword) =>
    axiosPrivate.post("/api/Profile/ChangePassword", {
      currentPassword,
      newPassword,
      confirmPassword,
    }),

  initiateEmailChange: (newEmail, currentPassword) =>
    axiosPrivate.post("/api/Profile/initiate-email-change", {
      newEmail,
      currentPassword,
    }),

  confirmEmailChange: (newEmail, code) =>
    axiosPrivate.post("/api/Profile/confirm-email-change", {
      newEmail,
      code,
    }),

  cancelEmailChange: () =>
    axiosPrivate.post("/api/Profile/cancel-email-change"),

  initiatePhoneChange: (newPhoneNo, currentPassword) =>
    axiosPrivate.post("/api/Profile/initiate-phone-change", {
      newPhoneNo,
      currentPassword,
    }),

  confirmPhoneChange: (newPhoneNo, code) =>
    axiosPrivate.post("/api/Profile/confirm-phone-change", {
      newPhoneNo,
      code,
    }),

  cancelPhoneChange: () =>
    axiosPrivate.post("/api/Profile/cancel-phone-change"),

  subscribeNotificationWay: (notifiactionWayId) =>
    axiosPrivate.post("/api/Profile/SubscribeNotificationWay", {
      notifiactionWayId,
    }),

  unsubscribeNotificationWay: (notifiactionWayId) =>
    axiosPrivate.post("/api/Profile/UnsubscribeNotificationWay", {
      notifiactionWayId,
    }),

  upgradePlan: (planId) =>
    axiosPrivate.post("/api/Profile/UpgradePlan", {
      planId,
    }),
};
