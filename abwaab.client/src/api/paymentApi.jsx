import { axiosPrivate } from "../services/axios";
export const paymentApi = {
  confirmPayment: (paymentCode) =>
    axiosPrivate.post("/api/Payment/confirm-payment", {
      paymentCode,
    }),
};
