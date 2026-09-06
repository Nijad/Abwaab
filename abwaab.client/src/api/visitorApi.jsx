import { axiosPrivate } from "../services/axios";
export const visitorApi = {
  homePageProperties: (signal) =>
    axiosPrivate.get("/api/Visitor/GetMainPageData", {
      signal: signal,
      headers: {
        "Content-Type": "multipart/form-data",
      },
    }),
};
