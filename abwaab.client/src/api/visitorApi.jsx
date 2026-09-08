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
  getMostViewed: (pageNo, signal) =>
    axiosPrivate.get(`/api/Visitor/GetMostViewed?pageNo=${pageNo}`, {
      signal: signal,
    }),
  getPremium: (pageNo, signal) =>
    axiosPrivate.get(`/api/Visitor/GetPremium?pageNo=${pageNo}`, {
      signal: signal,
    }),
  getRecentlyAdded: (pageNo, signal) =>
    axiosPrivate.get(`/api/Visitor/GetRecentlyAdded?pageNo=${pageNo}`, {
      signal: signal,
    }),
  search: (data, signal) =>
    axiosPrivate.post("/api/Visitor/Search", data, {
      signal: signal,
    }),
};
