import { authApi } from "../api";
import axios from "../services/axios";
import useAuth from "./useAuth";

const useRefreshToken = () => {
  const { login, token } = useAuth();

  const refresh = async () => {
    try {
      const response = await authApi.refreshToken(token.refreshToken);
      login(response.data);
      return response.data.accessToken;
    } catch (error) {
      console.log(error);
      return "";
    }
  };

  return refresh;
};

export default useRefreshToken;
