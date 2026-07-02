import api from "../lib/api";

export const usersService = {
  getAll: () => api.get("/users").then((r) => r.data),
  getById: (id) => api.get(`/users/${id}`).then((r) => r.data),
  create: (dto) => api.post("/users", dto).then((r) => r.data),
  update: (id, dto) => api.put(`/users/${id}`, dto).then((r) => r.data),
  changePassword: (id, password) =>
    api.patch(`/users/${id}/password`, { password }).then((r) => r.data),
  remove: (id) => api.delete(`/users/${id}`).then((r) => r.data),
};

export const rolesService = {
  getAll: () => api.get("/roles").then((r) => r.data),
};

export const userRolesService = {
  getByUser: (userId) =>
    api.get(`/userroles/user/${userId}`).then((r) => r.data),
  assign: (userId, roleId) =>
    api.post("/userroles", { userId, roleId }).then((r) => r.data),
  remove: (userId, roleId) =>
    api.delete(`/userroles/user/${userId}/role/${roleId}`).then((r) => r.data),
};
