import { axiosPrivate } from "../services/axios";

export const rolesApi = {
  getAllRoles: () => axiosPrivate.get("/api/Roles/all-roles"),

  getUserRoles: (userIdentifier) =>
    axiosPrivate.get("/api/Roles/user-roles", {
      params: { userIdentifier },
    }),

  addUserRole: (identifier, roleName) =>
    axiosPrivate.post("/api/Roles/add-user-role", {
      identifier,
      roleName,
    }),

  removeUserRole: (identifier, roleName) =>
    axiosPrivate.post("/api/Roles/remove-user-role", {
      identifier,
      roleName,
    }),
};
