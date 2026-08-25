import { axiosPrivate } from "../services/axios";

export const propertyApi = {
  getPropertyForUpdate: (id, signal) =>
    axiosPrivate.get(`/api/Property/GetPropertyForUpdate?propertyId=${id}`, {
      signal: signal,
    }),
  addProperty: (propertyData) =>
    axiosPrivate.post("/api/Property/add-property", propertyData),
  getMyProperties: (signal) =>
    axiosPrivate.get("/api/property", { signal: signal }),
  starProperty: (id, signal) =>
    axiosPrivate.post("/api/property/star", { id: id }, { signal: signal }),
  getPropertyVisitRequests: (signal) =>
    axiosPrivate.get("/api/Property/get-visits", {
      signal: signal,
    }),
  rejectVisit: (id, signal) =>
    axiosPrivate.post(
      "/api/property/reject-visit",
      { id: id },
      { signal: signal }
    ),
};
