import React, { useEffect } from "react";
import { authApi } from "../api";
import useAuth from "../hooks/useAuth";
import { Navigate } from "react-router";

const PresistLogin = ({ children }) => {
  const { login, refreshToken, loading, setLoading, token } = useAuth();

  console.log("loading is:", loading);
  console.log("token is:", token);
  console.log("i'm presis login");
  const storedRefreshToken = sessionStorage.getItem("refreshToken");
  console.log("my refresh:", storedRefreshToken);
  useEffect(() => {
    const getNewToken = async () => {
      console.log("i'm gettin new token");
      try {
        // const response = await authApi.refreshToken(storedRefreshToken);
        // login(response.data);
        await refreshToken();
        setLoading(false);
      } catch (error) {
        return children;
      }
    };
    !token ? getNewToken() : setLoading(false);
  }, []);
  if (!storedRefreshToken) {
    console.log("i have no ref tok");
    return <Navigate to={"/login"} replace />; //you didn't sign in before
  }
  if (loading) {
    console.log("im authenticating");

    return <p>Authenticating....</p>;
  }
  return children;
  //   return <>{loading ? (<p>Authenticating....</p>):<Ou}</>;
};

export default PresistLogin;
