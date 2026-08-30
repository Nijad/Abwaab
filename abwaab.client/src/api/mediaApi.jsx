import { axiosPrivate } from "../services/axios";
export const mediaApi = {
  upload: (form, signal) =>
    axiosPrivate.post("/api/Media/upload", form, {
      signal: signal,
      headers: {
        "Content-Type": "multipart/form-data",
      },
    }),
  delete: (id, signal) =>
    axiosPrivate.delete(`/api/Media/${id}`, {
      signal: signal,
    }),
};
