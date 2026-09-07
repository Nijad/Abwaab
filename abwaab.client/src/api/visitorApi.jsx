import { axiosPrivate } from "../services/axios";
export const visitorApi = {
  homePageProperties: (signal) =>
    axiosPrivate.get("/api/Visitor/GetMainPageData", {
      signal: signal,
    }),
  getSearchForm: (signal) =>
    axiosPrivate.get("/api/Visitor/GetSearchForm", {
      signal: signal,
    }),
};
