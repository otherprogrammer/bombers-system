import { useEffect, useMemo, useState } from "react";
import { Plus, Pencil, Trash2, Truck } from "lucide-react";
import PageHeader from "../components/PageHeader";
import Modal from "../components/Modal";
import Badge from "../components/Badge";
import { vehiclesService } from "../services/vehicles";
import { stationsService } from "../services/stations";

const VEHICLE_TYPES = ["Bomba", "Escalera", "Rescate"];
const STATUS_OPTIONS = ["Disponible", "En Servicio", "Mantenimiento"];

const statusColor = {
  Disponible: "green",
  "En Servicio": "yellow",
  Mantenimiento: "red",
};

const emptyForm = {
  stationId: "",
  waterLevelGallons: "",
  lastMaintenanceDate: "",
  radioCode: "",
  vehicleType: VEHICLE_TYPES[0],
  operationalStatus: STATUS_OPTIONS[0],
};

export default function Vehicles() {
  const [vehicles, setVehicles] = useState([]);
  const [stations, setStations] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [typeFilter, setTypeFilter] = useState("");
  const [statusFilter, setStatusFilter] = useState("");
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState(null);
  const [form, setForm] = useState(emptyForm);
  const [saving, setSaving] = useState(false);

  async function loadAll() {
    setLoading(true);
    setError("");
    try {
      const [v, s] = await Promise.all([
        vehiclesService.getAll({
          type: typeFilter || undefined,
          status: statusFilter || undefined,
        }),
        stationsService.getAll(),
      ]);
      setVehicles(v);
      setStations(s);
    } catch (err) {
      setError(
        err.response?.data?.message || "No se pudieron cargar los vehículos."
      );
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadAll();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [typeFilter, statusFilter]);

  const stationName = useMemo(() => {
    const map = {};
    stations.forEach((s) => (map[s.stationId] = `Estación ${s.stationNumber}`));
    return map;
  }, [stations]);

  function openCreate() {
    setEditing(null);
    setForm({ ...emptyForm, stationId: stations[0]?.stationId ?? "" });
    setModalOpen(true);
  }

  function openEdit(v) {
    setEditing(v);
    setForm({
      stationId: v.stationId,
      waterLevelGallons: v.waterLevelGallons,
      lastMaintenanceDate: v.lastMaintenanceDate,
      radioCode: v.radioCode,
      vehicleType: v.vehicleType,
      operationalStatus: v.operationalStatus,
    });
    setModalOpen(true);
  }

  async function handleSubmit(e) {
    e.preventDefault();
    setSaving(true);
    setError("");
    const dto = {
      stationId: Number(form.stationId),
      waterLevelGallons: Number(form.waterLevelGallons),
      lastMaintenanceDate: form.lastMaintenanceDate,
      radioCode: form.radioCode,
      vehicleType: form.vehicleType,
      operationalStatus: form.operationalStatus,
    };
    try {
      if (editing) {
        await vehiclesService.update(editing.vehicleId, dto);
      } else {
        await vehiclesService.create(dto);
      }
      setModalOpen(false);
      await loadAll();
    } catch (err) {
      setError(
        err.response?.data?.message || "No se pudo guardar el vehículo."
      );
    } finally {
      setSaving(false);
    }
  }

  async function handleDelete(v) {
    if (!confirm(`¿Eliminar el vehículo ${v.radioCode}?`)) return;
    try {
      await vehiclesService.remove(v.vehicleId);
      await loadAll();
    } catch (err) {
      setError(
        err.response?.data?.message || "No se pudo eliminar el vehículo."
      );
    }
  }

  return (
    <div className="p-8">
      <PageHeader
        title="Fleet Management"
        subtitle="Estado operativo de vehículos y asignación a estaciones."
        action={
          <button
            onClick={openCreate}
            disabled={stations.length === 0}
            className="bg-primary text-on-primary px-5 py-2.5 rounded-lg font-bold flex items-center gap-2 hover:brightness-110 active:scale-95 transition-all shadow-sm disabled:opacity-50"
          >
            <Plus size={18} />
            Nuevo Vehículo
          </button>
        }
      />

      {error && (
        <div className="mb-4 bg-error-container text-on-error-container text-sm font-medium px-4 py-3 rounded border-l-4 border-error">
          {error}
        </div>
      )}

      <div className="flex flex-wrap items-center gap-4 bg-surface-container-low border border-outline-variant rounded-xl p-4 mb-4">
        <div className="flex items-center gap-2">
          <label className="text-xs font-bold text-secondary uppercase">
            Tipo:
          </label>
          <select
            value={typeFilter}
            onChange={(e) => setTypeFilter(e.target.value)}
            className="bg-white border border-outline-variant rounded px-3 py-1.5 text-sm"
          >
            <option value="">Todos</option>
            {VEHICLE_TYPES.map((t) => (
              <option key={t} value={t}>
                {t}
              </option>
            ))}
          </select>
        </div>
        <div className="flex items-center gap-2">
          <label className="text-xs font-bold text-secondary uppercase">
            Estado:
          </label>
          <select
            value={statusFilter}
            onChange={(e) => setStatusFilter(e.target.value)}
            className="bg-white border border-outline-variant rounded px-3 py-1.5 text-sm"
          >
            <option value="">Todos</option>
            {STATUS_OPTIONS.map((s) => (
              <option key={s} value={s}>
                {s}
              </option>
            ))}
          </select>
        </div>
        <span className="ml-auto text-xs text-secondary italic">
          Mostrando {vehicles.length} vehículo(s)
        </span>
      </div>

      <div className="bg-surface-container-lowest border border-outline-variant rounded-lg overflow-hidden shadow-sm">
        <table className="w-full text-left border-collapse">
          <thead>
            <tr className="bg-surface-container text-on-surface-variant text-xs uppercase tracking-widest border-b border-outline-variant">
              <th className="px-4 py-3 font-black">Radio</th>
              <th className="px-4 py-3 font-black">Tipo</th>
              <th className="px-4 py-3 font-black">Estación</th>
              <th className="px-4 py-3 font-black">Estado</th>
              <th className="px-4 py-3 font-black">Nivel de Agua</th>
              <th className="px-4 py-3 font-black text-right">Acciones</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-outline-variant">
            {loading ? (
              <tr>
                <td colSpan={6} className="px-4 py-6 text-center text-secondary">
                  Cargando...
                </td>
              </tr>
            ) : vehicles.length === 0 ? (
              <tr>
                <td colSpan={6} className="px-4 py-6 text-center text-secondary">
                  No hay vehículos que coincidan con el filtro.
                </td>
              </tr>
            ) : (
              vehicles.map((v) => (
                <tr key={v.vehicleId} className="hover:bg-surface-container-low transition-colors">
                  <td className="px-4 py-3 font-mono font-bold flex items-center gap-2">
                    <Truck size={16} className="text-primary" />
                    {v.radioCode}
                  </td>
                  <td className="px-4 py-3">{v.vehicleType}</td>
                  <td className="px-4 py-3">
                    {stationName[v.stationId] ?? `#${v.stationId}`}
                  </td>
                  <td className="px-4 py-3">
                    <Badge color={statusColor[v.operationalStatus] || "gray"}>
                      {v.operationalStatus}
                    </Badge>
                  </td>
                  <td className="px-4 py-3 font-mono">
                    {v.waterLevelGallons} gal
                  </td>
                  <td className="px-4 py-3 text-right">
                    <div className="flex justify-end gap-2">
                      <button
                        onClick={() => openEdit(v)}
                        className="p-1.5 text-secondary hover:text-primary transition-colors"
                        title="Editar"
                      >
                        <Pencil size={16} />
                      </button>
                      <button
                        onClick={() => handleDelete(v)}
                        className="p-1.5 text-secondary hover:text-error transition-colors"
                        title="Eliminar"
                      >
                        <Trash2 size={16} />
                      </button>
                    </div>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {modalOpen && (
        <Modal
          title={editing ? "Editar Vehículo" : "Nuevo Vehículo"}
          onClose={() => setModalOpen(false)}
        >
          <form onSubmit={handleSubmit} className="flex flex-col gap-4">
            <div>
              <label className="block text-xs font-bold text-on-surface-variant uppercase mb-1">
                Estación
              </label>
              <select
                required
                value={form.stationId}
                onChange={(e) => setForm({ ...form, stationId: e.target.value })}
                className="w-full border border-outline-variant rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary outline-none"
              >
                {stations.map((s) => (
                  <option key={s.stationId} value={s.stationId}>
                    Estación {s.stationNumber} — {s.address}
                  </option>
                ))}
              </select>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="block text-xs font-bold text-on-surface-variant uppercase mb-1">
                  Tipo
                </label>
                <select
                  value={form.vehicleType}
                  onChange={(e) =>
                    setForm({ ...form, vehicleType: e.target.value })
                  }
                  className="w-full border border-outline-variant rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary outline-none"
                >
                  {VEHICLE_TYPES.map((t) => (
                    <option key={t} value={t}>
                      {t}
                    </option>
                  ))}
                </select>
              </div>
              <div>
                <label className="block text-xs font-bold text-on-surface-variant uppercase mb-1">
                  Estado Operativo
                </label>
                <select
                  value={form.operationalStatus}
                  onChange={(e) =>
                    setForm({ ...form, operationalStatus: e.target.value })
                  }
                  className="w-full border border-outline-variant rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary outline-none"
                >
                  {STATUS_OPTIONS.map((s) => (
                    <option key={s} value={s}>
                      {s}
                    </option>
                  ))}
                </select>
              </div>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="block text-xs font-bold text-on-surface-variant uppercase mb-1">
                  Código de Radio
                </label>
                <input
                  type="text"
                  required
                  value={form.radioCode}
                  onChange={(e) =>
                    setForm({ ...form, radioCode: e.target.value })
                  }
                  className="w-full border border-outline-variant rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary outline-none"
                />
              </div>
              <div>
                <label className="block text-xs font-bold text-on-surface-variant uppercase mb-1">
                  Nivel de Agua (gal)
                </label>
                <input
                  type="number"
                  required
                  min="0"
                  value={form.waterLevelGallons}
                  onChange={(e) =>
                    setForm({ ...form, waterLevelGallons: e.target.value })
                  }
                  className="w-full border border-outline-variant rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary outline-none"
                />
              </div>
            </div>

            <div>
              <label className="block text-xs font-bold text-on-surface-variant uppercase mb-1">
                Última Fecha de Mantenimiento
              </label>
              <input
                type="date"
                required
                value={form.lastMaintenanceDate}
                onChange={(e) =>
                  setForm({ ...form, lastMaintenanceDate: e.target.value })
                }
                className="w-full border border-outline-variant rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary outline-none"
              />
            </div>

            <button
              type="submit"
              disabled={saving}
              className="mt-2 bg-primary text-on-primary py-2.5 rounded-lg font-bold hover:brightness-110 transition-all disabled:opacity-60"
            >
              {saving ? "Guardando..." : "Guardar"}
            </button>
          </form>
        </Modal>
      )}
    </div>
  );
}
