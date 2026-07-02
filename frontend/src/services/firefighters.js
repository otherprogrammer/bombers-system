import api from "../lib/api";

export const firefightersService = {
  getAll: () => api.get("/firefighters").then((r) => r.data),
  getById: (id) => api.get(`/firefighters/${id}`).then((r) => r.data),
  getByStation: (stationId) =>
    api.get(`/firefighters/station/${stationId}`).then((r) => r.data),
  create: (dto) => api.post("/firefighters", dto).then((r) => r.data),
  update: (id, dto) => api.put(`/firefighters/${id}`, dto).then((r) => r.data),
  remove: (id) => api.delete(`/firefighters/${id}`).then((r) => r.data),
};
