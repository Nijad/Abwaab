import { axiosPrivate } from "../services/axios";

export const authApi = {
  register: (firstName, lastName, identifier, password, confirmPassword) =>
    axiosPrivate.post("/api/Auth/RegisterUser", {
      firstName,
      lastName,
      identifier,
      password,
      confirmPassword,
    }),
  verifyAccount: (identifier, code) =>
    axiosPrivate.post("/api/Auth/VerifyAccount", { identifier, code }),
  login: (identifier, password) =>
    axiosPrivate.post("/api/Auth/LoginUser", { identifier, password }),
  resendCode: (identifier) =>
    axiosPrivate.post("/api/Auth/ResendCode", { identifier }),
  refreshToken: (refreshToken) =>
    axiosPrivate.post("/api/Auth/refresh-token", { refreshToken }),
  logout: (revokeAll) => axiosPrivate.post("/api/Auth/Logout", { revokeAll }),
  forgotPassword: (identifier) =>
    axiosPrivate.post("/api/Auth/ForgotPassword", { identifier }),
  verifyResetCode: (identifier, code) =>
    axiosPrivate.post("/api/Auth/verify-reset-code", { identifier, code }),
  resetPassword: (identifier, code, newPassword, confirmNewPassword) =>
    axiosPrivate.post("/api/Auth/reset-password", {
      identifier,
      code,
      newPassword,
      confirmNewPassword,
    }),
};
