import axios from "../services/axios";
import useAuth from "./useAuth";

const useRefreshToken = () => {
  const { login } = useAuth();

  const refresh = async () => {
    try {
      const response = await axios.get("/api/token/refresh", {
        withCredentials: true,
      });
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
