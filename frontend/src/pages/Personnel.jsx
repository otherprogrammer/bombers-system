import { useEffect, useMemo, useState } from "react";
import { Plus, Pencil, Trash2, UserRound } from "lucide-react";
import PageHeader from "../components/PageHeader";
import Modal from "../components/Modal";
import Badge from "../components/Badge";
import { firefightersService } from "../services/firefighters";
import { stationsService } from "../services/stations";

const RANKS = ["Capitán", "Teniente", "Bombero"];
const STATUSES = ["Activo", "Descanso", "Baja"];

const statusColor = { Activo: "green", Descanso: "yellow", Baja: "gray" };

const emptyForm = {
  firefighterId: "",
  stationId: "",
  fullName: "",
  medicalCertification: "",
  hireDate: "",
  rank: RANKS[2],
  currentStatus: STATUSES[0],
};

export default function Personnel() {
  const [firefighters, setFirefighters] = useState([]);
  const [stations, setStations] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [stationFilter, setStationFilter] = useState("");
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState(null);
  const [form, setForm] = useState(emptyForm);
  const [saving, setSaving] = useState(false);

  async function loadAll() {
    setLoading(true);
    setError("");
    try {
      const [ff, st] = await Promise.all([
        stationFilter
          ? firefightersService.getByStation(stationFilter)
          : firefightersService.getAll(),
        stationsService.getAll(),
      ]);
      setFirefighters(ff);
      setStations(st);
    } catch (err) {
      setError(
        err.response?.data?.message || "No se pudo cargar el personal."
      );
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadAll();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [stationFilter]);

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

  function openEdit(f) {
    setEditing(f);
    setForm({
      firefighterId: f.firefighterId,
      stationId: f.stationId,
      fullName: f.fullName,
      medicalCertification: f.medicalCertification,
      hireDate: f.hireDate,
      rank: f.rank,
      currentStatus: f.currentStatus,
    });
    setModalOpen(true);
  }

  async function handleSubmit(e) {
    e.preventDefault();
    setSaving(true);
    setError("");
    try {
      if (editing) {
        await firefightersService.update(editing.firefighterId, {
          stationId: Number(form.stationId),
          fullName: form.fullName,
          medicalCertification: form.medicalCertification,
          rank: form.rank,
          currentStatus: form.currentStatus,
        });
      } else {
        await firefightersService.create({
          firefighterId: Number(form.firefighterId),
          stationId: Number(form.stationId),
          fullName: form.fullName,
          medicalCertification: form.medicalCertification,
          hireDate: form.hireDate,
          rank: form.rank,
          currentStatus: form.currentStatus,
        });
      }
      setModalOpen(false);
      await loadAll();
    } catch (err) {
      setError(
        err.response?.data?.message || "No se pudo guardar el registro."
      );
    } finally {
      setSaving(false);
    }
  }

  async function handleDelete(f) {
    if (!confirm(`¿Eliminar a ${f.fullName}?`)) return;
    try {
      await firefightersService.remove(f.firefighterId);
      await loadAll();
    } catch (err) {
      setError(
        err.response?.data?.message || "No se pudo eliminar el registro."
      );
    }
  }

  return (
    <div className="p-8">
      <PageHeader
        title="Personal (Bomberos)"
        subtitle="Directorio de personal, rango, certificación y estado."
        action={
          <button
            onClick={openCreate}
            disabled={stations.length === 0}
            className="bg-primary text-on-primary px-5 py-2.5 rounded-lg font-bold flex items-center gap-2 hover:brightness-110 active:scale-95 transition-all shadow-sm disabled:opacity-50"
          >
            <Plus size={18} />
            Nuevo Bombero
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
          Estación:
        </label>
        <select
          value={stationFilter}
          onChange={(e) => setStationFilter(e.target.value)}
          className="bg-white border border-outline-variant rounded px-3 py-1.5 text-sm"
        >
          <option value="">Todas</option>
          {stations.map((s) => (
            <option key={s.stationId} value={s.stationId}>
              Estación {s.stationNumber}
            </option>
          ))}
        </select>
        <span className="ml-auto text-xs text-secondary italic">
          {firefighters.length} registro(s)
        </span>
      </div>

      <div className="bg-surface-container-lowest border border-outline-variant rounded-lg overflow-hidden shadow-sm">
        <table className="w-full text-left border-collapse">
          <thead>
            <tr className="bg-surface-container text-on-surface-variant text-xs uppercase tracking-widest border-b border-outline-variant">
              <th className="px-4 py-3 font-black">Nombre</th>
              <th className="px-4 py-3 font-black">Rango</th>
              <th className="px-4 py-3 font-black">Estación</th>
              <th className="px-4 py-3 font-black">Certificación</th>
              <th className="px-4 py-3 font-black">Estado</th>
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
            ) : firefighters.length === 0 ? (
              <tr>
                <td colSpan={6} className="px-4 py-6 text-center text-secondary">
                  No hay personal registrado.
                </td>
              </tr>
            ) : (
              firefighters.map((f) => (
                <tr key={f.firefighterId} className="hover:bg-surface-container-low transition-colors">
                  <td className="px-4 py-3 font-bold flex items-center gap-2">
                    <UserRound size={16} className="text-primary" />
                    {f.fullName}
                  </td>
                  <td className="px-4 py-3">{f.rank}</td>
                  <td className="px-4 py-3">
                    {stationName[f.stationId] ?? `#${f.stationId}`}
                  </td>
                  <td className="px-4 py-3 text-sm">
                    {f.medicalCertification}
                  </td>
                  <td className="px-4 py-3">
                    <Badge color={statusColor[f.currentStatus] || "gray"}>
                      {f.currentStatus}
                    </Badge>
                  </td>
                  <td className="px-4 py-3 text-right">
                    <div className="flex justify-end gap-2">
                      <button
                        onClick={() => openEdit(f)}
                        className="p-1.5 text-secondary hover:text-primary transition-colors"
                        title="Editar"
                      >
                        <Pencil size={16} />
                      </button>
                      <button
                        onClick={() => handleDelete(f)}
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
          title={editing ? "Editar Bombero" : "Nuevo Bombero"}
          onClose={() => setModalOpen(false)}
        >
          <form onSubmit={handleSubmit} className="flex flex-col gap-4">
            {!editing && (
              <div>
                <label className="block text-xs font-bold text-on-surface-variant uppercase mb-1">
                  ID de Bombero
                </label>
                <input
                  type="number"
                  required
                  value={form.firefighterId}
                  onChange={(e) =>
                    setForm({ ...form, firefighterId: e.target.value })
                  }
                  className="w-full border border-outline-variant rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary outline-none"
                />
              </div>
            )}
            <div>
              <label className="block text-xs font-bold text-on-surface-variant uppercase mb-1">
                Nombre Completo
              </label>
              <input
                type="text"
                required
                value={form.fullName}
                onChange={(e) => setForm({ ...form, fullName: e.target.value })}
                className="w-full border border-outline-variant rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary outline-none"
              />
            </div>
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
                  Rango
                </label>
                <select
                  value={form.rank}
                  onChange={(e) => setForm({ ...form, rank: e.target.value })}
                  className="w-full border border-outline-variant rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary outline-none"
                >
                  {RANKS.map((r) => (
                    <option key={r} value={r}>
                      {r}
                    </option>
                  ))}
                </select>
              </div>
              <div>
                <label className="block text-xs font-bold text-on-surface-variant uppercase mb-1">
                  Estado
                </label>
                <select
                  value={form.currentStatus}
                  onChange={(e) =>
                    setForm({ ...form, currentStatus: e.target.value })
                  }
                  className="w-full border border-outline-variant rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary outline-none"
                >
                  {STATUSES.map((s) => (
                    <option key={s} value={s}>
                      {s}
                    </option>
                  ))}
                </select>
              </div>
            </div>
            <div>
              <label className="block text-xs font-bold text-on-surface-variant uppercase mb-1">
                Certificación Médica
              </label>
              <input
                type="text"
                required
                value={form.medicalCertification}
                onChange={(e) =>
                  setForm({ ...form, medicalCertification: e.target.value })
                }
                className="w-full border border-outline-variant rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary outline-none"
              />
            </div>
            {!editing && (
              <div>
                <label className="block text-xs font-bold text-on-surface-variant uppercase mb-1">
                  Fecha de Contratación
                </label>
                <input
                  type="date"
                  required
                  value={form.hireDate}
                  onChange={(e) =>
                    setForm({ ...form, hireDate: e.target.value })
                  }
                  className="w-full border border-outline-variant rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary outline-none"
                />
              </div>
            )}
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
