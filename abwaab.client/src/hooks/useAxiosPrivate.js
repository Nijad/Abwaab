import { axiosPrivate } from "../services/axios";
import { useEffect } from "react";
import useRefreshToken from "./useRefreshToken";
import useAuth from "./useAuth";
// import useAuth from "./useAuth";

const useAxiosPrivate = () => {
  const refresh = useRefreshToken();
  const { auth } = useAuth();
  // const { auth } = null;

  useEffect(() => {
    const requestIntercept = axiosPrivate.interceptors.request.use(
      (config) => {
        // if (!config.headers["Authorization"]) {
        //   config.headers["Authorization"] = `Bearer ${auth?.accessToken}`;
        // }
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
          const newAccessTokent = await refresh();
          prevRequest.headers["Authorization"] = `Bearer ${newAccessTokent}`;
          return axiosPrivate(prevRequest);
        }
        return Promise.reject(error);
      }
    );
    return () => {
      axiosPrivate.interceptors.response.eject(responsIntercept);
      axiosPrivate.interceptors.response.eject(requestIntercept);
    };
  }, [auth, refresh]);
  return axiosPrivate;
};
export default useAxiosPrivate;
