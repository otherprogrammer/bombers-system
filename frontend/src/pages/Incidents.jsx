import { useEffect, useState } from "react";
import { Plus, Flame, HeartPulse, TriangleAlert, CircleAlert } from "lucide-react";
import PageHeader from "../components/PageHeader";
import Modal from "../components/Modal";
import Badge from "../components/Badge";
import {
  incidentsService,
  INCIDENT_STATUSES,
  INCIDENT_STATUS_LABELS,
} from "../services/incidents";

const EMERGENCY_TYPES = ["Incendio", "Medico", "Rescate", "Materiales Peligrosos"];

const statusColor = {
  Reportado: "yellow",
  EnAtencion: "blue",
  Cerrado: "green",
};

const typeIcon = {
  Incendio: Flame,
  Medico: HeartPulse,
  Rescate: TriangleAlert,
  "Materiales Peligrosos": CircleAlert,
};

const emptyForm = {
  emergencyType: EMERGENCY_TYPES[0],
  priorityLevel: 3,
  latitude: "",
  longitude: "",
};

export default function Incidents() {
  const [incidents, setIncidents] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [modalOpen, setModalOpen] = useState(false);
  const [form, setForm] = useState(emptyForm);
  const [saving, setSaving] = useState(false);
  const [statusFilter, setStatusFilter] = useState("");

  async function load() {
    setLoading(true);
    setError("");
    try {
      const data = await incidentsService.getAll();
      setIncidents(data);
    } catch (err) {
      setError(
        err.response?.data?.message || "No se pudieron cargar los incidentes."
      );
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    load();
  }, []);

  async function handleSubmit(e) {
    e.preventDefault();
    setSaving(true);
    setError("");
    try {
      await incidentsService.create({
        emergencyType: form.emergencyType,
        priorityLevel: Number(form.priorityLevel),
        latitude: Number(form.latitude),
        longitude: Number(form.longitude),
      });
      setModalOpen(false);
      setForm(emptyForm);
      await load();
    } catch (err) {
      setError(
        err.response?.data?.message || "No se pudo crear el incidente."
      );
    } finally {
      setSaving(false);
    }
  }

  async function handleStatusChange(incident, status) {
    setError("");
    try {
      await incidentsService.changeStatus(incident.incidentId, status);
      await load();
    } catch (err) {
      setError(
        err.response?.data?.message || "No se pudo cambiar el estado."
      );
    }
  }

  const filtered = statusFilter
    ? incidents.filter((i) => i.status === statusFilter)
    : incidents;

  return (
    <div className="p-8">
      <PageHeader
        title="Gestión de Incidentes"
        subtitle="Registro y seguimiento de emergencias en tiempo real."
        action={
          <button
            onClick={() => setModalOpen(true)}
            className="bg-primary text-on-primary px-5 py-2.5 rounded-lg font-bold flex items-center gap-2 hover:brightness-110 active:scale-95 transition-all shadow-sm"
          >
            <Plus size={18} />
            Crear Incidente
          </button>
        }
      />

      {error && (
        <div className="mb-4 bg-error-container text-on-error-container text-sm font-medium px-4 py-3 rounded border-l-4 border-error">
          {error}
        </div>
      )}

      <div className="flex items-center gap-2 bg-surface-container-low border border-outline-variant rounded-xl p-4 mb-4">
        <label className="text-xs font-bold text-secondary uppercase">
          Estado:
        </label>
        <select
          value={statusFilter}
          onChange={(e) => setStatusFilter(e.target.value)}
          className="bg-white border border-outline-variant rounded px-3 py-1.5 text-sm"
        >
          <option value="">Todos</option>
          {INCIDENT_STATUSES.map((s) => (
            <option key={s} value={s}>
              {INCIDENT_STATUS_LABELS[s]}
            </option>
          ))}
        </select>
        <span className="ml-auto text-xs text-secondary italic">
          {filtered.length} incidente(s)
        </span>
      </div>

      <div className="bg-surface-container-lowest border border-outline-variant rounded-lg overflow-hidden shadow-sm">
        <table className="w-full text-left border-collapse">
          <thead>
            <tr className="bg-surface-container text-on-surface-variant text-xs uppercase tracking-widest border-b border-outline-variant">
              <th className="px-4 py-3 font-black">Código</th>
              <th className="px-4 py-3 font-black">Tipo</th>
              <th className="px-4 py-3 font-black text-center">Prioridad</th>
              <th className="px-4 py-3 font-black">Estado</th>
              <th className="px-4 py-3 font-black text-right">Cambiar estado</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-outline-variant">
            {loading ? (
              <tr>
                <td colSpan={5} className="px-4 py-6 text-center text-secondary">
                  Cargando...
                </td>
              </tr>
            ) : filtered.length === 0 ? (
              <tr>
                <td colSpan={5} className="px-4 py-6 text-center text-secondary">
                  No hay incidentes registrados.
                </td>
              </tr>
            ) : (
              filtered.map((inc) => {
                const Icon = typeIcon[inc.emergencyType] || Flame;
                return (
                  <tr key={inc.incidentId} className="hover:bg-surface-container-low transition-colors">
                    <td className="px-4 py-3 font-mono font-bold text-primary">
                      {inc.incidentCode}
                    </td>
                    <td className="px-4 py-3">
                      <div className="flex items-center gap-2">
                        <Icon size={18} className="text-primary" />
                        {inc.emergencyType}
                      </div>
                    </td>
                    <td className="px-4 py-3 text-center font-bold">
                      {inc.priorityLevel}
                    </td>
                    <td className="px-4 py-3">
                      <Badge color={statusColor[inc.status] || "gray"}>
                        {INCIDENT_STATUS_LABELS[inc.status] || inc.status}
                      </Badge>
                    </td>
                    <td className="px-4 py-3 text-right">
                      <select
                        value={inc.status}
                        onChange={(e) =>
                          handleStatusChange(inc, e.target.value)
                        }
                        className="border border-outline-variant rounded px-2 py-1 text-xs font-bold"
                      >
                        {INCIDENT_STATUSES.map((s) => (
                          <option key={s} value={s}>
                            {INCIDENT_STATUS_LABELS[s]}
                          </option>
                        ))}
                      </select>
                    </td>
                  </tr>
                );
              })
            )}
          </tbody>
        </table>
      </div>

      {modalOpen && (
        <Modal title="Crear Incidente" onClose={() => setModalOpen(false)}>
          <form onSubmit={handleSubmit} className="flex flex-col gap-4">
            <div>
              <label className="block text-xs font-bold text-on-surface-variant uppercase mb-1">
                Tipo de Emergencia
              </label>
              <select
                value={form.emergencyType}
                onChange={(e) =>
                  setForm({ ...form, emergencyType: e.target.value })
                }
                className="w-full border border-outline-variant rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary outline-none"
              >
                {EMERGENCY_TYPES.map((t) => (
                  <option key={t} value={t}>
                    {t}
                  </option>
                ))}
              </select>
            </div>
            <div>
              <label className="block text-xs font-bold text-on-surface-variant uppercase mb-1">
                Prioridad (1 = más alta)
              </label>
              <input
                type="number"
                min="1"
                max="5"
                required
                value={form.priorityLevel}
                onChange={(e) =>
                  setForm({ ...form, priorityLevel: e.target.value })
                }
                className="w-full border border-outline-variant rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary outline-none"
              />
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="block text-xs font-bold text-on-surface-variant uppercase mb-1">
                  Latitud
                </label>
                <input
                  type="number"
                  step="any"
                  required
                  value={form.latitude}
                  onChange={(e) =>
                    setForm({ ...form, latitude: e.target.value })
                  }
                  className="w-full border border-outline-variant rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary outline-none"
                />
              </div>
              <div>
                <label className="block text-xs font-bold text-on-surface-variant uppercase mb-1">
                  Longitud
                </label>
                <input
                  type="number"
                  step="any"
                  required
                  value={form.longitude}
                  onChange={(e) =>
                    setForm({ ...form, longitude: e.target.value })
                  }
                  className="w-full border border-outline-variant rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary outline-none"
                />
              </div>
            </div>
            <button
              type="submit"
              disabled={saving}
              className="mt-2 bg-primary text-on-primary py-2.5 rounded-lg font-bold hover:brightness-110 transition-all disabled:opacity-60"
            >
              {saving ? "Creando..." : "Crear Incidente"}
            </button>
          </form>
        </Modal>
      )}
    </div>
  );
}
