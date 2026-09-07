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
  search: (data, signal) =>
    axiosPrivate.post("/api/Visitor/Search", data, {
      signal: signal,
    }),
};
