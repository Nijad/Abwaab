import axios from "axios";
import useRefreshToken from "../hooks/useRefreshToken";

// const baseURL = "http://localhost:5000";
const baseURL = import.meta.env.VITE_API_BASE_URL;
export default axios.create({
  withCredentials: true,
  baseURL: baseURL,
});
export const axiosPrivate = axios.create({
  baseURL: baseURL,
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
    "Access-Control-Allow-Origin": "*",
  },
  mode: "cors",
});

//configure axios private for refresh token and handle response
axiosPrivate.interceptors.request.use(
  (config) => {
    // if (!config.headers["Authorization"]) {
    //   config.headers["Authorization"] = `Bearer ${auth?.accessToken}`;
    // }
    return config;
  },
  (error) => Promise.reject(error)
);
axiosPrivate.interceptors.response.use(
  (response) => response,
  async (error) => {
    const prevRequest = error?.config;
    if (
      (error?.response?.status === 403 || error?.response.status === 401) &&
      !prevRequest?.sent
    ) {
      prevRequest.sent = true;
      const newAccessTokent = await useRefreshToken();
      prevRequest.headers["Authorization"] = `Bearer ${newAccessTokent}`;
      return axiosPrivate(prevRequest);
    }
    const serverMessage =
      error.response?.data || // Standard custom backend error message
      // error.response?.data?.title || // ASP.NET Validation problem details
      error.message || // Network / Axios error message
      "An unexpected error occurred";
    return Promise.reject(error);
  }
);
