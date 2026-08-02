import axios from "../services/axios";
import useAuth from "./useAuth";

const useRefreshToken = () => {
  const { auth, setAuth } = useAuth();
  const tokenDto = {
    accessToken: auth.accessToken,
    refreshToken: auth.refreshToken,
    roles: auth.roles,
  };

  const refresh = async () => {
    // const response = await axios("/api/token/refresh", {
    //   headers: {
    //     "Content-Type": "Application/json",
    //     "Access-Control-Allow-Origin": "*",

    //   },
    //   method: "POST",
    //   mode: "cors",
    //   withCredentials: true,
    //   data: tokenDto,
    // });
    const response = await axios.get("/api/token/refresh", {
      withCredentials: true,
    });
    setAuth((prev) => {
      return {
        ...prev,
        fullName: response.data.fullName,
        userName: response.data.userName,
        accessToken: response.data.accessToken,
        refreshToken: response.data.refreshToken,
        roles: response.data.roles,
      };
    });
    return response.data.accessToken;
  };

  // const refresh = async () => {
  //   const response = await fetch("http://localhost:5000/api/token/refresh", {
  //     method: "POST",
  //     mode: "cors",
  //     headers: {
  //       "Content-Type": "application/json",
  //     },
  //     body: JSON.stringify(tokenDto),
  //   });
  //   setAuth((prev) => {
  //     console.log(JSON.stringify(prev));
  //     console.log(response.accessToken);
  //     return {
  //       ...prev,
  //       accessToken: response.accessToken,
  //       refreshToken: response.refreshToken,
  //       roles: response.roles,
  //     };
  //   });
  //   return response.accessToken;
  // };
  return refresh;
};

export default useRefreshToken;
