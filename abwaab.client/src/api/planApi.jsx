import { axiosPrivate } from "../services/axios";
export const planApi = {
  getAllPlans: () => axiosPrivate.get("/api/Plan/All-plans"),

  createPlan: (
    name,
    price,
    durationInDays,
    startDate,
    expieryDate,
    tempDurationInDays,
    maxPropertiesCountAtSameTime,
    maxStardPropertiesCountAtSameTime,
    maxImagesCount,
    maxVideosCount
  ) =>
    axiosPrivate.post("/api/Plan/create-plan", {
      name,
      price,
      durationInDays,
      startDate,
      expieryDate,
      tempDurationInDays,
      maxPropertiesCountAtSameTime,
      maxStardPropertiesCountAtSameTime,
      maxImagesCount,
      maxVideosCount,
    }),

  cancelUserPlan: (userPlanId) =>
    axiosPrivate.post("/api/Plan/cancel-user-plan", {
      userPlanId,
    }),
};
