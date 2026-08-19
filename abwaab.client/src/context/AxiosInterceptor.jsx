import React, { useEffect } from "react";
import useAuth from "../hooks/useAuth";
import { axiosPrivate } from "../services/axios";

const AxiosInterceptor = ({ children }) => {
  const { token, refreshToken, loading, logout } = useAuth();
  console.log("loadin is:", loading);
  const storedToken = sessionStorage.getItem("token");

  useEffect(() => {
    const requestIntercept = axiosPrivate.interceptors.request.use(
      (config) => {
        if (!config.headers["Authorization"] && storedToken) {
          config.headers["Authorization"] = `Bearer ${storedToken}`;
        }
        return config;
      },
      (error) => Promise.reject(error)
    );
    const responsIntercept = axiosPrivate.interceptors.response.use(
      (response) => response,
      async (error) => {
        const prevRequest = error?.config;
        if (
          (error?.response?.status === 403 || error?.response.status === 401) &&
          !prevRequest?.sent
        ) {
          prevRequest.sent = true;
          const newToken = await refreshToken();
          console.log("from axios- after refresh-token is:", newToken);

          prevRequest.headers["Authorization"] = `Bearer ${newToken}`;
          return axiosPrivate(prevRequest);
        }
        const serverMessage =
          error.response?.data || // Standard custom backend error message
          // error.response?.data?.title || // ASP.NET Validation problem details
          error.message || // Network / Axios error message
          "An unexpected error occurred";
        return Promise.reject(serverMessage);
      }
    );
    return () => {
      axiosPrivate.interceptors.response.eject(responsIntercept);
      axiosPrivate.interceptors.response.eject(requestIntercept);
    };
  }, [token, refreshToken]);
  return children;
};

export default AxiosInterceptor;
