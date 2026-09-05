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
  submitProperty: (properyData, signal) =>
    axiosPrivate.put(`/api/Property/submit-property`, properyData, {
      signal: signal,
    }),
  addProperty: (signal) =>
    axiosPrivate.post("/api/Property/add-property", null, { signal: signal }),
  userProperties: (signal) =>
    axiosPrivate.get("/api/Property/UserProperties", { signal: signal }),
  propertyDetails: (id, signal) =>
    axiosPrivate.get(`/api/Property/propertydetails?propertyId=${id}`, {
      signal: signal,
    }),
  starProperty: (id, signal) =>
    axiosPrivate.post("/api/property/star", { id: id }, { signal: signal }),
  acceptProperty: (propertyId, note, signal) =>
    axiosPrivate.post(
      "/api/Property/accept-property",
      { propertyId, note },
      { signal: signal }
    ),
  rejectProperty: (propertyId, note, signal) =>
    axiosPrivate.post(
      "/api/Property/reject-property",
      { propertyId, note },
      { signal: signal }
    ),
  getPropertyVisitRequests: (signal) =>
    axiosPrivate.get("/api/Property/get-visits", {
      signal: signal,
    }),
  propertyTimeSlots: (id, signal) =>
    axiosPrivate.get(`/api/Property/PropertyTimeSlots?propertyId=${id}`, {
      signal: signal,
    }),
  rejectVisit: (id, signal) =>
    axiosPrivate.post(
      "/api/property/reject-visit",
      { id: id },
      { signal: signal }
    ),
  bookAppointment: (dateInfo, signal) =>
    axiosPrivate.post("/api/Appointment/BookAppointment", dateInfo, {
      signal: signal,
    }),
};
