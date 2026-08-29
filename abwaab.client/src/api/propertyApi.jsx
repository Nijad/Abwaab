import { axiosPrivate } from "../services/axios";

export const propertyApi = {
  getPropertyForUpdate: (id, signal) =>
    axiosPrivate.get(`/api/Property/GetPropertyForUpdate?propertyId=${id}`, {
      signal: signal,
    }),
  updateProperty: (properyData, signal) =>
    axiosPrivate.put(`/api/Property/update-property`, properyData, {
      signal: signal,
    }),
  addProperty: (signal) =>
    axiosPrivate.post("/api/Property/add-property", null, { signal: signal }),
  getMyProperties: (signal) =>
    axiosPrivate.get("/api/property", { signal: signal }),
  propertyDetails: (id, signal) =>
    axiosPrivate.get(`/api/Property/propertydetails?propertyId=${id}`, {
      signal: signal,
    }),
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
