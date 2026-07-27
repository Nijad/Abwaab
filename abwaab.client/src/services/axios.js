import axios from "axios";
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
