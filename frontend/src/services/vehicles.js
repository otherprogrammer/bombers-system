import api from "../lib/api";

export const vehiclesService = {
  getAll: (params) => api.get("/vehicles", { params }).then((r) => r.data),
  getById: (id) => api.get(`/vehicles/${id}`).then((r) => r.data),
  getByStation: (stationId) =>
    api.get(`/vehicles/station/${stationId}`).then((r) => r.data),
  create: (dto) => api.post("/vehicles", dto).then((r) => r.data),
  update: (id, dto) => api.put(`/vehicles/${id}`, dto).then((r) => r.data),
  remove: (id) => api.delete(`/vehicles/${id}`).then((r) => r.data),
};
