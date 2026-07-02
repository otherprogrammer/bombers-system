import { useEffect, useState } from "react";
import { Plus, Pencil, Trash2, Truck } from "lucide-react";
import PageHeader from "../components/PageHeader";
import Modal from "../components/Modal";
import { stationsService } from "../services/stations";

const emptyForm = { stationNumber: "", address: "", vehicleCapacity: "" };

export default function Stations() {
  const [stations, setStations] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState(null);
  const [form, setForm] = useState(emptyForm);
  const [saving, setSaving] = useState(false);
  const [vehicleCounts, setVehicleCounts] = useState({});

  async function loadStations() {
    setLoading(true);
    setError("");
    try {
      const data = await stationsService.getAll();
      setStations(data);
      // Fetch vehicle count per station for the capacity display
      const counts = {};
      await Promise.all(
        data.map(async (s) => {
          try {
            const vehicles = await stationsService.getVehicles(s.stationId);
            counts[s.stationId] = vehicles.length;
          } catch {
            counts[s.stationId] = null;
          }
        })
      );
      setVehicleCounts(counts);
    } catch (err) {
      setError(
        err.response?.data?.message || "No se pudieron cargar las estaciones."
      );
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadStations();
  }, []);

  function openCreate() {
    setEditing(null);
    setForm(emptyForm);
    setModalOpen(true);
  }

  function openEdit(station) {
    setEditing(station);
    setForm({
      stationNumber: station.stationNumber,
      address: station.address,
      vehicleCapacity: station.vehicleCapacity,
    });
    setModalOpen(true);
  }

  async function handleSubmit(e) {
    e.preventDefault();
    setSaving(true);
    setError("");
    const dto = {
      stationNumber: Number(form.stationNumber),
      address: form.address,
      vehicleCapacity: Number(form.vehicleCapacity),
    };
    try {
      if (editing) {
        await stationsService.update(editing.stationId, dto);
      } else {
        await stationsService.create(dto);
      }
      setModalOpen(false);
      await loadStations();
    } catch (err) {
      setError(
        err.response?.data?.message || "No se pudo guardar la estación."
      );
    } finally {
      setSaving(false);
    }
  }

  async function handleDelete(station) {
    if (
      !confirm(
        `¿Eliminar la estación N° ${station.stationNumber}? Esta acción no se puede deshacer.`
      )
    )
      return;
    try {
      await stationsService.remove(station.stationId);
      await loadStations();
    } catch (err) {
      setError(
        err.response?.data?.message || "No se pudo eliminar la estación."
      );
    }
  }

  return (
    <div className="p-8">
      <PageHeader
        title="Estaciones"
        subtitle="Gestión de estaciones de bomberos, dirección y capacidad operativa."
        action={
          <button
            onClick={openCreate}
            className="bg-primary text-on-primary px-5 py-2.5 rounded-lg font-bold flex items-center gap-2 hover:brightness-110 active:scale-95 transition-all shadow-sm"
          >
            <Plus size={18} />
            Nueva Estación
          </button>
        }
      />

      {error && (
        <div className="mb-4 bg-error-container text-on-error-container text-sm font-medium px-4 py-3 rounded border-l-4 border-error">
          {error}
        </div>
      )}

      {loading ? (
        <p className="text-secondary">Cargando estaciones...</p>
      ) : stations.length === 0 ? (
        <div className="bg-surface-container-lowest border border-outline-variant rounded-lg p-10 text-center text-secondary">
          Aún no hay estaciones registradas.
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-4">
          {stations.map((s) => {
            const count = vehicleCounts[s.stationId];
            const full = count != null && count >= s.vehicleCapacity;
            return (
              <div
                key={s.stationId}
                className="bg-surface-container-lowest border border-outline-variant rounded-lg p-5 shadow-sm relative overflow-hidden"
              >
                <div className="absolute left-0 top-0 bottom-0 w-1.5 bg-primary" />
                <div className="flex justify-between items-start pl-2">
                  <div>
                    <p className="text-[11px] font-bold text-primary tracking-widest uppercase">
                      Estación N° {s.stationNumber}
                    </p>
                    <h3 className="font-bold text-lg mt-1">{s.address}</h3>
                  </div>
                  {count != null && (
                    <span
                      className={`px-2 py-1 rounded text-[10px] font-black uppercase ${
                        full
                          ? "bg-primary-container text-on-primary-container"
                          : "bg-surface-container-high text-secondary"
                      }`}
                    >
                      {full ? "Llena" : "Disponible"}
                    </span>
                  )}
                </div>

                <div className="flex items-center gap-2 mt-4 pl-2 text-sm text-secondary">
                  <Truck size={16} />
                  <span className="font-mono font-bold">
                    {count ?? "—"} / {s.vehicleCapacity}
                  </span>
                  <span>vehículos</span>
                </div>

                <div className="flex gap-2 mt-4 pl-2">
                  <button
                    onClick={() => openEdit(s)}
                    className="flex items-center gap-1 text-xs font-bold text-secondary hover:text-primary border border-outline-variant rounded px-3 py-1.5 transition-colors"
                  >
                    <Pencil size={14} /> Editar
                  </button>
                  <button
                    onClick={() => handleDelete(s)}
                    className="flex items-center gap-1 text-xs font-bold text-secondary hover:text-error border border-outline-variant rounded px-3 py-1.5 transition-colors"
                  >
                    <Trash2 size={14} /> Eliminar
                  </button>
                </div>
              </div>
            );
          })}
        </div>
      )}

      {modalOpen && (
        <Modal
          title={editing ? "Editar Estación" : "Nueva Estación"}
          onClose={() => setModalOpen(false)}
        >
          <form onSubmit={handleSubmit} className="flex flex-col gap-4">
            <div>
              <label className="block text-xs font-bold text-on-surface-variant uppercase mb-1">
                Número de Estación
              </label>
              <input
                type="number"
                required
                value={form.stationNumber}
                onChange={(e) =>
                  setForm({ ...form, stationNumber: e.target.value })
                }
                className="w-full border border-outline-variant rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary outline-none"
              />
            </div>
            <div>
              <label className="block text-xs font-bold text-on-surface-variant uppercase mb-1">
                Dirección
              </label>
              <input
                type="text"
                required
                value={form.address}
                onChange={(e) => setForm({ ...form, address: e.target.value })}
                className="w-full border border-outline-variant rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary outline-none"
              />
            </div>
            <div>
              <label className="block text-xs font-bold text-on-surface-variant uppercase mb-1">
                Capacidad de Vehículos
              </label>
              <input
                type="number"
                required
                min="0"
                value={form.vehicleCapacity}
                onChange={(e) =>
                  setForm({ ...form, vehicleCapacity: e.target.value })
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
