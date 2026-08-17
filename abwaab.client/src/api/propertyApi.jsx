import { axiosPrivate } from "../services/axios";

export const propertyApi = {
  addProperty: (propertyData) =>
    axiosPrivate.post("/api/Property/add-property", propertyData),
};
