import api from "../lib/api";

export const stationsService = {
  getAll: () => api.get("/stations").then((r) => r.data),
  getById: (id) => api.get(`/stations/${id}`).then((r) => r.data),
  getVehicles: (id) => api.get(`/stations/${id}/vehicles`).then((r) => r.data),
  create: (dto) => api.post("/stations", dto).then((r) => r.data),
  update: (id, dto) => api.put(`/stations/${id}`, dto).then((r) => r.data),
  remove: (id) => api.delete(`/stations/${id}`).then((r) => r.data),
};
