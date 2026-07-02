import api from "../lib/api";

export const incidentsService = {
  getAll: () => api.get("/incidents").then((r) => r.data),
  getById: (id) => api.get(`/incidents/${id}`).then((r) => r.data),
  create: (dto) => api.post("/incidents", { dto }).then((r) => r.data),
  update: (incidentId, dto) =>
    api.put("/incidents", { dto: { incidentId, ...dto } }).then((r) => r.data),
  changeStatus: (incidentId, status) =>
    api
      .patch("/incidents/status", { dto: { incidentId, status } })
      .then((r) => r.data),
  remove: (id) => api.delete(`/incidents/${id}`).then((r) => r.data),
};

// Must match ChangeIncidentStatusCommandHandler's validStatuses exactly.
export const INCIDENT_STATUSES = ["Reportado", "EnAtencion", "Cerrado"];

export const INCIDENT_STATUS_LABELS = {
  Reportado: "Reportado",
  EnAtencion: "En Atención",
  Cerrado: "Cerrado",
};
