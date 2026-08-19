import { axiosPrivate } from "../services/axios";

export const authApi = {
  register: (
    firstName,
    lastName,
    identifier,
    password,
    confirmPassword,
    signal
  ) =>
    axiosPrivate.post(
      "/api/Auth/RegisterUser",
      {
        firstName,
        lastName,
        identifier,
        password,
        confirmPassword,
      },
      { signal: signal }
    ),
  verifyAccount: (identifier, code, signal) =>
    axiosPrivate.post(
      "/api/Auth/VerifyAccount",
      { identifier, code },
      { signal: signal }
    ),
  login: (identifier, password, signal) =>
    axiosPrivate.post(
      "/api/Auth/LoginUser",
      { identifier, password },
      { signal: signal }
    ),
  resendCode: (identifier, signal) =>
    axiosPrivate.post(
      "/api/Auth/ResendCode",
      { identifier },
      { signal: signal }
    ),
  refreshToken: (refreshToken, signal) =>
    axiosPrivate.post(
      "/api/Auth/refresh-token",
      { refreshToken },
      { signal: signal }
    ),
  logout: (revokeAll, signal) =>
    axiosPrivate.post("/api/Auth/Logout", { revokeAll }, { signal: signal }),
  forgotPassword: (identifier, signal) =>
    axiosPrivate.post(
      "/api/Auth/ForgotPassword",
      { identifier },
      { signal: signal }
    ),
  verifyResetCode: (identifier, code, signal) =>
    axiosPrivate.post(
      "/api/Auth/verify-reset-code",
      { identifier, code },
      { signal: signal }
    ),
  resetPassword: (identifier, code, newPassword, confirmNewPassword, signal) =>
    axiosPrivate.post(
      "/api/Auth/reset-password",
      {
        identifier,
        code,
        newPassword,
        confirmNewPassword,
      },
      { signal: signal }
    ),
};
